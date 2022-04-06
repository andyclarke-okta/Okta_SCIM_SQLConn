using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SfdcOPPConn.Connectors;


namespace SfdcOPPConn.Models
{

    public enum localStatus
    {
        ByUsername,
        ByPersonNo,
        Missing,
        Error,
        Normal
    }

    public enum retryStatus
    {
        retryInitial,
        retryStart,
        retryTerminate
    }


    public class SabaUserBasic
    {
        [JsonProperty("id")]
        public string id { get; set; }
        [JsonProperty("person_no")]
        public string person_no { get; set; }
        [JsonProperty("username")]
        public string username { get; set; }
        [JsonProperty("fname")]
        public string fname { get; set; }
        [JsonProperty("lname")]
        public string lname { get; set; }
        [JsonProperty("gender")]
        public string gender { get; set; }
        [JsonProperty("email")]
        public string email { get; set; }
        [JsonProperty("password")]
        public string password { get; set; }
        [JsonProperty("status")]
        public string status { get; set; }
        [JsonProperty("zip")]
        public string zip { get; set; }
        [JsonProperty("company")]
        public string company { get; set; }
        [JsonProperty("manager")]
        public string manager { get; set; }
        [JsonProperty("person_type")]
        public string person_type { get; set; }
        [JsonProperty("ad_create_date")]
        public string ad_create_date { get; set; }
        [JsonProperty("hireDate")]
        public string hireDate { get; set; }
        [JsonProperty("hired_on")]
        public string hired_on { get; set; }
        [JsonProperty("terminated_on")]
        public string terminated_on { get; set; }
        [JsonProperty("location")]
        public string location { get; set; }
        [JsonProperty("locale")]
        public string locale { get; set; }
        [JsonProperty("is_manager")]
        public string is_manager { get; set; }
        [JsonProperty("alternate_manager")]
        public string alternate_manager { get; set; }
        [JsonProperty("job_type")]
        public string job_type { get; set; }
        [JsonProperty("workphone")]
        public string workphone { get; set; }
        [JsonProperty("securityroles")]
        public string securityroles { get; set; }
        [JsonProperty("affiliateCode")]
        public string affiliateCode { get; set; }
        [JsonProperty("href")]
        public string href { get; set; }
        [JsonProperty("locationRetry")]
        public retryStatus locationRetry { get; set; }
    }

    //handle Saba user entity for create user
    public class SabaUserCreate
    {
        [JsonProperty("username")]
        public string username { get; set; }
        [JsonProperty("status")]
        public string status { get; set; }
        [JsonProperty("password")]
        public string password { get; set; }
        [JsonProperty("suffix")]
        public string suffix { get; set; }
        [JsonProperty("title")]
        public string title { get; set; }
        [JsonProperty("state")]
        public string state { get; set; }
        [JsonProperty("country")]
        public string country { get; set; }
        [JsonProperty("city")]
        public string city { get; set; }
        [JsonProperty("zip")]
        public string zip { get; set; }
        [JsonProperty("fname")]
        public string fname { get; set; }
        [JsonProperty("lname")]
        public string lname { get; set; }
        [JsonProperty("email")]
        public string email { get; set; }
        [JsonProperty("gender")]
        public string gender { get; set; }
        [JsonProperty("homephone")]
        public string homephone { get; set; }
        [JsonProperty("workphone")]
        public string workphone { get; set; }
        [JsonProperty("fax")]
        public string fax { get; set; }
        [JsonProperty("is_manager")]
        public string is_manager { get; set; }
        [JsonProperty("password_changed")]
        public string password_changed { get; set; }
        [JsonProperty("ss_no")]
        public string ss_no { get; set; }
        [JsonProperty("job_title")]
        public string job_title { get; set; }
        [JsonProperty("terminated_on")]
        public string terminated_on { get; set; }
        [JsonProperty("person_type")]
        public string person_type { get; set; }
        [JsonProperty("mname")]
        public string mname { get; set; }
        [JsonProperty("started_on")]
        public string started_on { get; set; }
        [JsonProperty("job_title_type")]
        public string job_title_type { get; set; }
        [JsonProperty("religion")]
        public string religion { get; set; }
        [JsonProperty("ethnicity")]
        public string ethnicity { get; set; }
        [JsonProperty("date_of_birth")]
        public string date_of_birth { get; set; }
        [JsonProperty("person_no")]
        public string person_no { get; set; }
        [JsonProperty("desired_job_type_id")]
        public string desired_job_type_id { get; set; }
        [JsonProperty("special_user")]
        public string special_user { get; set; }
        [JsonProperty("addr1")]
        public string addr1 { get; set; }
        [JsonProperty("addr2")]
        public string addr2 { get; set; }
        [JsonProperty("addr3")]
        public string addr3 { get; set; }
        [JsonProperty("correspondence_preference1")]
        public string correspondence_preference1 { get; set; }
        [JsonProperty("correspondence_preference2")]
        public string correspondence_preference2 { get; set; }
        [JsonProperty("correspondence_preference3")]
        public string correspondence_preference3 { get; set; }
        [JsonProperty("alternate_manager")]
        public string alternate_manager { get; set; }
        [JsonProperty("securityDomain")]
        public SabaCompoundNameObject securityDomain { get; set; }
        [JsonProperty("jobtype_id")]
        public SabaCompoundNameObject jobtype_id { get; set; }
        [JsonProperty("company_id")]
        public SabaCompoundNameObject company_id { get; set; }
        [JsonProperty("home_company_id")]
        public SabaCompoundNameObject home_company_id { get; set; }
        [JsonProperty("home_domain")]
        public SabaCompoundNameObject home_domain { get; set; }
        [JsonProperty("timezone_id")]
        public SabaCompoundNameObject timezone_id { get; set; }
        [JsonProperty("locale_id")]
        public SabaCompoundNameObject locale_id { get; set; }
        [JsonProperty("manager_id")]
        public SabaCompoundNameObject manager_id { get; set; }
        [JsonProperty("location_id")]
        public SabaCompoundNameObject location_id { get; set; }
        [JsonProperty("securityRoles")]
        public List<SabaSecurityAssignment> securityRoles { get; set; }
    }
    //subsection of SabaUserCreate

    //handle Saba user entity for create user
    public class SabaUserUpdate
    {
        [JsonProperty("username")]
        public string username { get; set; }
        [JsonProperty("status")]
        public string status { get; set; }
        //[JsonProperty("password")]
        //public string password { get; set; }
        //[JsonProperty("suffix")]
        //public string suffix { get; set; }
        //[JsonProperty("title")]
        //public string title { get; set; }
        //[JsonProperty("state")]
        //public string state { get; set; }
        //[JsonProperty("country")]
        //public string country { get; set; }
        //[JsonProperty("city")]
        //public string city { get; set; }
        [JsonProperty("zip")]
        public string zip { get; set; }
        [JsonProperty("fname")]
        public string fname { get; set; }
        [JsonProperty("lname")]
        public string lname { get; set; }
        [JsonProperty("email")]
        public string email { get; set; }
        [JsonProperty("gender")]
        public string gender { get; set; }
        //[JsonProperty("homephone")]
        //public string homephone { get; set; }
        [JsonProperty("workphone")]
        public string workphone { get; set; }
        //[JsonProperty("fax")]
        //public string fax { get; set; }
        [JsonProperty("is_manager")]
        public string is_manager { get; set; }
        //[JsonProperty("password_changed")]
        //public string password_changed { get; set; }
        //[JsonProperty("ss_no")]
        //public string ss_no { get; set; }
        //[JsonProperty("job_title")]
        //public string job_title { get; set; }
        //[JsonProperty("terminated_on")]
        //public string terminated_on { get; set; }
        [JsonProperty("person_type")]
        public string person_type { get; set; }
        //[JsonProperty("mname")]
        //public string mname { get; set; }
        //[JsonProperty("started_on")]
        //public string started_on { get; set; }
        //[JsonProperty("job_title_type")]
        //public string job_title_type { get; set; }
        //[JsonProperty("religion")]
        //public string religion { get; set; }
        //[JsonProperty("ethnicity")]
        //public string ethnicity { get; set; }
        //[JsonProperty("date_of_birth")]
        //public string date_of_birth { get; set; }
        [JsonProperty("person_no")]
        public string person_no { get; set; }
        //[JsonProperty("desired_job_type_id")]
        //public string desired_job_type_id { get; set; }
        //[JsonProperty("special_user")]
        //public string special_user { get; set; }
        //[JsonProperty("addr1")]
        //public string addr1 { get; set; }
        //[JsonProperty("addr2")]
        //public string addr2 { get; set; }
        //[JsonProperty("addr3")]
        //public string addr3 { get; set; }
        //[JsonProperty("correspondence_preference1")]
        //public string correspondence_preference1 { get; set; }
        //[JsonProperty("correspondence_preference2")]
        //public string correspondence_preference2 { get; set; }
        //[JsonProperty("correspondence_preference3")]
        //public string correspondence_preference3 { get; set; }
        [JsonProperty("alternate_manager")]
        public string alternate_manager { get; set; }
        [JsonProperty("securityDomain")]
        public SabaCompoundNameObject securityDomain { get; set; }
        [JsonProperty("jobtype_id")]
        public SabaCompoundNameObject jobtype_id { get; set; }
        [JsonProperty("company_id")]
        public SabaCompoundNameObject company_id { get; set; }
        //[JsonProperty("home_company_id")]
        //public SabaHomeCompanyId home_company_id { get; set; }
        [JsonProperty("home_domain")]
        public SabaCompoundNameObject home_domain { get; set; }
        [JsonProperty("timezone_id")]
        public SabaCompoundNameObject timezone_id { get; set; }
        [JsonProperty("locale_id")]
        public SabaCompoundNameObject locale_id { get; set; }
        [JsonProperty("manager_id")]
        public SabaCompoundNameObject manager_id { get; set; }
        [JsonProperty("location_id")]
        public SabaCompoundNameObject location_id { get; set; }
        [JsonProperty("securityRoles")]
        public List<SabaSecurityAssignment> securityRoles { get; set; }
    }

    //
    public class SabaCompoundNameObject
    {
        [JsonProperty("id")]
        public string id { get; set; }
        [JsonProperty("displayName")]
        public string displayName { get; set; }
    }
    //
    public class SabaDisplayNameObject
    {
        [JsonProperty("displayName")]
        public string displayName { get; set; }
    }
    //
    public class SabaSecurityAssignment
    {
        [JsonProperty("name")]
        public string name { get; set; }
        [JsonProperty("securityDomain")]
        public SabaDisplayNameObject securityDomain { get; set; }
    }
    //
    public class SabaLocationId
    {
        [JsonProperty("id")]
        public string id { get; set; }
        [JsonProperty("displayName")]
        public string displayName { get; set; }
    }
    //
    public class SabaManagerId
    {
        [JsonProperty("id")]
        public string id { get; set; }
        [JsonProperty("displayName")]
        public string displayName { get; set; }
    }
    //
    public class SabaLocaleId
    {
        [JsonProperty("id")]
        public string id { get; set; }
        [JsonProperty("displayName")]
        public string displayName { get; set; }
    }
    //
    public class SabaTimezoneId
    {
        [JsonProperty("id")]
        public string id { get; set; }
        [JsonProperty("displayName")]
        public string displayName { get; set; }
    }
    //
    public class SabaHomeDomain
    {
        [JsonProperty("id")]
        public string id { get; set; }
        [JsonProperty("displayName")]
        public string displayName { get; set; }
    }
    //
    public class SabaHomeCompanyId
    {
        [JsonProperty("id")]
        public string id { get; set; }
        [JsonProperty("displayName")]
        public string displayName { get; set; }
    }
    //
    public class SabaCompanyId
    {
        [JsonProperty("id")]
        public string id { get; set; }
        [JsonProperty("displayName")]
        public string displayName { get; set; }
    }
    //
    public class SabaJobtypeId
    {
        [JsonProperty("id")]
        public string id { get; set; }
        [JsonProperty("displayName")]
        public string displayName { get; set; }
    }
    //
    public class SabaSecurityDomain
    {
        [JsonProperty("id")]
        public string id { get; set; }
        [JsonProperty("displayName")]
        public string displayName { get; set; }
    }







}