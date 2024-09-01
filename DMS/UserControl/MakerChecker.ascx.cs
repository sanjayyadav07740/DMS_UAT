using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DMS.BusinessLogic;
using System.Web.UI.HtmlControls;
using System.Data;
using System.Data.Common;
using System.IO;
using DMS.BusinessLogic.Entity;

namespace DMS.UserControl
{
    public partial class MakerChecker : System.Web.UI.UserControl
    {
        #region Private Member
        BusinessLogic.Utility objUtility = new BusinessLogic.Utility();
        BusinessLogic.DocumentManager objDocumentManager = new BusinessLogic.DocumentManager();
        BusinessLogic.MetaTemplateManager objMetaTemplateManager = new BusinessLogic.MetaTemplateManager();
        #endregion

        #region Enum
        public enum ViewMakerCheckerFor { DocumentEntry, DocumentVerification, ApprovedDocument, RejectedDocument,SearchDocument };
        #endregion

        #region Properties
        public ViewMakerCheckerFor ViewType
        {
            get;
            set;
        }
       
        public GridView DocumentGrid
        {
            get
            {
                return this.gvwDocument;
            }
        }

        private DataTable Version
        {
            get
            {
                return DocumentDetail.Tables[6]; //Version
            }
        }
        private DataSet DocumentDetail
        {
            get
            {
                return (DataSet)Session["DocumentDetail"];
            }
            set
            {
                Session["DocumentDetail"] = value;
            }
        }

        private DataTable DocumentList
        {
            get
            {
                return DocumentDetail.Tables[0]; //Document
            }
        }
        private DataTable ScreenType
        {
            get
            {
                return DocumentDetail.Tables[4]; //ScreenType
            }
        }

        #endregion

        #region Event
        protected override void OnInit(EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    UserSession.FileByte = null;
                    UserSession.Field = null;
                    UserSession.Document = null;
                    UserSession.List = null;

                    DataTable objDataTableMetaData = new DataTable();
                    if (UserSession.MetaDataID != 0)
                    {
                        objUtility.Result = objDocumentManager.SelectMetaData(out objDataTableMetaData, UserSession.MetaDataID);
                        switch (objUtility.Result)
                        {
                            case BusinessLogic.Utility.ResultType.Failure:
                                UserSession.DisplayMessage(this.Parent.Page, "No MetaData To Display .", MainMasterPage.MessageType.Warning);
                                return;
                                break;

                            case BusinessLogic.Utility.ResultType.Error:
                                UserSession.DisplayMessage(this.Parent.Page, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                                return;
                                break;
                        }
                    }

                    DataTable objDataTableField = new DataTable();

                    objUtility.Result = objDocumentManager.SelectField(out objDataTableField, Convert.ToInt32(objDataTableMetaData.Rows[0]["MetaTemplateID"]));

                    switch (objUtility.Result)
                    {
                        case BusinessLogic.Utility.ResultType.Failure:
                            UserSession.DisplayMessage(this.Parent.Page, "No Fields To Display .", MainMasterPage.MessageType.Warning);
                            return;
                            break;

                        case BusinessLogic.Utility.ResultType.Error:
                            UserSession.DisplayMessage(this.Parent.Page, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                            return;
                            break;
                    }


                    DataTable objDataTableDocument = new DataTable();
                    objDataTableDocument.Columns.Add("DocumentID");
                    objDataTableDocument.Columns.Add("FilePath");
                    objDataTableDocument.Columns.Add("FileByte", typeof(byte[]));
                    objDataTableDocument.Columns.Add("VerifyStatus");
                    objDataTableDocument.Columns.Add("IsCheck");
                    foreach (DataRow objDataRow in objDataTableField.Select("STATUS = 1"))
                    {
                        objDataTableDocument.Columns.Add(objDataRow["ID"].ToString());
                    }
                    objDataTableDocument.AcceptChanges();

                    string strDocumentID = string.Empty;
                    if (Request["DOCID"] != null)
                    {
                        strDocumentID = Request["DOCID"].ToString().Trim();
                    }

                    DataTable objDataTableMetaDataDocument = new DataTable();
                    if(ViewType == ViewMakerCheckerFor.DocumentEntry)
                        objUtility.Result = objDocumentManager.SelectDocument(out objDataTableMetaDataDocument, UserSession.MetaDataID, DocumentManager.Status.Uploaded, strDocumentID);
                    else if(ViewType == ViewMakerCheckerFor.DocumentVerification)
                        objUtility.Result = objDocumentManager.SelectDocument(out objDataTableMetaDataDocument, UserSession.MetaDataID, DocumentManager.Status.EntryCompleted, strDocumentID);
                    else if (ViewType == ViewMakerCheckerFor.ApprovedDocument)
                        objUtility.Result = objDocumentManager.SelectDocument(out objDataTableMetaDataDocument, UserSession.MetaDataID, DocumentManager.Status.Approved, strDocumentID);
                    else if (ViewType == ViewMakerCheckerFor.RejectedDocument)
                        objUtility.Result = objDocumentManager.SelectDocument(out objDataTableMetaDataDocument, UserSession.MetaDataID, DocumentManager.Status.Rejected, strDocumentID);
                   


                    switch (objUtility.Result)
                    {
                        case BusinessLogic.Utility.ResultType.Failure:
                            UserSession.DisplayMessage(this.Parent.Page, "No Document To Display .", MainMasterPage.MessageType.Warning);
                            return;
                            break;

                        case BusinessLogic.Utility.ResultType.Error:
                            UserSession.DisplayMessage(this.Parent.Page, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                            return;
                            break;
                    }

                   
                    if (ViewType == ViewMakerCheckerFor.DocumentEntry)
                    {
                        BusinessLogic.Utility.FillFileByteInDocumentDataTable(objDataTableMetaDataDocument, objDataTableDocument);
                    }
                    else
                    {
                        DataTable objDataTableDocumentData = new DataTable();
                        objUtility.Result = objDocumentManager.SelectDocumentData(out objDataTableDocumentData, UserSession.MetaDataID);
                        switch (objUtility.Result)
                        {
                            //case BusinessLogic.Utility.ResultType.Failure:
                            //    UserSession.DisplayMessage(this.Parent.Page, "No Document Data To Display .", MainMasterPage.MessageType.Warning);
                            //    return;
                            //    break;

                            case BusinessLogic.Utility.ResultType.Error:
                                UserSession.DisplayMessage(this.Parent.Page, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                                return;
                                break;
                        }

                        BusinessLogic.Utility.FillFileByteAndDataInDocumentDataTable(objDataTableMetaDataDocument, objDataTableDocument, objDataTableDocumentData, objDataTableField);
                    }

                    DataTable objDataTableList = new DataTable();
                    objUtility.Result = objMetaTemplateManager.SelectListItem(out objDataTableList, Convert.ToInt32(objDataTableMetaData.Rows[0]["MetaTemplateID"]));
                    switch (objUtility.Result)
                    {
                        case BusinessLogic.Utility.ResultType.Error:
                            UserSession.DisplayMessage(this.Parent.Page, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                            return;
                            break;
                    }

                    objDataTableList.AcceptChanges();

                    UserSession.Field = objDataTableField.Select("STATUS = 1").CopyToDataTable();
                    UserSession.Document = objDataTableDocument;
                    UserSession.List = objDataTableList;
                    Session["DocumentName"] = objDataTableMetaDataDocument.Rows[0]["DocumentName"].ToString();
                }
                LoadGridData();
                base.OnInit(e);
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this.Parent.Page, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(BusinessLogic.Utility.LogFilePath, ex);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //((AjaxControlToolkit.ToolkitScriptManager)this.Parent.Page.Master.FindControl("tsmManager")).RegisterPostBackControl(ibtnVersion);
            //if (!IsPostBack)
            //{

            //    DataTable dt4 = (DataTable)Session["sampletable"];
            //    gvwDocumentVersion.DataSource = dt4;
            //    gvwDocumentVersion.DataBind();

            //    DataTable dt2 = (DataTable)Session["sampletable"];
            //    gvwDocumentVersion.DataSource = dt2;
            //    gvwDocumentVersion.DataBind();

            //    DataTable dt1 = (DataTable)Session["sampletable"];
            //    gvwDocumentVersion.DataSource = dt1;
            //    gvwDocumentVersion.DataBind();

            //    DataTable dt3 = (DataTable)Session["sampletable"];
            //    gvwDocumentVersion.DataSource = dt3;
            //    gvwDocumentVersion.DataBind();

            //    this.Parent.Page.Master.FindControl("tblMainContent").Visible = false;
            //    ((HtmlTable)this.Parent.Page.Master.FindControl("tblMessage")).Style[HtmlTextWriterStyle.MarginLeft] = "400px";
            //}
            if (!IsPostBack)
            {
                this.Parent.Page.Master.FindControl("tblMainContent").Visible = false;
                ((HtmlTable)this.Parent.Page.Master.FindControl("tblMessage")).Style[HtmlTextWriterStyle.MarginLeft] = "400px";
            }
        }

        private void LoadGridData()
        {
            try
            {
                if (UserSession.Document != null)
                {
                    gvwDocument.DataSource = UserSession.Document;
                    gvwDocument.DataBind();
                }
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this.Parent.Page, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(BusinessLogic.Utility.LogFilePath, ex);
            }
        }

        protected void gvwDocument_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                Label objLabel;

                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    byte[] byteFileByte = null;
                    if(ViewType == ViewMakerCheckerFor.DocumentEntry || ViewType == ViewMakerCheckerFor.DocumentVerification)
                        BusinessLogic.Utility.GenerateControlAndFillData(e, gvwDocument, hdfField, ref byteFileByte, UserSession.Document, UserSession.Field, UserSession.List, BusinessLogic.Utility.EnableOrDisable.Enable);
                    else
                        BusinessLogic.Utility.GenerateControlAndFillData(e, gvwDocument, hdfField, ref byteFileByte, UserSession.Document, UserSession.Field, UserSession.List, BusinessLogic.Utility.EnableOrDisable.Disable);
                    UserSession.FileByte = byteFileByte;
                }

                if (ViewType == ViewMakerCheckerFor.DocumentEntry)
                {
                    #region DocumentEntry
                    if (e.Row.RowType == DataControlRowType.Header)
                    {
                        objLabel = (Label)e.Row.FindControl("lblTotalRecord");
                        objLabel.Text = string.Format("{0} / {1}", gvwDocument.PageIndex + 1, gvwDocument.PageCount);

                        objLabel = (Label)e.Row.FindControl("lblCheckedRecord");
                        objLabel.Text = string.Format("{0} / {1}", UserSession.Document.Select("IsCheck='1'").Length, UserSession.Document.Rows.Count);

                        objLabel = (Label)e.Row.FindControl("lblPendingRecord");
                        objLabel.Text = string.Format("{0} / {1}", UserSession.Document.Select("IsCheck='0'").Length, UserSession.Document.Rows.Count);
                    }
                    if (e.Row.RowType == DataControlRowType.Pager)
                    {
                        if (e.Row.RowType == DataControlRowType.Pager)
                        {
                            TableRow objTableRow = e.Row.Controls[0].Controls[0].Controls[0] as TableRow;

                            foreach (TableCell objTableCell in objTableRow.Cells)
                            {

                                Control objControl = objTableCell.Controls[0];
                                if (objControl is Label)
                                {
                                    ((Label)objControl).BorderStyle = BorderStyle.Solid;
                                    ((Label)objControl).BackColor = System.Drawing.Color.Yellow;
                                }
                                else if (objControl is LinkButton)
                                {
                                    if (((LinkButton)objControl).Text.Trim() != "...")
                                    {
                                        int rowIndex = Convert.ToInt32(((LinkButton)objControl).Text) - 1;
                                        string strValue = UserSession.Document.Rows[rowIndex]["IsCheck"].ToString().Trim();

                                        if (strValue == "0")
                                        {
                                            ((LinkButton)objControl).BorderStyle = BorderStyle.Solid;
                                            ((LinkButton)objControl).BackColor = System.Drawing.Color.Red;
                                            ((LinkButton)objControl).ForeColor = System.Drawing.Color.Yellow;
                                        }
                                        else if (strValue == "1")
                                        {
                                            ((LinkButton)objControl).BorderStyle = BorderStyle.Solid;
                                            ((LinkButton)objControl).BackColor = System.Drawing.Color.Green;
                                            ((LinkButton)objControl).ForeColor = System.Drawing.Color.Yellow;
                                        }
                                    }
                                }

                            }

                        }
                    }
                    #endregion
                }
                else if (ViewType == ViewMakerCheckerFor.DocumentVerification)
                {
                    #region DocumentVerification
                    if (e.Row.RowType == DataControlRowType.Header)
                    {
                        objLabel = (Label)e.Row.FindControl("lblTotalRecord");
                        objLabel.Text = string.Format("{0} / {1}", gvwDocument.PageIndex + 1, gvwDocument.PageCount);

                        objLabel = (Label)e.Row.FindControl("lblCheckedRecord");
                        objLabel.Text = string.Format("{0} / {1}", UserSession.Document.Select("IsCheck='1'").Length, UserSession.Document.Rows.Count);

                        objLabel = (Label)e.Row.FindControl("lblPendingRecord");
                        objLabel.Text = string.Format("{0} / {1}", UserSession.Document.Select("VerifyStatus='0'").Length, UserSession.Document.Rows.Count);

                        objLabel = (Label)e.Row.FindControl("lblApprovedRecord");
                        objLabel.Text = string.Format("{0} / {1}", UserSession.Document.Select("VerifyStatus='1'").Length, UserSession.Document.Rows.Count);

                        objLabel = (Label)e.Row.FindControl("lblRejectedRecord");
                        objLabel.Text = string.Format("{0} / {1}", UserSession.Document.Select("VerifyStatus='2'").Length, UserSession.Document.Rows.Count);
                    }
                    if (e.Row.RowType == DataControlRowType.Pager)
                    {
                        if (e.Row.RowType == DataControlRowType.Pager)
                        {
                            TableRow objTableRow = e.Row.Controls[0].Controls[0].Controls[0] as TableRow;

                            foreach (TableCell objTableCell in objTableRow.Cells)
                            {

                                Control objControl = objTableCell.Controls[0];
                                if (objControl is Label)
                                {
                                    ((Label)objControl).BorderStyle = BorderStyle.Solid;
                                    ((Label)objControl).BackColor = System.Drawing.Color.Yellow;
                                }
                                else if (objControl is LinkButton)
                                {
                                    if (((LinkButton)objControl).Text.Trim() != "...")
                                    {
                                        int rowIndex = Convert.ToInt32(((LinkButton)objControl).Text) - 1;
                                        string strValue = UserSession.Document.Rows[rowIndex]["IsCheck"].ToString().Trim();

                                        if (strValue == "1")
                                        {
                                            ((LinkButton)objControl).BorderStyle = BorderStyle.Solid;
                                            ((LinkButton)objControl).BackColor = System.Drawing.Color.Green;
                                            ((LinkButton)objControl).ForeColor = System.Drawing.Color.Yellow;
                                        }

                                        string strVerifyValue = UserSession.Document.Rows[rowIndex]["VerifyStatus"].ToString().Trim();

                                        if (strVerifyValue == "0" && strValue == "0")
                                        {
                                            ((LinkButton)objControl).BorderStyle = BorderStyle.Solid;
                                            ((LinkButton)objControl).BackColor = System.Drawing.Color.Red;
                                            ((LinkButton)objControl).ForeColor = System.Drawing.Color.Yellow;
                                        }
                                        else if (strVerifyValue == "1")
                                        {
                                            ((LinkButton)objControl).BorderStyle = BorderStyle.Solid;
                                            ((LinkButton)objControl).BackColor = System.Drawing.Color.Orange;
                                            ((LinkButton)objControl).ForeColor = System.Drawing.Color.Black;
                                        }
                                        else if (strVerifyValue == "2")
                                        {
                                            ((LinkButton)objControl).BorderStyle = BorderStyle.Solid;
                                            ((LinkButton)objControl).BackColor = System.Drawing.Color.DarkMagenta;
                                            ((LinkButton)objControl).ForeColor = System.Drawing.Color.Black;
                                        }
                                    }
                                }

                            }

                        }

                    }
                    #endregion
                }
                else if (ViewType == ViewMakerCheckerFor.ApprovedDocument || ViewType == ViewMakerCheckerFor.SearchDocument)
                {
                    #region ApprovedDocument
                    if (e.Row.RowType == DataControlRowType.Header)
                    {
                        objLabel = (Label)e.Row.FindControl("lblTotalRecord");
                        objLabel.Text = string.Format("{0} / {1}", gvwDocument.PageIndex + 1, gvwDocument.PageCount);
                    }
                    if (e.Row.RowType == DataControlRowType.Pager)
                    {
                        if (e.Row.RowType == DataControlRowType.Pager)
                        {
                            TableRow objTableRow = e.Row.Controls[0].Controls[0].Controls[0] as TableRow;

                            foreach (TableCell objTableCell in objTableRow.Cells)
                            {

                                Control objControl = objTableCell.Controls[0];
                                if (objControl is Label)
                                {
                                    ((Label)objControl).BorderStyle = BorderStyle.Solid;
                                    ((Label)objControl).BackColor = System.Drawing.Color.Yellow;
                                }
                                else if (objControl is LinkButton)
                                {
                                    if (((LinkButton)objControl).Text.Trim() != "...")
                                    {
                                        ((LinkButton)objControl).BorderStyle = BorderStyle.Solid;
                                        ((LinkButton)objControl).BackColor = System.Drawing.Color.Green;
                                        ((LinkButton)objControl).ForeColor = System.Drawing.Color.Yellow;
                                    }
                                }

                            }

                        }

                    }
                    #endregion
                }
                else if (ViewType == ViewMakerCheckerFor.RejectedDocument)
                {
                    #region RejectedDocument
                    if (e.Row.RowType == DataControlRowType.Header)
                    {
                        objLabel = (Label)e.Row.FindControl("lblTotalRecord");
                        objLabel.Text = string.Format("{0} / {1}", gvwDocument.PageIndex + 1, gvwDocument.PageCount);
                    }
                    if (e.Row.RowType == DataControlRowType.Pager)
                    {
                        if (e.Row.RowType == DataControlRowType.Pager)
                        {
                            TableRow objTableRow = e.Row.Controls[0].Controls[0].Controls[0] as TableRow;

                            foreach (TableCell objTableCell in objTableRow.Cells)
                            {

                                Control objControl = objTableCell.Controls[0];
                                if (objControl is Label)
                                {
                                    ((Label)objControl).BorderStyle = BorderStyle.Solid;
                                    ((Label)objControl).BackColor = System.Drawing.Color.Yellow;
                                }
                                else if (objControl is LinkButton)
                                {
                                    if (((LinkButton)objControl).Text.Trim() != "...")
                                    {
                                        ((LinkButton)objControl).BorderStyle = BorderStyle.Solid;
                                        ((LinkButton)objControl).BackColor = System.Drawing.Color.Red;
                                        ((LinkButton)objControl).ForeColor = System.Drawing.Color.Yellow;
                                    }
                                }

                            }

                        }

                    }
                    #endregion
                }

                if (ViewType == ViewMakerCheckerFor.DocumentEntry)
                {
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        ((ImageButton)e.Row.FindControl("pnlHolderMain").FindControl("pnlHolder").FindControl("btnApprove")).Visible = false;
                        ((ImageButton)e.Row.FindControl("pnlHolderMain").FindControl("pnlHolder").FindControl("btnReject")).Visible = false;
                        ((ImageButton)e.Row.FindControl("pnlHolderMain").FindControl("pnlHolder").FindControl("btnSave")).Visible = true;
                    }
                    if (e.Row.RowType == DataControlRowType.Header)
                    {
                        ((Label)e.Row.FindControl("lblApprovedRecordLabel")).Visible = false;
                        ((Label)e.Row.FindControl("lblRejectedRecordLabel")).Visible = false;
                    }
                }
                else if (ViewType == ViewMakerCheckerFor.DocumentVerification)
                {
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        ((ImageButton)e.Row.FindControl("pnlHolderMain").FindControl("pnlHolder").FindControl("btnApprove")).Visible = true;
                        ((ImageButton)e.Row.FindControl("pnlHolderMain").FindControl("pnlHolder").FindControl("btnReject")).Visible = true;
                        ((ImageButton)e.Row.FindControl("pnlHolderMain").FindControl("pnlHolder").FindControl("btnSave")).Visible = true;
                    }
                    if (e.Row.RowType == DataControlRowType.Header)
                    {
                        ((Label)e.Row.FindControl("lblApprovedRecordLabel")).Visible = true;
                        ((Label)e.Row.FindControl("lblRejectedRecordLabel")).Visible = true;
                    }
                }
                else if (ViewType == ViewMakerCheckerFor.ApprovedDocument || ViewType == ViewMakerCheckerFor.RejectedDocument || ViewType == ViewMakerCheckerFor.SearchDocument)
                {
                     if (e.Row.RowType == DataControlRowType.DataRow)
                     {
                        ((ImageButton)e.Row.FindControl("pnlHolderMain").FindControl("pnlHolder").FindControl("btnApprove")).Visible = false;
                        ((ImageButton)e.Row.FindControl("pnlHolderMain").FindControl("pnlHolder").FindControl("btnReject")).Visible = false;
                        ((ImageButton)e.Row.FindControl("pnlHolderMain").FindControl("pnlHolder").FindControl("btnSave")).Visible = false;
                     }
                    if (e.Row.RowType == DataControlRowType.Header)
                    {
                        ((Label)e.Row.FindControl("lblCheckedRecordLabel")).Visible = false;
                        ((Label)e.Row.FindControl("lblPendingRecordLabel")).Visible = false;
                        ((Label)e.Row.FindControl("lblApprovedRecordLabel")).Visible = false;
                        ((Label)e.Row.FindControl("lblRejectedRecordLabel")).Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this.Parent.Page, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(BusinessLogic.Utility.LogFilePath, ex);
            }
        }

        protected void gvwDocument_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                if (UserSession.Document != null)
                {
                    gvwDocument.PageIndex = e.NewPageIndex;
                    gvwDocument.DataSource = UserSession.Document;
                    gvwDocument.DataBind();
                }
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this.Parent.Page, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(BusinessLogic.Utility.LogFilePath, ex);
            }
        }

        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                int intDataKeyValue = Convert.ToInt32(((ImageButton)sender).CommandArgument);

                DataRow[] objDataRowDocument = UserSession.Document.Select("DocumentID='" + intDataKeyValue.ToString() + "'");

                string strErrorMessage = string.Empty;

                foreach (DataRow objDataRow in UserSession.Field.Rows)
                {
                    string FieldValue = string.Empty;
                    if (objDataRow["FieldDataTypeID"].ToString().Trim() != "4" && objDataRow["FieldDataTypeID"].ToString().Trim() != "9")
                    {
                        FieldValue = ((TextBox)gvwDocument.Rows[0].FindControl("pnlHolderMain").FindControl("pnlHolder").FindControl(objDataRow["ID"].ToString())).Text;
                    }
                    else if (objDataRow["FieldDataTypeID"].ToString().Trim() == "4")
                    {
                        FieldValue = ((DropDownList)gvwDocument.Rows[0].FindControl("pnlHolderMain").FindControl("pnlHolder").FindControl(objDataRow["ID"].ToString())).SelectedItem.Value;
                    }
                    else if (objDataRow["FieldDataTypeID"].ToString().Trim() == "9")
                    {
                        CheckBoxList objCheckBoxList = ((CheckBoxList)gvwDocument.Rows[0].FindControl("pnlHolderMain").FindControl("pnlHolder").FindControl(objDataRow["ID"].ToString()));
                        foreach (ListItem objListItem in objCheckBoxList.Items)
                        {
                            if (objListItem.Selected)
                                FieldValue = FieldValue + objListItem.Value + ",";
                        }
                        if (FieldValue != string.Empty)
                            FieldValue = FieldValue.Remove(FieldValue.LastIndexOf(','));

                        string[] strArray = FieldValue.Split(',');
                        if (strArray.Length > 0)
                        {
                            FieldValue = string.Empty;
                            Array.Sort(strArray);
                        }

                        foreach (string strItem in strArray)
                        {
                            FieldValue = FieldValue + strItem + ",";
                        }
                        if (FieldValue != string.Empty)
                            FieldValue = FieldValue.Remove(FieldValue.LastIndexOf(','));

                        FieldValue = FieldValue.Trim(',');
                    }
                   
                    if (objDataRow["IsPrimary"].ToString().Trim() == "1")
                    {
                        if (UserSession.Document.Select("DocumentID <>'" + intDataKeyValue.ToString() + "' AND [" + objDataRow["ID"].ToString().Trim() + "]='" + FieldValue.Trim() + "'").Length > 0)
                        {
                            strErrorMessage = strErrorMessage + "Value Of " + objDataRow["FieldName"].ToString() + " Is Already Exists .";
                        }
                    }

                    objDataRowDocument[0][objDataRow["ID"].ToString()] = FieldValue.ToString();
                }
                if (strErrorMessage.Trim() == string.Empty)
                {
                    objDataRowDocument[0]["IsCheck"] = 1;
                }
                else
                {
                    objDataRowDocument[0]["IsCheck"] = 0;
                    UserSession.DisplayMessage(this.Parent.Page, strErrorMessage, MainMasterPage.MessageType.Warning);
                }

                UserSession.Document.AcceptChanges();

                if (ViewType == ViewMakerCheckerFor.DocumentVerification)
                {
                    gvwDocument.DataSource = UserSession.Document;
                    gvwDocument.DataBind();
                }
                else
                {
                    if (strErrorMessage.Trim() == string.Empty)
                    {
                        if (gvwDocument.PageIndex < gvwDocument.PageCount)
                        {
                            gvwDocument.PageIndex = gvwDocument.PageIndex + 1;
                            gvwDocument.DataSource = UserSession.Document;
                            gvwDocument.DataBind();
                        }
                    }
                    else
                    {
                        gvwDocument.DataSource = UserSession.Document;
                        gvwDocument.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this.Parent.Page, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(BusinessLogic.Utility.LogFilePath, ex);
            }
        }

        protected void gvwDocument_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                BusinessLogic.Utility.DocumentNavigation(e, gvwDocument, UserSession.Document);

                if (e.CommandName.ToLower() == "approve")
                {
                    int intDataKeyValue = Convert.ToInt32(e.CommandArgument);

                    DataRow[] objDataRowDocument = UserSession.Document.Select("DocumentID='" + intDataKeyValue.ToString() + "'");

                    objDataRowDocument[0]["VerifyStatus"] = 1;

                    UserSession.Document.AcceptChanges();

                    if (gvwDocument.PageIndex < gvwDocument.PageCount)
                    {
                        gvwDocument.PageIndex = gvwDocument.PageIndex + 1;
                        gvwDocument.DataSource = UserSession.Document;
                        gvwDocument.DataBind();
                    }
                }
                else if (e.CommandName.ToLower() == "reject")
                {
                    int intDataKeyValue = Convert.ToInt32(e.CommandArgument);

                    DataRow[] objDataRowDocument = UserSession.Document.Select("DocumentID='" + intDataKeyValue.ToString() + "'");

                    objDataRowDocument[0]["VerifyStatus"] = 2;

                    UserSession.Document.AcceptChanges();

                    if (gvwDocument.PageIndex < gvwDocument.PageCount)
                    {
                        gvwDocument.PageIndex = gvwDocument.PageIndex + 1;
                        gvwDocument.DataSource = UserSession.Document;
                        gvwDocument.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this.Parent.Page, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(BusinessLogic.Utility.LogFilePath, ex);
            }
        }

        protected void gvwDocumentVersion_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvwDocumentVersion.PageIndex = e.NewPageIndex;
                gvwDocumentVersion.DataSource = UserSession.Document;
                gvwDocumentVersion.DataBind();
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this.Parent.Page, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(BusinessLogic.Utility.LogFilePath, ex);
            }
        }

        protected void gvwDocumentVersion_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Download")
                {
                    int intRowIndex = Convert.ToInt32(e.CommandArgument);
                    string strDocumentPath = gvwDocumentVersion.DataKeys[intRowIndex].Values["DocumentPath"].ToString();
                    string strDocumentName = gvwDocumentVersion.DataKeys[intRowIndex].Values["DocumentName"].ToString();

                    if (!File.Exists(strDocumentPath))
                    {
                        UserSession.DisplayMessage(this.Parent.Page, "File Not Found .", MainMasterPage.MessageType.Warning);
                        return;
                    }

                    Response.ContentType = "application/octet-stream";
                    Response.AppendHeader("content-disposition", "attachment;filename=\"" + strDocumentName + "\"");
                    Response.TransmitFile(strDocumentPath);
                    Response.Flush();
                    Response.End();
                }
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this.Parent.Page, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(BusinessLogic.Utility.LogFilePath, ex);
            }
        }

        protected void gvwDocumentVersion_RowDataBound(object sender, GridViewRowEventArgs e)
        {
           
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    ((AjaxControlToolkit.ToolkitScriptManager)this.Parent.Page.Master.FindControl("tsmManager")).RegisterPostBackControl(e.Row.FindControl("lnkDownload"));
                    
                }
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this.Parent.Page, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(BusinessLogic.Utility.LogFilePath, ex);
            }
        }

       
        //protected void ibtnVersion_Click(object sender, ImageClickEventArgs e)
        //{
        //    DbTransaction objDbTransaction = Utility.GetTransaction;
        //    try
        //    {
               
        //        if (filUploadVersion.HasFile == true)
        //        {
        //            int intDocumentID = Convert.ToInt32(Request["DocID"]);
        //            DataTable objExistingDocument = DataHelper.ExecuteDataTable("select * from vwDocument where id=" + intDocumentID, null);

                    
        //            DocumentVersion objDocumentVersion = new DocumentVersion();
        //            objDocumentVersion.DocumentID = intDocumentID;
        //            objDocumentVersion.DocumentName = objExistingDocument.Rows[0]["DocumentName"].ToString();
        //            objDocumentVersion.DocumentSize = Convert.ToInt32(objExistingDocument.Rows[0]["Size"].ToString());
        //            objDocumentVersion.DocumentType = Path.GetExtension(objExistingDocument.Rows[0]["DocumentName"].ToString()).ToLower();
        //            objDocumentVersion.DocumentPath = Path.Combine(Utility.DMSVersionPath, intDocumentID.ToString(), DateTime.Now.ToString("dd-MMM-yyyy hh-mm-ss") + objDocumentVersion.DocumentType);
        //            objDocumentVersion.CreatedBy = UserSession.UserID;
        //            objDocumentVersion.CreatedOn = DateTime.Now;             


        //            if (DocumentVersion.Insert(objDocumentVersion, objDbTransaction) == Utility.ResultType.Error)
        //            {
        //                objDbTransaction.Rollback();
        //                UserSession.DisplayMessage(this.Parent.Page, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
        //                return;
        //            }

        //            Document objDocument = new Document();
        //            objDocument.DocumentID = intDocumentID;
        //            objDocument.DocumentName = filUploadVersion.FileName;
        //            objDocument.Size = filUploadVersion.PostedFile.ContentLength;
        //            objDocument.DocumentType = Path.GetExtension(filUploadVersion.FileName).ToLower();
        //            objDocument.UpdatedBy = UserSession.UserID;
        //            objDocument.UpdatedOn = DateTime.Now;
        //            objDocument.DocumentPath = Path.Combine(Path.GetDirectoryName(objExistingDocument.Rows[0]["DocumentPath"].ToString()), objDocument.DocumentName);

        //            if (Document.UpdateForVersion(objDocument, objDbTransaction) == Utility.ResultType.Error)
        //            {
        //                objDbTransaction.Rollback();
        //                UserSession.DisplayMessage(this.Parent.Page, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
        //                return;
        //            }

        //            if (!Directory.Exists(Path.Combine(Utility.DMSVersionPath, intDocumentID.ToString())))
        //                Utility.CreateDirectory(Path.Combine(Utility.DMSVersionPath, intDocumentID.ToString()));

        //            File.Copy(objExistingDocument.Rows[0]["DocumentPath"].ToString(), objDocumentVersion.DocumentPath);

        //            filUploadVersion.SaveAs(objDocument.DocumentPath);

        //            objDbTransaction.Commit();
        //            DataTable dt = DataHelper.ExecuteDataTable("select * from vwDocumentversion where documentid=" + intDocumentID, null);
                    
        //            gvwDocumentVersion.DataSource = dt;
        //            gvwDocumentVersion.DataBind();
                    

        //            UserSession.DisplayMessage(this.Parent.Page, "New Document Has Been Uploaded Successfully", MainMasterPage.MessageType.Success);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        objDbTransaction.Rollback();
        //        UserSession.DisplayMessage(this.Parent.Page, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
        //        LogManager.ErrorLog(BusinessLogic.Utility.LogFilePath, ex);
        //    }
        //}

        //private void LoadMakerChecker()
        //{
        //    try
        //    {
        //        lblDocumentName.Text = "Name : " + Utility.Trim(DocumentList.Rows[0]["DocumentName"].ToString(), 75);
        //        lblDocumentPath.Text = "Path : " + Utility.Trim(DocumentList.Rows[0]["DocumentPathDescription"].ToString(), 75);
        //        lblDocumentTag.Text = "Tag : " + Utility.Trim(DocumentList.Rows[0]["DocumentTag"].ToString(), 75);
        //        lblDocumentSize.Text = "Size : " + DocumentList.Rows[0]["DocumentSize"].ToString();
        //        lblDocumentStatus.Text = "Current Status : " + DocumentList.Rows[0]["StatusName"].ToString();
        //        lblUserName.Text = "From : " + DocumentList.Rows[0]["UserFullName"].ToString();
        //        lblModifiedOn.Text = "Modified On : " + Convert.ToDateTime(DocumentList.Rows[0]["ModificationDate"]).ToString("f");
        //        imgDocumentLogo.ImageUrl = DocumentList.Rows[0]["DocumentImageUrl"].ToString();
        //    }
        //    catch (Exception ex)
        //    {
        //        UserSession.DisplayMessage(this.Parent.Page, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
        //        LogManager.ErrorLog(BusinessLogic.Utility.LogFilePath, ex);
        //    }
        //}
        #endregion    
    }
}