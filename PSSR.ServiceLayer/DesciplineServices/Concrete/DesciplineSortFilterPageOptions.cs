using PSSR.ServiceLayer.DesciplineServices.QueryObjects;
using PSSR.ServiceLayer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSSR.ServiceLayer.DesciplineServices.Concrete
{
    public class DesciplineSortFilterPageOptions: SortFilterPageOptions
    {

        public OrderByOptions OrderByOptions { get; set; }

        public DesiplineFilterBy FilterBy { get; set; }


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
