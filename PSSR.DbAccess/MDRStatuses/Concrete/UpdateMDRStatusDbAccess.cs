
using PSSR.DataLayer.EfClasses.Projects.MDRS;
using PSSR.DataLayer.EfCode;
using System;
using System.Linq;

namespace PSSR.DbAccess.MDRStatuses.Concrete
{
    public class UpdateMDRStatusDbAccess : IUpdateMDRStatusDbAccess
    {
        private readonly EfCoreContext _context;
        public UpdateMDRStatusDbAccess(EfCoreContext context)
        {
            this._context = context;
        }

        public MDRStatus GetMdrStatus(int mdrStatusId)
        {
            return _context.Find<MDRStatus>(mdrStatusId);
        }
        public MDRStatus GetDefaultStatus(Guid projectId)
        {
            return _context.MDRStatus.Where(s=>s.ProjectId==projectId).FirstOrDefault();
        }

        public MDRStatus GetNextStatus(Guid projectId,MDRStatus current)
        {
            var list = _context.MDRStatus.Where(s => s.ProjectId == projectId).ToList();
            return list.SkipWhile(x => !x.Equals(current)).Skip(1).FirstOrDefault();
        }
    }
}
