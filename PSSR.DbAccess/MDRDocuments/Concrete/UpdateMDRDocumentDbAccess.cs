
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PSSR.DataLayer.EfClasses.Projects.MDRS;
using PSSR.DataLayer.EfCode;

namespace PSSR.DbAccess.MDRDocuments.Concrete
{
    public class UpdateMDRDocumentDbAccess : IUpdateMDRDocumentDbAccess
    {
        private readonly EfCoreContext _context;
        public UpdateMDRDocumentDbAccess(EfCoreContext context)
        {
            this._context = context;
        }

        public MDRDocument GetMDRDocument(long mdrId)
        {
            return _context.Find<MDRDocument>(mdrId);
        }

        public MDRDocument GetMDRDocumentWithStatusAndComment(long mdrId)
        {
            return _context.MDRDocuments.Where(m => m.Id == mdrId)
                .Include(m => m.MDRStatusHistoryies).Include(s => s.MDRDocumentComments).SingleOrDefault();
        }

        public bool HasDefaultStatus(long mdrId)
        {
            var statusCounter = _context.MDRDocuments.Where(s => s.Id == mdrId)
                .SelectMany(s => s.MDRStatusHistoryies).Count();
            return statusCounter == 1;
        }

    }
}
