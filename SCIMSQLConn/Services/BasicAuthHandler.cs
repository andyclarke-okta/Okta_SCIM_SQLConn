using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace SCIMSQLConn.Services
{
    public class BasicAuthHandler : DelegatingHandler
    {
        public string BasicRealm { get; set; }
        protected string userName { get; set; }
        protected string password { get; set; }

        public BasicAuthHandler(string username, string password)
        {
            this.userName = username;
            this.password = password;
        }


        //Method to validate credentials from Authorization
        //header value
        private bool ValidateCredentials(AuthenticationHeaderValue authenticationHeaderVal)
        {
            try
            {
                if (authenticationHeaderVal != null && !String.IsNullOrEmpty(authenticationHeaderVal.Parameter))
                {
                    string[] decodedCredentials = System.Text.ASCIIEncoding.ASCII.GetString(Convert.FromBase64String(authenticationHeaderVal.Parameter)).Split(new[] { ':' });

                    //now decodedCredentials[0] will contain
                    //username and decodedCredentials[1] will
                    //contain password.

                    if (decodedCredentials[0].Equals(userName)
                    && decodedCredentials[1].Equals(password))
                    {
                        //userName = "andy.clarke";
                        return true;//request authenticated.
                    }
                }
                return false;//request not authenticated.
            }
            catch
            {
                return false;
            }
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            //if the credentials are validated,
            //set CurrentPrincipal and Current.User
            if (ValidateCredentials(request.Headers.Authorization))
            {
                Thread.CurrentPrincipal = new ApiPrincipal(userName);
                HttpContext.Current.User = new ApiPrincipal(userName);
            }
            //Execute base.SendAsync to execute default
            //actions and once it is completed,
            //capture the response object and add
            //WWW-Authenticate header if the request
            //was marked as unauthorized.

            //Allow the request to process further down the pipeline
            var response = await base.SendAsync(request, cancellationToken);

            if (response.StatusCode == HttpStatusCode.Unauthorized
                && !response.Headers.Contains("WwwAuthenticate"))
            {
                response.Headers.Add("WwwAuthenticate", "Basic");
            }

            return response;
        }


    }
}