using System;
using System.Collections.Generic;
using PSSR.DataLayer.EfClasses.Projects.Activities;

namespace PSSR.DbAccess.Punchs
{
    public interface IUpadePunchDbAccess
    {
        Punch GetPunch(long punchid);
        void UpdateBulck(List<Punch> items);
    }
}
