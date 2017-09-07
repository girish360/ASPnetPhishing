using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASPnetPhishing.Models.HelperTools
{
    public class UnitViewModel
    {
        public string SelectedID { get; set; }
        public IEnumerable<SelectListItem> Units { get; set; }
    }
}