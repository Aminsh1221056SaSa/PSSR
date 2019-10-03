using Microsoft.AspNetCore.Mvc;
using PSSR.Common.CommonModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PSSR.UI.ViewComponents
{
    public class ValuUnitTreeViewComponent : ViewComponent
    {
        public ValuUnitTreeViewComponent()
        {
        }

        public async Task<IViewComponentResult> InvokeAsync(List<ValueUnitModel> ValueUnits)
        {
          var items=  await Task.Run(() =>
            {
               return ValueUnits;
            });

            return View(items);
        }
    }
}
