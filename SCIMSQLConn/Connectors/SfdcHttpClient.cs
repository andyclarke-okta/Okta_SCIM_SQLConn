using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
//using Okta.Core;
using SfdcOPPConn.Models;
using SfdcOPPConn.Exceptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Utilities;
using System.IO;
using System.Net.Http.Headers;
using log4net;


namespace SfdcOPPConn.Connectors
{
    public class SfdcHttpClient : ISfdcHttpClient
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static HttpClientHandler handler = new HttpClientHandler();
       //{
       //    AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
       //};


       //        private HttpClient httpClient = new HttpClient(handler)
        private HttpClient httpClient = new HttpClient()
        {
            Timeout = SfdcConstants.DefaultTimeout
        };

       // public SabaContext localContext { get; set; }

        public override Uri BaseUri
        {
            get { return httpClient.BaseAddress; }
            set { httpClient.BaseAddress = value; }
        }

     

     //   public SabaHttpClient(OktaSettings oktaSettings)
        public SfdcHttpClient()
        {
           // httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
          //  localContext = context;
        }

        public override HttpResponseMessage Execute(HttpRequestType requestType, Uri uri = null, string relativeUri = null, string content = null, int waitMillis = 0, int retryCount = 0)
        {
            try
            {
                var response = ExecuteAsync(requestType, uri, relativeUri, content, waitMillis).Result;

                // Handle any errors
                try
                {
                    SabaExceptionResolver.ParseHttpResponse(response);
                }
                catch (SabaDuplicateValueForUsernameException e)
                {
                 //   logger.Debug("Error Duplicate username " + e.ErrorMessage);
                    throw e;
                }
                catch (SabaUnknownErrorException e)
                {
                 //   logger.Debug("Saba Unknown Error " + e.ErrorMessage);
                    throw e;
                }
                catch (SabaLocationIdErrorException e)
                {
                 //   logger.Debug("Saba Location Error " + e.ErrorMessage);
                    throw e;
                }
                catch (SabaManagerIdErrorException e)
                {
                    //   logger.Debug("Saba Location Error " + e.ErrorMessage);
                    throw e;
                }
                catch ( SabaInvalidTimezoneIDException e)
                {
                    //   logger.Debug("Saba Timezone Error " + e.ErrorMessage);
                    throw e;
                }
                // If it's a rate-limiting error
                catch (SabaRequestThrottlingException e)
                {
                    // If we haven't met the retry threshold
                    if (waitMillis < SfdcConstants.MaxThrottlingRetryMillis * 1000)
                    {
                        // If this is our second retry, then we need to scale back
                        if (retryCount > 0)
                        {
                            // Use exponential backoff
                            int millis = (int)Math.Pow(2, retryCount) * 1000;
                            return Execute(requestType, uri, relativeUri, content, millis, retryCount: retryCount++);
                        }
                        else
                        {
                            // Determine the number of milliseconds to wait using the header
                            IEnumerable<string> resetValues;
                            if (!response.Headers.TryGetValues("X-Rate-Limit-Reset", out resetValues))
                            {
                                // TODO: Log that we were unable to get the reset pageSize
                                throw e;
                            }
                            else
                            {
                                // Parse the string header to an int
                                var waitUntilString = resetValues.FirstOrDefault();
                                int waitUntilUnixTime;
                                if (!int.TryParse(waitUntilString, out waitUntilUnixTime))
                                {
                                    // TODO: Log that we were unable to convert the header
                                    throw e;
                                }
                                else
                                {
                                    // See how long until we hit that time
                                    var unixTime = (Int64)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
                                    var millisToWait = unixTime - ((Int64)waitUntilUnixTime * 1000);

                                    if (millisToWait > Int32.MaxValue)
                                    {
                                        // TODO: Log that we miscalculated the wait time
                                        throw e;
                                    }

                                    // Then attempt to send the request again
                                    return Execute(requestType, uri, relativeUri, content, (int)millisToWait, retryCount: 1);
                                }
                            }
                        }
                    }
                    else
                    {
                        // TODO: Log that there are too many requests queued
                        throw e;
                    }
                }
                catch (SabaGenericInternalServerErrorException e)
                {
                    logger.Debug("Error Generic Internal Server => " + e.ErrorCode);
                    throw e;
                }
                // If there were no errors, just return
                return response;
            }
            catch (SfdcException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new SfdcException("Error making an HTTP request: " + e.Message, e);
            }
        }

        public override Task<HttpResponseMessage> ExecuteAsync(HttpRequestType requestType, Uri uri = null, string relativeUri = null, string content = null, int waitMillis = 1000)
        {
            // Ensure we have exactly one useable Uri
            if (String.IsNullOrEmpty(relativeUri) && uri == null)
            {
                throw new SfdcException("Cannot execute an Http request without a Uri");
            }
            else if (!String.IsNullOrEmpty(relativeUri) && uri != null)
            {
                throw new SfdcException("Http request is ambiguous: cannot determine whether to execute " + uri.ToString() + " or " + relativeUri);
            }

            try
            {
                // Wait
                SfdcUtils.Sleep(waitMillis);

                // Handle GETs
                if (requestType == HttpRequestType.GET)
                {
                    if (uri != null)
                    {
                        return httpClient.GetAsync(uri);
                    }
                    else
                    {
                        return httpClient.GetAsync(relativeUri);
                    }
                }

                // Handle GETAuth
                if (requestType == HttpRequestType.GetAuth)
                {
                    if (uri != null)
                    {
                        // Add a new Request Message
                        HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
                        //requestMessage.Headers.Add("user", SabaContext.username);
                        //requestMessage.Headers.Add("password", SabaContext.password);
                        return httpClient.SendAsync(requestMessage);

                        // return httpClient.GetAsync(uri);
                    }
                    else
                    {
                        return httpClient.GetAsync(relativeUri);
                    }
                }
                // Handle GETSpecial
                if (requestType == HttpRequestType.GETSpecial)
                {
                    if (uri != null)
                    {
                        // Add a new Request Message
                        HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
                        requestMessage.Headers.Add("Authorization",  "Bearer  " + SfdcContext.bearerToken);
                       // requestMessage.Headers.Add("ExpectContinue", "false");
                        return httpClient.SendAsync(requestMessage);

                       // return httpClient.GetAsync(uri);
                    }
                    else
                    {
                        return httpClient.GetAsync(relativeUri);
                    }
                }
                // Handle POSTToken
                if (requestType == HttpRequestType.POSTToken)
                {
                    if (uri != null)
                    {
                        // Add a new Request Message
                        HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, uri);
                        //requestMessage.Headers.Add("user", SabaContext.username);
                        //requestMessage.Headers.Add("password", SabaContext.password);
                        return httpClient.SendAsync(requestMessage);

                        // return httpClient.GetAsync(uri);
                    }
                    else
                    {
                        return httpClient.GetAsync(relativeUri);
                    }
                }
                // Handle POSTs
                else if (requestType == HttpRequestType.POST)
                {
                    content = content ?? "";
                    if (uri != null)
                    {
                        return httpClient.PostAsync(uri, new StringContent(content, Encoding.UTF8, "application/json"));
                    }
                    else
                    {
                        return httpClient.PostAsync(relativeUri, new StringContent(content, Encoding.UTF8, "application/json"));
                    }
                }
                // Handle POSTSpecial
                else if (requestType == HttpRequestType.POSTSpecial)
                {
                    content = content ?? "";
                    if (uri != null)
                    {
                        // Add a new Request Message
                        HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, uri);
                       // requestMessage.Headers.Add("SabaCertificate", SfdcContext.sabaCert);
                        requestMessage.Headers.Add("Authorization", "Bearer  " + SfdcContext.bearerToken);
                      //  requestMessage.Headers.Add("ExpectContinue", "false");


                        requestMessage.Content = new StringContent(content, Encoding.UTF8, "application/json");
                        try
                        {
                            var rspClient = httpClient.SendAsync(requestMessage);

                            return rspClient;
                        }
                        catch (SfdcException e)
                        {
                            
                            throw e;
                        }

                    }
                    else
                    {
                        //should not get here
                        return httpClient.PostAsync(relativeUri, new StringContent(content, Encoding.UTF8, "application/json"));

                    }
                }

                // Handle PUTs
                else if (requestType == HttpRequestType.PUT)
                {
                    content = content ?? "";
                    if (uri != null)
                    {
                        return httpClient.PutAsync(uri, new StringContent(content, Encoding.UTF8, "application/json"));
                    }
                    else
                    {
                        return httpClient.PutAsync(relativeUri, new StringContent(content, Encoding.UTF8, "application/json"));
                    }
                }
                // Handle PUTSpecial
                else if (requestType == HttpRequestType.PUTSpecial)
                {
                    content = content ?? "";
                    if (uri != null)
                    {
                        // Add a new Request Message
                        HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Put, uri);
               //         requestMessage.Headers.Add("SabaCertificate", SfdcContext.sabaCert);
                        requestMessage.Headers.Add("Authorization", "Bearer  " + SfdcContext.bearerToken);
                       // requestMessage.Headers.Add("ExpectContinue", "false");



                        requestMessage.Content = new StringContent(content, Encoding.UTF8, "application/json");
                        //HttpContent  myContent= new StringContent(content);
                        //myContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                        //requestMessage.Content = myContent;

                        return httpClient.SendAsync(requestMessage);
                    }
                    else
                    {
                        //should not get here
                        return httpClient.PutAsync(relativeUri, new StringContent(content, Encoding.UTF8, "application/json"));
                    }
                }
                // Handle PATCHs
                else if (requestType == HttpRequestType.PATCH)
                {
                    content = content ?? "";
                    if (uri != null)
                    {
                        return httpClient.PostAsync (uri, new StringContent(content, Encoding.UTF8, "application/json"));
                    }
                    else
                    {
                        return httpClient.PostAsync(relativeUri, new StringContent(content, Encoding.UTF8, "application/json"));
                    }
                }
                // Handle PATCHSpecial
                else if (requestType == HttpRequestType.PATCHSpecial)
                {
                    content = content ?? "";
                    if (uri != null)
                    {
                        // Add a new Request Message
                        var patchMethod = new HttpMethod("PATCH");
                        HttpRequestMessage requestMessage = new HttpRequestMessage(patchMethod, uri);
                        // requestMessage.Headers.Add("SabaCertificate", SfdcContext.sabaCert);
                        requestMessage.Headers.Add("Authorization", "Bearer  " + SfdcContext.bearerToken);
                        //  requestMessage.Headers.Add("ExpectContinue", "false");


                        requestMessage.Content = new StringContent(content, Encoding.UTF8, "application/json");
                        try
                        {
                            var rspClient = httpClient.SendAsync(requestMessage);

                            return rspClient;
                        }
                        catch (SfdcException e)
                        {

                            throw e;
                        }

                    }
                    else
                    {
                        //should not get here
                        return httpClient.PostAsync(relativeUri, new StringContent(content, Encoding.UTF8, "application/json"));

                    }
                }
                // Handle DELETEs
                else if (requestType == HttpRequestType.DELETE)
                {
                    if (uri != null)
                    {
                        return httpClient.DeleteAsync(uri);
                    }
                    else
                    {
                        return httpClient.DeleteAsync(relativeUri);
                    }
                }

                else
                {
                    throw new SfdcException("The " + requestType.ToString() + " http verb is not yet supported");
                }
            }
            catch (SfdcException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new SfdcException("Error making an HTTP request: " + e.Message, e);
            }
        }

    }
}