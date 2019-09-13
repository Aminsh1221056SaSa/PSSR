using BskaGenericCoreLib;
using PSSR.DataLayer.EfClasses.Management;
using System.Collections.Generic;

namespace PSSR.Logic.FormDictionaries
{
    public interface IPlcaeFormDictionaryBulkAction : IGenericActionInOnlyWriteDb<List<FormDictionary>> { }
}
