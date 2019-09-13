using EFCore.BulkExtensions;
using PSSR.DataLayer.EfClasses.Projects.Activities;
using PSSR.DataLayer.EfCode;
using System;
using System.Collections.Generic;
using System.Text;

namespace PSSR.DbAccess.Activityes.Concrete
{
    public class PlaceActivityDbAccess : IPlaceActivityDbAccess
    {
        private readonly EfCoreContext _context;

        public PlaceActivityDbAccess(EfCoreContext context)
        {
            _context = context;
        }

        public void Add(Activity activity)
        {
            _context.Add<Activity>(activity);
        }

        public void AddBulck(List<Activity> items)
        {
            _context.BulkInsert(items);
        }
    }
}
