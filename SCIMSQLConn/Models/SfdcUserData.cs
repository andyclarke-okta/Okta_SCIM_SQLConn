using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SfdcOPPConn.Models
{

    public class SfdcUserBasic
    {
        public string Username { get; set; }
        public string PrimaryEmail { get; set; }
        public string SecondaryEmail { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Id { get; set; }
        public string MobilePhone { get; set; }
        public string PrimaryPhone { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Organization { get; set; }
        public string is_clinician { get; set; }
        public string profession { get; set; }
        public string specialty { get; set; }
        public string years_practicing { get; set; }
        public string tctmd_newsletter { get; set; }
        public string crf_newsletter { get; set; }
        public string tct_newsletter { get; set; }
        //recent add
        public string tctmdSubscriptionLevel { get; set; }
        public string OktaInternalId { get; set; }
    }
    public  class SfdcUserUpdate
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Secondary_Email__c { get; set; }
        public string Company_Name__c { get; set; }
        public string Clinician__c { get; set; }
        public string Profession__c { get; set; }
        public string Specialty__c { get; set; }
        public string Years_Practicing__c { get; set; }
        public string TCTMD_Newsletter__c { get; set; }
        public string CRF_Conference_News__c { get; set; }
        public string TCT_Newsletter__c { get; set; }
        //recent add
        public string TCTMD_Subscription_ID__c { get; set; }
        public string Okta_Id__c { get; set; }

    }



    //  public Address Address { get; set; }

    public class Address
    {
    public string city { get; set; }
    public string country { get; set; }
    public object countryCode { get; set; }
    public object geocodeAccuracy { get; set; }
    public object latitude { get; set; }
    public object longitude { get; set; }
    public string postalCode { get; set; }
    public string state { get; set; }
    public object stateCode { get; set; }
    public string street { get; set; }
    }


//public class Rootobject
//{
//public Address Address { get; set; }
//}


   // {
      //  public UserAttributes attributes { get; set; }
      //  public string Id { get; set; }
        //public string Username { get; set; }
 //       public string LastName { get; set; }
 //       public string FirstName { get; set; }
//        public string Name { get; set; }
//        public string CompanyName { get; set; }
        //public object Division { get; set; }
        //public object Department { get; set; }
        //public object Title { get; set; }
//        public string Street { get; set; }
//        public string City { get; set; }
//        public string State { get; set; }
//        public string PostalCode { get; set; }
//        public string Country { get; set; }
        //public object Latitude { get; set; }
        //public object Longitude { get; set; }
        //public object GeocodeAccuracy { get; set; }
        //public object Address { get; set; }
//        public string Email { get; set; }
        //public bool EmailPreferencesAutoBcc { get; set; }
        //public bool EmailPreferencesAutoBccStayInTouch { get; set; }
        //public bool EmailPreferencesStayInTouchReminder { get; set; }
        //public object SenderEmail { get; set; }
        //public object SenderName { get; set; }
        //public object Signature { get; set; }
        //public object StayInTouchSubject { get; set; }
        //public object StayInTouchSignature { get; set; }
        //public object StayInTouchNote { get; set; }
//        public object Phone { get; set; }
        //public object Fax { get; set; }
        //public object MobilePhone { get; set; }
        //public string Alias { get; set; }
        //public string CommunityNickname { get; set; }
        //public string BadgeText { get; set; }
        //public bool IsActive { get; set; }
        //public string TimeZoneSidKey { get; set; }
        //public object UserRoleId { get; set; }
        //public string LocaleSidKey { get; set; }
        //public bool ReceivesInfoEmails { get; set; }
        //public bool ReceivesAdminInfoEmails { get; set; }
        //public string EmailEncodingKey { get; set; }
        //public string ProfileId { get; set; }
        //public string UserType { get; set; }
        //public string LanguageLocaleKey { get; set; }
        //public object EmployeeNumber { get; set; }
        //public object DelegatedApproverId { get; set; }
        //public object ManagerId { get; set; }
        //public DateTime LastLoginDate { get; set; }
        //public DateTime LastPasswordChangeDate { get; set; }
        //public DateTime CreatedDate { get; set; }
        //public string CreatedById { get; set; }
        //public DateTime LastModifiedDate { get; set; }
        //public string LastModifiedById { get; set; }
        //public DateTime SystemModstamp { get; set; }
        //public object OfflineTrialExpirationDate { get; set; }
        //public object OfflinePdaTrialExpirationDate { get; set; }
        //public bool UserPermissionsMarketingUser { get; set; }
        //public bool UserPermissionsOfflineUser { get; set; }
        //public bool UserPermissionsCallCenterAutoLogin { get; set; }
        //public bool UserPermissionsMobileUser { get; set; }
        //public bool UserPermissionsSFContentUser { get; set; }
        //public bool UserPermissionsKnowledgeUser { get; set; }
        //public bool UserPermissionsInteractionUser { get; set; }
        //public bool UserPermissionsSupportUser { get; set; }
        //public bool UserPermissionsJigsawProspectingUser { get; set; }
        //public bool UserPermissionsSiteforceContributorUser { get; set; }
        //public bool UserPermissionsSiteforcePublisherUser { get; set; }
        //public bool UserPermissionsChatterAnswersUser { get; set; }
        //public bool UserPermissionsWorkDotComUserFeature { get; set; }
        //public bool ForecastEnabled { get; set; }
        //public bool UserPreferencesActivityRemindersPopup { get; set; }
        //public bool UserPreferencesEventRemindersCheckboxDefault { get; set; }
        //public bool UserPreferencesTaskRemindersCheckboxDefault { get; set; }
        //public bool UserPreferencesReminderSoundOff { get; set; }
        //public bool UserPreferencesDisableAllFeedsEmail { get; set; }
        //public bool UserPreferencesDisableFollowersEmail { get; set; }
        //public bool UserPreferencesDisableProfilePostEmail { get; set; }
        //public bool UserPreferencesDisableChangeCommentEmail { get; set; }
        //public bool UserPreferencesDisableLaterCommentEmail { get; set; }
        //public bool UserPreferencesDisProfPostCommentEmail { get; set; }
        //public bool UserPreferencesContentNoEmail { get; set; }
        //public bool UserPreferencesContentEmailAsAndWhen { get; set; }
        //public bool UserPreferencesApexPagesDeveloperMode { get; set; }
        //public bool UserPreferencesHideCSNGetChatterMobileTask { get; set; }
        //public bool UserPreferencesDisableMentionsPostEmail { get; set; }
        //public bool UserPreferencesDisMentionsCommentEmail { get; set; }
        //public bool UserPreferencesHideCSNDesktopTask { get; set; }
        //public bool UserPreferencesHideChatterOnboardingSplash { get; set; }
        //public bool UserPreferencesHideSecondChatterOnboardingSplash { get; set; }
        //public bool UserPreferencesDisCommentAfterLikeEmail { get; set; }
        //public bool UserPreferencesDisableLikeEmail { get; set; }
        //public bool UserPreferencesDisableMessageEmail { get; set; }
        //public bool UserPreferencesJigsawListUser { get; set; }
        //public bool UserPreferencesDisableBookmarkEmail { get; set; }
        //public bool UserPreferencesDisableSharePostEmail { get; set; }
        //public bool UserPreferencesEnableAutoSubForFeeds { get; set; }
        //public bool UserPreferencesDisableFileShareNotificationsForApi { get; set; }
        //public bool UserPreferencesShowTitleToExternalUsers { get; set; }
        //public bool UserPreferencesShowManagerToExternalUsers { get; set; }
        //public bool UserPreferencesShowEmailToExternalUsers { get; set; }
        //public bool UserPreferencesShowWorkPhoneToExternalUsers { get; set; }
        //public bool UserPreferencesShowMobilePhoneToExternalUsers { get; set; }
        //public bool UserPreferencesShowFaxToExternalUsers { get; set; }
        //public bool UserPreferencesShowStreetAddressToExternalUsers { get; set; }
        //public bool UserPreferencesShowCityToExternalUsers { get; set; }
        //public bool UserPreferencesShowStateToExternalUsers { get; set; }
        //public bool UserPreferencesShowPostalCodeToExternalUsers { get; set; }
        //public bool UserPreferencesShowCountryToExternalUsers { get; set; }
        //public bool UserPreferencesShowProfilePicToGuestUsers { get; set; }
        //public bool UserPreferencesShowTitleToGuestUsers { get; set; }
        //public bool UserPreferencesShowCityToGuestUsers { get; set; }
        //public bool UserPreferencesShowStateToGuestUsers { get; set; }
        //public bool UserPreferencesShowPostalCodeToGuestUsers { get; set; }
        //public bool UserPreferencesShowCountryToGuestUsers { get; set; }
        //public bool UserPreferencesDisableFeedbackEmail { get; set; }
        //public bool UserPreferencesDisableWorkEmail { get; set; }
        //public bool UserPreferencesHideS1BrowserUI { get; set; }
        //public bool UserPreferencesDisableEndorsementEmail { get; set; }
        //public bool UserPreferencesPathAssistantCollapsed { get; set; }
        //public bool UserPreferencesCacheDiagnostics { get; set; }
        //public bool UserPreferencesShowEmailToGuestUsers { get; set; }
        //public bool UserPreferencesShowManagerToGuestUsers { get; set; }
        //public bool UserPreferencesShowWorkPhoneToGuestUsers { get; set; }
        //public bool UserPreferencesShowMobilePhoneToGuestUsers { get; set; }
        //public bool UserPreferencesShowFaxToGuestUsers { get; set; }
        //public bool UserPreferencesShowStreetAddressToGuestUsers { get; set; }
        //public bool UserPreferencesLightningExperiencePreferred { get; set; }
        //public object ContactId { get; set; }
        //public object AccountId { get; set; }
        //public object CallCenterId { get; set; }
        //public object Extension { get; set; }
        //public object FederationIdentifier { get; set; }
        //public object AboutMe { get; set; }
        //public string FullPhotoUrl { get; set; }
        //public string SmallPhotoUrl { get; set; }
        //public string DigestFrequency { get; set; }
        //public string DefaultGroupNotificationFrequency { get; set; }
        //public int JigsawImportLimitOverride { get; set; }
        //public DateTime LastViewedDate { get; set; }
        //public DateTime LastReferencedDate { get; set; }
        //public string BannerPhotoUrl { get; set; }
        //public bool IsProfilePhotoActive { get; set; }

    //}



}