using BskaGenericCoreLib;
using PSSR.DataLayer.EfClasses.Management;
using PSSR.DbAccess.FormDictionaries;

namespace PSSR.Logic.FormDictionaries.Concrete
{
    public class PlaceFormDictionaryAction : BskaActionStatus, IPlaceFormDictionaryAction
    {
        private readonly IPlaceFormDictionaryDbAccess _dbAccess;
        public PlaceFormDictionaryAction(IPlaceFormDictionaryDbAccess dbAccess)
        {
            _dbAccess = dbAccess;
        }
        public FormDictionary BizAction(FormDictionaryDto inputData)
        {
            if (string.IsNullOrWhiteSpace(inputData.Code))
            {
                AddError("Form Dictionary Code is Required.");
                return null;
            }

            if (inputData.AvailableDesciplines == null)
            {
                AddError(" .");
                return null;
            }

            foreach(var desId in inputData.AvailableDesciplines)
            {
                if(!_dbAccess.HasValidDescipline(desId))
                {
                    AddError("Could not find the descipline.Someone entering illegal descipline id ? .");
                    return null;
                }
            }

            var desStatus = FormDictionary.CreateFormDicFactory(inputData.Type
                ,inputData.Description,inputData.Code,
                inputData.ActivityName,inputData.FileName,
                inputData.WorkPackageId,inputData.AvailableDesciplines,inputData.Priority,inputData.Mh);

            CombineErrors(desStatus);

            if (!HasErrors)
                _dbAccess.Add(desStatus.Result);

            return HasErrors ? null : desStatus.Result;
        }
    }
}
