using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Okta.SCIM.Models;
//using Okta.SCIM.Server.Connectors;
using Okta.SCIM.Server.Exceptions;
using log4net;
using SCIMSQLConn;
using SCIMSQLConn.Connectors;
//using SCIMInMemConn.Exceptions;

namespace Okta.SCIM.Server.Controllers
{
    [Authorize]
    public class UsersController : ApiController
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // this connector will be initilized via the IOC container.  Passed to the controller constructor.
        // see SimpleInjectorWebAppInitializer.cs
        private static ISCIMConnector connector;

        public UsersController(ISCIMConnector conn)
        {
            connector = conn;
        }

        [HttpGet]
        public IHttpActionResult getAllUsers(int startIndex, int count)
        {
            // used on okta import
            logger.Debug(" Enter getAllUsers 1 ");
            try
            {
                PaginationProperties pp = new PaginationProperties(count, startIndex);
                return Ok<SCIMUserQueryResponse>(connector.getUsers(pp, null));
            }
            catch (Exception e)
            {
                logger.Debug(" Error at getAllUsers 1");
                logger.Error(e);

                return InternalServerError(e);
            }
        }

        [HttpGet]
        public IHttpActionResult getAllUsers(string filter, int startIndex, int count)
        {
            logger.Debug(" Enter getAllUsers 2 ");
            try
            {
                SCIMFilter f = SCIMFilter.TryParse(filter);
                PaginationProperties pp = new PaginationProperties(count, startIndex);

                Okta.SCIM.Models.SCIMUserQueryResponse rGetUsers = connector.getUsers(pp, f);
                //logger.Debug("Exit getAllUsers extId " + rGetUsers.Resources[0].externalId);
                logger.Debug("Exit getAllUsers extId ");
                return Ok<SCIMUserQueryResponse>(rGetUsers);
            }
            catch (Exception e)
            {
                logger.Debug(" Error at getAllUsers 2 ");
                logger.Error(e);
                return InternalServerError(e);
            }
        }



        //for debug only
        [HttpGet]
        public IHttpActionResult getBulkUsers()
        {
            logger.Debug(" Enter getAllUsers 3 ");

            SCIMFilter f = new SCIMFilter();
            PaginationProperties pp = new PaginationProperties(200, 1);

            try
            {


                Okta.SCIM.Models.SCIMUserQueryResponse rGetUsers = connector.getUsers(pp, f);
                logger.Debug("Exit getAllUsers ");
                return Ok<SCIMUserQueryResponse>(rGetUsers);
            }
            catch (Exception e)
            {
                logger.Debug(" Error at getAllUsers 2 ");
                logger.Error(e);
                return InternalServerError(e);
            }
        }

        [HttpGet]
        public IHttpActionResult getUser(string id)
        {
            logger.Debug(" Enter getUser " + id);
            try
            {
                return Ok(connector.getUser(id));
            }
            catch (EntityNotFoundException e)
            {
                logger.Debug(" entity not found tryin gto get user");
                return NotFound();
            }
            catch (Exception e)
            {
                logger.Debug(" Error at getUser  ");
                logger.Error(e);
                return InternalServerError(e);
            }
        }


        [HttpPost]
        public IHttpActionResult PostUser([FromBody] SCIMUser user)
        {
            logger.Debug(" Enter PostUser " + user.userName);
            try
            {
                user = connector.createUser(user);
                string uri = Url.Link("DefaultAPI", new { id = user.id });
                logger.Debug("created user ");
                return Created<SCIMUser>(uri, user);
            }
            catch (Exception e)
            {
                logger.Debug(" Exception at PostUser " + user.userName);
                logger.Error(e);

                return InternalServerError(e);
            }
        }


        [HttpPut]
        public IHttpActionResult Put(string id, [FromBody]SCIMUser user)
        {
            logger.Debug(" Enter Put " + user.displayName + " Id " + id);

            if (user.id != id)
            {
                logger.Error(" Error at PUT User, user.id and id dont match ");
                return InternalServerError();
            }

            if (user.active)
            {
                try
                {
                    //user.id = id;
                    user = connector.updateUser(user);
                    if (string.IsNullOrEmpty(user.id))
                    {
                        logger.Debug(" application Id not set ");
                        return NotFound();
                    }
                    else
                    {
                        //return Ok();
                        logger.Debug("updated  user  username " + user.userName + "  appId " + user.id);
                        return Ok<SCIMUser>(user);
                    }
                }
                catch (Exception e)
                {
                    logger.Debug(" Error at PUT User  ");
                    logger.Error(e);
                    return InternalServerError(e);
                }
            }
            else
            {
                //implementation choice, if user.active is false, delete
                try
                {
                    connector.deleteUser(id);
                    logger.Debug("delete user id " + id);
                    return Ok();
                }
                catch (Exception e)
                {
                    logger.Debug(" Error when deleting User  ");
                    logger.Error(e);
                    return InternalServerError(e);
                }

            }

        }


        [HttpPatch]
        public IHttpActionResult Patch(String id)
        {
            logger.Debug(" Enter Patch " + id);
            try
            {
                //SCIMUser user = connector.getUser(id);
                //if (user == null)
                //{
                //    return NotFound();
                //}
                connector.deleteUser(id);
                logger.Debug("delete user id " + id);
                return Ok();
            }
            catch (Exception e)
            {
                logger.Debug(" Error at Delete User  ");
                logger.Error(e);
                return InternalServerError(e);
            }
        }


        [HttpDelete]
        public IHttpActionResult Delete(String id)
        {
            logger.Debug(" Enter Delete " + id);
            try
            {
                //SCIMUser user = connector.getUser(id);
                //if (user == null)
                //{
                //    return NotFound();
                //}
                connector.deleteUser(id);
                logger.Debug("delete user id " + id);
                return Ok();
            }
            catch (Exception e)
            {
                logger.Debug(" Error at Delete User  ");
                logger.Error(e);
                return InternalServerError(e);
            }
        }
    }
}
