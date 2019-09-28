using System;
using System.Collections.Generic;
using System.Text;
using PSSR.DataLayer.EfClasses.Management;
using PSSR.DataLayer.EfCode;

namespace PSSR.DbAccess.Desciplines.Concrete
{
    public class DeleteDesciplineDbAccess: IDeleteDesciplineDbAccess
    {
        private readonly EfCoreContext _context;

        public DeleteDesciplineDbAccess(EfCoreContext context)
        {
            _context = context;
        }

        public void Delete(Descipline descipline)
        {
            _context.Desciplines.Remove(descipline);
        }
    }
}
