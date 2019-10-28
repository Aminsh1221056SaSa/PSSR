using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PSSR.UI.Helpers.Http
{
    public interface IProtectedApiClient
    {
        Task<string> GetStringAsync(string uri);
        Task<HttpResponseMessage> PostAsync<T>(string uri, T item);
        Task<HttpResponseMessage> PostAsyncV1(string uri, MultipartFormDataContent item);
        Task<HttpResponseMessage> PutAsync<T>(string uri, T item);
        Task<HttpResponseMessage> DeleteAsync(string uri);
    }
}
