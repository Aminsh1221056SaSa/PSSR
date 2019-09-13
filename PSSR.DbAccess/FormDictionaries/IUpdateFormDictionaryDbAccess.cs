
using PSSR.DataLayer.EfClasses.Management;

namespace PSSR.DbAccess.FormDictionaries
{
    public interface IUpdateFormDictionaryDbAccess
    {
        FormDictionary GetFormDictionary(long formDictionaryId);
    }
}
