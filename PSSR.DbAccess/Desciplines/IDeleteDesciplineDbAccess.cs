using PSSR.DataLayer.EfClasses.Management;
using System;
using System.Collections.Generic;
using System.Text;

namespace PSSR.DbAccess.Desciplines
{
    public interface IDeleteDesciplineDbAccess
    {
        void Delete(Descipline descipline);
    }
}
