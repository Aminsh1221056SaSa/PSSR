using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PSSR.DataLayer.EfClasses.Projects;
using PSSR.DataLayer.EfCode;

namespace PSSR.DbAccess.Projects.Concrete
{
    public class UpdateProjectDbAccess : IUpdateProjectDbAccess
    {
        private readonly EfCoreContext _context;

        public UpdateProjectDbAccess(EfCoreContext context)
        {
            _context = context;
        }

        public Project GetProject(Guid projectId)
        {
            return _context.Find<Project>(projectId);
        }

        public IEnumerable<Project> GetProjectForPerson(int personId)
        {
            return _context.Projects.Where(p => p.AgentsLink.Any(s => s.PersonId == personId))
                .Include(s=>s.AgentsLink).AsEnumerable();
        }

        public bool haveAnyWbs(Guid projectId)
        {
            return _context.Projects.Any(p => p.Id==projectId && p.ProjectSystems.Any());
        }
    }
}
