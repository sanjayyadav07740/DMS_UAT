using DMS.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;

namespace DMS.Reports
{
    public partial class DashBoardCentrum : System.Web.UI.Page
    {

        #region PRIVATE MEMBERS ------
       
        Utility objUtility = new Utility();
        Document objdocument = new Document();
        SqlConnection con =new SqlConnection(Utility.ConnectionString);

        #endregion

        #region EVENTS -------

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
            scriptManager.RegisterPostBackControl(this.ibtnExport);
            if (!IsPostBack)
            {
                //LoadBranch();
                LoadDepartment();
               // BindGridReport();//Load all uploded file data in grid on page load
            }
        }

        //protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    LoadDepartment();
        //}
        protected void ddldepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadBranch();
        }
        #endregion

        #region METHODS ------

        private void LoadBranch()
        {
            try
            {
                if (ddldepartment.SelectedItem.Text == "--Select--")
                {
                    UserSession.DisplayMessage(this, "Please Select Department", MainMasterPage.MessageType.Warning);
                    ddlBranch.Items.Clear();
                    ddlBranch.Items.Insert(0, new ListItem("--Select--", "0"));
                    return;

                }
                else
                {
                    //string strQuery = "select distinct(Branch) from Centrum_Master where Department='" + ddldepartment.SelectedItem.Text + "'";
                    string strQuery = @"select CategoryName,Id from Category where RepositoryID=57 and MetaTemplateID=" + ddldepartment.SelectedItem.Value;
                    SqlCommand cmd = new SqlCommand(strQuery, con);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    con.Close();
                    ddlBranch.DataSource = dt;
                   // ddlBranch.DataTextField = "Branch";
                    ddlBranch.DataTextField ="CategoryName";
                    ddlBranch.DataValueField = "id";
                    ddlBranch.DataBind();
                    ddlBranch.Items.Insert(0, new ListItem("--Select--", "0"));
                }
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
            }
        }

        private void LoadDepartment()
        {
            try
            {
                //string strbranch = ddlBranch.SelectedItem.Text.ToString();
              //  string strQuery = "select distinct(department) from Centrum_Master";
                string strQuery = @"Select MetaTemplateName,id from Metatemplate where RepositoryID=57";
                SqlCommand cmd = new SqlCommand(strQuery, con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                con.Close();
                ddldepartment.DataSource = dt;
                //ddldepartment.DataTextField = "department";.
                ddldepartment.DataTextField = "MetaTemplateName";
                ddldepartment.DataValueField = "id";
                ddldepartment.DataBind();
                ddldepartment.Items.Insert(0, new ListItem("--Select--", "0"));
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
            }
        }

//        private void BindGridReport()
//        {
//            try
//            {


//                string strQuery = @"select CM.Customer_Name,CM.Pan_No,CM.Branch,CM.Department,CM.Account_Opening_Date,CM.Account_Closing_Date,A.DocumentName,A.FolderName,A.PageCount,A.Size,A.DocumentType,A.UpdatedOn As DateOfModification,A.UserName as DocUploadedBy,A.CreatedOn as DateOfUpload,A.ApprovarName,A.Status,A.DateOfApprove from(
//select D.DocumentName,D.ID,M.MetaDataCode,D.PageCount,D.Size,D.DocumentType,U.UserName,D.CreatedOn,D.UpdatedOn,D.CreatedBy,
//C.CategoryName,MT.MetaTemplateName,f.FolderName,AR.UserName as ApprovarName,AR.Status,AR.DateOfApprove,
//(select FolderName from Folder where ID=F.ParentFolderID) as Pan_no from Document D 
//left join DocApproveRejectDetails AR on D.ID=AR.DocId
//inner join MetaData M on D.MetaDataID=M.ID
//inner join Folder F on F.id=M.FolderID
//inner join MetaTemplate MT on MT.ID=M.MetaTemplateID 
//inner join Category C on C.ID=M.CategoryID
//inner join Repository R on R.ID=M.RepositoryID
//inner join vwUser U on U.ID=d.CreatedBy)AS A
//inner join (select distinct(Pan_No),Customer_Name,Branch, Department,Account_Opening_Date, Account_Closing_Date from Centrum_Master) as CM on CM.Pan_No=A.Pan_no";

//                SqlCommand cmd = new SqlCommand(strQuery, con);
//                SqlDataAdapter da = new SqlDataAdapter(cmd);
//                DataTable dt = new DataTable();
//                da.Fill(dt);
//                gvDashboard.DataSource = dt;
//                Session["DocData"] = dt;
//                gvDashboard.DataBind();
//                con.Close(); 

 
//            }
//            catch (Exception ex)
//            {
//                LogManager.ErrorLog(Utility.LogFilePath, ex);
//                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
//            }
//        }

        #endregion

        protected void ibtnSubmit_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (ddldepartment.SelectedItem.Text.ToString() == "--Select--")
                {
                    UserSession.DisplayMessage(this, "Please Select Department.", MainMasterPage.MessageType.Warning);
                    return;
                }

                   string Department = ddldepartment.SelectedItem.Text.ToString();
                    //string MetatemplateId = ddldepartment.SelectedItem.Value;
                    string Branch = ddlBranch.SelectedItem.Value;                   
                    string AccOpFrm;
                    string AccOpTo;
                    string AccClFrm;
                    string AccClTo;
                    string DateOfUpFrm;
                    string DateOfUpTo;
                    string ModifyDtFrm;
                    string ModifyDtTo;
                    string Status;


                if(Convert.ToInt32(Branch)==0)
                {
                    Branch = null;
                }
                else { Branch = ddlBranch.SelectedItem.Value; }
                    if (string.IsNullOrEmpty(txtOpnFrom.Text) || string.IsNullOrEmpty(txtOpnTo.Text))
                {
                     AccOpFrm =null;
                     AccOpTo=null;
                }
                else
                {
                     AccOpFrm = txtOpnFrom.Text;
                     AccOpTo = txtOpnTo.Text;
                }

                if (string.IsNullOrEmpty(txtFromClose.Text) || string.IsNullOrEmpty(txtToClose.Text))
                {
                     AccClFrm=null;
                     AccClTo = null;
                }
                else
                {
                     AccClFrm = txtFromClose.Text;
                     AccClTo = txtToClose.Text;
                }
                if (string.IsNullOrEmpty(txtUploadFrom.Text) || string.IsNullOrEmpty(txtUploadTo.Text)) { DateOfUpFrm = null; DateOfUpTo = null; }
                else{ DateOfUpFrm = txtUploadFrom.Text; DateOfUpTo = txtUploadTo.Text;}

                if (string.IsNullOrEmpty(txtModifyFrom.Text) || string.IsNullOrEmpty(txtModifyTo.Text)) { ModifyDtFrm = null; ModifyDtTo = null; }
                else { ModifyDtFrm = txtModifyFrom.Text; ModifyDtTo = txtModifyTo.Text; }


                if (ddlStatus.SelectedItem.Text.ToString() == "--Select--")
                {
                    Status = null;
                }
                else
                {
                    Status = ddlStatus.SelectedItem.Text.Trim().ToString();
                }

                    DataTable dtDocSearch = new DataTable();
                    DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;
                    DbParameter[] objDbParameter = new DbParameter[11];

                    objDbParameter[0] = objDbProviderFactory.CreateParameter();
                    objDbParameter[0].ParameterName = "@AccOpFrm";
                    objDbParameter[0].Value = AccOpFrm;

                    objDbParameter[1] = objDbProviderFactory.CreateParameter();
                    objDbParameter[1].ParameterName = "@AccOpTo";
                    objDbParameter[1].Value = AccOpTo;

                    objDbParameter[2] = objDbProviderFactory.CreateParameter();
                    objDbParameter[2].ParameterName = "@AccClFrm";
                    objDbParameter[2].Value = AccClFrm;

                    objDbParameter[3] = objDbProviderFactory.CreateParameter();
                    objDbParameter[3].ParameterName = "@AccClTo";
                    objDbParameter[3].Value = AccClTo;

                    objDbParameter[4] = objDbProviderFactory.CreateParameter();
                    objDbParameter[4].ParameterName = "@DateOfUpFrm";
                    objDbParameter[4].Value = DateOfUpFrm;

                    objDbParameter[5] = objDbProviderFactory.CreateParameter();
                    objDbParameter[5].ParameterName = "@DateOfUpTo";
                    objDbParameter[5].Value = DateOfUpTo;

                    objDbParameter[6] = objDbProviderFactory.CreateParameter();
                    objDbParameter[6].ParameterName = "@ModifyDtFrm";
                    objDbParameter[6].Value = ModifyDtFrm;

                    objDbParameter[7] = objDbProviderFactory.CreateParameter();
                    objDbParameter[7].ParameterName = "@ModifyDtTo";
                    objDbParameter[7].Value = ModifyDtTo;

                    objDbParameter[8] = objDbProviderFactory.CreateParameter();
                    objDbParameter[8].ParameterName = "@Status";
                    objDbParameter[8].Value = Status;

                    objDbParameter[9] = objDbProviderFactory.CreateParameter();
                    objDbParameter[9].ParameterName = "@Branch";
                    objDbParameter[9].Value = Branch;

                    objDbParameter[10] = objDbProviderFactory.CreateParameter();
                    objDbParameter[10].ParameterName = "@Department";
                    objDbParameter[10].Value = Department;

                    dtDocSearch = DataHelper.ExecuteDataTableForProcedure("SP_S_SelectDashBordData1", null, objDbParameter);
                    Session["DocData"] = dtDocSearch;
                    if (dtDocSearch.Rows.Count < 0)
                    {
                        UserSession.DisplayMessage(this, "Sorry ,No Data Found.", MainMasterPage.MessageType.Error);
                    }
                    else
                    {
                        gvDashboard.DataSource=null;
                        gvDashboard.Visible = true;
                        gvDashboard.DataSource = dtDocSearch;
                        gvDashboard.DataBind();
                    }
               

            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                UserSession.DisplayMessage(this, "Please Try Some Other Combinations.", MainMasterPage.MessageType.Warning);
            }
        }

        protected void ibtnExport_Click(object sender, ImageClickEventArgs e)
        {
            
             DataTable dt = new DataTable();
            dt=(DataTable) Session["DocData"];
            ExportToExcel(dt,"Centrum DashBoard");
           
        }

        public void ExportToExcel(DataTable objDataTable, string strFileName)
        {
 	         try
            {
                HttpContext.Current.Response.ContentType = "application/octet-stream";
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + strFileName.Trim() + ".xls");
                System.Text.StringBuilder objStringBuilder = new System.Text.StringBuilder();
                objStringBuilder.Append("<table style='border:1px solid blue;'>");
                objStringBuilder.Append("<tr>");
                foreach (DataColumn objDataColumn in objDataTable.Columns)
                {
                    objStringBuilder.Append("<th style='border:1px solid black; background:black; font-weight: bold; color:white;'>");
                    objStringBuilder.Append(objDataColumn.ColumnName);
                    objStringBuilder.Append("</th>");
                }
                objStringBuilder.Append("</tr>");


                foreach (DataRow objDataRow in objDataTable.Rows)
                {
                    objStringBuilder.Append("<tr>");
                    foreach (DataColumn objDataColumn in objDataTable.Columns)
                    {
                        objStringBuilder.Append("<td style='border:1px solid black;'>");
                        objStringBuilder.Append(objDataRow[objDataColumn.ColumnName].ToString() == string.Empty ? "N/A" : objDataRow[objDataColumn.ColumnName].ToString());
                        objStringBuilder.Append("</td>");
                    }
                    objStringBuilder.Append("</tr>");
                }
                objStringBuilder.Append("</table>");
                HttpContext.Current.Response.Write(objStringBuilder.ToString());
                HttpContext.Current.Response.End();
                
            }
            catch (Exception ex)
            {
                //UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured.", MainMasterPage.MessageType.Error);
                //LogManager.ErrorLog(Utility.LogFilePath, ex);
                throw ex;
            }
        }

        protected void gvDashboard_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvDashboard.PageIndex = e.NewPageIndex;
                gvDashboard.DataSource = Session["DocData"];
                gvDashboard.DataBind();
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
            }
        }

       

    }
}