using System;
using System.Collections.Generic;
using System.Text;

namespace PSSR.ServiceLayer.WorkPackageSteps
{
    public class WorkPackageStepListDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string WorkPackageName { get; set; }
        public int WorkPackageId { get; set; }
    }
}
