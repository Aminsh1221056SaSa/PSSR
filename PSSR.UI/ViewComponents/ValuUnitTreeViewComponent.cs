using Microsoft.AspNetCore.Mvc;
using PSSR.DataLayer.EfClasses;
using PSSR.DataLayer.EfCode;
using PSSR.ServiceLayer.ValueUnits.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSSR.UI.ViewComponents
{
    public class ValuUnitTreeViewComponent : ViewComponent
    {
        private readonly EfCoreContext _context;
        public ValuUnitTreeViewComponent(EfCoreContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
           var valueUnitList =
                  new ListValueUnitService(_context);

          var items=  await Task.Run(() =>
            {
               return valueUnitList.GetAllValueUnits().ToList();
            });

            return View(items);
        }
    }
}
