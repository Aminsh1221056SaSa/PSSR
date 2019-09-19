
using PSSR.DataLayer.EfClasses.Person;

namespace PSSR.DbAccess.Contractors
{
    public interface IDeleteContractorDbAccess
    {
        void Delete(Contractor contractor);
    }
}
