
using PSSR.DataLayer.EfClasses.Management;
using PSSR.DataLayer.EfCode;

namespace PSSR.DbAccess.FormDictionaries.Concrete
{
    public class UpdateFormDictionaryDbAccess : IUpdateFormDictionaryDbAccess
    {
        private readonly EfCoreContext _context;

        public UpdateFormDictionaryDbAccess(EfCoreContext context)
        {
            _context = context;
        }
        public FormDictionary GetFormDictionary(long formDictionaryId)
        {
            return _context.Find<FormDictionary>(formDictionaryId);
        }
    }
}
