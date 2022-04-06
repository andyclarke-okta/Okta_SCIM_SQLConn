using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Threading.Tasks;

namespace SfdcOPPConn.Connectors
{
    public abstract class ISfdcHttpClient
    {

        public enum HttpRequestType
        {
            GET,
            GETSpecial,
            POSTToken,
            GetAuth,
            PUT,
            PUTSpecial,
            POST,
            POSTSpecial,
            PATCH,
            PATCHSpecial,
            DELETE
        }

        public virtual string ApiToken { get; set; }
        public virtual Uri BaseUri { get; set; }

        public abstract HttpResponseMessage Execute(HttpRequestType requestType, Uri uri = null, string relativeUri = null, string content = null, int waitMillis = 0, int retryCount = 0);
        public abstract Task<HttpResponseMessage> ExecuteAsync(HttpRequestType requestType, Uri uri = null, string relativeUri = null, string content = null, int waitMillis = 0);

        #region GET methods
        public HttpResponseMessage Get(string relativeUri)
        {
            return Execute(HttpRequestType.GET, relativeUri: relativeUri);
        }

        public Task<HttpResponseMessage> GetAsync(string relativeUri)
        {
            return ExecuteAsync(HttpRequestType.GET, relativeUri: relativeUri);
        }

        public HttpResponseMessage Get(Uri uri)
        {
            return Execute(HttpRequestType.GET, uri: uri);
        }

        public Task<HttpResponseMessage> GetAsync(Uri uri)
        {
            return ExecuteAsync(HttpRequestType.GET, uri: uri);
        }

        public HttpResponseMessage GetSpecial(Uri uri)
        {
            return Execute(HttpRequestType.GETSpecial, uri: uri);
        }

        public HttpResponseMessage PostToken(Uri uri)
        {
            return Execute(HttpRequestType.POSTToken, uri: uri, waitMillis: 1000);
        }

        public HttpResponseMessage GetAuth(Uri uri)
        {
            return Execute(HttpRequestType.GetAuth, uri: uri, waitMillis: 1000);
        }


        public Task<HttpResponseMessage> GetSpecialAsync(Uri uri)
        {
            return ExecuteAsync(HttpRequestType.GETSpecial, uri: uri);
        }
        #endregion

        #region POST methods
        public HttpResponseMessage Post(string relativeUri, string content = null)
        {
            return Execute(HttpRequestType.POST, relativeUri: relativeUri, content: content);
        }

        public Task<HttpResponseMessage> PostAsync(string relativeUri, string content = null)
        {
            return ExecuteAsync(HttpRequestType.POST, relativeUri: relativeUri, content: content);
        }

        public HttpResponseMessage Post(Uri uri, string content)
        {
            return Execute(HttpRequestType.POST, uri: uri, content: content);
        }

        public Task<HttpResponseMessage> PostAsync(Uri uri, string content)
        {
            return ExecuteAsync(HttpRequestType.POST, uri: uri, content: content);
        }
//ajc debug
        public HttpResponseMessage PostSpecial(Uri uri, string content)
        {
            return Execute(HttpRequestType.POSTSpecial, uri: uri, content: content);
        }

        public Task<HttpResponseMessage> PostSpecialAsync(Uri uri, string content)
        {
            return ExecuteAsync(HttpRequestType.POSTSpecial, uri: uri, content: content);
        }
        #endregion

        #region PUT methods
        public HttpResponseMessage Put(string relativeUri, string content = null)
        {
            return Execute(HttpRequestType.PUT, relativeUri: relativeUri, content: content);
        }

        public Task<HttpResponseMessage> PutAsync(string relativeUri, string content = null)
        {
            return ExecuteAsync(HttpRequestType.PUT, relativeUri: relativeUri, content: content);
        }

        public HttpResponseMessage Put(Uri uri, string content = null)
        {
            return Execute(HttpRequestType.PUT, uri: uri, content: content);
        }

        public Task<HttpResponseMessage> PutAsync(Uri uri, string content = null)
        {
            return ExecuteAsync(HttpRequestType.PUT, uri: uri, content: content);
        }

        public HttpResponseMessage PutSpecial(Uri uri, string content = null)
        {
            return Execute(HttpRequestType.PUTSpecial, uri: uri, content: content);
        }

        public Task<HttpResponseMessage> PutSpecialAsync(Uri uri, string content = null)
        {
            return ExecuteAsync(HttpRequestType.PUT, uri: uri, content: content);
        }
        #endregion

        #region DELETE methods
        public HttpResponseMessage Delete(string relativeUri)
        {
            return Execute(HttpRequestType.DELETE, relativeUri: relativeUri);
        }

        public Task<HttpResponseMessage> DeleteAsync(string relativeUri)
        {
            return ExecuteAsync(HttpRequestType.DELETE, relativeUri: relativeUri);
        }

        public HttpResponseMessage Delete(Uri uri)
        {
            return Execute(HttpRequestType.DELETE, uri: uri);
        }

        public Task<HttpResponseMessage> DeleteAsync(Uri uri)
        {
            return ExecuteAsync(HttpRequestType.DELETE, uri: uri);
        }
        #endregion

        #region PATCH methods
        public HttpResponseMessage Patch(string relativeUri, string content = null)
        {
            return Execute(HttpRequestType.PATCH, relativeUri: relativeUri, content: content);
        }

        public Task<HttpResponseMessage> PatchAsync(string relativeUri, string content = null)
        {
            return ExecuteAsync(HttpRequestType.PATCH, relativeUri: relativeUri, content: content);
        }

        public HttpResponseMessage Patch(Uri uri, string content)
        {
            return Execute(HttpRequestType.PATCH, uri: uri, content: content);
        }

        public Task<HttpResponseMessage> PatchAsync(Uri uri, string content)
        {
            return ExecuteAsync(HttpRequestType.PATCH, uri: uri, content: content);
        }
        //ajc debug
        public HttpResponseMessage PatchSpecial(Uri uri, string content)
        {
            return Execute(HttpRequestType.PATCHSpecial, uri: uri, content: content);
        }

        public Task<HttpResponseMessage> PatchSpecialAsync(Uri uri, string content)
        {
            return ExecuteAsync(HttpRequestType.PATCHSpecial, uri: uri, content: content);
        }
        #endregion



    }
}