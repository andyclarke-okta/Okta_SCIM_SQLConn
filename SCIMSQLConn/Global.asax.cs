using CsvHelper;
using SCIMSQLConn;
using SCIMSQLConn.Connectors;
//using Okta.SCIM.Server.Connectors;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;


namespace SCIMSQLConn
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            log4net.Config.XmlConfigurator.Configure(new FileInfo(Server.MapPath("~/Web.config")));

            //FileStream fs = null;
            //try
            //{
            //    string filePath = Context.Server.MapPath("~/App_Data/igloomap.csv");
            //    fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            //    PPOAADToIglooGroupMap map = new PPOAADToIglooGroupMap();
            //    using (TextReader tr = new StreamReader(fs))
            //    {
            //        fs = null;
            //        var csv = new CsvReader(tr);
            //        var records = csv.GetRecords<PPOAADToIglooGroupMapRecord>();
            //        map.mappings = records.ToList<PPOAADToIglooGroupMapRecord>();
            //        Application.Add("IglooGroupMap", map);
            //    }
            //}
            //finally
            //{
            //    if (fs != null)
            //        fs.Dispose();
            //}

        }
    }
}
