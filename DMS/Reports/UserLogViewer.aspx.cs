using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using DMS.BusinessLogic;


namespace DMS.Reports
{
    public partial class UserLogViewer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                UserSession.GridData = null;
                UserSession.FilterData = null;


                txtFromDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
                txtToDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
                LoadGridData();


                ddl_RoleName();
            }
            ((AjaxControlToolkit.ToolkitScriptManager)this.Master.FindControl("tsmManager")).RegisterPostBackControl(ibtnExport);


        }

        #region Event

        protected void ibtnsubmit_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                Convert.ToDateTime(txtFromDate.Text);
                Convert.ToDateTime(txtToDate.Text);
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Invalid Date .", MainMasterPage.MessageType.Warning);
                return;
            }

            if (Convert.ToDateTime(txtFromDate.Text) > Convert.ToDateTime(txtToDate.Text))
            {
                UserSession.DisplayMessage(this, "To Date Must Be Greater Than From Date .", MainMasterPage.MessageType.Warning);
                return;
            }
            ViewState["selectusername"] = null;

            ddlRoleName.DataBind();
            ddlUserViewer.DataBind();

            loadSelectedData();

        }

      
  

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            DropDownList ddlUserName = gvwUserLogViewer.FooterRow.FindControl("ddlUserName") as DropDownList;
            ViewState["selectusername"] = ddlUserName.SelectedItem.Text;

            if (ddlUserName.SelectedValue != "-1")
            {
                DataRow[] objdatarow = UserSession.GridData.Select("UserName='" + ddlUserName.SelectedItem.Text + "'");

                if (objdatarow.Count() == 0)
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "msg", "alert('No Data Found .');", true);
                    return;

                }
                UserSession.FilterData = objdatarow.CopyToDataTable();
                gvwUserLogViewer.DataSource = UserSession.FilterData;
                gvwUserLogViewer.DataBind();
                LoadUserName();


            }


            else
            {
                UserSession.FilterData = null;
                gvwUserLogViewer.DataSource = UserSession.GridData;
                gvwUserLogViewer.DataBind();
                LoadUserName();
            }



        }

   

        protected void gvwUserLogViewer_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

            if (UserSession.GridData != null)
            {
                DropDownList DdlUserName = gvwUserLogViewer.FooterRow.FindControl("DdrUserName") as DropDownList;
                gvwUserLogViewer.PageIndex = e.NewPageIndex;

                loadSelectedData();

            }
        }

        protected void ibtnExport_Click(object sender, ImageClickEventArgs e)
        {
            DataTable objTable = UserSession.FilterData == null ? UserSession.GridData : UserSession.FilterData;
            if (objTable != null && objTable.Rows.Count > 0)
            {
                if (Request.IsSecureConnection || Utility.IsInternetExplorer(this.Context))
                {
                    string attachment = "Report.xls";
                    Response.ClearHeaders();
                    Response.ContentType = "application/octet-stream";
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + attachment);
                    Response.Clear();
                }
                else
                {
                    this.Context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    this.Context.Response.AppendHeader(@"Pragma", @"no-cache");
                    string attachment = "Report.xls";
                    Response.ClearHeaders();
                    Response.ContentType = "application/octet-stream";
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + attachment);
                    Response.Clear();
                }

                string tab = "";
                foreach (DataColumn objColumn in objTable.Columns)
                {
                    Response.Write(tab + objColumn.ColumnName);
                    tab = "\t";
                }
                Response.Write("\n");
                int i;
                foreach (DataRow objRow in objTable.Rows)
                {
                    tab = "";
                    for (i = 0; i < objTable.Columns.Count; i++)
                    {
                        Response.Write(tab + (objRow[i].ToString().Trim() == string.Empty ? "N/A" : objRow[i].ToString()));
                        tab = "\t";
                    }
                    Response.Write("\n");
                }
                Response.End();

            }
        }
        #endregion

        #region Method

        private void loadSelectedData()
        {


            string querry = "SELECT * FROM vwRoleLogViewer where UserName ='" + ddlUserViewer.SelectedItem.Text + "' and RoleName ='" + ddlRoleName.SelectedItem.Text + "' and (DBO.TO_DATE(LoginTime) between '" + txtFromDate.Text + "' and '" + txtToDate.Text + "')";
            DataTable objDataTable = DataHelper.ExecuteDataTable(querry, null);
            UserSession.GridData = objDataTable;
            UserSession.FilterData = null;
            gvwUserLogViewer.DataSource = UserSession.GridData;
            gvwUserLogViewer.DataBind();

        }


        protected void LoadUserName()
        {
            if (UserSession.GridData != null && UserSession.GridData.Rows.Count > 0)
            {

                DataView objView = UserSession.GridData.DefaultView;

                objView.Sort = "UserName";

                ddlUserViewer.DataSource = objView.ToTable(true, "UserName");
                ddlUserViewer.DataTextField = "UserName";

                ddlUserViewer.Items.Insert(0, new ListItem("--SELECT--", "-1"));
                ddlUserViewer.DataBind();

                if (ViewState["selectusername"] != null)
                {
                    ddlUserViewer.SelectedIndex = ddlUserViewer.Items.IndexOf(ddlUserViewer.Items.FindByText(ViewState["selectusername"].ToString()));
                }


            }

        }


        protected void LoadGridData()
        {
            SqlConnection con = new SqlConnection(Utility.ConnectionString);

            DataTable objDataTable = DataHelper.ExecuteDataTable("SELECT * FROM vwRoleLogViewer", null);
            UserSession.GridData = objDataTable;
            UserSession.FilterData = null;
            gvwUserLogViewer.DataSource = UserSession.GridData;
            gvwUserLogViewer.DataBind();



        }

        protected void ddl_RoleName()
        {
            DataTable objTable = DataHelper.ExecuteDataTable("SELECT ID,RoleName FROM vwRole WHERE Status = 1", null);
            ddlRoleName.DataSource = objTable;
            ddlRoleName.DataTextField = "RoleName";
            ddlRoleName.DataValueField = "ID";
            ddlRoleName.DataBind();
            ddlRoleName.Items.Insert(0, new ListItem("--SELECT--", "0"));
            ddlUserViewer.Items.Insert(0, new ListItem("--SELECT--", "0"));
        }

        protected void ddll_Username()
        {
            DataTable objTable = DataHelper.ExecuteDataTable("SELECT ID,UserName FROM vwUser WHERE RoleID='" + ddlRoleName.SelectedValue + "' AND Status = 1", null);
            ddlUserViewer.DataSource = objTable;

            ddlUserViewer.DataTextField = "UserName";
            ddlUserViewer.DataValueField = "ID";

            ddlUserViewer.DataBind();

            ddlUserViewer.Items.Insert(0, new ListItem("--SELECT--", "0"));
        }

        #endregion

        protected void ddlRoleName_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddll_Username();

        }

        protected void gvwUserLogViewer_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

    }


}