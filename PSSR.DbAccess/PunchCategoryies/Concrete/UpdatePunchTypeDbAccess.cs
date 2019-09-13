
using PSSR.DataLayer.EfCode;
using System.Linq;
using PSSR.DataLayer.EfClasses.Projects.Activities;

namespace PSSR.DbAccess.PunchCategoryies.Concrete
{
    public class UpdatePunchTypeDbAccess : IUpdatePunchCategoryDbAccess
    {
        private readonly EfCoreContext _context;

        public UpdatePunchTypeDbAccess(EfCoreContext context)
        {
            _context = context;
        }

        public PunchCategory GetPunchCategory(int punchcategoryid)
        {
            return _context.Find<PunchCategory>(punchcategoryid);
        }

        public bool HaveAnyPunch(int punchcategoryid)
        {
            return _context.PunchCategories.Where(s => s.Id == punchcategoryid).Any(s => s.Punches.Any());
        }
    }
}
