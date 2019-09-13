using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PSSR.UI.Controllers;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PSSR.UI.Areas.Admin.Controllers
{
    public class AdminConfigController : BaseSuperAdminController
    {
        // GET: /<controller>/
        public IActionResult MDRConfig()
        {
            return View();
        }
    }
}
