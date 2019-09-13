

using PSSR.DataLayer.EfClasses.Management;

namespace PSSR.DbAccess.ValueUnits
{
    public interface IPlaceValueUnitDbAccess
    {
        void Add(ValueUnit valueUnit);
        bool HaveValidParent(int parentId);
        bool HaveValidDesciplines(int desciplineId);
    }
}
