using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PSSR.UI.Configuration;
using PSSR.UI.Helpers.Navigation;
using PSSR.UserSecurity.Configuration.IdentityContextModels;
using PSSR.UserSecurity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSSR.UI.Helpers.CashHelper
{
    public interface INavigationCacheOperations
    {
        Task<NavigationMenu> GetNavigationCacheAsync();
        Task CreateNavigationCacheAsync();
    }

    public class NavigationCacheOperations : INavigationCacheOperations
    {
        private readonly IDistributedCache _cache;
        private readonly string NavigationCacheName = "NavigationCache";
        private readonly AppIdentityDbContext _identityDbContext;
        private readonly IMapper _mapper;
        private readonly IOptions<ApplicationSettings> _settings;

        public NavigationCacheOperations(IDistributedCache cache, AppIdentityDbContext context, IMapper mapper
            , IOptions<ApplicationSettings> settings)
        {
            _cache = cache;
            _identityDbContext = context;
            _mapper = mapper;
            _settings = settings;
        }
        public async Task CreateNavigationCacheAsync()
        {
            var mItems = new NavigationMenu();
            var roles = await _identityDbContext.Roles.ToListAsync();
            var itemTypes = await _identityDbContext.NavigationMenus.Where(s => s.Type == MenuType.PCMSWEBLeft
            && s.ClientName == _settings.Value.ApplicationTitle).Include(s => s.Parent)
                .Include(s => s.Childeren).Include(s => s.Roles).ToListAsync();
            if (itemTypes.Any())
            {
                Parallel.ForEach(itemTypes, item =>
                {
                    var rIds = item.Roles.Select(s => s.RoleId);
                    var rstring = roles.Where(o => rIds.Contains(o.Id)).Select(o => o.Name).ToList();
                    item.SelectedRoles = rstring;
                });

                var itemsDto = _mapper.Map<IEnumerable<NavigationMenuType>, IEnumerable<NavigationMenuItem>>(itemTypes.Where(s => s.Parent == null));

                mItems.MenuItems = itemsDto.ToList();
                var items = JsonConvert.SerializeObject(mItems);
                await _cache.SetStringAsync(NavigationCacheName, items);
            }
        }
        public async Task<NavigationMenu> GetNavigationCacheAsync()
        {
            return JsonConvert.DeserializeObject<NavigationMenu>(await _cache.GetStringAsync
            (NavigationCacheName));
        }
    }
}
