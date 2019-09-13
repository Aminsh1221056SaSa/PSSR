using PSSR.DataLayer.EfClasses.Management;
using System.Collections.Generic;

namespace PSSR.DbAccess.FormDictionaries
{
    public interface IPlaceFormDictionaryDbAccess
    {
        void Add(FormDictionary formDictionary);
        void AddBulck(List<FormDictionary> items);
        bool HasValidDescipline(int desciplineId);
    }
}
