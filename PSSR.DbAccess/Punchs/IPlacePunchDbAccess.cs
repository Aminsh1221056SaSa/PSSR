using System;
using System.Collections.Generic;
using PSSR.DataLayer.EfClasses.Projects.Activities;

namespace PSSR.DbAccess.Punchs
{
    public interface IPlacePunchDbAccess
    {
        void Add(Punch punch);
        void AddBulck(List<Punch> items);
    }
}
