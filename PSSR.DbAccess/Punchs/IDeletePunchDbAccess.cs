
using PSSR.DataLayer.EfClasses.Projects.Activities;

namespace PSSR.DbAccess.Punchs
{
    public interface IDeletePunchDbAccess
    {
        void Delete(Punch punch);
    }
}
