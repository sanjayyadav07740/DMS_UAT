using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using PdfViewer;
using AjaxControlToolkit;
using System.Data;
using System.Data.Common;
using System.Web.UI.HtmlControls;
using DMS.BusinessLogic;

namespace DMS.Shared
{
    public partial class DocumentVerification : System.Web.UI.Page
    {
        #region Private Member
        BusinessLogic.Utility objUtility = new BusinessLogic.Utility();
        BusinessLogic.DocumentManager objDocumentManager = new BusinessLogic.DocumentManager();
        BusinessLogic.MetaTemplateManager objMetaTemplateManager = new BusinessLogic.MetaTemplateManager();
        #endregion

        #region Event
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //DataTable dt4 = (DataTable)Session["sampletable"];
                //gvwDocumentVersion.DataSource = dt4;
                //gvwDocumentVersion.DataBind();

                if (Request.UrlReferrer != null)
                {
                    ViewState["LASTPAGEURL"] = Request.UrlReferrer.ToString().ToLower();
                }
            }
        }

        protected void ibtnSubmit_Click(object sender, ImageClickEventArgs e)
        {
            DbTransaction objDbTransaction = BusinessLogic.Utility.GetTransaction;
            try
            {
                if (UserSession.Document.Select("VerifyStatus='1' OR VerifyStatus='2' OR IsCheck='1'").Length == 0)
                {
                    UserSession.DisplayMessage(this, "Please Verify Atleast Single Entry .", MainMasterPage.MessageType.Warning);
                    return;
                }

                DataTable objDataTableCompletedEntry = null;
                if (UserSession.Document.Select("IsCheck=1").Length > 0)
                {
                    objDataTableCompletedEntry = UserSession.Document.Select("IsCheck=1").CopyToDataTable();
                }
                BusinessLogic.DocumentEntry objDocumentEntryExist = new BusinessLogic.DocumentEntry();
                foreach (DataRow objDataRow in UserSession.Field.Rows)
                {
                    if (objDataRow["IsPrimary"].ToString().Trim() == "1")
                    {
                        if (objDataTableCompletedEntry != null)
                        {
                            for (int i = 0; i < objDataTableCompletedEntry.Rows.Count; i++)
                            {
                                objDocumentEntryExist.FieldID = Convert.ToInt32(objDataRow["ID"]);
                                objDocumentEntryExist.FieldData = objDataTableCompletedEntry.Rows[i][objDataRow["ID"].ToString()].ToString();
                                objDocumentEntryExist.DocumentID = Convert.ToInt32(objDataTableCompletedEntry.Rows[i]["DocumentID"].ToString());
                                objUtility.Result = objDocumentManager.SelectDocumentData(objDocumentEntryExist);

                                switch (objUtility.Result)
                                {
                                    case BusinessLogic.Utility.ResultType.Success:
                                        UserSession.DisplayMessage(this, "Value Of " + objDataRow["FieldName"] + " Field Of Document " + (i + 1).ToString() + " Is Already Exist .", MainMasterPage.MessageType.Warning);
                                        mkcChecker.DocumentGrid.DataSource = UserSession.Document;
                                        mkcChecker.DocumentGrid.DataBind();
                                        return;
                                        break;

                                    case BusinessLogic.Utility.ResultType.Error:
                                        objDbTransaction.Rollback();
                                        UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                                        return;
                                        break;
                                }

                            }
                        }
                    }
                }

                foreach (DataRow objDataRow in UserSession.Document.Select("IsCheck='1'"))
                {
                    BusinessLogic.DocumentEntry objDocumentEntry = new BusinessLogic.DocumentEntry();
                    objDocumentEntry.DocumentID = Convert.ToInt32(objDataRow["DocumentID"]);

                    foreach (DataRow objDataRowNew in UserSession.Field.Rows)
                    {
                        int intAffectedRows = 0;

                        objDocumentEntry.FieldID = Convert.ToInt32(objDataRowNew["ID"]);
                        objDocumentEntry.FieldData = objDataRow[objDataRowNew["ID"].ToString()].ToString();
                        objDocumentEntry.UpdatedBy = UserSession.UserID;

                        objUtility.Result = objDocumentManager.UpdateDocumentEntry(objDocumentEntry, objDbTransaction, out intAffectedRows);
                        switch (objUtility.Result)
                        {
                            case BusinessLogic.Utility.ResultType.Failure:
                            case BusinessLogic.Utility.ResultType.Error:
                                objDbTransaction.Rollback();
                                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                                return;
                                break;
                        }

                        if (intAffectedRows == 0)
                        {
                            objUtility.Result = objDocumentManager.InsertDocumentEntry(objDocumentEntry, objDbTransaction);
                            switch (objUtility.Result)
                            {
                                case BusinessLogic.Utility.ResultType.Failure:
                                case BusinessLogic.Utility.ResultType.Error:
                                    objDbTransaction.Rollback();
                                    UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                                    return;
                                    break;
                            }
                        }

                    }
                }

                foreach (DataRow objDataRow in UserSession.Document.Select("VerifyStatus <> '0'"))
                {
                    BusinessLogic.Document objDocument = new BusinessLogic.Document();
                    objDocument.DocumentID = Convert.ToInt32(objDataRow["DocumentID"]);
                    objDocument.DocumentStatusID = Convert.ToInt32(objDataRow["VerifyStatus"]);
                    objDocument.UpdatedBy = UserSession.UserID;


                    objUtility.Result = objDocumentManager.UpdateDocument(objDocument, objDbTransaction);
                    switch (objUtility.Result)
                    {
                        case BusinessLogic.Utility.ResultType.Failure:
                        case BusinessLogic.Utility.ResultType.Error:
                            objDbTransaction.Rollback();
                            UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                            return;
                            break;
                    }
                }
                objDbTransaction.Commit();

                Response.Redirect("../MetaData/ViewForDocumentVerification.aspx?Type=1", false);
            }
            catch (Exception ex)
            {
                objDbTransaction.Rollback();
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(BusinessLogic.Utility.LogFilePath, ex);
            }
        }

        protected void ibtnBack_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (ViewState["LASTPAGEURL"] != null)
                {
                    if (ViewState["LASTPAGEURL"].ToString().Contains("documententry"))
                    {
                        Response.Redirect("../MetaData/ViewForDocumentEntry.aspx", false);
                    }
                    else if (ViewState["LASTPAGEURL"].ToString().Contains("documentverification"))
                    {
                        Response.Redirect("../MetaData/ViewForDocumentVerification.aspx", false);
                    }
                    else if (ViewState["LASTPAGEURL"].ToString().Contains("approveddocument"))
                    {
                        Response.Redirect("../MetaData/ViewForApprovedDocument.aspx", false);
                    }
                    else if (ViewState["LASTPAGEURL"].ToString().Contains("rejecteddocument"))
                    {
                        Response.Redirect("../MetaData/RejectedDocument.aspx", false);
                    }
                    else if (ViewState["LASTPAGEURL"].ToString().Contains("documentdashboard"))
                    {
                        Response.Redirect("../MetaData/DocumentDashBoard.aspx", false);
                    }
                    else if (ViewState["LASTPAGEURL"].ToString().Contains("searchdocument"))
                    {
                        Response.Redirect("../MetaData/SearchDocument.aspx", false);
                    }
                }
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(BusinessLogic.Utility.LogFilePath, ex);
            }
        }
        #endregion

        protected void gvwDocumentVersion_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            
        }

        protected void gvwDocumentVersion_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void gvwDocumentVersion_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

       
    }
}