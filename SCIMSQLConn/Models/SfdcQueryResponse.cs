using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SfdcOPPConn.Models
{



    public class SfdcCreateTokenResponse
    {
        public string access_token { get; set; }
        public string instance_url { get; set; }
        public string id { get; set; }
        public string token_type { get; set; }
        public string issued_at { get; set; }
        public string signature { get; set; }
    }



    public class SfdcListUsersResponse
    {
        public Objectdescribe objectDescribe { get; set; }
        //public Recentitem[] recentItems { get; set; }
        public List<Recentitem> recentItems { get; set; }
    }

    public class Objectdescribe
    {
        public bool activateable { get; set; }
        public bool createable { get; set; }
        public bool custom { get; set; }
        public bool customSetting { get; set; }
        public bool deletable { get; set; }
        public bool deprecatedAndHidden { get; set; }
        public bool feedEnabled { get; set; }
        public string keyPrefix { get; set; }
        public string label { get; set; }
        public string labelPlural { get; set; }
        public bool layoutable { get; set; }
        public bool mergeable { get; set; }
        public string name { get; set; }
        public bool queryable { get; set; }
        public bool replicateable { get; set; }
        public bool retrieveable { get; set; }
        public bool searchable { get; set; }
        public bool triggerable { get; set; }
        public bool undeletable { get; set; }
        public bool updateable { get; set; }
        public Urls urls { get; set; }
    }

    public class Urls
    {
        public string compactLayouts { get; set; }
        public string rowTemplate { get; set; }
        public string namedLayouts { get; set; }
        public string passwordUtilities { get; set; }
        public string defaultValues { get; set; }
        public string describe { get; set; }
        public string quickActions { get; set; }
        public string layouts { get; set; }
        public string sobject { get; set; }
    }

    public class Recentitem
    {
        public Attributes attributes { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class Attributes
    {
        public string type { get; set; }
        public string url { get; set; }
    }


    public class SfdcGetUserResponse
    {
        public UserAttributes attributes { get; set; }
        public string Id { get; set; }
        public string Username { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Name { get; set; }
        public object CompanyName { get; set; }
        public object Division { get; set; }
        public object Department { get; set; }
        public object Title { get; set; }
        public object Street { get; set; }
        public object City { get; set; }
        public object State { get; set; }
        public object PostalCode { get; set; }
        public object Country { get; set; }
        public object Latitude { get; set; }
        public object Longitude { get; set; }
        public object GeocodeAccuracy { get; set; }
        public object Address { get; set; }
        public string Email { get; set; }
        public bool EmailPreferencesAutoBcc { get; set; }
        public bool EmailPreferencesAutoBccStayInTouch { get; set; }
        public bool EmailPreferencesStayInTouchReminder { get; set; }
        public object SenderEmail { get; set; }
        public object SenderName { get; set; }
        public object Signature { get; set; }
        public object StayInTouchSubject { get; set; }
        public object StayInTouchSignature { get; set; }
        public object StayInTouchNote { get; set; }
        public object Phone { get; set; }
        public object Fax { get; set; }
        public object MobilePhone { get; set; }
        public string Alias { get; set; }
        public string CommunityNickname { get; set; }
        public string BadgeText { get; set; }
        public bool IsActive { get; set; }
        public string TimeZoneSidKey { get; set; }
        public object UserRoleId { get; set; }
        public string LocaleSidKey { get; set; }
        public bool ReceivesInfoEmails { get; set; }
        public bool ReceivesAdminInfoEmails { get; set; }
        public string EmailEncodingKey { get; set; }
        public string ProfileId { get; set; }
        public string UserType { get; set; }
        public string LanguageLocaleKey { get; set; }
        public object EmployeeNumber { get; set; }
        public object DelegatedApproverId { get; set; }
        public object ManagerId { get; set; }
        public DateTime LastLoginDate { get; set; }
        public DateTime LastPasswordChangeDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedById { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public string LastModifiedById { get; set; }
        public DateTime SystemModstamp { get; set; }
        public object OfflineTrialExpirationDate { get; set; }
        public object OfflinePdaTrialExpirationDate { get; set; }
        public bool UserPermissionsMarketingUser { get; set; }
        public bool UserPermissionsOfflineUser { get; set; }
        public bool UserPermissionsCallCenterAutoLogin { get; set; }
        public bool UserPermissionsMobileUser { get; set; }
        public bool UserPermissionsSFContentUser { get; set; }
        public bool UserPermissionsKnowledgeUser { get; set; }
        public bool UserPermissionsInteractionUser { get; set; }
        public bool UserPermissionsSupportUser { get; set; }
        public bool UserPermissionsJigsawProspectingUser { get; set; }
        public bool UserPermissionsSiteforceContributorUser { get; set; }
        public bool UserPermissionsSiteforcePublisherUser { get; set; }
        public bool UserPermissionsChatterAnswersUser { get; set; }
        public bool UserPermissionsWorkDotComUserFeature { get; set; }
        public bool ForecastEnabled { get; set; }
        public bool UserPreferencesActivityRemindersPopup { get; set; }
        public bool UserPreferencesEventRemindersCheckboxDefault { get; set; }
        public bool UserPreferencesTaskRemindersCheckboxDefault { get; set; }
        public bool UserPreferencesReminderSoundOff { get; set; }
        public bool UserPreferencesDisableAllFeedsEmail { get; set; }
        public bool UserPreferencesDisableFollowersEmail { get; set; }
        public bool UserPreferencesDisableProfilePostEmail { get; set; }
        public bool UserPreferencesDisableChangeCommentEmail { get; set; }
        public bool UserPreferencesDisableLaterCommentEmail { get; set; }
        public bool UserPreferencesDisProfPostCommentEmail { get; set; }
        public bool UserPreferencesContentNoEmail { get; set; }
        public bool UserPreferencesContentEmailAsAndWhen { get; set; }
        public bool UserPreferencesApexPagesDeveloperMode { get; set; }
        public bool UserPreferencesHideCSNGetChatterMobileTask { get; set; }
        public bool UserPreferencesDisableMentionsPostEmail { get; set; }
        public bool UserPreferencesDisMentionsCommentEmail { get; set; }
        public bool UserPreferencesHideCSNDesktopTask { get; set; }
        public bool UserPreferencesHideChatterOnboardingSplash { get; set; }
        public bool UserPreferencesHideSecondChatterOnboardingSplash { get; set; }
        public bool UserPreferencesDisCommentAfterLikeEmail { get; set; }
        public bool UserPreferencesDisableLikeEmail { get; set; }
        public bool UserPreferencesDisableMessageEmail { get; set; }
        public bool UserPreferencesJigsawListUser { get; set; }
        public bool UserPreferencesDisableBookmarkEmail { get; set; }
        public bool UserPreferencesDisableSharePostEmail { get; set; }
        public bool UserPreferencesEnableAutoSubForFeeds { get; set; }
        public bool UserPreferencesDisableFileShareNotificationsForApi { get; set; }
        public bool UserPreferencesShowTitleToExternalUsers { get; set; }
        public bool UserPreferencesShowManagerToExternalUsers { get; set; }
        public bool UserPreferencesShowEmailToExternalUsers { get; set; }
        public bool UserPreferencesShowWorkPhoneToExternalUsers { get; set; }
        public bool UserPreferencesShowMobilePhoneToExternalUsers { get; set; }
        public bool UserPreferencesShowFaxToExternalUsers { get; set; }
        public bool UserPreferencesShowStreetAddressToExternalUsers { get; set; }
        public bool UserPreferencesShowCityToExternalUsers { get; set; }
        public bool UserPreferencesShowStateToExternalUsers { get; set; }
        public bool UserPreferencesShowPostalCodeToExternalUsers { get; set; }
        public bool UserPreferencesShowCountryToExternalUsers { get; set; }
        public bool UserPreferencesShowProfilePicToGuestUsers { get; set; }
        public bool UserPreferencesShowTitleToGuestUsers { get; set; }
        public bool UserPreferencesShowCityToGuestUsers { get; set; }
        public bool UserPreferencesShowStateToGuestUsers { get; set; }
        public bool UserPreferencesShowPostalCodeToGuestUsers { get; set; }
        public bool UserPreferencesShowCountryToGuestUsers { get; set; }
        public bool UserPreferencesDisableFeedbackEmail { get; set; }
        public bool UserPreferencesDisableWorkEmail { get; set; }
        public bool UserPreferencesHideS1BrowserUI { get; set; }
        public bool UserPreferencesDisableEndorsementEmail { get; set; }
        public bool UserPreferencesPathAssistantCollapsed { get; set; }
        public bool UserPreferencesCacheDiagnostics { get; set; }
        public bool UserPreferencesShowEmailToGuestUsers { get; set; }
        public bool UserPreferencesShowManagerToGuestUsers { get; set; }
        public bool UserPreferencesShowWorkPhoneToGuestUsers { get; set; }
        public bool UserPreferencesShowMobilePhoneToGuestUsers { get; set; }
        public bool UserPreferencesShowFaxToGuestUsers { get; set; }
        public bool UserPreferencesShowStreetAddressToGuestUsers { get; set; }
        public bool UserPreferencesLightningExperiencePreferred { get; set; }
        public object ContactId { get; set; }
        public object AccountId { get; set; }
        public object CallCenterId { get; set; }
        public object Extension { get; set; }
        public object FederationIdentifier { get; set; }
        public object AboutMe { get; set; }
        public string FullPhotoUrl { get; set; }
        public string SmallPhotoUrl { get; set; }
        public string DigestFrequency { get; set; }
        public string DefaultGroupNotificationFrequency { get; set; }
        public int JigsawImportLimitOverride { get; set; }
        public DateTime LastViewedDate { get; set; }
        public DateTime LastReferencedDate { get; set; }
        public string BannerPhotoUrl { get; set; }
        public bool IsProfilePhotoActive { get; set; }
    }

    public class UserAttributes
    {
        public string type { get; set; }
        public string url { get; set; }
    }




    public class SfdcSearchUserResponse
    {
        public int totalSize { get; set; }
        public bool done { get; set; }
        public List<Record> records { get; set; }
    }

    public class Record
    {
        public Attributes2 attributes { get; set; }
        public string Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Profile Profile { get; set; }
        public string Username { get; set; }
        public bool IsActive { get; set; }
    }

    public class Attributes2
    {
        public string type { get; set; }
        public string url { get; set; }
    }

    public class Profile
    {
        public Attributes1 attributes { get; set; }
        public string Name { get; set; }
    }

    public class Attributes1
    {
        public string type { get; set; }
        public string url { get; set; }
    }




    ////response entity for create user

    //    public class SabaCreateResponse
    //    {
    //        [JsonProperty("id")]
    //        public string id { get; set; }

    //        [JsonProperty("displayName")]
    //        public string displayName { get; set; }

    //        [JsonProperty("href")]
    //        public string href { get; set; }
    //    }

    ////response entity for people/searchQuery
    //    public class SabaPeopleResponse
    //    {
    //        [JsonProperty("totalResults")]
    //        public string totalResults { get; set; }

    //        [JsonProperty("startIndex")]
    //        public string startIndex { get; set; }

    //        [JsonProperty("itemsPerPage")]
    //        public string itemsPerPage { get; set; }

    //        [JsonProperty("results")]
    //        public List<SabaUserResponse> results { get; set; }

    //        [JsonProperty("hasMoreResults")]
    //        public string hasMoreResults { get; set; }

    //        [JsonProperty("facets")]
    //        public List<SabaFacetsQueryResponse> facets { get; set; }
    //    }

    //    //subsection of SabaPeopleResponse
    //    public class SabaUserResponse
    //    {
    //        [JsonProperty("fname")]
    //        public string fname { get; set; }

    //        [JsonProperty("lname")]
    //        public string lname { get; set; }

    //        [JsonProperty("username")]
    //        public string username { get; set; }

    //        [JsonProperty("id")]
    //        public string id { get; set; }

    //        [JsonProperty("person_no")]
    //        public string person_no { get; set; }

    //        [JsonProperty("status")]
    //        public string status { get; set; }

    //        [JsonProperty("href")]
    //        public string href { get; set; }
    //    }

    ////subsection of SabaPeopleResponse
    //    public class SabaFacetsQueryResponse
    //    {
    //        [JsonProperty("tbd")]
    //        public string tbd { get; set; }
    //    }


    //    public class ApiGenericResponse
    //    {
    //        [JsonProperty("StatusCode")]
    //        public string StatusCode { get; set; }

    //        [JsonProperty("ReasonPhrase")]
    //        public string ReasonPhrase { get; set; }

    //        [JsonProperty("Version")]
    //        public string Version { get; set; }

    //        [JsonProperty("Content")]
    //        public string Content { get; set; }
    //    }



    ////unused alternate people/search  with POST
    //    public class SabaQueryRequestOuter
    //    {
    //        public SabaQueryRequestInner[] conditions;

    //    }


    //    public class SabaQueryRequestInner
    //    {
    //      [JsonProperty("name")]
    //        public string name { get; set; }

    //      [JsonProperty("myoperator")]
    //        public string myoperator { get; set; }

    //       [JsonProperty("value")]
    //        public string value { get; set; }

    //    }





}