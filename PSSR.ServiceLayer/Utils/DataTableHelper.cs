using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace PSSR.ServiceLayer.Utils
{
    public class DataTableHelper
    {
        public DataTable getUpdateActivityWFTable()
        {
            DataTable dt = new DataTable("MAsset");
            dt.Columns.Add("ActivityId", typeof(long));
            dt.Columns.Add("WF", typeof(float));
            return dt;
        }

        public DataTable getUpdateWBSWFTable()
        {
            DataTable dt = new DataTable("MAsset");
            dt.Columns.Add("Id", typeof(long));
            dt.Columns.Add("WF", typeof(float));
            return dt;
        }
    }
}
