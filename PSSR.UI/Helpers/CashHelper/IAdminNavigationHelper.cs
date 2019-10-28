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
    public interface IAdminNavigationHelper
    {
        Task<NavigationMenu> GetNavigationCacheAsync();
        Task CreateNavigationCacheAsync();
    }

    public class AdminNavigationHelper : IAdminNavigationHelper
    {
        private readonly IDistributedCache _cache;
        private readonly string NavigationCacheName = "AdminNavigationCache";
        private readonly AppIdentityDbContext _identityDbContext;
        private readonly IMapper _mapper;
        private readonly IOptions<ApplicationSettings> _settings;
        public AdminNavigationHelper(IDistributedCache cache, AppIdentityDbContext context, IMapper mapper
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
            var itemTypes = await _identityDbContext.NavigationMenus.Where(s => s.Type == MenuType.PCMSWEBRight
              && s.ClientName == _settings.Value.ApplicationTitle).Include(s => s.Parent)
                .Include(s => s.Childeren).Include(s => s.Roles).ToListAsync();
            var itemsDto = new List<NavigationMenuItem>();
            foreach(var item in itemTypes)
            {
                var rIds = item.Roles.Select(s => s.RoleId);
                var rstring = roles.Where(o => rIds.Contains(o.Id)).Select(o => o.Name).ToList();
                itemsDto.Add(new NavigationMenuItem
                {
                    SelectedRoles = rstring,
                    ClientName = item.ClientName,
                    DisplayName = item.DisplayName,
                    Type = item.Type,
                    IsNested = item.IsNested,
                    Sequence = item.Sequence,
                    MaterialIcon = item.MaterialIcon,
                    Link = item.Link,
                });
            }
            mItems.MenuItems = itemsDto.ToList();
            var items = JsonConvert.SerializeObject(mItems);
            await _cache.SetStringAsync(NavigationCacheName, items);
        }

        public async Task<NavigationMenu> GetNavigationCacheAsync()
        {
            return JsonConvert.DeserializeObject<NavigationMenu>(await _cache.GetStringAsync
          (NavigationCacheName));
        }
    }
}
