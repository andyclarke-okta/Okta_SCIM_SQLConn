using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using log4net;
using SfdcOPPConn.Connectors;

namespace SfdcOPPConn.Exceptions
{
    public class SabaExceptionResolver
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void ParseHttpResponse(HttpResponseMessage httpResponseMessage)
        {

            if (httpResponseMessage == null)
            {
                throw new SfdcException("SabaExceptionResolver.ParseHttpResponse has invalid arguments", new ArgumentNullException("httpResponseMessage"));
            }
            //handle known error types
            if (!SfdcConstants.AcceptableHttpStatusCodes.Contains(httpResponseMessage.StatusCode))
            {
                var exception = SfdcUtils.Deserialize<SfdcException>(httpResponseMessage);
                exception.HttpStatusCode = httpResponseMessage.StatusCode;
                if (exception.ErrorCode == SabaErrorCodes.TooManyRequestsException)
                {
                    throw new SabaRequestThrottlingException(exception);
                }

                else if (exception.ErrorCode == SabaErrorCodes.SabaDuplicateValueForUsername)
                {
                    logger.Debug(" found duplicate username ");

                    throw exception;
                }
                else if (exception.ErrorCode == SabaErrorCodes.SabaInvalidPersonType)
                {
                    logger.Debug(" found invalid person_type ");

                    throw exception;
                }
                else if (exception.ErrorCode == SabaErrorCodes.SabaInvalidTimezoneID)
                {
                    logger.Debug(" found invalid timezone id ");

                    throw exception;
                }
                else if (exception.ErrorCode == SabaErrorCodes.SabaUnknownError)
                {
                    logger.Debug(" An unknown error occurred");

                    string tempResult = SfdcUtils.DeserializeError<SfdcException>(httpResponseMessage);
                    if (exception.ErrorMessage.IndexOf("location_id", 0) != -1)
                    {
                        logger.Debug(" error concerning location_id ");
                        throw new SabaLocationIdErrorException(exception);
                    }
                    if (exception.ErrorMessage.IndexOf("manager_id", 0) != -1)
                    {
                        logger.Debug(" error concerning manager_id ");
                        throw new SabaManagerIdErrorException(exception);
                    }
                    throw new SabaUnknownErrorException(exception);
                }
                else
                {
                    logger.Debug(" got Internal Server Error");
                    if(exception.ErrorMessage.IndexOf("location_id", 0)  != -1)
                    {
                        logger.Debug(" error concerning location_id ");
                        throw new SabaLocationIdErrorException(exception);
                    }
                    if (exception.ErrorMessage.IndexOf("manager_id", 0) != -1)
                    {
                        logger.Debug(" error concerning manager_id ");
                        throw new SabaManagerIdErrorException(exception);
                    }
                    throw new SabaGenericInternalServerErrorException(exception);
                   
                }
            }
        }
    }
}