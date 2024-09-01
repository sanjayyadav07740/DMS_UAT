using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DMS.BusinessLogic;
using System.IO;
using System.Data.Common;
using Ionic.Zip;
using iTextSharp.text.pdf;
using System.Net;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using System.Drawing.Imaging;

namespace DMS.Shared
{
    public partial class UploadDocuments : System.Web.UI.Page
    {
        #region Private Member
        Utility objUtility = new Utility();
        DMS.BusinessLogic.Document objDocument = new BusinessLogic.Document();

        #endregion

        #region Event

        protected void Page_Load(object sender, EventArgs e)
        {
            ((AjaxControlToolkit.ToolkitScriptManager)Master.FindControl("tsmManager")).RegisterPostBackControl(ibtnSubmit);
            if (!IsPostBack)
            {

                UserSession.GridData = null;
                DataTable objDataTable = new DataTable();
                objDataTable.Columns.Add("ID", typeof(int)).AutoIncrement = true;
                objDataTable.Columns.Add("DocumentName", typeof(string));
                objDataTable.Columns.Add("DocumentGuid", typeof(string));
                objDataTable.Columns.Add("DocumentPath", typeof(string));
                objDataTable.Columns.Add("DocumentType", typeof(string));
                objDataTable.Columns.Add("Size", typeof(int));
                objDataTable.Columns.Add("Image", typeof(byte[]));
                objDataTable.Columns.Add("Tag", typeof(string));
                //objDataTable.Columns.Add("ExpiryDate", typeof(DateTime));
                //objDataTable.Columns.Add("NotificationBefore", typeof(int));
                //objDataTable.Columns.Add("NotificationInterval", typeof(int));
                objDataTable.AcceptChanges();
                UserSession.GridData = objDataTable;
                Log.AuditLog(HttpContext.Current, "Visit", "UplaodDocument");
            }
        }

        protected void ibtnDeleteChecked_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                foreach (GridViewRow objGridViewRow in gvwDocument.Rows)
                {
                    if (((CheckBox)objGridViewRow.FindControl("chkRow")).Checked)
                    {
                        int intRowIndex = objGridViewRow.RowIndex;
                        int intDataKeyValue = Convert.ToInt32(gvwDocument.DataKeys[intRowIndex].Value);

                        UserSession.GridData.Select("ID=" + intDataKeyValue)[0].Delete();
                    }
                }

                UserSession.GridData.AcceptChanges();

                DataView objDataView = new DataView(UserSession.GridData);
                objDataView.Sort = "ID DESC";
                gvwDocument.DataSource = objDataView;
                gvwDocument.DataBind();
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }


        }

        public bool IsSpecialCharacters(string Filename)
        {
            Match match = Regex.Match(Filename, "[^a-z0-9. _]",
            RegexOptions.IgnoreCase);
            while (match.Success)
            {
                return true;
                break;
            }
            return false;
        }

        protected void ibtnSubmit_Click(object sender, ImageClickEventArgs e)
        {

            #region OldCodeTill_11_06_2021

            //string strDocumentName = null;
            //string strDocumentGuid = null;
            //string strDocumentPath = null;
            //string strDocumentType = null;
            //string TagValue = null;
            ////DateTime dtExpiryDate=DateTime.MinValue;

            //if (emodModule.SelectedRepository == 0 || emodModule.SelectedRepository == 1002 && txtTagValue.Text== "")
            //{
            //    UserSession.DisplayMessage(this, "Please Enter 13 Digit Tag Value .", MainMasterPage.MessageType.Warning);
            //    return;
            //}

            //if (emodModule.SelectedRepository == 0 || emodModule.SelectedRepository == 1002)
            //{
            //    TagValue = txtTagValue.Text;
            //}
            //else
            //{
            //    TagValue = Path.GetFileNameWithoutExtension(filUpload.FileName);
            //}

            //txtTagValue.Text = TagValue;

            ////string strNotificationBefore = null;
            ////string strNotificationInterval = null;
            //int intSize = 0;
            //byte[] byteImage = null;

            //try
            //{
            //    //if (Path.GetExtension(filUpload.FileName).ToLower().Trim() == ".jpeg" || Path.GetExtension(filUpload.FileName).ToLower().Trim() == ".jpg")
            //    //{
            //    //    UserSession.DisplayMessage(this, "This format is not allowed.", MainMasterPage.MessageType.Warning);
            //    //}
            //    //all file formats except for pdf and tif are not allowed
            //    if (emodModule.SelectedRepository != 82) //Capital Small Finance Bank
            //    {
            //        if (Path.GetExtension(filUpload.FileName).ToLower().Trim() == ".jpeg" || Path.GetExtension(filUpload.FileName).ToLower().Trim() == ".jpg" || Path.GetExtension(filUpload.FileName).ToLower().Trim() == ".xlsx" || Path.GetExtension(filUpload.FileName).ToLower().Trim() == ".xls" || Path.GetExtension(filUpload.FileName).ToLower().Trim() == ".docx" || Path.GetExtension(filUpload.FileName).ToLower().Trim() == ".doc" || Path.GetExtension(filUpload.FileName).ToLower().Trim() == ".htm" || Path.GetExtension(filUpload.FileName).ToLower().Trim() == ".xml" || Path.GetExtension(filUpload.FileName).ToLower().Trim() == ".exe")
            //        {
            //            UserSession.DisplayMessage(this, "This format is not allowed.", MainMasterPage.MessageType.Warning);
            //            return;
            //        }
            //    }
            //    else
            //    {
            //        if (Path.GetExtension(filUpload.FileName).ToLower().Trim() == ".htm" || Path.GetExtension(filUpload.FileName).ToLower().Trim() == ".xml" || Path.GetExtension(filUpload.FileName).ToLower().Trim() == ".exe")
            //        {
            //            UserSession.DisplayMessage(this, "This format is not allowed.", MainMasterPage.MessageType.Warning);
            //            return;
            //        }
            //    }
            //    //else
            //    {
            //        if (filUpload.PostedFile.ContentLength == 0)
            //        {
            //            UserSession.DisplayMessage(this, "Can Not Add Zero Length Or Damage File .", MainMasterPage.MessageType.Warning);
            //            return;
            //        }
            //        // strDocumentName = filUpload.FileName.Replace(" ", string.Empty);
            //        strDocumentName = filUpload.FileName;
            //        if (Path.GetExtension(filUpload.FileName).ToLower().Trim() != ".zip")
            //        {

            //            if (emodModule.SelectedRepository != 53)
            //            {
            //                bool flag;
            //                flag = IsSpecialCharacters(strDocumentName);
            //                if (flag == true)
            //                {
            //                    UserSession.DisplayMessage(this, "This document contains special characters. Special characters not allowed.", MainMasterPage.MessageType.Warning);
            //                    return;
            //                }
            //                else
            //                {
            //                    int repositoryid = emodModule.SelectedRepository;
            //                    DocumentManager objDocumentManager = new DocumentManager();
            //                    DataSet ds = new DataSet();
            //                    ds = objDocumentManager.CheckDocumentSingleUpload(strDocumentName, repositoryid);
            //                    if (ds.Tables[0].Rows.Count > 0)
            //                    {
            //                        UserSession.DisplayMessage(this, "This document is already uploaded. Kindly rename the document or upload another document.", MainMasterPage.MessageType.Warning);
            //                        return;
            //                    }
            //                    else
            //                    {
            //                        // ds = objDocumentManager.CheckDocument(strDocumentName, numberOfPages, repositoryid);
            //                        string RepName = "";
            //                        DMS.BusinessLogic.RepositoryManager objRepositoryManager = new RepositoryManager();
            //                        DataTable dt = new DataTable();
            //                        objUtility.Result = objRepositoryManager.SelectRepository(out dt, repositoryid);
            //                        switch (objUtility.Result)
            //                        {
            //                            case Utility.ResultType.Failure:
            //                            case Utility.ResultType.Error:

            //                                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
            //                                break;
            //                            case Utility.ResultType.Success:
            //                                RepName = dt.Rows[0]["RepositoryName"].ToString();
            //                                break;
            //                        }
            //                        strDocumentGuid = System.Guid.NewGuid().ToString() + Path.GetExtension(strDocumentName);
            //                        if (!Directory.Exists(Utility.DocumentPath + RepName))
            //                        {
            //                            Directory.CreateDirectory(Utility.DocumentPath + RepName);
            //                        }
            //                        strDocumentPath = Utility.DocumentPath + RepName + @"\" + strDocumentGuid;
            //                        strDocumentType = Path.GetExtension(strDocumentName).ToLower();
            //                        intSize = filUpload.PostedFile.ContentLength;
            //                        byteImage = null;
            //                        byteImage = filUpload.FileBytes;
            //                        //if(txtExpiryDate.Text.Trim()!="")
            //                        //    dtExpiryDate = Convert.ToDateTime(txtExpiryDate.Text.Trim());
            //                        //if (txtNotification.Text.Trim() == "")
            //                        //    strNotificationBefore = "0";
            //                        //else
            //                        //    strNotificationBefore = txtNotification.Text.Trim();
            //                        //if (txtNotificationInterval.Text.Trim() == "")
            //                        //    strNotificationInterval = "0";
            //                        //else
            //                        //    strNotificationInterval = txtNotificationInterval.Text.Trim();
            //                        if (byteImage.Length != 0)
            //                        {
            //                            DataRow objDataRow = UserSession.GridData.NewRow();
            //                            objDataRow["DocumentName"] = strDocumentName;
            //                            objDataRow["DocumentGuid"] = strDocumentGuid;
            //                            objDataRow["DocumentPath"] = strDocumentPath;
            //                            objDataRow["DocumentType"] = strDocumentType;
            //                            objDataRow["Size"] = intSize;
            //                            objDataRow["Image"] = byteImage;
            //                            objDataRow["Tag"] = txtTagValue.Text.Trim();
            //                            //objDataRow["ExpiryDate"] = dtExpiryDate;
            //                            //objDataRow["NotificationBefore"] = strNotificationBefore;
            //                            //objDataRow["NotificationInterval"] = strNotificationInterval;
            //                            UserSession.GridData.Rows.Add(objDataRow);
            //                            UserSession.GridData.AcceptChanges();
            //                        }
            //                        else
            //                        {
            //                            UserSession.DisplayMessage(this, "Can Not Add Zero Length Or Damage File .", MainMasterPage.MessageType.Warning);
            //                            return;
            //                        }
            //                       // txtTagValue.Text = "";
            //                       // txtTagValue.Text = string.Empty;
            //                    }
            //                }
            //            }
            //            else
            //            {

            //                int repositoryid = emodModule.SelectedRepository;
            //                DocumentManager objDocumentManager = new DocumentManager();
            //                DataSet ds = new DataSet();
            //                ds = objDocumentManager.CheckDocumentSingleUpload(strDocumentName, repositoryid);
            //                if (ds.Tables[0].Rows.Count > 0)
            //                {
            //                    UserSession.DisplayMessage(this, "This document is already uploaded. Kindly rename the document or upload another document.", MainMasterPage.MessageType.Warning);
            //                    return;
            //                }
            //                else
            //                {
            //                    // ds = objDocumentManager.CheckDocument(strDocumentName, numberOfPages, repositoryid);
            //                    string RepName = "";
            //                    DMS.BusinessLogic.RepositoryManager objRepositoryManager = new RepositoryManager();
            //                    DataTable dt = new DataTable();
            //                    objUtility.Result = objRepositoryManager.SelectRepository(out dt, repositoryid);
            //                    switch (objUtility.Result)
            //                    {
            //                        case Utility.ResultType.Failure:
            //                        case Utility.ResultType.Error:

            //                            UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
            //                            break;
            //                        case Utility.ResultType.Success:
            //                            RepName = dt.Rows[0]["RepositoryName"].ToString();
            //                            break;
            //                    }
            //                    strDocumentGuid = System.Guid.NewGuid().ToString() + Path.GetExtension(strDocumentName);
            //                    if (!Directory.Exists(Utility.DocumentPath + RepName))
            //                    {
            //                        Directory.CreateDirectory(Utility.DocumentPath + RepName);
            //                    }
            //                    strDocumentPath = Utility.DocumentPath + RepName + @"\" + strDocumentGuid;
            //                    strDocumentType = Path.GetExtension(strDocumentName).ToLower();
            //                    intSize = filUpload.PostedFile.ContentLength;
            //                    byteImage = null;
            //                    byteImage = filUpload.FileBytes;
            //                    //if(txtExpiryDate.Text.Trim()!="")
            //                    //    dtExpiryDate = Convert.ToDateTime(txtExpiryDate.Text.Trim());
            //                    //if (txtNotification.Text.Trim() == "")
            //                    //    strNotificationBefore = "0";
            //                    //else
            //                    //    strNotificationBefore = txtNotification.Text.Trim();
            //                    //if (txtNotificationInterval.Text.Trim() == "")
            //                    //    strNotificationInterval = "0";
            //                    //else
            //                    //    strNotificationInterval = txtNotificationInterval.Text.Trim();
            //                    if (byteImage.Length != 0)
            //                    {
            //                        DataRow objDataRow = UserSession.GridData.NewRow();
            //                        objDataRow["DocumentName"] = strDocumentName;
            //                        objDataRow["DocumentGuid"] = strDocumentGuid;
            //                        objDataRow["DocumentPath"] = strDocumentPath;
            //                        objDataRow["DocumentType"] = strDocumentType;
            //                        objDataRow["Size"] = intSize;
            //                        objDataRow["Image"] = byteImage;
            //                        objDataRow["Tag"] = txtTagValue.Text.Trim();
            //                        //objDataRow["ExpiryDate"] = dtExpiryDate;
            //                        //objDataRow["NotificationBefore"] = strNotificationBefore;
            //                        //objDataRow["NotificationInterval"] = strNotificationInterval;
            //                        UserSession.GridData.Rows.Add(objDataRow);
            //                        UserSession.GridData.AcceptChanges();
            //                    }
            //                    else
            //                    {
            //                        UserSession.DisplayMessage(this, "Can Not Add Zero Length Or Damage File .", MainMasterPage.MessageType.Warning);
            //                        return;
            //                    }
            //                }

            //            }
            //        }
            //        else
            //        {
            //            ZipFile objZipFile = ZipFile.Read(filUpload.PostedFile.InputStream);

            //            foreach (ZipEntry objZipEntry in objZipFile)
            //            {
            //                if (objZipEntry.IsDirectory == false && Path.GetExtension(objZipEntry.FileName).ToLower().Trim() != ".db")
            //                {
            //                    //strDocumentName = Path.GetFileName(objZipEntry.FileName.Replace(" ", string.Empty));
            //                    strDocumentName = Path.GetFileName(objZipEntry.FileName);
            //                    strDocumentGuid = System.Guid.NewGuid().ToString() + Path.GetExtension(strDocumentName);
            //                    string RepName = "";
            //                    DMS.BusinessLogic.RepositoryManager objRepositoryManager = new RepositoryManager();
            //                    DataTable dt = new DataTable();
            //                    objUtility.Result = objRepositoryManager.SelectRepository(out dt, emodModule.SelectedRepository);
            //                    switch (objUtility.Result)
            //                    {
            //                        case Utility.ResultType.Failure:
            //                        case Utility.ResultType.Error:

            //                            UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
            //                            break;
            //                        case Utility.ResultType.Success:
            //                            RepName = dt.Rows[0]["RepositoryName"].ToString();
            //                            break;
            //                    }
            //                    if (!Directory.Exists(Utility.DocumentPath + RepName))
            //                    {
            //                        Directory.CreateDirectory(Utility.DocumentPath + RepName);
            //                    }
            //                    strDocumentPath = Utility.DocumentPath + RepName + @"\" + strDocumentGuid;
            //                    strDocumentType = Path.GetExtension(objZipEntry.FileName).ToLower();
            //                    Stream objStream = objZipEntry.OpenReader();
            //                    intSize = Convert.ToInt32(objStream.Length);
            //                    byteImage = null;
            //                    byteImage = new byte[intSize];
            //                    objStream.Read(byteImage, 0, intSize);
            //                    objStream.Close();
            //                    //if (txtExpiryDate.Text.Trim() != "")
            //                    //    dtExpiryDate = Convert.ToDateTime(txtExpiryDate.Text.Trim());
            //                    //if (txtNotification.Text.Trim() == "")
            //                    //    strNotificationBefore = "0";
            //                    //else
            //                    //    strNotificationBefore = txtNotification.Text.Trim();
            //                    //if (txtNotificationInterval.Text.Trim() == "")
            //                    //    strNotificationInterval = "0";
            //                    //else
            //                    //    strNotificationInterval = txtNotificationInterval.Text.Trim();
            //                    if (byteImage.Length != 0)
            //                    {
            //                        DataRow objDataRow = UserSession.GridData.NewRow();
            //                        objDataRow["DocumentName"] = strDocumentName;
            //                        objDataRow["DocumentGuid"] = strDocumentGuid;
            //                        objDataRow["DocumentPath"] = strDocumentPath;
            //                        objDataRow["DocumentType"] = strDocumentType;
            //                        objDataRow["Size"] = intSize;
            //                        objDataRow["Image"] = byteImage;
            //                        objDataRow["Tag"] = txtTagValue.Text.Trim();
            //                        //objDataRow["ExpiryDate"] = dtExpiryDate;
            //                        //objDataRow["NotificationBefore"] = strNotificationBefore;
            //                        //objDataRow["NotificationInterval"] = strNotificationInterval;
            //                        UserSession.GridData.Rows.Add(objDataRow);
            //                        UserSession.GridData.AcceptChanges();
            //                    }
            //                }
            //            }
            //        }



            //        DataView objDataView = new DataView(UserSession.GridData);
            //        objDataView.Sort = "ID DESC";
            //        gvwDocument.DataSource = objDataView;
            //        gvwDocument.DataBind();
            //    }

            //}
            //catch (Exception ex)
            //{
            //    UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
            //    LogManager.ErrorLog(Utility.LogFilePath, ex);
            //}

            #endregion

            string strDocumentName = null;
            string strDocumentGuid = null;
            string strDocumentPath = null;
            string strDocumentType = null;
            int intSize = 0;
            byte[] byteImage = null;

            try
            {
                HttpFileCollection hfc = Request.Files;
                for (int i = 0; i < hfc.Count; i++)
                {
                    HttpPostedFile Files = hfc[i];
                    if (Files.ContentLength == 0)
                    {
                        UserSession.DisplayMessage(this, "Can Not Add Zero Length Or Damage File .", MainMasterPage.MessageType.Warning);
                        continue;
                    }
                    strDocumentName = Files.FileName.Replace(" ", string.Empty);
                    //DataSet dtset = Document.CheckDocument(strDocumentName, emodModule.SelectedDepartMentName, emodModule.SelectedMetaTemplate);

                    DataSet ds = new DataSet();
                    DocumentManager objDocumentManager = new DocumentManager();
                    ds = objDocumentManager.CheckDocumentVirescent(strDocumentName, emodModule.SelectedRepository, emodModule.SelectedMetaTemplate);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        //UserSession.DisplayMessage(this, " Document Already Exists .", MainMasterPage.MessageType.Warning);
                        //continue;

                        if (Path.GetExtension(Files.FileName).ToLower().Trim() == ".pdf")
                        {
                            strDocumentName = Path.GetFileNameWithoutExtension(strDocumentName);
                            int count = ds.Tables[0].Rows.Count;
                            string date = DateTime.Now.ToString("yyyy-MM-dd HH.mm");
                            strDocumentName = Path.GetFileNameWithoutExtension(strDocumentName) + "_" + date + ".pdf";

                            //strDocumentName = Path.GetFileNameWithoutExtension(strDocumentName) + "_" + count + ".pdf";

                        }
                        else if (Path.GetExtension(Files.FileName).ToLower().Trim() == ".tif" || Path.GetExtension(Files.FileName).ToLower().Trim() == ".tiff")
                        {
                            strDocumentName = Path.GetFileNameWithoutExtension(strDocumentName);
                            int count = ds.Tables[0].Rows.Count;
                            string date = DateTime.Now.ToString("yyyy-MM-dd HH.mm");
                            strDocumentName = Path.GetFileNameWithoutExtension(strDocumentName) + "_" + date + ".tif";

                            //strDocumentName = Path.GetFileNameWithoutExtension(strDocumentName) + "_" + count + ".tif";
                        }
                        else if (Path.GetExtension(Files.FileName).ToLower().Trim() == ".jpg" || Path.GetExtension(Files.FileName).ToLower().Trim() == ".jpeg")
                        {
                            strDocumentName = Path.GetFileNameWithoutExtension(strDocumentName);
                            int count = ds.Tables[0].Rows.Count;
                            string date = DateTime.Now.ToString("yyyy-MM-dd HH.mm");
                            strDocumentName = Path.GetFileNameWithoutExtension(strDocumentName) + "_" + date + ".jpeg";

                            //strDocumentName = Path.GetFileNameWithoutExtension(strDocumentName) + "_" + count + ".jpeg";

                        }
                        else if (Path.GetExtension(Files.FileName).ToLower().Trim() == ".png")
                        {
                            strDocumentName = Path.GetFileNameWithoutExtension(strDocumentName);
                            int count = ds.Tables[0].Rows.Count;
                            string date = DateTime.Now.ToString("yyyy-MM-dd HH.mm");
                            strDocumentName = Path.GetFileNameWithoutExtension(strDocumentName) + "_" + date + ".png";

                            //strDocumentName = Path.GetFileNameWithoutExtension(strDocumentName) + "_" + count + ".png";          
                        }

                    }
                    if (Path.GetExtension(Files.FileName).ToLower().Trim() == ".pdf" || Path.GetExtension(Files.FileName).ToLower().Trim() == ".xlsx" || Path.GetExtension(Files.FileName).ToLower().Trim() == ".xls" || Path.GetExtension(Files.FileName).ToLower().Trim() == ".docx" || Path.GetExtension(Files.FileName).ToLower().Trim() == ".doc" || Path.GetExtension(Files.FileName).ToLower().Trim() == ".ppt" || Path.GetExtension(Files.FileName).ToLower().Trim() == ".pptx" || Path.GetExtension(Files.FileName).ToLower().Trim() == ".tif" || Path.GetExtension(Files.FileName).ToLower().Trim() == ".tiff" ||
                        Path.GetExtension(Files.FileName).ToLower().Trim() == ".jpg" || Path.GetExtension(Files.FileName).ToLower().Trim() == ".jpeg" || Path.GetExtension(Files.FileName).ToLower().Trim() == ".png")
                    {

                        string RepName = "";
                        DMS.BusinessLogic.RepositoryManager objRepositoryManager = new RepositoryManager();
                        DataTable dt = new DataTable();
                        objUtility.Result = objRepositoryManager.SelectRepository(out dt, emodModule.SelectedRepository);
                        switch (objUtility.Result)
                        {
                            case Utility.ResultType.Failure:
                            case Utility.ResultType.Error:

                                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                                break;
                            case Utility.ResultType.Success:
                                RepName = dt.Rows[0]["RepositoryName"].ToString();
                                break;
                        }

                        if (!Directory.Exists(Utility.DocumentPath + RepName))
                        {
                            Directory.CreateDirectory(Utility.DocumentPath + RepName);
                        }

                        strDocumentGuid = System.Guid.NewGuid().ToString() + Path.GetExtension(strDocumentName);
                        strDocumentPath = Utility.DocumentPath + RepName + @"\" + strDocumentGuid;
                        strDocumentType = Path.GetExtension(strDocumentName).ToLower();
                        intSize = Files.ContentLength;
                        byteImage = null;
                        //byteImage = filUpload.FileBytes;
                        Stream stream = Files.InputStream;
                        BinaryReader br = new BinaryReader(stream);
                        byteImage = br.ReadBytes((Int32)stream.Length);
                        txtTagValue.Text = Path.GetFileNameWithoutExtension(strDocumentName);
                        if (byteImage.Length != 0)
                        {
                            DataRow objDataRow = UserSession.GridData.NewRow();
                            objDataRow["DocumentName"] = strDocumentName;
                            objDataRow["DocumentGuid"] = strDocumentGuid;
                            objDataRow["DocumentPath"] = strDocumentPath;
                            objDataRow["DocumentType"] = strDocumentType;
                            objDataRow["Size"] = intSize;
                            objDataRow["Image"] = byteImage;
                            objDataRow["Tag"] = txtTagValue.Text;

                            if (UserSession.GridData.Select("DocumentName = '" + strDocumentName + "'").Length == 0)
                            {
                                UserSession.GridData.Rows.Add(objDataRow);
                                UserSession.GridData.AcceptChanges();
                            }
                        }
                        else
                        {
                            UserSession.DisplayMessage(this, "Can Not Add Zero Length Or Damage File .", MainMasterPage.MessageType.Warning);
                            continue;
                        }
                    }

                    //else if (Path.GetExtension(filUpload.FileName).ToLower().Trim() == ".zip")
                    //{
                    //    ZipFile objZipFile = ZipFile.Read(filUpload.PostedFile.InputStream);

                    //    foreach (ZipEntry objZipEntry in objZipFile)
                    //    {
                    //        if (objZipEntry.IsDirectory == false && Path.GetExtension(objZipEntry.FileName).ToLower().Trim() != ".db")
                    //        {
                    //            strDocumentName = Path.GetFileName(objZipEntry.FileName.Replace(" ", string.Empty));
                    //            strDocumentGuid = System.Guid.NewGuid().ToString() + Path.GetExtension(strDocumentName);
                    //            strDocumentPath = Utility.DocumentPath + strDocumentGuid;
                    //            strDocumentType = Path.GetExtension(objZipEntry.FileName).ToLower();

                    //            Stream objStream = objZipEntry.OpenReader();
                    //            intSize = Convert.ToInt32(objStream.Length);
                    //            byteImage = null;
                    //            byteImage = new byte[intSize];
                    //            objStream.Read(byteImage, 0, intSize);
                    //            objStream.Close();

                    //            if (byteImage.Length != 0)
                    //            {
                    //                DataRow objDataRow = UserSession.GridData.NewRow();
                    //                objDataRow["DocumentName"] = strDocumentName;
                    //                objDataRow["DocumentGuid"] = strDocumentGuid;
                    //                objDataRow["DocumentPath"] = strDocumentPath;
                    //                objDataRow["DocumentType"] = strDocumentType;
                    //                objDataRow["Size"] = intSize;
                    //                objDataRow["Image"] = byteImage;
                    //                objDataRow["Tag"] = txtTagValue.Text.Trim();
                    //                UserSession.GridData.Rows.Add(objDataRow);
                    //                UserSession.GridData.AcceptChanges();
                    //            }
                    //        }
                    //    }
                    //}

                    else
                    {
                        UserSession.DisplayMessage(this, "This File Format is not allow for Uploading .", MainMasterPage.MessageType.Warning);
                        continue;
                    }
                }

                DataView objDataView = new DataView(UserSession.GridData);
                objDataView.Sort = "ID DESC";
                ibtnUpload.Visible = true;
                gvwDocument.DataSource = objDataView;
                gvwDocument.DataBind();
                Session["update"] = Server.UrlEncode(System.DateTime.Now.ToString());
                //}
                //else
                //{
                //    DataView objDataView = new DataView(UserSession.GridData);
                //    objDataView.Sort = "ID DESC";
                //    ibtnUpload.Visible = true;
                //    gvwDocument.DataSource = objDataView;
                //    gvwDocument.DataBind();
                //}
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Something Went Wrong.", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }


        }

        protected void ibtnBack_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                UserSession.GridData = null;
                Response.Redirect("../Shared/NewDashBoard.aspx", false);
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
                if (e.CommandName.ToLower() == "removedocument")
                {
                    int intRowIndex = Convert.ToInt32(e.CommandArgument);
                    int intDataKeyValue = Convert.ToInt32(gvwDocument.DataKeys[intRowIndex].Value);

                    UserSession.GridData.Select("ID=" + intDataKeyValue)[0].Delete();
                    UserSession.GridData.AcceptChanges();

                    DataView objDataView = new DataView(UserSession.GridData);
                    objDataView.Sort = "ID DESC";
                    gvwDocument.DataSource = objDataView;
                    gvwDocument.DataBind();
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
                    DataView objDataView = new DataView(UserSession.GridData);
                    objDataView.Sort = "ID DESC";
                    gvwDocument.PageIndex = e.NewPageIndex;
                    gvwDocument.DataSource = objDataView;
                    gvwDocument.DataBind();
                }
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

        protected void gvwDocument_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvwDocument.EditIndex = e.NewEditIndex;
                if (UserSession.GridData != null)
                {
                    DataView objDataView = new DataView(UserSession.GridData);
                    objDataView.Sort = "ID DESC";
                    gvwDocument.DataSource = objDataView;
                    gvwDocument.DataBind();
                }

            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        protected void gvwDocument_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvwDocument.EditIndex = -1;
                if (UserSession.GridData != null)
                {
                    DataView objDataView = new DataView(UserSession.GridData);
                    objDataView.Sort = "ID DESC";
                    gvwDocument.DataSource = objDataView;
                    gvwDocument.DataBind();
                }

            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        protected void gvwDocument_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int intRowIndex = e.RowIndex;
                int intDataKeyValue = Convert.ToInt32(gvwDocument.DataKeys[intRowIndex].Value);

                UserSession.GridData.Select("ID=" + intDataKeyValue)[0]["Tag"] = ((TextBox)gvwDocument.Rows[intRowIndex].Cells[5].Controls[0]).Text.Trim();
                UserSession.GridData.AcceptChanges();
                gvwDocument.EditIndex = -1;
                DataView objDataView = new DataView(UserSession.GridData);
                objDataView.Sort = "ID DESC";
                gvwDocument.DataSource = objDataView;
                gvwDocument.DataBind();
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        protected void ibtnUpload_Click(object sender, ImageClickEventArgs e)
        {
            #region OldCodeTill_11_06_2021

            //DbTransaction objDbTransaction = Utility.GetTransaction;
            //try
            //{
            //    if (emodModule.SelectedRepository == -1 || emodModule.SelectedMetaTemplate == -1)
            //    {
            //        UserSession.DisplayMessage(this, "Please Select Repository And Metatemplate For Uploading .", MainMasterPage.MessageType.Warning);
            //        return;
            //    }

            //    if (emodModule.SelectedRepository == 82) //Capital small finance
            //    {
            //        DataTable objDBSizeTable = new DataTable();
            //        DocumentManager objDocumentManager = new DocumentManager();

            //        objDocumentManager.FileSizeCount(out objDBSizeTable, emodModule.SelectedRepository);
            //        long DBFileSize = 0, FileSizeCount = 0;
            //        if (objDBSizeTable.Rows[0].IsNull("Size"))
            //        {
            //            DBFileSize = 0;
            //        }
            //        else
            //        {
            //            DBFileSize = Convert.ToInt64(objDBSizeTable.Rows[0]["size"]);
            //        }

            //        foreach (DataRow objDataRow2 in UserSession.GridData.Rows)
            //        {
            //            FileSizeCount += Convert.ToInt64(objDataRow2["size"]);
            //        }
            //        long TotalSize = FileSizeCount + DBFileSize;

            //        long FixedFileSize = 10737418240; //bytes in binary (10 GB);
            //        if (TotalSize >= FixedFileSize)
            //        {
            //            UserSession.DisplayMessage(this, "Insufficient Memory.", MainMasterPage.MessageType.Warning);
            //            return;

            //        }
            //    }

            //    if (UserSession.GridData != null && UserSession.GridData.Rows.Count > 0)
            //    {
            //        if (Utility.FileStorageType == Utility.StorageType.FileSystem)
            //        {
            //            foreach (DataRow objDataRow in UserSession.GridData.Rows)
            //            {
            //                FileStream objFileStream = new FileStream(objDataRow["DocumentPath"].ToString(), FileMode.CreateNew, FileAccess.Write);
            //                objFileStream.Write((byte[])objDataRow["Image"], 0, ((byte[])objDataRow["Image"]).Length);
            //                objFileStream.Close();
            //            }
            //        }

            //        DMS.BusinessLogic.MetaData objMetaData = new BusinessLogic.MetaData();
            //        DocumentManager objDocumentManager = new DocumentManager();
            //        if (emodModule.SelectedMetaDataCode == -1)
            //        {
            //            objMetaData.MetaDataCode = "DMS-000001234";
            //            objMetaData.RepositoryID = emodModule.SelectedRepository;
            //            objMetaData.MetaTemplateID = emodModule.SelectedMetaTemplate;
            //            objMetaData.FolderID = emodModule.SelectedFolder;
            //            objMetaData.CategoryID = emodModule.SelectedCategory;
            //            objMetaData.CreatedBy = UserSession.UserID;
            //            objMetaData.DocumentStatusID = 1;
            //            objMetaData.Status = 1;

            //            objUtility.Result = objDocumentManager.InsertMetaData(objMetaData, objDbTransaction);
            //        }
            //        else
            //        {
            //            objUtility.Result = Utility.ResultType.Success;
            //            objMetaData.MetaDataID = emodModule.SelectedMetaDataCode;
            //            objMetaData.MetaDataCode = ((DropDownList)emodModule.FindControl("ddlMetaDataCode")).SelectedItem.Text.Trim();
            //        }

            //        switch (objUtility.Result)
            //        {
            //            case Utility.ResultType.Success:

            //                foreach (DataRow objDataRow in UserSession.GridData.Rows)
            //                {

            //                    DMS.BusinessLogic.Document objDocument = new BusinessLogic.Document();
            //                    objDocument.MetaDataID = objMetaData.MetaDataID;
            //                    objDocument.DocumentName = objDataRow["DocumentName"].ToString();
            //                    objDocument.DocumentGuid = objDataRow["DocumentGuid"].ToString();
            //                    objDocument.DocumentPath = objDataRow["DocumentPath"].ToString();
            //                    objDocument.DocumentType = objDataRow["DocumentType"].ToString();
            //                    objDocument.Size = Convert.ToInt32((objDataRow["Size"].ToString()));
            //                    if (objDocument.DocumentType == ".dwg")
            //                    {
            //                        objDocument.Size = 0;
            //                    }
            //                    objDocument.Image = (byte[])objDataRow["Image"];
            //                    objDocument.Tag = objDataRow["Tag"].ToString();
            //                    objDocument.IsLucened = 0;
            //                    objDocument.CreatedBy = UserSession.UserID;
            //                    objDocument.DocumentStatusID = (int)DocumentManager.Status.Uploaded;
            //                    objDocument.Status = 1;
            //                    //objDocument.ExpiryDate = Convert.ToDateTime(objDataRow["ExpiryDate"].ToString());
            //                    //objDocument.NotificationBefore = Convert.ToInt16(objDataRow["NotificationBefore"].ToString());
            //                    //objDocument.NotificationInterval = Convert.ToInt16(objDataRow["NotificationInterval"].ToString());
            //                    //check if user selects to ssl
            //                    int repositoryid = 0;
            //                    repositoryid = emodModule.SelectedRepository;
            //                    //check if pdf is numeric and has six numbers'
            //                    if (repositoryid == 47)
            //                    {
            //                        string docname = objDocument.DocumentName.Substring(0, objDocument.DocumentName.IndexOf('.'));
            //                        int NoOfChars = docname.Length;
            //                        if (NoOfChars > 6)
            //                        {
            //                            UserSession.DisplayMessage(this, objDocument.DocumentName + " Cannot be uploaded. It contains more than 6 numbers!", MainMasterPage.MessageType.Error);
            //                            return;
            //                        }
            //                        int errorCounter = Regex.Matches(docname, @"[a-zA-Z]").Count;
            //                        if (errorCounter > 0)
            //                        {
            //                            UserSession.DisplayMessage(this, objDocument.DocumentName + " Cannot be uploaded. It contains characters!", MainMasterPage.MessageType.Error);
            //                            return;
            //                        }
            //                    }
            //                    int numberOfPages = 0;
            //                    //if pdf file can be read and getting the page count of the pdf file
            //                    if (objDocument.DocumentType == ".pdf")
            //                    {
            //                        //int numberOfPages = 0;
            //                        try
            //                        {
            //                            PdfReader pdfReader = new PdfReader(objDocument.DocumentPath);
            //                            numberOfPages = pdfReader.NumberOfPages;
            //                            objDocument.PageCount = numberOfPages;
            //                        }
            //                        catch (Exception ex)
            //                        {
            //                            UserSession.DisplayMessage(this, objDocument.DocumentName + " is Corrupted!", MainMasterPage.MessageType.Error);
            //                            return;
            //                        }
            //                    }
            //                    //if tif file can be read and getting the page count of the tif file
            //                    else if (objDocument.DocumentType == ".tif")
            //                    {

            //                        try
            //                        {
            //                            numberOfPages = FrameCount(objDocument.DocumentPath);
            //                            objDocument.PageCount = numberOfPages;
            //                        }
            //                        catch (Exception ex)
            //                        {
            //                            UserSession.DisplayMessage(this, objDocument.DocumentName + " is Corrupted!", MainMasterPage.MessageType.Error);
            //                            return;
            //                        }
            //                    }
            //                    else
            //                    {
            //                        objDocument.PageCount = 0;
            //                    }
            //                    //check if document with same page count has been uploaded already
            //                    // objDocument.PageCount = 0;

            //                    //DataSet ds = new DataSet();
            //                    //ds = objDocumentManager.CheckDocument(objDocument.DocumentName, numberOfPages, objMetaData.RepositoryID);
            //                    //if (ds.Tables[0].Rows.Count > 0)
            //                    //{
            //                    //    objDbTransaction.Rollback();
            //                    //    UserSession.DisplayMessage(this, objDocument.DocumentName + " is already uploaded!", MainMasterPage.MessageType.Error);
            //                    //    return;
            //                    //}
            //                    //else
            //                    //{
            //                    objUtility.Result = objDocumentManager.InsertDocument(objDocument, objDbTransaction);
            //                    //}
            //                    //}

            //                    string strUrl = Request.Url.Scheme + "://" + Request.Url.Authority + "/Handler/PDFHandler.ashx?DOCID=" + objDocument.DocumentID;

            //                    //callFlo(objDocument.DocumentName, strUrl, Convert.ToInt32(objDataRow["Tag"].ToString() == string.Empty ? "0" : objDataRow["Tag"]));

            //                    switch (objUtility.Result)
            //                    {
            //                        case Utility.ResultType.Failure:
            //                        case Utility.ResultType.Error:
            //                            objDbTransaction.Rollback();
            //                            UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
            //                            break;
            //                        case Utility.ResultType.Success:
            //                            InsertAuditLog(objDocument.DocumentName);
            //                            break;
            //                    }

            //                }
            //                objDbTransaction.Commit();
            //                UserSession.GridData.Rows.Clear();
            //                gvwDocument.DataSource = UserSession.GridData;
            //                gvwDocument.DataBind();
            //                if (emodModule.SelectedMetaDataCode == -1)
            //                {
            //                    UserSession.DisplayMessage(this, "Documents Are Uploaded Successfully With MetaData Code : " + objMetaData.MetaDataCode, MainMasterPage.MessageType.Success);

            //                    objUtility.Result = Utility.LoadMetaDataCode(((DropDownList)emodModule.FindControl("ddlMetaDataCode")), objMetaData);
            //                    if (objUtility.Result == Utility.ResultType.Error)
            //                    {
            //                        UserSession.DisplayMessage(this.Parent.Page, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
            //                    }
            //                }
            //                else
            //                {
            //                    UserSession.DisplayMessage(this, "Documents Are Uploaded Successfully In Existing MetaData Code : " + objMetaData.MetaDataCode, MainMasterPage.MessageType.Success);
            //                }

            //                break;
            //            case Utility.ResultType.Failure:
            //            case Utility.ResultType.Error:
            //                objDbTransaction.Rollback();
            //                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
            //                break;
            //        }

            //    }

            //    else
            //    {
            //        UserSession.DisplayMessage(this, "There Is No Document To Upload .", MainMasterPage.MessageType.Warning);
            //    }

            //}
            //catch (Exception ex)
            //{
            //    objDbTransaction.Rollback();
            //    UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
            //    LogManager.ErrorLog(Utility.LogFilePath, ex);
            //}

            #endregion

            string MetaDataCode = string.Empty;
            DbTransaction objDbTransaction = Utility.GetTransaction;
            List<int> documentid = new List<int>();

            try
            {
                if (emodModule.SelectedRepository == -1 || emodModule.SelectedMetaTemplate == -1)
                {
                    UserSession.DisplayMessage(this, "Please Select Repository And Metatemplate For Uploading .", MainMasterPage.MessageType.Warning);
                    return;
                }

                if (UserSession.GridData != null && UserSession.GridData.Rows.Count > 0)
                {
                    if (Utility.FileStorageType == Utility.StorageType.FileSystem)
                    {
                        foreach (DataRow objDataRow in UserSession.GridData.Rows)
                        {
                            FileStream objFileStream = new FileStream(objDataRow["DocumentPath"].ToString(), FileMode.CreateNew, FileAccess.Write);
                            objFileStream.Write((byte[])objDataRow["Image"], 0, ((byte[])objDataRow["Image"]).Length);
                            objFileStream.Close();
                        }
                    }

                    DMS.BusinessLogic.MetaData objMetaData = new BusinessLogic.MetaData();
                    DocumentManager objDocumentManager = new DocumentManager();
                    if (emodModule.SelectedMetaDataCode == -1)
                    {
                        objMetaData.MetaDataCode = "DMS-000001234";
                        objMetaData.RepositoryID = emodModule.SelectedRepository;
                        objMetaData.MetaTemplateID = emodModule.SelectedMetaTemplate;
                        objMetaData.FolderID = emodModule.SelectedFolder;
                        objMetaData.CategoryID = emodModule.SelectedCategory;
                        objMetaData.CreatedBy = UserSession.UserID;
                        objMetaData.DocumentStatusID = 1;
                        objMetaData.Status = 1;

                        objUtility.Result = objDocumentManager.InsertMetaData(objMetaData, objDbTransaction);
                        objDbTransaction.Commit();
                    }
                    else
                    {
                        objUtility.Result = Utility.ResultType.Success;
                        objMetaData.MetaDataID = emodModule.SelectedMetaDataCode;
                        objMetaData.MetaDataCode = ((DropDownList)emodModule.FindControl("ddlMetaDataCode")).SelectedItem.Text.Trim();
                    }

                    switch (objUtility.Result)
                    {
                        case Utility.ResultType.Success:
                            DbTransaction objDbTransaction1 = Utility.GetTransaction;
                            foreach (DataRow objDataRow in UserSession.GridData.Rows)
                            {

                                //DMS.BusinessLogic.Document objDocument = new BusinessLogic.Document();
                                objDocument.MetaDataID = objMetaData.MetaDataID;
                                objDocument.DocumentName = objDataRow["DocumentName"].ToString();
                                objDocument.DocumentGuid = objDataRow["DocumentGuid"].ToString();
                                objDocument.DocumentPath = objDataRow["DocumentPath"].ToString();
                                objDocument.DocumentType = objDataRow["DocumentType"].ToString();
                                objDocument.Size = Convert.ToInt32((objDataRow["Size"].ToString()));
                                objDocument.Image = (byte[])objDataRow["Image"];
                                ////objDocument.Tag = objDataRow["DocumentName"].ToString().Remove(objDataRow["DocumentName"].ToString().Length - 4, 4);
                                objDocument.Tag = Path.GetFileNameWithoutExtension(objDataRow["DocumentName"].ToString());
                                //objDataRow["Tag"].ToString();
                                objDocument.IsLucened = 0;
                                objDocument.CreatedBy = UserSession.UserID;
                                objDocument.DocumentStatusID = (int)DocumentManager.Status.Uploaded;
                                objDocument.Status = 1;
                                int PageCount = 1;

                                if (objDocument.DocumentType == ".pdf")
                                {
                                    try
                                    {
                                        PdfReader pdfReader = new PdfReader(objDocument.DocumentPath);
                                        // numberOfPages = pdfReader.NumberOfPages;
                                        PageCount = pdfReader.NumberOfPages; ;
                                    }
                                    catch (Exception ex)
                                    {
                                        UserSession.DisplayMessage(this, objDocument.DocumentName + " is Corrupted!", MainMasterPage.MessageType.Error);
                                        return;
                                    }
                                }
                                else if (objDocument.DocumentType == ".tiff" || objDocument.DocumentType == ".tif")
                                {
                                    try
                                    {
                                        PageCount = FrameCount(objDocument.DocumentPath);
                                        objDocument.PageCount = PageCount;
                                    }
                                    catch (Exception ex)
                                    {
                                        UserSession.DisplayMessage(this, objDocument.DocumentName + " is Corrupted!", MainMasterPage.MessageType.Error);
                                        return;
                                    }
                                }
                                else
                                {
                                    PageCount = 1;
                                }

                                objDocument.PageCount = PageCount;

                                objUtility.Result = objDocumentManager.InsertDocument(objDocument, objDbTransaction1);
                                ////documentid.Add(objDocument.DocumentID);
                                switch (objUtility.Result)
                                {
                                    case Utility.ResultType.Failure:
                                    case Utility.ResultType.Error:
                                        objDbTransaction1.Rollback();
                                        UserSession.DisplayMessage(this, "Something Went Wrong.", MainMasterPage.MessageType.Error);
                                        break;
                                    case Utility.ResultType.Success:
                                        documentid.Add(objDocument.DocumentID);
                                        break;
                                }

                            }
                            objDbTransaction1.Commit();
                            UserSession.GridData.Rows.Clear();
                            gvwDocument.DataSource = UserSession.GridData;
                            gvwDocument.DataBind();
                            MetaDataCode = objMetaData.MetaDataCode;

                            foreach (int id in documentid)
                            {
                                InsertAuditLog(objDocument.DocumentName);
                                Log.DocumentAuditLog(HttpContext.Current, "Document Uploaded", "UploadDocuments", id);
                            }
                            if (emodModule.SelectedMetaDataCode == -1)
                            {
                                UserSession.DisplayMessage(this, "Documents Are Uploaded Successfully With MetaData Code : " + MetaDataCode, MainMasterPage.MessageType.Success);

                            }
                            else
                            {
                                UserSession.DisplayMessage(this, "Documents Are Uploaded Successfully In Existing MetaData Code : " + MetaDataCode, MainMasterPage.MessageType.Success);
                            }

                            //UserSession.DisplayMessage(this, CommonFunctions.Errormessage, MainMasterPage.MessageType.Error);
                            break;
                    }

                }
                else
                {
                    UserSession.DisplayMessage(this, "There Is No Document To Upload .", MainMasterPage.MessageType.Warning);
                }

            }
            catch (Exception ex)
            {
                //objDbTransaction.Rollback();
                //UserSession.DisplayMessage(this, "Something went Wrong.", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }

        }

        public int FrameCount(string imageFileName)
        {
            int _PageNumber = 0;
            System.Drawing.Image Tiff = System.Drawing.Image.FromFile(imageFileName);
            _PageNumber = Tiff.GetFrameCount(FrameDimension.Page);
            Tiff.Dispose();
            return _PageNumber;
        }

        public string GetIPAddress(HttpContext context)
        {
            string ipList = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipList))
            {
                return ipList.Split(',')[0];
            }

            return context.Request.ServerVariables["REMOTE_ADDR"];

        }

        public void InsertAuditLog(string DocumentName)
        {
            //insert audit log
            string strHostName = "";
            strHostName = System.Net.Dns.GetHostName();
            IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(strHostName);
            //IPAddress[] addr = ipEntry.AddressList;
            string IPAddress = GetIPAddress(HttpContext.Current);
            string MacAddress = GetMacAddress();
            string Activity = "Document Uploading";
            string DocName = DocumentName;
            int UserId = UserSession.UserID;
            Report objReport = new Report();
            objReport.InsertAuditLog(IPAddress, MacAddress, Activity, DocName, UserId);
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

        //void callFlo(string name, string url, int amount)
        //{
        //    Service.ServiceClient client = new Service.ServiceClient();
        //    Service.Document DocumentInstance = new Service.Document();
        //    DocumentInstance.Created = DateTime.UtcNow;
        //    DocumentInstance.Name = name;
        //    DocumentInstance.Status = 1;
        //    DocumentInstance.Url = url;
        //    DocumentInstance.Amount = amount;
        //    client.Initialize(3, DocumentInstance);
        //    client.Close();
        //}
        #endregion


    }
}