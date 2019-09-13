using BskaGenericCoreLib;
using PSSR.DbAccess.PunchCategoryies;
using System;

namespace PSSR.Logic.PunchCategoryes.Concrete
{
    public class UpdatePunchCategoryAction : BskaActionStatus, IUpdatePunchCategoryAction
    {
        private readonly IUpdatePunchCategoryDbAccess _dbAccess;

        public UpdatePunchCategoryAction(IUpdatePunchCategoryDbAccess dbAccess)
        {
            _dbAccess = dbAccess;
        }
        public void BizAction(PunchCategoryDto inputData)
        {
            var formDictioanry = _dbAccess.GetPunchCategory(inputData.Id);
            if (formDictioanry == null)
                throw new NullReferenceException("Could not find the punch Category. Someone entering illegal ids?");

            var status = formDictioanry.UpdatePunchCategory(inputData.Name);

            CombineErrors(status);

            Message = $"punch Category is update: {formDictioanry.ToString()}.";
        }
    }
}
