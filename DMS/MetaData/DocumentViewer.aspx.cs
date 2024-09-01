using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DMS.BusinessLogic;

namespace DMS.Shared
{
    public partial class SharedDocumentViewer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.UrlReferrer != null)
                {
                    ViewState["LASTPAGEURL"] = Request.UrlReferrer.ToString().ToLower();
                }
            }
        }
    }
}