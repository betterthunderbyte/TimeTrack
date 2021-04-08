using System;
using System.Net.Http;

namespace TimeTrack.Client
{
    public abstract class WebClientBase
    {
        public Uri BaseAddress
        {
            get => HttpClient.BaseAddress;
            set {
                HttpClient.BaseAddress = value;
            }
        }

        public HttpClientHandler HttpClientHandler { get; set; }
        public HttpClient HttpClient { get; set; }

        public WebClientBase()
        {
            HttpClientHandler = new HttpClientHandler() {};
            HttpClientHandler.ClientCertificateOptions = ClientCertificateOption.Manual;
            HttpClientHandler.ServerCertificateCustomValidationCallback = (httpRequestMessage, cert, cetChain, policyErrors) =>
            {
                return true;
            };
            HttpClient = new HttpClient(HttpClientHandler);
            HttpClient.BaseAddress = BaseAddress;
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