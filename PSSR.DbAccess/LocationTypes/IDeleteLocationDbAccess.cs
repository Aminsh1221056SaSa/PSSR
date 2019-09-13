
using PSSR.DataLayer.EfClasses.Management;

namespace PSSR.DbAccess.LocationTypes
{
    public interface IDeleteLocationDbAccess
    {
        void Delete(LocationType location);
    }
}
