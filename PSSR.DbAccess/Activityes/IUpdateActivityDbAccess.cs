
using PSSR.DataLayer.EfClasses.Projects.Activities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace PSSR.DbAccess.Activityes
{
    public interface IUpdateActivityDbAccess
    {
        Activity GetActivity(long activityId);
        Activity GetActivityToPunches(long activityId);
        IEnumerable<Activity> GetActivityByActivityId(List<long> acCode);
        IEnumerable<Activity> GetActivityForConfigPlan(int workId,int locationId,long subsystemId, int desId);
        void UpdateActivityWf(DataTable items);
        void UpdateActivityPlane(IEnumerable<Activity> items);
        void UpdateBulck(List<Activity> items);
    }
}
