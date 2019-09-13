
using PSSR.DataLayer.EfClasses.Projects.MDRS;

namespace PSSR.DbAccess.MDRStatuses
{
    public interface IPlaceMDRStatusDbAccess
    {
        void Add(MDRStatus mdrStatus);
    }
}
