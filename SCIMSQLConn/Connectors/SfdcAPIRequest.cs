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
using Newtonsoft.Json;
using Okta.Core.Models;
using System.Net.Http;
using SfdcOPPConn.Models;
using SfdcOPPConn.Exceptions;

namespace SfdcOPPConn.Connectors
{

    public class SfdcApiRequest
    {
        private  readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public  string searchQuery { get; set; }
        internal SfdcProvHelper helper = new SfdcProvHelper();



        public void CreateAuthAPI(SfdcHttpClient baseClient)
        {

            //   logger.Debug("ENTER SessionCreate username " + SabaContext.username + " baseURL " + SabaContext.baseURL + " APIversion " + SabaContext.APIversion);
            logger.Debug("ENTER CreateAuthAPI username ");

     

            string baseURL = "https://login.salesforce.com/services/oauth2/authorize?";
            string response_type = "response_type=code&";
            string client_id = "client_id=3MVG9KI2HHAq33RxMbzT1LlttGxoQPBUk6r.S_oh5p4nodd4kkAEk4jyOZYa5Bd04RQHlZ6INzYnmSPFUmEWt&";
            string redirect_uri = "redirect_uri=http://localhost:8899/";
            string sabaGetLoginUrl = String.Concat(baseURL, response_type, client_id, redirect_uri);

            logger.Debug("url" + sabaGetLoginUrl);

            Uri resourcePath = new Uri(sabaGetLoginUrl);

            HttpResponseMessage response = baseClient.GetAuth(resourcePath);
            string responseTopContent = response.ToString();
            var index = responseTopContent.IndexOf(",", 20);
            var rspStatus = responseTopContent.Substring(0, index);
            string content = response.Content.ReadAsStringAsync().Result;
            logger.Debug("response " + rspStatus + " content " + content);





            return;
        }

        public bool CreateTokenAPI( SfdcHttpClient baseClient)
        {

         //   logger.Debug("ENTER SessionCreate username " + SabaContext.username + " baseURL " + SabaContext.baseURL + " APIversion " + SabaContext.APIversion);
            logger.Debug("ENTER CreateTokenAPI ");
            string accessToken;
            SfdcCreateTokenResponse tokenResponse;

            //string baseURL = "https://login.salesforce.com/services/oauth2/token?";
            //string code = "";
            //string grant_type = "grant_type=authorization_code&";
            //string client_id = "client_id=3MVG9KI2HHAq33RxMbzT1LlttGxoQPBUk6r.S_oh5p4nodd4kkAEk4jyOZYa5Bd04RQHlZ6INzYnmSPFUmEWt&";
            //string client_secret = "83951689855516670";
            //string redirect_uri = "redirect_uri=http://localhost:8899/api/Session";

           // string tokenUrl = "https://login.salesforce.com/services/oauth2/token";
            string grant_type = "?grant_type=password";
            string client_id = "&client_id=" + SfdcContext.clientId;
            string client_secret = "&client_secret="+SfdcContext.clientSecret;
            string username = "&username="+SfdcContext.username;
            string password = "&password="+SfdcContext.password;


            string sfdcGetLoginUrl = String.Concat(SfdcContext.tokenUrl,grant_type, client_id,client_secret, username,password);

            logger.Debug("url" + sfdcGetLoginUrl);

            Uri resourcePath = new Uri(sfdcGetLoginUrl);

            try
            {
                HttpResponseMessage response = baseClient.PostToken(resourcePath);
                string responseTopContent = response.ToString();
                var index = responseTopContent.IndexOf(",", 20);
                var rspStatus = responseTopContent.Substring(0, index);
                string content = response.Content.ReadAsStringAsync().Result;
                logger.Debug("response " + rspStatus + " content " + content);
                tokenResponse = new SfdcCreateTokenResponse();
                tokenResponse = SfdcUtils.Deserialize<SfdcCreateTokenResponse>(response);
            }
            catch (Exception ex)
            {
                logger.Error("error in http call while searching for user");
                return false;
               // throw ex;
            }

            accessToken = tokenResponse.access_token;
            SfdcContext.bearerToken = accessToken;
            return true;
 
        }


        public bool GetUserAPI(SfdcUserBasic sfdcUserBasic, SfdcHttpClient baseClient)
        {
            logger.Debug("ENTER GetUserAPI username = " + sfdcUserBasic.Username);

            string localId;
            string fullName;

            //check for Sfdc token
            bool rCreateToken = CreateTokenAPI(baseClient);
            if (!rCreateToken)
            {
                logger.Error("failed to create token");
                return false;
            }



            ////  string baseURL = "https://na34.salesforce.com";
            //  string extentionUrl = "/services/data/";
            //  string padding = "/query?q=";
            //  //string myQuery = "SELECT Name, Id from Account LIMIT 100";
            //  string myQuery = "SELECT email, name, roleid from User LIMIT 100";
            //  string encodeQuery = HttpUtility.UrlEncode(myQuery);
            //  string sfdcGetUserUrl = String.Concat(SfdcContext.baseUrl,extentionUrl,SfdcContext.apiVersion,padding,  encodeQuery);

            //ajc debug
            //   string testUrl = "https://na34.salesforce.com/services/data/v36.0/sobjects/User/describe";
            //string testUrl = "https://na34.salesforce.com/services/data/v36.0/sobjects/User/describe/compactLayouts";
            //string testUrl =  "https://na34.salesforce.com/services/data/v36.0/sobjects/User";
            string extentionUrl = "/services/data/";
            string listUserUrl = "/sobjects/User/";
            string sfdcGetUserUrl = String.Concat(SfdcContext.baseUrl, extentionUrl, SfdcContext.apiVersion, listUserUrl,sfdcUserBasic.Id);

            logger.Debug("url " + sfdcGetUserUrl);

            Uri resourcePath = new Uri(sfdcGetUserUrl);


            HttpResponseMessage response = baseClient.GetSpecial(resourcePath);
            //ajc debug
            string responseTopContent = response.ToString();
            var index = responseTopContent.IndexOf(",", 20);
            var rspStatus = responseTopContent.Substring(0, index);
            string content = response.Content.ReadAsStringAsync().Result;
            logger.Debug("response " + rspStatus + " content " + content);
            SfdcGetUserResponse sfdcGetUserResponse = new SfdcGetUserResponse();
          //  sfdcGetUserResponse.attributes = new UserAttributes();
            sfdcGetUserResponse = SfdcUtils.Deserialize<SfdcGetUserResponse>(response);



            //SfdcListUsersResponse sfdcListUsersResponse = new SfdcListUsersResponse();
            //sfdcListUsersResponse = SfdcUtils.Deserialize<SfdcListUsersResponse>(response);
            //foreach (Recentitem item in sfdcListUsersResponse.recentItems)
            //{
            //    fullName = sfdcUserBasic.firstname + " " + sfdcUserBasic.lastname;
            //    if (item.Name == fullName)
            //    {
            //        localId = item.Id;
            //        return localId;
            //    }

            //}


            //SabaPeopleResponse peopleSearchResults = new SabaPeopleResponse();
            //peopleSearchResults = SabaUtils.Deserialize<SabaPeopleResponse>(response);

            //if(Int32.Parse(peopleSearchResults.totalResults) == 0)
            //{
            //    logger.Debug("query result empty");
            //    searchResults = "";
            //}
            //else if (Int32.Parse(peopleSearchResults.totalResults) > 1)
            //{
            //    logger.Error("more than one result returned form query");
            //    searchResults = "";
            //}
            //else if (Int32.Parse(peopleSearchResults.totalResults) == 1)
            //{
            //    logger.Debug("query found username " + peopleSearchResults.results[0].status);
            //    //need internal saba id for saba update API
            //    sabaUserBasic.href = peopleSearchResults.results[0].href;
            //    sabaUserBasic.id = peopleSearchResults.results[0].id;
            //    searchResults = peopleSearchResults.results[0].status;
            //}
            return true;

        }     
        
        //
        public  string SearchUserAPI(SfdcUserBasic sfdcUserBasic,  SfdcHttpClient baseClient)
        {
            logger.Debug("ENTER SearchUserAPI username = " + sfdcUserBasic.Username );

            string localId;
            string fullName;

            SfdcSearchUserResponse sfdcSearchUserResponse;
            //check for Saba cert
            bool rCreateToken = CreateTokenAPI(baseClient);
            if (!rCreateToken)
            {
            
                logger.Error("failed to create token");
                return "error creating token";
            }


//ajc debug
            //  string baseURL = "https://na34.salesforce.com";
            string extentionUrl = "/services/data/";
            string padding = "/query?q=";
            //string myQuery = "SELECT Name, Id from Account LIMIT 100";
       //     string myQuery = "SELECT user.id, user.Email, user.FirstName, user.LastName, user.profile.name, user.Username, user.IsActive from User LIMIT 100";
        //    string myQuery = "SELECT user.id, user.Email, user.FirstName, user.LastName, user.profile.name, user.Username, user.IsActive from User where user.Username = 'okta04.oktatest@mailinator.com' ";
            string myQuery = "SELECT user.id, user.Email, user.FirstName, user.LastName, user.profile.name, user.Username, user.IsActive from User where user.Username = '"  + sfdcUserBasic.Username + "'";
            string encodeQuery = HttpUtility.UrlEncode(myQuery);
            string sfdcGetUserUrl = String.Concat(SfdcContext.baseUrl, extentionUrl, SfdcContext.apiVersion, padding, encodeQuery);

         //   string testUrl = "https://na34.salesforce.com/services/data/v36.0/sobjects/User/describe";
            //string testUrl = "https://na34.salesforce.com/services/data/v36.0/sobjects/User/describe/compactLayouts";
            //string testUrl =  "https://na34.salesforce.com/services/data/v36.0/sobjects/User";



            //string extentionUrl = "/services/data/";
            //string listUserUrl = "/sobjects/User";
            //string sfdcGetUserUrl = String.Concat(SfdcContext.baseUrl, extentionUrl, SfdcContext.apiVersion, listUserUrl);

            logger.Debug("url " + sfdcGetUserUrl);
            Uri resourcePath = new Uri(sfdcGetUserUrl);

            try
            {
                HttpResponseMessage response = baseClient.GetSpecial(resourcePath);
                string responseTopContent = response.ToString();
                var index = responseTopContent.IndexOf(",", 20);
                var rspStatus = responseTopContent.Substring(0, index);
                string content = response.Content.ReadAsStringAsync().Result;
                logger.Debug("response " + rspStatus + " content " + content);

                sfdcSearchUserResponse = new SfdcSearchUserResponse();
                sfdcSearchUserResponse.records = new List<Record>();
                sfdcSearchUserResponse.records.Add(new Record());
                sfdcSearchUserResponse.records[0].attributes = new Attributes2();
                sfdcSearchUserResponse.records[0].Profile = new Profile();
                sfdcSearchUserResponse.records[0].Profile.attributes = new Attributes1();
                sfdcSearchUserResponse = SfdcUtils.Deserialize<SfdcSearchUserResponse>(response);
            

            }
            catch (Exception ex)
            {
                logger.Error("error in http call while searching for user");
                throw ex;
            }

            foreach ( Record item in sfdcSearchUserResponse.records)
            {
                if (item.Username == sfdcUserBasic.Username)
                {
                    localId = item.Id;
                    return localId;
                }

            }
            logger.Debug("User not found in SFDC " + sfdcUserBasic.PrimaryEmail);
            return "not found";

                //SabaPeopleResponse peopleSearchResults = new SabaPeopleResponse();
                //peopleSearchResults = SabaUtils.Deserialize<SabaPeopleResponse>(response);
   
                //if(Int32.Parse(peopleSearchResults.totalResults) == 0)
                //{
                //    logger.Debug("query result empty");
                //    searchResults = "";
                //}
                //else if (Int32.Parse(peopleSearchResults.totalResults) > 1)
                //{
                //    logger.Error("more than one result returned form query");
                //    searchResults = "";
                //}
                //else if (Int32.Parse(peopleSearchResults.totalResults) == 1)
                //{
                //    logger.Debug("query found username " + peopleSearchResults.results[0].status);
                //    //need internal saba id for saba update API
                //    sabaUserBasic.href = peopleSearchResults.results[0].href;
                //    sabaUserBasic.id = peopleSearchResults.results[0].id;
                //    searchResults = peopleSearchResults.results[0].status;
                //}


        }

      //  }

        //
     //   public SabaUserBasic CreateUserAPI(SabaUserBasic sabaUserBasic,  SfdcHttpClient baseClient)
     //   {
     //       logger.Debug("ENTER CreateUserAPI username = " + sabaUserBasic.username);
           
     ////       string rspContent = "";
     ////       string rspStatus = "";
     ////       string apiEndPoint = "people?type=internal";
     ////       string sabaGetUserUrl = String.Concat(SfdcContext.baseURL, SfdcContext.APIversion, apiEndPoint);
     ////       logger.Debug("url " + sabaGetUserUrl);
     //       SabaUserBasic sabaUserBasicReturn = new SabaUserBasic();

     //       Uri resourcePath = new Uri(sabaGetUserUrl);

     //       //check for Saba cert
     //       CreateTokenAPI( baseClient);

     //       //create request body
     //       SabaUserCreate sabaUserCreate = new SabaUserCreate();
     //       sabaUserCreate = helper.SabaUserBasicToSabaUserCreate(sabaUserBasic);


     //       // Serialize the Request Body
     //       string serializedbody = SabaUtils.SerializeObject(sabaUserCreate);
     //       logger.Debug("serializedBody " + serializedbody);

     //       try
     //       {

     //           HttpResponseMessage response = baseClient.PostSpecial(resourcePath, serializedbody);
     ////ajc debug
     //           string responseTopContent = response.ToString();
     //           var index = responseTopContent.IndexOf(",", 20);
     //           rspStatus = responseTopContent.Substring(0, index);
     //           rspContent = response.Content.ReadAsStringAsync().Result;
     //           logger.Debug("response " + rspStatus + " content " + rspContent);
     //           //need the saba user id to be written into the return record
     //           SabaAuditLog.SabaAudit(sabaUserCreate, sabaUserBasic, rspStatus, "create");
     //           SabaCreateResponse userCreateResponse = SabaUtils.Deserialize<SabaCreateResponse>(response);
     //           sabaUserBasicReturn = sabaUserBasic;
     //           sabaUserBasicReturn.id = userCreateResponse.id;

     //       }
     //       catch (SabaLocationIdErrorException ex)
     //       {
     //           logger.Warn("Saba Location Error " + ex.ErrorMessage);
     //           //found error with Location Id Saba attribute
     //           //set to blank, rerun command
     //           if (sabaUserBasic.locationRetry == retryStatus.retryInitial)
     //           {
     //               sabaUserBasicReturn = sabaUserBasic;
     //               sabaUserBasicReturn.location = "";
     //               sabaUserBasicReturn.locationRetry = retryStatus.retryStart;
     //               SabaAuditLog.SabaAudit(sabaUserCreate, sabaUserBasic, "StatusCode: 500, ReasonPhrase: Warning LocationId", "create retry");
     //           }
     //       }
     //       catch (SabaManagerIdErrorException ex)
     //       {
     //           logger.Debug("Saba Manager Error " + ex.ErrorMessage);
     //           SabaAuditLog.SabaAudit(sabaUserCreate, sabaUserBasic, "StatusCode: 500, ReasonPhrase: Error ManagerId", "create failed");
     //           throw ex;
                
     //       }
     //       catch (SabaException ex)
     //       {
     //           SabaAuditLog.SabaAudit(sabaUserCreate, sabaUserBasic, "StatusCode: 500, ReasonPhrase: 'Internal Server Error'", "create failed");
     //           throw ex;
     //       }


        //    return sabaUserBasicReturn;
        //}
        //
        public SfdcUserBasic UpdateUserAPI(SfdcUserBasic sfdcUserBasic,  SfdcHttpClient baseClient)
        {
            logger.Debug("ENTER UpdateUserAPI username = " + sfdcUserBasic.Username + " Id " + sfdcUserBasic.Id);
            string content = "";
            string rspStatus = "";
            //string apiEndPoint = "people/" + sfdcUserBasic.id;
            //SabaUserBasic sabaUserBasicReturn = new SabaUserBasic();
            SfdcUserBasic sfdcUserBasicReturn = new SfdcUserBasic();

            //check for Sfdc token
            bool rCreateToken =   CreateTokenAPI(baseClient);
            if (!rCreateToken)
            {
                sfdcUserBasicReturn.Id = "empty";
                logger.Error("failed to create token");
                return sfdcUserBasicReturn;
            }

            string extentionUrl = "/services/data/";
            string listUserUrl = "/sobjects/User/";
            string sfdcGetUserUrl = String.Concat(SfdcContext.baseUrl, extentionUrl, SfdcContext.apiVersion, listUserUrl, sfdcUserBasic.Id);
            logger.Debug("url " + sfdcGetUserUrl);
            Uri resourcePath = new Uri(sfdcGetUserUrl);

            //create request body
            SfdcUserUpdate sfdcUserUpdate = new SfdcUserUpdate();
            sfdcUserUpdate = helper.SfdcUserBasicToSfdcUserUpdate(sfdcUserBasic);

            // Serialize the Request Body
            string serializedbody = SfdcUtils.SerializeObject(sfdcUserUpdate);
            logger.Debug("serializedbody  " + serializedbody);

            try
            {
                HttpResponseMessage response = baseClient.PatchSpecial(resourcePath, serializedbody);
                string responseTopContent = response.ToString();
                var index = responseTopContent.IndexOf(",", 20);
                rspStatus = responseTopContent.Substring(0, index);
                content = response.Content.ReadAsStringAsync().Result;
                logger.Debug("response " + rspStatus + " content " + content);
                sfdcUserBasicReturn = sfdcUserBasic;
            }
            catch (Exception ex)
            {
                sfdcUserBasicReturn.Id = "empty";
                logger.Error("error with Http while updating record");
               // throw ex;
            }

                //SabaPeopleResponse userUpdateResponse = new SabaPeopleResponse();
                //userUpdateResponse = SfdcUtils.Deserialize<SabaPeopleResponse>(response);

                //SfdcAuditLog.SabaAudit(sfdcUserUpdate, sfdcUserBasic, rspStatus, "update");
                //sabaUserBasicReturn = sfdcUserBasic;

             return sfdcUserBasicReturn;


            //}
            //catch (SabaLocationIdErrorException ex)
            //{
            //    logger.Warn("Saba Location Error " + ex.ErrorMessage);
            //    //found error with Location Id Saba attribute
            //    //set to blank, rerun command
            //    if (sabaUserBasic.locationRetry == retryStatus.retryInitial)
            //    {
            //        sabaUserBasicReturn = sabaUserBasic;
            //        sabaUserBasicReturn.location = "";
            //        sabaUserBasicReturn.locationRetry = retryStatus.retryStart;
            //        SabaAuditLog.SabaAudit(sabaUserUpdate, sabaUserBasic, "StatusCode: 500, ReasonPhrase: Warning LocationId", "update retry");
            //    }
            //}
            //catch (SabaManagerIdErrorException ex)
            //{
            //    logger.Debug("Saba Manager Error " + ex.ErrorMessage);
            //    SabaAuditLog.SabaAudit(sabaUserUpdate, sabaUserBasic, "StatusCode: 500, ReasonPhrase: Error ManagerId", "update failed");
            //    throw ex;
            //}
            //catch (SabaException ex)
            //{
            //    SabaAuditLog.SabaAudit(sabaUserUpdate, sabaUserBasic, "StatusCode: 500, ReasonPhrase: 'Internal Server Error'", "update failed");
            //    throw ex;
            //}

        }

        
 
    }
}