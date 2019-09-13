
using PSSR.DataLayer.EfClasses.Projects.Activities;
using System.Linq;

namespace PSSR.ServiceLayer.PunchTypeServices.QueryObjects
{
    public static class PunchTypeListDtoSelect
    {
        public static IQueryable<PunchTypeListDto>
         MapPanchTypeToDto(this IQueryable<PunchType> formDictionary)
        {
            return formDictionary.Select(p => new PunchTypeListDto
            {
               Id=p.Id,
               Name=p.Name
            });
        }
    }
}
