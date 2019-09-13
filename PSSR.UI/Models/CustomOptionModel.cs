using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSSR.UI.Models
{
    public class CustomOptionModel<TOption,TM>
    {
        public CustomOptionModel(TOption opetion,IEnumerable<TM> items)
        {
            this.Items = items;
            this.Option = opetion;
        }

        public IEnumerable<TM> Items { get; set; }
        public TOption Option { get; set; }
    }
}
