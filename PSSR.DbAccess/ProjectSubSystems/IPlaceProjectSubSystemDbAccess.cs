using PSSR.DataLayer.EfClasses.Projects;
using System;
using System.Collections.Generic;
using System.Text;

namespace PSSR.DbAccess.ProjectSubSystems
{
    public interface IPlaceProjectSubSystemDbAccess
    {
        void Add(ProjectSubSystem projectSubSystem);
        void AddBulck(List<ProjectSubSystem> items);
    }
}
