using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Com.MrIT.Common;
using Com.MrIT.Services;
using Com.MrIT.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Com.MrIT.PublicSite.Controllers
{
    public class StaffController : Controller
    {
        private readonly IStaffService _svsStaff;
        public StaffController(IStaffService svsStaff)
        {
            this._svsStaff = svsStaff;
        }

        [HttpGet]
        public IActionResult Listing()
        {
            var result = _svsStaff.GetAllStaff();
            return View(result);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(VmStaff staff)
        {
            staff.CreatedBy = staff.ModifiedBy = "System"; //Session["LogOnUser"]
            var result = _svsStaff.CreateStaff(staff);
            if (result.IsSuccess)
            {
                return RedirectToAction("Listing", "Staff");
            }
            else
            {
                return View(staff);
            }

        }

        [HttpGet]
        public IActionResult Edit(string a)
        {
            try
            {
                a = Md5.Decrypt(System.Net.WebUtility.UrlDecode(a));
                int id = 0;
                int.TryParse(a, out id);
                var result = _svsStaff.GetStaff(id);
                if (result == null)
                {
                    return RedirectToAction("Listing", "Staff");
                }
                return View(result);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Listing", "Staff");
            }
        }

        [HttpPost]
        public IActionResult Edit(VmStaff staff)
        {
            staff.ModifiedBy = "System"; //Session["LogOnUser"]
            var result = _svsStaff.UpdateStaff(staff);
            if (result.IsSuccess)
            {
                return RedirectToAction("Listing", "Staff");
            }
            else
            {
                return View(staff);
            }

        }

        [HttpGet]
        public IActionResult Detail(string a)
        {
            try
            {
                a = Md5.Decrypt(System.Net.WebUtility.UrlDecode(a));
                int id = 0;
                int.TryParse(a, out id);
                var result = _svsStaff.GetStaff(id);
                if (result == null)
                {
                    return RedirectToAction("Listing", "Staff");
                }
                return View(result);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Listing", "Staff");
            }
        }

        [HttpGet]
        public IActionResult CreateEducation(string a)
        {
            try
            {
                a = Md5.Decrypt(System.Net.WebUtility.UrlDecode(a));
                int id = 0;
                int.TryParse(a, out id);
                var checkStaff = _svsStaff.GetStaff(id);
                if (checkStaff == null)
                {
                    return RedirectToAction("Listing", "Staff");
                }
                var staffEducation = new VmStaffEducation();
                staffEducation.StaffID = id;
                return View(staffEducation);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Listing", "Staff");
            }

        }

        [HttpPost]
        public IActionResult CreateEducation(VmStaffEducation staffEducation)
        {
            staffEducation.CreatedBy = staffEducation.ModifiedBy = "System"; //Session["LogOnUser"]
            var result = _svsStaff.CreateStaffEducation(staffEducation);
            if (result.IsSuccess)
            {
                return RedirectToAction("Listing", "Staff");
            }
            else
            {
                return RedirectToAction("Detail", "Staff", new { a = System.Net.WebUtility.UrlEncode(Md5.Encrypt(staffEducation.StaffID.ToString())) });
            }

        }


        [HttpGet]
        public IActionResult EditEducation(string a)
        {
            try
            {
                a = Md5.Decrypt(System.Net.WebUtility.UrlDecode(a));
                int id = 0;
                int.TryParse(a, out id);
                var staffEducation = _svsStaff.GetStaffEducation(id);
                if (staffEducation == null)
                {
                    return RedirectToAction("Listing", "Staff");
                }
               
                return View(staffEducation);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Listing", "Staff");
            }

        }

        [HttpPost]
        public IActionResult EditEducation(VmStaffEducation staffEducation)
        {
            staffEducation.ModifiedBy = "System"; //Session["LogOnUser"]
            var result = _svsStaff.EditStaffEducation(staffEducation);
            if (result.IsSuccess)
            {
                return RedirectToAction("Listing", "Staff");
            }
            else
            {
                return RedirectToAction("Detail", "Staff", new { a = System.Net.WebUtility.UrlEncode(Md5.Encrypt(staffEducation.StaffID.ToString())) });
            }

        }


        public IActionResult CreateExperience(string a)
        {
            try
            {
                a = Md5.Decrypt(System.Net.WebUtility.UrlDecode(a));
                int id = 0;
                int.TryParse(a, out id);
                var checkStaff = _svsStaff.GetStaff(id);
                if (checkStaff == null)
                {
                    return RedirectToAction("Listing", "Staff");
                }

                var staffExperience = new VmStaffExperience();
                staffExperience.StaffID = id;

                return View(staffExperience);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Listing", "Staff");
            }
        }

        [HttpPost]
        public IActionResult CreateExperience(VmStaffExperience staffExperience)
        {
            staffExperience.CreatedBy = staffExperience.ModifiedBy = "System"; //Session["LogOnUser"]
            var result = _svsStaff.CreateStaffExperience(staffExperience);

            return RedirectToAction("Detail", "Staff", new { a = System.Net.WebUtility.UrlEncode(Md5.Encrypt(staffExperience.StaffID.ToString())) });

        }
    }
}
