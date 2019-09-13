
using System.Collections.Generic;
using PSSR.DataLayer.EfClasses.Management;
using PSSR.DataLayer.EfCode;

namespace PSSR.DbAccess.FormDictionaries.Concrete
{
    public class PlaceFormDictionaryDbAccess : IPlaceFormDictionaryDbAccess
    {
        private readonly EfCoreContext _context;

        public PlaceFormDictionaryDbAccess(EfCoreContext context)
        {
            _context = context;
        }

        public void Add(FormDictionary formDictionary)
        {
            _context.Add<FormDictionary>(formDictionary);
        }

        public void AddBulck(List<FormDictionary> items)
        {
            //using (var transaction = _context.Database.BeginTransaction())
            //{
            //    _context.BulkInsert<FormDictionary>(items);
            //    transaction.Commit();
            //}

            _context.FormDictionaries.AddRange(items);
        }

        public bool HasValidDescipline(int desciplineId)
        {
            return _context.Find<Descipline>(desciplineId) !=null;
        }
        
    }
}
