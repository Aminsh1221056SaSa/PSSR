using PSSR.Common;

namespace PSSR.ServiceLayer.ActivityServices
{
    public class ActivityStatusHistoryListDto
    {
        public long Id { get;  set; }
        public ActivityHolBy HoldBy { get;  set; }
        public string Description { get;  set; }
        public string CreateDate { get;  set; }
        public long ActivityId { get;  set; }
    }
}
