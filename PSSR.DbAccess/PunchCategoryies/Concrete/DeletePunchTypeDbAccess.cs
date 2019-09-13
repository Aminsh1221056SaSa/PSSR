using System;
using System.Collections.Generic;
using PSSR.DataLayer.EfClasses.Projects.Activities;
using PSSR.DataLayer.EfCode;

namespace PSSR.DbAccess.PunchCategoryies.Concrete
{
    public class DeletePunchTypeDbAccess : IDeletePunchCategoryDbAccess
    {
        private readonly EfCoreContext _context;

        public DeletePunchTypeDbAccess(EfCoreContext context)
        {
            _context = context;
        }
        public void Delete(PunchCategory punchCategory)
        {
            _context.PunchCategories.Remove(punchCategory);
        }
    }
}
