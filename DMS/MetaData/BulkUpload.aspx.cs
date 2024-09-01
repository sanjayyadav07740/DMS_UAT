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
    public partial class BulkUpload : System.Web.UI.Page
    {
        #region Private Member
        Utility objUtility = new Utility();
        #endregion

        #region Event
        protected void Page_Load(object sender, EventArgs e)
        {
            ((AjaxControlToolkit.ToolkitScriptManager)Master.FindControl("tsmManager")).RegisterPostBackControl(ibtnDescription);
            ((AjaxControlToolkit.ToolkitScriptManager)Master.FindControl("tsmManager")).RegisterPostBackControl(ibtnUploadPattern);
            ((AjaxControlToolkit.ToolkitScriptManager)Master.FindControl("tsmManager")).RegisterPostBackControl(ibtnUploadExcel);
            ((AjaxControlToolkit.ToolkitScriptManager)Master.FindControl("tsmManager")).RegisterPostBackControl(ibtnUploadDocument);
            ((AjaxControlToolkit.ToolkitScriptManager)Master.FindControl("tsmManager")).RegisterPostBackControl(gvwDocument);
        }

        protected void ibtnDescription_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                objUtility.Result = Utility.ExportMetaTemplateDescription(emodModule.SelectedMetaTemplate);
                switch (objUtility.Result)
                {
                    case Utility.ResultType.Failure:
                        UserSession.DisplayMessage(this, "MetaTemplate Field Structure Or List Structure Does Not Present .", MainMasterPage.MessageType.Warning);
                        return;
                        break;

                    case Utility.ResultType.Error:
                        UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                        return;
                        break;
                }
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        protected void ibtnUploadPattern_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                objUtility.Result = Utility.ExportMetaTemplateUploadPattern(emodModule.SelectedMetaTemplate);
                switch (objUtility.Result)
                {
                    case Utility.ResultType.Failure:
                        UserSession.DisplayMessage(this, "MetaTemplate Field Structure Or List Structure Does Not Present .", MainMasterPage.MessageType.Warning);
                        return;
                        break;

                    case Utility.ResultType.Error:
                        UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                        return;
                        break;
                }
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        protected void ibtnUploadExcel_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (filUpload.PostedFile.ContentLength == 0)
                {
                    UserSession.DisplayMessage(this, "Can Not Add Zero Length Or Damage File .", MainMasterPage.MessageType.Warning);
                    return;
                }
                if (!Path.GetExtension(filUpload.PostedFile.FileName).ToLower().Contains(".xls"))
                {
                    UserSession.DisplayMessage(this, "Please Upload Excel File Only.", MainMasterPage.MessageType.Warning);
                    return;
                }

                DMS.BusinessLogic.Entity.BulkUpload objBulkUpload = new BusinessLogic.Entity.BulkUpload();
                string strDocumentGuid = System.Guid.NewGuid() + System.IO.Path.GetExtension(filUpload.PostedFile.FileName);
                objBulkUpload.DocumentGuid = strDocumentGuid;
                objBulkUpload.DocumentPath = Utility.BulkFilePath + strDocumentGuid;
                objBulkUpload.RepositoryID = emodModule.SelectedRepository;
                objBulkUpload.MetaTemplateID = emodModule.SelectedMetaTemplate;
                objBulkUpload.FolderID = emodModule.SelectedFolder;
                objBulkUpload.CategoryID = emodModule.SelectedCategory;
                objBulkUpload.CreatedBy = UserSession.UserID;
                objBulkUpload.UploadStatus = 1;
                objBulkUpload.Status = (int)Utility.Status.Active;

                FileStream objFileStream = new FileStream(objBulkUpload.DocumentPath, FileMode.CreateNew, FileAccess.Write);
                objFileStream.Write(filUpload.FileBytes, 0, filUpload.PostedFile.ContentLength);
                objFileStream.Close();

                objUtility.Result = new DocumentManager().InsertBulkUpload(objBulkUpload);

                switch (objUtility.Result)
                {
                    case Utility.ResultType.Success:
                        UserSession.DisplayMessage(this, "File Is Uploaded Successfully With ID As "+objBulkUpload.BulkUploadCode+" .", MainMasterPage.MessageType.Success);
                        return;
                        break;

                    case Utility.ResultType.Error:
                        UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                        return;
                        break;
                }
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        protected void ibtnUploadDocument_Click(object sender, ImageClickEventArgs e)
        {
            string strDocumentName = null;
            string strDocumentGuid = null;
            string strDocumentPath = null;
            string strDocumentType = null;
            int intSize = 0;
            byte[] byteImage = null;
            try
            {
                if (filUpload.PostedFile.ContentLength == 0)
                {
                    UserSession.DisplayMessage(this, "Can Not Add Zero Length Or Damage File .", MainMasterPage.MessageType.Warning);
                    return;
                }
                if (UserSession.GridData == null || UserSession.GridData.Columns["Tag"] == null)
                {
                    gvwDocument.Visible = false;
                    InitializeDataTable();
                }

                if (filUpload.PostedFile.ContentLength == 0)
                {
                    UserSession.DisplayMessage(this, "Can Not Add Zero Length Or Damage File .", MainMasterPage.MessageType.Warning);
                    return;
                }
                gvwUploadDocument.Visible = true;
                gvwDocument.Visible = false;

                strDocumentName = filUpload.FileName.Replace(" ", string.Empty);
                if (Path.GetExtension(filUpload.FileName).ToLower().Trim() != ".zip")
                {
                    strDocumentGuid = System.Guid.NewGuid().ToString() + Path.GetExtension(strDocumentName);
                    strDocumentPath = Utility.DocumentPath + strDocumentGuid;
                    strDocumentType = Path.GetExtension(strDocumentName).ToLower();
                    intSize = filUpload.PostedFile.ContentLength;
                    byteImage = null;
                    byteImage = filUpload.FileBytes;

                    if (byteImage.Length != 0)
                    {
                        DataRow objDataRow = UserSession.GridData.NewRow();
                        objDataRow["DocumentName"] = strDocumentName;
                        objDataRow["DocumentGuid"] = strDocumentGuid;
                        objDataRow["DocumentPath"] = strDocumentPath;
                        objDataRow["DocumentType"] = strDocumentType;
                        objDataRow["Size"] = intSize;
                        objDataRow["Image"] = byteImage;
                        objDataRow["Tag"] = string.Empty;
                        UserSession.GridData.Rows.Add(objDataRow);
                        UserSession.GridData.AcceptChanges();
                    }
                    else
                    {
                        UserSession.DisplayMessage(this, "Can Not Add Zero Length Or Damage File .", MainMasterPage.MessageType.Warning);
                        return;
                    }
                }
                else
                {
                    ZipFile objZipFile = ZipFile.Read(filUpload.PostedFile.InputStream);

                    foreach (ZipEntry objZipEntry in objZipFile)
                    {
                        if (objZipEntry.IsDirectory == false && Path.GetExtension(objZipEntry.FileName).ToLower().Trim() != ".db")
                        {
                            strDocumentName = Path.GetFileName(objZipEntry.FileName.Replace(" ", string.Empty));
                            strDocumentGuid = System.Guid.NewGuid().ToString() + Path.GetExtension(strDocumentName);
                            strDocumentPath = Utility.DocumentPath + strDocumentGuid;
                            strDocumentType = Path.GetExtension(objZipEntry.FileName).ToLower();

                            Stream objStream = objZipEntry.OpenReader();
                            intSize = Convert.ToInt32(objStream.Length);
                            byteImage = null;
                            byteImage = new byte[intSize];
                            objStream.Read(byteImage, 0, intSize);
                            objStream.Close();

                            if (byteImage.Length != 0)
                            {
                                DataRow objDataRow = UserSession.GridData.NewRow();
                                objDataRow["DocumentName"] = strDocumentName;
                                objDataRow["DocumentGuid"] = strDocumentGuid;
                                objDataRow["DocumentPath"] = strDocumentPath;
                                objDataRow["DocumentType"] = strDocumentType;
                                objDataRow["Size"] = intSize;
                                objDataRow["Image"] = byteImage;
                                objDataRow["Tag"] = string.Empty;
                                UserSession.GridData.Rows.Add(objDataRow);
                                UserSession.GridData.AcceptChanges();
                            }
                        }
                    }
                }



                DataView objDataView = new DataView(UserSession.GridData);
                objDataView.Sort = "ID DESC";
                gvwUploadDocument.DataSource = objDataView;
                gvwUploadDocument.DataBind();

                
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        protected void ibtnDeleteChecked_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                foreach (GridViewRow objGridViewRow in gvwUploadDocument.Rows)
                {
                    if (((CheckBox)objGridViewRow.FindControl("chkRow")).Checked)
                    {
                        int intRowIndex = objGridViewRow.RowIndex;
                        int intDataKeyValue = Convert.ToInt32(gvwUploadDocument.DataKeys[intRowIndex].Value);

                        UserSession.GridData.Select("ID=" + intDataKeyValue)[0].Delete();
                        
                    }
                }
                UserSession.GridData.AcceptChanges();

                DataView objDataView = new DataView(UserSession.GridData);
                objDataView.Sort = "ID DESC";
                gvwUploadDocument.DataSource = objDataView;
                gvwUploadDocument.DataBind();
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }


        }

        protected void ibtnShow_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                LoadGridDataByCriteria();
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        protected void gvwDocument_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                if (UserSession.GridData != null)
                {
                    if (ViewState[UserSession.SortExpression] == null || ViewState[UserSession.SortExpression].ToString() == "DESC")
                        ViewState[UserSession.SortExpression] = "ASC";
                    else
                        ViewState[UserSession.SortExpression] = "DESC";

                    gvwDocument.DataSource = UserSession.SortedGridData(e.SortExpression, ViewState[UserSession.SortExpression].ToString());
                    gvwDocument.DataBind();
                }
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        protected void gvwDocument_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                Utility.SetGridHoverStyle(e);               
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        protected void gvwDocument_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.ToLower().Trim() == "download")
                {
                    int intRowIndex = Convert.ToInt32(e.CommandArgument);
                    string strDownloadPath= gvwDocument.DataKeys[intRowIndex].Values["DownloadPath"].ToString().Trim();
                    if (strDownloadPath != string.Empty)
                    {
                        if (System.IO.File.Exists(strDownloadPath))
                        {
                            Response.ContentType = "appliction/octet-stream";
                            Response.AddHeader("content-disposition", "attachment;filename=Download" + System.IO.Path.GetExtension(strDownloadPath));
                            Response.TransmitFile(strDownloadPath);
                            Response.End();
                        }
                        else
                        {
                            UserSession.DisplayMessage(this, "No File To Download .", MainMasterPage.MessageType.Warning);
                        }
                    }
                    else
                    {
                        UserSession.DisplayMessage(this, "No File To Download .", MainMasterPage.MessageType.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        protected void gvwDocument_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                if (UserSession.GridData != null)
                {
                    gvwDocument.PageIndex = e.NewPageIndex;
                    gvwDocument.DataSource = UserSession.GridData;
                    gvwDocument.DataBind();
                }
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        protected void gvwUploadDocument_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                if (UserSession.GridData != null)
                {
                    if (ViewState[UserSession.SortExpression] == null || ViewState[UserSession.SortExpression].ToString() == "DESC")
                        ViewState[UserSession.SortExpression] = "ASC";
                    else
                        ViewState[UserSession.SortExpression] = "DESC";

                    gvwUploadDocument.DataSource = UserSession.SortedGridData(e.SortExpression, ViewState[UserSession.SortExpression].ToString());
                    gvwUploadDocument.DataBind();
                }
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        protected void gvwUploadDocument_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                Utility.SetGridHoverStyle(e);
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        protected void gvwUploadDocument_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.ToLower() == "removedocument")
                {
                    int intRowIndex = Convert.ToInt32(e.CommandArgument);
                    int intDataKeyValue = Convert.ToInt32(gvwUploadDocument.DataKeys[intRowIndex].Value);

                    UserSession.GridData.Select("ID=" + intDataKeyValue)[0].Delete();
                    UserSession.GridData.AcceptChanges();

                    DataView objDataView = new DataView(UserSession.GridData);
                    objDataView.Sort = "ID DESC";
                    gvwUploadDocument.DataSource = objDataView;
                    gvwUploadDocument.DataBind();
                }
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        protected void gvwUploadDocument_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                if (UserSession.GridData != null)
                {
                    gvwUploadDocument.PageIndex = e.NewPageIndex;
                    gvwUploadDocument.DataSource = UserSession.GridData;
                    gvwUploadDocument.DataBind();
                }
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        protected void ibtnSubmit_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (emodModule.SelectedMetaDataCode == -1)
                {
                    UserSession.DisplayMessage(this, "Please Select The MetaDataCode .", MainMasterPage.MessageType.Warning);
                    return;
                }
                CheckDocumentStatus();
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        #endregion

        #region Method
        private void LoadGridDataByCriteria()
        {
            try
            {
                gvwUploadDocument.Visible = false;
                gvwDocument.Visible = true;

                DocumentManager objDocumentManager = new DocumentManager();
                BusinessLogic.Entity.BulkUpload objBulkUpload = new BusinessLogic.Entity.BulkUpload()
                {
                    RepositoryID = emodModule.SelectedRepository,
                    MetaTemplateID = emodModule.SelectedMetaTemplate,
                    CategoryID = emodModule.SelectedCategory,
                    FolderID = emodModule.SelectedFolder
                };
                DataTable objDataTable = new DataTable();
                objUtility.Result = objDocumentManager.SelectBulkUpload(out objDataTable, objBulkUpload);

                switch (objUtility.Result)
                {
                    case Utility.ResultType.Success:
                        gvwDocument.DataSource = objDataTable;
                        gvwDocument.DataBind();
                        UserSession.GridData = objDataTable;
                        break;

                    case Utility.ResultType.Failure:
                        gvwDocument.DataSource = objDataTable;
                        gvwDocument.DataBind();
                        UserSession.DisplayMessage(this, "No Data To Display .", MainMasterPage.MessageType.Warning);
                        break;

                    case Utility.ResultType.Error:
                        gvwDocument.DataSource = objDataTable;
                        gvwDocument.DataBind();
                        UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                        break;
                }
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
            }
        }

        private void InitializeDataTable()
        {
            try
            {
                DataTable objDataTable = new DataTable();
                objDataTable.Columns.Add("ID", typeof(int)).AutoIncrement = true;
                objDataTable.Columns.Add("DocumentName", typeof(string));
                objDataTable.Columns.Add("DocumentGuid", typeof(string));
                objDataTable.Columns.Add("DocumentPath", typeof(string));
                objDataTable.Columns.Add("DocumentType", typeof(string));
                objDataTable.Columns.Add("Size", typeof(int));
                objDataTable.Columns.Add("Image", typeof(byte[]));
                objDataTable.Columns.Add("Tag", typeof(string));
                objDataTable.AcceptChanges();
                UserSession.GridData = objDataTable;
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        private void CheckDocumentStatus()
        {
            try
            {
                if (UserSession.GridData != null && UserSession.GridData.Columns["Tag"] != null)
                {
                    DocumentManager objDocumentManager = new DocumentManager();
                    Document objDocument = new Document();
                    foreach (DataRow objDataRow in UserSession.GridData.Rows)
                    {
                        int intDocumentID =0;
                        bool boolResult = int.TryParse(Path.GetFileNameWithoutExtension(objDataRow["DocumentName"].ToString()),out intDocumentID);
                        if(boolResult == false)
                        {
                            UserSession.DisplayMessage(this, "Invalid Document Name " +objDataRow["DocumentName"].ToString() , MainMasterPage.MessageType.Warning);
                            return ;
                        }
                        objDocument.DocumentID = intDocumentID;
                        objDocument.MetaDataID = emodModule.SelectedMetaDataCode;
                        objUtility.Result = objDocumentManager.SelectDocumentForBulkUpload(objDocument);
                        if (objUtility.Result == Utility.ResultType.Failure)
                        {
                            UserSession.DisplayMessage(this, "No Such Document Is Bulk Uploaded ."+objDataRow["DocumentName"].ToString(), MainMasterPage.MessageType.Warning);
                            return;
                        }
                        else if (objUtility.Result == Utility.ResultType.Error)
                        {
                            UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                            return;
                        }
                    }

                    if (Utility.FileStorageType == Utility.StorageType.FileSystem)
                    {
                        foreach (DataRow objDataRow in UserSession.GridData.Rows)
                        {
                            FileStream objFileStream = new FileStream(objDataRow["DocumentPath"].ToString(), FileMode.CreateNew, FileAccess.Write);
                            objFileStream.Write((byte[])objDataRow["Image"], 0, ((byte[])objDataRow["Image"]).Length);
                            objFileStream.Close();
                        }
                    }

                    DbTransaction objDbTransaction = Utility.GetTransaction;
                    foreach (DataRow objDataRow in UserSession.GridData.Rows)
                    {

                        objDocument = new BusinessLogic.Document();
                        objDocument.DocumentID = int.Parse(Path.GetFileNameWithoutExtension(objDataRow["DocumentName"].ToString()));
                        objDocument.DocumentName = objDataRow["DocumentName"].ToString();
                        objDocument.DocumentGuid = objDataRow["DocumentGuid"].ToString();
                        objDocument.DocumentPath = objDataRow["DocumentPath"].ToString();
                        objDocument.DocumentType = objDataRow["DocumentType"].ToString();
                        objDocument.Size = Convert.ToInt32((objDataRow["Size"].ToString()));
                        objDocument.Image = (byte[])objDataRow["Image"];
                        objDocument.IsLucened = 0;
                        objDocument.UpdatedBy = UserSession.UserID;
                        objDocument.DocumentStatusID = (int)DocumentManager.Status.EntryCompleted;
                        objDocument.IsLucened = 0;
                        

                        objUtility.Result = objDocumentManager.UpdateDocumentForBulkUpload(objDocument, objDbTransaction);

                        switch (objUtility.Result)
                        {
                            case Utility.ResultType.Failure:
                            case Utility.ResultType.Error:
                                objDbTransaction.Rollback();
                                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                                break;
                        }

                    }
                    objDbTransaction.Commit();
                    UserSession.DisplayMessage(this, "Files Are Uploaded Successfully .", MainMasterPage.MessageType.Success);
                    UserSession.GridData = null;
                    gvwUploadDocument.DataSource = UserSession.GridData;
                    gvwUploadDocument.DataBind();
                }
                else
                {
                    UserSession.DisplayMessage(this, "No File To Upload .", MainMasterPage.MessageType.Warning);
                    return;
                }
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }
        #endregion

        

    }
}