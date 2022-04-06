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
using System.Configuration;

namespace SfdcOPPConn.Connectors
{
    public class SfdcProvConnector : ISCIMConnector
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
       
        //public enum localStatus
        //{
        //    ByUsername,
        //    ByPersonNo,
        //    Missing,
        //    Error,
        //    Normal
        //}

        //public enum retryStatus
        //{
        //    retryInitial,
        //    retryStart,
        //    retryterminate
        //}

      //  public SabaContext sabaContext { get; set; }
        public SfdcHttpClient baseClient;
        public SfdcApiRequest sfdcAPIRequest;
        internal SfdcProvHelper helper = new SfdcProvHelper();


        public SfdcProvConnector()
        {

     //       var appSettings = ConfigurationManager.AppSettings;
          //  logger.Debug(" enter constructor SabaProvConnector context may have cert " + context.sabaCert);
            //check if cert is expired  ( 24 hour time limit)
            //if (string.IsNullOrEmpty(context.sabaCert))
            //{
            //    logger.Debug(" cert is empty");




            //sabaContext = new SabaContext(appSettings["saba.username"],
            //                           appSettings["saba.password"],
            //                           appSettings["saba.baseURL"],
            //                           appSettings["saba.APIversion"]
            //                           );

            baseClient = new SfdcHttpClient();
            sfdcAPIRequest = new SfdcApiRequest();
         //   sabaAPIRequest = new SabaApiRequest(baseClient);

         //   SabaApiRequest.CreateSessionAPI(context);


            
            
        }


        private SfdcUserBasic SCIMUserToSfdcUserBasic(SCIMUser scimUser)
        {
            logger.Debug("ENTER SCIMUserToSfdcUserBasic ");

            //   sfdcUserBasic.MobilePhone = helper.getMobilePhone(scimUser);

            SfdcUserBasic sfdcUserBasic = new SfdcUserBasic();

            sfdcUserBasic.Username = scimUser.userName;
            logger.Debug("userName " + scimUser.userName);
            sfdcUserBasic.Firstname = scimUser.name.givenName;
            logger.Debug("firstName " + scimUser.name.givenName);
            sfdcUserBasic.Lastname = scimUser.name.familyName;
            logger.Debug("lastName " + scimUser.name.familyName);
            sfdcUserBasic.Id = scimUser.id;
            logger.Debug("Id " + scimUser.id);
            
            sfdcUserBasic.PrimaryEmail = helper.getPrimaryEmail(scimUser);
            sfdcUserBasic.PrimaryPhone = helper.getSfdcExtensionData(scimUser, "primaryPhone");
            sfdcUserBasic.Street = helper.getSfdcExtensionData(scimUser, "street");
            sfdcUserBasic.City = helper.getSfdcExtensionData(scimUser, "city");
            sfdcUserBasic.State = helper.getSfdcExtensionData(scimUser, "state");
            sfdcUserBasic.PostalCode = helper.getSfdcExtensionData(scimUser, "zip");
            sfdcUserBasic.Country = helper.getSfdcExtensionData(scimUser, "country");
            sfdcUserBasic.Organization = helper.getSfdcExtensionData(scimUser, "organization");
            sfdcUserBasic.SecondaryEmail = helper.getSecondaryEmail(scimUser);
            sfdcUserBasic.is_clinician = helper.getSfdcExtensionData(scimUser, "is_clinician");
            sfdcUserBasic.profession = helper.getSfdcExtensionData(scimUser, "profession");
            sfdcUserBasic.specialty = helper.getSfdcExtensionData(scimUser, "specialty");
            sfdcUserBasic.years_practicing = helper.getSfdcExtensionData(scimUser, "years_practicing");
            sfdcUserBasic.tctmd_newsletter = helper.getSfdcExtensionData(scimUser, "tctmd_newsletter");
            sfdcUserBasic.crf_newsletter = helper.getSfdcExtensionData(scimUser, "crf_newsletter");
            sfdcUserBasic.tct_newsletter = helper.getSfdcExtensionData(scimUser, "tct_newsletter");
            //recent add
            sfdcUserBasic.tctmdSubscriptionLevel = helper.getSfdcExtensionData(scimUser, "tctmdSubscriptionLevel");
            sfdcUserBasic.OktaInternalId = helper.getOktaInternalId(scimUser);

            return sfdcUserBasic;
        }

        private SCIMUser SfdcUserBasicToSCIMUser(SfdcUserBasic sfdcUserBasic)
        {
            logger.Debug("ENTER SfdcUserBasicToSCIMUser ");
            SCIMUser scimUser = new SCIMUser();

  
            try
            {
                
                scimUser.userName = sfdcUserBasic.Username;
                scimUser.externalId = sfdcUserBasic.Id;
                scimUser.id = sfdcUserBasic.Id;
                //
                if (sfdcUserBasic.Lastname != null)
                {
                    if (scimUser.name == null)
                    {
                        scimUser.name = new Name();
                    }
                    scimUser.name.familyName = sfdcUserBasic.Lastname;
                }
                //
                if (sfdcUserBasic.Firstname != null)
                {
                    //  scimUser.name = new Name();
                    scimUser.name.givenName = sfdcUserBasic.Firstname;
                }
                if (sfdcUserBasic.PrimaryEmail != null)
                {
                    scimUser.emails = new List<Email>();
                    Email email = new Email();
                    email.primary = true;
                    email.type = "Work";
                    email.value = sfdcUserBasic.PrimaryEmail;
                    scimUser.emails.Add(email);
                }

            }
            catch (Exception ex)
            {
                logger.Error("ex " + ex.ToString());
                throw;
            }



            return scimUser;
        }

        //convert user models
       // private SabaUserBasic SCIMUserToSabaUserBasic(SCIMUser scimUser)
       // {
       //     logger.Debug("ENTER SCIMUserToSabaUser ");
       //     //move this to class attribute
       //     var appSettings = ConfigurationManager.AppSettings;
       //     try
       //     {
       //         SabaUserBasic sabaUserBasic = new SabaUserBasic();

       //         sabaUserBasic.person_no = helper.getOktaInternalId(scimUser);  //okta internal id
       //         sabaUserBasic.username = scimUser.userName;
       //         sabaUserBasic.fname = scimUser.name.givenName;
       //         sabaUserBasic.lname = scimUser.name.familyName;
       //       //  sabaUserBasic.gender = appSettings["saba.gender"];
       //         sabaUserBasic.email = helper.getPrimaryEmail(scimUser);
       //        // sabaUserBasic.password = helper.setPassword(scimUser);
       //        // sabaUserBasic.status = helper.getStatus(scimUser);
       //         sabaUserBasic.zip = helper.getSfdcExtensionData(scimUser, "zipcode");
       //         sabaUserBasic.company = helper.getSfdcExtensionData(scimUser, "affiliate_code");
       //         sabaUserBasic.manager = helper.getSfdcExtensionData(scimUser, "manager_email");
       //         sabaUserBasic.person_type = helper.getSfdcExtensionData(scimUser, "user_job_type");
       //         sabaUserBasic.ad_create_date = helper.getSfdcExtensionData(scimUser, "AD_Create_Date");
       //         sabaUserBasic.hireDate = helper.getSfdcExtensionData(scimUser, "hireDate");
       //         sabaUserBasic.hired_on = helper.getHireDate(scimUser);
       //         sabaUserBasic.terminated_on = helper.getTerminationDate();
       //         sabaUserBasic.location = helper.getSfdcExtensionData(scimUser, "User_Facility_ID");
       //        // sabaUserBasic.locale = appSettings["saba.locale"];
       //         sabaUserBasic.is_manager = helper.getSfdcExtensionData(scimUser, "Is_User_A_Manager");
       //         sabaUserBasic.alternate_manager = helper.getSfdcExtensionData(scimUser, "CAL_Manager_Email");
       //         sabaUserBasic.job_type = appSettings["saba.job_type"];
       //         sabaUserBasic.workphone = helper.getSfdcExtensionData(scimUser, "primaryphone");
       //         //sabaUserBasic.securityroles = helper.getSecurityRoles(scimUser);
       //         sabaUserBasic.affiliateCode = helper.getSfdcExtensionData(scimUser, "affiliate_code");
       //         //allow for initial retry if location value is reject by Saba API
       //         sabaUserBasic.locationRetry = retryStatus.retryInitial;
       //         if (string.IsNullOrEmpty(sabaUserBasic.manager))
       //         {
       //             logger.Error(" error manager attribute is null or empty ");
       //             SfdcAuditLog.SabaAudit(sabaUserBasic, "Error manager attribute null or empty", "not processed");
       //             throw new SabaManagerIdErrorException();
       //         }

       ////         logger.Info("exit SCIMUserToSabaUser");

       //         return sabaUserBasic;
       //     }
       //     catch (SfdcException e)
       //     {
       //         logger.Error(e);
       //         throw e;
       //     }

       // }
        ////convert user models
        //private SabaUserCreate SabaUserBasicToSabaUserCreate(SabaUserBasic sabaUserBasic)
        //{
        //    logger.Debug("ENTER SabaUserBasicToSabaUserCreate ");
        //    //move this to class attribute
        //    var appSettings = ConfigurationManager.AppSettings;
        //    try
        //    {
        //        //SabaUserBasic sabaUserBasic = new SabaUserBasic();
        //  //      SabaUserCreate sabaUserCreate = new SabaUserCreate();

        //        //include in body
        //        //removed ID field ref note from Phineas
        //        SabaLocationId createLocationId = new SabaLocationId();
        //        createLocationId.displayName = sabaUserBasic.location;
        //       // createLocationId.id = "locat000000000001000";
        //        SabaManagerId createManagerId = new SabaManagerId();
        //        createManagerId.displayName = sabaUserBasic.manager;
        //        //createManagerId.id = sabaUserBasic.manager;
        //        //get from config file
        //        SabaLocaleId createLocaleId = new SabaLocaleId();
        //        createLocaleId.displayName = appSettings["saba.localeDisplayName"];
        //        createLocaleId.id = appSettings["saba.localeId"];
        //        //lookup timezone in json file
        //        SabaTimezoneId createTimezoneId = new SabaTimezoneId();
        //        helper.getTimezone(sabaUserBasic, createTimezoneId);
        //        //not mentioned in design doc
        //        SabaHomeDomain createHomeDomain = new SabaHomeDomain();
        //        createHomeDomain.displayName = appSettings["saba.domainGC"];
        //        //createHomeDomain.id = "domin000000000036429";
        //        //not mentioned in design doc
        //        SabaHomeCompanyId createHomeCompanyId = new SabaHomeCompanyId();
        //        createHomeCompanyId.displayName = "ARMS [Affiliates Risk Management Services]";
        //        createHomeCompanyId.id = "bisut000000000001000";
        //        //removed ID field ref note from Phineas
        //        SabaCompanyId createCompanyId = new SabaCompanyId();
        //        createCompanyId.displayName = sabaUserBasic.company;
        //        //createCompanyId.id = "bisut000000000002654";
        //        //get from config file
        //        //removed ID field ref note from Phineas
        //        SabaJobtypeId createJobtypeId = new SabaJobtypeId();
        //        createJobtypeId.displayName = appSettings["saba.jobTypeDisplayName"]; 
        //        //createJobtypeId.id = appSettings["saba.jobTypeId"]; 
        //        //need clarification
        //        SabaSecurityDomain createSecurityDomain = new SabaSecurityDomain();
        //        helper.getSecurityDomain(sabaUserBasic, createSecurityDomain);

        //        //main body
        //        SabaUserCreate sabaUserCreate = new SabaUserCreate();
        //        sabaUserCreate.username = sabaUserBasic.username;
        //        sabaUserCreate.fname = sabaUserBasic.fname;
        //        sabaUserCreate.lname = sabaUserBasic.lname;
        //        sabaUserCreate.email = sabaUserBasic.email;
        //        sabaUserCreate.person_no = sabaUserBasic.person_no;
        //        //set in sabaUserBasic based on scim user; hardcode
        //        sabaUserCreate.status = "Active";
        //        sabaUserCreate.password = sabaUserBasic.password;
        //        //createNewUser.suffix = "emSuffix";
        //        //createNewUser.title = "Mr.";
        //        //createNewUser.state = "em1state1111";
        //        //createNewUser.country = "em1country2222";
        //        //createNewUser.city = "em1cityllllllll";
        //        sabaUserCreate.zip = sabaUserBasic.zip;
        //        sabaUserCreate.gender = appSettings["saba.gender"];
        //        //createNewUser.homephone = "";
        //        sabaUserCreate.workphone = sabaUserBasic.workphone;
        //        //createNewUser.fax = "";
        //        sabaUserCreate.is_manager = sabaUserBasic.is_manager;
        //        //no direction on this; hardcode
        //        sabaUserCreate.password_changed = "false";
        //        //need to HttpWorkerRequest on this
        //        //createNewUser.securityRoles = "";
        //        //createNewUser.ss_no = "111-11-1116";
        //        //createNewUser.job_title = "emJobTitle";
        //        //set to blank for create
        //        sabaUserCreate.terminated_on = "";//
        //        //mapped from user.user_job_type, need default value
        //        //removed due to errors
        //        //sabaUserCreate.person_type = sabaUserBasic.person_type;
        //        //createNewUser.mname = "";
        //        sabaUserCreate.started_on = sabaUserBasic.hired_on;
        //        //createNewUser.job_title_type = "";
        //        //createNewUser.religion = "Hinduism";
        //        //createNewUser.ethnicity = "Hispanic Origin";
        //        //createNewUser.date_of_birth = "1975-03-25";
        //        // createNewUser.desired_job_type_id = "";
        //        //createNewUser.special_user = "false";
        //        //createNewUser.addr1 = "erge";
        //        //createNewUser.addr2 = "ergeg";
        //        //createNewUser.addr3 = "addr3333";
        //        //createNewUser.correspondence_preference1 = "1";
        //        //createNewUser.correspondence_preference2 = "0";
        //        //createNewUser.correspondence_preference3 = "1";
        //        sabaUserCreate.alternate_manager = sabaUserBasic.alternate_manager;
        //        //include complex types
        //        sabaUserCreate.securityDomain = createSecurityDomain;
        //        //removed due to errors
        //        sabaUserCreate.jobtype_id = createJobtypeId;
        //        sabaUserCreate.company_id = createCompanyId;
        //        //sabaUserCreate.home_company_id = createHomeCompanyId;
        //        sabaUserCreate.home_domain = createHomeDomain;
        //        sabaUserCreate.timezone_id = createTimezoneId;
        //        sabaUserCreate.locale_id = createLocaleId;
        //        //removoed due to errors
        //        //need valid value to mapp in from user.user_facility_id
        //        //sabaUserCreate.location_id = createLocationId;
        //        sabaUserCreate.manager_id = createManagerId;






        //    return sabaUserCreate;
        //    }
        //    catch (Exception e)
        //    {
        //        logger.Error(e);
        //        throw;
        //    }

        //}
        //convert user models
        //private SCIMUser SabaUserBasicToSCIMUser(SabaUserBasic sabaUserBasic)
        //{
        //    logger.Debug("ENTER SabaUserToSCIMUser user " + sabaUserBasic.username);


        //    SCIMUser scimUser = new SCIMUser();

        //    scimUser.active = true;
        //    if (sabaUserBasic.username != null)
        //    {
        //        scimUser.userName = sabaUserBasic.username;
        //    }
        //    //
        //    if (sabaUserBasic.person_no != null)
        //    {
        //        scimUser.id = sabaUserBasic.person_no;
        //    }
        //    //
        //    if (sabaUserBasic.lname != null)
        //    {
        //        if (scimUser.name == null)
        //        {
        //             scimUser.name = new Name();
        //        }
        //        scimUser.name.familyName = sabaUserBasic.lname;
        //    }
        //    //
        //    if (sabaUserBasic.fname != null)
        //    {
        //        //  scimUser.name = new Name();
        //        scimUser.name.givenName = sabaUserBasic.fname;
        //    }

        //    //
        //    if (sabaUserBasic.email != null)
        //    {
        //        scimUser.emails = new List<Email>();
        //        Email email = new Email();
        //        email.primary = true;
        //        email.type = "Work";
        //        email.value = sabaUserBasic.email;
        //        scimUser.emails.Add(email);
        //    }
        //    //
        //    if (sabaUserBasic.password != null)
        //    {
        //        scimUser.password = sabaUserBasic.password;
        //    }
        //    //sabaUser.gender;
        //    //sabaUser.status;
        //    //sabaUser.timezone;
        //    //sabaUser.company;
        //    //sabaUser.manager;
        //    //sabaUser.person_type;
        //    //sabaUser.hired_on;
        //    //sabaUser.terminated_on;
        //    //sabaUser.location;
        //    //sabaUser.locale;
        //    //sabaUser.is_manager;
        //    //sabaUser.alternate_manager;
        //    //sabaUser.job_type;
        //    //sabaUser.workphone;
        //    //sabaUser.securityroles;
        //    //sabaUser.securitydomain;
        //    return scimUser;
        //}

       

        //called on POST request from controller
        public SCIMUser createUser(SCIMUser scimUser)
        {
            logger.Debug("createUser not handled userName " + scimUser.userName);
            throw new NotImplementedException();
           // System.Diagnostics.Debug.WriteLine(context.ToString());
           // localStatus sabaUserStatus;
           // localStatus compareUserStatus;
           // bool apiResults;
           //// string oktainternalid;
           // SabaUserBasic sabaUserBasic = new SabaUserBasic();
           // SabaUserBasic sabaUserBasicReturn = new SabaUserBasic();
           // SabaUserCreate sabaUserCreate = new SabaUserCreate();
           // SCIMUser scimUserReturn = new SCIMUser();



//            try
//            {
//                sabaUserBasic = SCIMUserToSabaUserBasic(scimUser);
//                //check is scimUser has affilaiteCode in alliliate.json
//                bool result = helper.CheckAffiliateCode(scimUser);
//                if (!result)
//                {
//                    logger.Debug("affiliateCode not in file");
//                    SabaAuditLog.SabaAudit(sabaUserCreate, sabaUserBasic, "affiliateCode not in file", "no action");
//                    return scimUserReturn;
//                }
//                sabaUserStatus = EvaluateSabaRecord(sabaUserBasic);
//                //compare scimUser to Sabauser as per design
//                compareUserStatus = CompareUserContext(sabaUserBasic, scimUser);
//                //implement action logic
//                switch (sabaUserStatus)
//                {
//                    case localStatus.ByPersonNo:
//                        //saba update user event
//                        logger.Debug("createUser found " + sabaUserStatus);
//                       // sabaUserCreate = SabaUserBasicToSabaUserCreate(sabaUserBasic);
//                       // sabaUserBasicReturn = sabaAPIRequest.UpdateUserAPI(sabaUserCreate, sabaUserBasic, context, baseClient);
//                        sabaUserBasicReturn = sabaAPIRequest.UpdateUserAPI(sabaUserBasic, baseClient);
//                        if (sabaUserBasicReturn.locationRetry == retryStatus.retryStart)
//                        {
//                            sabaUserBasicReturn.locationRetry = retryStatus.retryTerminate;
//                            logger.Debug("createUser Retry process ");
//                            sabaUserBasicReturn = sabaAPIRequest.UpdateUserAPI(sabaUserBasicReturn, baseClient);
//                            sabaUserBasic = sabaUserBasicReturn;
//                        }
//                        apiResults =     SetSabaManager(sabaUserBasic);
//                        break;
//                    case localStatus.ByUsername:
//                        //saba update user event
//                        logger.Debug("createUser found " + sabaUserStatus);
//                       // sabaUserCreate = SabaUserBasicToSabaUserCreate(sabaUserBasic);
//                        //since was found by username, need to update person_no = okta intenal id
//                        //sabaUserBasic aleady has the value just need to update
////ajc debug
//                  //      sabaUserBasicReturn = sabaAPIRequest.UpdateUserAPI(sabaUserCreate,sabaUserBasic, context, baseClient);
//                        sabaUserBasicReturn = sabaAPIRequest.UpdateUserAPI(sabaUserBasic, baseClient);
//                        if (sabaUserBasicReturn.locationRetry == retryStatus.retryStart)
//                        {
//                            sabaUserBasicReturn.locationRetry = retryStatus.retryTerminate;
//                            logger.Debug("createUser Retry process ");
//                            sabaUserBasicReturn = sabaAPIRequest.UpdateUserAPI(sabaUserBasicReturn, baseClient);
//                            sabaUserBasic = sabaUserBasicReturn;
//                        }
//                        apiResults = SetSabaManager(sabaUserBasic);
                        
////ajc debug   force create user
//                        //sabaUserCreate = SabaUserBasicToSabaUserCreate(sabaUserBasic);
//                        //sabaUserBasicReturn = sabaAPIRequest.CreateUserAPI(sabaUserCreate, context, baseClient);

//                        break;
//                    case localStatus.Missing:
//                        //saba create user event
//                        logger.Debug("createUser user " + sabaUserStatus);
//                       // sabaUserCreate = SabaUserBasicToSabaUserCreate(sabaUserBasic);
//                        //use return saba basic 
//                        //sabaUserBasicReturn = sabaAPIRequest.CreateUserAPI(sabaUserCreate, context, baseClient);
//                        sabaUserBasicReturn = sabaAPIRequest.CreateUserAPI(sabaUserBasic, baseClient);
//                        if (sabaUserBasicReturn.locationRetry == retryStatus.retryStart)
//                        {
//                            sabaUserBasicReturn.locationRetry = retryStatus.retryTerminate;
//                            logger.Debug("createUser Retry process ");
//                            sabaUserBasicReturn = sabaAPIRequest.CreateUserAPI(sabaUserBasicReturn, baseClient);
//                        }
//                        sabaUserBasic.id = sabaUserBasicReturn.id;
//                        apiResults = SetSabaManager(sabaUserBasic);
//                        break;
//                    case localStatus.Error:
//                        logger.Error("createUser Error " + sabaUserStatus);
//                        break;
//                    default:
//                        break;  
//                }

//                //make any update sin saba record before tranforming back into scim record
//                scimUserReturn = SabaUserBasicToSCIMUser(sabaUserBasic);
//                logger.Info("completed createUser " + scimUserReturn.userName);
//                return scimUserReturn;
//            }
//            catch (SabaException ex)
//            {
//                logger.Error( ex);
//                throw ex;
//            }
//            catch (Exception e)
//            {
//                logger.Error(new OnPremUserManagementException("createUser", e));
//                throw new OnPremUserManagementException("createUser", e);
//            }
        }

        //called on PUT request from controller
        public SCIMUser updateUser(SCIMUser scimUser)
        {
            logger.Debug("ENTER updateUser " + scimUser.userName + " extId " + scimUser.externalId);
            int retryCount = Convert.ToInt16(ConfigurationManager.AppSettings["sfdc.api_retry_count"]);
            int waitMillis = Convert.ToInt16(ConfigurationManager.AppSettings["sfdc.waitMillis"]);

            SfdcUserBasic sfdcUserBasic = new SfdcUserBasic();
            SfdcUserBasic sfdcUserBasicReturn = new SfdcUserBasic();
            SCIMUser scimUserReturn = new SCIMUser();
            sfdcUserBasic = SCIMUserToSfdcUserBasic(scimUser);

          //  sfdcAPIRequest.GetUserAPI(sfdcUserBasic, baseClient);

            while (retryCount >= 0)
            {

                sfdcUserBasicReturn = sfdcAPIRequest.UpdateUserAPI(sfdcUserBasic, baseClient);
                if (sfdcUserBasicReturn.Id == "empty")
                {
                    logger.Debug(" updateUser retryCount = " + retryCount.ToString());
                    // Wait
                    SfdcUtils.Sleep(waitMillis);
                    retryCount--;
                }
                else
                {
                    retryCount = -1;
                }

            }

            if (sfdcUserBasicReturn.Id == "empty")
            {
                throw new SabaGenericInternalServerErrorException("failed to update SFDC ");
            }


            scimUserReturn = SfdcUserBasicToSCIMUser(sfdcUserBasicReturn);

            logger.Debug("Exit updateuser user " + scimUserReturn.userName + " id " + scimUserReturn.id + " extId " + scimUserReturn.externalId);
            return scimUserReturn;
        }


        public bool deleteUser(string id)
        {
            logger.Debug("deleteUser not handled userName id " + id);
            throw new NotImplementedException();

        }
        //called on from updateuser via PUT request from controller
        public bool deleteUser(SCIMUser scimUser)
        {
            logger.Debug("ENTER deleteUser 02 with scimUser " + scimUser.userName);
            throw new NotImplementedException();


            ////localStatus sabaUserStatus;
            ////localStatus compareUserStatus;
            ////SabaUserBasic sabaUserBasic = new SabaUserBasic();
            ////SCIMUser scimUserReturn = new SCIMUser();



            ////try
            ////{
            ////    sabaUserBasic = SCIMUserToSabaUserBasic(scimUser);

            ////    //check is scimUser has affilaiteCode in alliliate.json
            ////    bool result = helper.CheckAffiliateCode(scimUser);
            ////    if (!result)
            ////    {
            ////        logger.Debug("affiliateCode not in file");
            ////        SabaAuditLog.SabaAudit(sabaUserBasic, "affiliateCode not in file", "no action");
            ////        return false;
            ////    }

            ////    sabaUserStatus = EvaluateSabaRecord(sabaUserBasic);
            ////    //compare scimUser to Sabauser as per design
            ////    compareUserStatus = CompareUserContext(sabaUserBasic, scimUser);
            ////    //implement action logic
            ////    switch (sabaUserStatus)
            ////    {
            ////        case localStatus.ByPersonNo:
            ////            //saba deactivate user event
            ////            logger.Debug("deleteUser " + sabaUserBasic.username  + " found  " + sabaUserStatus);
            ////            sabaUserBasic.username = string.Concat("TERM_", sabaUserBasic.username);
            ////            logger.Debug("deleteUser new username " + sabaUserBasic.username);
            ////            sabaAPIRequest.DeleteUserAPI(sabaUserBasic,  baseClient);
            ////            break;
            ////        case localStatus.ByUsername:
            ////            //saba deactivate user event
            ////            logger.Debug("deleteUser " + sabaUserBasic.username + " found  " + sabaUserStatus);
            ////            sabaUserBasic.username = string.Concat("TERM_", sabaUserBasic.username);
            ////            logger.Debug("deleteUser new username " + sabaUserBasic.username);
            ////            sabaAPIRequest.DeleteUserAPI(sabaUserBasic,  baseClient);
            ////            break;
            ////        case localStatus.Missing:
            ////            //no processing required
            ////            logger.Debug("deleteUser " + sabaUserStatus  + " no action");
            ////            SabaAuditLog.SabaAudit(sabaUserBasic, "non existing user ", " deativate no action");
            ////            break;
            ////        case localStatus.Error:
            ////            logger.Error("deleteUser Error " + sabaUserStatus);
            ////            break;
            ////        default:
            ////            break;
            ////    }

            ////    scimUserReturn = SabaUserBasicToSCIMUser(sabaUserBasic);
            ////    logger.Info("completed deactivateUser " + scimUserReturn.userName);
            ////    return true;
            ////}
            ////catch (SabaException ex)
            ////{
            ////    logger.Error(ex);
            ////    throw ex;
            ////}
            ////catch (Exception e)
            ////{
            ////    logger.Error(new OnPremUserManagementException("deleteUser", e));
            ////    throw new OnPremUserManagementException("deleteUser", e);
            ////}
            ////return true;
        }

        //public static string getPrimaryEmail(SCIMUser user)
        //{
        //    logger.Debug(" enter getPrimaryEmail ");
        //    try
        //    {
        //        string email = null;
        //        Email primaryEmail = user.emails.SingleOrDefault<Email>(e => e.type == "primary");
        //        if (primaryEmail != null)
        //        {
        //            email = primaryEmail.value;
        //        }
        //        return email;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}
        //called on POST request from controller
        
        public Okta.SCIM.Models.SCIMUser getUser(string id)
        {
            logger.Debug(" enter enter getUser id " + id + " baseURL " + SfdcContext.baseUrl );
            throw new NotImplementedException();
        }

        public Okta.SCIM.Models.SCIMUserQueryResponse getUsers(Okta.SCIM.Models.PaginationProperties pageProperties, Okta.SCIM.Models.SCIMFilter filter)
        {
            logger.Debug("getUsers filter "  + filter.FilterValue);

            SfdcUserBasic sfdcUserBasic = new SfdcUserBasic();
            SfdcUserBasic sfdcUserBasicReturn = new SfdcUserBasic();
            SCIMUserQueryResponse SCIMResponse = new SCIMUserQueryResponse();
            SCIMResponse.Resources = new List<SCIMUser>();
            SCIMUser scimUserReturn = new SCIMUser();
            string userId;

            //sfdcUserBasic.Username = "okta04.oktatest@Okta.com";
            //sfdcUserBasic.Firstname = "okta04";
            //sfdcUserBasic.Lastname = "oktatest";
            //sfdcUserBasic.Email = "okta04.oktatest@mailinator.com";
            //cleanup filterValue
            string unformattedUsername = filter.FilterValue;
            string formattedUsername = unformattedUsername.Replace("\"", "");
            sfdcUserBasic.Username = formattedUsername;
            userId = sfdcAPIRequest.SearchUserAPI(sfdcUserBasic, baseClient);

            sfdcUserBasicReturn = sfdcUserBasic;
            sfdcUserBasicReturn.Id = userId;


          try
          {
              scimUserReturn = SfdcUserBasicToSCIMUser(sfdcUserBasicReturn);
              scimUserReturn.active = true;
         //     scimUserReturn.id = "00u60q2onnZsS41UG0h7";
         //     scimUserReturn.id = rEvalUser;
              SCIMResponse.Resources.Add(scimUserReturn);
              SCIMResponse.totalResults = 1;
              SCIMResponse.itemsPerPage = 1;
              SCIMResponse.startIndex = 1;
          }
          catch (Exception ex)
          {
              logger.Error("ex" + ex.ToString());
              throw;
          }

            return SCIMResponse;
           // throw new NotImplementedException();
        }
        
        public Okta.SCIM.Models.SCIMGroup createGroup(Okta.SCIM.Models.SCIMGroup group)
        {
            logger.Debug(" enter createGroup not handled");
            throw new NotImplementedException();
        }

        public Okta.SCIM.Models.SCIMGroup getGroup(string id)
        {
            logger.Debug(" enter getGroup not handled");
            SCIMGroup scimGroup = new SCIMGroup();
            return scimGroup;
            //throw new NotImplementedException();
        }

        public Okta.SCIM.Models.SCIMGroupQueryResponse getGroups(Okta.SCIM.Models.PaginationProperties pageProperties)
        {
            logger.Debug(" enter getGroups not handled");
            SCIMGroup scimGroup = new SCIMGroup();
            SCIMGroupQueryResponse  scimGroupQueryResponse = new SCIMGroupQueryResponse();
            scimGroupQueryResponse.Resources.Add(scimGroup);
            return scimGroupQueryResponse;
          //  throw new NotImplementedException();

        }

        public bool deleteGroup(string id)
        {
            logger.Debug(" enter deleteGroup not handled");
            throw new NotImplementedException();
        }

        public bool updateGroup(Okta.SCIM.Models.SCIMGroup group)
        {
            logger.Debug(" enter updateGroup not handled");
            throw new NotImplementedException();
        }

        public Okta.SCIM.Models.ServiceProviderConfiguration getServiceProviderConfig()
        {
            logger.Debug(" getServiceProviderConfig ");

            var appSettings = ConfigurationManager.AppSettings;
            ServiceProviderConfiguration cfg = new ServiceProviderConfiguration();
            cfg.schemas = new List<string>() { "urn:schemas:core:1.0", "urn:okta:schemas:scim:providerconfig:1.0" };
            List<string> usermgmt = new List<string>() {
            UserManagementCapabilities.PUSH_NEW_USERS,
            UserManagementCapabilities.PUSH_PROFILE_UPDATES};
            cfg.ExtensionData = new Dictionary<string, object>();
            cfg.ExtensionData.Add("urn:okta:schemas:scim:providerconfig:1.0", usermgmt);
            return cfg;


            //var appSettings = ConfigurationManager.AppSettings;
            //ServiceProviderConfiguration cfg = new ServiceProviderConfiguration();
            //cfg.schemas = new List<string>() { "urn:schemas:core:1.0", "urn:okta:schemas:scim:providerconfig:1.0" };
            //List<string> usermgmt = new List<string>() { 
            //UserManagementCapabilities.PUSH_PASSWORD_UPDATES, 
            //UserManagementCapabilities.PUSH_NEW_USERS, 
            //UserManagementCapabilities.PUSH_PROFILE_UPDATES};
            //cfg.ExtensionData = new Dictionary<string, object>();
            //cfg.ExtensionData.Add(appSettings["sfdc.custom_extension_urn"], usermgmt);
            //return cfg;

            //full config
            //var appSettings = ConfigurationManager.AppSettings;
            //ServiceProviderConfiguration cfg = new ServiceProviderConfiguration();
            //cfg.schemas = new List<string>() { "urn:schemas:core:1.0", "urn:okta:schemas:scim:providerconfig:1.0" };
            //List<string> usermgmt = new List<string>() { UserManagementCapabilities.PUSH_PASSWORD_UPDATES, 
            //UserManagementCapabilities.GROUP_PUSH,
            //UserManagementCapabilities.IMPORT_NEW_USERS, UserManagementCapabilities.IMPORT_PROFILE_UPDATES,
            //UserManagementCapabilities.PUSH_NEW_USERS, UserManagementCapabilities.PUSH_PROFILE_UPDATES};
            //cfg.ExtensionData = new Dictionary<string, object>();
            //cfg.ExtensionData.Add(appSettings["sfdc.custom_extension_urn"], usermgmt);



        }
    }
}