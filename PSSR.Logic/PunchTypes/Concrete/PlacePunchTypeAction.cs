using BskaGenericCoreLib;
using System;
using System.Collections.Generic;
using System.Text;
using PSSR.DataLayer.EfClasses;
using PSSR.DbAccess.PunchCategoryies;
using PSSR.DataLayer.EfClasses.Projects.Activities;

namespace PSSR.Logic.PunchCategoryes.Concrete
{
    public class PlacePunchCategoryAction : BskaActionStatus, IPlacePunchCategoryAction
    {
        private readonly IPlacePunchCategoryDbAccess _dbAccess;
        public PlacePunchCategoryAction(IPlacePunchCategoryDbAccess dbAccess)
        {
            _dbAccess = dbAccess;
        }

        public PunchCategory BizAction(PunchCategoryDto inputData)
        {
            if (string.IsNullOrWhiteSpace(inputData.Name))
            {
                AddError("Punch Category Name is Required.");
                return null;
            }

            var desStatus = PunchCategory.CreatePunchCategory(inputData.Name,inputData.ProjectId);

            CombineErrors(desStatus);

            if (!HasErrors)
                _dbAccess.Add(desStatus.Result);

            return HasErrors ? null : desStatus.Result;
        }
    }
}
