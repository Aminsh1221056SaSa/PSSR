using PSSR.DataLayer.EfClasses.Management;
using PSSR.DataLayer.EfCode;
using System;
using System.Collections.Generic;
using System.Text;

namespace PSSR.DbAccess.ValueUnits.Concrete
{
    public class DeleteValueUnitDbAccess:IDeleteValueUnitDbAccess
    {
        private readonly EfCoreContext _context;

        public DeleteValueUnitDbAccess(EfCoreContext context)
        {
            _context = context;
        }

        public void Delete(ValueUnit valueUnit)
        {
            _context.ValueUnits.Remove(valueUnit);
        }
    }
}
