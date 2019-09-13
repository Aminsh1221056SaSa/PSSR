using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSSR.ServiceLayer.Utils
{
    public abstract class SortFilterPageOptions
    {
        public const int DefaultPageSize = 10;   //default page size is 10

        private int _pageSize = DefaultPageSize;
        private int _pageNum = 1;
        private string _query = null;

        public string FilterValue { get; set; }

        //-----------------------------------------
        //Paging parts, which require the use of the method

        public int PageNum
        {
            get { return _pageNum; }
            set { _pageNum = value; }
        }

        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = value; }
        }

        public string QueryFilter
        {
            get { return _query; }
            set { _query = value; }
        }
        /// <summary>
        /// This holds the possible page sizes
        /// </summary>
        public int[] PageSizes = new[] { 5, DefaultPageSize, 20, 50, 100, 500, 1000 };



        /// <summary>
        /// This is set to the number of pages available based on the number of entries in the query
        /// </summary>
        public int NumPages { get; internal set; }

        /// <summary>
        /// This holds the state of the key parts of the SortFilterPage parts 
        /// </summary>
        public string PrevCheckState { get; set; }
    }
}
