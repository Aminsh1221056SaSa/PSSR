
using PSSR.DataLayer.EfClasses.Management;
using PSSR.DataLayer.EfCode;

namespace PSSR.DbAccess.Desciplines.Concrete
{
    public class UpdateDesciplineDbAccess : IUpdateDesciplineDbAccess
    {
        private readonly EfCoreContext _context;

        public UpdateDesciplineDbAccess(EfCoreContext context)
        {
            _context = context;
        }
        public Descipline GetDescipline(int desciplineId)
        {
            return _context.Find<Descipline>(desciplineId);
        }
    }
}
