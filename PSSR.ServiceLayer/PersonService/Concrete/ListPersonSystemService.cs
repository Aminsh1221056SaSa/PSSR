using Microsoft.EntityFrameworkCore;
using PSSR.Common.PersonService;
using PSSR.DataLayer.EfCode;
using PSSR.DataLayer.QueryObjects;
using PSSR.ServiceLayer.PersonService.QueryObjects;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSSR.ServiceLayer.PersonService.Concrete
{
    public class ListPersonSystemService
    {
        private readonly EfCoreContext _context;

        public ListPersonSystemService(EfCoreContext context)
        {
            _context = context;
        }

        public bool HasTakenNationalId(string nationalId)
        {
            return _context.Persons.Any(s => string.Equals(s.NationalId, nationalId,System.StringComparison.OrdinalIgnoreCase));
        }

        public async Task<PersonListDto> GetPerson(int id)
        {
            return await _context.Persons.Where(s => s.Id == id)
                .Select(p => new PersonListDto
                {
                    FirstName=p.FirstName,
                    Id=p.Id,
                    LastName=p.LastName,
                    MobileNumber=p.MobileNumber,
                    NationalId=p.NationalId,
                    CurrentProjects=p.ProjectLink.Select(s=>s.ProjectId)
                }).SingleAsync();
        }

        public async Task<IEnumerable<PersonSummaryDto>> GetPersons()
        {
            return await _context.Persons.Select(p => new PersonSummaryDto
            {
                    Name = p.FirstName+" "+ p.LastName,
                    Id = p.Id,
                }).ToListAsync();
        }

        public IEnumerable<PersonListDto> SortFilterPage(PersonSortFilterPageOptions options)
        {
            var projectSubSystmes = _context.Persons
                .AsNoTracking()
                .Where(sb => sb.NationalId.Contains(options.QueryFilter) || 
                sb.FirstName.Contains(options.QueryFilter) || sb.LastName.Contains(options.QueryFilter)
                || string.IsNullOrWhiteSpace(options.QueryFilter))
                .FiltePersonBy(options.FilterBy,options.FilterValue)
                .Include(s=>s.ProjectLink)
                .MapPersonToDto()
                .OrderPersonBy(options.OrderByOptions);

            options.SetupRestOfDto(projectSubSystmes);

            return projectSubSystmes.Page(options.PageNum - 1, options.PageSize);
        }

    }
}
