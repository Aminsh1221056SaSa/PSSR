using PSSR.DataLayer.EfClasses.Projects.Activities;

namespace PSSR.DbAccess.PunchCategoryies
{
    public interface IUpdatePunchCategoryDbAccess
    {
        PunchCategory GetPunchCategory(int punchcategoryid);
        bool HaveAnyPunch(int punchcategoryid);
    }
}
