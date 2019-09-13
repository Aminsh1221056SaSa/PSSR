using BskaGenericCoreLib;
using PSSR.DataLayer.EfClasses.Projects;
using System.Collections.Generic;

namespace PSSR.Logic.ProjectSystmes
{
    public interface IPlcaeSystemBulkAction : IGenericActionInOnlyWriteDb<List<ProjectSystem>> { }
}
