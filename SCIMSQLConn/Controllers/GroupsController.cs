using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
//using Okta.SCIM.Server.Connectors;
using Okta.SCIM.Models;
using Okta.SCIM.Server.Exceptions;
using SCIMSQLConn;
using SCIMSQLConn.Connectors;
using log4net;

namespace Okta.SCIM.Server.Controllers
{

    public class GroupsController : ApiController
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // this connector will be initilized via the IOC container.  Passed to the controller constructor.
        // see SimpleInjectorWebAppInitializer.cs
        private static ISCIMConnector connector;
        public GroupsController(ISCIMConnector conn)
        {
            connector = conn;
        }



        [HttpGet]
        public IHttpActionResult getAllGroups(int startIndex, int count)
        {
            logger.Debug("getAllGroups ");
            try
            {
                PaginationProperties pp = new PaginationProperties(count, startIndex);
                return Ok<SCIMGroupQueryResponse>(connector.getGroups(pp));
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        [HttpGet]
        public IHttpActionResult getGroup(string id)
        {
            logger.Debug("getGroup " + id);
            try
            {
                return Ok(connector.getGroup(id));
            }
            catch (EntityNotFoundException e)
            {
                return NotFound();
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        [HttpPost]
        public IHttpActionResult PostGroup([FromBody] SCIMGroup group)
        {
            logger.Debug("PostGroup " + group.displayName);
            try
            {
                group = connector.createGroup(group);
                string uri = Url.Link("DefaultAPI", new { id = group.id });
                return Created<SCIMGroup>(uri, group);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        [HttpPut]
        public IHttpActionResult Put(string id, [FromBody]SCIMGroup group)
        {
            logger.Debug("Put group " + group.displayName);
            try
            {
                // TODO: duplicate group exception
                group.id = id;
                if (!connector.updateGroup(group))
                {
                    return NotFound();
                }
                else
                {
                    return Ok();
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        [HttpDelete]
        public IHttpActionResult Delete(String id)
        {
            logger.Debug("Delete id " + id);
            try
            {
                SCIMGroup group = connector.getGroup(id);
                if (group == null)
                {
                    return NotFound();
                }

                connector.deleteGroup(id);
                return Ok();
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

    }
}
