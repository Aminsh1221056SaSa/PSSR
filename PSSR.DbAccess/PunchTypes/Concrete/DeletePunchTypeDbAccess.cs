
using PSSR.DataLayer.EfClasses.Projects.Activities;
using PSSR.DataLayer.EfCode;

namespace PSSR.DbAccess.PunchTypes.Concrete
{
    public class DeletePunchTypeDbAccess : IDeletePunchTypeDbAccess
    {
        private readonly EfCoreContext _context;

        public DeletePunchTypeDbAccess(EfCoreContext context)
        {
            _context = context;
        }
        public void Delete(PunchType punchType)
        {
            _context.PunchTypes.Remove(punchType);
        }
    }
}
