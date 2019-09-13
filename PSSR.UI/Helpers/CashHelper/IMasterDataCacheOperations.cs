using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSSR.UI.Helpers.CashHelper
{
    public interface IMasterDataCacheOperations
    {
        Task<T> GetMasterDataCacheAsync<T>(string key);
        Task CreateMasterDataCacheAsync<T>(string key,T item);
        void SetUserCurrentProject(string key, Guid projectId);
        Guid GetUserCurrentProject(string key);
    }
}
