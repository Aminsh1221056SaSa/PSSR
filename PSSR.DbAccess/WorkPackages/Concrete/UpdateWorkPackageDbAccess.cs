
using PSSR.DataLayer.EfClasses.Management;
using PSSR.DataLayer.EfCode;
using System.Linq;

namespace PSSR.DbAccess.RoadMaps.Concrete
{
    public class UpdateRoadMapDbAccess : IUpdateRoadMapDbAccess
    {
        private readonly EfCoreContext _context;

        public UpdateRoadMapDbAccess(EfCoreContext context)
        {
            _context = context;
        }

        public WorkPackage GetRoadMap(int roadmapId)
        {
            var item = _context.Find<WorkPackage>(roadmapId);
            return item;
        }

        public bool HaveAnyActivity(int id)
        {
            return _context.ProjectRoadMaps.Where(s=>s.Id==id).Any(s => s.Activityes.Any());
        }
    }
}
