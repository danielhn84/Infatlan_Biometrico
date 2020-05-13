using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BiometricoWeb.pages.servicios
{
    public partial class organizationalChart : System.Web.UI.Page
    {
        public string EmployeeName { get; set; }
        public string ManagerName { get; set; }
        public string Role { get; set; }
        public string ClassStyle { get; set; }

        protected void Page_Load(object sender, EventArgs e){

        }

    }
}