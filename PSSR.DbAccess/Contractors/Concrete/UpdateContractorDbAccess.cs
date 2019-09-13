
using PSSR.DataLayer.EfClasses.Person;
using PSSR.DataLayer.EfCode;

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
    }
}
