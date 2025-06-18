using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDIC_TAXATION_MODELS.ViewManager
{
    public class ViewSelectors
    {
        public IEnumerable<SelectListItem> Localgovernments { get; set; }
        //public IEnumerable<SelectListItem> Department { get; set; }
    }
}
