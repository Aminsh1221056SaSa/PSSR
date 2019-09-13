
using PSSR.DataLayer.EfClasses.Management;
using System.Linq;

namespace PSSR.ServiceLayer.FormDictionaryServices.QueryObjects
{
    public static class FormDictionaryListDtoSelect
    {
        public static IQueryable<FormDictionaryListDto>
           MapFormDicToDto(this IQueryable<FormDictionary> formDictionary)
        {
            return formDictionary.Select(p => new FormDictionaryListDto
            {
              ActivityName=p.ActivityName,
              Code=p.Code,
              Description=p.Description,
              Id=p.Id,
              WorkPackageId=p.WorkPackageId,
              Type=p.Type,
              FileName=p.FileName,
              WrokPackageName=p.WorkPackage.Name,
              Desciplines=p.DesciplineLink.Select(s=>s.Descipline.Name).ToList(),
              DesciplinesIds= p.DesciplineLink.Select(s => s.Descipline.Id).ToArray(),
              Priority=p.Priority,
              Mh=p.ManHours
            });
        }
    }
}
