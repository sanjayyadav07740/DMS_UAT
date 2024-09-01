using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DMS.BusinessLogic;

namespace DMS.Shared
{
    public partial class Upload_CentrumMaster : System.Web.UI.Page
    {
        Utility objUtility = new Utility();
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                DataTable dt = new DataTable();
                objUtility.Result = DMS.BusinessLogic.MetaTemplateManager.SelectMetaTemplateForDropDown(out dt, 57);
                if(dt.Rows.Count>0)
                {
                    ddlMetatemplateName.DataSource = dt;
                    ddlMetatemplateName.DataTextField = "MetaTemplateName";
                    ddlMetatemplateName.DataValueField = "ID";
                    ddlMetatemplateName.DataBind();
                }
            }

        }
    }
}