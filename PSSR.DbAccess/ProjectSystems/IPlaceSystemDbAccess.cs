using PSSR.DataLayer.EfClasses.Projects;
using System;
using System.Collections.Generic;
using System.Text;

namespace PSSR.DbAccess.ProjectSystems
{
    public interface IPlaceSystemDbAccess
    {
        void Add(ProjectSystem projectSystem);
        void AddBulck(List<ProjectSystem> items);
    }
}
