
using PSSR.Common.DesciplineServices;
using PSSR.DataLayer.EfClasses.Management;
using System.Linq;

namespace PSSR.ServiceLayer.DesciplineServices.QueryObjects
{
    public static class DesciplineListDtoSelect
    {
        public static IQueryable<DesciplineListDto>
            MapBookToDto(this IQueryable<Descipline> desciplines)
        {
            return desciplines.Select(p => new DesciplineListDto
            {
                Id = p.Id,
                Title = p.Name,
                Description=p.Description,
                CreatedDate=p.CreatedDate
            });
        }
    }
}
