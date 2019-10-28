using PSSR.Common.WorkPackageSteps;
using PSSR.DataLayer.EfClasses.Management;
using System.Linq;

namespace PSSR.ServiceLayer.WorkPackageSteps.QueryObjects
{
    public static class WorkPackageStepListDtoSelect
    {
        public static IQueryable<WorkPackageStepListDto>
         MapWorkPackageStepToDto(this IQueryable<WorkPackageStep> formDictionary)
        {
            return formDictionary.Select(p => new WorkPackageStepListDto
            {
                Id = p.Id,
                Title = p.Title,
                Description=p.Description,
                WorkPackageName=p.WorkPackage.Name,
                WorkPackageId=p.WorkPackageId
            });
        }
    }
}
