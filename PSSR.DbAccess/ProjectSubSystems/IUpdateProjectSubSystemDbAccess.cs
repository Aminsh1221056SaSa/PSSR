using PSSR.DataLayer.EfClasses.Projects;
using System;
using System.Collections.Generic;
using System.Text;

namespace PSSR.DbAccess.ProjectSubSystems
{
    public interface IUpdateProjectSubSystemDbAccess
    {
        ProjectSubSystem GetSubSystme(long subsystemId);
    }
}
