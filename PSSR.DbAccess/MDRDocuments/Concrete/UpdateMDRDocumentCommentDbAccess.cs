
using PSSR.DataLayer.EfClasses.Projects.MDRS;
using PSSR.DataLayer.EfCode;

namespace PSSR.DbAccess.MDRDocuments.Concrete
{
    public class UpdateMDRDocumentCommentDbAccess:IUpdateMDRDocumentCommentDbAccess
    {
        private readonly EfCoreContext _context;
        public UpdateMDRDocumentCommentDbAccess(EfCoreContext context)
        {
            this._context = context;
        }
        
        public MDRDocumentComment GetMDRDocumentCommdent(long mdrCoId)
        {
            return _context.Find<MDRDocumentComment>(mdrCoId);
        }
    }
}
