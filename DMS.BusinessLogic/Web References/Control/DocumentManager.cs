using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DMS.BusinessLogic;
using System.Data;
using System.Data.Common;
using DMS.BusinessLogic;
using System.Web;
using System.Net.NetworkInformation;

namespace DMS.BusinessLogic
{
    public  class DocumentManager
    {
        #region Private Member
        Utility objUtility = new Utility();
        #endregion

        #region Enum
        public enum Status { Approved = 1, Rejected = 2, Uploaded = 3, EntryCompleted = 4, TotalUploaded = 5 };
        #endregion

        # region Method
        public Utility.ResultType InsertDocApproveRejectDetails(DocumentStatus objDocumentStaus, DbTransaction objDbTransaction)
        {
            try
            {
                return DocumentStatus.InsertDocApproveRejectDetails(objDocumentStaus, objDbTransaction);
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return Utility.ResultType.Error;
            }
        }
        public Utility.ResultType InsertMetaData(MetaData objMetaData, DbTransaction objDbTransaction)
        {
            try
            {
                return MetaData.Insert(objMetaData, objDbTransaction);
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return Utility.ResultType.Error;
            }
        }

        public Utility.ResultType InsertBulkUpload(Entity.BulkUpload objBulkUpload)
        {
            try
            {
                return Entity.BulkUpload.Insert(objBulkUpload);
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return Utility.ResultType.Error;
            }
        }

        public Utility.ResultType UpdateBulkUpload(Entity.BulkUpload objBulkUpload)
        {
            try
            {
                return Entity.BulkUpload.Update(objBulkUpload);
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return Utility.ResultType.Error;
            }
        }

        public Utility.ResultType UpdateDocumentForBulkUploadInsertMetaDataID(Document objDocument, DbTransaction objDbTransaction)
        {
            return Document.UpdateDocumentInsertMetaDataID(objDocument, objDbTransaction);
        }

        public Utility.ResultType InsertDocumentEntry(DocumentEntry objDocumentEntry, DbTransaction objDbTransaction)
        {
            try
            {
                return DocumentEntry.Insert(objDocumentEntry, objDbTransaction);
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return Utility.ResultType.Error;
            }
        }

        public Utility.ResultType UpdateDocumentEntry(DocumentEntry objDocumentEntry, DbTransaction objDbTransaction, out int intAffectedRows)
        {
           return DocumentEntry.Update(objDocumentEntry, objDbTransaction,out intAffectedRows);
        }

        public Utility.ResultType DeleteDocumentEntry(DocumentEntry objDocumentEntry, DbTransaction objDbTransaction)
        {
            try
            {
                return DocumentEntry.Delete(objDocumentEntry, objDbTransaction);
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return Utility.ResultType.Error;
            }
        }

        public Utility.ResultType InsertDocument(Document objDocument, DbTransaction objDbTransaction)
        {
            try
            {
                return Document.Insert(objDocument, objDbTransaction);
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return Utility.ResultType.Error;
            }
        }

        public Utility.ResultType UpdateDocument(Document objDocument, DbTransaction objDbTransaction)
        {
            try
            {
                return Document.Update(objDocument, objDbTransaction);
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return Utility.ResultType.Error;
            }
        }

        public Utility.ResultType SelectField(out DataTable objDataTable, int intMetaTemplateID)
        {
            try
            {
                return MetaTemplateFields.Select(out objDataTable, intMetaTemplateID);
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                objDataTable = null;
                return Utility.ResultType.Error;
            }
        }

        public Utility.ResultType SelectMetaData(out DataTable objDataTable, int intMetaDataID)
        {
            try
            {
                return MetaData.Select(out objDataTable, intMetaDataID);
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                objDataTable = null;
                return Utility.ResultType.Error;
            }
        }

        public Utility.ResultType SelectMetaDataForGrid(out DataTable objDataTable, Status enumStatus, MetaData objMetaData)
        {
            try
            {
                return MetaData.SelectForGrid(out objDataTable, enumStatus, objMetaData);
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                objDataTable = null;
                return Utility.ResultType.Error;
            }
        }

        public Utility.ResultType SelectMetaDataForGridNew(out DataTable objDataTable, Status enumStatus, MetaData objMetaData, string FromDate, string ToDate)
        {
            try
            {
                return MetaData.SelectForGridNew(out objDataTable, enumStatus, objMetaData, FromDate, ToDate);
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                objDataTable = null;
                return Utility.ResultType.Error;
            }
        }

        public Utility.ResultType SelectMetaDataForSearch(out DataTable objDataTable, MetaData objMetaData, DocumentEntry objDocumentEntry, MetaData.SearchType enumSearchType)
        {
            try
            {
                return MetaData.SelectForFieldSearch(out objDataTable, objMetaData, objDocumentEntry, enumSearchType);
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                objDataTable = null;
                return Utility.ResultType.Error;
            }
        }

        //public Utility.ResultType SelectMetaDataForSearch_Indepay(out DataTable objDataTable, MetaData objMetaData, DocumentEntry objDocumentEntry, MetaData.SearchType enumSearchType)
        //{
        //    try
        //    {
        //        return MetaData.SelectForFieldSearch_Indepay(out objDataTable, objMetaData, objDocumentEntry, enumSearchType);
        //    }
        //    catch (Exception ex)
        //    {
        //        LogManager.ErrorLog(Utility.LogFilePath, ex);
        //        objDataTable = null;
        //        return Utility.ResultType.Error;
        //    }
        //}

        public Utility.ResultType SelectMetaDataForTagSearch(out DataTable objDataTable, MetaData objMetaData, Document objDocument, MetaData.SearchType enumSearchType)
        {
            try
            {
                return MetaData.SelectForTagSearch(out objDataTable, objMetaData, objDocument, enumSearchType);
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                objDataTable = null;
                return Utility.ResultType.Error;
            }
        }

        public Utility.ResultType SelectMetaDataForContentSearch(out DataTable objDataTable, MetaData objMetaData,string strDocumentID)
        {
            try
            {
                return MetaData.SelectForContentSearch(out objDataTable, objMetaData,strDocumentID);
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                objDataTable = null;
                return Utility.ResultType.Error;
            }
        }

        public Utility.ResultType SelectDocument(out DataTable objDataTable, int intMetaDataID, Status enumStatus,string strDocumentID)
        {
            try
            {
                if (enumStatus == Status.Uploaded)
                {
                    return Document.SelectUploaded(out objDataTable, intMetaDataID, strDocumentID);
                }
                else if (enumStatus == Status.EntryCompleted)
                {
                    return Document.SelectEntryCompleted(out objDataTable, intMetaDataID, strDocumentID);
                }
                else if (enumStatus == Status.Approved)
                {
                    return Document.SelectApproved(out objDataTable, intMetaDataID, strDocumentID);
                }
                else if (enumStatus == Status.Rejected)
                {
                    return Document.SelectRejected(out objDataTable, intMetaDataID, strDocumentID);
                }

                objDataTable = null;
                return Utility.ResultType.Failure;
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                objDataTable = null;
                return Utility.ResultType.Error;
            }
        }

        public Utility.ResultType SelectDocumentData(out DataTable objDataTable, int intMetaDataID)
        {
            try
            {
                return DocumentEntry.Select(out objDataTable, intMetaDataID);
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                objDataTable = null;
                return Utility.ResultType.Error;
            }
        }

        public Utility.ResultType SelectDocumentData(DocumentEntry objDocumentEntry)
        {
            try
            {
                return DocumentEntry.SelectForValueExistance(objDocumentEntry);
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return Utility.ResultType.Error;
            }
        }

        public Utility.ResultType SelectMetaDataForDropDown(out DataTable objDataTable, MetaData objMetaData)
        {
            return MetaData.SelectByMetaDataForDropDown(out objDataTable, objMetaData);
        }

        public Utility.ResultType SelectBulkUpload(out DataTable objDataTable, Entity.BulkUpload objBulkUpload)
        {
            return Entity.BulkUpload.Select(out objDataTable, objBulkUpload);
        }

        public Utility.ResultType SelectDocumentForBulkUpload(Document objDocument)
        {
            return Document.SelectDocumentForBulkUpload(objDocument);
        }

        public Utility.ResultType SelectAllDocument(out DataTable objDataTable, int intMetaDataID,DocumentManager.Status enumStatus)
        {
            try
            {
                return Document.SelectAllDocument(out objDataTable, intMetaDataID, enumStatus);
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                objDataTable = null;
                return Utility.ResultType.Error;
            }
        }

        public Utility.ResultType SelectDocument(out DataTable objDataTable, int intMetaDataID)
        {
            try
            {
                return Document.SelectDocument(out objDataTable, intMetaDataID);
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                objDataTable = null;
                return Utility.ResultType.Error;
            }
        }

        public Utility.ResultType UpdateDocumentForBulkUpload(Document objDocument, DbTransaction objDbTransaction)
        {
            try
            {
                return Document.UpdateForBulkUpload(objDocument, objDbTransaction);
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return Utility.ResultType.Error;
            }
        }

        public Utility.ResultType SelectDocumentDataForDashBoard(out DataTable objDataTable,MetaData.DashBoard enumDashBoard,MetaData objMetaData)
        {
            return MetaData.SelectForDashBoard(out objDataTable, enumDashBoard, objMetaData);
        }
        public Utility.ResultType SelectDocument(out DataTable objDataTable,string strWhereCondition)
        {
            return Document.Select(out objDataTable, strWhereCondition);
        }

        public static Utility.ResultType Update(int Status, int ID, DbTransaction objDbTransaction)
        {
            return MetaData.Update(Status, ID, objDbTransaction);
        }

        public Utility.ResultType UpdateDocumentForMerging(Document objDocument, DbTransaction objDbTransaction)
        {
            return Document.UpdateDocumentForMerging(objDocument, objDbTransaction);
        }

        public Utility.ResultType SelectReportDocument(out DataTable objDataTable, MetaData objMetaData, string FromDate, string ToDate)
        {
            return Document.SelectReportDocumentNew(out objDataTable, objMetaData, FromDate, ToDate);
        }
        public Utility.ResultType SelectReportDocumentMHADA(out DataTable objDataTable, MetaData objMetaData, string FromDate, string ToDate)
       {
           return Document.SelectReportDocumentNewMHADA(out objDataTable, objMetaData, FromDate, ToDate);
       }
       public Utility.ResultType SelectReportDocumentIDBI(out DataTable objDataTable,string FromDate,string ToDate)
       {
           return Document.SelectReportDocumentIDBI(out objDataTable, FromDate,ToDate);
       }
        public DataSet CheckDocument(string Docname,int PageCount, int RepId)
       {
           return Document.CheckDocument(Docname, PageCount, RepId);
       }
        public DataSet CheckDocumentSingleUpload(string Docname, int RepId)
        {
            return Document.CheckDocumentSingleUpload(Docname, RepId);
        }
        public DataSet CheckDocument_Centrum(string Docname, int RepId, int metatemplateId, int CategoryId, int FolderId)
        {
            return Document.CheckDocument_centrum(Docname, RepId, metatemplateId, CategoryId, FolderId);
        }

        public Utility.ResultType UpdateDocumentName(int DocId, string DocName, string DocTag, DbTransaction objDBTransaction)
        {
            return Document.UpdateDocumentName(DocId, DocName, DocTag, objDBTransaction);
        }

        public Utility.ResultType SelectDocumentForDelete(int Repository, int MetaTemplate, int Category, int Folder, out DataTable objDataTable)
        {
            return Document.SelectDocumentForDelete(Repository, MetaTemplate, Category, Folder, out objDataTable);
        }

        public Utility.ResultType InsertMergedDocumentDetails(Document objDocument, DbTransaction objDbTransaction)
        {
            try
            {
                return Document.InsertMergedDocumentDetails(objDocument, objDbTransaction);
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return Utility.ResultType.Error;
            }
        }

        public Utility.ResultType InsertMergedDocumentDetailsTIFF(Document objDocument, DbTransaction objDbTransaction)
        {
            try
            {
                return Document.InsertMergedDocumentDetailsTIFF(objDocument, objDbTransaction);
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return Utility.ResultType.Error;
            }
        }

        public Utility.ResultType MergeInsert(Document objDocument, DbTransaction objDbTransaction)
        {
            try
            {
                return Document.MergeInsert(objDocument, objDbTransaction);
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return Utility.ResultType.Error;
            }
        }

         #region Seema 7 dec 2017
        public DataSet CheckDocument_Reliance(string Docname,  int RepId,int MetId,int CatId)
        {
            return Document.CheckDocument_Reliance(Docname, RepId,MetId,CatId);
        }
        #endregion

        #region Seema 16 July 2018
        public DataSet CheckDocument_SBIMutual(string Docname, int RepId, int MetId, int Folderid)
        {
            return Document.CheckDocument_SBIMutual(Docname, RepId, MetId, Folderid);
        }
        #endregion
        #region Seema 16 July 2018
         public static DataSet CheckDocument_IDBI(string Docname, int MetId,int RepId)
        {
            return Document.CheckDocument_IDBI(Docname, MetId, RepId);
        }
              #endregion

         #region Sneha 16 Jan 2019
         public Utility.ResultType InsertErrorDoc(string MetatemplateName, string DocumentName, string ErrorType, DbTransaction objDbTransaction)
         {
             return Document.InsertErrorDoc(MetatemplateName,DocumentName, ErrorType, objDbTransaction);
         }
         #endregion
         #region Sneha 17 Jan 2019
         public Utility.ResultType InsertExcelEntry_IDBI_Ahm(int DocumentId, int MetadataId, DateTime DateOfRecievalAtRpu, DateTime DateOfAccOpening, string Custid, string AccNo, string BoxNo, string DocNo, string SHIL_Barcode, string SHCIL_Barcode_Date, string CloseFlag, string ExcelName, DbTransaction objDbTransaction)
         {
             return Document.InsertExcelEntry_IDBI_Ahm(DocumentId, MetadataId, DateOfRecievalAtRpu, DateOfAccOpening, Custid, AccNo, BoxNo, DocNo, SHIL_Barcode, SHCIL_Barcode_Date, CloseFlag, ExcelName, objDbTransaction);
         }

         public static DataTable SelectDocumentID(string DocName, int RepId)
         {
             return Document.SelectDocumentID(DocName, RepId);
         }
         #endregion

        #region Sneha 18 Jan 2019
         public static DataTable ExistsInExcelEntry(int DocId)
         {
             return Document.ExistsInExcelEntry(DocId);
         }

        public static Utility.ResultType InsertExcelEntryError_IDBI_Ahm(int DocumentId, int MetadataId, string ErrorName, DateTime DateOfRecievalAtRpu, DateTime DateOfAccOpening, string Custid, string AccNo, string BoxNo, string DocNo, string SHIL_Barcode, string SHCIL_Barcode_Date, string CloseFlag, string ExcelName, DbTransaction objDbTransaction)
         {
             return Document.InsertExcelEntryError_IDBI_Ahm(DocumentId, MetadataId,ErrorName, DateOfRecievalAtRpu, DateOfAccOpening, Custid, AccNo, BoxNo, DocNo, SHIL_Barcode, SHCIL_Barcode_Date, CloseFlag, ExcelName, objDbTransaction);
         }
        #endregion

        #region Sneha 25 Jan 2019
         public static DataTable GetErrorDocs()
        {
            return Document.GetErrorDocs();
        }

         public static DataTable GetMissingEntryDocs()
         {
             return Document.GetMissingEntryDocs();
         }
        #endregion

        #region Sneha 06 Feb 2019
         public Utility.ResultType InsertIDBIDownloadLog(int DocId, DbTransaction objDbTransaction)
         {
             try
             {
                 return Document.InsertIDBIDownloadLog(DocId, objDbTransaction);
             }
             catch (Exception ex)
             {
                 LogManager.ErrorLog(Utility.LogFilePath, ex);
                 return Utility.ResultType.Error;
             }
         }

         public static DataTable GetIDBIDownloadCount()
         {
             return Document.GetIDBIDownloadCount();
         }
        #endregion

        #region Sneha 26 Feb 2019
         public DataTable SelectErrorDocs(string ErrorType)
         {
             return Document.SelectErrorDocs(ErrorType);
         }
         public DataTable CheckExcelEntry(string DocName)
         {
             return Document.CheckExcelEntry(DocName);
         }
        #endregion

         public Utility.ResultType SelectMetaDataForFolderSearch(out DataTable objDataTable, MetaData objMetaData)
         {
             try
             {

                 return MetaData.SelectForFolderSearch(out objDataTable, objMetaData);
             }
             catch (Exception ex)
             {
                 LogManager.ErrorLog(Utility.LogFilePath, ex);
                 objDataTable = null;
                 return Utility.ResultType.Error;
             }
         }


         public  Utility.ResultType DocumentRenameMetaData(DocumentRename1 objDocumentRename, DbTransaction objDbTransaction)
         {
             try
             {
                 return DocumentRename1.DocumentRenameInsert(objDocumentRename, objDbTransaction);
             }
             catch (Exception ex)
             {
                 LogManager.ErrorLog(Utility.LogFilePath, ex);
                 return Utility.ResultType.Error;
             }

         }

         public Utility.ResultType DocumentRename(DocumentRename1 objDocumentRename, DbTransaction objDbTransaction)
         {
             try
             {
                 return DocumentRename1.DocumentRename(objDocumentRename, objDbTransaction);
             }
             catch (Exception ex)
             {
                 LogManager.ErrorLog(Utility.LogFilePath, ex);
                 return Utility.ResultType.Error;
             }

         }


         public Utility.ResultType DocumentDelete(int docid, int userid)
         {
             try
             {
                 return Document.DocumentDelete(docid, userid);
             }
             catch (Exception ex)
             {
                 LogManager.ErrorLog(Utility.LogFilePath, ex);
                 return Utility.ResultType.Error;
             }

         }

         public DataTable FileSizeCount(out DataTable objDataTable,int RepoID)
         {
             try
             {
                 Document.SelectFileSize(out objDataTable, RepoID);
                 return objDataTable;
             }
             catch (Exception ex)
             {
                 LogManager.ErrorLog(Utility.LogFilePath, ex);
                 objDataTable = null;
                 return objDataTable;
             }
         }

         public void DocumentMoveHistory(HttpContext context, int DocId, int OldMetadataID, int NewMetadataID, string Entity, string from, string to,int UserID)
         {
             Document.InsertDocMoveHistory(GetIPAddress(context), Utility.GetMacAddress(), UserID, DocId, OldMetadataID, NewMetadataID, Entity, from, to);

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

         //public static Utility.ResultType DHSUpdate( DocumentRename1 objDocu, DbTransaction objDbTransaction)
         //{
         //    try
         //    {
         //        return DocumentRename1.updateDocument(  objDocu,  objDbTransaction);
         //    }
         //    catch (Exception ex)
         //    {
         //         LogManager.ErrorLog(Utility.LogFilePath, ex);
         //        return Utility.ResultType.Error;
         //    }
         //}
       
        
         //public Utility.ResultType DocumentSearch_New(out DataTable objDataTable, MetaData objMetaData, Document objDocument, MetaData.SearchType enumSearchType)
         //{
         //    try
         //    {
         //        return MetaData.DocumentSearch_New(out objDataTable, objMetaData, objDocument, enumSearchType);
         //    }
         //    catch (Exception ex)
         //    {
         //        LogManager.ErrorLog(Utility.LogFilePath, ex);
         //        objDataTable = null;
         //        return Utility.ResultType.Error;
         //    }
         //}
       
         public Utility.ResultType DocumentSearch_New(out DataTable objDataTable, out int RecordCount, MetaData objMetaData,
             Document objDocument, MetaData.SearchType enumSearchType, int PageIndex, int PageSize)
         {
             RecordCount = 0;
             try
             {
                 return MetaData.DocumentSearch_New(out objDataTable, out RecordCount, objMetaData,
                     objDocument, enumSearchType, PageIndex, PageSize);
             }
             catch (Exception ex)
             {
                 LogManager.ErrorLog(Utility.LogFilePath, ex);
                 objDataTable = null;
                 return Utility.ResultType.Error;
             }
         }
       
        #endregion

         public DataSet CheckDocumentVirescent(string Docname, int RepId, int metatemplateId)
         {
             return Document.CheckDocumentVirescent(Docname, RepId, metatemplateId);
         }

         public Utility.ResultType UpdateDocumentForMergingNEW(Document objDocument, DbTransaction objDbTransaction)
         {
             return Document.UpdateDocumentForMergingNEW(objDocument, objDbTransaction);
         }

         public Utility.ResultType SelectMetaDataForContentFolderOptSearch(out DataTable objDataTable, MetaData objMetaData, string strDocumentID)
         {
             try
             {
                 return MetaData.SelectForContentOptFolderSearch(out objDataTable, objMetaData, strDocumentID);
             }
             catch (Exception ex)
             {
                 LogManager.ErrorLog(Utility.LogFilePath, ex);
                 objDataTable = null;
                 return Utility.ResultType.Error;
             }
         }

         //public Utility.ResultType UploadedDcoumentReport(out DataTable dt, int RepositoryID, int MetatemplateId, int CategoryId, int FolderID, string fromdate, string todate)
         //{
         //    return Document.UploadedDcoumentReport(out dt, RepositoryID, MetatemplateId,CategoryId, FolderID, fromdate, todate);
         //}

         public Utility.ResultType UploadedDcoumentReport(out DataTable dt, int RepositoryID, int MetatemplateId, int FolderID, string fromdate, string todate)
         {
             return Document.UploadedDcoumentReport(out dt, RepositoryID, MetatemplateId, FolderID, fromdate, todate);
         }

         public Utility.ResultType FieldSearch(out DataTable dt, int RepositoryID, int MetatemplateId, int FolderID, string FieldData)
         {
             return Document.FieldSearch(out dt, RepositoryID, MetatemplateId, FolderID, FieldData);
         }


         public Utility.ResultType SearchInAll(out DataTable objDataTable, string DocName, int userID, int Flag)
         {
             return Document.SearchInAll(out objDataTable, DocName, userID, Flag);
         }

         public Utility.ResultType DocumentRename(string DocName, int DocID)
         {
             DbTransaction objDbTransaction = Utility.GetTransaction;
             return Document.DocumentRename(DocName, DocID, objDbTransaction);
         }

         public Utility.ResultType AuditReports(out DataTable dt, string username, string fromdate, string todate, string UserID)
         {
             return Document.AuditReports(out dt, username, fromdate, todate, UserID);
         }

         public Utility.ResultType PageHitCount(out DataTable dt, string username, string fromdate, string todate)
         {
             return Document.PageHitCount(out dt, username, fromdate, todate);
         }

         public DataTable GetFieldsandData(int metatemplateid, int documentid)
         {
             string query = "select doc.id,mtf.FieldName,doc.FieldData from MetaTemplateFields mtf join DocumentEntry doc on doc.FieldID=mtf.ID where mtf.MetaTemplateID=" + metatemplateid + " and doc.DocumentID = " + documentid;
             DataTable objDataTable = DataHelper.ExecuteDataTable(query, null);
             objDataTable.TableName = "DATA";
             return objDataTable;
         }

         public Utility.ResultType LoadRecentDocument(out DataTable objDataTable, string Action, int roleid, int userid)
         {
             try
             {
                 return Document.LoadRecentDocument(out objDataTable, Action, roleid, userid);
             }
             catch (Exception ex)
             {
                 LogManager.ErrorLog(Utility.LogFilePath, ex);
                 objDataTable = null;
                 return Utility.ResultType.Error;
             }
         }
        
         public Utility.ResultType LoadCountDocumnet(out DataTable objDataTable, int roleid, int userid)
         {
             try
             {
                 return Document.LoadCountDocumnet(out objDataTable, roleid, userid);
             }
             catch (Exception ex)
             {
                 LogManager.ErrorLog(Utility.LogFilePath, ex);
                 objDataTable = null;
                 return Utility.ResultType.Error;
             }
         }
        
         # region Download

         public Utility.ResultType DownloadUserTrue(out DataTable dt, String id)
         {
             return Document.DownloadUserTrue(out dt, id);
         }

         public Utility.ResultType DownloadUserFalse(out DataTable dt, String id)
         {
             return Document.DownloadUserFalse(out dt, id);
         }

         #endregion

         # region View

         public Utility.ResultType ViewTrue(out DataTable dt, String id)
         {
             return Document.ViewTrue(out dt, id);
         }

         public Utility.ResultType ViewFalse(out DataTable dt, String id)
         {
             return Document.ViewFalse(out dt, id);
         }

         #endregion

         # region Edit

         public Utility.ResultType EditTrue(out DataTable dt, String id)
         {
             return Document.EditTrue(out dt, id);
         }

         public Utility.ResultType EditFalse(out DataTable dt, String id)
         {
             return Document.EditFalse(out dt, id);
         }

         #endregion

         # region Merge

         public Utility.ResultType MergeTrue(out DataTable dt, String id)
         {
             return Document.MergeTrue(out dt, id);
         }

         public Utility.ResultType MergeFalse(out DataTable dt, String id)
         {
             return Document.MergeFalse(out dt, id);
         }

         #endregion

         # region Split

         public Utility.ResultType SplitTrue(out DataTable dt, String id)
         {
             return Document.SplitTrue(out dt, id);
         }

         public Utility.ResultType SplitFalse(out DataTable dt, String id)
         {
             return Document.SplitFalse(out dt, id);
         }

         #endregion

         # region Delete

         public Utility.ResultType DeleteTrue(out DataTable dt, String id)
         {
             return Document.DeleteTrue(out dt, id);
         }

         public Utility.ResultType DeleteFalse(out DataTable dt, String id)
         {
             return Document.DeleteFalse(out dt, id);
         }

         #endregion

         public Utility.ResultType GetUserData(out DataTable dt, int UserId)
         {
             return Document.GetUserData(out dt, UserId);
         }

         public Utility.ResultType ValidateUser(out DataTable dt, int UserId)
         {
             return Document.ValidateUser(out dt, UserId);
         }



    }
}
