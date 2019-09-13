using BskaGenericCoreLib;
using PSSR.DataLayer.EfClasses.Projects.Activities;
using System.Collections.Generic;

namespace PSSR.Logic.Activityes
{
    public interface IUpdateActivityBulkAction : IGenericActionInOnlyWriteDb<List<Activity>> { }
}
