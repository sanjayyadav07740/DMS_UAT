using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMS.BusinessLogic
{
    public class Module
    {
        public int ModuleID { get; set; }
        public string ModuleName { get; set; }
        public string ModuleDescription { get; set; }
        public int ParentModuleID { get; set; }
        public string NavigationUrl { get; set; }
        public int DisplayOrder { get; set; }
        public int Status { get; set; }
    }
}
