namespace FluentTc.Engine
{
    internal interface IHttpClientWrapperFactory
    {
        IHttpClientWrapper CreateHttpClientWrapper();
    }

    internal class HttpClientWrapperFactory : IHttpClientWrapperFactory
    {
        public IHttpClientWrapper CreateHttpClientWrapper()
        {
            return new HttpClientWrapper();
        }
    }
}