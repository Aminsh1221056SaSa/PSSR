
using PSSR.DataLayer.EfClasses.Person;
using PSSR.DataLayer.EfCode;

namespace PSSR.DbAccess.Contractors.Concrete
{
    public class DeleteContractorDbAccess : IDeleteContractorDbAccess
    {
        private readonly EfCoreContext _context;

        public DeleteContractorDbAccess(EfCoreContext context)
        {
            _context = context;
        }

        public void Delete(Contractor contractor)
        {
            _context.Contractors.Remove(contractor);
        }
    }
}
