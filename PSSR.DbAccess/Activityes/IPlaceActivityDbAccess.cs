
using PSSR.DataLayer.EfClasses.Projects.Activities;
using System.Collections.Generic;

namespace PSSR.DbAccess.Activityes
{
    public interface IPlaceActivityDbAccess
    {
        void Add(Activity activity);
        void AddBulck(List<Activity> items);
    }
}
