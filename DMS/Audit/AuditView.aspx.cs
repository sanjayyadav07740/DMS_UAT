using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DMS.BusinessLogic;
using System.IO;
using Ionic.Zip;
using System.Data.Common;

namespace DMS.Audit
{
    public partial class AuditView : System.Web.UI.Page
    {

        #region Private Memeber
        Utility objUtility = new Utility();
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request["Type"] != null)
                {

                }

            }
            UserSession.GridData = null;
            UserSession.MetaDataID = 0;
        }


        private void BindGrid()
        {
            //DMS.BusinessLogic.Utility objrow = new Utility();
            //DataTable dt = objrow.LoadDataRow();
            //if (dt.Rows.Count > 0)
            //{
            //    grvaudit.DataSource = dt;
            //    grvaudit.DataBind();
            //}
            //else
            //{
            //    grvaudit.DataSource = null;
            //    grvaudit.DataBind();
            //}
        }
        #region GridView1 Event Handlers

        protected void grvaudit_rowcommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Details")
            {
                int rowindex = Convert.ToInt32(e.CommandArgument);
                
            }
            grvaudit.EditIndex = -1;
            BindGrid();
        }

        protected void grvaudit_onrowdatabound(object sender, GridViewRowEventArgs e)
        {
            GridViewRow row = e.Row;
            string strSort = string.Empty;
            if (row.DataItem == null)
            {
                return;
            }
             GridView gv = new GridView();
            gv = (GridView)row.FindControl("grvauditreport");
            BindGrid();
        }

       
        #endregion


        #region GridView2 Event Handlers


       
       
             
                    
        #endregion

     
    }
}