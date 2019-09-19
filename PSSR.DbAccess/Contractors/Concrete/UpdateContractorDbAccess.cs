
using PSSR.DataLayer.EfClasses.Person;
using PSSR.DataLayer.EfCode;
using System.Linq;

namespace PSSR.DbAccess.Contractors.Concrete
{
    public class UpdateContractorDbAccess : IUpdateContractorDbAccess
    {
        private readonly EfCoreContext _context;

        public UpdateContractorDbAccess(EfCoreContext context)
        {
            _context = context;
        }

        public Contractor GetContractor(int contractorId)
        {
            return _context.Find<Contractor>(contractorId);
        }

        public bool HaveAnyPorjects(int contractorId)
        {
            return _context.Contractors.Any(c => c.Id == contractorId && c.Projects.Any());
        }
    }
}
