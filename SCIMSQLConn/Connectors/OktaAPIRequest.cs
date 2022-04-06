using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Okta.Core.Clients;
using Okta.Core.Models;
using log4net;
using System.Configuration;

namespace SCIMSQLConn.Connectors
{
    public class OktaAPIRequest
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static string GetOktaId(string oktausername)
        {
            logger.Debug("enter getOktaId");
            var appSettings = ConfigurationManager.AppSettings;
            User oktaUser = new User();

            //create OktaClient
            //A convenience client to build all other clients without building a new BaseClient for every one.
            //OktaClass class contains OrgUrl, Org Token
            OktaClient oktaClient = new OktaClient(appSettings["org.token"], new Uri(appSettings["org.baseURL"]));

            //A client to manage Users
            //UserClient class contains OrgUrl, API resource path, OrgToken
            UsersClient usersClient = oktaClient.GetUsersClient();
            oktaUser = usersClient.Get(oktausername);
            return oktaUser.Id;
        }
    }
}