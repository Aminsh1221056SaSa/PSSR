using PSSR.DataLayer.EfClasses.Management;
using PSSR.DataLayer.EfCode;

namespace PSSR.DbAccess.FormDictionaries.Concrete
{
    public class FormDictionaryDeleteDbAccess:IFormDictionaryDeleteDbAccess
    {
        private readonly EfCoreContext _context;

        public FormDictionaryDeleteDbAccess(EfCoreContext context)
        {
            _context = context;
        }

        public void Delete(FormDictionary formDocument)
        {
            _context.FormDictionaries.Remove(formDocument);
        }
    }
}
