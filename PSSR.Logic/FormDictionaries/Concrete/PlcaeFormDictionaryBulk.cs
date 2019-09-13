using BskaGenericCoreLib;
using PSSR.DbAccess.FormDictionaries;
using System.Collections.Generic;
using System.Linq;
using PSSR.DataLayer.EfClasses.Management;

namespace PSSR.Logic.FormDictionaries.Concrete
{
    public class PlcaeFormDictionaryBulkAction : BskaActionStatus, IPlcaeFormDictionaryBulkAction
    {
        private readonly IPlaceFormDictionaryDbAccess _dbAccess;
        public PlcaeFormDictionaryBulkAction(IPlaceFormDictionaryDbAccess dbAccess)
        {
            _dbAccess = dbAccess;
        }

        public void BizAction(List<FormDictionary> inputData)
        {
            if (!inputData.Any())
            {
                AddError("Form Dictionary Code is Required.");
            }

            _dbAccess.AddBulck(inputData);
        }
    }
}
