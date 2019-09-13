using PSSR.ServiceLayer.ActivityServices.QueryObjects;
using PSSR.ServiceLayer.Utils;
using System;
using System.Linq;

namespace PSSR.ServiceLayer.ActivityServices.Concrete
{
    public class ActivitySortFilterPageOptions : SortFilterPageOptions
    {
        public ActivitySortFilterPageOptions()
        {
            this.PageSize = 20;
        }

        public OrderByOptions OrderByOptions { get; set; }

        public ActivityFilterBy FilterBy { get; set; }
        //
        public int WorkPackageId { get; set; }
        public int LocationId { get; set; }
        public int DesciplineId { get; set; }
        public int WorkPackageStepId { get; set; }
        public int SystemId { get; set; }
        public long SubSystemId { get; set; }
        //grouping
        public bool WorkPackageGroup { get; set; }
        public bool LocationGroup { get; set; }
        public bool SystemGroup { get; set; }
        public bool SubSystemGroup { get; set; }
        public bool DesciplineGroup { get; set; }

        public void SetupRestOfDto<T>(IQueryable<T> query)
        {
            NumPages = (int)Math.Ceiling(
                (double)query.Count() / PageSize);
            PageNum = Math.Min(
                Math.Max(1, PageNum), NumPages);

            var newCheckState = GenerateCheckState();
            if (PrevCheckState != newCheckState)
                PageNum = 1;

            PrevCheckState = newCheckState;
        }

        //----------------------------------------
        //private methods

        /// <summary>
        /// This returns a string containing the state of the SortFilterPage data
        /// that, if they change, should cause the PageNum to be set back to 0
        /// </summary>
        /// <returns></returns>
        private string GenerateCheckState()
        {
            return $"{(int)FilterBy},{FilterValue},{PageSize},{NumPages}";
        }
    }
}
