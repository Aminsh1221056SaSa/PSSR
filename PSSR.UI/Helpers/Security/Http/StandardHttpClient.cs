using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PSSR.UI.Helpers.Http
{
    public class StandardHttpClient : IProtectedApiClient
    {
        private HttpClient _client;

        public StandardHttpClient(HttpClient httpClient)
        {
            _client = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<string> GetStringAsync(string uri)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
            
            var response = await _client.SendAsync(requestMessage);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<HttpResponseMessage> PostAsync<T>(string uri, T item)
        {
            return await DoPostPutAsync(HttpMethod.Post, uri, item);
        }

        public async Task<HttpResponseMessage> PostAsyncV1(string uri, MultipartFormDataContent item)
        {
            return await DoPostPutAsyncV1(HttpMethod.Post, uri, item);
        }
        public async Task<HttpResponseMessage> PutAsync<T>(string uri, T item)
        {
            return await DoPostPutAsync(HttpMethod.Put, uri, item);
        }

        public async Task<HttpResponseMessage> DeleteAsync(string uri)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);
           
            return await _client.SendAsync(requestMessage);
        }


        private async Task<HttpResponseMessage> DoPostPutAsync<T>(HttpMethod method, string uri, T item)
        {
            if (method != HttpMethod.Post && method != HttpMethod.Put)
            {
                throw new ArgumentException("Value must be either post or put.", nameof(method));
            }
            // a new StringContent must be created for each retry
            // as it is disposed after each call
            var requestMessage = new HttpRequestMessage(method, uri);
            requestMessage.Content = new StringContent(JsonConvert.SerializeObject(item), System.Text.Encoding.UTF8, "application/json");
           
            var response = await _client.SendAsync(requestMessage);
            // raise exception if HttpResponseCode 500
            // needed for circuit breaker to track fails
            if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                throw new HttpRequestException();
            }
            return response;
        }

        private async Task<HttpResponseMessage> DoPostPutAsyncV1(HttpMethod method, string uri, MultipartFormDataContent item)
        {
            if (method != HttpMethod.Post && method != HttpMethod.Put)
            {
                throw new ArgumentException("Value must be either post or put.", nameof(method));
            }
            // a new StringContent must be created for each retry
            // as it is disposed after each call
            var requestMessage = new HttpRequestMessage(method, uri);
        
            requestMessage.Content =item;
           
            var response = await _client.SendAsync(requestMessage);
            // raise exception if HttpResponseCode 500
            // needed for circuit breaker to track fails
            if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                throw new HttpRequestException();
            }
            return response;
        }

        //private void SetAuthorizationHeader(HttpRequestMessage requestMessage)
        //{
        //    var authorizationHeader = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
        //    if (!string.IsNullOrEmpty(authorizationHeader))
        //    {
        //        requestMessage.Headers.Add("Authorization", new List<string>() { authorizationHeader });
        //    }
        //}

    }
}
