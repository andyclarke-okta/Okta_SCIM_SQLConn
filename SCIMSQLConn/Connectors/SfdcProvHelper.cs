using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Text;
using System.Xml;
using System.Collections;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Okta.SCIM.Models;
using Okta.SCIM.Server.Exceptions;
using Newtonsoft.Json.Linq;
using System.Configuration;
using log4net;
using SfdcOPPConn.Models;
using SfdcOPPConn.Exceptions;

namespace SfdcOPPConn.Connectors
{
   internal class SfdcProvHelper
    {
        static Random myRandom = new Random();
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        internal  string getSfdcExtensionData(SCIMUser user, string key)
        {
          //  logger.Debug(" enter getSfdcExtensionData ");
            string value = String.Empty;

            var appSettings = ConfigurationManager.AppSettings;
            string urn = appSettings["sfdc.custom_extension_urn"];

            if (user.ExtensionData != null && user.ExtensionData.ContainsKey(urn))
            {
                IDictionary<string, JToken> data = (JObject)user.ExtensionData[urn];
                if (data.ContainsKey(key))
                {
                    value = data[key].ToString();
                }
            }
            logger.Debug(" ExtensionData key " + key + ":" + value);
            return value;
        }
        //
        internal object getJSONLookupData(string fileName, string searchField, object returnObject)
        {

            System.Type type = returnObject.GetType();

            return returnObject;
        }
        
        internal string getOktaInternalId(SCIMUser user)
        {
            //use oktaAPI call to get internal okta id for current user
            string myUsername = getSfdcExtensionData(user, "alt_username");
            string rOktaId = OktaAPIRequest.GetOktaId(myUsername);
            string rUpperOktaId = rOktaId.ToUpper();
            logger.Debug("Okta Internal Id = " + rUpperOktaId);
            return rUpperOktaId;
        }
        //
        internal string setPassword()
        {
            int myInt = myRandom.Next();
            string rPassword = myInt.ToString();
            logger.Debug("password = " + rPassword);
            return rPassword;
        }
        //
        internal string getStatus(SCIMUser user)
        {
            //default to Active
            //create/update => Active
            //Deactivate => Terminated
            string   rStatus = "Active";
            logger.Debug("status = " + rStatus);
            return  rStatus;
        }
        //
        internal bool getTimezone(SabaUserBasic sabaUserBasic, SabaCompoundNameObject timezoneId)
        {

            var appSettings = ConfigurationManager.AppSettings;
            //lookup shortname in affiliate json
            // read in json file
            string TimeZoneJsonFile = appSettings["ppfa.sabaTimezone_json"];
            string path = AppDomain.CurrentDomain.GetData("DataDirectory").ToString();
            string fullPath = path + "\\" + TimeZoneJsonFile;
            JArray jsonInput = new JArray();
            try
            {
                jsonInput = JArray.Parse(System.IO.File.ReadAllText(fullPath));
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                throw new OPPJSONException(TimeZoneJsonFile + ex.Message, ex);
                //return false;
 
            }

            // lookup affiliate properties
            var zipQuery = (from p in jsonInput
                            where p.SelectToken("zipcode").Value<string>() == sabaUserBasic.zip
                            select new
                            {
                                Id = (string)p["sabatimezoneId"],
                                DisplayName = (string)p["sabatimezoneDisplayName"]
                            }
            );

            try
            {
                // set affiliate object
                timezoneId.id = zipQuery.First().Id;
                timezoneId.displayName = zipQuery.First().DisplayName;
                logger.Debug("timezoneId.Id = " + timezoneId.id);
            }
            catch (Exception ex)
            {
                logger.Debug("timezoneId not retrieved");
                throw new OPPTimezoneNotFoundException(ex.Message, ex);
                //  return false;
            }

            return true ;
        }
        //
        internal string getHireDate(SCIMUser user)
        {
            string rHireDate = getSfdcExtensionData(user, "hireDate");

            if (string.IsNullOrEmpty(rHireDate))
            {
                rHireDate = getSfdcExtensionData(user, "AD_Create_Date");
                if (string.IsNullOrEmpty(rHireDate))
                {
                    rHireDate = DateTime.Now.ToString("yyyy-MM-dd");
                }
            }
            logger.Debug("hireDate = " + rHireDate);
            return rHireDate;
        }
        //need to calculate termination date
        internal string getTerminationDate()
        {
            string  rTerminationDate  = "";
            //rTerminationDate= DateTime.Now.ToShortDateString();   
            rTerminationDate = DateTime.Now.ToString("yyyy-MM-dd");
            logger.Debug("termination date = " + rTerminationDate);
            return rTerminationDate;
        }

        //need to fill out this
       //can be set to null
        internal bool getSecurityRoles(SabaUserBasic sabaUserBasic, List<SabaSecurityAssignment> createSecurityRoles)
        {
            var appSettings = ConfigurationManager.AppSettings;
            //lookup shortname in affiliate json
            // read in json file
            string AffiliateJsonFile = appSettings["ppfa.affiliate_json"];
            string path = AppDomain.CurrentDomain.GetData("DataDirectory").ToString();
            string fullPath = path + "\\" + AffiliateJsonFile;
            JArray jsonInput = new JArray();
            try
            {
                jsonInput = JArray.Parse(System.IO.File.ReadAllText(fullPath));
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }

            // lookup affiliate properties
            var affQuery = (from p in jsonInput
                            where p.SelectToken("affiliateCode").Value<string>() == sabaUserBasic.affiliateCode
                            select new
                            {
                                shortName = (string)p["affiliateShortname"]
                                //securityDomainId = (string)p["securityDomainId"]
                            }
            );
            // set affiliate object
            string myShortName = affQuery.First().shortName;


            createSecurityRoles.Count();
            createSecurityRoles[0].name = appSettings["saba.security_role1"];
            createSecurityRoles[0].securityDomain.displayName = appSettings["saba.domainGC"];
            createSecurityRoles[1].name = appSettings["saba.security_role1"];
            createSecurityRoles[1].securityDomain.displayName = myShortName;
            createSecurityRoles[2].name = appSettings["saba.security_role2"];
            createSecurityRoles[2].securityDomain.displayName = myShortName;
            createSecurityRoles[3].name = appSettings["saba.security_role3"];
            createSecurityRoles[3].securityDomain.displayName = appSettings["saba.domainWorld"];
            createSecurityRoles[4].name = appSettings["saba.security_role4"];
            createSecurityRoles[4].securityDomain.displayName = appSettings["saba.domainWorld"];

            logger.Debug("SecurityRoles = " + createSecurityRoles.Count().ToString());
            return  true;
        }
        //
        internal bool getSecurityDomain(SabaUserBasic sabaUserBasic, SabaCompoundNameObject securityDomain)
        {
            logger.Debug(" enter getSecurityDomain ");
          //  string myAffCode = getSabaExtensionData(user, "affiliate_code");

            var appSettings = ConfigurationManager.AppSettings;
            //lookup shortname in affiliate json
            // read in json file
            string AffiliateJsonFile = appSettings["ppfa.affiliate_json"];
            string path = AppDomain.CurrentDomain.GetData("DataDirectory").ToString();
            string fullPath = path + "\\" + AffiliateJsonFile;
            JArray jsonInput = new JArray();
            try
            {
                jsonInput = JArray.Parse(System.IO.File.ReadAllText(fullPath));
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }

            // lookup affiliate properties
            var affQuery = (from p in jsonInput
                            where p.SelectToken("affiliateCode").Value<string>() == sabaUserBasic.affiliateCode
                            select new
                            {
                                securityDomainDisplayName = (string)p["affiliateShortname"]
                                //securityDomainId = (string)p["securityDomainId"]
                            }
            );

            // set affiliate object
            securityDomain.displayName = affQuery.First().securityDomainDisplayName;
         //   securityDomain.id = affQuery.First().securityDomainId;

            logger.Debug("Security Domain = " + securityDomain.displayName);
            return true; 

        }

        internal string getPrimaryEmail(SCIMUser user)
        {

            try
            {
                string rEmail = null;
                Email primaryEmail = user.emails.SingleOrDefault<Email>(e => e.type == "primary");
                if (primaryEmail != null)
                {
                    rEmail = primaryEmail.value;
                }
                logger.Debug("primary email = " + rEmail);
                return rEmail;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        internal string getSecondaryEmail(SCIMUser user)
        {

            try
            {
                string rEmail = null;
                Email secondaryEmail = user.emails.SingleOrDefault<Email>(e => e.type == "secondary");
                if (secondaryEmail != null)
                {
                    rEmail = secondaryEmail.value;
                }
                logger.Debug("secondary email = " + rEmail);
                return rEmail;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
       //
        internal string getMobilePhone(SCIMUser user)
        {

            try
            {
                string rMobile = null;
                PhoneNumber mobilePhone = user.phoneNumbers.SingleOrDefault<PhoneNumber>(e => e.type == "mobile");
                if (mobilePhone != null)
                {
                    rMobile = mobilePhone.value;
                }
                logger.Debug("mobile phone = " + rMobile);
                return rMobile;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //
        internal string getPrimaryPhone(SCIMUser user)
        {

            try
            {
                string rPrimary = null;
                PhoneNumber mobilePhone = user.phoneNumbers.SingleOrDefault<PhoneNumber>(e => e.type == "primary");
                if (mobilePhone != null)
                {
                    rPrimary = mobilePhone.value;
                }
                logger.Debug("primary phone = " + rPrimary);
                return rPrimary;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


//
        internal bool CheckAffiliateCode(SCIMUser user)
        {


            string myAffCode = getSfdcExtensionData(user, "affiliate_code");
            var appSettings = ConfigurationManager.AppSettings;
            //lookup shortname in affiliate json
            // read in json file
            string AffiliateJsonFile = appSettings["ppfa.affiliate_json"];
            string path = AppDomain.CurrentDomain.GetData("DataDirectory").ToString();
            string fullPath = path + "\\" + AffiliateJsonFile;
            JArray jsonInput = new JArray();
            try
            {
                jsonInput = JArray.Parse(System.IO.File.ReadAllText(fullPath));
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                throw new OPPJSONException(AffiliateJsonFile + ex.Message, ex);
                //return false;
            }

            // lookup affiliate properties
            var affQuery = (from p in jsonInput
                            where p.SelectToken("affiliateCode").Value<string>() == myAffCode
                            select new
                            {
                                Name = (string)p["name"]
                            }
            );
            try
            {
                // set affiliate object
                string rAffiliateName = affQuery.First().Name;
                logger.Debug("AffiliateCode  found ");
            }
            catch (Exception ex)
            {
                logger.Debug("AffiliateCode not retrieved");
                throw new OPPAffiliateNotFoundException(ex.Message, ex);
                  //  return false;
            }

            return true; 
        }
        //convert user models
        internal SabaUserCreate SabaUserBasicToSabaUserCreate(SabaUserBasic sabaUserBasic)
        {
            logger.Debug("ENTER SabaUserBasicToSabaUserCreate ");
            //move this to class attribute
            var appSettings = ConfigurationManager.AppSettings;
            try
            {
                //SabaUserBasic sabaUserBasic = new SabaUserBasic();
                //      SabaUserCreate sabaUserCreate = new SabaUserCreate();

                //include in body
                //removed ID field ref note from Phineas
                SabaCompoundNameObject createLocationId = new SabaCompoundNameObject();
                createLocationId.displayName = sabaUserBasic.location;
                //createLocationId.id = sabaUserBasic.location;
                SabaCompoundNameObject createManagerId = new SabaCompoundNameObject();
                createManagerId.displayName = sabaUserBasic.manager;
                //createManagerId.id = sabaUserBasic.manager;
                //get from config file
                SabaCompoundNameObject createLocaleId = new SabaCompoundNameObject();
                createLocaleId.displayName = appSettings["saba.locale"];
                //createLocaleId.id = appSettings["saba.localeId"];
                //lookup timezone in json file
                SabaCompoundNameObject createTimezoneId = new SabaCompoundNameObject();
                getTimezone(sabaUserBasic, createTimezoneId);
                //not mentioned in design doc
                SabaCompoundNameObject createHomeDomain = new SabaCompoundNameObject();
                createHomeDomain.displayName = appSettings["saba.domainGC"];
                //createHomeDomain.id = "domin000000000036429";
                //not mentioned in design doc
                //SabaCompoundNameObject createHomeCompanyId = new SabaCompoundNameObject();
                //createHomeCompanyId.displayName = "ARMS [Affiliates Risk Management Services]";
                //createHomeCompanyId.id = "bisut000000000001000";
                //removed ID field ref note from Phineas
                SabaCompoundNameObject createCompanyId = new SabaCompoundNameObject();
                createCompanyId.displayName = sabaUserBasic.company;
                //createCompanyId.id = "bisut000000000002654";
                //get from config file
                //removed ID field ref note from Phineas
                SabaCompoundNameObject createJobtypeId = new SabaCompoundNameObject();
                createJobtypeId.displayName = appSettings["saba.job_type"];
                //createJobtypeId.id = appSettings["saba.jobTypeId"]; 
                //need clarification
                SabaCompoundNameObject createSecurityDomain = new SabaCompoundNameObject();
                getSecurityDomain(sabaUserBasic, createSecurityDomain);
                //create security roles; add 5 security assignments
               // SabaSetSecurityRoles createSecurityRoles = new SabaSetSecurityRoles();
                SabaSecurityAssignment sabaSecurityAssignment01 = new SabaSecurityAssignment();
                sabaSecurityAssignment01.securityDomain = new SabaDisplayNameObject();
                SabaSecurityAssignment sabaSecurityAssignment02 = new SabaSecurityAssignment();
                sabaSecurityAssignment02.securityDomain = new SabaDisplayNameObject();
                SabaSecurityAssignment sabaSecurityAssignment03 = new SabaSecurityAssignment();
                sabaSecurityAssignment03.securityDomain = new SabaDisplayNameObject();
                SabaSecurityAssignment sabaSecurityAssignment04 = new SabaSecurityAssignment();
                sabaSecurityAssignment04.securityDomain = new SabaDisplayNameObject();
                SabaSecurityAssignment sabaSecurityAssignment05 = new SabaSecurityAssignment();
                sabaSecurityAssignment05.securityDomain = new SabaDisplayNameObject();
                List<SabaSecurityAssignment> createSecurityRoles = new List<SabaSecurityAssignment>();
                createSecurityRoles.Add(sabaSecurityAssignment01);
                createSecurityRoles.Add(sabaSecurityAssignment02);
                createSecurityRoles.Add(sabaSecurityAssignment03);
                createSecurityRoles.Add(sabaSecurityAssignment04);
                createSecurityRoles.Add(sabaSecurityAssignment05);
                getSecurityRoles(sabaUserBasic,createSecurityRoles);

                //main body
                SabaUserCreate sabaUserCreate = new SabaUserCreate();
                sabaUserCreate.username = sabaUserBasic.username;
                sabaUserCreate.fname = sabaUserBasic.fname;
                sabaUserCreate.lname = sabaUserBasic.lname;
                sabaUserCreate.email = sabaUserBasic.email;
                sabaUserCreate.person_no = sabaUserBasic.person_no;
                //set in sabaUserBasic based on scim user; hardcode
                sabaUserCreate.status = "Active";
                //sabaUserCreate.password = sabaUserBasic.password;
                sabaUserCreate.password = setPassword();
                //createNewUser.suffix = "emSuffix";
                //createNewUser.title = "Mr.";
                //createNewUser.state = "em1state1111";
                //createNewUser.country = "em1country2222";
                //createNewUser.city = "em1cityllllllll";
                sabaUserCreate.zip = sabaUserBasic.zip;
                sabaUserCreate.gender = appSettings["saba.gender"];
                //createNewUser.homephone = "";
                sabaUserCreate.workphone = sabaUserBasic.workphone;
                //createNewUser.fax = "";
                sabaUserCreate.is_manager = sabaUserBasic.is_manager;
                //no direction on this; hardcode
                sabaUserCreate.password_changed = "false";
                //createNewUser.ss_no = "111-11-1116";
                //createNewUser.job_title = "emJobTitle";
                //set to blank for create
                //sabaUserCreate.terminated_on = getTerminationDate();
            //mapped from user.user_job_type, need default value
            //removed due to errors
            sabaUserCreate.person_type = sabaUserBasic.person_type;
                //createNewUser.mname = "";
                sabaUserCreate.started_on = sabaUserBasic.hired_on;
                //createNewUser.job_title_type = "";
                //createNewUser.religion = "Hinduism";
                //createNewUser.ethnicity = "Hispanic Origin";
                //createNewUser.date_of_birth = "1975-03-25";
                // createNewUser.desired_job_type_id = "";
                //createNewUser.special_user = "false";
                //createNewUser.addr1 = "erge";
                //createNewUser.addr2 = "ergeg";
                //createNewUser.addr3 = "addr3333";
                //createNewUser.correspondence_preference1 = "1";
                //createNewUser.correspondence_preference2 = "0";
                //createNewUser.correspondence_preference3 = "1";
                sabaUserCreate.alternate_manager = sabaUserBasic.alternate_manager;
                //include complex types
                sabaUserCreate.securityDomain = createSecurityDomain;
                sabaUserCreate.jobtype_id = createJobtypeId;
                sabaUserCreate.company_id = createCompanyId;
                //sabaUserCreate.home_company_id = createHomeCompanyId;
                sabaUserCreate.home_domain = createHomeDomain;
                sabaUserCreate.timezone_id = createTimezoneId;
                sabaUserCreate.locale_id = createLocaleId;
                sabaUserCreate.location_id = createLocationId;
                sabaUserCreate.manager_id = createManagerId;
                sabaUserCreate.securityRoles = createSecurityRoles;


                return sabaUserCreate;
            }
            catch (Exception e)
            {
                logger.Error(e);
                throw;
            }

        }

        //convert user models
        internal SabaUserUpdate SabaUserBasicToSabaUserUpdate(SabaUserBasic sabaUserBasic)
        {
            logger.Debug("ENTER SabaUserBasicToSabaUserUpdate ");
            //move this to class attribute
            var appSettings = ConfigurationManager.AppSettings;
            try
            {
                //SabaUserBasic sabaUserBasic = new SabaUserBasic();
                //      SabaUserCreate sabaUserCreate = new SabaUserCreate();

                //include in body
                //removed ID field ref note from Phineas
                SabaCompoundNameObject createLocationId = new SabaCompoundNameObject();
                createLocationId.displayName = sabaUserBasic.location;
                //createLocationId.id = sabaUserBasic.location;
                SabaCompoundNameObject createManagerId = new SabaCompoundNameObject();
                createManagerId.displayName = sabaUserBasic.manager;
                //createManagerId.id = sabaUserBasic.manager;
                //get from config file
                SabaCompoundNameObject createLocaleId = new SabaCompoundNameObject();
                createLocaleId.displayName = appSettings["saba.locale"];
                //createLocaleId.id = appSettings["saba.localeId"];
                //lookup timezone in json file
                SabaCompoundNameObject createTimezoneId = new SabaCompoundNameObject();
                getTimezone(sabaUserBasic, createTimezoneId);
                //not mentioned in design doc
                SabaCompoundNameObject createHomeDomain = new SabaCompoundNameObject();
                createHomeDomain.displayName = appSettings["saba.domainGC"];
                //createHomeDomain.id = "domin000000000036429";
                //not mentioned in design doc
                //SabaCompoundNameObject createHomeCompanyId = new SabaCompoundNameObject();
                //createHomeCompanyId.displayName = "ARMS [Affiliates Risk Management Services]";
                //createHomeCompanyId.id = "bisut000000000001000";
                //removed ID field ref note from Phineas
                SabaCompoundNameObject createCompanyId = new SabaCompoundNameObject();
                createCompanyId.displayName = sabaUserBasic.company;
                //createCompanyId.id = "bisut000000000002654";
                //get from config file
                //removed ID field ref note from Phineas
                SabaCompoundNameObject createJobtypeId = new SabaCompoundNameObject();
                createJobtypeId.displayName = appSettings["saba.job_type"];
                //createJobtypeId.id = appSettings["saba.jobTypeId"]; 
                //need clarification
                SabaCompoundNameObject createSecurityDomain = new SabaCompoundNameObject();
                getSecurityDomain(sabaUserBasic, createSecurityDomain);
                //create security roles; add 5 security assignments
                // SabaSetSecurityRoles createSecurityRoles = new SabaSetSecurityRoles();
                SabaSecurityAssignment sabaSecurityAssignment01 = new SabaSecurityAssignment();
                sabaSecurityAssignment01.securityDomain = new SabaDisplayNameObject();
                SabaSecurityAssignment sabaSecurityAssignment02 = new SabaSecurityAssignment();
                sabaSecurityAssignment02.securityDomain = new SabaDisplayNameObject();
                SabaSecurityAssignment sabaSecurityAssignment03 = new SabaSecurityAssignment();
                sabaSecurityAssignment03.securityDomain = new SabaDisplayNameObject();
                SabaSecurityAssignment sabaSecurityAssignment04 = new SabaSecurityAssignment();
                sabaSecurityAssignment04.securityDomain = new SabaDisplayNameObject();
                SabaSecurityAssignment sabaSecurityAssignment05 = new SabaSecurityAssignment();
                sabaSecurityAssignment05.securityDomain = new SabaDisplayNameObject();
                List<SabaSecurityAssignment> createSecurityRoles = new List<SabaSecurityAssignment>();
                createSecurityRoles.Add(sabaSecurityAssignment01);
                createSecurityRoles.Add(sabaSecurityAssignment02);
                createSecurityRoles.Add(sabaSecurityAssignment03);
                createSecurityRoles.Add(sabaSecurityAssignment04);
                createSecurityRoles.Add(sabaSecurityAssignment05);
                getSecurityRoles(sabaUserBasic, createSecurityRoles);

                //main body
                SabaUserUpdate sabaUserUpdate = new SabaUserUpdate();
                sabaUserUpdate.username = sabaUserBasic.username;
                sabaUserUpdate.fname = sabaUserBasic.fname;
                sabaUserUpdate.lname = sabaUserBasic.lname;
                sabaUserUpdate.email = sabaUserBasic.email;
                sabaUserUpdate.person_no = sabaUserBasic.person_no;
                //set in sabaUserBasic based on scim user; hardcode
                sabaUserUpdate.status = "Active";
                //sabaUserCreate.password = sabaUserBasic.password;
                //sabaUserCreate.password = setPassword();
                //createNewUser.suffix = "emSuffix";
                //createNewUser.title = "Mr.";
                //createNewUser.state = "em1state1111";
                //createNewUser.country = "em1country2222";
                //createNewUser.city = "em1cityllllllll";
                sabaUserUpdate.zip = sabaUserBasic.zip;
                sabaUserUpdate.gender = appSettings["saba.gender"];
                //createNewUser.homephone = "";
                sabaUserUpdate.workphone = sabaUserBasic.workphone;
                //createNewUser.fax = "";
                sabaUserUpdate.is_manager = sabaUserBasic.is_manager;
                //no direction on this; hardcode
                //sabaUserCreate.password_changed = "false"
                //createNewUser.ss_no = "111-11-1116";
                //createNewUser.job_title = "emJobTitle";
                //set to blank for create
                //sabaUserCreate.terminated_on = getTerminationDate();
            //mapped from user.user_job_type, need default value
            //removed due to errors
                sabaUserUpdate.person_type = sabaUserBasic.person_type;
                //createNewUser.mname = "";
                //sabaUserCreate.started_on = sabaUserBasic.hired_on;
                //createNewUser.job_title_type = "";
                //createNewUser.religion = "Hinduism";
                //createNewUser.ethnicity = "Hispanic Origin";
                //createNewUser.date_of_birth = "1975-03-25";
                // createNewUser.desired_job_type_id = "";
                //createNewUser.special_user = "false";
                //createNewUser.addr1 = "erge";
                //createNewUser.addr2 = "ergeg";
                //createNewUser.addr3 = "addr3333";
                //createNewUser.correspondence_preference1 = "1";
                //createNewUser.correspondence_preference2 = "0";
                //createNewUser.correspondence_preference3 = "1";
                sabaUserUpdate.alternate_manager = sabaUserBasic.alternate_manager;
                //include complex types
                sabaUserUpdate.securityDomain = createSecurityDomain;
                sabaUserUpdate.jobtype_id = createJobtypeId;
                sabaUserUpdate.company_id = createCompanyId;
                //sabaUserCreate.home_company_id = createHomeCompanyId;
                sabaUserUpdate.home_domain = createHomeDomain;
                sabaUserUpdate.timezone_id = createTimezoneId;
                sabaUserUpdate.locale_id = createLocaleId;
                sabaUserUpdate.location_id = createLocationId;
                sabaUserUpdate.manager_id = createManagerId;
                sabaUserUpdate.securityRoles = createSecurityRoles;


                return sabaUserUpdate;
            }
            catch (Exception e)
            {
                logger.Error(e);
                throw;
            }

        }
    
   public SfdcUserUpdate SfdcUserBasicToSfdcUserUpdate(SfdcUserBasic sfdcUserBasic)
        {
            logger.Debug("SfdcUserBasicTo.TCSfdcUserUpdate " + sfdcUserBasic.Username);

            //       sfdcUserUpdate.Address = new Models.Address();
            //sfdcUserUpdate.Name = sfdcUserBasic.Firstname + " " + sfdcUserBasic.Lastname;

            //  sfdcUserUpdate.MobilePhone = sfdcUserBasic.MobilePhone;
            //      sfdcUserUpdate.Address.city = sfdcUserBasic.City;


            SfdcUserUpdate sfdcUserUpdate = new SfdcUserUpdate();

            sfdcUserUpdate.FirstName = sfdcUserBasic.Firstname;
            sfdcUserUpdate.LastName  = sfdcUserBasic.Lastname;
            sfdcUserUpdate.Email = sfdcUserBasic.PrimaryEmail;
            sfdcUserUpdate.Phone = sfdcUserBasic.PrimaryPhone;
            sfdcUserUpdate.Street = sfdcUserBasic.Street;
            sfdcUserUpdate.City = sfdcUserBasic.City;
            sfdcUserUpdate.State = sfdcUserBasic.State;
            sfdcUserUpdate.PostalCode = sfdcUserBasic.PostalCode;
            sfdcUserUpdate.Country = sfdcUserBasic.Country;
            sfdcUserUpdate.Company_Name__c = sfdcUserBasic.Organization;
            sfdcUserUpdate.Secondary_Email__c = sfdcUserBasic.SecondaryEmail;
            sfdcUserUpdate.Clinician__c = sfdcUserBasic.is_clinician;
            sfdcUserUpdate.Profession__c = sfdcUserBasic.profession;
            sfdcUserUpdate.Specialty__c = sfdcUserBasic.specialty;
            sfdcUserUpdate.Years_Practicing__c = sfdcUserBasic.years_practicing;
            sfdcUserUpdate.TCTMD_Newsletter__c = sfdcUserBasic.tctmd_newsletter;
            sfdcUserUpdate.CRF_Conference_News__c = sfdcUserBasic.crf_newsletter;
            sfdcUserUpdate.TCT_Newsletter__c = sfdcUserBasic.tct_newsletter;
       //recent add
            sfdcUserUpdate.TCTMD_Subscription_ID__c = sfdcUserBasic.tctmdSubscriptionLevel;
            sfdcUserUpdate.Okta_Id__c = sfdcUserBasic.OktaInternalId;

            return sfdcUserUpdate;

        }
   
   
   }
}