using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Okta.SCIM.Models;
//using Okta.SCIM.Server.Connectors;
using log4net;
using SCIMSQLConn;
using SCIMSQLConn.Connectors;

namespace Okta.SCIM.Server.Controllers
{
    //this is for scim 1.1
    //note the 's' on Configs
    public class ServiceProviderConfigsController : ApiController
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // this connector will be initilized via the IOC container.  Passed to the controller constructor.
        // see SimpleInjectorWebAppInitializer.cs
        private static ISCIMConnector connector;
        public ServiceProviderConfigsController(ISCIMConnector conn)
        {
            connector = conn;
        }
        public IHttpActionResult getAll()
        {
            logger.Debug(" enter getAll serviceproviderConfig");
            return Ok(connector.getServiceProviderConfig());
        }
    }
}
