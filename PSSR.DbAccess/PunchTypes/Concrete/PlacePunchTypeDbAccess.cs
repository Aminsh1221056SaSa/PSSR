
using PSSR.DataLayer.EfCode;
using PSSR.DataLayer.EfClasses.Projects.Activities;

namespace PSSR.DbAccess.PunchTypes.Concrete
{
    public class PlacePunchTypeDbAccess : IPlacePunchTypeDbAccess
    {
        private readonly EfCoreContext _context;

        public PlacePunchTypeDbAccess(EfCoreContext context)
        {
            _context = context;
        }
        public void Add(PunchType punchType)
        {
            _context.PunchTypes.Add(punchType);
        }
    }
}
