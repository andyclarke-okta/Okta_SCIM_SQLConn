using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;
using SfdcOPPConn.Connectors;
using SfdcOPPConn.Models;

namespace SfdcOPPConn.Connectors
{
    public static class SfdcAuditLog
    {
        static ILog audit = LogManager.GetLogger("AuditLogFile");

        public static void SabaAudit(SabaUserBasic sabaUserBasic,string sabaApiStatus,string eventResults)
        {
            string currentTime = DateTime.Now.ToString();
          //  audit.Info("sabaUserBasic");
            audit.Info(" " + "," + sabaUserBasic.affiliateCode + "," + sabaUserBasic.username + "," + sabaUserBasic.fname + "," + sabaUserBasic.lname + "," + " " + "," + " " + "," + sabaUserBasic.zip + "," + sabaUserBasic.person_no + "," +
sabaUserBasic.manager + "," + sabaUserBasic.job_type + "," + sabaUserBasic.person_type + "," + " " + "," + sabaUserBasic.is_manager + "," + sabaUserBasic.alternate_manager + ","   + sabaUserBasic.workphone + "," +
" " + "," + " " + "," + " " + "," + " " + "," +
" " + "," + " " + "," + " " + "," + " " + "," +
" " + "," + " " + sabaApiStatus + "," + eventResults + "," + currentTime);

        }

        


        public static void SabaAudit(SabaUserCreate sabaUserCreate, SabaUserBasic sabaUserBasic, string sabaApiStatus, string eventResults)
        {
            string currentTime = DateTime.Now.ToString();
         //   audit.Info("sabaUserCreate");
            audit.Info(sabaUserCreate.securityDomain.displayName + "," + sabaUserBasic.affiliateCode + "," + sabaUserBasic.username + "," + sabaUserBasic.fname + "," + sabaUserBasic.lname + "," + sabaUserCreate.status + "," + sabaUserCreate.timezone_id.id + "," + sabaUserBasic.zip + "," + sabaUserBasic.person_no + "," +
    sabaUserBasic.manager + "," + sabaUserBasic.job_type + "," + sabaUserBasic.person_type + "," + sabaUserCreate.locale_id.displayName + "," + sabaUserBasic.is_manager + "," + sabaUserBasic.alternate_manager + ","  + sabaUserBasic.workphone + "," +
    sabaUserCreate.securityRoles[0].name + "," + sabaUserCreate.securityRoles[0].securityDomain.displayName + "," + sabaUserCreate.securityRoles[1].name + "," + sabaUserCreate.securityRoles[1].securityDomain.displayName  + "," +
    sabaUserCreate.securityRoles[2].name + "," + sabaUserCreate.securityRoles[2].securityDomain.displayName + "," + sabaUserCreate.securityRoles[3].name + "," + sabaUserCreate.securityRoles[3].securityDomain.displayName + "," +
    sabaUserCreate.securityRoles[4].name + "," + sabaUserCreate.securityRoles[4].securityDomain.displayName + "," + sabaApiStatus + "," + eventResults + "," + currentTime);
        }

        public static void SabaAudit(SabaUserUpdate sabaUserUpdate, SabaUserBasic sabaUserBasic, string sabaApiStatus, string eventResults)
        {
            string currentTime = DateTime.Now.ToString();
           // audit.Info("sabaUserUpdate");
            audit.Info(sabaUserUpdate.securityDomain.displayName + "," + sabaUserBasic.affiliateCode + "," + sabaUserBasic.username + "," + sabaUserBasic.fname + "," + sabaUserBasic.lname + "," + sabaUserUpdate.status + "," + sabaUserUpdate.timezone_id.id + "," + sabaUserBasic.zip + "," + sabaUserBasic.person_no + "," +
                sabaUserBasic.manager + "," + sabaUserBasic.job_type + "," + sabaUserBasic.person_type + "," + sabaUserUpdate.locale_id.displayName + "," + sabaUserBasic.is_manager + "," + sabaUserBasic.alternate_manager + ","  + sabaUserBasic.workphone + "," +
                sabaUserUpdate.securityRoles[0].name + "," + sabaUserUpdate.securityRoles[0].securityDomain.displayName + "," + sabaUserUpdate.securityRoles[1].name + "," + sabaUserUpdate.securityRoles[1].securityDomain.displayName + "," +
                sabaUserUpdate.securityRoles[2].name + "," + sabaUserUpdate.securityRoles[2].securityDomain.displayName + "," + sabaUserUpdate.securityRoles[3].name + "," + sabaUserUpdate.securityRoles[3].securityDomain.displayName + "," +
                sabaUserUpdate.securityRoles[4].name + "," + sabaUserUpdate.securityRoles[4].securityDomain.displayName + "," + sabaApiStatus + "," + eventResults + "," + currentTime);

        }

    }
}