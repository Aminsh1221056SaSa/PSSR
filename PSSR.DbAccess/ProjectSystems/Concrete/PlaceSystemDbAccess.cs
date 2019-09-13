using System;
using System.Collections.Generic;
using System.Text;
using PSSR.DataLayer.EfClasses.Projects;
using PSSR.DataLayer.EfCode;

namespace PSSR.DbAccess.ProjectSystems.Concrete
{
    public class PlaceSystemDbAccess : IPlaceSystemDbAccess
    {
        private readonly EfCoreContext _context;

        public PlaceSystemDbAccess(EfCoreContext context)
        {
            _context = context;
        }

        public void Add(ProjectSystem projectSystem)
        {
            _context.ProjectSystems.Add(projectSystem);
        }

        public void AddBulck(List<ProjectSystem> items)
        {
            _context.ProjectSystems.AddRange(items);
        }
    }
}
