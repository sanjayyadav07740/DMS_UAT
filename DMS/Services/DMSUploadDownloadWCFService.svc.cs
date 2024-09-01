using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Data.Common;
using DMS.BusinessLogic;
using System.IO;
using System.Data;
using iTextSharp.text.pdf;

namespace DMS.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "DMSUploadDownloadWCFService" in code, svc and config file together.
    public class DMSUploadDownloadWCFService : IDMSUploadDownloadWCFService
    {
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

        public bool UploadDocumentToServer(byte[] objFileByte, Document objDocument)
        {
            try
            {
                objDocument.DocumentGuid = Utility.GetUniqueFileName(Path.GetExtension(objDocument.DocumentName));
                objDocument.DocumentPath = Utility.DocumentPath + objDocument.DocumentGuid;
                objDocument.DocumentType = Path.GetExtension(objDocument.DocumentName);
                objDocument.Size = Convert.ToInt32(objFileByte.Length);

                FileStream objStream = File.Create(objDocument.DocumentPath);
                objStream.Write(objFileByte, 0, objFileByte.Length);
                objStream.Close();
                objStream.Dispose();


                PdfReader reader = new PdfReader(objDocument.DocumentPath);
                objDocument.PageCount = reader.NumberOfPages;

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

        public string GetUniqueFileName(string strExtension)
        {
            return Utility.GetUniqueFileName(strExtension);
        }

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

        public bool InsertDocument(DMS.BusinessLogic.Document objDocument)
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

        public DataTable LoadRepository()
        {
            DataTable objDataTable = DataHelper.ExecuteDataTable("SELECT ID,RepositoryName FROM vwRepository WHERE STATUS = 1 ORDER BY RepositoryName", null);
            objDataTable.TableName = "DATA";
            return objDataTable;
        }

        public DataTable LoadMetaTemplate(int intRepositoryID)
        {
            DataTable objDataTable = DataHelper.ExecuteDataTable("SELECT ID,MetaTemplateName FROM vwMetaTemplate WHERE RepositoryID = " + intRepositoryID + " AND STATUS = 1 ORDER BY MetaTemplateName", null);
            objDataTable.TableName = "DATA";
            return objDataTable;
        }

        public DataTable LoadCategory(int intMetaTemplateID)
        {
            DataTable objDataTable = DataHelper.ExecuteDataTable("SELECT ID,CategoryName FROM vwCategory WHERE MetaTemplateID = " + intMetaTemplateID + " AND STATUS = 1 ORDER BY CategoryName", null);
            objDataTable.TableName = "DATA";
            return objDataTable;
        }

        public DataTable LoadFolder(int intMetaTemplateID)
        {
            DataTable objDataTable = DataHelper.ExecuteDataTable("SELECT * FROM vwFolder WHERE MetaTemplateID =" + intMetaTemplateID + " AND Status=1", null);
            objDataTable.TableName = "DATA";
            return objDataTable;
        }

        public DataTable LoadMetaDataCode(DMS.BusinessLogic.MetaData objMetaData)
        {
            DataTable objDataTable = new DataTable();
            DocumentManager objDocumentManager = new DocumentManager();
            objDocumentManager.SelectMetaDataForDropDown(out objDataTable, objMetaData);
            objDataTable.TableName = "DATA";
            return objDataTable;
        }

        public DataTable LoadDocument(string query)
        {
            DataTable objDataTable = DataHelper.ExecuteDataTable(query, null);
            objDataTable.TableName = "DATA";
            return objDataTable;
        }

        public DataTable AuthenticateUser(string userName, string password)
        {
            string query = "SELECT * FROM vwUser WHERE UserName='" + userName + "' AND Password='" + password + "'";
            DataTable objDataTable = DataHelper.ExecuteDataTable(query, null);
            objDataTable.TableName = "DATA";
            return objDataTable;
        }
    }
}
