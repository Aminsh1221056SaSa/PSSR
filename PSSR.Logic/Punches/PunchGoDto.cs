using System;
using System.Collections.Generic;
using System.Text;

namespace PSSR.Logic.Punches
{
    public class PunchGoDto
    {
        public long Id { get; set; }
        public string ApproveBy { get; set; }
        public string CheckBy { get; set; }
        public string ClearBy { get; set; }
        public DateTime? CheckDate { get; set; }
        public DateTime? ClearDate { get; set; }
    }
}
