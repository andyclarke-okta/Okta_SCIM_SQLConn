using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace SfdcOPPConn.Exceptions
{

    /// <summary>
    /// An Saba-specific exception
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class SfdcException : Exception
    {
        [JsonConstructor]
        public SfdcException() : base() { }
        public SfdcException(string message) : base(message) { }
        public SfdcException(string message, Exception exception) : base(message, exception) { }
        public SfdcException(SfdcException sabaException) {
            this.ErrorCode = sabaException.ErrorCode;
            this.ErrorMessage = sabaException.ErrorMessage;
            this.ErrorSummary = sabaException.ErrorSummary;
            this.ErrorLink = sabaException.ErrorLink;
            this.ErrorId = sabaException.ErrorId;
            this.ErrorCauses = sabaException.ErrorCauses;
            this.HttpStatusCode = sabaException.HttpStatusCode;
        }

        [JsonProperty("errorCode")]
        public string ErrorCode { get; set; }

        [JsonProperty("errorMessage")]
        public string ErrorMessage { get; set; }

        [JsonProperty("errorSummary")]
        public string ErrorSummary { get; set; }

        [JsonProperty("errorLink")]
        public string ErrorLink { get; set; }

        [JsonProperty("errorId")]
        public string ErrorId { get; set; }

        [JsonProperty("errorCauses")]
        public ErrorCause[] ErrorCauses { get; set; }
        public HttpStatusCode HttpStatusCode { get; set; }
    }


        /// <summary>
    /// Further explanation for why an <see cref="OktaException"/> occurred
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class ErrorCause
    {
        [JsonProperty("errorSummary")]
        public string ErrorSummary { get; set; }
    }

    public class SabaRequestThrottlingException : SfdcException
    {
        [JsonConstructor]
        public SabaRequestThrottlingException() : base() { }
        public SabaRequestThrottlingException(string message) : base(message) { }
        public SabaRequestThrottlingException(string message, Exception exception) : base(message, exception) { }
        public SabaRequestThrottlingException(SfdcException sabaException) : base(sabaException) { }
    }

    public class SabaDuplicateValueForUsernameException : SfdcException
    {
        [JsonConstructor]
        public SabaDuplicateValueForUsernameException() : base() { }
        public SabaDuplicateValueForUsernameException(string message) : base(message) { }
        public SabaDuplicateValueForUsernameException(string message, Exception exception) : base(message, exception) { }
        public SabaDuplicateValueForUsernameException(SfdcException sabaException) : base(sabaException) { }
    }

    public class SabaInvalidPersonTypeException : SfdcException
    {
        [JsonConstructor]
        public SabaInvalidPersonTypeException() : base() { }
        public SabaInvalidPersonTypeException(string message) : base(message) { }
        public SabaInvalidPersonTypeException(string message, Exception exception) : base(message, exception) { }
        public SabaInvalidPersonTypeException(SfdcException sabaException) : base(sabaException) { }
    }

    public class SabaInvalidTimezoneIDException : SfdcException
    {
        [JsonConstructor]
        public SabaInvalidTimezoneIDException() : base() { }
        public SabaInvalidTimezoneIDException(string message) : base(message) { }
        public SabaInvalidTimezoneIDException(string message, Exception exception) : base(message, exception) { }
        public SabaInvalidTimezoneIDException(SfdcException sabaException) : base(sabaException) { }
    }

    public class SabaLocationIdErrorException : SfdcException
    {
        [JsonConstructor]
        public SabaLocationIdErrorException() : base() { }
        public SabaLocationIdErrorException(string message) : base(message) { }
        public SabaLocationIdErrorException(string message, Exception exception) : base(message, exception) { }
        public SabaLocationIdErrorException(SfdcException sabaException) : base(sabaException) { }
    }

    public class SabaManagerIdErrorException : SfdcException
    {
        [JsonConstructor]
        public SabaManagerIdErrorException() : base() { }
        public SabaManagerIdErrorException(string message) : base(message) { }
        public SabaManagerIdErrorException(string message, Exception exception) : base(message, exception) { }
        public SabaManagerIdErrorException(SfdcException sabaException) : base(sabaException) { }
    }

    public class SabaUnknownErrorException : SfdcException
    {
        [JsonConstructor]
        public SabaUnknownErrorException() : base() { }
        public SabaUnknownErrorException(string message) : base(message) { }
        public SabaUnknownErrorException(string message, Exception exception) : base(message, exception) { }
        public SabaUnknownErrorException(SfdcException sabaException) : base(sabaException) { }
    }

    public class SabaGenericInternalServerErrorException : SfdcException
    {
        [JsonConstructor]
        public SabaGenericInternalServerErrorException() : base() { }
        public SabaGenericInternalServerErrorException(string message) : base(message) { }
        public SabaGenericInternalServerErrorException(string message, Exception exception) : base(message, exception) { }
        public SabaGenericInternalServerErrorException(SfdcException sabaException) : base(sabaException) { }
    }

    public class OPPJSONException : SfdcException
    {
        [JsonConstructor]
        public OPPJSONException() : base() { }
        public OPPJSONException(string message) : base(message) { }
        public OPPJSONException(string message, Exception exception) : base(message, exception) { }
        public OPPJSONException(SfdcException sabaException) : base(sabaException) { }
    }

    public class OPPAffiliateNotFoundException : SfdcException
    {
        [JsonConstructor]
        public OPPAffiliateNotFoundException() : base() { }
        public OPPAffiliateNotFoundException(string message) : base(message) { }
        public OPPAffiliateNotFoundException(string message, Exception exception) : base(message, exception) { }
        public OPPAffiliateNotFoundException(SfdcException sabaException) : base(sabaException) { }
    }

    public class OPPTimezoneNotFoundException : SfdcException
    {
        [JsonConstructor]
        public OPPTimezoneNotFoundException() : base() { }
        public OPPTimezoneNotFoundException(string message) : base(message) { }
        public OPPTimezoneNotFoundException(string message, Exception exception) : base(message, exception) { }
        public OPPTimezoneNotFoundException(SfdcException sabaException) : base(sabaException) { }
    }
}


