using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Okta.SCIM.Models;
using Okta.SCIM.Server.Exceptions;
using System.Data.Entity.Validation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using log4net;
using SCIMSQLConn;
using System.Configuration;
using System.Collections.Specialized;

namespace SCIMSQLConn.Connectors
{
    public class SQLServerSCIMConnector : ISCIMConnector
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        NameValueCollection appSettings = ConfigurationManager.AppSettings;


        private static employeesEntities context = new employeesEntities();
        private String buildUserName(String familyName, String givenName)
        {
            return (familyName + givenName).Replace(" ", "") + "@mysql - db.org";
        }
        private employee userToEmployee(SCIMUser user)
        {
            employee e = new employee();
            userToEmployee(user, e);
            return e;
        }
        private employee userToEmployee(SCIMUser user, employee e)
        {
            e.first_name = user.name.givenName;
            e.last_name = user.name.familyName;
            e.username = user.userName;
            e.primary_email = user.emails[0].value;
            e.active = user.active;
            if (user.ExtensionData != null && user.ExtensionData.ContainsKey(appSettings["okta.custom_extension_urn"]))
            {
                IDictionary<string,JToken> o = (JObject) user.ExtensionData[appSettings["okta.custom_extension_urn"]];
                if (o["birth_date"] != null)
                {
                    e.birth_date = Convert.ToDateTime(o["birth_date"]);
                }
                if (o["hire_date"] != null)
                {
                    e.hire_date = Convert.ToDateTime(o["hire_date"]);
                }
                e.gender = o["gender"].ToString();
            }
            //ajc debug
            //e.birth_date = DateTime.Now;
            //e.gender = "M";
            //e.birth_date = DateTime.Now;
            return e;
        }     
        private SCIMUser employeeToUser(employee emp)
        {
            SCIMUser user = new SCIMUser();
            user.id = Convert.ToString(emp.emp_no);
            user.name = new Name()
            {
                familyName = emp.last_name,
                formatted = emp.first_name + " " + emp.last_name,
                givenName = emp.first_name
            };
            //string email = buildUserName(user.name.familyName, user.name.givenName);
            user.userName = emp.username;
            user.emails = new List<Email> { new Email() { primary = true, type = "work", value = emp.primary_email } };
            //user.active = true;
            user.active = emp.active;
            user.password = null;

            var query = from d in context.dept_emp
                        where d.emp_no == emp.emp_no
                        select d.department;
 
            user.groups = new List<Member>();
            foreach (department d in query)
            {
                user.groups.Add(new Member() { display = d.dept_name, value = d.dept_no });
            }

            // check for extension data
            Dictionary<string, string> extensions = new Dictionary<string, string>();
            //extensions.Add("birth_date", emp.birth_date.ToString("yyyy-MM-dd"));
            extensions.Add("birth_date", emp.birth_date.ToString());
            extensions.Add("gender", emp.gender);
            //extensions.Add("hire_date", emp.hire_date.ToString("yyyy-MM-dd"));
            extensions.Add("hire_date", emp.hire_date.ToString());
            user.ExtensionData = new Dictionary<string, object>();
            user.ExtensionData.Add(appSettings["okta.custom_extension_urn"], extensions);

            return user;
             
        }
        public SCIMUser createUser(SCIMUser user)
        {
            logger.Debug(" create user " + user.displayName);

            try
            {
                employee e = userToEmployee(user);
                context.employees.Add(e);
                context.SaveChanges();
                user.id = Convert.ToString(e.emp_no);
                return user;
            }
            catch (DbEntityValidationException dbex)
            {
                foreach (var evx in dbex.EntityValidationErrors)
                {
                    foreach(var ve in evx.ValidationErrors)
                    {
                        System.Diagnostics.Debug.WriteLine(ve.ErrorMessage);
                        System.Diagnostics.Debug.WriteLine(ve.PropertyName);
                    }
                }
                throw dbex;
            }
            catch (Exception ex)
            {
                logger.Error(" error creating user");
                throw new OnPremUserManagementException("createUser", ex);
            }
        }
        public SCIMUser getUser(string id)
        {
            logger.Debug(" getUser");

            employee e;
            try
            {
                int converted_id = Convert.ToInt32(id);
                e = context.employees.FirstOrDefault<employee>(x => x.emp_no == converted_id);
                if (e == null)
                {
                    throw new EntityNotFoundException(id);
                }
                return employeeToUser(e);
            }
            catch (Exception ex)
            {
                if (ex is EntityNotFoundException)
                {
                    logger.Error(" error getting user Entity not found");
                    throw ex;
                }
                else
                {
                    logger.Error(" error getting user");
                    throw new OnPremUserManagementException("getUser", ex);
                }
            }
        }


        private List<SCIMUser> filterUsers(List<SCIMUser> users, SCIMFilter filter)
        {
            //System.Diagnostics.Debug.WriteLine(filter.FilterAttribute.AttributeName);
            logger.Debug("filterUsers type " + filter.FilterAttribute + " value " + filter.FilterValue);
            List<SCIMUser> filteredUsers = new List<SCIMUser>();
            string criteria = filter.FilterValue.Replace("\"", "");


            // simple attribute
            // dictionary attribute
            // equals query
            // or query

            if (filter.FilterType == SCIMFilterType.EQUALS)
            {
                if (filter.FilterAttribute.AttributeName == "userName")
                {
                    //check userName equals any existing users
                    foreach (var item in users)
                    {
                        if (item.userName == criteria)
                        {
                            filteredUsers.Add(item);
                        }
                    }

                }
            }

            return filteredUsers;
        }

        public SCIMUserQueryResponse getUsers(PaginationProperties pageProperties, SCIMFilter filter)
        {

            logger.Debug("getUsers by filter ");
            int totalRecords = 0;
            try
            {

                string criteria = filter.FilterValue.Replace("\"", "");
                var query = from e in context.employees
                where e.username == criteria
                            orderby e.emp_no
                select e;

                List<SCIMUser> uList = new List<SCIMUser>();

                int current_page = pageProperties.startIndex / pageProperties.count + 1;

                foreach (var emp in query.Page<employee>(current_page, pageProperties.count))
                {
                    uList.Add(employeeToUser(emp));
                }
                totalRecords = uList.Count;

                //List<SCIMUser> fList = null;

                //if (filter != null)
                //{
                //    fList = filterUsers(uList, filter);
                //    totalRecords = fList.Count;
                //}
                //else
                //{
                //    totalRecords = uList.Count;
                //}


                int startIndex = pageProperties.startIndex > 0 && pageProperties.startIndex <= totalRecords ? pageProperties.startIndex : 1;
                int recordCount = startIndex + pageProperties.count <= totalRecords ? pageProperties.count : totalRecords - startIndex + 1;

                SCIMUserQueryResponse response = new SCIMUserQueryResponse();
                response.schemas = new List<String>();
                //response.schemas = userSchemas;
                response.totalResults = totalRecords;
                response.startIndex = startIndex;
                response.itemsPerPage = recordCount;
                //if (filter != null)
                //{
                //    response.Resources = fList.GetRange(startIndex - 1, recordCount);
                //}
                //else
                //{
                    response.Resources = uList.GetRange(startIndex - 1, recordCount);
                //}

                return response;
            }
            catch (Exception e)
            {
                logger.Error(" error getting users");
                throw new OnPremUserManagementException("getUsers", e);
            }



            //           try
            //           {
            //               SCIMUserQueryResponse response = new SCIMUserQueryResponse();

            //               //TODO: Apply Filter
            ////               Func<employee, bool> filterFunction = EmployeeFilters.buildFilterFunction(filter);

            //               var query = from e in context.employees
            //                           orderby e.emp_no
            //                           select e;

            //               response.Resources = new List<SCIMUser>();

            //               int current_page = pp.startIndex / pp.count + 1;

            //               foreach (var emp in query.Page<employee>(current_page,pp.count))
            //               {
            //                   response.Resources.Add(employeeToUser(emp));
            //               }
            //               response.startIndex = pp.startIndex;
            //               response.itemsPerPage = pp.count;
            //               response.totalResults = query.Count();
            //               return response;
            //           }



        }
        public SCIMUser updateUser(SCIMUser user)
        {
            logger.Debug(" updateUser "  + user.displayName);

            try
            {

                int eid = Convert.ToInt32(user.id);
                employee emp = (from e in context.employees
                                where e.emp_no == eid
                                select e).Single();
                emp = userToEmployee(user, emp);
                emp.emp_no = eid;
                context.Entry(emp).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
                return user;
            }
            catch (Exception e)
            {
                logger.Error(" error updating user");
                throw new OnPremUserManagementException("updateUser", e);
            }
        }
        public bool deleteUser(string id)
        {
            logger.Debug(" deleteUser "  + id);

            bool result = false;
            try
            {
                int convertedID = Convert.ToInt32(id);
                var deleteCMD = (from e in context.employees
                                where e.emp_no == convertedID
                                select e).First();
                logger.Debug("deleting user with CMD " + deleteCMD);
                context.employees.Remove(deleteCMD);
                context.SaveChanges();
            }
            catch (Exception e)
            {
                logger.Error(" error deleting user");
                throw new OnPremUserManagementException("deleteUser", e);
            }
            return result;
        }
        private List<Member> getGroupMembers(string id)
        {
            logger.Debug(" getGroupMembers ");
            try
            {
                var query = from employee e in context.employees
                            join dept_emp de in context.dept_emp
                            on e.emp_no equals de.emp_no
                            where de.dept_no == id
                            select e;

                List<Member> members = new List<Member>();

                foreach (var emp in query)
                {
                    members.Add(new Member()
                    {
                        value = Convert.ToString(emp.emp_no),
                        display = buildUserName(emp.last_name, emp.first_name)
                    });
                }

                return members;

            }
            catch (Exception e)
            {
                logger.Error(" error gettign group members");
                throw new OnPremUserManagementException("Error getting group members", e);
            }
        }
        private SCIMGroup departmentToGroup(department dept)
        {
            logger.Debug(" departmentToGroup ");
            try
            {
                SCIMGroup g = new SCIMGroup();
                g.id = dept.dept_no;
                g.displayName = dept.dept_name;
                //TODO: how to handle membership
                //g.members = getGroupMembers(g.id);
                return g;
            }
            catch (Exception e)
            {
                logger.Error(" error department to group");
                throw new OnPremUserManagementException("Error converting department to group", e);
            }
        }
        private department groupToDepartment(SCIMGroup group)
        {
            logger.Debug(" groupToDepartment ");
            try
            {
                department d = new department();
                d.dept_no = group.id;
                d.dept_name = group.displayName;
                //TODO: how to handle membership
                // d.dept_emp = new List<dept_emp>();
                //foreach (Member m in group.members)
                //{
                //    // find the employee
                //    var employee = (from e in context.employees
                //                 where e.emp_no == 1
                //                 select e).First<employee>();
                //    dept_emp de = new dept_emp() {d}
                //    d.dept_emp.Add();
                //}
                return d;
            }
            catch (Exception e)
            {
                logger.Error(" error group to department");
                throw new OnPremUserManagementException("Error converting SCIM group to Department", e);
            }
        }
        public SCIMGroup createGroup(SCIMGroup group)
        {
            logger.Debug(" createGroup ");
            try
            {
                string groupName = group.displayName;
                department dept = (from d in context.departments
                             where d.dept_name == groupName
                             select d).FirstOrDefault<department>();
                if (dept != null)
                {
                    throw new DuplicateGroupException("Group already exists in the system");
                }
                dept = groupToDepartment(group);
                dept.dept_no = Convert.ToString(context.departments.Count() + 1);
                group.id = dept.dept_no;
                context.departments.Add(dept);
                // TODO: add membership?
                context.SaveChanges();
                return group;
            }
            catch (Exception e)
            {
                if (e is DuplicateGroupException)
                {
                    logger.Error(" error creating group: duplicate");
                    throw e;
                }
                else
                {
                    logger.Error(" error creating group");
                    throw new OnPremUserManagementException("Error creating Department", e);
                }
            }
        }
        public SCIMGroup getGroup(string id)
        {
            logger.Debug(" getGroup ");
            try
            {
                department dept = (from d in context.departments
                                   where d.dept_no == id
                                   select d).First();
                if (dept == null)
                {
                    throw new EntityNotFoundException(id);
                }
                return departmentToGroup(dept);
            }
            catch (Exception e)
            {
                if (e is EntityNotFoundException)
                {
                    logger.Error(" error getting group: entity not found");
                    throw e;
                }
                else
                {
                    logger.Error(" error getting group");
                    throw new OnPremUserManagementException("getGroup", e);
                }
            }
        }
        public SCIMGroupQueryResponse getGroups(PaginationProperties pp)
        {
            logger.Debug(" getGroups ");
            try
            {
                SCIMGroupQueryResponse response = new SCIMGroupQueryResponse();
                var query = from d in context.departments
                            orderby d.dept_no
                            select d;
                response.Resources = new List<SCIMGroup>();

                int current_page = pp.startIndex / pp.count + 1;

                foreach (var dept in query.Page<department>(current_page, pp.count))
                {
                    response.Resources.Add(departmentToGroup(dept));
                }
                response.startIndex = pp.startIndex;
                response.itemsPerPage = pp.count;
                response.totalResults = query.Count();
                return response;
            }
            catch (Exception e)
            {
                logger.Error(" error getting groups");
                throw new OnPremUserManagementException("Error getting groups", e);
            }
        }
        //TODO
        public bool updateGroup(SCIMGroup group)
        {
            logger.Debug(" updateGroups: not implemented ");
            //String query = "UPDATE departments set dept_name = ? WHERE dept_no = ?";
            throw new NotImplementedException();
        }
        //TODO
        public bool deleteGroup(string id)
        {
            logger.Debug(" deleteGroup: not implemented ");
            try
            {
                // delete any dept_emp records
                // delete any department records
            }
            catch (Exception e)
            {
                throw;
            }
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
            cfg.ExtensionData.Add(appSettings["okta.custom_extension_urn"], usermgmt);

            //cfg.schemas = new List<string>() { "urn:schemas:core:1.0", "urn:okta:schemas:scim:providerconfig:1.0" };
            //List<string> usermgmt = new List<string>() {
            //    UserManagementCapabilities.PUSH_PASSWORD_UPDATES,
            //    UserManagementCapabilities.GROUP_PUSH,
            //    UserManagementCapabilities.IMPORT_NEW_USERS,
            //    UserManagementCapabilities.IMPORT_PROFILE_UPDATES,
            //    UserManagementCapabilities.PUSH_NEW_USERS,
            //    UserManagementCapabilities.PUSH_PROFILE_UPDATES
            //    };
            //cfg.ExtensionData = new Dictionary<string, object>();
            //cfg.ExtensionData.Add("urn:okta:schemas:scim:providerconfig:1.0", usermgmt);


            return cfg;
        }
    }
}