using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SfdcOPPConn.Exceptions
{
    public class SabaErrorCodes
    {

        ///<summary>Invalid person type entered</summary>
        public const string TooManyRequestsException = "9999999999999999";

        ///<summary>Invalid person type entered</summary>
        public const string SabaDuplicateValueForUsername = "1026";

        ///<summary>A duplicate value for Username was used</summary>
        public const string SabaInvalidPersonType = "130434";

        ///<summary>A duplicate value for Username was used</summary>
        public const string SabaInvalidTimezoneID = "130293";

        ///<summary>An unknown error occurred</summary>
        public const string SabaUnknownError = "99999";
    }
}