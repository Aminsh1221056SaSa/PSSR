
using PSSR.DataLayer.EfClasses.Management;
using PSSR.DataLayer.EfCode;

namespace PSSR.DbAccess.RoadMaps.Concrete
{
    public class PlaceWorkPackageDbAccess : IPlaceWorkPackageDbAccess
    {
        private readonly EfCoreContext _context;

        public PlaceWorkPackageDbAccess(EfCoreContext context)
        {
            _context = context;
        }

        public void Add(WorkPackage roadMap)
        {
            _context.ProjectRoadMaps.Add(roadMap);
        }
    }
}
