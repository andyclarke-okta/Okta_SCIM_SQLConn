using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Web;
using Okta.SCIM.Models;
using Okta.SCIM.Server.Exceptions;
using System.Web.Http;
using System.Web.Http.Tracing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Configuration;


namespace SCIMSQLConn.Connectors
{
    // a partially implemented In Memory Connector using threadsafe concurrent dictionary
    // switched to building the SQLServerSCIM connector once I was able to demonstrate that 
    // I could switch containers using the IOC container.
    public class InMemorySCIMConnector : ISCIMConnector
    {
        private static ConcurrentDictionary<string, SCIMUser> users = null;
        private static ConcurrentDictionary<string, SCIMGroup> groups = null;
        public InMemorySCIMConnector()
        {
            
            if (users == null)
            {
                users = new ConcurrentDictionary<string, SCIMUser>();
                groups = new ConcurrentDictionary<string, SCIMGroup>();
                //createGroup(new SCIMGroup { displayName = "admins" });
                //createGroup(new SCIMGroup { displayName = "execs" });
                String path = AppDomain.CurrentDomain.GetData("DataDirectory").ToString();
                System.Diagnostics.Debug.WriteLine(path);
                using (System.IO.StreamReader file = System.IO.File.OpenText(path + "/10Users.json"))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    SCIMUserQueryResponse response =
                        (SCIMUserQueryResponse)serializer.Deserialize(file, typeof(SCIMUserQueryResponse));
                    foreach (SCIMUser u in response.Resources)
                    {
                        //System.Diagnostics.Debug.WriteLine(u.userName);
                        createUser(u);
                    }
                }

            }
        }
        public SCIMUser createUser(SCIMUser user)
        {
            try
            {
                if (user.id == null)
                {
                    user.id = Guid.NewGuid().ToString();
                }
                else if (users.ContainsKey(user.id))
                {
                        // duplicate user
                        throw new OnPremUserManagementException("User id already exists");
                }
                users.TryAdd(user.id, user);               
            }
            catch (Exception e)
            {
                throw e;
            }
            return user;
        }
        public SCIMUser getUser(string id)
        {
            SCIMUser user = null;
            try
            {
                users.TryGetValue(id, out user);
            }
            catch (Exception e)
            {
                throw new OnPremUserManagementException("getUser", e);
            }
            if (user == null)
            {
                throw new EntityNotFoundException(id);
            }
            return user;
        }
        private List<SCIMUser> filterUsers(List<SCIMUser> users, SCIMFilter filter)
        {
            System.Diagnostics.Debug.WriteLine(filter.FilterAttribute.AttributeName);
            return users;
            // simple attribute
            // dictionary attribute
            // equals query
            // or query
        }
        public SCIMUserQueryResponse getUsers(PaginationProperties pageProperties, SCIMFilter filter)
        {           
            try
            {
                List<SCIMUser> ulist = users.Values.ToList<SCIMUser>();

                if (filter != null)
                {
                    ulist = filterUsers(ulist, filter);
                }

                int totalRecords = users.Count;
                int startIndex = pageProperties.startIndex > 0 && pageProperties.startIndex <= totalRecords ? pageProperties.startIndex : 1;
                int recordCount = startIndex + pageProperties.count <= totalRecords ? pageProperties.count : totalRecords - startIndex + 1;

                SCIMUserQueryResponse response = new SCIMUserQueryResponse();
                response.schemas = new List<String>();
                response.totalResults = totalRecords;
                response.startIndex = startIndex;
                response.itemsPerPage = recordCount;

                response.Resources = ulist.GetRange(startIndex - 1, recordCount);
                return response;
            }
            catch (Exception e)
            {
                throw new OnPremUserManagementException("getUsers", e);
            }
        }
        public SCIMUser updateUser(SCIMUser user)
        {
            bool result = false;
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            SCIMUser existingUser;
            if (users.TryGetValue(user.id, out existingUser))
            {
                result = users.TryUpdate(user.id, user, existingUser);
            }
            return user;
        }
        public bool deleteUser(string id)
        {
            SCIMUser user;
            bool result;
            result = users.TryRemove(id, out user);
            return result;
        }

        // groups
        public SCIMGroup createGroup(SCIMGroup group)
        {
            if (group == null)
            {
                throw new ArgumentNullException("group");
            }
            group.id = Guid.NewGuid().ToString();
            groups.TryAdd(group.id, group);
            return group;

        }
        public SCIMGroup getGroup(string id)
        {
            SCIMGroup group;
            try
            {
                if (groups.TryGetValue(id, out group))
                {
                    return group;
                }
                else
                {
                    throw new EntityNotFoundException(id);
                }
            }
            catch (Exception)
            {
                throw new OnPremUserManagementException();
            }
        }
        public SCIMGroupQueryResponse getGroups(PaginationProperties pp)
        {
            try
            {
                SCIMGroupQueryResponse response = new SCIMGroupQueryResponse();
                response.Resources = groups.Values.ToList();
                response.totalResults = groups.Count;
                return response;
            }
            catch (Exception)
            {
                throw new OnPremUserManagementException();
            }
        }
        public bool deleteGroup(string id)
        {
            SCIMGroup group;
            bool result;
            result = groups.TryRemove(id, out group);
            return result;
        }
        public bool updateGroup(SCIMGroup group)
        {
            throw new NotImplementedException();
        }
        public ServiceProviderConfiguration getServiceProviderConfig()
        {
            //throw new NotImplementedException();
            var appSettings = ConfigurationManager.AppSettings;
            ServiceProviderConfiguration cfg = new ServiceProviderConfiguration();
            cfg.schemas = new List<string>() { "urn:schemas:core:1.0", "urn:okta:schemas:scim:providerconfig:1.0" };
            List<string> usermgmt = new List<string>() { 
                UserManagementCapabilities.PUSH_PASSWORD_UPDATES, 
                UserManagementCapabilities.GROUP_PUSH,
                UserManagementCapabilities.IMPORT_NEW_USERS, 
                UserManagementCapabilities.IMPORT_PROFILE_UPDATES,
                UserManagementCapabilities.PUSH_NEW_USERS, 
                UserManagementCapabilities.PUSH_PROFILE_UPDATES
                };
            cfg.ExtensionData = new Dictionary<string, object>();
            cfg.ExtensionData.Add(appSettings["sfdc.custom_extension_urn"], usermgmt);
            return cfg;
        }
    }
}