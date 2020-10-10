﻿using Com.MrIT.Common;
using Com.MrIT.Common.Configuration;
using Com.MrIT.DataRepository;
using Com.MrIT.DBEntities;
using Com.MrIT.ViewModels;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.MrIT.Services
{
    public class StaffService : BaseService, IStaffService
    {
        private readonly IStaffRepository _repoStaff;
        private readonly AppSettings _appSettings;
        public StaffService(IStaffRepository repoStaff, IOptions<AppSettings> appSettings)
        {
            this._repoStaff = repoStaff;
            this._appSettings = appSettings.Value;
        }

        public VmGenericServiceResult CreateStaff(VmStaff staff)
        {
            var result = new VmGenericServiceResult();
            var dbStaff = new Staff();
            try
            {
                Copy<VmStaff, Staff>(staff, dbStaff);
                var dbResult = _repoStaff.Add(dbStaff);

                result.IsSuccess = true;
                result.MessageToUser = "Success";
                result.RequestId = dbResult.ID.ToString();
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.MessageToUser = ex.Message;
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
            var dbStaff = _repoStaff.GetWithoutAsync(id);
            var result = new VmStaff();
            Copy<Staff, VmStaff>(dbStaff, result);
            result.EncryptId = Md5.Encrypt(result.ID.ToString());

            return result;
        }

    }
}