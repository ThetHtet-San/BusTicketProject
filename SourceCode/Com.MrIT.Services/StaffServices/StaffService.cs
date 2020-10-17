using Com.MrIT.Common;
using Com.MrIT.Common.Configuration;
using Com.MrIT.DataRepository;
using Com.MrIT.DBEntities;
using Com.MrIT.ViewModels;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
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
