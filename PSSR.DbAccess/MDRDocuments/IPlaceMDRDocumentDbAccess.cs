using PSSR.DataLayer.EfClasses.Projects.MDRS;

namespace PSSR.DbAccess.MDRDocuments
{
    public interface IPlaceMDRDocumentDbAccess
    {
        void Add(MDRDocument mDRDocument);
    }
}
