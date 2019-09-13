using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSSR.UI.Helpers.CashHelper
{
    public class MasterDataCacheOperations : IMasterDataCacheOperations
    {
        private readonly IDistributedCache _cache;
        public MasterDataCacheOperations(IDistributedCache cache)
        {
            _cache = cache;
        }
        public async Task CreateMasterDataCacheAsync<T>(string key, T item)
        {
            await _cache.SetStringAsync(key, JsonConvert.SerializeObject(item));
        }

        public async Task<T> GetMasterDataCacheAsync<T>(string key)
        {
            var item = await _cache.GetStringAsync(key);
            var deserializedItem= JsonConvert.DeserializeObject<T>(item);
            return deserializedItem;
        }

        public Guid GetUserCurrentProject(string key)
        {
            var instance = SingletonObjectCreator.UniqueInstance._currentprojectToUser;
            Tuple<Guid, TimeSpan> outling = null;
            if (!instance.ContainsKey(key))
            {
                throw new KeyNotFoundException("Some things is be wrong!!!please contact to Administrator.");
            }
            instance.TryGetValue(key, out outling);
            return outling.Item1;
        }

        public void SetUserCurrentProject(string key, Guid projectId)
        {
            if (projectId == default(Guid))
            {
                return;
            }

            var instance = SingletonObjectCreator.UniqueInstance._currentprojectToUser;
            var user = key;

            Tuple<Guid, TimeSpan> outling = null;
            if (instance.ContainsKey(user))
            {
                instance.TryGetValue(user, out outling);
                if (outling == null)
                {
                    throw new KeyNotFoundException("Some things is be wrong!!!please contact to Administrator.");
                }
                var newValue = new Tuple<Guid, TimeSpan>(projectId, DateTime.Now.TimeOfDay);
                instance.TryUpdate(key, newValue,outling);
            }
            else
            {
                outling = new Tuple<Guid, TimeSpan>(projectId, DateTime.Now.TimeOfDay);
                instance.TryAdd(user, outling);
            }
        }
    }
}
