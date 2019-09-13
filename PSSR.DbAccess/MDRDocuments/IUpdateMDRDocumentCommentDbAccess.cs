using PSSR.DataLayer.EfClasses.Projects.MDRS;

namespace PSSR.DbAccess.MDRDocuments
{
    public interface IUpdateMDRDocumentCommentDbAccess
    {
        MDRDocumentComment GetMDRDocumentCommdent(long mdrCoId);
    }
}
