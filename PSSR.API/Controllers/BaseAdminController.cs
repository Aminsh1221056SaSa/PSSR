using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PSSR.API.Controllers
{
    [Authorize(Policy = "dataEventRecordsAdmin")]
    [Route("oilapi/[controller]")]
    [ApiController]
    public class BaseAdminController : ControllerBase
    {
    }
}
