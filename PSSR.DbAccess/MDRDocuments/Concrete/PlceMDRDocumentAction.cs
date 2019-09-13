using PSSR.DataLayer.EfClasses.Projects.MDRS;
using PSSR.DataLayer.EfCode;

namespace PSSR.DbAccess.MDRDocuments.Concrete
{
    public class PlceMDRDocumentAction:IPlaceMDRDocumentDbAccess
    {
        private readonly EfCoreContext _context;
        public PlceMDRDocumentAction(EfCoreContext context)
        {
            this._context = context;
        }

        public void Add(MDRDocument mDRDocument)
        {
            _context.Add<MDRDocument>(mDRDocument);
        }

    }
}
