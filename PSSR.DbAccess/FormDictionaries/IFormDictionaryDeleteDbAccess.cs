using PSSR.DataLayer.EfClasses.Management;
using System;
using System.Collections.Generic;
using System.Text;

namespace PSSR.DbAccess.FormDictionaries
{
    public interface IFormDictionaryDeleteDbAccess
    {
        void Delete(FormDictionary formDocument);
    }
}
