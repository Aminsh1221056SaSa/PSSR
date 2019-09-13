using BskaGenericCoreLib;
using PSSR.DataLayer.EfClasses.Projects;
using PSSR.DbAccess.Activityes;
using PSSR.DbAccess.Desciplines;
using PSSR.DbAccess.LocationTypes;
using PSSR.DbAccess.Projects;
using PSSR.DbAccess.ProjectSubSystems;
using PSSR.DbAccess.RoadMaps;
using System;
using System.Linq;

namespace PSSR.Logic.Activityes.Concrete
{
    public class UpdateActivityPlaneAction : BskaActionStatus, IUpdateActivityPlaneAction
    {
        private readonly IUpdateActivityDbAccess _dbAccess;
        private readonly IUpdateRoadMapDbAccess _workPackageDbAccess;
        private readonly IUpdateDesciplineDbAccess _desciplineDbAccess;
        private readonly IUpdateProjectSubSystemDbAccess _subsystemDbAccess;
        private readonly IUpdateLocationTypeDbAccess _locationDbAccess;
        private readonly IUpdateProjectDbAccess _projectDbAccess;

        public UpdateActivityPlaneAction(IUpdateActivityDbAccess dbAccess, IUpdateDesciplineDbAccess desiplineDbAccess
            , IUpdateProjectDbAccess projectDbAccess, 
            IUpdateRoadMapDbAccess workPackageDbAccess, IUpdateProjectSubSystemDbAccess subsystemdbaccess
            , IUpdateLocationTypeDbAccess locationDbAccess)
        {
            _dbAccess = dbAccess;
            _workPackageDbAccess = workPackageDbAccess;
            _projectDbAccess = projectDbAccess;
            _desciplineDbAccess = desiplineDbAccess;
            _subsystemDbAccess = subsystemdbaccess;
            _locationDbAccess = locationDbAccess;
        }

        public void BizAction(ActivityPlaneDto inputData)
        {
            var project = _projectDbAccess.GetProject(inputData.ProjectId);

            this.calculateBySubSystem(inputData, project);
        }

        private void calculateBySubSystem(ActivityPlaneDto inputData, Project project)
        {
            var workPackages = _workPackageDbAccess.GetRoadMap(inputData.WorkPackageId);
            if (workPackages == null)
            {
                AddError("Not available WorkPackage!!!", "activity");
            }

            var location = _locationDbAccess.GetLocationType(inputData.LocationId);
            if (location == null)
            {
                AddError("Not available location!!!", "activity");
            }

            var subSystem = _subsystemDbAccess.GetSubSystme(inputData.SubSystemId);

            if (subSystem == null)
            {
                AddError("Not available subSystem!!!", "activity");
            }

            var descipline = _desciplineDbAccess.GetDescipline(inputData.DesciplineId);

            if (subSystem == null)
            {
                AddError("Not available descipline!!!", "activity");
            }

            if (inputData.StartDate > inputData.EndDate)
            {
                AddError("Inserted Start Date is greater of End Date!!!", "activity");
            }

            if (inputData.StartDate < project.StartDate)
            {
                AddError("Start Date must be greater of project start date!!!", "activity");
            }

            if (inputData.EndDate > project.EndDate)
            {
                AddError("End Date must be lowest of project end date!!!", "activity");
            }

            if (!this.HasErrors)
            {
                var items = _dbAccess.GetActivityForConfigPlan(inputData.WorkPackageId,inputData.LocationId,inputData.SubSystemId, inputData.DesciplineId)
                    .ToList();

                float formMh = items.Sum(s => s.FormDictionary.ManHours);
                double totalHours = (inputData.EndDate - inputData.StartDate).TotalHours;

                if(totalHours<formMh)
                {
                    AddError("Total hours for claculate date is Smaller than sum of Tasks's form Mh", "activity");
                    return;
                }

                double fac = 0;

                if (totalHours > formMh)
                {
                    double dif = (totalHours - formMh);
                    fac =Math.Floor(dif / items.Count);
                }

                var groupItems = items
                    .OrderBy(s => s.FormDictionary.Priority).GroupBy(s => s.FormDictionary.Id);

                DateTime startDate = inputData.StartDate;
                DateTime endDate = inputData.StartDate;

                foreach (var g in groupItems)
                {
                    var orderbySubSystem = g.OrderBy(o => o.SubSystem.PriorityNo).ThenBy(o => o.SubSystem.SubPriorityNo);
                    foreach (var ac in orderbySubSystem)
                    {
                        var fmh = ac.FormDictionary.ManHours;
                        fmh = (float)(fmh + fac);

                        endDate = startDate.AddHours(fmh);
                        ac.UpdateActivityPlane(startDate, endDate);
                        startDate = endDate;
                    }
                }

                _dbAccess.UpdateActivityPlane(items);
            }
        }
    }
}
