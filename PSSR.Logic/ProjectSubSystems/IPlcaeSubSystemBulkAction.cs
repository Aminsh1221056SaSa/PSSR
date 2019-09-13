using BskaGenericCoreLib;
using PSSR.DataLayer.EfClasses.Projects;
using System.Collections.Generic;

namespace PSSR.Logic.ProjectSubSystems
{
    public interface IPlcaeSubSystemBulkAction : IGenericActionInOnlyWriteDb<List<ProjectSubSystem>> { }
}
