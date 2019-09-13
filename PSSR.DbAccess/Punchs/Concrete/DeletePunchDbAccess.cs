
using PSSR.DataLayer.EfClasses.Projects.Activities;
using PSSR.DataLayer.EfCode;

namespace PSSR.DbAccess.Punchs.Concrete
{
    public class DeletePunchDbAccess : IDeletePunchDbAccess
    {
        private readonly EfCoreContext _context;

        public DeletePunchDbAccess(EfCoreContext context)
        {
            _context = context;
        }

        public void Delete(Punch punch)
        {
            _context.Punchs.Remove(punch);
        }
    }
}
