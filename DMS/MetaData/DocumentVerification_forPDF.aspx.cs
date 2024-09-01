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
using System.Configuration;
using System.Data.SqlClient;

namespace DMS.Shared
{
    public partial class DocumentVerification_forPDF : System.Web.UI.Page
    {
        #region Private Member
        public enum DocumentStatus { Default,Approve,Reject}
        public static DocumentStatus Status = DocumentStatus.Default;
        BusinessLogic.Utility objUtility = new BusinessLogic.Utility();
        BusinessLogic.DocumentManager objDocumentManager = new BusinessLogic.DocumentManager();
        BusinessLogic.MetaTemplateManager objMetaTemplateManager = new BusinessLogic.MetaTemplateManager();
        string strDocumentID = string.Empty;
        #endregion

        #region Event
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.UrlReferrer != null)
                {
                    ViewState["LASTPAGEURL"] = Request.UrlReferrer.ToString().ToLower();

                    if (ViewState["LASTPAGEURL"].ToString().Contains("withfielddata"))
                    {
                        ibtnCancel.Visible = false;
                    }
                    else
                    {
                        ibtnCancel.Visible = true;
                    }
                }
            }
            //DocumentViewer.FindControl("ImageButton1").Visible = false;
            //ImageButton btnImagesubmit = DocumentViewer.FindControl("ibtnsubmit") as ImageButton;
           // btnImagesubmit.Visible = false;
            //btnImagesubmit.Click += new ImageClickEventHandler(btnImagesubmit_Click);
            gdv_DocumentField.DataSource = null;
            if (Request["DOCID"] != null)
            {
                strDocumentID = Request["DOCID"].ToString().Trim();
                Session["DOCID"] = strDocumentID;
            }

            pdfViewer.FilePath = "../Handler/PDFHandler.ashx?DocID=" + strDocumentID + "";

            pdfViewer.Visible = true;

            bindgrid(strDocumentID);

           // GridBinding();

        }

        void btnImagesubmit_Click(object sender, ImageClickEventArgs e)
        {
                if(Status == DocumentStatus.Approve)
            {
                //objDocumentManager.ApproveRejectStatus(int.Parse(strDocumentID), 1, txtComment.Text.Trim(), UserSession.UserID);
                UserSession.DisplayMessage(this, "Document Status Updated Successfully.", MainMasterPage.MessageType.Success);
            }
            else if(Status == DocumentStatus.Reject)
            {
                //objDocumentManager.ApproveRejectStatus(int.Parse(strDocumentID), 2, txtComment.Text.Trim(), UserSession.UserID);
                UserSession.DisplayMessage(this, "Document Status Updated Successfully.", MainMasterPage.MessageType.Success);
            }
            

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
                    else if (ViewState["LASTPAGEURL"].ToString().Contains("searchinall"))
                    {
                        Response.Redirect("../MetaData/SearchInAll.aspx", false);
                    }
                }
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(BusinessLogic.Utility.LogFilePath, ex);
            }

        }

        void bindgrid(string strDocumentID)
        {
            DataTable dt = new DataTable();
            DataTable objDataTableMetaData = new DataTable();
            if (UserSession.MetaDataID != 0)
                objUtility.Result = objDocumentManager.SelectMetaData(out objDataTableMetaData, UserSession.MetaDataID);

            dt = objDocumentManager.GetFieldsandData(Convert.ToInt32(objDataTableMetaData.Rows[0]["MetaTemplateID"]), int.Parse(strDocumentID));

            gdv_DocumentField.DataSource = dt;
            gdv_DocumentField.DataBind();
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
                                        //mkcChecker.DocumentGrid.DataSource = UserSession.Document;
                                        //mkcChecker.DocumentGrid.DataBind();
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
                    else if (ViewState["LASTPAGEURL"].ToString().Contains("searchinall"))
                    {
                        Response.Redirect("../MetaData/SearchInAll.aspx", false);
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

        protected void gdv_DocumentField_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gdv_DocumentField.EditIndex = -1;
            gdv_DocumentField.DataBind();
        }

        protected void gdv_DocumentField_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gdv_DocumentField.EditIndex = e.NewEditIndex;
            gdv_DocumentField.DataBind();

        }

        protected void gdv_DocumentField_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                GridViewRow row = gdv_DocumentField.Rows[e.RowIndex];
                int id = Convert.ToInt32(gdv_DocumentField.DataKeys[e.RowIndex].Values[0]);
                string txtEditFieldData = (row.FindControl("txtEditFieldData") as TextBox).Text;

                UpdateDocumentEntry(id, txtEditFieldData);

                UserSession.DisplayMessage(this, "Document Entry Updated Successfully .", MainMasterPage.MessageType.Success);
                gdv_DocumentField.EditIndex = -1;
                bindgrid(strDocumentID);
            }
            catch (Exception ex)
            {
                //LogManager.ErrorLog(Utility.LogFilePath, ex);
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Occurred .", MainMasterPage.MessageType.Error);
            }

        }


        public void UpdateDocumentEntry(int ID, string FieldData)
        {
            string strCon = ConfigurationManager.AppSettings["ConnectionString"];

            using (SqlConnection conn = new SqlConnection(strCon))
            {
                SqlDataAdapter da = new SqlDataAdapter();
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SP_U_DocumentEntry_Virescent", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", ID);
                    cmd.Parameters.AddWithValue("@FieldData", FieldData);
                    cmd.Parameters.AddWithValue("@UpdatedOn", DateTime.Now);
                    cmd.Parameters.AddWithValue("@UpdatedBy", UserSession.UserID);
                    int RowAffected = cmd.ExecuteNonQuery();
                    //da.SelectCommand = cmd;
                    // da.Fill(dt);
                    conn.Close();
                }
            }
        }


        protected void ibtnApprove_Click(object sender, ImageClickEventArgs e)
        {
            //lblComment.Visible = false;
            //txtComment.Visible = false;
            Status = DocumentStatus.Approve;
        }

        protected void ibtnReject_Click(object sender, ImageClickEventArgs e)
        {
            //lblComment.Visible = true;
            //txtComment.Visible = true;
            Status = DocumentStatus.Reject;
        }

        protected void ibtnCancel_Click(object sender, ImageClickEventArgs e)
        {
           // Response.Redirect("../MetaData/DocumentEntry_New.aspx", false);
            try
            {
                if (ViewState["LASTPAGEURL"] != null)
                {
                    if (ViewState["LASTPAGEURL"].ToString().Contains("documententry_new"))
                    {
                        Response.Redirect("../MetaData/DocumentEntry_New.aspx", false);
                    }
                    if (ViewState["LASTPAGEURL"].ToString().Contains("withfielddata"))
                    {
                        Response.Redirect("../MetaData/SearchDocumentWithFieldData.aspx", false);
                    }
                    else if (ViewState["LASTPAGEURL"].ToString().Contains("tag"))
                    {
                        Response.Redirect("../MetaData/SearchDocument_Tag.aspx", false);
                    }
                    else if (ViewState["LASTPAGEURL"].ToString().Contains("folder"))
                    {
                        Response.Redirect("../MetaData/SearchByFolder.aspx", false);
                    }
                    else if (ViewState["LASTPAGEURL"].ToString().Contains("searchdocument"))
                    {
                        Response.Redirect("../MetaData/SearchDocument.aspx", false);
                    }
                    else if (ViewState["LASTPAGEURL"].ToString().Contains("searchinall"))
                    {
                        Response.Redirect("../MetaData/SearchInAll.aspx", false);
                    }
                    else if (ViewState["LASTPAGEURL"].ToString().Contains("datatablesearch"))
                    {
                        Response.Redirect("../MetaData/DataTableSearch.aspx", false);
                    }
                    
                   
                }
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(BusinessLogic.Utility.LogFilePath, ex);
            }
        }

        protected void gvwDocumentView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.ToLower().Trim() == "documentview")
                {
                    int intRowIndex = Convert.ToInt32(e.CommandArgument);
                    UserSession.MetaDataID = Convert.ToInt32(gvwDocumentView.DataKeys[intRowIndex].Values["MetaDataID"]);
                    strDocumentID = gvwDocumentView.DataKeys[intRowIndex].Values["ID"].ToString().Trim();
                    Session["DocId"] = strDocumentID;

                    pdfViewer.FilePath = "../Handler/PDFHandler.ashx?DocID=" + strDocumentID + "";

                    pdfViewer.Visible = true;

                    //bindgrid(strDocumentID);

                    //Response.Redirect("../MetaData/DocumentVerification_forPDF.aspx?DOCID=" + strDocumentID+"");
                    //GridBinding();
                }
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(BusinessLogic.Utility.LogFilePath, ex);
            }
                                            
         }

        

        public void GridBinding()
        {
            DataTable dt = new DataTable();
            dt = GetNotingFile(Convert.ToInt32(Session["DOCID"]));
            gvwDocumentView.DataSource = dt;
            gvwDocumentView.DataBind();
            
        }
        
        public DataTable GetNotingFile(int DOCID)
        {
            DataTable dt = new DataTable();
            try
            {
                string strCon = ConfigurationManager.AppSettings["ConnectionString"];

                using (SqlConnection conn = new SqlConnection(strCon))
                {
                    SqlDataAdapter da = new SqlDataAdapter();
                    using (SqlCommand cmd = new SqlCommand("Sp_S_LoadNotingFile", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@DocID", DOCID);
                        cmd.Parameters.Add("@Tag", SqlDbType.Int).Direction = ParameterDirection.Output;

                        da.SelectCommand = cmd;
                        da.Fill(dt);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(BusinessLogic.Utility.LogFilePath, ex);
            }
            return dt;
        }


    }
}