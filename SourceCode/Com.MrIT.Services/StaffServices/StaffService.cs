using Com.MrIT.Common;
using Com.MrIT.Common.Configuration;
using Com.MrIT.DataRepository;
using Com.MrIT.DBEntities;
using Com.MrIT.ViewModels;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Com.MrIT.Services
{
    public class StaffService : BaseService, IStaffService
    {
        private readonly IStaffRepository _repoStaff;
        private readonly IStaffEducationRepository _repoStaffEducation;
        private readonly IStaffExperienceRepository _repoStaffExperience;
        private readonly AppSettings _appSettings;
        public StaffService(IStaffRepository repoStaff, IStaffEducationRepository repoStaffEducation, IStaffExperienceRepository repoStaffExperience, IOptions<AppSettings> appSettings)
        {
            this._repoStaff = repoStaff;
            this._repoStaffExperience = repoStaffExperience;
            this._repoStaffEducation = repoStaffEducation;
            this._appSettings = appSettings.Value;
        }

        #region Staff
        public VmGenericServiceResult CreateStaff(VmStaff staff)
        {
            //return format
            var result = new VmGenericServiceResult();
            try
            {
                var dbStaff = new Staff();
                Copy<VmStaff, Staff>(staff, dbStaff);
                var dbResult = _repoStaff.Add(dbStaff);

                if(staff.Experiences != null)
                {
                    foreach(var item in staff.Experiences)
                    {
                        item.StaffID = dbResult.ID;
                        item.CreatedBy = item.ModifiedBy = dbResult.CreatedBy;
                        var dbStaffExperience = new StaffExperience();
                        Copy<VmStaffExperience, StaffExperience>(item, dbStaffExperience);
                        _repoStaffExperience.Add(dbStaffExperience);
                    }
                }

                if(staff.EducationList != null)
                {
                    foreach(var item in staff.EducationList)
                    {
                        var dbStaffEducation = new StaffEducation();
                        item.StaffID = dbResult.ID;
                        item.CreatedBy = item.ModifiedBy = dbResult.CreatedBy;
                        Copy<VmStaffEducation, StaffEducation>(item, dbStaffEducation);

                        _repoStaffEducation.Add(dbStaffEducation);

                    }
                }

                result.IsSuccess = true;
                result.MessageToUser = "Success";
                result.RequestId = dbResult.ID.ToString();
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.MessageToUser = "Error while creating data. Please log a ticket at Web helpdesk."; //ex.Message;
            }

            return result;
        }

        public VmGenericServiceResult UpdateStaff(VmStaff staff)
        {
            //return format
            var result = new VmGenericServiceResult();
            try
            {
                var dbStaff = new Staff();
                Copy<VmStaff, Staff>(staff, dbStaff);
                var dbResult = _repoStaff.Update(dbStaff);

                if(staff.EducationList != null)
                {
                    //get id list from old record 
                    var oldIDs = staff.EducationList.Where(e => e.ID != 0).Select(e => e.ID).ToList();

                    var oldEducationList = _repoStaffEducation.GetOldEducationList(dbStaff.ID);
                    foreach(var item in oldEducationList.Where(e=> !oldIDs.Contains(e.ID)).ToList()) //get removed item
                    {
                        //disable 
                        item.Active = false;
                        item.ModifiedBy = dbStaff.ModifiedBy;
                        _repoStaffEducation.Update(item);
                    }

                    foreach(var item in staff.EducationList)
                    {
                        if (item.ID == 0)
                        {
                            //add new
                            var dbItem = new StaffEducation();
                            item.CreatedBy = item.ModifiedBy = dbStaff.ModifiedBy;
                            item.StaffID = dbStaff.ID;
                            Copy<VmStaffEducation, StaffEducation>(item, dbItem);

                            _repoStaffEducation.Add(dbItem);
                        }
                        else
                        {
                            //update
                            var dbItem = new StaffEducation();
                            item.ModifiedBy = dbStaff.ModifiedBy;
                            Copy<VmStaffEducation, StaffEducation>(item, dbItem);

                            _repoStaffEducation.Update(dbItem);
                        }
                    }



                }

                result.IsSuccess = true;
                result.MessageToUser = "Success";
                result.RequestId = dbResult.ID.ToString();
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.MessageToUser = "Error while updating data. Please log a ticket at Web helpdesk."; //ex.Message;
            }

            return result;
        }

        public VmGenericServiceResult DeleteStaff(int id)
        {
            //return format
            var result = new VmGenericServiceResult();
            try
            {
                var dbStaff = _repoStaff.GetWithoutAsync(id);
                if(dbStaff == null)
                {
                    result.IsSuccess = false;
                    result.MessageToUser = "No data.";
                    return result;
                }
                dbStaff.Active = false;
                var dbResult = _repoStaff.Update(dbStaff);

                result.IsSuccess = true;
                result.MessageToUser = "Success";
                result.RequestId = dbResult.ID.ToString();
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.MessageToUser = "Error while updating data. Please log a ticket at Web helpdesk."; //ex.Message;
            }

            return result;
        }

        public List<VmStaff> GetAllStaff()
        {
            var result = new List<VmStaff>();

            var dbStaffList = _repoStaff.GetActiveAll();

            foreach (var dbStaff in dbStaffList)
            {
                var resultItem = new VmStaff();
                Copy<Staff, VmStaff>(dbStaff, resultItem);
                resultItem.EncryptId = Md5.Encrypt(resultItem.ID.ToString());

                result.Add(resultItem);
            }
            return result;
        }

        public VmStaff GetStaff(int id)
        {
            var dbStaff = _repoStaff.GetStaff(id);
            if (dbStaff == null)
            {
                return null;
            }
            var result = new VmStaff();
            Copy<Staff, VmStaff>(dbStaff, result);
            result.EncryptId = Md5.Encrypt(result.ID.ToString());

            result.Experiences = new List<VmStaffExperience>();
            if(dbStaff.Experiences != null)
            {
                foreach( var dbItem in dbStaff.Experiences)
                {
                    if(dbItem.Active && dbItem.SystemActive == true)
                    {
                        var resultItem = new VmStaffExperience();
                        Copy<StaffExperience, VmStaffExperience>(dbItem, resultItem);
                        resultItem.EncryptId = Md5.Encrypt(resultItem.ID.ToString());

                        result.Experiences.Add(resultItem);
                    }
                }
            }

            result.EducationList = new List<VmStaffEducation>();
            if (dbStaff.Educations != null)
            {
                foreach (var dbItem in dbStaff.Educations)
                {
                    if (dbItem.Active && dbItem.SystemActive)
                    {
                        var resultItem = new VmStaffEducation();
                        Copy<StaffEducation, VmStaffEducation>(dbItem, resultItem);
                        resultItem.EncryptId = Md5.Encrypt(resultItem.ID.ToString());

                        result.EducationList.Add(resultItem);
                    }
                }
            }

            return result;
        }
        #endregion

        #region Education
        public VmGenericServiceResult CreateStaffEducation(VmStaffEducation staffEducation)
        {
            //return format
            var result = new VmGenericServiceResult();
            try
            {
                var dbStaffEducation = new StaffEducation();
                Copy<VmStaffEducation, StaffEducation>(staffEducation, dbStaffEducation);
                var dbResult = _repoStaffEducation.Add(dbStaffEducation);

                result.IsSuccess = true;
                result.MessageToUser = "Success";
                result.RequestId = dbResult.ID.ToString();
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.MessageToUser = "Error while creating data. Please log a ticket at Web helpdesk."; //ex.Message;
            }

            return result;
        }

        public VmGenericServiceResult EditStaffEducation(VmStaffEducation staffEducation)
        {
            //return format
            var result = new VmGenericServiceResult();
            try
            {
                var dbStaffEducation = new StaffEducation();
                Copy<VmStaffEducation, StaffEducation>(staffEducation, dbStaffEducation);
                var dbResult = _repoStaffEducation.Update(dbStaffEducation);

                result.IsSuccess = true;
                result.MessageToUser = "Success";
                result.RequestId = dbResult.ID.ToString();
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.MessageToUser = "Error while updating data. Please log a ticket at Web helpdesk."; //ex.Message;
            }

            return result;
        }

        public VmStaffEducation GetStaffEducation(int id)
        {
            var result = new VmStaffEducation();

            var dbResult = _repoStaffEducation.GetWithoutAsync(id);
            Copy<StaffEducation, VmStaffEducation>(dbResult, result);
            result.EncryptId = Md5.Encrypt(result.ID.ToString());

            return result;
        }
        #endregion

        #region Experience
        public VmGenericServiceResult CreateStaffExperience(VmStaffExperience staffExperience)
        {
            var result = new VmGenericServiceResult();

            var dbStaffExperience = new StaffExperience();
            Copy<VmStaffExperience, StaffExperience>(staffExperience, dbStaffExperience);

            var dbResult = _repoStaffExperience.Add(dbStaffExperience);

            result.IsSuccess = true;
            result.MessageToUser = "Sucessfully created.";

            result.RequestId = dbResult.ID.ToString();


            return result;
        }
        #endregion








    }
}
