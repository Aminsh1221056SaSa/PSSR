using BskaGenericCoreLib;
using PSSR.DataLayer.EfClasses.Projects.Activities;
using System.Collections.Generic;

namespace PSSR.Logic.Punches
{
    public interface IUpdatePunchBulkAction : IGenericActionInOnlyWriteDb<List<Punch>> { }
}
