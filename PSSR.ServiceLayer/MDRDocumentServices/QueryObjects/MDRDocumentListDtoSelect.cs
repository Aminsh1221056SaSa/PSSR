
using PSSR.DataLayer.EfClasses.Projects.MDRS;
using System.Linq;

namespace PSSR.ServiceLayer.MDRDocumentServices.QueryObjects
{
    public static class MDRDocumentListDtoSelect
    {
        public static IQueryable<MDRDocumentListDto>
              MapMDRDicToDto(this IQueryable<MDRDocument> mdrDocuments)
        {
            return mdrDocuments.Select(p => new MDRDocumentListDto
            {
                Title=p.Title,
                CreatedDate=p.CreatedDate.ToString("dddd, dd MMMM yyyy"),
                UpdatedDate=p.UpdatedDate.ToString("dddd, dd MMMM yyyy"),
                WorkPackageId=p.WorkPackageId,
                WBSName=p.WorkPackage.Name,
                Description = p.Description,
                Id = p.Id,
                Code=p.Code,
                LastStatusId=p.MDRStatusHistoryies.Any()?p.MDRStatusHistoryies
                .OrderByDescending(s=>s.Id).First().MdrStatusId:-1,
                LastStatusName = p.MDRStatusHistoryies.Any() ? p.MDRStatusHistoryies
                .OrderByDescending(s => s.Id).First().HStatus.Name : "",
                CommentCount=p.MDRDocumentComments.Count(),
                HaveUnclearComment=p.MDRDocumentComments.Any(s=>!s.IsClear),
                LastIssuanceDate= p.MDRStatusHistoryies.Any()? p.MDRStatusHistoryies
                .OrderByDescending(s => s.Id).First().CreatedDate.ToString("dddd, dd MMMM yyyy") : "",
                Progress = p.MDRStatusHistoryies.Any() ? p.MDRStatusHistoryies.Where(s=>s.IsContractorConfirm)
                .OrderByDescending(s => s.Id).Sum(s=>s.HStatus.Wf) :0,
                Type=p.Type,
                IsCompleted=p.IsCompleted
            });
        }
    }
}
