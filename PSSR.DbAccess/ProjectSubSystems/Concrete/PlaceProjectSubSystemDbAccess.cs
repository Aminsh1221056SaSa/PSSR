using System;
using System.Collections.Generic;
using System.Text;
using PSSR.DataLayer.EfClasses.Projects;
using PSSR.DataLayer.EfCode;

namespace PSSR.DbAccess.ProjectSubSystems.Concrete
{
    public class PlaceProjectSubSystemDbAccess : IPlaceProjectSubSystemDbAccess
    {
        private readonly EfCoreContext _context;

        public PlaceProjectSubSystemDbAccess(EfCoreContext context)
        {
            _context = context;
        }

        public void Add(ProjectSubSystem projectSubSystem)
        {
            _context.ProjectSubSystems.Add(projectSubSystem);
        }

        public void AddBulck(List<ProjectSubSystem> items)
        {
            _context.ProjectSubSystems.AddRange(items);
        }
    }
}
