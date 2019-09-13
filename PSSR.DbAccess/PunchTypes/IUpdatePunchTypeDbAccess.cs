
using PSSR.DataLayer.EfClasses.Projects.Activities;

namespace PSSR.DbAccess.PunchTypes
{
    public interface IUpdatePunchTypeDbAccess
    {
        PunchType GetPunchType(int punchTypeid);
        PunchType GetPunchTypeToWorkPackages(int punchTypeid);
        bool HaveAnyPunch(int punchTypeid);
    }
}
