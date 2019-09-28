using BskaGenericCoreLib;
using PSSR.DbAccess.FormDictionaries;
using System;
using System.Collections.Generic;
using System.Text;

namespace PSSR.Logic.FormDictionaries.Concrete
{
    public class FormDictionaryDeleteAction : BskaActionStatus,IFormDictionaryDeleteAction
    {
        private readonly IFormDictionaryDeleteDbAccess _dbAccess;
        private readonly IUpdateFormDictionaryDbAccess _updatedbAccess;
        public FormDictionaryDeleteAction(IFormDictionaryDeleteDbAccess dbAccess
            , IUpdateFormDictionaryDbAccess updatedbAccess)
        {
            _dbAccess = dbAccess;
            _updatedbAccess = updatedbAccess;
        }

        public void BizAction(long inputData)
        {
            var item = _updatedbAccess.GetFormDictionary(inputData);
            if (item == null)
                AddError("Could not find the Form Document. Someone entering illegal ids?");

            _dbAccess.Delete(item);

            Message = $"Form Document is Delete: {item.ToString()}.";
        }
    }
}
