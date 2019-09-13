using PSSR.ServiceLayer.ActivityServices.Concrete;
using PSSR.ServiceLayer.RoadMapServices;
using PSSR.ServiceLayer.Utils;
using System.Collections.Generic;

namespace PSSR.ServiceLayer.ActivityServices
{
    public class ActivityListCombinedDto
    {
        public ActivityListCombinedDto(ActivitySortFilterPageOptions sortFilterPageData, IEnumerable<ActivityListDto> activityList
            , List<WorkPackageListDto> workpackages, List<LocationListDto> locations, List<DropdownTuple> systems)
        {
            SortFilterPageData = sortFilterPageData;
            ActivityList = activityList;
            WorkPackagesItems = workpackages;
            LocationItems = locations;
            Systems = systems;
        }

        public ActivitySortFilterPageOptions SortFilterPageData { get; private set; }
        public IEnumerable<ActivityListDto> ActivityList { get; private set; }
        public List<WorkPackageListDto> WorkPackagesItems { get; set; }
        public List<LocationListDto> LocationItems { get; set; }
        public List<DropdownTuple> Systems { get; set; }
    }
}
