
using PSSR.DataLayer.EfCode;
using PSSR.DataLayer.EfClasses.Projects.Activities;

namespace PSSR.DbAccess.PunchCategoryies.Concrete
{
    public class PlacePunchTypeDbAccess : IPlacePunchCategoryDbAccess
    {
        private readonly EfCoreContext _context;

        public PlacePunchTypeDbAccess(EfCoreContext context)
        {
            _context = context;
        }
        public void Add(PunchCategory punchCategory)
        {
            _context.PunchCategories.Add(punchCategory);
        }
    }
}
