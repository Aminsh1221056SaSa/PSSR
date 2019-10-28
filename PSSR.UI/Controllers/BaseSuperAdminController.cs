
using Microsoft.AspNetCore.Authorization;

namespace PSSR.UI.Controllers
{
    [Authorize(Policy = "dataEventRecordsAdmin")]
    public class BaseSuperAdminController : BaseTraceController
    {
    }
}