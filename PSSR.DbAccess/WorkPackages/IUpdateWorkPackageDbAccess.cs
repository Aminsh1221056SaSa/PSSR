
using PSSR.DataLayer.EfClasses.Management;

namespace PSSR.DbAccess.RoadMaps
{
    public interface IUpdateRoadMapDbAccess
    {
        WorkPackage GetRoadMap(int roadmapId);
        bool HaveAnyActivity(int id);
    }
}
