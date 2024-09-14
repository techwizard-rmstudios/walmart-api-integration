
/// *************************************************************
/// *             Coded by Ekaterina Bozhko                     *
/// *             bozhkokateryna12@gmail.com                    *
/// *************************************************************

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WalmartAPI
{
    public class WalmartAuth
    {
        private readonly Credentials _credentials;

        public WalmartAuth(string clientID, string clientSecret)
        {
            _credentials = new Credentials(clientID, clientSecret);
        }

        public async Task<string> GetTokenAsync()
        {
            var client = GetHttpClient();
            var endpoint = "https://marketplace.walmartapis.com/v3/token";
            var content = new StringContent("grant_type=client_credentials", Encoding.UTF8, "application/x-www-form-urlencoded");
            HttpResponseMessage responseMessage = null;
            Token response = null;
            try
            {
                responseMessage = await client.PostAsync(endpoint, content);
                response = await responseMessage.Content.ReadAsAsync<Token>();
            }
            finally
            {
                responseMessage?.Dispose();
                content.Dispose();
                client.Dispose();
            }

            return response.AccessToken;
        }

        private HttpClient GetHttpClient()
        {
            var authHeader = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_credentials.ClientId}:{_credentials.ClientSecret}"));

            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeader);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("WM_SVC.NAME", "Walmart Marketplace Service");
            client.DefaultRequestHeaders.Add("WM_QOS.CORRELATION_ID", Guid.NewGuid().ToString("N"));

            return client;
        }
    }
    public class Token
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }
    }
    public class Credentials
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }

        public Credentials(string clientID, string clientSecret)
        {
            ClientId = clientID;
            ClientSecret = clientSecret;
        }
    }
}
