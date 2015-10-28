using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Authentication;
using System.Web;
using EasyHttp.Http;
using EasyHttp.Infrastructure;
using HttpException = EasyHttp.Infrastructure.HttpException;
using HttpResponse = EasyHttp.Http.HttpResponse;

namespace FluentTc
{
    internal interface ITeamCityCaller
    {
        T GetFormat<T>(string urlPart, params object[] parts);

        void GetFormat(string urlPart, params object[] parts);

        T PostFormat<T>(object data, string contenttype, string accept, string urlPart, params object[] parts);

        void PostFormat(object data, string contenttype, string urlPart, params object[] parts);

        void PutFormat(object data, string contenttype, string urlPart, params object[] parts);

        void DeleteFormat(string urlPart, params object[] parts);

        void GetDownloadFormat(Action<string> downloadHandler, string urlPart, params object[] parts);

        string StartBackup(string urlPart);

        T Get<T>(string urlPart);

        void Get(string urlPart);

        T Post<T>(string data, string contenttype, string urlPart, string accept);

        bool Authenticate(string urlPart);

        HttpResponse Post(object data, string contenttype, string urlPart, string accept);

        HttpResponse Put(object data, string contenttype, string urlPart, string accept);

        void Delete(string urlPart);

        string GetRaw(string urlPart);
        T GetByFullUrl<T>(string fullUrl);
    }

    internal class TeamCityCaller : ITeamCityCaller
    {
        private readonly ITeamCityConnectionDetails m_TeamCityConnectionDetails;
        private readonly IHttpClientWrapperFactory m_HttpClientWrapperFactory;

        public TeamCityCaller(ITeamCityConnectionDetails teamCityConnectionDetails, IHttpClientWrapperFactory httpClientWrapperFactory)
        {
            m_TeamCityConnectionDetails = teamCityConnectionDetails;
            m_HttpClientWrapperFactory = httpClientWrapperFactory;
        }

        public T GetFormat<T>(string urlPart, params object[] parts)
        {
            return Get<T>(string.Format(urlPart, parts));
        }

        public void GetFormat(string urlPart, params object[] parts)
        {
            Get(string.Format(urlPart, parts));
        }

        public virtual T PostFormat<T>(object data, string contenttype, string accept, string urlPart, params object[] parts)
        {
            return Post<T>(data.ToString(), contenttype, string.Format(urlPart, parts), accept);
        }

        public virtual void PostFormat(object data, string contenttype, string urlPart, params object[] parts)
        {
            Post(data.ToString(), contenttype, string.Format(urlPart, parts), string.Empty);
        }

        public virtual void PutFormat(object data, string contenttype, string urlPart, params object[] parts)
        {
            Put(data.ToString(), contenttype, string.Format(urlPart, parts), string.Empty);
        }

        public virtual void DeleteFormat(string urlPart, params object[] parts)
        {
            Delete(string.Format(urlPart, parts));
        }

        public void GetDownloadFormat(Action<string> downloadHandler, string urlPart, params object[] parts)
        {
            if (CheckForUserNameAndPassword())
            {
                throw new ArgumentException("If you are not acting as a guest you must supply userName and password");
            }

            if (string.IsNullOrEmpty(urlPart))
            {
                throw new ArgumentException("Url must be specfied");
            }

            if (urlPart.Contains("+"))
            {
                urlPart = System.Web.HttpUtility.UrlEncode(urlPart);
            }

            if (downloadHandler == null)
            {
                throw new ArgumentException("A download handler must be specfied.");
            }

            string tempFileName = Path.GetRandomFileName();
            var url = CreateUrl(string.Format(urlPart, parts));

            try
            {
                var httpResponse = CreateHttpClient(m_TeamCityConnectionDetails.Username, m_TeamCityConnectionDetails.Password, HttpContentTypes.ApplicationJson).GetAsFile(url, tempFileName);
                if (httpResponse.StatusCode != HttpStatusCode.OK)
                {
                    throw new HttpException(httpResponse.StatusCode, httpResponse.StatusDescription);
                }
                downloadHandler.Invoke(tempFileName);
            }
            finally
            {
                if (File.Exists(tempFileName))
                {
                    File.Delete(tempFileName);
                }
            }
        }

        public string StartBackup(string urlPart)
        {
            if (CheckForUserNameAndPassword())
                throw new ArgumentException("If you are not acting as a guest you must supply userName and password");

            if (string.IsNullOrEmpty(urlPart))
                throw new ArgumentException("Url must be specfied");

            var url = CreateUrl(urlPart);

            var httpClient = CreateHttpClient(m_TeamCityConnectionDetails.Username, m_TeamCityConnectionDetails.Password, HttpContentTypes.TextPlain);
            var response = httpClient.Post(url, null, HttpContentTypes.TextPlain);
            ThrowIfHttpError(response, url);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return response.RawText;
            }

            return string.Empty;
        }

        public T GetByFullUrl<T>(string fullUrl)
        {
            var url = string.Format("{0}{1}{2}", GetProtocol(), m_TeamCityConnectionDetails.TeamCityHost, fullUrl);
            var response = GetResponseByFullUrl(url);
            return response.StaticBody<T>();
        }

        public virtual T Get<T>(string urlPart)
        {
            var response = GetResponse(urlPart);
            return response.StaticBody<T>();
        }

        public void Get(string urlPart)
        {
            GetResponse(urlPart);
        }

        private HttpResponse GetResponse(string urlPart)
        {
            if (CheckForUserNameAndPassword())
                throw new ArgumentException("If you are not acting as a guest you must supply userName and password");

            if (string.IsNullOrEmpty(urlPart))
                throw new ArgumentException("Url must be specfied");

            var url = CreateUrl(urlPart);

            return GetResponseByFullUrl(url);
        }

        private HttpResponse GetResponseByFullUrl(string url)
        {
            var response =
                CreateHttpClient(m_TeamCityConnectionDetails.Username, m_TeamCityConnectionDetails.Password, HttpContentTypes.ApplicationJson).Get(url);
            ThrowIfHttpError(response, url);
            return response;
        }

        public T Post<T>(string data, string contenttype, string urlPart, string accept)
        {
            return Post(data, contenttype, urlPart, accept).StaticBody<T>();
        }

        public bool Authenticate(string urlPart)
        {
            try
            {
                var httpClient = CreateHttpClient(m_TeamCityConnectionDetails.Username, m_TeamCityConnectionDetails.Password, HttpContentTypes.TextPlain);
                httpClient.ThrowExceptionOnHttpError = true;
                httpClient.Get(CreateUrl(urlPart));

                var response = httpClient.Response;
                return response.StatusCode == HttpStatusCode.OK;
            }
            catch (HttpException exception)
            {
                throw new AuthenticationException(exception.StatusDescription);
            }
        }

        public virtual HttpResponse Post(object data, string contenttype, string urlPart, string accept)
        {
            var client = MakePostRequest(data, contenttype, urlPart, accept);

            return client.Response;
        }

        public virtual HttpResponse Put(object data, string contenttype, string urlPart, string accept)
        {
            var client = MakePutRequest(data, contenttype, urlPart, accept);

            return client.Response;
        }

        public virtual void Delete(string urlPart)
        {
            MakeDeleteRequest(urlPart);
        }

        private void MakeDeleteRequest(string urlPart)
        {
            var client = CreateHttpClient(m_TeamCityConnectionDetails.Username, m_TeamCityConnectionDetails.Password, HttpContentTypes.TextPlain);
            client.Delete(CreateUrl(urlPart));
            ThrowIfHttpError(client.Response, client.Request.Uri);
        }

        private IHttpClientWrapper MakePostRequest(object data, string contenttype, string urlPart, string accept)
        {
            var client = CreateHttpClient(m_TeamCityConnectionDetails.Username, m_TeamCityConnectionDetails.Password, string.IsNullOrWhiteSpace(accept) ? GetContentType(data.ToString()) : accept);

            client.Request.Accept = accept;

            client.Post(CreateUrl(urlPart), data, contenttype);
            ThrowIfHttpError(client.Response, client.Request.Uri);

            return client;
        }

        private IHttpClientWrapper MakePutRequest(object data, string contenttype, string urlPart, string accept)
        {
            var client = CreateHttpClient(m_TeamCityConnectionDetails.Username, m_TeamCityConnectionDetails.Password, string.IsNullOrWhiteSpace(accept) ? GetContentType(data.ToString()) : accept);

            client.Request.Accept = accept;

            client.Put(CreateUrl(urlPart), data, contenttype);
            ThrowIfHttpError(client.Response, client.Request.Uri);

            return client;
        }

        private static bool IsHttpError(HttpResponse response)
        {
            var num = (int)response.StatusCode / 100;

            return (num == 4 || num == 5);
        }

        /// <summary>
        /// <para>If the <paramref name="response"/> is OK (see <see cref="IsHttpError"/> for definition), does nothing.</para>
        /// <para>Otherwise, throws an exception which includes also the response raw text.
        /// This would often contain a Java exception dump from the TeamCity REST Plugin, which reveals the cause of some cryptic cases otherwise showing just "Bad Request" in the HTTP error.
        /// Also this comes in handy when TeamCity goes into maintenance, and you get back the banner in HTML instead of your data.</para> 
        /// </summary>
        private static void ThrowIfHttpError(HttpResponse response, string url)
        {
            if (!IsHttpError(response))
                return;
            throw new HttpException(response.StatusCode, string.Format("Error: {0}\nHTTP: {3}\nURL: {1}\n{2}", response.StatusDescription, url, response.RawText, response.StatusCode));
        }

        private string CreateUrl(string urlPart)
        {
            var protocol = GetProtocol();
            var authType = m_TeamCityConnectionDetails.ActAsGuest ? "/guestAuth" : "/httpAuth";

            return string.Format("{0}{1}{2}{3}", protocol, m_TeamCityConnectionDetails.TeamCityHost, authType, urlPart);
        }

        private string GetProtocol()
        {
            return m_TeamCityConnectionDetails.UseSSL ? "https://" : "http://";
        }

        private IHttpClientWrapper CreateHttpClient(string userName, string password, string accept)
        {
            var httpClient = m_HttpClientWrapperFactory.CreateHttpClientWrapper();
            httpClient.SetRequestAccept(accept);
            if (!m_TeamCityConnectionDetails.ActAsGuest)
            {
                httpClient.SetRequestBasicAuthentication(userName, password, true);
            }

            return httpClient;
        }

        // only used by the artifact listing methods since i havent found a way to deserialize them into a domain entity
        public string GetRaw(string urlPart)
        {
            if (CheckForUserNameAndPassword())
                throw new ArgumentException("If you are not acting as a guest you must supply userName and password");

            if (string.IsNullOrEmpty(urlPart))
                throw new ArgumentException("Url must be specfied");

            var url = CreateUrl(urlPart);

            var httpClient = CreateHttpClient(m_TeamCityConnectionDetails.Username, m_TeamCityConnectionDetails.Password, HttpContentTypes.TextPlain);
            var response = httpClient.Get(url);
            if (IsHttpError(response))
            {
                throw new HttpException(response.StatusCode, string.Format("Error {0}: Thrown with URL {1}", response.StatusDescription, url));
            }

            return response.RawText;
        }

        private bool CheckForUserNameAndPassword()
        {
            return !m_TeamCityConnectionDetails.ActAsGuest && string.IsNullOrEmpty(m_TeamCityConnectionDetails.Username) && string.IsNullOrEmpty(m_TeamCityConnectionDetails.Password);
        }

        private string GetContentType(string data)
        {
            if (data.StartsWith("<"))
                return HttpContentTypes.ApplicationXml;
            return HttpContentTypes.TextPlain;
        }
    }
}