
using PSSR.DataLayer.EfClasses.Management;

namespace PSSR.DbAccess.LocationTypes
{
    public interface IUpdateLocationTypeDbAccess
    {
        LocationType GetLocationType(int locationId);
        bool HaveAnyActivity(int id);
    }
}
