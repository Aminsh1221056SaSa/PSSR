using Microsoft.EntityFrameworkCore;
using PSSR.DataLayer.EfCode;
using PSSR.DataLayer.QueryObjects;
using PSSR.ServiceLayer.ActivityServices;
using PSSR.ServiceLayer.PunchServices.QueryObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSSR.ServiceLayer.PunchServices.Concrete
{
    public class ListPunchService
    {
        private readonly EfCoreContext _context;

        public ListPunchService(EfCoreContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PunchEditableListDto>> GetActivityPunchs(long activityId)
        {
            return await _context.Activites.Where(s => s.Id == activityId).SelectMany(s => s.Punchs)
                .Select(s => new PunchEditableListDto
                {
                    ActivityId = s.ActivityId,
                    OrginatedDate = s.OrginatedDate.ToString("d"),
                    OrginatedBy = s.OrginatedBy,
                    Code = s.Code,
                    Id = s.Id,
                    PunchTypeId = s.PunchTypeId,
                    Type = s.PunchType.Name,
                    IsEditable = s.ClearDate.HasValue,
                    IsApprove=s.CheckDate.HasValue

                }).ToListAsync();
        }

        public async Task<PunchEditableListDto> GetPunchGoDetails(long punchId)
        {
            var punch = await _context.Punchs.Where(s => s.Id == punchId).Include(s=>s.Activity).FirstOrDefaultAsync();
            if (punch == null) return null;
            return new PunchEditableListDto
            {
                ActivityId = punch.ActivityId,
                Code = punch.Code,
                Id = punch.Id,
                OrginatedDate = punch.OrginatedDate.ToString("d"),
                OrginatedBy = punch.OrginatedBy,
                PunchTypeId = punch.PunchTypeId,
                IsApprove = punch.CheckDate.HasValue,
                IsClear = punch.ClearDate.HasValue,
                CheckDate = punch.CheckDate != null ? punch.CheckDate.Value.ToString("d") : "",
                ClearDate = punch.ClearDate != null ? punch.ClearDate.Value.ToString("d") : "",
                ApproveBy = punch.ApproveBy,
                CheckBy = punch.CheckBy,
                CreatedBy = punch.ClearBy,
            };
        }

        public async Task<PunchListDto> GetPunchDetails(long punchId)
        {
            var punch = await _context.Punchs.Where(s => s.Id == punchId).Include(s => s.Activity).FirstOrDefaultAsync();
            if (punch == null) return null;
            return new PunchListDto
            {
                ActivityId = punch.ActivityId,
                Code = punch.Code,
                ActualMh = punch.ActualMh,
                ApproveBy = punch.ApproveBy,
                CheckBy = punch.CheckBy,
                DefectDescription = punch.DefectDescription,
                CheckDate = punch.CheckDate != null ? punch.CheckDate.Value.ToString("d") : "",
                ClearDate = punch.ClearDate != null ? punch.ClearDate.Value.ToString("d") : "",
                Id = punch.Id,
                ClearPlan = punch.ClearPlan,
                CorectiveAction = punch.CorectiveAction,
                OrginatedDate = punch.OrginatedDate.ToString("d"),
                CreatedBy = punch.ClearBy,
                EnginerigRequired = punch.EnginerigRequired,
                EstimateMh = punch.EstimateMh,
                MaterialRequired = punch.MaterialRequired,
                OrginatedBy = punch.OrginatedBy,
                PunchTypeId = punch.PunchTypeId,
                VendorName = punch.VendorName,
                VendorRequired = punch.VendorRequired,
                ActivityCode = punch.Activity.ActivityCode,
                Progress = punch.Activity.Progress,
                Condition = punch.Activity.Condition,
                IsEditable = punch.ClearDate.HasValue,
            };
        }

        public async Task<IEnumerable<ActivityDocumentListDto>> GetDocuments(long punchId)
        {
            return await _context.ActivityDocuments.Where(s=>s.PunchId==punchId)
                .Select(s => new ActivityDocumentListDto
                {
                    Id=s.Id,
                    ActivityId = s.ActivityId,
                    CreatedDate = s.CreatedDate.ToString("dddd, dd MMMM yyyy"),
                    Description = s.Description,
                    PunchId = s.PunchId,
                    PunchCode = s.Punch.Code
                }).DefaultIfEmpty().ToListAsync();
        }

        public Task<IQueryable<PunchListDto>> SortFilterPage(PunchSortFilterPageOptions options,Guid ProjectId)
        {
            return Task.Run(() =>
            {
                var punchQuery = _context.Punchs.Where(s=>s.PunchType.ProjectId==ProjectId)
               .AsNoTracking()
               .Where(item => item.Code.Contains(options.QueryFilter) || string.IsNullOrWhiteSpace(options.QueryFilter))
               .FilterPunchBy(options.FilterBy, options.FilterValue)
               .OrderPunchBy(options.OrderByOptions)
               .MapPunchToDto();
                options.SetupRestOfDto(punchQuery);

                return punchQuery.Page(options.PageNum - 1, options.PageSize);
            });
        }
    }
}
