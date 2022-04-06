

using System.Web;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using SfdcOPPConn.Exceptions;

namespace SfdcOPPConn.Connectors
{
    public class SfdcUtils
    {
        public static Tuple<string, Uri> ParseLinkHeader(string header)
        {
            if (header == null)
            {
                throw new SfdcException("Cannot parse a null header", new ArgumentNullException("header"));
                
            }

            // Parse this format: <http://rain.okta1.com:1802/api/v1/users?pageSize=10000>; rel="self"

            // Split the header on semicolons
            var split = header.Split(';');

            if (split.Count() == 1)
            {
                //throw new OktaException("The header \"" + header + "\" is formatted improperly");
                
            }

            // Get and sanitize the url
            var url = split[0];
            url = url.Trim('<', '>', ' ');

            if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                //throw new OktaException("The header uri \"" + url + "\" is not an absolute URI");
            
            }

            // Get and sanitize the relation
            var relation = split[1];
            var relationSplit = relation.Split('=');

            if (relationSplit.Count() == 1)
            {
                throw new SfdcException("The header \"" + header + "\" is formatted improperly");
              
            }

            relation = relationSplit[1];
            relation = relation.Trim('"');

            return new Tuple<string, Uri>(relation, new Uri(url));
        }

        private static SfdcJsonConverter sabaJsonConverter = new SfdcJsonConverter();
        public static T Deserialize<T>(string value)
        {
            try
            {
               // string jsonResult = (JsonConvert.DeserializeObject<T>(value, sabaJsonConverter)).ToString();
                return JsonConvert.DeserializeObject<T>(value, sabaJsonConverter);
            }
            catch (Exception e)
            {
                throw new SfdcException("Unable to deserialize the response properly", e);
                //throw e;
            }
        }

        public static T Deserialize<T>(HttpResponseMessage response)
        {
            string content;
            try
            {
                content = response.Content.ReadAsStringAsync().Result;
            }
            catch (Exception e)
            {
                throw new SfdcException("Unable to read the results of the Http response", e);
               // throw;
            }
           // return content;
            return Deserialize<T>(content);
        }


        public static string DeserializeError<T>(HttpResponseMessage response)
        {
            string content;
            try
            {
                content = response.Content.ReadAsStringAsync().Result;
            }
            catch (Exception e)
            {
                throw new SfdcException("Unable to read the results of the Http response", e);
                // throw;
            }
            return content;
            //return Deserialize<T>(content);
        }

        public static object DeserializeObject(string value, Type type)
        {
            try
            {
                string jsonResult = (JsonConvert.DeserializeObject(value.ToString(), type, sabaJsonConverter)).ToString();
                return JsonConvert.DeserializeObject(value.ToString(), type, sabaJsonConverter);
            }
            catch (Exception e)
            {
                throw new SfdcException("Unable to deserialize the response properly", e);
               // throw;
            }
        }

        public static string SerializeObject(object value)
        {
            try
            {
                return JsonConvert.SerializeObject(value, sabaJsonConverter);
            }
            catch (Exception e)
            {
                throw new SfdcException("Unable to serialize the object properly", e);
                //throw;
            }
        }

        internal static string StripPairedQuotes(string s)
        {
            // We only remove quotes if they are a pair
            if (s.Length > 1 && s.StartsWith("\"") && s.EndsWith("\""))
            {
                return s.Remove(s.Length - 1).Remove(0);
            }

            return s;
        }

        internal static string AddPairedQuotes(string s)
        {
            return s.Insert(s.Length, "\"").Insert(0, "\"");
        }

        public static string BuildUrlParams(Dictionary<string, object> urlParams)
        {
            if (urlParams == null || urlParams.Count < 1)
            {
                return "";
            }

            var paramList = new List<string>();
            foreach (var kvp in urlParams)
            {
                paramList.Add(kvp.Key + "=" + kvp.Value.ToString());
            }
            return "?" + string.Join("&", paramList);
        }

        public static void Sleep(int milliseconds)
        {
            if (milliseconds > 0)
            {
                // Cross platform sleep
                using (var mre = new ManualResetEvent(false))
                {
                    mre.WaitOne(milliseconds);
                }
            }
        }

        public static string GetAssemblyVersion()
        {
            var regex = new Regex(@"Version=[\d\.]*");
            var fullAssemblyName = typeof(SfdcUtils).Assembly.FullName;
            var match = regex.Match(fullAssemblyName);
            if (match.Success)
            {
                return match.Value.Split('=')[1];
            }
            else
            {
                return String.Empty;
            }
        }




    }
}