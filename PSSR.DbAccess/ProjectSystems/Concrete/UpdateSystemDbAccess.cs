using System;
using System.Collections.Generic;
using System.Text;
using PSSR.DataLayer.EfClasses.Projects;
using PSSR.DataLayer.EfCode;

namespace PSSR.DbAccess.ProjectSystems.Concrete
{
    public class UpdateSystemDbAccess : IUpdateSystemDbAccess
    {
        private readonly EfCoreContext _context;

        public UpdateSystemDbAccess(EfCoreContext context)
        {
            _context = context;
        }


        public ProjectSystem GetSystme(int systemId)
        {
            return _context.Find<ProjectSystem>(systemId);
        }
    }
}
