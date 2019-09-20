using PSSR.DataLayer.EfClasses.Projects;
using System;
using System.Collections.Generic;

namespace PSSR.DbAccess.Projects
{
    public interface IUpdateProjectDbAccess
    {
        Project GetProject(Guid projectId);
        IEnumerable<Project> GetProjectForPerson(int personId);
        bool haveAnyWbs(Guid projectId);
    }
}
