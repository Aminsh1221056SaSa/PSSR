
using PSSR.DataLayer.EfClasses.Projects.Activities;
using System.Linq;

namespace PSSR.ServiceLayer.PunchServices.QueryObjects
{
    public static class PunchListDtoSelect
    {
        public static IQueryable<PunchListDto>
             MapPunchToDto(this IQueryable<Punch> punches)
        {
            return punches.Select(punch => new PunchListDto
            {
                ActivityCode = punch.Activity.ActivityCode,
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
                Type=punch.PunchType.Name
            });
        }
    }
}
