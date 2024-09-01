using DMS.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DMS.Shared
{
    public partial class DocumentVerificationCentrum : System.Web.UI.Page
    {
        #region PRIVATE MEMBERS ------
        Utility objUtility = new Utility();
        Document objdocument = new Document();       
        SqlConnection con = new SqlConnection(Utility.ConnectionString);

        #endregion

        #region EVENTS -------
        protected void Page_Load(object sender, EventArgs e)
        {
          
            if(!IsPostBack)
            {
               
                LoadRepository();
            }
            if (Request.UrlReferrer != null)
            {
                ViewState["LASTPAGEURL"] = Request.UrlReferrer.ToString().ToLower();

                if (ViewState["LASTPAGEURL"].ToString().Contains("viewdocumentforsearch"))
                {
                    //BusinessLogic.MetaData objMetaData = new BusinessLogic.MetaData();
                    //objMetaData = (Session["SearchCriteria"]);
                    //ddlRepositoryName.SelectedValue=
                   
                //    BindgvwDocumnetList();
                    //ddlMetaTemplateName.DataSource =Session["MetaTable"];
                    LoadRepository();
                    ddlRepositoryName.SelectedValue=Session["Repo"].ToString();
                    LoadMetatemplate();
                    ddlMetaTemplateName.SelectedValue = Session["MetaValue"].ToString();
                    LoadCategory();
                    ddlCategoryName.SelectedValue = Session["Category"].ToString();
                    BindgvwDocumnetList();

                    if (Session["FilterBy"] != null && Session["FilterText"] != null)
                    {
                        GridFilter();
                    }

                    if (Session["Approve"] == "approve" && Session["Reject"]==null)
                    {
                          UserSession.DisplayMessage(this, "Document is Approved Successfully .", MainMasterPage.MessageType.Success);
                          Session["Approve"] = null;
                    }
                    if (Session["Approve"] == null && Session["Reject"] == "reject")
                     {
                         UserSession.DisplayMessage(this, "Document is Rejected Successfully .", MainMasterPage.MessageType.Success);
                         Session["Reject"] = null;
                     }
                    
                }
            }             
           

        }

       

        protected void ddlRepositoryName_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadMetatemplate();
        }

        protected void ddlMetaTemplateName_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadCategory();
        }      

        #endregion

        #region METHODS ------
        private void LoadRepository()
        {
            try
            {
                string strQuery = "Select RepositoryName, id from Repository where id=57";
                con.Open();
                SqlCommand cmd = new SqlCommand(strQuery, con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                con.Close();
                ddlRepositoryName.DataSource = dt;
                ddlRepositoryName.DataTextField = "RepositoryName";
                ddlRepositoryName.DataValueField = "id";
                ddlRepositoryName.DataBind();
                ddlRepositoryName.Items.Insert(0, new ListItem("--Select--", "-1"));
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
            }
        }

        private void LoadMetatemplate()
        {
            try
            {
                string strRepository = ddlRepositoryName.SelectedItem.Value.ToString();
                string strQuery = "select MetaTemplateName,id from MetaTemplate where RepositoryID='" + strRepository + "'";
                con.Open();
                SqlCommand cmd = new SqlCommand(strQuery, con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                con.Close();
                ddlMetaTemplateName.DataSource = dt;
                ddlMetaTemplateName.DataTextField = "MetaTemplateName";
                ddlMetaTemplateName.DataValueField = "id";
                ddlMetaTemplateName.DataBind();
                ddlMetaTemplateName.Items.Insert(0, new ListItem("--Select--", "-1"));
                Session["MetaTable"] = dt;
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
            }
        }

        private void LoadCategory()
        {
            try
            {
                string strMetatemplate = ddlMetaTemplateName.SelectedItem.Value.ToString();
                string strQuery = "select CategoryName,id from Category where MetaTemplateID='" + strMetatemplate + "'";
                con.Open();
                SqlCommand cmd = new SqlCommand(strQuery, con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                con.Close();
                ddlCategoryName.DataSource = dt;
                ddlCategoryName.DataTextField = "CategoryName";
                ddlCategoryName.DataValueField = "id";
                ddlCategoryName.DataBind();
                ddlCategoryName.Items.Insert(0, new ListItem("--Select--", "-1"));
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
            }
        }

        #endregion

        protected void ibtnSubmit_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (ddlRepositoryName.SelectedItem.Text.ToString() == "--Select--")
                {
                    UserSession.DisplayMessage(this, "Please Select Repository.", MainMasterPage.MessageType.Warning);
                    return;
                }
                gvwAppRejectDtls.Visible = false;
                lblHeadingAppRejGrid.Visible = false;
                BindgvwDocumnetList();
            }
            catch (Exception ex)
            {
               LogManager.ErrorLog(Utility.LogFilePath, ex);
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
            }
        }

        private void BindgvwDocumnetList()
        {
            DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;
           // string Repository = ddlRepositoryName.SelectedItem.Value;
           // int Metatemplate = Convert.ToInt32(ddlMetaTemplateName.SelectedItem.Value.Trim());
           // string Departmentname = ddlMetaTemplateName.SelectedItem.Text.ToString().Trim();
           // string Category = ddlCategoryName.SelectedItem.Text.ToString();

            BusinessLogic.MetaData objMetaData = new BusinessLogic.MetaData()
            {
                RepositoryID =  Convert.ToInt32(ddlRepositoryName.SelectedItem.Value.Trim()),
                MetaTemplateID = Convert.ToInt32(ddlMetaTemplateName.SelectedItem.Value.Trim()),
                CategoryID = Convert.ToInt32(ddlCategoryName.SelectedItem.Value.Trim())
          
            };
            

            Session["SearchCriteria"]=objMetaData;

         
            DbParameter[] objDbParameter = new DbParameter[3];           

            objDbParameter[0] = objDbProviderFactory.CreateParameter();
            objDbParameter[0].ParameterName = "@Repository";
            if (Convert.ToInt32(ddlRepositoryName.SelectedItem.Value) == -1)
            {
                objDbParameter[0].Value = null;
            }
            else
            {
               objDbParameter[0].Value =objMetaData.RepositoryID;
               // objDbParameter[0].Value = ddlRepositoryName.SelectedItem.Value;
            }

            objDbParameter[1] = objDbProviderFactory.CreateParameter();
            objDbParameter[1].ParameterName = "@metatemplate";
            if (Convert.ToInt32(ddlMetaTemplateName.SelectedItem.Value) == -1)
            {
                objDbParameter[1].Value = null;
            }
            else
            {
                objDbParameter[1].Value =objMetaData.MetaTemplateID;
               // objDbParameter[1].Value = ddlMetaTemplateName.SelectedItem.Value; 
            }

            objDbParameter[2] = objDbProviderFactory.CreateParameter();
            objDbParameter[2].ParameterName = "@category";
            if (Convert.ToInt32(ddlCategoryName.SelectedItem.Value) == -1)
            {
                objDbParameter[2].Value = null;
            }
            else
            {
                objDbParameter[2].Value = objMetaData.CategoryID;
               // objDbParameter[2].Value = ddlCategoryName.SelectedItem.Value;
            }
          
            
//            string strQuery = @"select A.ID,A.DocumentName, A.Pan_no,k.Customer_Name,A.MetaTemplateName,a.FolderName,a.CreatedOn,k.Account_Opening_Date, k.Account_Closing_Date,K.Department from (select D.DocumentName,D.ID,M.MetaDataCode, D.CreatedOn,C.CategoryName,MT.MetaTemplateName,f.FolderName, 
//(select FolderName from Folder where ID=F.ParentFolderID) as Pan_no from Document D 
// left join DocApproveRejectDetails AR on AR.DocId=D.ID
//  inner join MetaData M on  D.MetaDataID=M.ID
//   inner join Folder F on F.id=M.FolderID  
//   inner join MetaTemplate MT on MT.ID=M.MetaTemplateID  
//   inner join Category C on C.ID=M.CategoryID 
//   inner join Repository R on R.ID=M.RepositoryID
//    where r.ID=@Repository and MT.ID=@Metatemplate and AR.DocId IS null) As A 
//	inner join (select distinct(Pan_No),Department,Customer_Name,Account_Opening_Date, Account_Closing_Date from Centrum_Master) as K on k.Pan_No=A.Pan_no where k.Department= '" + Departmentname + "'";
           DataTable objDataTable = new DataTable();


//            DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;
//            DbParameter[] objDbParameter = new DbParameter[2];

//            objDbParameter[0] = objDbProviderFactory.CreateParameter();
//            objDbParameter[0].ParameterName = "Repository";
//            objDbParameter[0].Value = Repository;

//            objDbParameter[1] = objDbProviderFactory.CreateParameter();
//            objDbParameter[1].ParameterName = "Metatemplate";
//            objDbParameter[1].Value = Metatemplate;

//            ////objDbParameter[2] = objDbProviderFactory.CreateParameter();
//            ////objDbParameter[2].ParameterName = "Category";
//            ////objDbParameter[2].Value = Category;

//            objDataTable = DataHelper.ExecuteDataTable(strQuery, objDbParameter);
           objDataTable = DataHelper.ExecuteDataTableForProcedure("SP_S_SelectDocumentVarificationCentrum", null, objDbParameter);
            Session["DocVerification"] = objDataTable;
            UserSession.GridData = objDataTable;
            if (objDataTable.Rows.Count<= 0)
            {
                UserSession.DisplayMessage(this, "Sorry ,No Data Found.", MainMasterPage.MessageType.Error);
            }
            else
            {
                gvwDocumentList.Visible = true;
                gvwDocumentList.DataSource = objDataTable;
                gvwDocumentList.DataBind();                 
               
            }
        }

        protected void gvwDocumentList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvwDocumentList.PageIndex = e.NewPageIndex;
            gvwDocumentList.DataSource = Session["DocVerification"];
            gvwDocumentList.DataBind();
        }

        public static string GetMacAddress()
        {
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                // Only consider Ethernet network interfaces
                if (nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet &&
                    nic.OperationalStatus == OperationalStatus.Up)
                {
                    return nic.GetPhysicalAddress().ToString();
                }
            }
            return null;
        }

        protected void gvwDocumentList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.ToLower().Trim() == "documentview")
                {
                    Session["Repo"] = ddlRepositoryName.SelectedValue;
                    Session["MetaValue"] = ddlMetaTemplateName.SelectedValue;
                    Session["Category"] = ddlCategoryName.SelectedValue;
                    int intRowIndex = Convert.ToInt32(e.CommandArgument);                 
                    string strDocumentID = gvwDocumentList.DataKeys[intRowIndex].Values["ID"].ToString().Trim();
                    string strDocumentName = gvwDocumentList.DataKeys[intRowIndex].Values["DocumentName"].ToString().Trim();
                    //string strStatus = gvwDocumentList.DataKeys[intRowIndex].Values["DocumentStatusID"].ToString().Trim();
                    Session["DocIdVerify"] = strDocumentID;
                    Session["DocId"] = strDocumentID;
                    //enter in audit log
                    string strHostName = "";
                    strHostName = System.Net.Dns.GetHostName();

                    IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(strHostName);

                    IPAddress[] addr = ipEntry.AddressList;
                    Report objReport = new Report();
                    string IPAddress = addr[addr.Length - 1].ToString();
                    //DateTime Date = Convert.ToDateTime(DateTime.Today.ToShortDateString());
                    //DateTime Logintime = Convert.ToDateTime(DateTime.Now);
                    DateTime LoginDate = DateTime.Today;
                    string Activity = "Document Viewed";

                    string MacAddress = GetMacAddress();
                    objReport.InsertAuditLog(IPAddress, MacAddress, Activity, strDocumentName, UserSession.UserID);
                    Response.Redirect("../MetaData/ViewDocumentForSearch.aspx?DOCID=" + strDocumentID, false);
                  // Response.Redirect("../MetaData/DocumentViewer.aspx?DOCID=" + strDocumentID, false);
                }

                if (e.CommandName.ToLower().Trim() == "docapprove")
                  {
                      int intRowIndex = Convert.ToInt32(e.CommandArgument);
                      string strDocumentID = gvwDocumentList.DataKeys[intRowIndex].Values["ID"].ToString().Trim();
                      string strDocumentName = gvwDocumentList.DataKeys[intRowIndex].Values["DocumentName"].ToString().Trim();
                      DbTransaction objDbTransaction = Utility.GetTransaction;
                      DocumentManager objDocumentManager = new DocumentManager();
                      DMS.BusinessLogic.DocumentStatus objDocumentStaus = new DMS.BusinessLogic.DocumentStatus();

                      objDocumentStaus.DocId = Convert.ToInt32(strDocumentID);
                     
                      objDocumentStaus.UserName = SelectUserName();
                      objDocumentStaus.statusAproveRej = "Approve";
                      objDocumentStaus.UserId = UserSession.UserID;
                      objDocumentStaus.ApprovedOn = DateTime.Now;
                      objUtility.Result = objDocumentManager.InsertDocApproveRejectDetails(objDocumentStaus, objDbTransaction);

                      switch (objUtility.Result)
                      {
                          case Utility.ResultType.Success:
                              objDbTransaction.Commit();
                              UserSession.DisplayMessage(this, "Document is Approved Successfully .", MainMasterPage.MessageType.Success);
                              //enter in audit log
                             string strHostName = "";
                            strHostName = System.Net.Dns.GetHostName();

                            IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(strHostName);

                            IPAddress[] addr = ipEntry.AddressList;
                            Report objReport = new Report();
                            string IPAddress = addr[addr.Length - 1].ToString();
                            //DateTime Date = Convert.ToDateTime(DateTime.Today.ToShortDateString());
                            //DateTime Logintime = Convert.ToDateTime(DateTime.Now);
                            DateTime LoginDate = DateTime.Today;
                            string Activity = "Document Approved";

                            string MacAddress = GetMacAddress();
                           objReport.InsertAuditLog(IPAddress, MacAddress, Activity, strDocumentName, UserSession.UserID);
                              break;
                          case Utility.ResultType.Failure:
                          case Utility.ResultType.Error:
                              objDbTransaction.Rollback();
                              UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                              break;
                      }
                      BindgvwDocumnetList();
                   
                          Session["DirectDocRowApprove"] = 1;
                          GridFilter();
                      
                  }
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
            }
        }

        protected void gvwDocument_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                Utility.SetGridHoverStyle(e);
                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    e.Row.Cells[0].ColumnSpan = 8;
                    for (int i = 1; i < 9; i++)
                    {
                        e.Row.Cells[i].Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }
        private void ApproveRejectDetailGrid()
        {
            try
            {
                //This Function is work on Filter of gvwDocumentList Grid. To show list of Documents Approve and Rejected on the basis of Pan no or Cust Name.
                string strFilterBy = ((DropDownList)gvwDocumentList.FooterRow.FindControl("ddlFilterGrid")).SelectedValue.Trim();
                string strFilterText =Convert.ToString( Session["FilterText"]);// ((TextBox)gvwDocumentList.FooterRow.FindControl("txtFilterGrid")).Text.Trim();

                BusinessLogic.MetaData objMetaData = new BusinessLogic.MetaData()
                {
                    RepositoryID = Convert.ToInt32(ddlRepositoryName.SelectedItem.Value.Trim()),
                    MetaTemplateID = Convert.ToInt32(ddlMetaTemplateName.SelectedItem.Value.Trim()),
                    CategoryID = Convert.ToInt32(ddlCategoryName.SelectedItem.Value.Trim())

                };

                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;
                DbParameter[] objDbParameter = new DbParameter[5];
                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "@Repository";
                if (Convert.ToInt32(ddlRepositoryName.SelectedItem.Value) == -1)
                {
                    objDbParameter[0].Value = null;
                }
                else
                {
                    objDbParameter[0].Value = objMetaData.RepositoryID;                   
                }

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "@metatemplate";
                if (Convert.ToInt32(ddlMetaTemplateName.SelectedItem.Value) == -1)
                {
                    objDbParameter[1].Value = null;
                }
                else
                {
                    objDbParameter[1].Value = objMetaData.MetaTemplateID;                  
                }

                objDbParameter[2] = objDbProviderFactory.CreateParameter();
                objDbParameter[2].ParameterName = "@category";
                if (Convert.ToInt32(ddlCategoryName.SelectedItem.Value) == -1)
                {
                    objDbParameter[2].Value = null;
                }
                else
                {
                    objDbParameter[2].Value = objMetaData.CategoryID;                   
                }
                objDbParameter[3] = objDbProviderFactory.CreateParameter();
                objDbParameter[3].ParameterName = "@panno";
                if (strFilterBy == "1")
                {
                    objDbParameter[3].Value = strFilterText;
                }
                else { objDbParameter[3].Value = null; }

                objDbParameter[4] = objDbProviderFactory.CreateParameter();
                objDbParameter[4].ParameterName = "@custname";
                if (strFilterBy == "2")
                {
                    objDbParameter[4].Value = strFilterText;
                }
                else { objDbParameter[4].Value = null; }
                DataTable objAppRejDtls = new DataTable();
                objAppRejDtls = DataHelper.ExecuteDataTableForProcedure("SP_S_SelectDocVerificationFilterAppRej", null, objDbParameter);

                if (objAppRejDtls.Rows.Count > 0)
                    {
                        gvwAppRejectDtls.Visible = true;
                        lblHeadingAppRejGrid.Visible = true;
                        lblHeadingAppRejGrid.Text = "Following are the documents are Approve/Rejected for Pan No/ Customer Name " +"'"+ strFilterText +"'";
                        lblHeadingAppRejGrid.ForeColor=System.Drawing.Color.Blue;
                        gvwAppRejectDtls.DataSource = objAppRejDtls;
                        gvwAppRejectDtls.DataBind();
                    }
                
            }
            catch (Exception)
            {
               
            }
        }
        private void GridFilter()
        {
            try
            {
                
                if (UserSession.GridData != null && !(ViewState["LASTPAGEURL"].ToString().Contains("viewdocumentforsearch")) && Session["DirectDocRowApprove"]==null)
                {
                    Session["FilterBy"] = null;
                    Session["FilterText"] = null;
                    string strFilterBy = ((DropDownList)gvwDocumentList.FooterRow.FindControl("ddlFilterGrid")).SelectedValue.Trim();
                    string strFilterText = ((TextBox)gvwDocumentList.FooterRow.FindControl("txtFilterGrid")).Text.Trim();
                    Session["FilterBy"] = strFilterBy;
                    Session["FilterText"] = strFilterText;
                    if (strFilterText == string.Empty)
                    {
                        gvwDocumentList.DataSource = UserSession.GridData;
                        gvwDocumentList.DataBind();
                        UserSession.FilterData = null;
                    }
                    else
                    {//
                        DataRow[] objRows = null;
                        
                        if (strFilterBy == "1")
                            objRows = UserSession.GridData.Select("Pan_No LIKE '%" + strFilterText + "%'");
                        else if (strFilterBy == "2")
                            objRows = UserSession.GridData.Select("Customer_Name LIKE '%" + strFilterText + "%'");

                        if (objRows.Length > 0)
                        {
                            UserSession.FilterData = objRows.CopyToDataTable();
                            gvwDocumentList.DataSource = UserSession.FilterData;
                            gvwDocumentList.DataBind();
                        }
                    }
                }

                if (UserSession.GridData != null && ViewState["LASTPAGEURL"].ToString().Contains("viewdocumentforsearch") && Session["DirectDocRowApprove"] == null)
                {

                    string strFilterBy = Convert.ToString(Session["FilterBy"]);
                    string strFilterText = Convert.ToString(Session["FilterText"]);
                    //Session["FilterBy"] = null;
                    //Session["FilterText"] = null;
                    if (strFilterText == string.Empty)
                    {
                        gvwDocumentList.DataSource = UserSession.GridData;
                        gvwDocumentList.DataBind();
                        UserSession.FilterData = null;
                    }
                    else
                    {
                        DataRow[] objRows = null;

                        if (strFilterBy == "1")
                            objRows = UserSession.GridData.Select("Pan_No LIKE '%" + strFilterText + "%'");
                        else if (strFilterBy == "2")
                            objRows = UserSession.GridData.Select("Customer_Name LIKE '%" + strFilterText + "%'");

                        if (objRows.Length > 0)
                        {
                            UserSession.FilterData = objRows.CopyToDataTable();
                            gvwDocumentList.DataSource = UserSession.FilterData;
                            gvwDocumentList.DataBind();
                        }
                    }
                }

                if (UserSession.GridData != null && !(ViewState["LASTPAGEURL"].ToString().Contains("viewdocumentforsearch")) && Convert.ToUInt32(Session["DirectDocRowApprove"])==1)
                {
                    Session["DirectDocRowApprove"] = null;
                    string strFilterBy = Convert.ToString(Session["FilterBy"]);
                    string strFilterText = Convert.ToString(Session["FilterText"]);

                    if (strFilterText == string.Empty)
                    {
                        gvwDocumentList.DataSource = UserSession.GridData;
                        gvwDocumentList.DataBind();
                        UserSession.FilterData = null;
                    }
                    else
                    {
                        DataRow[] objRows = null;

                        if (strFilterBy == "1")
                            objRows = UserSession.GridData.Select("Pan_No LIKE '%" + strFilterText + "%'");
                        else if (strFilterBy == "2")
                            objRows = UserSession.GridData.Select("Customer_Name LIKE '%" + strFilterText + "%'");

                        if (objRows.Length > 0)
                        {
                            UserSession.FilterData = objRows.CopyToDataTable();
                            gvwDocumentList.DataSource = UserSession.FilterData;
                            gvwDocumentList.DataBind();
                        }
                    }
                }
                
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
            }
        }

       
        protected void ibtnFilterGrid_Click(object sender, ImageClickEventArgs e)
        {
            GridFilter();
            ApproveRejectDetailGrid();
        }

        public string SelectUserName()
        {
            try
            {
                int UserId = UserSession.UserID;
                string strQuery = "select UserName from vwUser where Status=1 and id=" + UserId;
                SqlCommand com = new SqlCommand(strQuery, con);
                SqlDataAdapter da = new SqlDataAdapter(com);
                DataTable dtUserName = new DataTable();
                da.Fill(dtUserName);
                string username = dtUserName.Rows[0][0].ToString();
                return username;
            }
            catch (Exception ex)
            {
                return null;
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
            }
        }
    }
}