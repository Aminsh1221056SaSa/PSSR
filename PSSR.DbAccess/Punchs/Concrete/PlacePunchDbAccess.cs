using System;
using System.Collections.Generic;
using PSSR.DataLayer.EfClasses.Projects.Activities;
using PSSR.DataLayer.EfCode;

namespace PSSR.DbAccess.Punchs.Concrete
{
    public class PlacePunchDbAccess : IPlacePunchDbAccess
    {
        private readonly EfCoreContext _context;

        public PlacePunchDbAccess(EfCoreContext context)
        {
            _context = context;
        }

        public void Add(Punch punch)
        {
            _context.Punchs.Add(punch);
        }

        public void AddBulck(List<Punch> items)
        {
            _context.Punchs.AddRange(items);
        }
    }
}
