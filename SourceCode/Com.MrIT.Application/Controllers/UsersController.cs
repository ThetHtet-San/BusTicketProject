using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using Com.MrIT.App.Filters;
using Com.MrIT.Common;
using Com.MrIT.Services;
using Com.MrIT.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Com.MrIT.PublicSite.Controllers
{

    public class UsersController : Controller
    {
        private readonly IEmailService _svsEmail;
        private readonly IMrUserService _svsUser;
        private readonly IHttpContextAccessor _accessor;

        public UsersController(IMrUserService svsUser, IEmailService svsEmail, IHttpContextAccessor accessor)
        {
            this._svsUser = svsUser;
            this._svsEmail = svsEmail;
            this._accessor = accessor;
        }

        #region Authentication
        [MrITActionFilter]
        [HttpGet]
        [ActionName("SignIn")]
        public IActionResult SignIn()
        {
            var currentUrl = HttpContext.Request.Query["currenturl"].ToString();
            

            ViewBag.CurrentUrl = "";
            if (!string.IsNullOrEmpty(currentUrl))
            {
                ViewBag.CurrentUrl = currentUrl;
                TempData["CurrentUrl"] = currentUrl;
            }

            return View();
        }

        [HttpPost]
        [ActionName("SignIn")]
        public IActionResult SignIn(VmMrUser user)
        {
            var currenturl = Request.Form["currenturl"].ToString();
            user.Browser = Request.Headers["User-Agent"].ToString();
            user.IPAddress = _accessor.HttpContext.Connection.RemoteIpAddress.ToString();
            user.UserType = "C";            
            var result = _svsUser.ValidateUser(user);
            // User Validating
            if (result.FullName == null)
            {
                TempData["ErrorMessage"] = "Email or Password is Invalid";
                return View(user);
            }

            if (!result.Active)
            {
                TempData["ErrorMessage"] = "Your acount is disabled.";
                return View(user);
            }

            if (result.IsLocked)
            {
                TempData["ErrorMessage"] = "Your acount is locked.";
                return View(user);
            }

            if (!result.IsActivated)
            {
                TempData["ErrorMessage"] = "Your acount is not activated.";
                return View(user);
            }

            if (result.UserType != "C")
            {
                TempData["ErrorMessage"] = "Email or Password is Invalid";
                return View(user);
            }
            // Set Session
            HttpContext.Session.SetString("UserID", result.ID.ToString());
            HttpContext.Session.SetString("Email", result.Email.ToString());
            HttpContext.Session.SetString("FullName", result.FullName.ToString());
            HttpContext.Session.SetString("UserType", result.UserType.ToString());
            HttpContext.Session.SetString("UserProfile", result.strProfileImage.ToString());

            if (string.IsNullOrEmpty(currenturl))
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return Redirect(currenturl.Replace("_AND_", "&"));
            }
        }

        public ActionResult SignOut()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("SignIn", "Users");
        }

        [HttpGet]
        [ActionName("Register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Register")]
        public IActionResult Register(VmMrUser user)
        {          
            user.Email = user.Email.Trim();
            var result = _svsUser.AddMrUser(user);
            if (result.IsSuccess)
            {
                var emailcontent = new VmRegistrationEmail();
                emailcontent.Email = user.Email;
                emailcontent.Name = user.FullName;
                emailcontent.URL = Url.Action("Signin", "Users");
                _svsEmail.SendForRegistration(emailcontent, result.MessageToUser);
                return RedirectToAction("SuccessfulRegistration", "Users");
            }
            else
            {
                ViewBag.ErrorMessage = result.MessageToUser;
                return View(user);
            }       
        }

        [HttpGet]
        [ActionName("SuccessfulRegistration")]
        public IActionResult SuccessfulRegistration()
        {
            return View();
        }

        [HttpGet]
        public JsonResult CheckEmailForRegister(string Email)
        {
            var result =  _svsUser.CheckEmailForRegister(Email.EmptyIfNull(),"C");

            return Json(result);
        }

        [HttpGet]
        public JsonResult CheckIsEmailExist(string Email, string FullName)
        {           
            var result = _svsUser.CheckEmailForRegister(Email.EmptyIfNull(), "C", FullName);

            return Json(result);
        }      

        public ActionResult FacebookLogin()
        {
            var url = "https://www.facebook.com/v7.0/dialog/oauth/?client_id=" + Constants.FacebookLoginAppID + "&redirect_uri=https://localhost:5001/Users/AfterFacebookLogin&response_type=code&state=1"; 
            return Redirect(url);
        }

        public ActionResult AfterFacebookLogin()
        {
            try
            {
                var currenturl = "";
                if (TempData["CurrentUrl"] != null)
                {
                    currenturl = TempData["CurrentUrl"].ToString();
                }
                // Get the Facebook code from the querystring
                if (!string.IsNullOrEmpty(HttpContext.Request.Query["code"] ))
                {
                    var code = HttpContext.Request.Query["code"].ToString();
                    if (code == "")
                    {
                       
                        return RedirectToAction("SignIn", "Users", new { currenturl = currenturl });
                    }
                    var userInfo = GetUserInfoFromFacebook(code);

                    var checkResult =  _svsUser.CheckEmailForRegister(userInfo.Email, "C");
                    //check userWithEmail
                    if (!checkResult.IsSuccess)
                    {
                        //user exist
                        var user = new VmMrUser();
                        user.Email = userInfo.Email;
                        user.Password = "nnhhyy66"; //special
                        user.Browser = Request.Headers["User-Agent"].ToString();
                        user.IPAddress = _accessor.HttpContext.Connection.RemoteIpAddress.ToString();
                        user.UserType = "C";
                        var result =  _svsUser.ValidateUser(user);
                        // User Validating
                        if (result.FullName == null)
                        {
                            TempData["ErrorMessage"] = "Email or Password is Invalid";
                            return View(user);
                        }

                        if (!result.Active)
                        {
                            TempData["ErrorMessage"] = "Your acount is disabled.";
                            return View(user);
                        }

                        if (!result.IsLocked)
                        {
                            TempData["ErrorMessage"] = "Your acount is locked.";
                            return View(user);
                        }

                        if (!result.IsLocked)
                        {
                            TempData["ErrorMessage"] = "Your acount is not activated.";
                            return View(user);
                        }

                        if (result.UserType != "C")
                        {
                            TempData["ErrorMessage"] = "Email or Password is Invalid";
                            return View(user);
                        }
                        //Set Session
                        HttpContext.Session.SetString("UserID", result.ID.ToString());
                        HttpContext.Session.SetString("Email", result.Email.ToString());
                        HttpContext.Session.SetString("FullName", result.FullName.ToString());
                        HttpContext.Session.SetString("UserType", result.UserType.ToString());
                        HttpContext.Session.SetString("UserProfile", result.strProfileImage.ToString());
                    }
                    else
                    {
                        //register
                        var user = new VmMrUser();
                        user.Email = userInfo.Email;
                        user.Password = userInfo.Password;
                        user.FullName = userInfo.FullName;
                        var result = _svsUser.AddMrUser(user);
                        if (!result.IsSuccess)
                        {
                            TempData["ErrorMessage"] = result.MessageToUser;
                            return RedirectToAction("SignIn", "Users", new { currenturl = currenturl });
                        }
                        
                        user.Browser = Request.Headers["User-Agent"].ToString();
                        user.IPAddress = _accessor.HttpContext.Connection.RemoteIpAddress.ToString();
                        user.UserType = "S";
                        var resultLogin = _svsUser.ValidateUser(user);
                        HttpContext.Session.SetString("UserID", resultLogin.ID.ToString());
                        HttpContext.Session.SetString("Email", resultLogin.Email.ToString());
                        HttpContext.Session.SetString("FullName", resultLogin.FullName.ToString());
                        HttpContext.Session.SetString("UserType", resultLogin.UserType.ToString());
                        HttpContext.Session.SetString("UserProfile", resultLogin.strProfileImage.ToString());
                    }
                    
                    if(currenturl == "")
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        return Redirect(currenturl);
                    }                   
                }
                else
                {
                    return RedirectToAction("SignIn", "Users", new { currenturl = currenturl });
                }
            }
            catch(Exception ex)
            {
                string currenturl = Request.Scheme + "://" + Request.Host.Value + Request.Path.Value + Request.QueryString.Value;
                string user = "normal user";
                var userName = HttpContext.Session.GetString("UserName");
                if (!string.IsNullOrEmpty(userName))
                {
                    user = userName;
                }
                _svsEmail.InsertErrorAndEmail(ex, user, currenturl);

                return RedirectToAction("SystemIssue", "Errors", new { message = System.Web.HttpUtility.UrlEncode(ex.Message) });
            }                   
        }

        private VmMrUser GetUserInfoFromFacebook(string code)
        {
            // Exchange the code for an access token
            Uri targetUri = new Uri("https://graph.facebook.com/oauth/access_token?client_id=" + Constants.FacebookLoginAppID + "&client_secret=" + Constants.FacebookLoginSecret + "&redirect_uri=https://localhost:5001/Users/AfterFacebookLogin&code=" + code);
            HttpWebRequest at = (HttpWebRequest)HttpWebRequest.Create(targetUri);

            System.IO.StreamReader str = new System.IO.StreamReader(at.GetResponse().GetResponseStream());
            dynamic jsonToken = JObject.Parse(str.ReadToEnd().ToString());

            string accessToken = jsonToken.access_token;


            // Exchange the code for an extended access token
            Uri eatTargetUri = new Uri("https://graph.facebook.com/oauth/access_token?grant_type=fb_exchange_token&client_id=" + Constants.FacebookLoginAppID + "&client_secret=" + Constants.FacebookLoginSecret + "&fb_exchange_token=" + accessToken);
            HttpWebRequest eat = (HttpWebRequest)HttpWebRequest.Create(eatTargetUri);

            StreamReader eatStr = new StreamReader(eat.GetResponse().GetResponseStream());
            // Deserialize and convert the JSON object to the Facebook.User object type
            dynamic jsoneatStr = JObject.Parse(eatStr.ReadToEnd().ToString());
            string extendedAccessToken = jsoneatStr.access_token;



            // Request the Facebook user information
            Uri targetUserUri = new Uri("https://graph.facebook.com/v7.0/me?fields=name,email&access_token=" + extendedAccessToken);
            HttpWebRequest user = (HttpWebRequest)HttpWebRequest.Create(targetUserUri);

            // Read the returned JSON object response
            StreamReader userInfo = new StreamReader(user.GetResponse().GetResponseStream());
            // Deserialize and convert the JSON object to the Facebook.User object type
            dynamic jsonUserInfo = JObject.Parse(userInfo.ReadToEnd().ToString());

            var result = new VmMrUser();
            result.FullName = jsonUserInfo.name;
            result.Email = jsonUserInfo.email;
            result.Password = jsonUserInfo.id;
            return result;
        }
        #endregion

        #region Account Setting

        [MrITActionFilter]
        public IActionResult Profile()
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("UserID"));
                var result = _svsUser.GetMrUserByID(userID);

                return View(result);
            }
            catch (Exception ex)
            {
                //string currenturl = Request.Scheme + "://" + Request.Host.Value + Request.Path.Value + Request.QueryString.Value;
                //string user = "admin";
                //var userName = HttpContext.Session.GetString("UserName");
                //if (!string.IsNullOrEmpty(userName))
                //{
                //    user = userName;
                //}
                //_svsEmail.InsertErrorAndEmail(ex, user, currenturl);

                return RedirectToAction("SystemIssue", "Errors", new { message = System.Web.HttpUtility.UrlEncode(ex.Message) });
            }
        }

        [MrITActionFilter]
        public IActionResult ProfileEdit()
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Session.GetString("UserID"));
                var result = _svsUser.GetMrUserByID(userID);

                return View(result);
            }
            catch (Exception ex)
            {
                //string currenturl = Request.Scheme + "://" + Request.Host.Value + Request.Path.Value + Request.QueryString.Value;
                //string user = "admin";
                //var userName = HttpContext.Session.GetString("UserName");
                //if (!string.IsNullOrEmpty(userName))
                //{
                //    user = userName;
                //}
                // _svsEmail.InsertErrorAndEmail(ex, user, currenturl);

                return RedirectToAction("SystemIssue", "Errors", new { message = System.Web.HttpUtility.UrlEncode(ex.Message) });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ProfileEdit(VmMrUser vmMruser)
        {
            try
            {
                vmMruser.ModifiedBy = vmMruser.FullName;

                for (int i = 0; i < Request.Form.Files.Count; i++)
                {
                    if (Request.Form.Files[i].Length > 0)
                    {
                        if (Request.Form.Files[i].Name.ToLower() == "file_ProfileImage")
                        {
                            vmMruser.ProfileImage = ConvertFiletoBytes(Request.Form.Files[i]);
                        }
                    }
                }

                if (vmMruser.ProfileImage != null)
                {
                    HttpContext.Session.SetString("UserProfile", Convert.ToBase64String(vmMruser.ProfileImage).ToString());
                }
                vmMruser.ID = Convert.ToInt32(Md5.Decrypt(vmMruser.EncryptId));
                vmMruser.Active = vmMruser.SystemActive = true;

                var result = _svsUser.UpdateMrUser(vmMruser);
                TempData["MessageToUser"] = result.MessageToUser;
                TempData["Pass"] = vmMruser.Password;
                if (!result.IsSuccess)
                {
                    return RedirectToAction("SystemIssue", "Errors", new { message = HttpUtility.UrlEncode(result.MessageToUser) });
                }


                return RedirectToAction("Profile", "Users");
            }
            catch (Exception ex)
            {
                //string currenturl = Request.Scheme + "://" + Request.Host.Value + Request.Path.Value + Request.QueryString.Value;
                //string user = "admin";
                //var userName = HttpContext.Session.GetString("UserName");
                //if (!string.IsNullOrEmpty(userName))
                //{
                //    user = userName;
                //}
                //_svsEmail.InsertErrorAndEmail(ex, user, currenturl);

                return RedirectToAction("SystemIssue", "Errors", new { message = System.Web.HttpUtility.UrlEncode(ex.Message) });
            }
        }

        public JsonResult DisableAccount(string Password)
        {
            int userID = Convert.ToInt32(HttpContext.Session.GetString("UserID"));
            var result = _svsUser.DisableAccount(userID, Password, "C");

            return Json(result);
        }

        public JsonResult ChangePassword(string CurrentPassword, string NewPassword)
        {
            int userID = Convert.ToInt32(HttpContext.Session.GetString("UserID"));
            var result = _svsUser.ChangePassword(userID, CurrentPassword, NewPassword, "C");

            return Json(result);
        }
        #endregion

        private byte[] ConvertFiletoBytes(IFormFile file)
        {
            byte[] p1 = null;
            if (file != null)
            {
                if (file.Length > 0)
                //Convert Image to byte and save to database
                {
                    using (var fs1 = file.OpenReadStream())
                    using (var ms1 = new System.IO.MemoryStream())
                    {
                        fs1.CopyTo(ms1);
                        p1 = ms1.ToArray();
                    }
                }
            }
            return p1;
        }
    }
}