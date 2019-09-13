
using PSSR.DataLayer.EfClasses.Projects.MDRS;
using PSSR.DataLayer.EfCode;

namespace PSSR.DbAccess.MDRDocuments.Concrete
{
    public class PlaceMDRDocumentCommentDbAccess:IPlaceMDRDocumentCommentDbAccess
    {
        private readonly EfCoreContext _context;
        public PlaceMDRDocumentCommentDbAccess(EfCoreContext context)
        {
            this._context = context;
        }

        public void Add(MDRDocumentComment mDRDocumentComment)
        {
            _context.Add<MDRDocumentComment>(mDRDocumentComment);
        }
    }
}
