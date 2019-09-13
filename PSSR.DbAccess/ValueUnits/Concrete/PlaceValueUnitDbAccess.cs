using System;
using PSSR.DataLayer.EfCode;
using System.Linq;
using PSSR.DataLayer.EfClasses.Management;

namespace PSSR.DbAccess.ValueUnits.Concrete
{
    public class PlaceValueUnitDbAccess : IPlaceValueUnitDbAccess
    {
        private readonly EfCoreContext _context;

        public PlaceValueUnitDbAccess(EfCoreContext context)
        {
            _context = context;
        }

        public void Add(ValueUnit valueUnit)
        {
            _context.ValueUnits.Add(valueUnit);
        }

        public bool HaveValidParent(int parentId)
        {
           return _context.ValueUnits.Any(s=>s.Id==parentId);
        }

        public bool HaveValidDesciplines(int desciplineId)
        {
            return _context.Desciplines.Any(s => s.Id == desciplineId);
        }
    }
}
