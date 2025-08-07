using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace TopSpeed.Application.ApplicationConstants
{
    public class ApplicationConstant
    {

    }

    public static class CommonMessage 
    {
        public static string RecordCreated = "Record Created Success fully";
        public static string RecordUpdated = "Record Updated Success fully";
        public static string RecordDeleted = "Record Deleted Success fully";
    }

    public static class CustomRole 
    {
        public const string MasterAdmin = "MASTERADMIN";
        public const string Admin = "ADMIN";
        public const string Customer = "CUSTOMER";
    }
}
