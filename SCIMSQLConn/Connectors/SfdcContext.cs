using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;
using Newtonsoft.Json;
using Okta.SCIM.Server.Exceptions;
using System.Configuration;


namespace SfdcOPPConn.Connectors
{

    public class sabaCertJson
    {
        public string @type { get; set; }
        public string certificate { get; set; }
    }


    public static class SfdcContext
    {
      //  System.Collections.Specialized.NameValueCollection  appSettings = ConfigurationManager.AppSettings;

                    


        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static string username = ConfigurationManager.AppSettings["sfdc.username"];
        public static string password = ConfigurationManager.AppSettings["sfdc.password"];
        public static string tokenUrl = ConfigurationManager.AppSettings["sfdc.tokenUrl"];
        public static string authUrl = ConfigurationManager.AppSettings["sfdc.authUrl"];
        public static string clientId = ConfigurationManager.AppSettings["sfdc.clientId"];
        public static string clientSecret = ConfigurationManager.AppSettings["sfdc.clientSecret"];
        public static string baseUrl = ConfigurationManager.AppSettings["sfdc.baseUrl"];
        public static string redirectUrl = ConfigurationManager.AppSettings["sfdc.redirectUrl"];
        public static string apiVersion = ConfigurationManager.AppSettings["sfdc.apiVersion"];
        public static  string bearerToken;
        public  static DateTime sabaCertCreateDate;
 

    }
    
}