using Com.MrIT.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.MrIT.Services
{
    public interface IEmailService
    {
        void SendForRegistration(VmRegistrationEmail emailContent, string user);

        void InsertErrorAndEmail(Exception ex, string user, string url);

        void SendForContactUs(VmContactUsEmail emailContent, string user);
    }
}
