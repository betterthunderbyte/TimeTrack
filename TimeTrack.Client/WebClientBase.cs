using System;
using System.Net.Http;

namespace TimeTrack.Client
{
    public abstract class WebClientBase
    {
        protected HttpClientHandler HttpClientHandler { get; set; }
        protected HttpClient HttpClient { get; set; }

        public WebClientBase(string baseAddress)
        {
            HttpClientHandler = new HttpClientHandler() {};
            HttpClientHandler.ClientCertificateOptions = ClientCertificateOption.Manual;
            HttpClientHandler.ServerCertificateCustomValidationCallback = (httpRequestMessage, cert, cetChain, policyErrors) =>
            {
                return true;
            };
            HttpClient = new HttpClient(HttpClientHandler);
            HttpClient.BaseAddress = new Uri(baseAddress);
        }
        
        public WebClientBase(HttpClient client)
        {
            HttpClient = client;
        }

        public WebClientBase(HttpClient client, HttpClientHandler handler)
        {
            HttpClient = client;
            HttpClientHandler = handler;
        }
    }
}