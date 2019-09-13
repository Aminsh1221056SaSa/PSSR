using PSSR.DataLayer.EfClasses.Projects.Activities;
using PSSR.DataLayer.EfCode;

namespace PSSR.DbAccess.Activityes.Concrete
{
    public class DeleteActivityDbAccess:IDeleteActivityDbAccess
    {
        private readonly EfCoreContext _context;

        public DeleteActivityDbAccess(EfCoreContext context)
        {
            _context = context;
        }

        public void Delete(Activity activity)
        {
            _context.Activites.Remove(activity);
        }
    }
}
