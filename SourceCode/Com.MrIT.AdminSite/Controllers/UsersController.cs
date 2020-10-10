using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Com.MrIT.Services;
using Com.MrIT.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Com.MrIT.Common;
using Microsoft.AspNetCore.Mvc.Rendering;
using Com.MrIT.AdminSite.Filters;

namespace Com.MrIT.AdminSite.Controllers
{
    [MrITActionFilter]
    public class UsersController : Controller
    {
        private readonly IMrUserService _svsMrUser;
        private readonly IHttpContextAccessor _accessor;

        public UsersController(IMrUserService svsMrUser, IHttpContextAccessor accessor)
        {
            this._svsMrUser = svsMrUser;
            this._accessor = accessor;
        }

        #region Authentication
        [HttpGet]
        [ActionName("SignIn")]
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        [ActionName("SignIn")]
        public ActionResult SignIn(VmMrUser user)
        {
            // Get User-Agent & IP
            user.Browser = Request.Headers["User-Agent"].ToString();
            user.IPAddress = _accessor.HttpContext.Connection.RemoteIpAddress.ToString();

            user.UserType = "S";
            var result = _svsMrUser.ValidateUser(user);

            // Validations
            if (result.IsLocked) // If account is locked
            {
                TempData["ErrorMessage"] = "Your acount is locked.";
                return View(user);
            }

            if (!result.IsActivated) // If account is not activated
            {
                TempData["ErrorMessage"] = "Your acount is not activated.";
                return View(user);
            }

            if (result.UserType != "S") // If user type is not Staff
            {
                TempData["ErrorMessage"] = "Email or Password is Invalid";
                return View(user);
            }

            // Set Sessions
            HttpContext.Session.SetString("Email", result.Email.ToString());
            HttpContext.Session.SetString("FullName", result.FullName.ToString());
            HttpContext.Session.SetString("UserType", result.UserType.ToString());
            HttpContext.Session.SetString("UserRole", result.UserRole.ToString());
            HttpContext.Session.SetString("UserID", result.ID.ToString());
            HttpContext.Session.SetString("UserProfile", result.strProfileImage.ToString());

            return RedirectToAction("Index", "Home");
        }

        public ActionResult SignOut()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("SignIn", "Users");
        }
        #endregion

        #region Manage Staff User

        // Listing

        public IActionResult StaffListing()
        {
            string queryPage = HttpContext.Request.Query["page"];
            int page = 1;
            if (queryPage != null)
            {
                page = Convert.ToInt32(queryPage);
            }

            string keyword = HttpContext.Request.Query["keyword"];
            if (keyword == null)
            {
                keyword = "";
            }
            TempData["Keyword"] = keyword;
            var result = _svsMrUser.GetMrUserByPage(keyword.ToLower(), page, Constants.app_totalRecords);

            return View(result);
        }

        // Create // Admin, Manager, Staff
        public ActionResult StaffCreate()
        {
            var listUserRole = new List<SelectListItem> {
                                        new SelectListItem { Text="Admin", Value="Admin" },
                                        new SelectListItem { Text="Manager", Value="Manager" },
                                        new SelectListItem { Text="Staff", Value="Staff" },
                                    };
            ViewBag.UserRoleList = new SelectList(listUserRole, "Value", "Text");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult StaffCreate(VmMrUser vmMrUser)
        {
            try
            {
                var userName = HttpContext.Session.GetString("FullName");
                vmMrUser.CreatedBy = vmMrUser.ModifiedBy = userName;
                vmMrUser.IsActivated = true;
                vmMrUser.Password = Md5.Encrypt(vmMrUser.Password);
                vmMrUser.UserType = "S";
                for (int i = 0; i < Request.Form.Files.Count; i++)
                {
                    if (Request.Form.Files[i].Length > 0)
                    {
                        if (Request.Form.Files[i].Name.ToLower() == "file_profile")
                        {
                            vmMrUser.ProfileImage = ConvertFiletoBytes(Request.Form.Files[i]);
                        }
                    }
                }
                var result = _svsMrUser.AddMrUser(vmMrUser); // Add to Database
                TempData["MessageToUser"] = result.MessageToUser;
                if (!result.IsSuccess)
                {
                    return RedirectToAction("SystemIssues", "Errors", new { message = HttpUtility.UrlEncode(result.MessageToUser) });
                }

                return RedirectToAction(nameof(StaffListing));

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
        // Update
        public ActionResult StaffEdit(string a)
        {
            try
            {
                int id = 0;
                if (a != "")
                {
                    id = Convert.ToInt32(Md5.Decrypt(System.Web.HttpUtility.UrlDecode(a)));
                }
                var listUserRole = new List<SelectListItem> {
                                        new SelectListItem { Text="Admin", Value="Admin" },
                                        new SelectListItem { Text="Manager", Value="Manager" },
                                        new SelectListItem { Text="Staff", Value="Staff" },
                                    };
                
                var result = _svsMrUser.GetMrUserByID(id); // Get User Info By ID
                result.Password = Md5.Decrypt(result.Password);

                ViewBag.UserRoleList = new SelectList(listUserRole, "Value", "Text",result.UserRole);
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult StaffEdit(VmMrUser vmMrUser)
        {
            try
            {
                var userName = HttpContext.Session.GetString("FullName");
                vmMrUser.ModifiedBy = userName;
                vmMrUser.Password = Md5.Encrypt(vmMrUser.Password);
                vmMrUser.ID = Convert.ToInt32(Md5.Decrypt(vmMrUser.EncryptId));
                for (int i = 0; i < Request.Form.Files.Count; i++)
                {
                    if (Request.Form.Files[i].Length > 0)
                    {
                        if (Request.Form.Files[i].Name.ToLower() == "file_profile")
                        {
                            vmMrUser.ProfileImage = ConvertFiletoBytes(Request.Form.Files[i]);
                        }
                    }
                }
                var result = _svsMrUser.UpdateMrUser(vmMrUser); // Update Database
                TempData["MessageToUser"] = result.MessageToUser;
                if (!result.IsSuccess)
                {
                    return RedirectToAction("SystemIssues", "Errors", new { message = HttpUtility.UrlEncode(result.MessageToUser) });
                }
                return RedirectToAction(nameof(StaffListing));

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