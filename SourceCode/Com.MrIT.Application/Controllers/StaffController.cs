using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
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
            int recordsPerPage = 2;
            string def = "test";
            string abc = "123";
            string queryPage = HttpContext.Request.Query["page"];
            int page = 1;
            if (queryPage != null)
            {
                int.TryParse(queryPage, out page);
                if(page == 0)
                {
                    page = 1;
                }
            }

            string queryKeyword = HttpContext.Request.Query["keyword"];
            string keyword = "";
            if (queryKeyword != null)
            {
                keyword = queryKeyword;
            }
            TempData["Keyword"] = keyword;

            var result = _svsStaff.GetStaffListByPage(keyword, page, recordsPerPage);

            var resultPage = new VmStaffPage();
            resultPage.Result = result;
            //var result = _svsStaff.GetAllStaff();
            return View(resultPage);
        }

        [HttpGet]
        public IActionResult ExportToExcel()
        {
            var staffList = _svsStaff.GetAllStaff();
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Staff");
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "Code";
                worksheet.Cell(currentRow, 2).Value = "Name";
                worksheet.Cell(currentRow, 3).Value = "Address";
                foreach (var staff in staffList)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = staff.Code;
                    worksheet.Cell(currentRow, 2).Value = staff.Name;
                    worksheet.Cell(currentRow, 3).Value = staff.Address;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    string fileName = "staff" + DateTime.Now.ToString("yyyyMMddHHmmss") +".xlsx";
                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        fileName);
                }
            }
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
            staff.Code = ShortId.Generate(true, false, 7);
            staff.Name = staff.Name.EmptyIfNull();
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
            if(staff.EducationList == null)
            {
                staff.EducationList = new List<VmStaffEducation>();
            }
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
        public IActionResult Delete(string a)
        {
            try
            {
                a = Md5.Decrypt(System.Net.WebUtility.UrlDecode(a));
                int id = 0;
                int.TryParse(a, out id);
                var result = _svsStaff.DeleteStaff(id);
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
