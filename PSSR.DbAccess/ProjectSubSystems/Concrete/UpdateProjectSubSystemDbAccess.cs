using PSSR.DataLayer.EfCode;
using System;
using System.Collections.Generic;
using System.Text;
using PSSR.DataLayer.EfClasses.Projects;

namespace PSSR.DbAccess.ProjectSubSystems.Concrete
{
    public class UpdateProjectSubSystemDbAccess:IUpdateProjectSubSystemDbAccess
    {
        private readonly EfCoreContext _context;

        public UpdateProjectSubSystemDbAccess(EfCoreContext context)
        {
            _context = context;
        }

        public ProjectSubSystem GetSubSystme(long subsystemId)
        {
            return _context.Find<ProjectSubSystem>(subsystemId);
        }
    }
}
