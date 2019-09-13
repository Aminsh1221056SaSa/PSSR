using PSSR.DataLayer.EfClasses.Projects.Activities;

namespace PSSR.DbAccess.PunchCategoryies
{
    public interface IDeletePunchCategoryDbAccess
    {
        void Delete(PunchCategory punchCategory);
    }
}
