using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    }
}
