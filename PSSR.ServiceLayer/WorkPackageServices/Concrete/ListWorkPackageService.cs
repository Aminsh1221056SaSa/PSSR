using Microsoft.EntityFrameworkCore;
using PSSR.Common.RoadMapServices;
using PSSR.DataLayer.EfCode;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSSR.ServiceLayer.RoadMapServices.Concrete
{
    public class ListWorkPackageService
    {
        private readonly EfCoreContext _context;

        public ListWorkPackageService(EfCoreContext context)
        {
            _context = context;
        }

        public async Task<List<WorkPackageListDto>> GetTwoFirstWorkPackagesAsync()
        {
            return await _context.ProjectRoadMaps.OrderBy(s=>s.Id).Take(2).Select(s => new WorkPackageListDto
            {
                Id = s.Id,
                Title = s.Name,
            }).ToListAsync();
        }

        public async Task<List<WorkPackageListDto>> GetRoadMapsAsync()
        {
            return await _context.ProjectRoadMaps.Select(s => new WorkPackageListDto
            {
                Id=s.Id,
                Title=s.Name,
            }).ToListAsync();
        }

        public async Task<List<LocationListDto>> GetLocationsAsync()
        {
            return await _context.LocationTypes.Select(s => new LocationListDto
            {
                Id = s.Id,
                Title = s.Title
            }).ToListAsync();
        }

        public async Task<WorkPackageListDto> GetRoadMapAsycn(int id)
        {
            return await _context.ProjectRoadMaps.Where(s => s.Id == id).Select(s => new WorkPackageListDto
            {
                Id = s.Id,
                Title = s.Name,
            }).FirstOrDefaultAsync();
        }

        public async Task<LocationListDto> GetLocationAsycn(int id)
        {
            return await _context.LocationTypes.Where(s => s.Id == id).Select(s => new LocationListDto
            {
               Id=s.Id,
               Title=s.Title
            }).FirstOrDefaultAsync();
        }

    }
}
