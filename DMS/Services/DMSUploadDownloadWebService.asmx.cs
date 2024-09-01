using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using DMS.BusinessLogic;
using System.Data.Common;
using System.IO;
using System.Data;
using iTextSharp.text.pdf;
using System.Drawing;
using System.Drawing.Imaging;

namespace DMS.Services
{
    /// <summary>
    /// Summary description for DMSUploadDownloadWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.None)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class DMSUploadDownloadWebService : System.Web.Services.WebService
    {
        [WebMethod(MessageName = "Upload File To Server", Description = "Upload File To Server")]
        public bool UploadDocumentToServer(string filePath, byte[] objFileByte, bool boolIsNew, int intContentLength)
        {
            try
            {
                if (boolIsNew == true)
                {
                    FileStream objStreamWriter = new FileStream(filePath, FileMode.Create);
                    objStreamWriter.Write(objFileByte, 0, intContentLength);
                    objStreamWriter.Dispose();
                }
                else
                {
                    FileStream objStreamWriter = new FileStream(filePath, FileMode.Append);
                    objStreamWriter.Write(objFileByte, 0, intContentLength);
                    objStreamWriter.Dispose();
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
                return false;
            }

        }

        [WebMethod(MessageName = "Get Repository Name from MetadataID", Description = "Get Repository Name from MetadataID")]
        public DataSet SelectRepName(int MetadataID)
        {
            try
            {
                DataSet ds = new DataSet();
                ds = DMS.BusinessLogic.MetaData.SelectRepName(MetadataID);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
                return null;
            }
        }

        [WebMethod]
        public byte[] DownloadDocumentFromServer(string filePath)
        {
            byte[] objFileByte = null;
            if (File.Exists(filePath))
            {
                objFileByte = File.ReadAllBytes(filePath);
            }
            else
            {
                objFileByte = new byte[0];
            }
            return objFileByte;
        }
        
        [WebMethod(MessageName = "Check Document", Description = "Check Document")]
        public DataSet CheckDocument(string Docname, int PageCount, int RepId)
        {
            try
            {
                DataSet ds = new DataSet();
                DocumentManager objDocumentManager = new DocumentManager();
                ds = objDocumentManager.CheckDocument(Docname, PageCount, RepId);
                return ds;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [WebMethod(MessageName = "Check DocumentCentrum", Description = "Check Document")]
        public DataSet CheckDocumentCentrum(string Docname, int RepId, int MetId, int CatId, int FolId)
        {
            try
            {
                DataSet ds = new DataSet();
                DocumentManager objDocumentManager = new DocumentManager();
                ds = objDocumentManager.CheckDocument_Centrum(Docname, RepId, MetId, CatId, FolId);
                //CheckDocument(Docname, PageCount, RepId);
                return ds;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [WebMethod(MessageName = "Select Folder", Description = "Select Folder")]
        public DataTable selectFolderCentrum2(string FolderName, int MetatemplateID, int ParentFolderID)
        {
            DataTable dt = new DataTable();
            try
            {


                Utility objUtility = new Utility();
                FolderManager objFolderManager = new FolderManager();
                dt = FolderManager.SelectFolderCentrum2(FolderName, MetatemplateID, ParentFolderID);
                dt.TableName = "FolderList";
                return dt;
            }
            catch (Exception ex)
            {
                dt = null;
                return dt;
                //objDbTransaction.Rollback();
                //LogManager.ErrorLog(Utility.LogFilePath, ex);
                //return Utility.ResultType.Error;
            }
            //if (objUtility.Result == Utility.ResultType.Success)
            //{
            //    return dt;
            //}
            //else
            //{
            //    return null;
            //}
            //}
            //catch (Exception ex)
            //{
            //    return null;
            //}
        }

        [WebMethod(MessageName = "Select Folder Centrum", Description = "Select Folder Centrum")]
        public DataTable selectFolderCentrum(string FolderName, int MetatemplateID, int ParentFolder, int CategoryID)
        {
            DataTable dt = new DataTable();
            try
            {
                //DataTable dt;

                Utility objUtility = new Utility();
                FolderManager objFolderManager = new FolderManager();
                dt = FolderManager.SelectFolderCentrum(FolderName, MetatemplateID, ParentFolder, CategoryID);
                dt.TableName = "FolderList2";
                // SelectFolder(out dt, objFolder.FolderName, objFolder.MetaTemplateID, objFolder.ParentFolderID);
                return dt;
            }
            catch (Exception ex)
            {
                dt = null;
                return dt;
            }
        }
        [WebMethod(MessageName = "Get Pagecount of image", Description = "Get Pagecount of image")]
        public int FrameCount(string imageFileName)
        {
            int _PageNumber = 0;
            Image Tiff = Image.FromFile(imageFileName);
            _PageNumber = Tiff.GetFrameCount(FrameDimension.Page);
            Tiff.Dispose();
            return _PageNumber;
        }
         [WebMethod(MessageName = "Upload File To Server With Detail using SFTP", Description = "Upload File To Server With Detail using SFTP")]
        public bool UploadDocumentToServerSFTP(Document objDocument)
        {
             try
             {
                 DbTransaction objDbTransaction = Utility.GetTransaction;
                 Utility objUtility = new Utility();
                 DocumentManager objDocumentManager = new DocumentManager();
                 objUtility.Result = objDocumentManager.InsertDocument(objDocument, objDbTransaction);
                 if (objUtility.Result == Utility.ResultType.Success)
                 {
                     objDbTransaction.Commit();
                     return true;
                 }
                 else
                 {
                     objDbTransaction.Rollback();
                     return false;
                 }
             }
             catch (Exception ex)
             {
                 return false;
             }
        }

        [WebMethod(MessageName = "Upload File To Server With Detail", Description = "Upload File To Server With Detail")]
        //public bool UploadDocumentToServer(byte[] objFileByte, Document objDocument)

        public bool UploadDocumentToServer(byte[] objFileByte, Document objDocument)
        {
            try
            {
              
                // objDocument.DocumentGuid = Utility.GetUniqueFileName(Path.GetExtension(objDocument.DocumentName));
                //objDocument.DocumentPath = Utility.DocumentPath + objDocument.DocumentGuid;
                objDocument.DocumentType = Path.GetExtension(objDocument.DocumentName);
                objDocument.Size = Convert.ToInt32(objFileByte.Length);
                string FilePath = objDocument.DocumentPath;
                Path.Combine(objDocument.DocumentPath, objDocument.DocumentGuid);
                FileStream objStream = File.Create(FilePath);
                objStream.Write(objFileByte, 0, objFileByte.Length);
                objStream.Close();
                objStream.Dispose();
                if (objDocument.DocumentType.ToUpper() == ".PDF")
                {
                    PdfReader reader = new PdfReader(FilePath);
                    objDocument.PageCount = reader.NumberOfPages;
                }
                else
                {
                    objDocument.PageCount = FrameCount(FilePath);
                }
                DbTransaction objDbTransaction = Utility.GetTransaction;
                Utility objUtility = new Utility();
                DocumentManager objDocumentManager = new DocumentManager();
                //DataSet ds = new DataSet();
                //ds = objDocumentManager.CheckDocument(objDocument.DocumentName);
                //if (ds.Tables[0].Rows.Count > 0)
                //{
                //    return false;
                //}
                //else
                //{
                objUtility.Result = objDocumentManager.InsertDocument(objDocument, objDbTransaction);
                if (objUtility.Result == Utility.ResultType.Success)
                {
                    objDbTransaction.Commit();
                    return true;
                }
                else
                {
                    objDbTransaction.Rollback();
                    return false;
                }
                //}
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        //{
        //    try
        //    {
        //       // objDocument.DocumentGuid = Utility.GetUniqueFileName(Path.GetExtension(objDocument.DocumentName));
        //        //objDocument.DocumentPath = Utility.DocumentPath + objDocument.DocumentGuid;
        //        objDocument.DocumentType = Path.GetExtension(objDocument.DocumentName);
        //        objDocument.Size = Convert.ToInt32(objFileByte.Length);
        //        string FilePath = objDocument.DocumentPath;
        //        Path.Combine(objDocument.DocumentPath, objDocument.DocumentGuid);
        //        FileStream objStream = File.Create(FilePath);
        //        objStream.Write(objFileByte, 0, objFileByte.Length);
        //        objStream.Close();
        //        objStream.Dispose();
        //        if (objDocument.DocumentType.ToUpper() == ".PDF")
        //        {
        //            PdfReader reader = new PdfReader(FilePath);
        //            objDocument.PageCount = reader.NumberOfPages;
        //        }
        //        else
        //        {
        //            objDocument.PageCount = FrameCount(FilePath);
        //        }
        //        DbTransaction objDbTransaction = Utility.GetTransaction;
        //        Utility objUtility = new Utility();
        //        DocumentManager objDocumentManager = new DocumentManager();
        //        //DataSet ds = new DataSet();
        //        //ds = objDocumentManager.CheckDocument(objDocument.DocumentName);
        //        //if (ds.Tables[0].Rows.Count > 0)
        //        //{
        //        //    return false;
        //        //}
        //        //else
        //        //{
        //        objUtility.Result = objDocumentManager.InsertDocument(objDocument, objDbTransaction);
        //        if (objUtility.Result == Utility.ResultType.Success)
        //        {
        //            objDbTransaction.Commit();
        //            return true;
        //        }
        //        else
        //        {
        //            objDbTransaction.Rollback();
        //            return false;
        //        }
        //        //}
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}


        [WebMethod(MessageName = "Upload File To Server With Detail For Centrum", Description = "Upload File To Server With Detail")]
        public bool UploadDocumentToServerCentrum(byte[] objFileByte, Document objDocument)
        {
            try
            {
               // objDocument.DocumentGuid = Utility.GetUniqueFileName(Path.GetExtension(objDocument.DocumentName));
                objDocument.DocumentPath = objDocument.DocumentPath + objDocument.DocumentGuid;
                objDocument.DocumentType = Path.GetExtension(objDocument.DocumentName);
                objDocument.Size = Convert.ToInt32(objFileByte.Length);
                string FilePath = objDocument.DocumentPath;
               
                //FileStream objStream = File.Create(FilePath);
                //objStream.Write(objFileByte, 0, objFileByte.Length);
                //objStream.Close();
                //objStream.Dispose();
                if (objDocument.DocumentType.ToUpper() == ".PDF")
                {
                    PdfReader reader = new PdfReader(FilePath);
                    objDocument.PageCount = reader.NumberOfPages;
                }
                else
                {
                    objDocument.PageCount = FrameCount(FilePath);
                }
                
                objDocument.DocumentPath = FilePath;
                DbTransaction objDbTransaction = Utility.GetTransaction;
                Utility objUtility = new Utility();
                DocumentManager objDocumentManager = new DocumentManager();
                //DataSet ds = new DataSet();
                //ds = objDocumentManager.CheckDocument(objDocument.DocumentName);
                //if (ds.Tables[0].Rows.Count > 0)
                //{
                //    return false;
                //}
                //else
                //{
                objUtility.Result = objDocumentManager.InsertDocument(objDocument, objDbTransaction);
                if (objUtility.Result == Utility.ResultType.Success)
                {
                    objDbTransaction.Commit();
                    return true;
                }
                else
                {
                    objDbTransaction.Rollback();
                    return false;
                }
                //}
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        [WebMethod]
        public string GetUniqueFileName(string strExtension)
        {
            return Utility.GetUniqueFileName(strExtension);
        }

        [WebMethod]
        public int InsertMetaData(DMS.BusinessLogic.MetaData objMetaData)
        {
            DbTransaction objDbTransaction = Utility.GetTransaction;
            Utility objUtility = new Utility();
            DocumentManager objDocumentManager = new DocumentManager();

            objUtility.Result = objDocumentManager.InsertMetaData(objMetaData, objDbTransaction);
            if (objUtility.Result == Utility.ResultType.Success)
            {
                objDbTransaction.Commit();
                return objMetaData.MetaDataID;
            }
            else
            {
                objDbTransaction.Rollback();
                return 0;
            }
        }
       
        [WebMethod]
        public bool InsertDocument(DMS.BusinessLogic.Document objDocument)
        {
            DbTransaction objDbTransaction = Utility.GetTransaction;
            Utility objUtility = new Utility();
            DocumentManager objDocumentManager = new DocumentManager();
            //if (objDocument.DocumentType.ToUpper() == ".PDF")
            //{
            //    PdfReader reader = new PdfReader(objDocument.DocumentPath);
            //    objDocument.PageCount = reader.NumberOfPages;
            //}
            //else if(objDocument.PageCount==0)
            //{
            //    objDocument.PageCount = FrameCount(objDocument.DocumentPath);
            //}
            objUtility.Result = objDocumentManager.InsertDocument(objDocument, objDbTransaction);
            if (objUtility.Result == Utility.ResultType.Success)
            {
                objDbTransaction.Commit();
                return true;
            }
            else
            {
                objDbTransaction.Rollback();
                return false;
            }
        }

        //[WebMethod]
        //public bool InsertErrorDocument(string DocumentName, string ErrorType)
        //{
        //    DbTransaction objDbTransaction = Utility.GetTransaction;
        //    Utility objUtility = new Utility();
        //    DocumentManager objDocumentManager = new DocumentManager();
        //    //if (objDocument.DocumentType.ToUpper() == ".PDF")
        //    //{
        //    //    PdfReader reader = new PdfReader(objDocument.DocumentPath);
        //    //    objDocument.PageCount = reader.NumberOfPages;
        //    //}
        //    //else if(objDocument.PageCount==0)
        //    //{
        //    //    objDocument.PageCount = FrameCount(objDocument.DocumentPath);
        //    //}
        //    objUtility.Result = objDocumentManager.InsertErrorDoc(DocumentName, ErrorType, objDbTransaction);
        //    if (objUtility.Result == Utility.ResultType.Success)
        //    {
        //        objDbTransaction.Commit();
        //        return true;
        //    }
        //    else
        //    {
        //        objDbTransaction.Rollback();
        //        return false;
        //    }
        //}

        [WebMethod(MessageName = "select error documents", Description = "select error documents")]
        public DataTable SelectErrorDocs(string ErrorType)
        {
            try
            {
                DataTable dt = new DataTable();
                DocumentManager objDocumentManager = new DocumentManager();
                dt = objDocumentManager.SelectErrorDocs(ErrorType);
                //CheckDocument(Docname, PageCount, RepId);
                dt.TableName = "DATA";
                
                return dt;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

       

        [WebMethod]
        public int InsertFolder(DMS.BusinessLogic.Folder objFolder)
        {
            DbTransaction objDbTransaction = Utility.GetTransaction;
            Utility objUtility = new Utility();
            //DocumentManager objDocumentManager = new DocumentManager();
            FolderManager objFolderManager = new FolderManager();
            //PdfReader reader = new PdfReader(objDocument.DocumentPath);
            //objDocument.PageCount = reader.NumberOfPages;
            // objUtility.Result = objDocumentManager.InsertDocument(objDocument, objDbTransaction);
            objUtility.Result = FolderManager.InsertFolder(objFolder);
            if (objUtility.Result == Utility.ResultType.Success)
            {
                objDbTransaction.Commit();
                return objFolder.FolderID;
            }
            else
            {
                objDbTransaction.Rollback();
                return 0;
            }
        }

        [WebMethod]
        public int InsertFolderAxisTrustee(DMS.BusinessLogic.Folder objFolder)
        {
            DbTransaction objDbTransaction = Utility.GetTransaction;
            Utility objUtility = new Utility();
            //DocumentManager objDocumentManager = new DocumentManager();
            FolderManager objFolderManager = new FolderManager();
            //PdfReader reader = new PdfReader(objDocument.DocumentPath);
            //objDocument.PageCount = reader.NumberOfPages;
            // objUtility.Result = objDocumentManager.InsertDocument(objDocument, objDbTransaction);
            objUtility.Result = FolderManager.InsertFolderAxisTrustee(objFolder);
            if (objUtility.Result == Utility.ResultType.Success)
            {
                objDbTransaction.Commit();
                return objFolder.FolderID;
            }
            else
            {
                objDbTransaction.Rollback();
                return 0;
            }
        }




        [WebMethod]
        public int InsertFolderCentrum(DMS.BusinessLogic.Folder objFolder)
        {
            DbTransaction objDbTransaction = Utility.GetTransaction;
            Utility objUtility = new Utility();
            //DocumentManager objDocumentManager = new DocumentManager();
            FolderManager objFolderManager = new FolderManager();
            //PdfReader reader = new PdfReader(objDocument.DocumentPath);
            //objDocument.PageCount = reader.NumberOfPages;
            // objUtility.Result = objDocumentManager.InsertDocument(objDocument, objDbTransaction);
            objUtility.Result = FolderManager.InsertFolderCentrum(objFolder);
            if (objUtility.Result == Utility.ResultType.Success)
            {
                objDbTransaction.Commit();
                return objFolder.FolderID;
            }
            else
            {
                objDbTransaction.Rollback();
                return 0;
            }
        }

        [WebMethod]
        public DataTable LoadRepository()
        {
            DataTable objDataTable = DataHelper.ExecuteDataTable("SELECT ID,RepositoryName FROM REPOSITORY WHERE STATUS = 1 ORDER BY RepositoryName", null);
            objDataTable.TableName = "DATA";
            return objDataTable;
        }

        [WebMethod]
        public DataTable LoadMetaTemplate(int intRepositoryID)
        {
            DataTable objDataTable = DataHelper.ExecuteDataTable("SELECT ID,MetaTemplateName FROM METATEMPLATE WHERE RepositoryID = " + intRepositoryID + " AND STATUS = 1 ORDER BY MetaTemplateName", null);
            objDataTable.TableName = "DATA";
            return objDataTable;
        }

        [WebMethod]
        public DataTable LoadCategory(int intMetaTemplateID)
        {
            DataTable objDataTable = DataHelper.ExecuteDataTable("SELECT ID,CategoryName FROM CATEGORY WHERE MetaTemplateID = " + intMetaTemplateID + " AND STATUS = 1 ORDER BY CategoryName", null);
            objDataTable.TableName = "DATA";
            return objDataTable;
        }

        [WebMethod]
        public DataTable LoadFolder(int intMetaTemplateID, int intCategoryID)
        {
            DataTable objDataTable = new DataTable();
            if (intCategoryID == -1)
            {
                objDataTable = DataHelper.ExecuteDataTable("SELECT * FROM FOLDER WHERE MetaTemplateID =" + intMetaTemplateID + "  AND Status=1", null);
            }
            else
            {
                objDataTable = DataHelper.ExecuteDataTable("SELECT * FROM FOLDER WHERE MetaTemplateID =" + intMetaTemplateID + " AND CategoryID=" + intCategoryID + " AND Status=1", null);
            }

            objDataTable.TableName = "DATA";
            return objDataTable;
        }

        [WebMethod]
        public DataTable CheckParentFolder(int MetatemplateID, string FolderName)
        {
            DataTable objDataTable = DataHelper.ExecuteDataTable("select * from Folder where MetaTemplateID=" + MetatemplateID + " and Parentfolderid=0 and FolderName='" + FolderName + "'", null);
            objDataTable.TableName = "DATA";
            return objDataTable;
        }

        [WebMethod]
        public DataTable CheckChildFolder(int MetatemplateID, int ParentFolderID, string FolderName)
        {
            DataTable objDataTable = DataHelper.ExecuteDataTable("select * from Folder where MetaTemplateID=" + MetatemplateID + " and Parentfolderid=" + ParentFolderID + "and FolderName='" + FolderName + "'", null);
            objDataTable.TableName = "DATA";
            return objDataTable;
        }

        [WebMethod]
        public DataTable LoadFolderAxisTrustee(int intMetaTemplateID, int intCategoryID)
        {
            DataTable objDataTable = DataHelper.ExecuteDataTable("SELECT * FROM FOLDER WHERE MetaTemplateID =" + intMetaTemplateID + " AND CategoryID=" + intCategoryID + " AND Status=1", null);
            objDataTable.TableName = "DATA";
            return objDataTable;
        }

        [WebMethod]
        public DataTable LoadMetaDataCode(DMS.BusinessLogic.MetaData objMetaData)
        {
            DataTable objDataTable = new DataTable();
            DocumentManager objDocumentManager = new DocumentManager();
            objDocumentManager.SelectMetaDataForDropDown(out objDataTable, objMetaData);
            objDataTable.TableName = "DATA";
            return objDataTable;
        }

        [WebMethod]
        public DataTable LoadDocument(string query)
        {
            DataTable objDataTable = DataHelper.ExecuteDataTable(query, null);
            objDataTable.TableName = "DATA";
            return objDataTable;
        }

        [WebMethod]
        public DataTable AuthenticateUser(string userName, string password)
        {
            string query = "SELECT * FROM vwuser WHERE UserName='" + userName + "' AND Password='" + password + "'";
            DataTable objDataTable = DataHelper.ExecuteDataTable(query, null);
            objDataTable.TableName = "DATA";
            return objDataTable;
        }

        [WebMethod]
        public void CreateFolderIfNeeded(string filename)
        {
            string folder = System.IO.Path.GetDirectoryName(filename);
            if (!System.IO.Directory.Exists(folder))
            {
                System.IO.Directory.CreateDirectory(folder);
            }
        }

        //Added for Axis trustee
        [WebMethod(MessageName = "Upload Files of AxisTrustee To Server With Detail", Description = "Upload File of AxisTrustee To Server With Detail")]
        public bool UploadDocumentToServer_AxisTrustee(byte[] objFileByte, Document objDocument)
        {
            try
            {
                objDocument.DocumentGuid = Utility.GetUniqueFileName(Path.GetExtension(objDocument.DocumentName));
                objDocument.DocumentPath = Utility.DMSDocumentAxisTrusteePath + objDocument.DocumentGuid;
                objDocument.DocumentType = Path.GetExtension(objDocument.DocumentName);
                objDocument.Size = Convert.ToInt32(objFileByte.Length);
                try
                {
                    string PhysicalPath = Utility.DMSDocumentAxisTrusteePath.Remove(Utility.DMSDocumentAxisTrusteePath.Length - 1);
                    //Added by Vivek 21-11 If directory not Exists
                    if (Directory.Exists(PhysicalPath))
                    {

                    }
                    else
                    {
                        DirectoryInfo di = Directory.CreateDirectory(PhysicalPath);
                    }
                }
                catch (Exception ex)
                {
                }
                FileStream objStream = File.Create(objDocument.DocumentPath);
                objStream.Write(objFileByte, 0, objFileByte.Length);
                objStream.Close();
                objStream.Dispose();

                PdfReader reader = new PdfReader(objDocument.DocumentPath);
                objDocument.PageCount = reader.NumberOfPages;

                DbTransaction objDbTransaction = Utility.GetTransaction;
                Utility objUtility = new Utility();
                DocumentManager objDocumentManager = new DocumentManager();

                //DataSet ds = new DataSet();
                //ds = objDocumentManager.CheckDocument(objDocument.DocumentName);
                //if (ds.Tables[0].Rows.Count > 0)
                //{
                //    return false;
                //}
                //else
                //{
                objUtility.Result = objDocumentManager.InsertDocument(objDocument, objDbTransaction);
                if (objUtility.Result == Utility.ResultType.Success)
                {
                    objDbTransaction.Commit();
                    return true;
                }
                else
                {
                    objDbTransaction.Rollback();
                    return false;
                }
                //}
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        //Added for Axis trustee
        [WebMethod(MessageName = "Upload Folders To Server With files&Detail", Description = "Upload Folders To Server With Detail")]
        public bool UploadDocumentToServer_FolderWise(byte[] objFileByte, Document objDocument)
        {
            try
            {
               // objDocument.DocumentGuid = Utility.GetUniqueFileName(Path.GetExtension(objDocument.DocumentName));
               // objDocument.DocumentPath = Utility.DMSDocumentGetFolderPath + objDocument.DocumentGuid;
                objDocument.DocumentType = Path.GetExtension(objDocument.DocumentName);
                objDocument.Size = Convert.ToInt32(objFileByte.Length);
                //try
                //{
                //    string PhysicalPath = Utility.DMSDocumentGetFolderPath.Remove(Utility.DMSDocumentGetFolderPath.Length - 1);
                //    //Added by Vivek 21-11 If directory not Exists
                //    if (Directory.Exists(PhysicalPath))
                //    {

                //    }
                //    else
                //    {
                //        DirectoryInfo di = Directory.CreateDirectory(PhysicalPath);
                //    }
                //}
                //catch (Exception ex)
                //{
                //}
                FileStream objStream = File.Create(objDocument.DocumentPath);
                objStream.Write(objFileByte, 0, objFileByte.Length);
                objStream.Close();
                objStream.Dispose();

                PdfReader reader = new PdfReader(objDocument.DocumentPath);
                objDocument.PageCount = reader.NumberOfPages;

                DbTransaction objDbTransaction = Utility.GetTransaction;
                Utility objUtility = new Utility();
                DocumentManager objDocumentManager = new DocumentManager();

                //DataSet ds = new DataSet();
                //ds = objDocumentManager.CheckDocument(objDocument.DocumentName);
                //if (ds.Tables[0].Rows.Count > 0)
                //{
                //    return false;
                //}
                //else
                //{
                objUtility.Result = objDocumentManager.InsertDocument(objDocument, objDbTransaction);
                if (objUtility.Result == Utility.ResultType.Success)
                {
                    objDbTransaction.Commit();
                    return true;
                }
                else
                {
                    objDbTransaction.Rollback();
                    return false;
                }
                //}
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        [WebMethod]
        public int InsertFolderForOthers(DMS.BusinessLogic.Folder objFolder)
        {
            DbTransaction objDbTransaction = Utility.GetTransaction;
            Utility objUtility = new Utility();
            //DocumentManager objDocumentManager = new DocumentManager();
            FolderManager objFolderManager = new FolderManager();
            //PdfReader reader = new PdfReader(objDocument.DocumentPath);
            //objDocument.PageCount = reader.NumberOfPages;
            // objUtility.Result = objDocumentManager.InsertDocument(objDocument, objDbTransaction);
            objUtility.Result = FolderManager.InsertFolderForAll(objFolder);
            if (objUtility.Result == Utility.ResultType.Success)
            {
                objDbTransaction.Commit();
                return objFolder.FolderID;
            }
            else
            {
                objDbTransaction.Rollback();
                return 0;
            }
        }

        #region Seema 7Dec 2017 
        // Reliance constraint for filename,repository,metatemplate,category
        [WebMethod(MessageName = "Check Document_Reliance", Description = "Check Document_Reliance")]
        public DataSet CheckDocument_Reliance(string Docname, int RepId,int MetId,int CatId)
        {
            try
            {
                DataSet ds = new DataSet();
                DocumentManager objDocumentManager = new DocumentManager();
                ds = objDocumentManager.CheckDocument_Reliance(Docname, RepId, MetId, CatId);
                return ds;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region Seema 16 July 2018
        // SBI Mutual Fund Trustee Company Private limited constraint for filename,repository,metatemplate,category
        [WebMethod(MessageName = "Check Document_SBI Mutual Fund Trustee Company Private limited", Description = "Check Document_SBI Mutual Fund Trustee Company Private limited")]
        public DataSet CheckDocument_SBIMutual(string Docname, int RepId, int MetId, int Folderid)
        {
            try
            {
                DataSet ds = new DataSet();
                DocumentManager objDocumentManager = new DocumentManager();
                ds = objDocumentManager.CheckDocument_SBIMutual(Docname, RepId, MetId, Folderid);
                return ds;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion
        #region Sneha 08/09/2018
        // IDBI Bank Ltd constraint for filename,metatemplate
        [WebMethod(MessageName = "Check IDBI Bank Ltd", Description = "Check IDBI Bank Ltd")]
        public DataSet CheckDocument_IDBI(string Docname,int MetId,int RepId)
        {
            try
            {
                DataSet ds = new DataSet();
                DocumentManager objDocumentManager = new DocumentManager();
                ds = DocumentManager.CheckDocument_IDBI(Docname, MetId, RepId);
                return ds;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region Sneha 16/01/2019
        // currupted filename entry in datavbase table Doc_Error_IDBI_Ahm
        [WebMethod(MessageName = "CorruptFile or duplicate file catch entry IDBI Ahmedabad", Description = "CorruptFile or duplicate file catch entry IDBI Ahmedabad")]
        public void ErrorDocument_IDBIAhm(string MetatemplateName,string DocName,string ErrorType)
        {
            try
            {
                DbTransaction objDbTransaction = Utility.GetTransaction;
                DataSet ds = new DataSet();
                Utility objUtility = new Utility();
                DocumentManager objDocumentManager = new DocumentManager();
                objUtility.Result = objDocumentManager.InsertErrorDoc(MetatemplateName, DocName, ErrorType, objDbTransaction);
                if (objUtility.Result == Utility.ResultType.Success)
                {
                    objDbTransaction.Commit();
                   
                }
                else
                {
                    objDbTransaction.Rollback();
                    
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Sneha 17/01/2019
        //get docid and metadataid for the document
        [WebMethod(MessageName = "get docid and metadataid", Description = "get docid and metadataid")]
        public DataTable SelectDocumentID(string DocName, int RepId)
        {
            try
            {
                 DataTable dt=new DataTable();
                DbTransaction objDbTransaction = Utility.GetTransaction;
                DataSet ds = new DataSet();
                Utility objUtility = new Utility();
                DocumentManager objDocumentManager = new DocumentManager();
                dt = DocumentManager.SelectDocumentID(DocName, RepId);
                dt.TableName = "DocList"; 
                return dt;
                    
            }
            catch (Exception ex)
            {
                return null;
            
            }
        }
        // excel entry in datavbase table Doc_Error_IDBI_Ahm
        [WebMethod(MessageName = "excel entry IDBI Ahmedabad", Description = "excel entry IDBI Ahmedabad")]
        public void ExcelEntry_IDBIAhm(int DocumentId, int MetadataId, DateTime DateOfRecievalAtRpu, DateTime DateOfAccOpening, string Custid, string AccNo, string BoxNo, string DocNo, string SHIL_Barcode, string SHCIL_Barcode_Date, string CloseFlag,string ExcelName)
        {
            try
            {
                DbTransaction objDbTransaction = Utility.GetTransaction;
                DataSet ds = new DataSet();
                Utility objUtility = new Utility();
                DocumentManager objDocumentManager = new DocumentManager();
                objUtility.Result = objDocumentManager.InsertExcelEntry_IDBI_Ahm(DocumentId, MetadataId, DateOfRecievalAtRpu, DateOfAccOpening, Custid, AccNo, BoxNo, DocNo, SHIL_Barcode, SHCIL_Barcode_Date, CloseFlag, ExcelName,objDbTransaction);
                if (objUtility.Result == Utility.ResultType.Success)
                {
                    objDbTransaction.Commit();

                }
                else
                {
                    objDbTransaction.Rollback();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Sneha 17/01/2019
        [WebMethod(MessageName = "check if docid is already present in excelentry", Description = "check if docid is already present in excelentry")]
        public DataTable SelectDocIdInExcel(int DocId)
        {
            try
            {
                DataTable dt = new DataTable();
                DbTransaction objDbTransaction = Utility.GetTransaction;
                DataSet ds = new DataSet();
                Utility objUtility = new Utility();
                DocumentManager objDocumentManager = new DocumentManager();
                dt = DocumentManager.ExistsInExcelEntry(DocId);
                dt.TableName = "EXCELENTRY";
                return dt;

            }
            catch (Exception ex)
            {
                return null;

            }
        }

        [WebMethod(MessageName = "duplicate docid insert in excelentry error table", Description = "duplicate docid insert in excelentry error table")]
        public void InsertInExcelEntry_Error_IDBI_Ahm(int DocumentId, int MetadataId,string ErrorName, DateTime DateOfRecievalAtRpu, DateTime DateOfAccOpening, string Custid, string AccNo, string BoxNo, string DocNo, string SHIL_Barcode, string SHCIL_Barcode_Date, string CloseFlag,string ExcelName)
        {
            try
            {
                DbTransaction objDbTransaction = Utility.GetTransaction;
                DataSet ds = new DataSet();
                Utility objUtility = new Utility();
                DocumentManager objDocumentManager = new DocumentManager();
                objUtility.Result = DocumentManager.InsertExcelEntryError_IDBI_Ahm(DocumentId, MetadataId,ErrorName, DateOfRecievalAtRpu, DateOfAccOpening, Custid, AccNo, BoxNo, DocNo, SHIL_Barcode, SHCIL_Barcode_Date, CloseFlag, ExcelName, objDbTransaction);
                    
                if (objUtility.Result == Utility.ResultType.Success)
                {
                    objDbTransaction.Commit();
                   
                }
                else
                {
                    objDbTransaction.Rollback();
                    
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        #endregion

        #region Sneha 25 Jan 2019
        [WebMethod(MessageName = "Get erroneous docs list", Description = "Get erroneous docs list")]
        public DataTable GetErrorDocs()
        {
            try
            {
                DataTable dt = new DataTable();
                DbTransaction objDbTransaction = Utility.GetTransaction;
                DataSet ds = new DataSet();
                Utility objUtility = new Utility();
                DocumentManager objDocumentManager = new DocumentManager();
                dt = DocumentManager.GetErrorDocs();
                dt.TableName = "ErrorDocs";
                return dt;

            }
            catch (Exception ex)
            {
                return null;

            }
        }

        [WebMethod(MessageName = "Get missing Entry Docs List", Description = "Get missing Entry Docs List")]
        public DataTable GetDocEntryStatus()
        {
            try
            {
                DataTable dt = new DataTable();
                DbTransaction objDbTransaction = Utility.GetTransaction;
                DataSet ds = new DataSet();
                Utility objUtility = new Utility();
                DocumentManager objDocumentManager = new DocumentManager();
                dt = DocumentManager.GetMissingEntryDocs();
                dt.TableName = "MissingEntryDocs";
                return dt;

            }
            catch (Exception ex)
            {
                return null;

            }
        }
        #endregion

        #region Sneha 15 Mar 2019
        [WebMethod(MessageName = "check if docname is present in excelentry", Description = "check if docname is present in excelentry")]
        public DataTable CheckDocNameInExcel(string DocName)
        {
            try
            {
                DataTable dt = new DataTable();
                DbTransaction objDbTransaction = Utility.GetTransaction;
                DataSet ds = new DataSet();
                Utility objUtility = new Utility();
                DocumentManager objDocumentManager = new DocumentManager();
                dt = objDocumentManager.CheckExcelEntry(DocName);
                dt.TableName = "EXCELENTRY1";
                return dt;

            }
            catch (Exception ex)
            {
                return null;

            }
        }
        [WebMethod(MessageName = "update document status", Description = "update document status")]
        public bool UpdateDocumentStatusId(Document objDocument)
        {
            try
            {
                DbTransaction objDbTransaction = Utility.GetTransaction;
                Utility objUtility = new Utility();
                DocumentManager objDocumentManager = new DocumentManager();
                objUtility.Result = objDocumentManager.UpdateDocument(objDocument, objDbTransaction);
                if (objUtility.Result == Utility.ResultType.Success)
                {
                    objDbTransaction.Commit();
                    return true;
                }
                else
                {
                    objDbTransaction.Rollback();
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

        


    }
}
