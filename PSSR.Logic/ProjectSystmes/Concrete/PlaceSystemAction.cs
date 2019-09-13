using BskaGenericCoreLib;
using System;
using System.Collections.Generic;
using System.Text;
using PSSR.DataLayer.EfClasses.Projects;
using PSSR.DbAccess.ProjectSystems;

namespace PSSR.Logic.ProjectSystmes.Concrete
{
    public class PlaceSystemAction : BskaActionStatus, IPlaceSystemAction
    {
        private readonly IPlaceSystemDbAccess _dbAccess;
        public PlaceSystemAction(IPlaceSystemDbAccess dbAccess)
        {
            _dbAccess = dbAccess;
        }

        public ProjectSystem BizAction(ProjectSystemDto inputData)
        {
            if (string.IsNullOrWhiteSpace(inputData.Code))
            {
                AddError("Code is Required.");
                return null;
            }

            var desStatus = ProjectSystem.CreateProjectSystem(inputData.Code,inputData.Description,inputData.Type,inputData.ProjectId);

            CombineErrors(desStatus);

            if (!HasErrors)
                _dbAccess.Add(desStatus.Result);

            return HasErrors ? null : desStatus.Result;
        }
    }
}
