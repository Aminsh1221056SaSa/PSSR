
using PSSR.DataLayer.EfCode;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PSSR.DataLayer.EfClasses.Projects.Activities;

namespace PSSR.DbAccess.PunchTypes.Concrete
{
    public class UpdatePunchTypeDbAccess : IUpdatePunchTypeDbAccess
    {
        private readonly EfCoreContext _context;

        public UpdatePunchTypeDbAccess(EfCoreContext context)
        {
            _context = context;
        }

        public PunchType GetPunchType(int punchTypeid)
        {
            return _context.Find<PunchType>(punchTypeid);
        }

        public PunchType GetPunchTypeToWorkPackages(int punchTypeid)
        {
            return _context.PunchTypes.Where(s => s.Id == punchTypeid)
                .Include(s => s.WorkPackages).FirstOrDefault();
        }

        public bool HaveAnyPunch(int punchTypeid)
        {
            return _context.PunchTypes.Where(s => s.Id == punchTypeid).Any(s => s.Punches.Any());
        }
    }
}
