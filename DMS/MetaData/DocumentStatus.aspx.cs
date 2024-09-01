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



namespace DMS.Shared
{
    public partial class DocumentStatus : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGrid();
            }
        }

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            Utility.ResultType result = new Utility.ResultType();
            GridViewRow row = GridView1.Rows[e.RowIndex];
            string lblId = row.Cells[0].Text;
            DropDownList ddlStatus = (DropDownList)row.FindControl("ddlStatus");
            DMS.BusinessLogic.MetaData rowstatus = new DMS.BusinessLogic.MetaData();
            try
            {
                DbTransaction objDbTransaction = Utility.GetTransaction;
                result = DMS.BusinessLogic.MetaData.Update(int.Parse(ddlStatus.SelectedValue), int.Parse((lblId == "") ? "0" : lblId), objDbTransaction);
                 if (result == Utility.ResultType.Success)
                {
                    lblMessage.Text = "Record updated successfully";
                    objDbTransaction.Commit();
                }
                else
                {
                    lblMessage.Text = "Record couldn't be updated";
                    objDbTransaction.Rollback();
                }
            }
            catch (Exception ee)
            {
                lblMessage.Text = ee.Message.ToString();
            }
            finally
            {
                rowstatus = null;
            }
            GridView1.EditIndex = -1;
            BindGrid();
        }

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            BindGrid();

        }

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            BindGrid();
            lblMessage.Visible = false;
        }
        private void BindGrid()
        {
            //Utility objrowstatus = new Utility();
            DMS.BusinessLogic.Utility objrowstatus = new Utility();
            //DataTable dt = objrowstatus.
            DataTable dt = objrowstatus.LoadDataGrid();
            if (dt.Rows.Count > 0)
            {
                GridView1.DataSource = dt;
                GridView1.DataBind();
            }
            else
                GridView1.DataSource=null;
                GridView1.DataBind();
           }
       protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GridView1.PageIndex = e.NewPageIndex;
                GridView1.DataSource = UserSession.GridData;
                BindGrid();
            }
           catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

       protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
       {
           if (e.CommandName == "Active")
           {
               int index = Convert.ToInt32(e.CommandArgument);
               GridViewRow row = GridView1.Rows[index];
           }
           if (e.CommandName == "InActive")
           {
               int index = Convert.ToInt32(e.CommandArgument);
               GridViewRow row = GridView1.Rows[index];
           }

       }
    }
}