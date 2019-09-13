using BskaGenericCoreLib;
using PSSR.DbAccess.FormDictionaries;

namespace PSSR.Logic.FormDictionaries.Concrete
{
    public class UpdateFormDictionaryAction : BskaActionStatus, IUpdateFormDictionaryAction
    {
        private readonly IUpdateFormDictionaryDbAccess _dbAccess;

        public UpdateFormDictionaryAction(IUpdateFormDictionaryDbAccess dbAccess)
        {
            _dbAccess = dbAccess;
        }

        public void BizAction(FormDictionaryDto inputData)
        {
            var formDictioanry = _dbAccess.GetFormDictionary(inputData.Id);
            if (formDictioanry == null)
                AddError("Could not find the formDictioanry. Someone entering illegal ids?");

            var status = formDictioanry.UpdateFormDictioanry(inputData.Type
                ,inputData.Description,inputData.Code,inputData.ActivityName
                , inputData.WorkPackageId,inputData.Priority,inputData.FileName,inputData.Mh);

            CombineErrors(status);

            Message = $"formDictioanry is update: {formDictioanry.ToString()}.";
        }
    }
}
