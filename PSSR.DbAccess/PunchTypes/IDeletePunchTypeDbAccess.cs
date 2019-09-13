
using PSSR.DataLayer.EfClasses.Projects.Activities;

namespace PSSR.DbAccess.PunchTypes
{
    public interface IDeletePunchTypeDbAccess
    {
        void Delete(PunchType punchType);
    }
}
