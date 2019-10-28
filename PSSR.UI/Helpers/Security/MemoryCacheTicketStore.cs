using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Caching.Memory;

namespace PSSR.UI.Helpers.Security
{
    public class MemoryCacheTicketStore : ITicketStore
    {
        private readonly int _expireInHours = 1;
        private const string KeyPrefix = "PSSRCookieSessionStore-";
        private IMemoryCache _cache;
        public MemoryCacheTicketStore()
        {
            _cache = new MemoryCache(new MemoryCacheOptions());
        }

        public Task RemoveAsync(string key)
        {
            _cache.Remove(key);
            return Task.FromResult(0);
        }

        public Task RenewAsync(string key, AuthenticationTicket ticket)
        {
            var options = new MemoryCacheEntryOptions();
            var expiresUtc = ticket.Properties.ExpiresUtc;
            if (expiresUtc.HasValue)
                options.SetAbsoluteExpiration(expiresUtc.Value);

            options.SetSlidingExpiration(TimeSpan.FromHours(_expireInHours));

            _cache.Set(key, ticket, options);

            return Task.FromResult(0);
        }

        public Task<AuthenticationTicket> RetrieveAsync(string key)
        {
            _cache.TryGetValue(key, out AuthenticationTicket ticket);
            return Task.FromResult(ticket);
        }

        public async Task<string> StoreAsync(AuthenticationTicket ticket)
        {
            var key = $"{KeyPrefix}{Guid.NewGuid()}";
            await RenewAsync(key, ticket);
            return key;
        }
    }
}
