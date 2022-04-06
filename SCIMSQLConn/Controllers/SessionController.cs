using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using log4net;

namespace SfdcOPSCIMInMemConnPConn.Controllers
{
    public class SessionController : ApiController
    {

        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        // GET: api/Session
        public IEnumerable<string> Get()
        {
            logger.Debug(" Enter PostUser string ");
            return new string[] { "value1", "value2" };
        }

        // GET: api/Session/5
        public string Get(int id)
        {
            logger.Debug(" Enter Get id " + id);
            return "value";
        }

        // POST: api/Session
        public void Post([FromBody]string value)
        {
            logger.Debug(" Enter Post " + value);
        }

        // PUT: api/Session/5
        public void Put(int id, [FromBody]string value)
        {
            logger.Debug(" Enter Put " + id);
        }

        // DELETE: api/Session/5
        public void Delete(int id)
        {
            logger.Debug(" Enter Delete " + id);
        }
    }
}
