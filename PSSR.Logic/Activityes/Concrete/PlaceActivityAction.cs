using BskaGenericCoreLib;
using PSSR.DataLayer.EfClasses.Projects.Activities;
using PSSR.DbAccess.Activityes;
using PSSR.DbAccess.Desciplines;
using PSSR.DbAccess.FormDictionaries;
using PSSR.DbAccess.LocationTypes;
using PSSR.DbAccess.ProjectSubSystems;
using PSSR.DbAccess.ProjectSystems;
using PSSR.DbAccess.RoadMaps;
using PSSR.DbAccess.WorkPackageSteps;

namespace PSSR.Logic.Activityes.Concrete
{
    public class PlaceActivityAction : BskaActionStatus, IPlaceActivityAction
    {
        private readonly IPlaceActivityDbAccess _dbAccess;
        private readonly IUpdateRoadMapDbAccess _workPackageDbAccess;
        private readonly IUpdateLocationTypeDbAccess _locationDbAccess;
        private readonly IUpdateFormDictionaryDbAccess _formDicDbAccess;
        private readonly IUpdateDesciplineDbAccess _desciplineDbAccess;
        private readonly IUpdateSystemDbAccess _systemDbAccess;
        private readonly IUpdateProjectSubSystemDbAccess _subSystemDbAccess;
        private readonly IUpdateWorkPackageStepDbAccess _workStepDbAccess;

        public PlaceActivityAction(IPlaceActivityDbAccess dbAccess, IUpdateRoadMapDbAccess workPackageAccess,
            IUpdateLocationTypeDbAccess locDbAccess, IUpdateFormDictionaryDbAccess formDicAccess
            , IUpdateDesciplineDbAccess desciplineDbAccess, IUpdateSystemDbAccess systemDbAccess,
            IUpdateProjectSubSystemDbAccess subsystemDbAccess, IUpdateWorkPackageStepDbAccess worksetpDbAccess)
        {
            _dbAccess = dbAccess;
            _workPackageDbAccess = workPackageAccess;
            _locationDbAccess = locDbAccess;
            _formDicDbAccess = formDicAccess;
            _desciplineDbAccess = desciplineDbAccess;
            _systemDbAccess = systemDbAccess;
            _subSystemDbAccess = subsystemDbAccess;
            _workStepDbAccess = worksetpDbAccess;
        }

        public Activity BizAction(ActivityDto inputData)
        {
            var workPackage = _workPackageDbAccess.GetRoadMap(inputData.WorkPackageId);
            if (workPackage == null)
            {
                AddError("WorkPackage is Not Valid.");
            }

            var location = _locationDbAccess.GetLocationType(inputData.LocationId);
            if (location == null)
            {
                AddError("Location is Not Valid.");
            }

            var descipline = _desciplineDbAccess.GetDescipline(inputData.DesciplineId);
            if (descipline == null)
            {
                AddError("Descipline is Not Valid.");
            }

            var form = _formDicDbAccess.GetFormDictionary(inputData.FormDictionaryId);
            if (form == null)
            {
                AddError("Form is Not Valid.");
            }

            var system = _systemDbAccess.GetSystme(inputData.SystemdId);
            if (system == null)
            {
                AddError("System is Not Valid.");
            }

            var subSystem = _subSystemDbAccess.GetSubSystme(inputData.SubsytemId);
            if(subSystem==null)
            {
                AddError("Subsystem is Not Valid.");
            }
            var workStep = _workStepDbAccess.GetWorkPackageStep(inputData.WorkPackageStepId);
            if (workStep == null)
            {
                AddError("WorkPackage Step is Not Valid.");
            }

            if (inputData.WeightFactor < 0)
            {
                AddError("WeightFactor is Not Valid.");
            }

            if (string.IsNullOrWhiteSpace(inputData.TagNumber))
            {
                AddError("TagNumber is Not Valid.");
            }

            var desStatus = Activity.CreateActivity(inputData.TagNumber,inputData.TagDescription,inputData.Progress,
                inputData.WeightFactor,inputData.ValueUnitNum,inputData.EstimateMh,inputData.ActualMh,inputData.Status,inputData.ActualStartDate,inputData.ActualEndDate
                ,inputData.PlanStartDate,inputData.PlanEndDate,inputData.FormDictionaryId,inputData.ValueUnitId
                ,inputData.WorkPackageId,inputData.LocationId,inputData.SubsytemId,
                inputData.Condition,inputData.ActivityCode,inputData.DesciplineId,inputData.WorkPackageStepId);

            CombineErrors(desStatus);

            if (!HasErrors)
                _dbAccess.Add(desStatus.Result);

            return HasErrors ? null : desStatus.Result;
        }
    }
}
