using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PSSR.API.Controllers
{
    [Authorize(Policy = "dataEventRecordsCustomer")]
    [Route("oilapi/[controller]")]
    [ApiController]
    public class BaseManagerController : ControllerBase
    {
    }
}
