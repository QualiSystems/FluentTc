using EasyHttp.Http;

namespace FluentTc.Engine
{
    internal interface IHttpClientWrapper
    {
        void SetRequestAccept(string accept);
        void SetRequestBasicAuthentication(string userName, string password, bool b);
        HttpResponse Get(string url);
        HttpResponse GetAsFile(string url, string tempFileName);
        HttpResponse Post(string url, object data, string textPlain, object query = null);
        bool ThrowExceptionOnHttpError { set; }
        HttpResponse Response { get; }
        HttpRequest Request { get; }
        void Delete(string createUrl);
        void Put(string createUrl, object data, string contenttype, object query = null);
    }

    internal class HttpClientWrapper : IHttpClientWrapper
    {
        private readonly HttpClient m_HttpClient;

        public HttpClientWrapper()
        {
            m_HttpClient = new HttpClient(new TeamcityJsonEncoderDecoderConfiguration());
        }

        public void SetRequestAccept(string accept)
        {
            m_HttpClient.Request.Accept = accept;
        }

        public void SetRequestBasicAuthentication(string userName, string password, bool forceBasicAuth)
        {
            m_HttpClient.Request.SetBasicAuthentication(userName, password);
            m_HttpClient.Request.ForceBasicAuth = forceBasicAuth;
        }

        public HttpResponse Get(string url)
        {
            return m_HttpClient.Get(url);
        }

        public HttpResponse GetAsFile(string url, string tempFileName)
        {
            return m_HttpClient.GetAsFile(url, tempFileName);
        }

        public HttpResponse Post(string url, object data, string textPlain, object query)
        {
            return m_HttpClient.Post(url, data, textPlain, query);
        }

        public bool ThrowExceptionOnHttpError
        {
            set { m_HttpClient.ThrowExceptionOnHttpError = value; }
        }

        public HttpResponse Response
        {
            get { return m_HttpClient.Response; }
        }

        public HttpRequest Request
        {
            get { return m_HttpClient.Request; }
        }

        public void Delete(string createUrl)
        {
            m_HttpClient.Delete(createUrl);
        }

        public void Put(string createUrl, object data, string contenttype, object query = null)
        {
            m_HttpClient.Put(createUrl, data, contenttype, query);
        }
    }
}