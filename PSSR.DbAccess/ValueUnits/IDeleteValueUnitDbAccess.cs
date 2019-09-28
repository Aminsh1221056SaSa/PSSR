using PSSR.DataLayer.EfClasses.Management;
using System;
using System.Collections.Generic;
using System.Text;

namespace PSSR.DbAccess.ValueUnits
{
    public interface IDeleteValueUnitDbAccess
    {
        void Delete(ValueUnit valueUnit);
    }
}
