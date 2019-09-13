
using PSSR.DataLayer.EfClasses.Projects.MDRS;

namespace PSSR.DbAccess.MDRDocuments
{
    public interface IUpdateMDRDocumentDbAccess
    {
        MDRDocument GetMDRDocument(long mdrId);
        MDRDocument GetMDRDocumentWithStatusAndComment(long mdrId);
        bool HasDefaultStatus(long mdrId);
    }
}
