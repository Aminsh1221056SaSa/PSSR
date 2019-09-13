
using PSSR.DataLayer.EfClasses.Projects.MDRS;
using System;

namespace PSSR.DbAccess.MDRStatuses
{
    public interface IUpdateMDRStatusDbAccess
    {
        MDRStatus GetMdrStatus(int mdrStatusId);
        MDRStatus GetDefaultStatus(Guid projectId);
        MDRStatus GetNextStatus(Guid projectId, MDRStatus current);
    }
}
