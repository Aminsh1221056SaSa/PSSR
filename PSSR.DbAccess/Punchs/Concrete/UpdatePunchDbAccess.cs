
using System.Collections.Generic;
using EFCore.BulkExtensions;
using PSSR.DataLayer.EfClasses.Projects.Activities;
using PSSR.DataLayer.EfCode;

namespace PSSR.DbAccess.Punchs.Concrete
{
    public class UpdatePunchDbAccess : IUpadePunchDbAccess
    {
        private readonly EfCoreContext _context;

        public UpdatePunchDbAccess(EfCoreContext context)
        {
            _context = context;
        }

        public Punch GetPunch(long punchid)
        {
            return _context.Find<Punch>(punchid);
        }

        public void UpdateBulck(List<Punch> items)
        {
            _context.BulkUpdate(items);
        }
    }
}
