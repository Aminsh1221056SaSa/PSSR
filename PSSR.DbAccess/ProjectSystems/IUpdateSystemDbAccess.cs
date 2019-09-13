using PSSR.DataLayer.EfClasses.Projects;
using System;
using System.Collections.Generic;
using System.Text;

namespace PSSR.DbAccess.ProjectSystems
{
    public interface IUpdateSystemDbAccess
    {
        ProjectSystem GetSystme(int systemId);
    }
}
