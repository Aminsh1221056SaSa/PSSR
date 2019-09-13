
using PSSR.DataLayer.EfClasses.Projects.MDRS;
using System.Linq;

namespace PSSR.ServiceLayer.MDRStatuses.QueryObjects
{
    public static class MDRStatusListDtoSelect
    {
        public static IQueryable<MDRStatusListDto>
                MapMDRStatusToDto(this IQueryable<MDRStatus> mdrStatus)
        {
            return mdrStatus.Select(p => new MDRStatusListDto
            {
                Id = p.Id,
                Name=p.Name,
                Wf=p.Wf,
                Description=p.Description
            });
        }
    }
}
