using BskaGenericCoreLib;
using PSSR.DbAccess.Desciplines;
using System;

namespace PSSR.Logic.Desciplines.Concrete
{
    public class UpdateDesciplineAction : BskaActionStatus, IUpdateDesciplineAction
    {
        private readonly IUpdateDesciplineDbAccess _dbAccess;

        public UpdateDesciplineAction(IUpdateDesciplineDbAccess dbAccess)
        {
            _dbAccess = dbAccess;
        }

        public void BizAction(PlaceDesciplineDto inputData)
        {
            var descipline = _dbAccess.GetDescipline(inputData.Descipline.Id);
            if (descipline == null)
                throw new NullReferenceException("Could not find the descipline. Someone entering illegal ids?");

            var status = descipline.UpdateDescipline(inputData.Descipline.Name,inputData.Descipline.Description);
            CombineErrors(status);

            Message = $"descipline is update: {descipline.ToString()}.";
        }
    }
}
