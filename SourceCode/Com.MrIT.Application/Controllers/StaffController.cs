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
                a = Md5.Decrypt(System.Web.HttpUtility.UrlDecode(a));
                int id = 0;
                int.TryParse(a, out id);
                var result = _svsStaff.GetStaff(id);
                if(result == null)
                {
                    return RedirectToAction("Listing", "Staff");
                }
                return View(result);
            }catch(Exception ex)
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
                a = Md5.Decrypt(System.Web.HttpUtility.UrlDecode(a));
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
    }
}
