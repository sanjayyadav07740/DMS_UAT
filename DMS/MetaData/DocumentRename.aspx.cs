using DMS.BusinessLogic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DMS.Shared
{
    public partial class DocumentRename : System.Web.UI.Page
    {
        #region Private Memeber
        Utility objUtility = new Utility();
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (UserSession.GridData != null)
                {
                    grvDocumentDetails.Visible = true;
                    grvDocumentDetails.DataSource = UserSession.GridData;
                }
                else
                    grvDocumentDetails.Visible = false;
                LoadGridDataFromTempararyList();
                Log.AuditLog(HttpContext.Current, "Visit", "DocumentRename");
            }

        }

        protected void ibtnSubmit_Click(object sender, ImageClickEventArgs e)
        {
            Hashtable objTempList = new Hashtable();


            if (ddlCriteria.SelectedItem.Text == "--Select--")
            {
                UserSession.DisplayMessage(this, "Please select a criteria", MainMasterPage.MessageType.Warning);
                return;
                //LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
            if (txtDocName.Text == "")
            {
                UserSession.DisplayMessage(this, "Please enter text to search", MainMasterPage.MessageType.Warning);
                return;
            }

            DMS.BusinessLogic.DocumentManager objDocManager = new DocumentManager();
            DataTable dt = new DataTable();
            if (ddlCriteria.SelectedItem.Text == "Like")
            {
                objUtility.Result = objDocManager.SearchInAll(out dt, txtDocName.Text, Convert.ToInt32(UserSession.UserID), 1);
            }
            else if (ddlCriteria.SelectedItem.Text == "Equal")
            {
                objUtility.Result = objDocManager.SearchInAll(out dt, txtDocName.Text, Convert.ToInt32(UserSession.UserID), 2);
            }
            else if (ddlCriteria.SelectedItem.Text == "Not")
            {
                objUtility.Result = objDocManager.SearchInAll(out dt, txtDocName.Text, Convert.ToInt32(UserSession.UserID), 3);
            }
            switch (objUtility.Result)
            {
                case Utility.ResultType.Success:
                    if (dt.Rows.Count > 0)
                    {
                        ViewState["dt"] = dt;
                        grvDocumentDetails.Visible = true;
                        grvDocumentDetails.DataSource = dt;
                        grvDocumentDetails.DataBind();
                        UserSession.FilterData = null;
                        UserSession.GridData = dt;

                        objTempList.Add("SEARCHCRITERIA", ddlCriteria.SelectedValue);
                        objTempList.Add("SEARCHTEXT", txtDocName.Text);
                        objTempList.Add("Page", "documentrename");
                        UserSession.TemporaryList = objTempList;
                    }
                    else
                    {
                        grvDocumentDetails.Visible = false;
                        UserSession.DisplayMessage(this, "No Data To Display for selected criteria", MainMasterPage.MessageType.Success);
                    }
                    break;

                case Utility.ResultType.Failure:
                    grvDocumentDetails.Visible = false;
                    grvDocumentDetails.DataSource = dt;
                    grvDocumentDetails.DataBind();
                    UserSession.FilterData = null;
                    UserSession.DisplayMessage(this, "No Data To Display .", MainMasterPage.MessageType.Warning);
                    break;

                case Utility.ResultType.Error:
                    grvDocumentDetails.Visible = false;
                    grvDocumentDetails.DataSource = dt;
                    grvDocumentDetails.DataBind();
                    UserSession.FilterData = null;
                    UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                    break;
            }
        }

        protected void grvDocumentDetails_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            try
            {
                if (e.CommandName.ToLower().Trim() == "documentsearch")
                {
                    int intRowIndex = Convert.ToInt32(e.CommandArgument);
                    UserSession.MetaDataID = Convert.ToInt32(grvDocumentDetails.DataKeys[intRowIndex].Values["MetaDataID"]);
                    string strDocumentID = grvDocumentDetails.DataKeys[intRowIndex].Values["ID"].ToString().Trim();
                    // string strStatus = grvDocumentDetails.DataKeys[intRowIndex].Values["DocumentStatusID"].ToString().Trim();

                    //switch (strStatus)
                    //{
                    //    case "1":
                    //        Response.Redirect("../MetaData/ApprovedDocument.aspx?DOCID=" + strDocumentID, false);
                    //        break;

                    //    case "2":
                    //        Response.Redirect("../MetaData/RejectedDocument.aspx?DOCID=" + strDocumentID, false);
                    //        break;

                    //    case "3":
                    //        Response.Redirect("../MetaData/DocumentEntry.aspx?DOCID=" + strDocumentID, false);
                    //        break;

                    //    case "4":
                    //Response.Redirect("../MetaData/DocumentVerification.aspx?DOCID=" + strDocumentID, false);
                    //break;
                    //}
                    Log.DocumentAuditLog(HttpContext.Current, "View", "DocumentRename", Convert.ToInt32(strDocumentID));
                    Response.Redirect("../MetaData/DocumentViewer.aspx?DOCID=" + strDocumentID, false);

                }
                if (e.CommandName.ToLower().Trim() == "documentrename")
                {
                    GridViewRow gvr = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    int RowIndex = gvr.RowIndex;
                    //int intRowIndex = Convert.ToInt32(e.CommandArgument);
                    //UserSession.MetaDataID = Convert.ToInt32(grvDocumentDetails.DataKeys[intRowIndex].Values["MetaDataID"]);
                    //string strDocumentID = grvDocumentDetails.DataKeys[intRowIndex].Values["ID"].ToString().Trim();
                    //GridViewRow row = grvDocumentDetails.Rows[Convert.ToInt32(e.CommandArgument)];
                    //string strDocumentname = row.Cells[2].Text;

                    grvDocumentDetails.EditIndex = RowIndex;
                    grvDocumentDetails.DataSource = UserSession.GridData;
                    grvDocumentDetails.DataBind();
                }
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        protected void OnRowEditing(object sender, GridViewEditEventArgs e)
        {
            grvDocumentDetails.EditIndex = e.NewEditIndex;
            //this.BindGrid();
        }

        protected void OnUpdate(object sender, EventArgs e)
        {
            DMS.BusinessLogic.DocumentManager objDocManager = new DocumentManager();
            GridViewRow row = (sender as ImageButton).NamingContainer as GridViewRow;
            //string documentname = (row.Cells[2].Controls[0] as TextBox).Text;
            TextBox tx1 = (TextBox)grvDocumentDetails.Rows[row.RowIndex].FindControl("txt_DocumentName");
            if (tx1.Text.Contains("."))
            {
                UserSession.DisplayMessage(this, "Document named should not contain extention!", MainMasterPage.MessageType.Warning);
                return;
            }
            string documentname = tx1.Text;
            int documentid = Convert.ToInt32(grvDocumentDetails.DataKeys[row.RowIndex].Values["ID"]);
            int DeptID = Convert.ToInt32(grvDocumentDetails.DataKeys[row.RowIndex].Values["DeptID"]);
            int MetatemplateID = Convert.ToInt32(grvDocumentDetails.DataKeys[row.RowIndex].Values["MetatemplateId"]);
            string DocType = Convert.ToString(grvDocumentDetails.DataKeys[row.RowIndex].Values["DocumentType"]);
            string OlDDocumentName = Convert.ToString(grvDocumentDetails.DataKeys[row.RowIndex].Values["DocumentName"]);
            string NewDocumentName = documentname + DocType;

            DataSet dtset = Document.CheckDocument(NewDocumentName, DeptID, MetatemplateID);
            if (dtset.Tables[0].Rows.Count > 0)
            {
                UserSession.DisplayMessage(this, "Document Already Exists! Please Update With Another Name.", MainMasterPage.MessageType.Warning);
                return;
            }


            objUtility.Result = objDocManager.DocumentRename(documentname, documentid);

            grvDocumentDetails.EditIndex = -1;
            grvDocumentDetails.DataBind();
            bindgrid();
            Log.DocumentRenameLog(documentid, OlDDocumentName, NewDocumentName, HttpContext.Current);
            Log.DocumentAuditLog(HttpContext.Current, "Document Rename Successfully", "DocumentRename", documentid);

            UserSession.DisplayMessage(this, "Document Renamed Successfully .", MainMasterPage.MessageType.Success);
        }

        protected void OnCancel(object sender, EventArgs e)
        {
            grvDocumentDetails.EditIndex = -1;
            grvDocumentDetails.DataBind();
            bindgrid();
        }

        private void bindgrid()
        {
            DMS.BusinessLogic.DocumentManager objDocManager = new DocumentManager();
            DataTable dt = new DataTable();
            if (ddlCriteria.SelectedItem.Text == "Like")
            {
                objUtility.Result = objDocManager.SearchInAll(out dt, txtDocName.Text, Convert.ToInt32(UserSession.UserID), 1);
            }
            else if (ddlCriteria.SelectedItem.Text == "Equal")
            {
                objUtility.Result = objDocManager.SearchInAll(out dt, txtDocName.Text, Convert.ToInt32(UserSession.UserID), 2);
            }
            else if (ddlCriteria.SelectedItem.Text == "Not")
            {
                objUtility.Result = objDocManager.SearchInAll(out dt, txtDocName.Text, Convert.ToInt32(UserSession.UserID), 3);
            }
            switch (objUtility.Result)
            {
                case Utility.ResultType.Success:
                    if (dt.Rows.Count > 0)
                    {
                        ViewState["dt"] = dt;
                        grvDocumentDetails.Visible = true;
                        grvDocumentDetails.DataSource = dt;
                        grvDocumentDetails.DataBind();
                        UserSession.FilterData = null;
                        UserSession.GridData = dt;
                    }
                    else
                    {
                        grvDocumentDetails.Visible = false;
                        UserSession.DisplayMessage(this, "No Data To Display for selected criteria", MainMasterPage.MessageType.Success);
                    }
                    break;

                case Utility.ResultType.Failure:
                    grvDocumentDetails.Visible = false;
                    grvDocumentDetails.DataSource = dt;
                    grvDocumentDetails.DataBind();
                    UserSession.FilterData = null;
                    UserSession.DisplayMessage(this, "No Data To Display .", MainMasterPage.MessageType.Warning);
                    break;

                case Utility.ResultType.Error:
                    grvDocumentDetails.Visible = false;
                    grvDocumentDetails.DataSource = dt;
                    grvDocumentDetails.DataBind();
                    UserSession.FilterData = null;
                    UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                    break;
            }
        }

        private void LoadGridDataFromTempararyList()
        {
            try
            {
                if (UserSession.TemporaryList != null && UserSession.TemporaryList["Page"].ToString() == "documentrename")
                {
                    if (UserSession.TemporaryList["SEARCHTEXT"] != null)
                    {
                        txtDocName.Text = UserSession.TemporaryList["SEARCHTEXT"].ToString();
                    }
                    if (UserSession.TemporaryList["SEARCHCRITERIA"] != null)
                    {
                        ddlCriteria.SelectedValue = UserSession.TemporaryList["SEARCHCRITERIA"].ToString();
                    }

                    DMS.BusinessLogic.DocumentManager objDocManager = new DocumentManager();
                    DataTable dt = new DataTable();
                    if (ddlCriteria.SelectedItem.Text == "Like")
                    {
                        objUtility.Result = objDocManager.SearchInAll(out dt, txtDocName.Text, Convert.ToInt32(UserSession.UserID), 1);
                    }
                    else if (ddlCriteria.SelectedItem.Text == "Equal")
                    {
                        objUtility.Result = objDocManager.SearchInAll(out dt, txtDocName.Text, Convert.ToInt32(UserSession.UserID), 2);
                    }
                    else if (ddlCriteria.SelectedItem.Text == "Not")
                    {
                        objUtility.Result = objDocManager.SearchInAll(out dt, txtDocName.Text, Convert.ToInt32(UserSession.UserID), 3);
                    }
                    switch (objUtility.Result)
                    {
                        case Utility.ResultType.Success:
                            if (dt.Rows.Count > 0)
                            {
                                ViewState["dt"] = dt;
                                grvDocumentDetails.Visible = true;
                                grvDocumentDetails.DataSource = dt;
                                grvDocumentDetails.DataBind();
                                UserSession.FilterData = null;
                                UserSession.GridData = dt;
                            }
                            else
                            {
                                grvDocumentDetails.Visible = false;
                                UserSession.DisplayMessage(this, "No Data To Display for selected criteria", MainMasterPage.MessageType.Success);
                            }
                            break;

                        case Utility.ResultType.Failure:
                            grvDocumentDetails.Visible = false;
                            grvDocumentDetails.DataSource = dt;
                            grvDocumentDetails.DataBind();
                            UserSession.FilterData = null;
                            UserSession.DisplayMessage(this, "No Data To Display .", MainMasterPage.MessageType.Warning);
                            break;

                        case Utility.ResultType.Error:
                            grvDocumentDetails.Visible = false;
                            grvDocumentDetails.DataSource = dt;
                            grvDocumentDetails.DataBind();
                            UserSession.FilterData = null;
                            UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }
    }
}