using System;
using System.Collections.Generic;
using System.Text;

namespace PSSR.ServiceLayer.ProjectServices
{
    public class HirecharyPlaneDto
    {
        public HirecharyPlaneDto()
        {
            this.Childeren = new List<HirecharyPlaneDto>();
        }
        public long Id { get; set; }
        public long? ParentId { get; set; }
        public string Title { get; set; }
        public string Resources { get; set; }
        public int Total { get; set; }
        public int Done { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public float Comp { get; set; }
        public DateTime StartDateActual { get; set; }
        //
        public int? WorkPackageId { get; set; }
        public int? LocationId { get; set; }
        public int? DesciplineId { get; set; }
        public int? WorkPackageStepId { get; set; }
        public int? SystemId { get; set; }
        public long? SubSystemId { get; set; }
        public List<HirecharyPlaneDto> Childeren { get; set; }
    }

    public class DesciplinePlanModelDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public int Total { get; set; }
        public int Done { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }

    public class FormDicPlanModelDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Resources { get; set; }
        public int Total { get; set; }
        public int Done { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public DateTime StartDateActual { get; set; }
    }
}
