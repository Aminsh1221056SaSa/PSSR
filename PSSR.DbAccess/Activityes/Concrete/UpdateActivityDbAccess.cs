
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Data;
using PSSR.DataLayer.EfCode;
using PSSR.DataLayer.EfClasses.Projects.Activities;
using EFCore.BulkExtensions;

namespace PSSR.DbAccess.Activityes.Concrete
{
    public class UpdateActivityDbAccess : IUpdateActivityDbAccess
    {
        private readonly EfCoreContext _context;

        public UpdateActivityDbAccess(EfCoreContext context)
        {
            _context = context;
        }

        public Activity GetActivity(long activityId)
        {
            return _context.Find<Activity>(activityId);
        }

        public IEnumerable<Activity> GetActivityByActivityId(List<long> acCode)
        {
            return _context.Activites.Where(s => acCode.Contains(s.Id)).AsEnumerable();
        }

        public IEnumerable<Activity> GetActivityForConfigPlan(int workId, int locationId, long subsystemId, int desId)
        {
            return _context.Activites.Where(s => s.WorkPackageId == workId && s.LocationId==locationId
            && s.SubsytemId == subsystemId && s.DesciplineId==desId)
            .Include(s=>s.SubSystem).Include(s => s.FormDictionary).AsEnumerable();
        }

        public Activity GetActivityToPunches(long activityId)
        {
            return _context.Activites.Where(s => s.Id == activityId)
                .Include(s => s.Punchs).Single();
        }

        public void UpdateActivityPlane(IEnumerable<Activity> items)
        {
            _context.UpdateRange(items);
        }

        public void UpdateActivityWf(DataTable items)
        {
            //IDatabaseService _dbService = new DatabaseService();
            //_dbService.ConnectionString = "Data Source=DESKTOP-8LJDK8P;Initial Catalog=RefineryArchives;User ID=sa;Password=1221056@Am";
            //_dbService.ExecuteNonQuery("dbo.UpdateTaskWF", CommandType.StoredProcedure, items);
        }

        public void UpdateBulck(List<Activity> items)
        {
            _context.BulkUpdate(items);
        }
    }
}
