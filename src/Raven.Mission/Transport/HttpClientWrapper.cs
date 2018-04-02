﻿using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Raven.Mission.Transport
{
    public class HttpClientWrapper:IHttpClient
    {
        private readonly HttpClient _client;
        public HttpClientWrapper(string host)
        {
            _client=new HttpClient{BaseAddress = new Uri(host)};
            _client.DefaultRequestHeaders.Connection.Add("keep-alive");
        }

        public void Dispose()
        {
            _client.Dispose();
        }

        public Task SendAsync<TRequest>(string resource, TRequest request)
        {
            var msg = JsonConvert.SerializeObject(request);
            var content = new StringContent(msg, Encoding.UTF8, "application/json");
            return _client.PostAsync(resource, content); 
        }

        
    }
}
