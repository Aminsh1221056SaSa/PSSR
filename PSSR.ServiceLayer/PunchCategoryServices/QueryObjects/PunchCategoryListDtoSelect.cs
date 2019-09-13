
using PSSR.DataLayer.EfClasses.Projects.Activities;
using System.Linq;

namespace PSSR.ServiceLayer.PunchCategoryServices.QueryObjects
{
    public static class PunchCategoryListDtoSelect
    {
        public static IQueryable<PunchCategoryListDto>
         MapPanchCategoryToDto(this IQueryable<PunchCategory> formDictionary)
        {
            return formDictionary.Select(p => new PunchCategoryListDto
            {
               Id=p.Id,
               Name=p.Name
            });
        }
    }
}
