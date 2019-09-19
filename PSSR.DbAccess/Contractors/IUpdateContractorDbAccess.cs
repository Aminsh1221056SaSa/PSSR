
using PSSR.DataLayer.EfClasses.Person;

namespace PSSR.DbAccess.Contractors
{
    public interface IUpdateContractorDbAccess
    {
        Contractor GetContractor(int contractorId);
        bool HaveAnyPorjects(int contractorId);
    }
}
