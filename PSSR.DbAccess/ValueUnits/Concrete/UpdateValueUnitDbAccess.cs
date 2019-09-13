
using PSSR.DataLayer.EfClasses.Management;
using PSSR.DataLayer.EfCode;

namespace PSSR.DbAccess.ValueUnits.Concrete
{
    public class UpdateValueUnitDbAccess : IUpdateValueUnitDbAccess
    {
        private readonly EfCoreContext _context;

        public UpdateValueUnitDbAccess(EfCoreContext context)
        {
            _context = context;
        }

        public ValueUnit GetValueUnit(int valueUnitId)
        {
            return _context.Find<ValueUnit>(valueUnitId);
        }
    }
}
