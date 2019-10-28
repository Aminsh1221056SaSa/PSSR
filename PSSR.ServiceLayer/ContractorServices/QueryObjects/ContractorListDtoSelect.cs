
using PSSR.Common.ContractorServices;
using PSSR.DataLayer.EfClasses.Person;
using System.Linq;

namespace PSSR.ServiceLayer.ContractorServices.QueryObjects
{
    public static class ContractorListDtoSelect
    {
        public static IQueryable<ContractorListDto>
           MapContractorToDto(this IQueryable<Contractor> contractors)
        {
            return contractors.Select(co => new ContractorListDto
            {
                Id=co.Id,
                Name=co.Name,
                PhoneNumber=co.PhoneNumber,
                ContractDate= co.ContractDate != null ? co.ContractDate.Value.ToString("d") : "[N/A]",
                Address=co.Address
            });
        }
    }
}
