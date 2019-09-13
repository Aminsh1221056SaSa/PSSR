using BskaGenericCoreLib;
using PSSR.DbAccess.RoadMaps;
using System;

namespace PSSR.Logic.RoadMaps.Concrete
{
    public class UpdateWorkPackageAction : BskaActionStatus, IUpdateRoadMapAction
    {
        private readonly IUpdateRoadMapDbAccess _dbAccess;
        public UpdateWorkPackageAction(IUpdateRoadMapDbAccess dbAccess)
        {
            _dbAccess = dbAccess;
        }

        public void BizAction(ProjectWorkPackageListDto inputData)
        {
            var roadMap = _dbAccess.GetRoadMap(inputData.Id);
            if (roadMap == null)
                throw new NullReferenceException("Could not find the road map. Someone entering illegal ids?");

            var status = roadMap.UpdateRoadMap(inputData.Name);

            CombineErrors(status);

            Message = $"road map is update: {roadMap.ToString()}.";
        }
    }
}
