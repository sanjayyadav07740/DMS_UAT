using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;


namespace DMS.BusinessLogic
{
    public class Document
    {
        #region Properties
        public int DocumentID { get; set; }
        public int MetaDataID { get; set; }
        public string DocumentName { get; set; }
        public string DocumentGuid { get; set; }
        public int Size { get; set; }
        public string DocumentPath { get; set; }
        public string DocumentType { get; set; }
        public byte[] Image { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public int UpdatedBy { get; set; }
        public int DocumentStatusID { get; set; }
        public int Status { get; set; }
        public string Tag { get; set; }
        public int IsLucened { get; set; }
        public int PageCount { get; set; }
        //public DateTime ExpiryDate { get; set; }
        //public int NotificationBefore { get; set; }
        //public int NotificationInterval { get; set; }
        public int OutPutValuues { get; set; }
        public string showcount { get; set; }

        public string SearchType { get; set; }
        public int VersionNo { get; set; }
        public int OldPageCount { get; set; }
        public int ID { get; set; }
        public int MergedBy { get; set; }
        public int MergedPageCount { get; set; }
        public string IPAddress { get; set; }
        #endregion

        #region Method

        #region Seema 27 sep 2017

        public static DataSet GetDocumentID(string DocumentName)
        {
            try
            {
                string strQuery = "select D.ID from document D inner join MetaData M on D.MetaDataID=M.ID where D.DocumentName like '%" + DocumentName + "%' and D.status=1 and D.pagecount>0 and M.RepositoryID=42";

                DataSet ds = new DataSet();
                ds = DataHelper.ExecuteDataSet(strQuery);
                return ds;
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                throw ex;
            }
        }

        public static Utility.ResultType MergeInsert(Document objDocument, DbTransaction objDbTransaction)
        {
            DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

            try
            {
                DbParameter[] objDbParameter = new DbParameter[9];

                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "DocId";
                objDbParameter[0].Value = objDocument.DocumentID;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "DocumentName";
                objDbParameter[1].Value = objDocument.DocumentName;

                objDbParameter[2] = objDbProviderFactory.CreateParameter();
                objDbParameter[2].ParameterName = "DocumentGuid";
                objDbParameter[2].Value = objDocument.DocumentGuid;

                objDbParameter[3] = objDbProviderFactory.CreateParameter();
                objDbParameter[3].ParameterName = "DocumentPath";
                objDbParameter[3].Value = objDocument.DocumentPath;

                objDbParameter[4] = objDbProviderFactory.CreateParameter();
                objDbParameter[4].ParameterName = "CreatedOn";
                objDbParameter[4].Value = DateTime.Now;

                objDbParameter[5] = objDbProviderFactory.CreateParameter();
                objDbParameter[5].ParameterName = "UpdatedBy";
                objDbParameter[5].Value = objDocument.UpdatedBy;

                objDbParameter[6] = objDbProviderFactory.CreateParameter();
                objDbParameter[6].ParameterName = "MergedPageCount";
                objDbParameter[6].Value = objDocument.MergedPageCount;

                objDbParameter[7] = objDbProviderFactory.CreateParameter();
                objDbParameter[7].ParameterName = "DocumentSize";
                objDbParameter[7].Value = objDocument.Size;

                objDbParameter[8] = objDbProviderFactory.CreateParameter();
                objDbParameter[8].ParameterName = "IPAddress";
                objDbParameter[8].Value = objDocument.IPAddress;

                DataHelper.ExecuteNonQuery("SP_I_MergeDocument", objDbTransaction, objDbParameter);

                return Utility.ResultType.Success;
            }
            catch (Exception ex)
            {

                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return Utility.ResultType.Error;
            }
        }



        #endregion

        public static Utility.ResultType Insert(Document objDocument, DbTransaction objDbTransaction)
        {
            DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

            try
            {
                DbParameter[] objDbParameter = new DbParameter[17];

                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "MetaDataID";
                objDbParameter[0].Value = objDocument.MetaDataID;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "DocumentName";
                objDbParameter[1].Value = objDocument.DocumentName;

                objDbParameter[2] = objDbProviderFactory.CreateParameter();
                objDbParameter[2].ParameterName = "DocumentGuid";
                objDbParameter[2].Value = objDocument.DocumentGuid;

                objDbParameter[3] = objDbProviderFactory.CreateParameter();
                objDbParameter[3].ParameterName = "DocumentSize";
                objDbParameter[3].Value = objDocument.Size;

                objDbParameter[4] = objDbProviderFactory.CreateParameter();
                objDbParameter[4].ParameterName = "DocumentPath";
                objDbParameter[4].Value = objDocument.DocumentPath;

                objDbParameter[5] = objDbProviderFactory.CreateParameter();
                objDbParameter[5].ParameterName = "DocumentType";
                objDbParameter[5].Value = objDocument.DocumentType;


                objDbParameter[6] = objDbProviderFactory.CreateParameter();
                objDbParameter[6].ParameterName = "Image";
                objDbParameter[6].Value = Utility.FileStorageType == Utility.StorageType.FileSystem ? new byte[0] : objDocument.Image;

                objDbParameter[7] = objDbProviderFactory.CreateParameter();
                objDbParameter[7].ParameterName = "CreatedOn";
                objDbParameter[7].Value = DateTime.Now;

                objDbParameter[8] = objDbProviderFactory.CreateParameter();
                objDbParameter[8].ParameterName = "CreatedBy";
                objDbParameter[8].Value = objDocument.CreatedBy;

                objDbParameter[9] = objDbProviderFactory.CreateParameter();
                objDbParameter[9].ParameterName = "UpdatedOn";
                objDbParameter[9].Value = DBNull.Value;

                objDbParameter[10] = objDbProviderFactory.CreateParameter();
                objDbParameter[10].ParameterName = "UpdatedBy";
                objDbParameter[10].Value = DBNull.Value;

                objDbParameter[11] = objDbProviderFactory.CreateParameter();
                objDbParameter[11].ParameterName = "DocumentStatusID";
                objDbParameter[11].Value = objDocument.DocumentStatusID;

                objDbParameter[12] = objDbProviderFactory.CreateParameter();
                objDbParameter[12].ParameterName = "Status";
                objDbParameter[12].Value = objDocument.Status;

                objDbParameter[13] = objDbProviderFactory.CreateParameter();
                objDbParameter[13].ParameterName = "DocumentID";
                objDbParameter[13].Size = 100;
                objDbParameter[13].Direction = ParameterDirection.Output;

                objDbParameter[14] = objDbProviderFactory.CreateParameter();
                objDbParameter[14].ParameterName = "Tag";
                objDbParameter[14].Value = objDocument.Tag;

                objDbParameter[15] = objDbProviderFactory.CreateParameter();
                objDbParameter[15].ParameterName = "IsLucened";
                objDbParameter[15].Value = objDocument.IsLucened;

                //objDbParameter[16] = objDbProviderFactory.CreateParameter();
                //objDbParameter[16].ParameterName = "ExpiryDate";
                //if (objDocument.ExpiryDate == DateTime.MinValue)
                //    objDbParameter[16].Value = DBNull.Value;
                //else
                //    objDbParameter[16].Value = objDocument.ExpiryDate;

                objDbParameter[16] = objDbProviderFactory.CreateParameter();
                objDbParameter[16].ParameterName = "PageCount";
                objDbParameter[16].Value = objDocument.PageCount;


                //objDbParameter[16] = objDbProviderFactory.CreateParameter();
                //objDbParameter[16].ParameterName = "PageCount";
                //objDbParameter[16].Value = objDocument.PageCount;


                //objDbParameter[17] = objDbProviderFactory.CreateParameter();
                //objDbParameter[17].ParameterName = "OLDPAGECOUNT";
                //objDbParameter[17].Value = objDocument.OldPageCount;


                //objDbParameter[18] = objDbProviderFactory.CreateParameter();
                //objDbParameter[18].ParameterName = "NotificationInterval";
                //objDbParameter[18].Value = objDocument.NotificationInterval;


                DataHelper.ExecuteNonQuery("SP_I_DOCUMENT", objDbTransaction, objDbParameter);

                objDocument.DocumentID = Convert.ToInt32(objDbParameter[13].Value);


                return Utility.ResultType.Success;
            }
            catch (Exception ex)
            {

                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return Utility.ResultType.Error;
            }
        }



        public static Utility.ResultType InsertDocumentShare(Document objDocument, DbTransaction objDbTransaction)
        {
            DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

            try
            {
                DbParameter[] objDbParameter = new DbParameter[17];

                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "EmailId";
                objDbParameter[0].Value = objDocument.MetaDataID;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "DocumentName";
                objDbParameter[1].Value = objDocument.DocumentName;

                objDbParameter[2] = objDbProviderFactory.CreateParameter();
                objDbParameter[2].ParameterName = "DocumentGuid";
                objDbParameter[2].Value = objDocument.DocumentGuid;

                objDbParameter[3] = objDbProviderFactory.CreateParameter();
                objDbParameter[3].ParameterName = "DocumentSize";
                objDbParameter[3].Value = objDocument.Size;

                objDbParameter[4] = objDbProviderFactory.CreateParameter();
                objDbParameter[4].ParameterName = "DocumentPath";
                objDbParameter[4].Value = objDocument.DocumentPath;

                objDbParameter[5] = objDbProviderFactory.CreateParameter();
                objDbParameter[5].ParameterName = "DocumentType";
                objDbParameter[5].Value = objDocument.DocumentType;


                objDbParameter[6] = objDbProviderFactory.CreateParameter();
                objDbParameter[6].ParameterName = "Image";
                objDbParameter[6].Value = Utility.FileStorageType == Utility.StorageType.FileSystem ? new byte[0] : objDocument.Image;

                objDbParameter[7] = objDbProviderFactory.CreateParameter();
                objDbParameter[7].ParameterName = "CreatedOn";
                objDbParameter[7].Value = DateTime.Now;

                objDbParameter[8] = objDbProviderFactory.CreateParameter();
                objDbParameter[8].ParameterName = "CreatedBy";
                objDbParameter[8].Value = objDocument.CreatedBy;

                objDbParameter[9] = objDbProviderFactory.CreateParameter();
                objDbParameter[9].ParameterName = "UpdatedOn";
                objDbParameter[9].Value = DBNull.Value;

                objDbParameter[10] = objDbProviderFactory.CreateParameter();
                objDbParameter[10].ParameterName = "UpdatedBy";
                objDbParameter[10].Value = DBNull.Value;

                objDbParameter[11] = objDbProviderFactory.CreateParameter();
                objDbParameter[11].ParameterName = "DocumentStatusID";
                objDbParameter[11].Value = objDocument.DocumentStatusID;

                objDbParameter[12] = objDbProviderFactory.CreateParameter();
                objDbParameter[12].ParameterName = "Status";
                objDbParameter[12].Value = objDocument.Status;

                objDbParameter[13] = objDbProviderFactory.CreateParameter();
                objDbParameter[13].ParameterName = "DocumentID";
                objDbParameter[13].Size = 100;
                objDbParameter[13].Direction = ParameterDirection.Output;

                objDbParameter[14] = objDbProviderFactory.CreateParameter();
                objDbParameter[14].ParameterName = "Tag";
                objDbParameter[14].Value = objDocument.Tag;

                objDbParameter[15] = objDbProviderFactory.CreateParameter();
                objDbParameter[15].ParameterName = "IsLucened";
                objDbParameter[15].Value = objDocument.IsLucened;

              

                objDbParameter[16] = objDbProviderFactory.CreateParameter();
                objDbParameter[16].ParameterName = "PageCount";
                objDbParameter[16].Value = objDocument.PageCount;


            


                DataHelper.ExecuteNonQuery("SP_I_DOCUMENT", objDbTransaction, objDbParameter);

                objDocument.DocumentID = Convert.ToInt32(objDbParameter[13].Value);


                return Utility.ResultType.Success;
            }
            catch (Exception ex)
            {

                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return Utility.ResultType.Error;
            }
        }

        //public static DataSet CheckDocument(string Docname,int PageCount,int RepId)
        //{
        //    try
        //    {
        //        //string strQuery = "select ID from DOCUMENT where DocumentName = '" + Docname + "' and Status=1";
        //        string strQuery = @"select D.ID from DOCUMENT D inner join MetaData M on D.MetaDataID=M.ID where D.DocumentName = '" + Docname + "' and D.PageCount='" + PageCount + "' and M.RepositoryID='" + RepId + "' and D.Status=1";
        //        DataSet ds = new DataSet();
        //        ds = DataHelper.ExecuteDataSet(strQuery);
        //        if (ds.Tables[0].Rows.Count == 0)
        //        {
        //            return ds;
        //        }
        //        return ds;

        //    }
        //    catch (Exception ex)
        //    {
        //        LogManager.ErrorLog(Utility.LogFilePath, ex);
        //        return null;
        //    }
        //}

        public static DataSet CheckDocument(string Docname, int PageCount, int RepId)
        {
            try
            {

                //string strQuery = "select ID from DOCUMENT where DocumentName = '" + Docname + "' and Status=1";
                string strQuery = @"select * from vwDocument D inner join MetaData M on D.MetaDataID=M.ID where D.DocumentName = '" + Docname + "' and D.PageCount='" + PageCount + "' and M.RepositoryID='" + RepId + "' and D.Status=1";
                DataSet ds = new DataSet();

                ds = DataHelper.ExecuteDataSet(strQuery);

                return ds;
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return null;
            }
        }


        public static DataSet CheckDocument(string Docname, int MetatemplateId)
        {
            try
            {
                //string strQuery = "select ID from DOCUMENT where DocumentName = '" + Docname + "' and Status=1";
                string strQuery = @"select DocumentName from vwDocument D inner join MetaData M 
                                  on D.MetaDataID=M.ID where D.DocumentName = '" + Docname + "' and M.MetaTemplateID='" + MetatemplateId + "' and D.Status=1";
                DataSet ds = new DataSet();
                ds = DataHelper.ExecuteDataSet(strQuery);
                return ds;
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return null;
            }
        }

        public static DataTable ChkCentrumMaster(string panno, string CustName)
        {
            try
            {
                string strQuery = string.Empty;
                if (panno != null)
                {
                    strQuery = "Select * from Centrum_Master where Pan_no like '%" + panno + "%'";
                }
                else
                { strQuery = "Select * from Centrum_Master where Cusomer_Name like '%" + CustName + "%'"; }

                DataTable dt = new DataTable();

                dt = DataHelper.ExecuteDataTable(strQuery);

                return dt;
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return null;
            }
        }

        public static DataSet CheckDocumentSingleUpload(string Docname, int RepId)
        {
            try
            {
                //string strQuery = "select ID from DOCUMENT where DocumentName = '" + Docname + "' and Status=1";
                string strQuery = @"select D.ID from DOCUMENT D inner join MetaData M 
                                  on D.MetaDataID=M.ID where D.DocumentName = '" + Docname + "' and M.RepositoryID='" + RepId + "' and D.Status=1";
                DataSet ds = new DataSet();
                ds = DataHelper.ExecuteDataSet(strQuery);
                return ds;
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return null;
            }
        }
        public static DataSet CheckDocument_centrum(string Docname, int RepId, int metatemplateId, int CategoryId, int FolderId)
        {
            try
            {
                //string strQuery = "select ID from DOCUMENT where DocumentName = '" + Docname + "' and Status=1";
                //commented and D.PageCount='" + PageCount + "'
                string strQuery = @"select D.ID from DOCUMENT D inner join MetaData M 
                                    on D.MetaDataID=M.ID inner join MetaTemplate MT
                                    on M.MetaTemplateID=MT.ID inner join Category C
                                    on M.CategoryID=C.ID inner join Folder F
                                    on M.FolderID=F.ID where D.DocumentName like '%" + Docname + "%'  and M.RepositoryID='" + RepId + "' and D.Status=1 and MT.ID='" + metatemplateId + "'  and C.ID='" + CategoryId + "' and F.ID='" + FolderId + "' and D.status=1";
                DataSet ds = new DataSet();
                ds = DataHelper.ExecuteDataSet(strQuery);
                return ds;
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return null;
            }
        }

        public static Utility.ResultType UpdateForVersion(Document objDocument, DbTransaction objDbTransaction)
        {
            DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

            try
            {
                DbParameter[] objDbParameter = new DbParameter[7];

                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "DocumentID";
                objDbParameter[0].Value = objDocument.DocumentID;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "DocumentName";
                objDbParameter[1].Value = objDocument.DocumentName;

                objDbParameter[2] = objDbProviderFactory.CreateParameter();
                objDbParameter[2].ParameterName = "Size";
                objDbParameter[2].Value = objDocument.Size;

                objDbParameter[3] = objDbProviderFactory.CreateParameter();
                objDbParameter[3].ParameterName = "DocumentPath";
                objDbParameter[3].Value = objDocument.DocumentPath;

                objDbParameter[4] = objDbProviderFactory.CreateParameter();
                objDbParameter[4].ParameterName = "DocumentType";
                objDbParameter[4].Value = objDocument.DocumentType;

                objDbParameter[5] = objDbProviderFactory.CreateParameter();
                objDbParameter[5].ParameterName = "UpdatedOn";
                objDbParameter[5].Value = DateTime.Now;

                objDbParameter[6] = objDbProviderFactory.CreateParameter();
                objDbParameter[6].ParameterName = "UpdatedBy";
                objDbParameter[6].Value = objDocument.UpdatedBy;

                DataHelper.ExecuteNonQuery("SP_U_DocumentForVersion", objDbTransaction, objDbParameter);

                return Utility.ResultType.Success;
            }
            catch (Exception ex)
            {

                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return Utility.ResultType.Error;
            }
        }

        public static Utility.ResultType Update(Document objDocument, DbTransaction objDbTransaction)
        {
            DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

            try
            {
                DbParameter[] objDbParameter = new DbParameter[4];

                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "DocumentID";
                objDbParameter[0].Value = objDocument.DocumentID;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "DocumentStatusIDValue";
                objDbParameter[1].Value = objDocument.DocumentStatusID;

                //objDbParameter[2] = objDbProviderFactory.CreateParameter();
                //objDbParameter[2].ParameterName = "UpdatedOn";
                //objDbParameter[2].Value = DateTime.Now;

                objDbParameter[2] = objDbProviderFactory.CreateParameter();
                objDbParameter[2].ParameterName = "UpdatedBy";
                objDbParameter[2].Value = objDocument.UpdatedBy;

                objDbParameter[3] = objDbProviderFactory.CreateParameter();
                objDbParameter[3].ParameterName = "UpdatedOn";
                objDbParameter[3].Value = DateTime.Now;

                DataHelper.ExecuteNonQuery("SP_U_Document_Virescent", objDbTransaction, objDbParameter);

                return Utility.ResultType.Success;
            }
            catch (Exception ex)
            {

                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return Utility.ResultType.Error;
            }
        }

        /// <summary>
        /// Change By Sashi To Increase count Of view DOCUMENT
        /// </summary>
        /// <param name="objDocument"></param>
        /// <param name="objDbTransaction"></param>
        /// <returns></returns>
        /// 
        public static Utility.ResultType Increasecount(Document objDocument, DbTransaction objDbTransaction)
        {
            DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

            try
            {
                DbParameter[] objDbParameter = new DbParameter[2];

                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "ID";
                objDbParameter[0].Value = objDocument.MetaDataID;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "count";
                objDbParameter[1].Value = objDocument.OutPutValuues;


                DataHelper.ExecuteNonQuery("increasecount", objDbTransaction, objDbParameter);

                return Utility.ResultType.Success;
            }
            catch (Exception ex)
            {

                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return Utility.ResultType.Error;
            }
        }

        /// <summary>
        /// Change By Sashi To Show count Of DOCUMENT
        /// </summary>
        /// <param name="objDocument"></param>
        /// <param name="objDbTransaction"></param>
        /// <param name="Count"></param>
        /// <returns></returns>
        public static Utility.ResultType LastTotalCount(Document objDocument, DbTransaction objDbTransaction, out int Count)
        {
            DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

            try
            {
                DbParameter[] objDbParameter = new DbParameter[2];

                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "ID";
                objDbParameter[0].Value = objDocument.MetaDataID;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "count";
                objDbParameter[1].Size = 100;
                objDbParameter[1].Direction = ParameterDirection.Output;

                DataHelper.ExecuteNonQuery("showcount", objDbTransaction, objDbParameter);

                //if (objDbParameter[1].Value != "0")
                //{
                Count = Convert.ToInt32(objDbParameter[1].Value);
                //}
                //else
                //{
                //    Count = 0;
                //}

                return Utility.ResultType.Success; ;
            }
            catch (Exception ex)
            {
                Count = 0;
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return Utility.ResultType.Error;
            }
        }

        public static Utility.ResultType UpdateForBulkUpload(Document objDocument, DbTransaction objDbTransaction)
        {
            DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

            try
            {
                DbParameter[] objDbParameter = new DbParameter[11];

                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "DocumentID";
                objDbParameter[0].Value = objDocument.DocumentID;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "DocumentName";
                objDbParameter[1].Value = objDocument.DocumentName;

                objDbParameter[2] = objDbProviderFactory.CreateParameter();
                objDbParameter[2].ParameterName = "DocumentGuid";
                objDbParameter[2].Value = objDocument.DocumentGuid;

                objDbParameter[3] = objDbProviderFactory.CreateParameter();
                objDbParameter[3].ParameterName = "DocumentSize";
                objDbParameter[3].Value = objDocument.Size;

                objDbParameter[4] = objDbProviderFactory.CreateParameter();
                objDbParameter[4].ParameterName = "DocumentPath";
                objDbParameter[4].Value = objDocument.DocumentPath;

                objDbParameter[5] = objDbProviderFactory.CreateParameter();
                objDbParameter[5].ParameterName = "DocumentType";
                objDbParameter[5].Value = objDocument.DocumentType;

                objDbParameter[6] = objDbProviderFactory.CreateParameter();
                objDbParameter[6].ParameterName = "Image";
                objDbParameter[6].Value = Utility.FileStorageType == Utility.StorageType.FileSystem ? new byte[0] : objDocument.Image;

                objDbParameter[7] = objDbProviderFactory.CreateParameter();
                objDbParameter[7].ParameterName = "UpdatedOn";
                objDbParameter[7].Value = DateTime.Now;

                objDbParameter[8] = objDbProviderFactory.CreateParameter();
                objDbParameter[8].ParameterName = "UpdatedBy";
                objDbParameter[8].Value = objDocument.UpdatedBy;

                objDbParameter[9] = objDbProviderFactory.CreateParameter();
                objDbParameter[9].ParameterName = "DocumentStatusID";
                objDbParameter[9].Value = objDocument.DocumentStatusID;

                objDbParameter[10] = objDbProviderFactory.CreateParameter();
                objDbParameter[10].ParameterName = "IsLucened";
                objDbParameter[10].Value = objDocument.IsLucened;

                DataHelper.ExecuteNonQuery("SP_U_DOCBULKUPLOAD", objDbTransaction, objDbParameter);

                return Utility.ResultType.Success;
            }
            catch (Exception ex)
            {

                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return Utility.ResultType.Error;
            }
        }

        public static Utility.ResultType UpdateDocumentInsertMetaDataID(Document objDocument, DbTransaction objDbTransaction)
        {
            DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

            try
            {
                DbParameter[] objDbParameter = new DbParameter[2];

                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "MetaDataID";
                objDbParameter[0].Value = objDocument.MetaDataID;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "DocumentID";
                objDbParameter[1].Value = objDocument.DocumentID;


                DataHelper.ExecuteNonQuery("U_DOC_INMETADATAID", objDbTransaction, objDbParameter);

                return Utility.ResultType.Success;
            }
            catch (Exception ex)
            {

                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return Utility.ResultType.Error;
            }
        }

        public static Utility.ResultType SelectUploaded(out DataTable objDataTable, int intMetaDataID, string strDocumentID)
        {
            try
            {
                string strQuery = string.Empty;
                if (strDocumentID == string.Empty)
                    strQuery = "SELECT * FROM DOCUMENT WHERE MetaDataID = @MetaDataID AND Status = 1 AND DocumentStatusID = 3";
                else
                    strQuery = "SELECT * FROM DOCUMENT WHERE MetaDataID = @MetaDataID AND Status = 1 AND ID = @ID";

                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;
                DbParameter[] objDbParameter = null;
                if (strDocumentID == string.Empty)
                {
                    objDbParameter = new DbParameter[1];
                    objDbParameter[0] = objDbProviderFactory.CreateParameter();
                    objDbParameter[0].ParameterName = "MetaDataID";
                    objDbParameter[0].Value = intMetaDataID;
                }
                else
                {
                    objDbParameter = new DbParameter[2];
                    objDbParameter[0] = objDbProviderFactory.CreateParameter();
                    objDbParameter[0].ParameterName = "MetaDataID";
                    objDbParameter[0].Value = intMetaDataID;

                    objDbParameter[1] = objDbProviderFactory.CreateParameter();
                    objDbParameter[1].ParameterName = "ID";
                    objDbParameter[1].Value = Convert.ToInt32(strDocumentID);
                }

                objDataTable = DataHelper.ExecuteDataTable(strQuery, objDbParameter);
                if (objDataTable.Rows.Count == 0)
                {
                    return Utility.ResultType.Failure;
                }
                return Utility.ResultType.Success;
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                objDataTable = null;
                return Utility.ResultType.Error;
            }
        }

        public static Utility.ResultType SelectEntryCompleted(out DataTable objDataTable, int intMetaDataID, string strDocumentID)
        {
            try
            {
                string strQuery = string.Empty;
                if (strDocumentID == string.Empty)
                    strQuery = "SELECT * FROM DOCUMENT WHERE MetaDataID = @MetaDataID AND Status = 1 AND DocumentStatusID = 4";
                else
                    strQuery = "SELECT * FROM DOCUMENT WHERE MetaDataID = @MetaDataID AND Status = 1 AND ID = @ID";

                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

                DbParameter[] objDbParameter = null;
                if (strDocumentID == string.Empty)
                {
                    objDbParameter = new DbParameter[1];
                    objDbParameter[0] = objDbProviderFactory.CreateParameter();
                    objDbParameter[0].ParameterName = "MetaDataID";
                    objDbParameter[0].Value = intMetaDataID;
                }
                else
                {
                    objDbParameter = new DbParameter[2];
                    objDbParameter[0] = objDbProviderFactory.CreateParameter();
                    objDbParameter[0].ParameterName = "MetaDataID";
                    objDbParameter[0].Value = intMetaDataID;

                    objDbParameter[1] = objDbProviderFactory.CreateParameter();
                    objDbParameter[1].ParameterName = "ID";
                    objDbParameter[1].Value = Convert.ToInt32(strDocumentID);
                }

                objDataTable = DataHelper.ExecuteDataTable(strQuery, objDbParameter);
                if (objDataTable.Rows.Count == 0)
                {
                    return Utility.ResultType.Failure;
                }
                return Utility.ResultType.Success;
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                objDataTable = null;
                return Utility.ResultType.Error;
            }
        }

        public static Utility.ResultType SelectApproved(out DataTable objDataTable, int intMetaDataID, string strDocumentID)
        {
            try
            {
                string strQuery = string.Empty;
                if (strDocumentID == string.Empty)
                    strQuery = "SELECT * FROM DOCUMENT WHERE MetaDataID = @MetaDataID AND Status = 1 AND DocumentStatusID = 1";
                else
                    strQuery = "SELECT * FROM DOCUMENT WHERE MetaDataID = @MetaDataID AND Status = 1 AND ID = @ID";

                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

                DbParameter[] objDbParameter = null;
                if (strDocumentID == string.Empty)
                {
                    objDbParameter = new DbParameter[1];
                    objDbParameter[0] = objDbProviderFactory.CreateParameter();
                    objDbParameter[0].ParameterName = "MetaDataID";
                    objDbParameter[0].Value = intMetaDataID;
                }
                else
                {
                    objDbParameter = new DbParameter[2];
                    objDbParameter[0] = objDbProviderFactory.CreateParameter();
                    objDbParameter[0].ParameterName = "MetaDataID";
                    objDbParameter[0].Value = intMetaDataID;

                    objDbParameter[1] = objDbProviderFactory.CreateParameter();
                    objDbParameter[1].ParameterName = "ID";
                    objDbParameter[1].Value = Convert.ToInt32(strDocumentID);
                }

                objDataTable = DataHelper.ExecuteDataTable(strQuery, objDbParameter);
                if (objDataTable.Rows.Count == 0)
                {
                    return Utility.ResultType.Failure;
                }
                return Utility.ResultType.Success;
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                objDataTable = null;
                return Utility.ResultType.Error;
            }
        }

        public static Utility.ResultType SelectRejected(out DataTable objDataTable, int intMetaDataID, string strDocumentID)
        {
            try
            {
                string strQuery = string.Empty;
                if (strDocumentID == string.Empty)
                    strQuery = "SELECT * FROM DOCUMENT WHERE MetaDataID = @MetaDataID AND Status = 1 AND DocumentStatusID = 2";
                else
                    strQuery = "SELECT * FROM DOCUMENT WHERE MetaDataID = @MetaDataID AND Status = 1 AND ID = @ID";

                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

                DbParameter[] objDbParameter = null;
                if (strDocumentID == string.Empty)
                {
                    objDbParameter = new DbParameter[1];
                    objDbParameter[0] = objDbProviderFactory.CreateParameter();
                    objDbParameter[0].ParameterName = "MetaDataID";
                    objDbParameter[0].Value = intMetaDataID;
                }
                else
                {
                    objDbParameter = new DbParameter[2];
                    objDbParameter[0] = objDbProviderFactory.CreateParameter();
                    objDbParameter[0].ParameterName = "MetaDataID";
                    objDbParameter[0].Value = intMetaDataID;

                    objDbParameter[1] = objDbProviderFactory.CreateParameter();
                    objDbParameter[1].ParameterName = "ID";
                    objDbParameter[1].Value = Convert.ToInt32(strDocumentID);
                }

                objDataTable = DataHelper.ExecuteDataTable(strQuery, objDbParameter);
                if (objDataTable.Rows.Count == 0)
                {
                    return Utility.ResultType.Failure;
                }
                return Utility.ResultType.Success;
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                objDataTable = null;
                return Utility.ResultType.Error;
            }
        }

        public static Utility.ResultType SelectAllDocument(out DataTable objDataTable, int intMetaDataID, DocumentManager.Status enumStatus)
        {
            try
            {
                char charQuoute = '"';
                string strQuery = "SELECT ID,MetaDataID,DocumentName," + charQuoute + "Size" + charQuoute + ",DocumentType,PageCount,DocumentStatusID,"
                                  + " (SELECT StatusName FROM DOCUMENTSTATUS WHERE ID=DOCUMENT.DocumentStatusID) AS " + charQuoute + "DocumentStatus" + charQuoute + ""
                                  + " FROM DOCUMENT WHERE MetaDataID=@MetaDataID AND Status=1 ";

                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;
                DbParameter[] objDbParameter = null;
                if (enumStatus == DocumentManager.Status.TotalUploaded)
                {
                    objDbParameter = new DbParameter[1];
                }
                else
                {
                    strQuery = strQuery + " AND DocumentStatusID = @DocumentStatusID";
                    objDbParameter = new DbParameter[2];
                }

                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "MetaDataID";
                objDbParameter[0].Value = intMetaDataID;

                if (enumStatus != DocumentManager.Status.TotalUploaded)
                {
                    objDbParameter[1] = objDbProviderFactory.CreateParameter();
                    objDbParameter[1].ParameterName = "DocumentStatusID";
                    objDbParameter[1].Value = (int)enumStatus;
                }


                objDataTable = DataHelper.ExecuteDataTable(strQuery, objDbParameter);
                if (objDataTable.Rows.Count == 0)
                {
                    return Utility.ResultType.Failure;
                }
                return Utility.ResultType.Success;
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                objDataTable = null;
                return Utility.ResultType.Error;
            }
        }

        public static Utility.ResultType SelectDocument(out DataTable objDataTable, int intMetaDataID)
        {
            try
            {
                char charQuoute = '"';
                string strQuery = "SELECT ID,MetaDataID,DocumentName," + charQuoute + "Size" + charQuoute + ",DocumentType,DocumentStatusID,CreatedOn"
                                  + " (SELECT StatusName FROM DOCUMENTSTATUS WHERE ID=DOCUMENT.DocumentStatusID)AS " + charQuoute + "DocumentStatus" + charQuoute + ""
                                  + " FROM DOCUMENT WHERE MetaDataID=@MetaDataID AND Status=1 AND DocumentStatusID IN (1,2,4)";

                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

                DbParameter[] objDbParameter = new DbParameter[1];
                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "MetaDataID";
                objDbParameter[0].Value = intMetaDataID;

                objDataTable = DataHelper.ExecuteDataTable(strQuery, objDbParameter);
                if (objDataTable.Rows.Count == 0)
                {
                    return Utility.ResultType.Failure;
                }
                return Utility.ResultType.Success;
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                objDataTable = null;
                return Utility.ResultType.Error;
            }
        }

        public static Utility.ResultType SelectDocumentForBulkUpload(Document objDocument)
        {
            try
            {
                string strQuery = "SELECT COUNT(*) FROM DOCUMENT WHERE MetaDataID=@MetaDataID and ID=@ID and DocumentStatusID=5";

                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

                DbParameter[] objDbParameter = new DbParameter[2];
                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "MetaDataID";
                objDbParameter[0].Value = objDocument.MetaDataID;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "ID";
                objDbParameter[1].Value = objDocument.DocumentID;

                object objResult = DataHelper.ExecuteScalar(strQuery, objDbParameter);
                if (objResult == null || int.Parse(objResult.ToString()) == 0)
                {
                    return Utility.ResultType.Failure;
                }
                return Utility.ResultType.Success;
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return Utility.ResultType.Error;
            }
        }

        public static Utility.ResultType Select(out DataTable objDataTable, string strWhereCondition)
        {
            try
            {
                char charQuote = '"';

                string strQuery = "select *"
                + " from "

                + " (SELECT  MetaDataID,DocumentName,DocumentType,Size,Tag,StatusName AS " + charQuote + "DocumentStatus" + charQuote + ",CreatedOn,UpdatedOn,User1.UserName AS  " + charQuote + "CreatedBy" + charQuote + ",User2.UserName  " + charQuote + "UpdatedBy" + charQuote
                + " FROM DOCUMENT Inner Join DOCUMENTSTATUS"
                + " on DOCUMENT.DocumentStatusID=DOCUMENTSTATUS.ID"


                + " Left Outer Join (select ID,UserName from  USER) AS " + charQuote + "User1" + charQuote
                + " on DOCUMENT.CreatedBy=User1.ID"

                + " Left Outer Join (select ID,UserName from  USER) AS " + charQuote + "User2" + charQuote
                + " on DOCUMENT.UpdatedBy=User2.ID"

                + " WHERE MetaDataID In (SELECT ID FROM METADATA Where " + strWhereCondition + " and status=1) AND Status=1) AS " + charQuote + "DocumentStatus" + charQuote

                + " Inner Join "

                + " ( select METADATA.ID,MetaDataCode,RepositoryName,MetaTemplateName,FolderName,CategoryName"
                + " from METADATA"
                + " Left Outer Join REPOSITORY "
                + " on METADATA.RepositoryID= REPOSITORY.ID "

                + " Left Outer Join METATEMPLATE"
                + " on METADATA.MetaTemplateID=METATEMPLATE.ID "

                + " Left Join FOLDER"
                + " on METADATA.FolderID=FOLDER.ID"

                + " Left Join CATEGORY"
                + " on METADATA.CategoryID=CATEGORY.ID"
                + ") as  " + charQuote + "METADATA" + charQuote

                + " on DocumentStatus.MetaDataID=METADATA.ID";


                objDataTable = DataHelper.ExecuteDataTable(strQuery, null);
                if (objDataTable.Rows.Count == 0)
                {
                    return Utility.ResultType.Failure;
                }
                return Utility.ResultType.Success;

            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                objDataTable = null;
                return Utility.ResultType.Error;

            }
        }

        public DataTable SearchDocument(string SearchParameter, string UserName)
        {
            DataTable dt = null;
            string query = "select * from vwDocumentSearch ";
            string whereclause = string.Empty;
            if (SearchType == "SoleId")
            {
                whereclause = "where Sole_ID=" + Convert.ToInt32(SearchParameter) + " and User_Name = '" + UserName + "'";
            }
            else if (SearchType == "Description")
                whereclause = "where Description like '%" + SearchParameter + "%'" + " and User_Name = '" + UserName + "'";

            else if (SearchType == "UserName")
                whereclause = "where User_Name like '%" + SearchParameter + "%'" + " and User_Name = '" + UserName + "'";

            else if (SearchType == "Barcode")
                whereclause = "where Barcode='" + SearchParameter + "'" + " and User_Name = '" + UserName + "'";

            else if (SearchType == "All")
                whereclause = "where User_Name = '" + UserName + "'";

            query = query + whereclause;
            dt = DataHelper.ExecuteDataTable(query, null);
            return dt;
        }

        public static DataTable GetDocumentPath(int documentID)
        {
            try
            {
                char charQuote = '"';
                string strQuery = "SELECT DocumentPath,DOCUMENTSTATUSID,DocumentType,PAGECOUNT,Image,DOCUMENTGUID,MergedPageCount,CreatedOn,DocumentName," + charQuote + "Size" + charQuote + " FROM DOCUMENT WHERE ID = @ID";

                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

                DbParameter[] objDbParameter = new DbParameter[1];

                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "ID";
                objDbParameter[0].Value = documentID;

                return DataHelper.ExecuteDataTable(strQuery, objDbParameter);
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                throw ex;
            }
        }

        public DataTable MostViewedDocuments()
        {
            try
            {
                DataTable dtMostViewedDocuments = DataHelper.ExecuteDataTable("SELECT d.ID,d.METADATAID,d.DOCUMENTSTATUSID,r.RepositoryName,d.DOCUMENTNAME,d.DOCUMENTPATH,d.Count FROM vwRecentlyViwedDocuments d,METADATA m,REPOSITORY r Where d.METADATAID=m.ID and m.RepositoryID=r.ID  and r.ID IN (" + Utility.GetAccessRight(Utility.AccessRight.Repository) + ") " + " AND MetaTemplateID IN (" + Utility.GetAccessRight(Utility.AccessRight.MetaTemplate) + ")" + " AND CategoryID IN (" + Utility.GetAccessRight(Utility.AccessRight.Category) + ",-1) " + " AND FolderID IN (" + Utility.GetAccessRight(Utility.AccessRight.Folder) + ",0)  order by COUNT desc", null);
                return dtMostViewedDocuments;
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                throw ex;
            }
        }

        public DataTable RecentlyAddedDocuments()
        {
            try
            {
                DataTable dtRecentlyAddedDocuments = DataHelper.ExecuteDataTable("Select DocumentName from DOCUMENT d inner join METADATA md on md.ID = d.MetaDataID inner join REPOSITORY r on r.ID = md.RepositoryID where r.ID IN (" + Utility.GetAccessRight(Utility.AccessRight.Repository) + ") " + " AND MetaTemplateID IN (" + Utility.GetAccessRight(Utility.AccessRight.MetaTemplate) + ")" + " AND CategoryID IN (" + Utility.GetAccessRight(Utility.AccessRight.Category) + ",-1) " + " AND FolderID IN (" + Utility.GetAccessRight(Utility.AccessRight.Folder) + ",0) order by d.CreatedOn desc", null);
                return dtRecentlyAddedDocuments;
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                throw ex;
            }
        }

        public DataTable DatewiseDocuments()
        {
            try
            {
                DataTable dtDatewiseDocuments = DataHelper.ExecuteDataTable("select DOCUMENTSTATUS.StatusName,COUNT(*) as TotalCount from DOCUMENT inner join DOCUMENTSTATUS ON DOCUMENT.DocumentStatusID = DOCUMENTSTATUS.ID inner join METADATA on DOCUMENT.MetaDataID=METADATA.ID inner join REPOSITORY r on r.ID = METADATA.RepositoryID where  r.ID IN (" + Utility.GetAccessRight(Utility.AccessRight.Repository) + ") " + " AND MetaTemplateID IN (" + Utility.GetAccessRight(Utility.AccessRight.MetaTemplate) + ")" + " AND CategoryID IN (" + Utility.GetAccessRight(Utility.AccessRight.Category) + ",-1) " + " AND FolderID IN (" + Utility.GetAccessRight(Utility.AccessRight.Folder) + ",0) GROUP BY StatusName", null);
                return dtDatewiseDocuments;
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                throw ex;
            }
        }

        public DataTable FilterDatewiseDocuments(string from, string to)
        {
            try
            {
                DataTable dtDatewiseDocuments = DataHelper.ExecuteDataTable("select DOCUMENTSTATUS.StatusName,COUNT(*) as TotalCount from DOCUMENT inner join DOCUMENTSTATUS ON DOCUMENT.DocumentStatusID = DOCUMENTSTATUS.ID inner join METADATA on DOCUMENT.MetaDataID=METADATA.ID inner join REPOSITORY r on r.ID = METADATA.RepositoryID where  ( dbo.To_date(DOCUMENT.createdon)  between '" + from + "'  and '" + to + "' ) and r.ID IN (" + Utility.GetAccessRight(Utility.AccessRight.Repository) + ") " + " AND MetaTemplateID IN (" + Utility.GetAccessRight(Utility.AccessRight.MetaTemplate) + ")" + " AND CategoryID IN (" + Utility.GetAccessRight(Utility.AccessRight.Category) + ",-1) " + " AND FolderID IN (" + Utility.GetAccessRight(Utility.AccessRight.Folder) + ",0) GROUP BY StatusName", null);
                return dtDatewiseDocuments;
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                throw ex;
            }
        }
        //Added by Vivek for show report on the basis of repository and metatemplate only
        public static Utility.ResultType SelectReportDocumentNew(out DataTable objDataTable, MetaData objMetaData, string FromDate, string ToDate)
        {

          #region Old_InlineQuery

//            try
//            {
//                char charQuote = '"';
//                string strQuery = "";
                
//                if(objMetaData.RepositoryID == 88)
//                {
//                     strQuery = @"SELECT D.ID, R.RepositoryName, MT.MetaTemplateName, C.CategoryName,(select FolderName from Folder where ID=f.ParentFolderID) as ParentFolderName, F.FolderName, D.MetaDataID,DocumentName," + charQuote + "Size" + charQuote + @",DocumentType,D.DocumentStatusID,convert(varchar,D.CreatedOn,101)as CreatedOn,UC.UserName CreatedBy,Tag,PageCount 
//                                            FROM DOCUMENT D LEFT OUTER JOIN METADATA M ON D.MetaDataID = M.ID
//                                            LEFT OUTER JOIN vwuser UC ON D.CreatedBy = UC.ID
//                                            LEFT OUTER JOIN vwuser UU ON D.UpdatedBy = UU.ID
//                                            LEFT OUTER JOIN REPOSITORY R ON R.ID=M.RepositoryID
//                                            LEFT OUTER JOIN METATEMPLATE MT ON MT.ID=M.MetaTemplateID
//                                            LEFT OUTER JOIN CATEGORY C ON C.ID=M.CategoryID
//                                            LEFT OUTER JOIN FOLDER F ON F.ID=M.FolderID WHERE D.Status=1 
//                                            ";
//                }
//                else
//                {
//                     strQuery = @"SELECT D.ID, R.RepositoryName, MT.MetaTemplateName, C.CategoryName, F.FolderName, D.MetaDataID,DocumentName," + charQuote + "Size" + charQuote + @",DocumentType,D.DocumentStatusID,convert(varchar,D.CreatedOn,101)as CreatedOn,UC.UserName CreatedBy,Tag,PageCount 
//                                            FROM DOCUMENT D LEFT OUTER JOIN METADATA M ON D.MetaDataID = M.ID
//                                            LEFT OUTER JOIN vwuser UC ON D.CreatedBy = UC.ID
//                                            LEFT OUTER JOIN vwuser UU ON D.UpdatedBy = UU.ID
//                                            LEFT OUTER JOIN REPOSITORY R ON R.ID=M.RepositoryID
//                                            LEFT OUTER JOIN METATEMPLATE MT ON MT.ID=M.MetaTemplateID
//                                            LEFT OUTER JOIN CATEGORY C ON C.ID=M.CategoryID
//                                            LEFT OUTER JOIN FOLDER F ON F.ID=M.FolderID WHERE D.Status=1 
//                                            ";
//                }
              
//                string strWhereQuery = string.Empty;

//                if (FromDate == "" || ToDate == "" || FromDate == string.Empty || ToDate == string.Empty)
//                {
//                    if (objMetaData.MetaDataID != -1)
//                    {
//                        strWhereQuery = " AND M.ID = " + objMetaData.MetaDataID;
//                    }
//                    else
//                    {
//                        if (objMetaData.CategoryID > 0 && objMetaData.FolderID > 0)
//                        {

//                            strWhereQuery = " AND M.RepositoryID =" + objMetaData.RepositoryID + " AND M.MetaTemplateID =" + objMetaData.MetaTemplateID + " AND M.CategoryID =" + objMetaData.CategoryID + " AND M.FolderID =" + objMetaData.FolderID + "" + " Order by D.CreatedOn";

//                        }
//                        else if (objMetaData.CategoryID > 0 && objMetaData.FolderID <= 0)
//                        {
//                            strWhereQuery = " AND M.RepositoryID =" + objMetaData.RepositoryID + " AND M.MetaTemplateID =" + objMetaData.MetaTemplateID + " AND M.CategoryID =" + objMetaData.CategoryID + "" + " Order by D.CreatedOn";

//                        }
//                        else if(objMetaData.RepositoryID > 0 && objMetaData.MetaTemplateID > 0)
//                        {
//                            strWhereQuery = " AND M.RepositoryID =" + objMetaData.RepositoryID + " AND M.MetaTemplateID =" + objMetaData.MetaTemplateID + "" + " Order by D.CreatedOn";

//                        }
//                        else
//                        {
//                            strWhereQuery = " AND M.RepositoryID =" + objMetaData.RepositoryID + "" + " Order by D.CreatedOn";
//                        }
//                    }

//                }
//                else
//                {
//                    if (objMetaData.MetaDataID != -1)
//                    {
//                        strWhereQuery = " WHERE M.ID = " + objMetaData.MetaDataID + " AND  D.Status=1" + " and (convert(date,D.CreatedOn) between '" + FromDate + "' and '" + ToDate + "')";
//                    }
//                    else
//                    {
//                        if (objMetaData.CategoryID > 0 && objMetaData.FolderID > 0)
//                        {
//                            strWhereQuery = " AND M.RepositoryID =" + objMetaData.RepositoryID + " AND M.MetaTemplateID =" + objMetaData.MetaTemplateID + " AND M.CategoryID =" + objMetaData.CategoryID + " AND M.FolderID =" + objMetaData.FolderID + "" + " AND  D.Status=1" + " and (convert(date,D.CreatedOn) between '" + FromDate + "' and '" + ToDate + "')" + " Order by D.CreatedOn";
//                        }
//                        else if (objMetaData.CategoryID > 0 && objMetaData.FolderID <= 0)
//                        {

//                            strWhereQuery = " AND M.RepositoryID =" + objMetaData.RepositoryID + " AND M.MetaTemplateID =" + objMetaData.MetaTemplateID + " AND M.CategoryID =" + objMetaData.CategoryID + "" + " AND  D.Status=1" + " and (convert(date,D.CreatedOn) between '" + FromDate + "' and '" + ToDate + "')" + " Order by D.CreatedOn";
//                        }
//                        else if (objMetaData.RepositoryID > 0 && objMetaData.MetaTemplateID > 0)
//                        {

//                            strWhereQuery = " AND M.RepositoryID =" + objMetaData.RepositoryID + " AND M.MetaTemplateID =" + objMetaData.MetaTemplateID + "" + " AND  D.Status=1" + " and (convert(date,D.CreatedOn) between '" + FromDate + "' and '" + ToDate + "')" + " Order by D.CreatedOn";
//                        }
//                        else
//                        {
//                            strWhereQuery = " AND M.RepositoryID =" + objMetaData.RepositoryID + "" + " AND  D.Status=1" + " and (convert(date,D.CreatedOn) between '" + FromDate + "' and '" + ToDate + "')" + " Order by D.CreatedOn";
//                        }
//                    }

//                }

                
//                objDataTable = DataHelper.ExecuteDataTable(strQuery + strWhereQuery, null);

//                if (objDataTable.Rows.Count == 0)
//                {
//                    return Utility.ResultType.Failure;
//                }
//                return Utility.ResultType.Success;
//            }
//            catch (Exception ex)
//            {
//                LogManager.ErrorLog(Utility.LogFilePath, ex);
//                objDataTable = null;
//                return Utility.ResultType.Error;
//            }

#endregion

            try
            {
                string FolderIDFinal = "";
                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;
                if (objMetaData.FolderID != 0)
                {
                    string folderids = string.Empty;
                    Utility.FolderHasChild(objMetaData.FolderID, ref folderids);
                    FolderIDFinal = folderids + objMetaData.FolderID;
                }
                else
                {
                    FolderIDFinal = "0";
                }

                DbParameter[] objDbParameter = new DbParameter[7];

                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "RepositoryID";
                objDbParameter[0].Value = objMetaData.RepositoryID;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "MetatemplateID";
                objDbParameter[1].Value = objMetaData.MetaTemplateID;

                objDbParameter[2] = objDbProviderFactory.CreateParameter();
                objDbParameter[2].ParameterName = "CategoryId";
                objDbParameter[2].Value = objMetaData.CategoryID;

                objDbParameter[3] = objDbProviderFactory.CreateParameter();
                objDbParameter[3].ParameterName = "MetaDataID";
                objDbParameter[3].Value = objMetaData.MetaDataID;

                objDbParameter[4] = objDbProviderFactory.CreateParameter();
                objDbParameter[4].ParameterName = "FolderID";
                objDbParameter[4].Value = FolderIDFinal;

                objDbParameter[5] = objDbProviderFactory.CreateParameter();
                objDbParameter[5].ParameterName = "FromDate";
                objDbParameter[5].Value = FromDate;

                objDbParameter[6] = objDbProviderFactory.CreateParameter();
                objDbParameter[6].ParameterName = "ToDate";
                objDbParameter[6].Value = ToDate;

                objDataTable = DataHelper.ExecuteDataTableForProcedure("USP_DocumentDashboard", null, objDbParameter);

                if (objDataTable.Rows.Count == 0)
                {
                    return Utility.ResultType.Failure;
                }
                else
                {
                    return Utility.ResultType.Success;
                }
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                objDataTable = null;
                return Utility.ResultType.Error;
            }


        }
        //added by Sneha for MHADA for showing merging details in dashboard report
        public static Utility.ResultType SelectReportDocumentNewMHADA(out DataTable objDataTable, MetaData objMetaData, string FromDate, string ToDate)
        {
            try
            {
                char charQuote = '"';
                string strQuery = @"SELECT R.RepositoryName, MT.MetaTemplateName, C.CategoryName, F.FolderName, M.MetaDataCode,DocumentName," + charQuote + "Size" + charQuote + @",DocumentType,D.CreatedOn,UC.UserName CreatedBy,D.UpdatedOn,UU.UserName UpdatedBy,Tag,PageCount,D.MergedPageCount,D.updatedon 
                                            FROM DOCUMENT D LEFT OUTER JOIN METADATA M ON D.MetaDataID = M.ID
                                            LEFT OUTER JOIN vwuser UC ON D.CreatedBy = UC.ID
                                            LEFT OUTER JOIN vwuser UU ON D.UpdatedBy = UU.ID
                                            LEFT OUTER JOIN REPOSITORY R ON R.ID=M.RepositoryID
                                            LEFT OUTER JOIN METATEMPLATE MT ON MT.ID=M.MetaTemplateID
                                            LEFT OUTER JOIN CATEGORY C ON C.ID=M.CategoryID
                                            LEFT OUTER JOIN FOLDER F ON F.ID=M.FolderID WHERE D.Status=1 
                                            ";
                string strWhereQuery = string.Empty;

                if (FromDate == "" || ToDate == "" || FromDate == string.Empty || ToDate == string.Empty)
                {
                    if (objMetaData.MetaDataID != -1)
                    {
                        strWhereQuery = " AND M.ID = " + objMetaData.MetaDataID;
                    }
                    else
                    {
                        if (objMetaData.CategoryID > 0 && objMetaData.FolderID > 0)
                        {

                            strWhereQuery = " AND M.RepositoryID =" + objMetaData.RepositoryID + " AND M.MetaTemplateID =" + objMetaData.MetaTemplateID + " AND M.CategoryID =" + objMetaData.CategoryID + " AND M.FolderID =" + objMetaData.FolderID + "";

                        }
                        else if (objMetaData.CategoryID > 0 && objMetaData.FolderID <= 0)
                        {
                            strWhereQuery = " AND M.RepositoryID =" + objMetaData.RepositoryID + " AND M.MetaTemplateID =" + objMetaData.MetaTemplateID + " AND M.CategoryID =" + objMetaData.CategoryID + "";

                        }
                        else
                        {
                            strWhereQuery = " AND M.RepositoryID =" + objMetaData.RepositoryID + " AND M.MetaTemplateID =" + objMetaData.MetaTemplateID + "";

                        }
                    }

                }
                else
                {
                    if (objMetaData.MetaDataID != -1)
                    {
                        strWhereQuery = " WHERE M.ID = " + objMetaData.MetaDataID + " AND  D.Status=1" + " and (convert(date,D.CreatedOn) between '" + FromDate + "' and '" + ToDate + "')";
                    }
                    else
                    {
                        if (objMetaData.CategoryID > 0 && objMetaData.FolderID > 0)
                        {
                            strWhereQuery = " AND M.RepositoryID =" + objMetaData.RepositoryID + " AND M.MetaTemplateID =" + objMetaData.MetaTemplateID + " AND M.CategoryID =" + objMetaData.CategoryID + " AND M.FolderID =" + objMetaData.FolderID + "" + " AND  D.Status=1" + " and (convert(date,D.CreatedOn) between '" + FromDate + "' and '" + ToDate + "')";
                        }
                        else if (objMetaData.CategoryID > 0 && objMetaData.FolderID <= 0)
                        {

                            strWhereQuery = " AND M.RepositoryID =" + objMetaData.RepositoryID + " AND M.MetaTemplateID =" + objMetaData.MetaTemplateID + " AND M.CategoryID =" + objMetaData.CategoryID + "" + " AND  D.Status=1" + " and (convert(date,D.CreatedOn) between '" + FromDate + "' and '" + ToDate + "')";
                        }
                        else
                        {
                            strWhereQuery = " AND M.RepositoryID =" + objMetaData.RepositoryID + " AND M.MetaTemplateID =" + objMetaData.MetaTemplateID + "" + " AND  D.Status=1" + " and (convert(date,D.CreatedOn) between '" + FromDate + "' and '" + ToDate + "')";
                        }
                    }
                }

                objDataTable = DataHelper.ExecuteDataTable(strQuery + strWhereQuery, null);

                if (objDataTable.Rows.Count == 0)
                {
                    return Utility.ResultType.Failure;
                }
                return Utility.ResultType.Success;
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                objDataTable = null;
                return Utility.ResultType.Error;
            }
        }
        public static Utility.ResultType SelectReportDocument(out DataTable objDataTable, MetaData objMetaData)
        {
            try
            {
                char charQuote = '"';
                string strQuery = @"SELECT R.RepositoryName, MT.MetaTemplateName, C.CategoryName, F.FolderName, M.MetaDataCode,DocumentName," + charQuote + "Size" + charQuote + @",DocumentType,D.CreatedOn,UC.UserName CreatedBy,D.UpdatedOn,UU.UserName UpdatedBy,Tag,PageCount 
                                            FROM DOCUMENT D LEFT OUTER JOIN METADATA M ON D.MetaDataID = M.ID
                                            LEFT OUTER JOIN vwuser UC ON D.CreatedBy = UC.ID
                                            LEFT OUTER JOIN vwuser UU ON D.UpdatedBy = UU.ID
                                            LEFT OUTER JOIN REPOSITORY R ON R.ID=M.RepositoryID
                                            LEFT OUTER JOIN METATEMPLATE MT ON MT.ID=M.MetaTemplateID
                                            LEFT OUTER JOIN CATEGORY C ON C.ID=M.CategoryID
                                            LEFT OUTER JOIN FOLDER F ON F.ID=M.FolderID WHERE D.Status=1 
                                            ";


                string strWhereQuery = string.Empty;

                if (objMetaData.MetaDataID != -1)
                {
                    strWhereQuery = " AND M.ID = " + objMetaData.MetaDataID;
                }
                else
                {
                    strWhereQuery = " AND M.RepositoryID =" + objMetaData.RepositoryID + " AND M.MetaTemplateID =" + objMetaData.MetaTemplateID + " AND M.CategoryID =" + objMetaData.CategoryID + " AND M.FolderID =" + objMetaData.FolderID + "";
                }

                objDataTable = DataHelper.ExecuteDataTable(strQuery + strWhereQuery, null);

                if (objDataTable.Rows.Count == 0)
                {
                    return Utility.ResultType.Failure;
                }
                return Utility.ResultType.Success;
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                objDataTable = null;
                return Utility.ResultType.Error;
            }
        }

        //added By Sneha for IDBI Ahemdabad
        public static Utility.ResultType SelectReportDocumentIDBI(out DataTable objDataTable, string FromDate,string ToDate)
        {
            try
            {
                char charQuote = '"';
                string strQuery = @"SELECT R.RepositoryName, MT.MetaTemplateName, C.CategoryName, F.FolderName, M.MetaDataCode,DocumentName," + charQuote + "Size" + charQuote + @",DocumentType,D.CreatedOn,UC.UserName CreatedBy,D.UpdatedOn,UU.UserName UpdatedBy,Tag,PageCount 
                                            FROM DOCUMENT D LEFT OUTER JOIN METADATA M ON D.MetaDataID = M.ID
                                            LEFT OUTER JOIN vwuser UC ON D.CreatedBy = UC.ID
                                            LEFT OUTER JOIN vwuser UU ON D.UpdatedBy = UU.ID
                                            LEFT OUTER JOIN REPOSITORY R ON R.ID=M.RepositoryID
                                            LEFT OUTER JOIN METATEMPLATE MT ON MT.ID=M.MetaTemplateID
                                            LEFT OUTER JOIN CATEGORY C ON C.ID=M.CategoryID
                                            LEFT OUTER JOIN FOLDER F ON F.ID=M.FolderID WHERE D.Status=1 and convert(date,D.createdon)>='"+FromDate+"' and  convert(date,D.createdon)<='"+ToDate+"' and R.id=72 and MT.id=187";
               
                objDataTable = DataHelper.ExecuteDataTable(strQuery , null);

                if (objDataTable.Rows.Count == 0)
                {
                    return Utility.ResultType.Failure;
                }
                return Utility.ResultType.Success;
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                objDataTable = null;
                return Utility.ResultType.Error;
            }
        }
        public static Utility.ResultType InsertArchiveDocs(Document objDocument, DbTransaction objDbTransaction)
        {
            DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;
            try
            {
                DbParameter[] objDbParameter = new DbParameter[7];

                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "DOCUMENTID";
                objDbParameter[0].Value = objDocument.DocumentID;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "V_SIZE";
                objDbParameter[1].Value = objDocument.Size;

                objDbParameter[2] = objDbProviderFactory.CreateParameter();
                objDbParameter[2].ParameterName = "DOCPATH";
                objDbParameter[2].Value = objDocument.DocumentPath;

                objDbParameter[3] = objDbProviderFactory.CreateParameter();
                objDbParameter[3].ParameterName = "MODIFIEDON";
                objDbParameter[3].Value = objDocument.UpdatedOn;

                objDbParameter[4] = objDbProviderFactory.CreateParameter();
                objDbParameter[4].ParameterName = "MODIFIEDBY";
                objDbParameter[4].Value = objDocument.UpdatedBy;

                objDbParameter[5] = objDbProviderFactory.CreateParameter();
                objDbParameter[5].ParameterName = "DOCUMENTGUID";
                objDbParameter[5].Value = objDocument.DocumentGuid;

                objDbParameter[6] = objDbProviderFactory.CreateParameter();
                objDbParameter[6].ParameterName = "VERSIONNO";
                objDbParameter[6].Value = objDocument.VersionNo;


                DataHelper.ExecuteNonQuery("I_ARCHIVEDOCUMENT", objDbTransaction, objDbParameter);

                return Utility.ResultType.Success;
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return Utility.ResultType.Error;
            }
        }

        public static Utility.ResultType UpdateDocumentForMerging(Document objDocument, DbTransaction objDbTransaction)
        {
            DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

            try
            {
                DbParameter[] objDbParameter = new DbParameter[6];

                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "V_DOCUMENTID";
                objDbParameter[0].Value = objDocument.DocumentID;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "V_SIZE";
                objDbParameter[1].Value = objDocument.Size;

                objDbParameter[2] = objDbProviderFactory.CreateParameter();
                objDbParameter[2].ParameterName = "V_OLDPAGECOUNT";
                objDbParameter[2].Value = objDocument.OldPageCount;

                objDbParameter[3] = objDbProviderFactory.CreateParameter();
                //objDbParameter[3].ParameterName = "V_PAGECOUNT";
                objDbParameter[3].ParameterName = "V_MergedPageCount";
                objDbParameter[3].Value = objDocument.MergedPageCount;

                objDbParameter[4] = objDbProviderFactory.CreateParameter();
                objDbParameter[4].ParameterName = "V_UPDATEDON";
                objDbParameter[4].Value = DateTime.Now;

                objDbParameter[5] = objDbProviderFactory.CreateParameter();
                objDbParameter[5].ParameterName = "V_UPDATEDBY";
                objDbParameter[5].Value = objDocument.UpdatedBy;



                DataHelper.ExecuteNonQuery("SP_U_DOCUMENTFORMERGING", objDbTransaction, objDbParameter);

                return Utility.ResultType.Success;
            }
            catch (Exception ex)
            {

                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return Utility.ResultType.Error;
            }
        }
        public static Utility.ResultType SelectAllByMetatemplateID(out DataTable objDataTable, int intUserID)
        {
            try
            {
                char charQuote = '"';
                //string strQuery = "SELECT *" +
                //                    " FROM DOCUMENT Left Outer Join (SELECT Distinct(MetaTemplateID) FROM vwRolePermission WHERE RoleID=@RoleID)  RolePermission" +
                //                    " ON METATEMPLATE.Id=RolePermission.MetaTemplateID" +
                //                    " ORDER BY ID DESC";
                string strQuery = "select * from DOCUMENT where MetaDataID in(select ID from METADATA where MetaTemplateID in (select distinct(MetaTemplateID) from USERPERMISSION where UserID= @UserID))";

                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

                DbParameter[] objDbParameter = new DbParameter[1];
                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "UserID";
                objDbParameter[0].Value = intUserID;

                objDataTable = DataHelper.ExecuteDataTable(strQuery, objDbParameter);

                if (objDataTable.Rows.Count == 0)
                {
                    return Utility.ResultType.Failure;
                }
                return Utility.ResultType.Success;
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                objDataTable = null;
                return Utility.ResultType.Error;
            }
        }
        public static Utility.ResultType InsertDocumentPermission(int DocID, int UserID, int PageNo, string x, string y, string height, string width, DbTransaction objDbTransaction)
        {
            DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

            try
            {
                //string strQuery = "insert into DEEDS_USERDOCUMENTPERMISSION values(@UserID,@DocumentID)";
                DbParameter[] objDbParameter = new DbParameter[7];

                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "UserID";
                objDbParameter[0].Value = UserID;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "DocumentID";
                objDbParameter[1].Value = DocID;

                objDbParameter[2] = objDbProviderFactory.CreateParameter();
                objDbParameter[2].ParameterName = "PageNo";
                objDbParameter[2].Value = PageNo;

                objDbParameter[3] = objDbProviderFactory.CreateParameter();
                objDbParameter[3].ParameterName = "x";
                objDbParameter[3].Value = x;

                objDbParameter[4] = objDbProviderFactory.CreateParameter();
                objDbParameter[4].ParameterName = "y";
                objDbParameter[4].Value = y;

                objDbParameter[5] = objDbProviderFactory.CreateParameter();
                objDbParameter[5].ParameterName = "width";
                objDbParameter[5].Value = width;

                objDbParameter[6] = objDbProviderFactory.CreateParameter();
                objDbParameter[6].ParameterName = "height";
                objDbParameter[6].Value = height;

                //DataHelper.ExecuteNonQuery(strQuery, objDbTransaction, objDbParameter);
                DataHelper.ExecuteNonQuery("DEEDS_SP_I_USERDOCPERMISSION", objDbTransaction, objDbParameter);

                return Utility.ResultType.Success;
            }
            catch (Exception ex)
            {

                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return Utility.ResultType.Error;
            }
        }

        public static Utility.ResultType DeleteDocumentPermission(int DocID, int UserID, int PageNo, DbTransaction objDbTransaction)
        {
            DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

            try
            {
                //string strQuery = "insert into DEEDS_USERDOCUMENTPERMISSION values(@UserID,@DocumentID)";
                DbParameter[] objDbParameter = new DbParameter[3];

                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "UserID";
                objDbParameter[0].Value = UserID;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "DocumentID";
                objDbParameter[1].Value = DocID;

                objDbParameter[2] = objDbProviderFactory.CreateParameter();
                objDbParameter[2].ParameterName = "PageNo";
                objDbParameter[2].Value = PageNo;

                //DataHelper.ExecuteNonQuery(strQuery, objDbTransaction, objDbParameter);
                DataHelper.ExecuteNonQuery("SP_D_USERDOCPERM", objDbTransaction, objDbParameter);

                return Utility.ResultType.Success;
            }
            catch (Exception ex)
            {

                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return Utility.ResultType.Error;
            }
        }

        public static DataSet CheckDocumentPermission(int intUserID, int DocId)
        {
            DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

            try
            {
                string strQuery = "select ID from DEEDS_USERDOCUMENTPERMISSION where UserID=" + intUserID + " and DocumentID=" + DocId + " and PAGENO=0";
                DataSet ds = new DataSet();
                ds = DataHelper.ExecuteDataSet(strQuery);
                return ds;
                //DataHelper.ExecuteNonQuery(strQuery, objDbTransaction, objDbParameter);
                //DataHelper.ExecuteNonQuery("SP_I_UserDocumentPermission", objDbTransaction, objDbParameter);


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataSet CheckDocPagePermission(int intUserID, int DocId, int PageNo)
        {
            DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

            try
            {
                string strQuery;
                if (PageNo == 0)
                    strQuery = "select PAGENO from DEEDS_USERDOCUMENTPERMISSION where UserID=" + intUserID + " and DocumentID=" + DocId + "";
                else
                    strQuery = "select ID from DEEDS_USERDOCUMENTPERMISSION where UserID=" + intUserID + " and DocumentID=" + DocId + " and PAGENO=" + PageNo + "";
                DataSet ds = new DataSet();
                ds = DataHelper.ExecuteDataSet(strQuery);
                return ds;
                //DataHelper.ExecuteNonQuery(strQuery, objDbTransaction, objDbParameter);
                //DataHelper.ExecuteNonQuery("SP_I_UserDocumentPermission", objDbTransaction, objDbParameter);


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static Utility.ResultType SelectDocId(out DataSet ds, string FileNumber)
        {
            try
            {
                string strQuery = "select ID from DOCUMENT where Tag ='" + FileNumber + "'";
                ds = DataHelper.ExecuteDataSet(strQuery);
                //objDataTable = DataHelper.ExecuteDataTable(strQuery, null);
                if (ds.Tables[0].Rows.Count == 0)
                {
                    return Utility.ResultType.Failure;
                }
                return Utility.ResultType.Success;
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                ds = null;
                return Utility.ResultType.Error;
            }
        }
        public static DataSet GetArchiveDocs(int DocId)
        {
            try
            {
                string query = "select DOCUMENT.DocumentName,ARCHIVEDOCUMENT.ModifiedOn,USERLOG.FirstName,ARCHIVEDOCUMENT.ID,ARCHIVEDOCUMENT.DocumentArchivePath from DOCUMENT,ARCHIVEDOCUMENT,USERLOG where DOCUMENT.ID=ARCHIVEDOCUMENT.DocumentID and ARCHIVEDOCUMENT.ModifiedBy=USERLOG.ID and ARCHIVEDOCUMENT.DocumentID='" + DocId + "'";
                DataSet ds = new DataSet();
                ds = DataHelper.ExecuteDataSet(query);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataSet SelectBlockedPages(int DocId, int UserId)
        {
            try
            {
                string query = "select PAGENO from DEEDS_USERDOCUMENTPERMISSION where USERID='" + UserId + "' and DOCUMENTID='" + DocId + "' and X='null'";
                DataSet ds = new DataSet();
                ds = DataHelper.ExecuteDataSet(query);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataSet SelectBlockedPagesArea(int DocId, int UserId)
        {
            try
            {
                string query = "select PAGENO from DEEDS_USERDOCUMENTPERMISSION where USERID='" + UserId + "' and DOCUMENTID='" + DocId + "' and x != 'null'";
                DataSet ds = new DataSet();
                ds = DataHelper.ExecuteDataSet(query);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataSet SelectHideArea(int DocId, int UserId, int PageNo)
        {
            try
            {
                string query = "select X,Y,WIDTH,HEIGHT from DEEDS_USERDOCUMENTPERMISSION  where USERID='" + UserId + "' and DOCUMENTID='" + DocId + "' and PAGENO='" + PageNo + "'";
                DataSet ds = new DataSet();
                ds = DataHelper.ExecuteDataSet(query);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataSet SelectDocMetatemplate(int MetatemplateID)
        {
            try
            {
                string query = @"select DOCUMENT.ID,DOCUMENT.documentname,DOCUMENTENTRY.fielddata from DOCUMENT,DOCUMENTENTRY,METATEMPLATEFIELDS
                                 where DOCUMENT.metadataid in (select id from METADATA where metatemplateid='" + MetatemplateID + "')and DOCUMENT.id=DOCUMENTENTRY.documentid and METATEMPLATEFIELDS.fieldname='TYPE OF FILE' and DOCUMENTENTRY.fieldid=METATEMPLATEFIELDS.id";
                DataSet ds = new DataSet();
                ds = DataHelper.ExecuteDataSet(query);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //get the documents which page count is 0
        public static DataSet SelectDocPageCount()
        {
            try
            {
                DataSet ds = new DataSet();
                string query = @"select * from DOCUMENT where PAGECOUNT=0";
                ds = DataHelper.ExecuteDataSet(query);
                return ds;
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                throw ex;
            }
        }
        public static Utility.ResultType UpdateDocMetadataId(DbTransaction objDbTransaction, int MetatemplateID, int UpdatedBy, int DocumentID, int OldFieldSplCode, int OldFieldTypeOfFile, int OldFieldFrom, int OldFieldTo, int OldFieldFilename, int NewFieldSplCode, int NewFieldTypeOfFile, int NewFieldFrom, int NewFieldTo, int NewFieldFilename, string NewTypeOfFile)
        {
            DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

            try
            {
                //string strQuery = "insert into DEEDS_USERDOCUMENTPERMISSION values(@UserID,@DocumentID)";
                DbParameter[] objDbParameter = new DbParameter[15];

                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "P_METATEMPLATED";
                objDbParameter[0].Value = MetatemplateID;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "P_UpdatedOn";
                objDbParameter[1].Value = DateTime.Now;

                objDbParameter[2] = objDbProviderFactory.CreateParameter();
                objDbParameter[2].ParameterName = "P_UpdatedBy";
                objDbParameter[2].Value = UpdatedBy;

                objDbParameter[3] = objDbProviderFactory.CreateParameter();
                objDbParameter[3].ParameterName = "P_DOCID";
                objDbParameter[3].Value = DocumentID;

                objDbParameter[4] = objDbProviderFactory.CreateParameter();
                objDbParameter[4].ParameterName = "P_OLD_FIELD_SPL_CD";
                objDbParameter[4].Value = OldFieldSplCode;

                objDbParameter[5] = objDbProviderFactory.CreateParameter();
                objDbParameter[5].ParameterName = "P_OLD_FIELD_TYPEOFFILE";
                objDbParameter[5].Value = OldFieldTypeOfFile;

                objDbParameter[6] = objDbProviderFactory.CreateParameter();
                objDbParameter[6].ParameterName = "P_OLD_From";
                objDbParameter[6].Value = OldFieldFrom;

                objDbParameter[7] = objDbProviderFactory.CreateParameter();
                objDbParameter[7].ParameterName = "P_OLD_TO";
                objDbParameter[7].Value = OldFieldTo;

                objDbParameter[8] = objDbProviderFactory.CreateParameter();
                objDbParameter[8].ParameterName = "P_OLD_FILENAME";
                objDbParameter[8].Value = OldFieldFilename;

                objDbParameter[9] = objDbProviderFactory.CreateParameter();
                objDbParameter[9].ParameterName = "P_NEW_FIELD_SPL_CD";
                objDbParameter[9].Value = NewFieldSplCode;

                objDbParameter[10] = objDbProviderFactory.CreateParameter();
                objDbParameter[10].ParameterName = "P_NEW_FIELD_TYPEOFFILE";
                objDbParameter[10].Value = NewFieldTypeOfFile;

                objDbParameter[11] = objDbProviderFactory.CreateParameter();
                objDbParameter[11].ParameterName = "P_NEW_From";
                objDbParameter[11].Value = NewFieldFrom;

                objDbParameter[12] = objDbProviderFactory.CreateParameter();
                objDbParameter[12].ParameterName = "P_NEW_TO";
                objDbParameter[12].Value = NewFieldTo;

                objDbParameter[13] = objDbProviderFactory.CreateParameter();
                objDbParameter[13].ParameterName = "P_NEW_FILENAME";
                objDbParameter[13].Value = NewFieldFilename;

                objDbParameter[14] = objDbProviderFactory.CreateParameter();
                objDbParameter[14].ParameterName = "P_NEW_TYPEOFFILE";
                objDbParameter[14].Value = NewTypeOfFile;

                //DataHelper.ExecuteNonQuery(strQuery, objDbTransaction, objDbParameter);
                DataHelper.ExecuteNonQuery("SP_U_DOC_DEPARTMENT", objDbTransaction, objDbParameter);

                return Utility.ResultType.Success;
            }
            catch (Exception ex)
            {

                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return Utility.ResultType.Error;
            }
        }
        public static Utility.ResultType UpdatePageCount(DbTransaction objDbTransaction, int DocId, int PageCount)
        {
            DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

            try
            {
                //string strQuery = "insert into DEEDS_USERDOCUMENTPERMISSION values(@UserID,@DocumentID)";
                DbParameter[] objDbParameter = new DbParameter[2];

                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "V_DocumentID";
                objDbParameter[0].Value = DocId;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "V_PAGECOUNT";
                objDbParameter[1].Value = PageCount;


                //DataHelper.ExecuteNonQuery(strQuery, objDbTransaction, objDbParameter);
                DataHelper.ExecuteNonQuery("SP_U_DOCPAGECOUNT", objDbTransaction, objDbParameter);

                return Utility.ResultType.Success;
            }
            catch (Exception ex)
            {

                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return Utility.ResultType.Error;
            }
        }
        public static DataSet ShowAllDocs(int MetatemplateId)
        {
            try
            {
                char charQuoute = '"';
                string strQuery = @"select a.id,b.metadatacode,a.documentname,a.'" + charQuoute + "'Size'" + charQuoute + "',a.tag,a.pagecount from DOCUMENT a inner join METADATA b on a.metadataid=b.id inner join metatemplate c on b.metatemplateid=c.id and c.id='" + MetatemplateId + "'";
                DataSet ds = new DataSet();
                ds = DataHelper.ExecuteDataSet(strQuery);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static Utility.ResultType SelectSSLDocument(out DataTable objDataTable, DateTime Dt, DateTime ToDate)
        {
            try
            {
                char charQuote = '"';

                string strQuery = @"select D.DocumentName,D.ID,D.CreatedOn,D.MetadataID,D.DocumentStatusID,D.DocumentPath from Document D inner join MetaData M
                                    on D.MetaDataID=M.ID and M.RepositoryID=47 and CAST(D.CreatedOn as DATE)>='" + Dt + "' and CAST(D.CreatedOn as DATE)<='" + ToDate + "' order by ID desc";

                string strWhereQuery = string.Empty;


                objDataTable = DataHelper.ExecuteDataTable(strQuery + strWhereQuery, null);

                if (objDataTable.Rows.Count == 0)
                {
                    return Utility.ResultType.Failure;
                }
                return Utility.ResultType.Success;
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                objDataTable = null;
                return Utility.ResultType.Error;
            }
        }

        public static DataSet SelectAllDocs(int RepositoryID)
        {
            try
            {
                string strquery = "select * from Document D inner join MetaData M on D.MetaDataID=M.ID and D.Status=1 and M.RepositoryID='" + RepositoryID + "'";
                DataSet ds = new DataSet();
                ds = DataHelper.ExecuteDataSet(strquery);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Utility.ResultType UpdateDocumentName(int DocId, string strDocName, string strDocTag, DbTransaction objDbTransaction)
        {
            try
            {

                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;
                DbParameter[] objDbParameter = new DbParameter[3];
                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "DocId";
                objDbParameter[0].Value = DocId;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "DocumentName";
                objDbParameter[1].Value = strDocName;

                objDbParameter[2] = objDbProviderFactory.CreateParameter();
                objDbParameter[2].ParameterName = "Tag";
                objDbParameter[2].Value = strDocTag;

                DataHelper.ExecuteNonQuery("SP_U_DocumentName", objDbTransaction, objDbParameter);

                return Utility.ResultType.Success;
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return Utility.ResultType.Error;
            }
        }

        public static Utility.ResultType InsertMergedDocumentDetails(Document objDocument, DbTransaction objDbTransaction)
        {
            DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

            try
            {
                DbParameter[] objDbParameter = new DbParameter[9];

                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "DocId";
                objDbParameter[0].Value = objDocument.DocumentID;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "DocumentName";
                objDbParameter[1].Value = objDocument.DocumentName;

                objDbParameter[2] = objDbProviderFactory.CreateParameter();
                objDbParameter[2].ParameterName = "DocumentGuid";
                objDbParameter[2].Value = objDocument.DocumentGuid;

                objDbParameter[3] = objDbProviderFactory.CreateParameter();
                objDbParameter[3].ParameterName = "DocumentPath";
                objDbParameter[3].Value = objDocument.DocumentPath;

               
                objDbParameter[4] = objDbProviderFactory.CreateParameter();
                objDbParameter[4].ParameterName = "CreatedOn";
                objDbParameter[4].Value = objDocument.CreatedOn;

                objDbParameter[5] = objDbProviderFactory.CreateParameter();
                objDbParameter[5].ParameterName = "UpdatedBy";
                objDbParameter[5].Value = objDocument.UpdatedBy;


                objDbParameter[6] = objDbProviderFactory.CreateParameter();
                objDbParameter[6].ParameterName = "MergedPageCount";
                objDbParameter[6].Value = objDocument.MergedPageCount;

                objDbParameter[7] = objDbProviderFactory.CreateParameter();
                objDbParameter[7].ParameterName = "DocumentSize";
                objDbParameter[7].Value = objDocument.Size;

                //objDbParameter[6] = objDbProviderFactory.CreateParameter();
                //objDbParameter[6].ParameterName = "UpdatedOn";
                //objDbParameter[6].Value = objDocument.UpdatedOn;

                objDbParameter[8] = objDbProviderFactory.CreateParameter();
                objDbParameter[8].ParameterName = "IPAddress";
                objDbParameter[8].Value = objDocument.IPAddress;

              
                //objDbParameter[6] = objDbProviderFactory.CreateParameter();
                //objDbParameter[6].ParameterName = "ID";
                //objDbParameter[6].Size = 100;
                //objDbParameter[6].Direction = ParameterDirection.Output;

                //objDbParameter[17] = objDbProviderFactory.CreateParameter();
                //objDbParameter[17].ParameterName = "OLDPAGECOUNT";
                //objDbParameter[17].Value = objDocument.OldPageCount;

                //objDbParameter[18] = objDbProviderFactory.CreateParameter();
                //objDbParameter[18].ParameterName = "NotificationInterval";
                //objDbParameter[18].Value = objDocument.NotificationInterval;


                DataHelper.ExecuteNonQuery("SP_I_MergeDocument", objDbTransaction, objDbParameter);

                //objDocument.ID = Convert.ToInt32(objDbParameter[6].Value);


                return Utility.ResultType.Success;
            }
            catch (Exception ex)
            {

                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return Utility.ResultType.Error;
            }
        }

        public static Utility.ResultType InsertMergedDocumentDetailsTIFF(Document objDocument, DbTransaction objDbTransaction)
        {
            DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

            try
            {
                DbParameter[] objDbParameter = new DbParameter[10];

                //objDbParameter[0] = objDbProviderFactory.CreateParameter();
                //objDbParameter[0].ParameterName = "DocumentID";
                //objDbParameter[0].Value = objDocument.DocumentID;
                ///Changes done for @DOCID

                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "DocId";
                objDbParameter[0].Value = objDocument.DocumentID;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "DocumentName";
                objDbParameter[1].Value = objDocument.DocumentName;

                objDbParameter[2] = objDbProviderFactory.CreateParameter();
                objDbParameter[2].ParameterName = "DocumentGuid";
                objDbParameter[2].Value = objDocument.DocumentGuid;

                objDbParameter[3] = objDbProviderFactory.CreateParameter();
                objDbParameter[3].ParameterName = "DocumentPath";
                objDbParameter[3].Value = objDocument.DocumentPath;

                objDbParameter[4] = objDbProviderFactory.CreateParameter();
                objDbParameter[4].ParameterName = "MergedBy";
                objDbParameter[4].Value = objDocument.MergedBy;

                objDbParameter[5] = objDbProviderFactory.CreateParameter();
                objDbParameter[5].ParameterName = "MergedPageCount";
                objDbParameter[5].Value = objDocument.MergedPageCount;

                objDbParameter[6] = objDbProviderFactory.CreateParameter();
                objDbParameter[6].ParameterName = "IPAddress";
                objDbParameter[6].Value = objDocument.IPAddress;


                objDbParameter[7] = objDbProviderFactory.CreateParameter();
                objDbParameter[7].ParameterName = "UpdatedBy";
                objDbParameter[7].Value = objDocument.UpdatedBy;

                objDbParameter[8] = objDbProviderFactory.CreateParameter();
                objDbParameter[8].ParameterName = "UpdatedOn";
                objDbParameter[8].Value = objDocument.UpdatedOn;


                objDbParameter[9] = objDbProviderFactory.CreateParameter();
                objDbParameter[9].ParameterName = "DocumentSize";
                objDbParameter[9].Value = objDocument.Size;



                //objDbParameter[17] = objDbProviderFactory.CreateParameter();
                //objDbParameter[17].ParameterName = "OLDPAGECOUNT";
                //objDbParameter[17].Value = objDocument.OldPageCount;


                //objDbParameter[18] = objDbProviderFactory.CreateParameter();
                //objDbParameter[18].ParameterName = "NotificationInterval";
                //objDbParameter[18].Value = objDocument.NotificationInterval;


                DataHelper.ExecuteNonQuery("SP_I_MergeDocumentTIFF", objDbTransaction, objDbParameter);

                // objDocument.ID = Convert.ToInt32(objDbParameter[6].Value);


                return Utility.ResultType.Success;
            }
            catch (Exception ex)
            {

                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return Utility.ResultType.Error;
            }
        }

        #endregion


        //        public static DataSet GetArchiveDocs(int DocId)
        //        {
        //            try
        //            {
        //                string query = "select Document.DocumentName,ArchiveDocument.ModifiedOn,UserLog.FirstName,ArchiveDocument.ID,ArchiveDocument.DocumentArchivePath from Document,ArchiveDocument,UserLog where Document.ID=ArchiveDocument.DocumentID and ArchiveDocument.ModifiedBy=UserLog.ID and ArchiveDocument.DocumentID='" + DocId + "'";
        //                DataSet ds = new DataSet();
        //                ds = DataHelper.ExecuteDataSet(query);
        //                return ds;
        //            }
        //            catch (Exception ex)
        //            {
        //                throw ex;
        //            }
        //        }

        //        public static DataSet SelectBlockedPages(int DocId,int UserId)
        //        {
        //            try
        //            {
        //                string query = "select PAGENO from USERDOCUMENTPERMISSION where USERID='"+UserId+"' and DOCUMENTID='"+DocId+"' and X='null'";
        //                DataSet ds = new DataSet();
        //                ds = DataHelper.ExecuteDataSet(query);
        //                return ds;
        //            }
        //            catch(Exception ex)
        //            {
        //                throw ex;
        //            }
        //        }

        //        public static DataSet SelectBlockedPagesArea(int DocId,int UserId)
        //        {
        //            try
        //            {
        //                string query = "select PAGENO from USERDOCUMENTPERMISSION where USERID='" + UserId + "' and DOCUMENTID='" + DocId + "' and x != 'null'";
        //                DataSet ds = new DataSet();
        //                ds = DataHelper.ExecuteDataSet(query);
        //                return ds;
        //            }
        //            catch(Exception ex)
        //            {
        //                throw ex;
        //            }
        //        }
        //        public static DataSet SelectHideArea(int DocId,int UserId, int PageNo)
        //        {
        //            try
        //            {
        //                string query = "select X,Y,WIDTH,HEIGHT from USERDOCUMENTPERMISSION  where USERID='" + UserId + "' and DOCUMENTID='" + DocId + "' and PAGENO='" + PageNo + "'";
        //                DataSet ds = new DataSet();
        //                ds = DataHelper.ExecuteDataSet(query);
        //                return ds;
        //            }
        //            catch(Exception ex)
        //            {
        //                throw ex;
        //            }
        //        }

        //        public static DataSet SelectDocMetatemplate(int MetatemplateID)
        //        {
        //            try
        //            {
        //                string query = @"select document.ID,document.documentname,documententry.fielddata from document,documententry,metatemplatefields
        //                                 where document.metadataid in (select id from metadata where metatemplateid='"+MetatemplateID+"')and document.id=documententry.documentid and metatemplatefields.fieldname='TYPE OF FILE' and documententry.fieldid=metatemplatefields.id";
        //                DataSet ds = new DataSet();
        //                ds = DataHelper.ExecuteDataSet(query);
        //                return ds;
        //            }
        //            catch(Exception ex)
        //            {
        //                throw ex;
        //            }
        //        }
        //        //get the documents which page count is 0
        //        public static DataSet SelectDocPageCount()
        //        {
        //            try
        //            {
        //                DataSet ds = new DataSet();
        //                string query = @"select * from DOCUMENT where PAGECOUNT=0";
        //                ds = DataHelper.ExecuteDataSet(query);
        //                return ds;
        //            }
        //            catch(Exception ex)
        //            {
        //                LogManager.ErrorLog(Utility.LogFilePath, ex);
        //                throw ex;
        //            }
        //        }
        //        public static Utility.ResultType UpdateDocMetadataId(DbTransaction objDbTransaction, int MetatemplateID, int UpdatedBy, int DocumentID, int OldFieldSplCode, int OldFieldTypeOfFile, int OldFieldFrom, int OldFieldTo, int OldFieldFilename, int NewFieldSplCode, int NewFieldTypeOfFile, int NewFieldFrom, int NewFieldTo, int NewFieldFilename,string NewTypeOfFile)
        //        {
        //            DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

        //            try
        //            {
        //                //string strQuery = "insert into UserDocumentPermission values(@UserID,@DocumentID)";
        //                DbParameter[] objDbParameter = new DbParameter[15];

        //                objDbParameter[0] = objDbProviderFactory.CreateParameter();
        //                objDbParameter[0].ParameterName = "P_METATEMPLATED";
        //                objDbParameter[0].Value = MetatemplateID;

        //                objDbParameter[1] = objDbProviderFactory.CreateParameter();
        //                objDbParameter[1].ParameterName = "P_UpdatedOn";
        //                objDbParameter[1].Value = DateTime.Now;

        //                objDbParameter[2] = objDbProviderFactory.CreateParameter();
        //                objDbParameter[2].ParameterName = "P_UpdatedBy";
        //                objDbParameter[2].Value = UpdatedBy;

        //                objDbParameter[3] = objDbProviderFactory.CreateParameter();
        //                objDbParameter[3].ParameterName = "P_DOCID";
        //                objDbParameter[3].Value = DocumentID;

        //                objDbParameter[4] = objDbProviderFactory.CreateParameter();
        //                objDbParameter[4].ParameterName = "P_OLD_FIELD_SPL_CD";
        //                objDbParameter[4].Value = OldFieldSplCode;

        //                objDbParameter[5] = objDbProviderFactory.CreateParameter();
        //                objDbParameter[5].ParameterName = "P_OLD_FIELD_TYPEOFFILE";
        //                objDbParameter[5].Value = OldFieldTypeOfFile;

        //                objDbParameter[6] = objDbProviderFactory.CreateParameter();
        //                objDbParameter[6].ParameterName = "P_OLD_From";
        //                objDbParameter[6].Value = OldFieldFrom;

        //                objDbParameter[7] = objDbProviderFactory.CreateParameter();
        //                objDbParameter[7].ParameterName = "P_OLD_TO";
        //                objDbParameter[7].Value = OldFieldTo;

        //                objDbParameter[8] = objDbProviderFactory.CreateParameter();
        //                objDbParameter[8].ParameterName = "P_OLD_FILENAME";
        //                objDbParameter[8].Value = OldFieldFilename;

        //                objDbParameter[9] = objDbProviderFactory.CreateParameter();
        //                objDbParameter[9].ParameterName = "P_NEW_FIELD_SPL_CD";
        //                objDbParameter[9].Value = NewFieldSplCode;

        //                objDbParameter[10] = objDbProviderFactory.CreateParameter();
        //                objDbParameter[10].ParameterName = "P_NEW_FIELD_TYPEOFFILE";
        //                objDbParameter[10].Value = NewFieldTypeOfFile;

        //                objDbParameter[11] = objDbProviderFactory.CreateParameter();
        //                objDbParameter[11].ParameterName = "P_NEW_From";
        //                objDbParameter[11].Value = NewFieldFrom;

        //                objDbParameter[12] = objDbProviderFactory.CreateParameter();
        //                objDbParameter[12].ParameterName = "P_NEW_TO";
        //                objDbParameter[12].Value = NewFieldTo;

        //                objDbParameter[13] = objDbProviderFactory.CreateParameter();
        //                objDbParameter[13].ParameterName = "P_NEW_FILENAME";
        //                objDbParameter[13].Value = NewFieldFilename;

        //                objDbParameter[14] = objDbProviderFactory.CreateParameter();
        //                objDbParameter[14].ParameterName = "P_NEW_TYPEOFFILE";
        //                objDbParameter[14].Value = NewTypeOfFile;

        //                //DataHelper.ExecuteNonQuery(strQuery, objDbTransaction, objDbParameter);
        //                DataHelper.ExecuteNonQuery("SP_U_DOCUMENT_DEPARTMENT", objDbTransaction, objDbParameter);

        //                return Utility.ResultType.Success;
        //            }
        //            catch (Exception ex)
        //            {

        //                LogManager.ErrorLog(Utility.LogFilePath, ex);
        //                return Utility.ResultType.Error;
        //            }
        //        }
        //        public static Utility.ResultType UpdatePageCount(DbTransaction objDbTransaction, int DocId,int PageCount)
        //        {
        //            DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

        //            try
        //            {
        //                //string strQuery = "insert into UserDocumentPermission values(@UserID,@DocumentID)";
        //                DbParameter[] objDbParameter = new DbParameter[2];

        //                objDbParameter[0] = objDbProviderFactory.CreateParameter();
        //                objDbParameter[0].ParameterName = "V_DocumentID";
        //                objDbParameter[0].Value = DocId;

        //                objDbParameter[1] = objDbProviderFactory.CreateParameter();
        //                objDbParameter[1].ParameterName = "V_PAGECOUNT";
        //                objDbParameter[1].Value = PageCount;


        //                //DataHelper.ExecuteNonQuery(strQuery, objDbTransaction, objDbParameter);
        //                DataHelper.ExecuteNonQuery("SP_U_DOCUMENTPAGECOUNT", objDbTransaction, objDbParameter);

        //                return Utility.ResultType.Success;
        //            }
        //            catch (Exception ex)
        //            {

        //                LogManager.ErrorLog(Utility.LogFilePath, ex);
        //                return Utility.ResultType.Error;
        //            }
        //        }
        //        public static DataSet ShowAllDocs(int MetatemplateId)
        //        {
        //            try
        //            {
        //                char charQuoute = '"';
        //                string strQuery = @"select a.id,b.metadatacode,a.documentname,a.'"+charQuoute+"'Size'"+charQuoute+"',a.tag,a.pagecount from document a inner join metadata b on a.metadataid=b.id inner join metatemplate c on b.metatemplateid=c.id and c.id='"+MetatemplateId+"'";
        //                DataSet ds = new DataSet();
        //                ds = DataHelper.ExecuteDataSet(strQuery);
        //                return ds;
        //            }
        //            catch(Exception ex)
        //            {
        //                throw ex;
        //            }
        //        }

        public static Utility.ResultType SelectDocumentForDelete(int Repository, int MetaTemplate, int Category, int Folder, out DataTable objDataTable)
        {
            try
            {
                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;
                DbParameter[] objDbParameter = new DbParameter[4];
                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "Repository";

                if (Repository == -1)
                {
                    objDbParameter[0].Value = null;
                }
                else
                {
                    objDbParameter[0].Value = Repository;
                }

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "Metatemplate";
                if (MetaTemplate == -1)
                {
                    objDbParameter[1].Value = null;
                }
                else
                {
                    objDbParameter[1].Value = MetaTemplate;
                }

                objDbParameter[2] = objDbProviderFactory.CreateParameter();
                objDbParameter[2].ParameterName = "Category";
                if (Category == -1)
                {
                    objDbParameter[2].Value = null;
                }
                else
                {
                    objDbParameter[2].Value = Category;
                }

                objDbParameter[3] = objDbProviderFactory.CreateParameter();
                objDbParameter[3].ParameterName = "Folder";
                if (Folder == 0)
                {
                    objDbParameter[3].Value = null;
                }
                else
                {
                    objDbParameter[3].Value = Folder;
                }

                objDataTable = DataHelper.ExecuteDataTableForProcedure("SP_S_DocumentForDelete", null, objDbParameter);

                if (objDataTable.Rows.Count == 0)
                {
                    return Utility.ResultType.Failure;
                }
                return Utility.ResultType.Success;
            }
            catch (Exception ex)
            {

                LogManager.ErrorLog(Utility.LogFilePath, ex);
                objDataTable = null;
                return Utility.ResultType.Error;
            }
        }

        #region Seema 7Dec 2017

        public static DataSet CheckDocument_Reliance(string Docname, int RepId, int MetId, int CatId)
        {
            try
            {

                //string strQuery = "select ID from DOCUMENT where DocumentName = '" + Docname + "' and Status=1";
                string strQuery = @"select * from vwDocument D inner join MetaData M on D.MetaDataID=M.ID where D.DocumentName = '" + Docname + "' and M.MetaTemplateID='" + MetId + "' and M.RepositoryID='" + RepId + "' and M.CategoryID='" + CatId + "' and D.Status=1";
                DataSet ds = new DataSet();

                ds = DataHelper.ExecuteDataSet(strQuery);

                return ds;
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return null;
            }
        }

#endregion

        #region Seema 16 July 2018

        public static DataSet CheckDocument_SBIMutual(string Docname, int RepId, int MetId, int Folderid)
        {
            try
            {

                //string strQuery = "select ID from DOCUMENT where DocumentName = '" + Docname + "' and Status=1";
                string strQuery = @"select * from vwDocument D inner join MetaData M on D.MetaDataID=M.ID where D.DocumentName = '" + Docname + "' and M.MetaTemplateID='" + MetId + "' and M.RepositoryID='" + RepId + "' and M.FolderID='" + Folderid + "' and D.Status=1";
                DataSet ds = new DataSet();

                ds = DataHelper.ExecuteDataSet(strQuery);

                return ds;
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return null;
            }
        }

        #endregion

        #region Sneha 8 Sept 2018

        public static DataSet CheckDocument_IDBI(string Docname, int MetId,int RepId)
        {
            try
            {

                //string strQuery = "select ID from DOCUMENT where DocumentName = '" + Docname + "' and Status=1";
                string strQuery = @"select * from vwDocument D inner join MetaData M on D.MetaDataID=M.ID where D.DocumentName = '" + Docname + "' and M.MetaTemplateID='" + MetId + "' and M.RepositoryID='" + RepId + "' and D.Status=1";
                DataSet ds = new DataSet();

                ds = DataHelper.ExecuteDataSet(strQuery);

                return ds;
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return null;
            }
        }

        #endregion

        #region Sneha 16 Jan 2019

        public static Utility.ResultType InsertErrorDoc(string MetatemplateName, string DocumentName,string ErrorType, DbTransaction objDbTransaction)
        {
            DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

            try
            {
                DbParameter[] objDbParameter = new DbParameter[3];

                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "MetatemplateName";
                objDbParameter[0].Value = DocumentName;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "DocName";
                objDbParameter[1].Value = DocumentName;

                objDbParameter[2] = objDbProviderFactory.CreateParameter();
                objDbParameter[2].ParameterName = "ErrorType";
                objDbParameter[2].Value = ErrorType;

                DataHelper.ExecuteNonQuery("SP_I_Doc_Error_IDBI_Ahm", objDbTransaction, objDbParameter);

               // objDocument.DocumentID = Convert.ToInt32(objDbParameter[13].Value);


                return Utility.ResultType.Success;
            }
            catch (Exception ex)
            {

                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return Utility.ResultType.Error;
            }
        }

        #endregion

        public static DataTable SelectFileSize(out DataTable objDataTable,int RepoID)
        {
            try
            {
                string strQuery = "select Sum(cast(d.Size as bigint)) as Size  from Document d join MetaData m on m.ID = d.MetaDataID join Repository R  on m.RepositoryID = r.ID where R.ID = "+RepoID+" and d.Status=1";
                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;
                objDataTable = DataHelper.ExecuteDataTable(strQuery);
                if (objDataTable.Rows.Count == 0)
                {
                    return objDataTable;
                }
                return objDataTable;
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                objDataTable = null;
                return objDataTable = null;
            }
        }


        #region Sneha 17 Jan 2019

        public static Utility.ResultType InsertExcelEntry_IDBI_Ahm(int DocumentId, int MetadataId, DateTime DateOfRecievalAtRpu, DateTime DateOfAccOpening, string Custid, string AccNo, string BoxNo, string DocNo, string SHIL_Barcode, string SHCIL_Barcode_Date, string CloseFlag,string ExcelName, DbTransaction objDbTransaction)
        {
            DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

            try
            {
                DbParameter[] objDbParameter = new DbParameter[12];

                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "DocumentId";
                objDbParameter[0].Value = DocumentId;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "MetadataId";
                objDbParameter[1].Value = MetadataId;

                objDbParameter[2] = objDbProviderFactory.CreateParameter();
                objDbParameter[2].ParameterName = "DateOfRecievalAtRpu";
                objDbParameter[2].Value = DateOfRecievalAtRpu;

                objDbParameter[3] = objDbProviderFactory.CreateParameter();
                objDbParameter[3].ParameterName = "DateOfAccOpening";
                objDbParameter[3].Value = DateOfAccOpening;

                objDbParameter[4] = objDbProviderFactory.CreateParameter();
                objDbParameter[4].ParameterName = "Custid";
                objDbParameter[4].Value = Custid;

                objDbParameter[5] = objDbProviderFactory.CreateParameter();
                objDbParameter[5].ParameterName = "AccNo";
                objDbParameter[5].Value = AccNo;

                objDbParameter[6] = objDbProviderFactory.CreateParameter();
                objDbParameter[6].ParameterName = "BoxNo";
                objDbParameter[6].Value = BoxNo;

                objDbParameter[7] = objDbProviderFactory.CreateParameter();
                objDbParameter[7].ParameterName = "DocNo";
                objDbParameter[7].Value = DocNo;

                objDbParameter[8] = objDbProviderFactory.CreateParameter();
                objDbParameter[8].ParameterName = "SHCIL_Barcode";
                objDbParameter[8].Value = SHIL_Barcode;

                objDbParameter[9] = objDbProviderFactory.CreateParameter();
                objDbParameter[9].ParameterName = "SHCIL_Barcode_Date";
                objDbParameter[9].Value = SHCIL_Barcode_Date;

                objDbParameter[10] = objDbProviderFactory.CreateParameter();
                objDbParameter[10].ParameterName = "Close_Flag";
                objDbParameter[10].Value = CloseFlag;

                objDbParameter[11] = objDbProviderFactory.CreateParameter();
                objDbParameter[11].ParameterName = "ExcelName";
                objDbParameter[11].Value = ExcelName;

                DataHelper.ExecuteNonQuery("SP_I_ExcelEntry_IDBI_Ahm", objDbTransaction, objDbParameter);

                // objDocument.DocumentID = Convert.ToInt32(objDbParameter[13].Value);


                return Utility.ResultType.Success;
            }
            catch (Exception ex)
            {

                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return Utility.ResultType.Error;
            }
        }

        public static DataTable SelectDocumentID(string DocName,int RepId)
        {
            try
            {
                DataTable objDataTable = new DataTable();
                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;
                DbParameter[] objDbParameter = new DbParameter[2];
                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "DocumentName";
                objDbParameter[0].Value = DocName;
                

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "RepositoryId";
                objDbParameter[1].Value = RepId;


                objDataTable = DataHelper.ExecuteDataTableForProcedure("SP_S_DocumentId", null, objDbParameter);

                if (objDataTable.Rows.Count == 0)
                {
                    return null;
                }
                else
                {
                    return objDataTable;
                }
            }
            catch (Exception ex)
            {

                LogManager.ErrorLog(Utility.LogFilePath, ex);
                //objDataTable = null;
                return null;
            }
        }
        #endregion

        #region Sneha 18 Jan 2019
        public static DataTable ExistsInExcelEntry(int DocId)
        {
            try
            {
                DataTable objDataTable = new DataTable();
                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;
                DbParameter[] objDbParameter = new DbParameter[1];
                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "DocId";
                objDbParameter[0].Value = DocId;


                objDataTable = DataHelper.ExecuteDataTableForProcedure("SP_S_DocumentIdInExcel", null, objDbParameter);

                if (objDataTable.Rows.Count == 0)
                {
                    return null;
                }
                else
                {
                    return objDataTable;
                }
            }
            catch (Exception ex)
            {

                LogManager.ErrorLog(Utility.LogFilePath, ex);
                //objDataTable = null;
                return null;
            }
        }

        public static Utility.ResultType InsertExcelEntryError_IDBI_Ahm(int DocumentId, int MetadataId, string ErrorName,DateTime DateOfRecievalAtRpu, DateTime DateOfAccOpening, string Custid, string AccNo, string BoxNo, string DocNo, string SHIL_Barcode, string SHCIL_Barcode_Date, string CloseFlag, string ExcelName, DbTransaction objDbTransaction)
        {
            DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

            try
            {
                DbParameter[] objDbParameter = new DbParameter[13];

                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "DocumentId";
                objDbParameter[0].Value = DocumentId;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "MetadataId";
                objDbParameter[1].Value = MetadataId;

                objDbParameter[2] = objDbProviderFactory.CreateParameter();
                objDbParameter[2].ParameterName = "ErrorName";
                objDbParameter[2].Value = ErrorName;

                objDbParameter[3] = objDbProviderFactory.CreateParameter();
                objDbParameter[3].ParameterName = "DateOfRecievalAtRpu";
                objDbParameter[3].Value = DateOfRecievalAtRpu;

                objDbParameter[4] = objDbProviderFactory.CreateParameter();
                objDbParameter[4].ParameterName = "DateOfAccOpening";
                objDbParameter[4].Value = DateOfAccOpening;

                objDbParameter[5] = objDbProviderFactory.CreateParameter();
                objDbParameter[5].ParameterName = "Custid";
                objDbParameter[5].Value = Custid;

                objDbParameter[6] = objDbProviderFactory.CreateParameter();
                objDbParameter[6].ParameterName = "AccNo";
                objDbParameter[6].Value = AccNo;

                objDbParameter[7] = objDbProviderFactory.CreateParameter();
                objDbParameter[7].ParameterName = "BoxNo";
                objDbParameter[7].Value = BoxNo;

                objDbParameter[8] = objDbProviderFactory.CreateParameter();
                objDbParameter[8].ParameterName = "DocNo";
                objDbParameter[8].Value = DocNo;

                objDbParameter[9] = objDbProviderFactory.CreateParameter();
                objDbParameter[9].ParameterName = "SHCIL_Barcode";
                objDbParameter[9].Value = SHIL_Barcode;

                objDbParameter[10] = objDbProviderFactory.CreateParameter();
                objDbParameter[10].ParameterName = "SHCIL_Barcode_Date";
                objDbParameter[10].Value = SHCIL_Barcode_Date;

                objDbParameter[11] = objDbProviderFactory.CreateParameter();
                objDbParameter[11].ParameterName = "Close_Flag";
                objDbParameter[11].Value = CloseFlag;

                objDbParameter[12] = objDbProviderFactory.CreateParameter();
                objDbParameter[12].ParameterName = "ExcelName";
                objDbParameter[12].Value = ExcelName;

                DataHelper.ExecuteNonQuery("SP_I_ExcelEntry_Error_IDBI_Ahm", objDbTransaction, objDbParameter);

                // objDocument.DocumentID = Convert.ToInt32(objDbParameter[13].Value);


                return Utility.ResultType.Success;
            }
            catch (Exception ex)
            {

                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return Utility.ResultType.Error;
            }
        }
        #endregion

        #region sneha 20 FEb 2018\
        public static DataSet SelectDocForContentSearch(int RepId, int MetId, int CatId,int foldId)
        {
            try
            {
                string strQuery = @"select D.DocumentPath,D.ID
                                    from Document D inner join MetaData M
                                    on D.MetaDataID=M.ID and M.RepositoryID='" + RepId + "' and M.MetaTemplateID='" + MetId + "' and M.CategoryID='" + CatId + "'and M.FolderID='"+foldId+"'";
                DataSet ds = new DataSet();

                ds = DataHelper.ExecuteDataSet(strQuery);

                return ds;
            }
            catch(Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return null;
            }
        }

        public static DataSet SelectDocDetailsForContentSearch(string DocId)
        {
            try
            {
                string strQuery = @"select D.id,D.documentname,D.Size,D.DocumentType,D.DocumentStatusID,D.Tag,D.MetaDataID,DS.StatusName as DocumentStatus,M.MetaDataCode
                                    from Document D inner join DocumentStatus DS 
                                    on D.DocumentStatusID=DS.ID
                                    inner join MetaData M
                                    on D.MetaDataID=M.ID
                                    where D.ID in("+DocId+")";
                DataSet ds = new DataSet();

                ds = DataHelper.ExecuteDataSet(strQuery);

                return ds;
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return null;
            }
        }

        #endregion

        #region Sneha 25 Jan 2019
        public static DataTable GetErrorDocs()
        {
            try
            {
                DataTable objDataTable = new DataTable();
                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;
                DbParameter[] objDbParameter = new DbParameter[0];


                objDataTable = DataHelper.ExecuteDataTableForProcedure("SP_S_ErrorDocs", null, objDbParameter);

                if (objDataTable.Rows.Count == 0)
                {
                    return null;
                }
                else
                {
                    return objDataTable;
                }
            }
            catch(Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return null;
            }
        }

        public static DataTable GetMissingEntryDocs()
        {
            try
            {
                DataTable objDataTable = new DataTable();
                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;
                DbParameter[] objDbParameter = new DbParameter[0];


                objDataTable = DataHelper.ExecuteDataTableForProcedure("SP_S_MissingEntryDocs", null, objDbParameter);

                if (objDataTable.Rows.Count == 0)
                {
                    return null;
                }
                else
                {
                    return objDataTable;
                }
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return null;
            }
        }
        #endregion

        #region Sneha 06 Feb 2019
        public static Utility.ResultType InsertIDBIDownloadLog(int DocId, DbTransaction objDbTransaction)
        {
            DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

            try
            {
                DbParameter[] objDbParameter = new DbParameter[1];

                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "DocId";
                objDbParameter[0].Value = DocId;

                DataHelper.ExecuteNonQuery("SP_I_IDBI_Download_Log", objDbTransaction, objDbParameter);

               // objDocument.DocumentID = Convert.ToInt32(objDbParameter[13].Value);


                return Utility.ResultType.Success;
            }
            catch (Exception ex)
            {

                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return Utility.ResultType.Error;
            }
        }

        public static DataTable GetIDBIDownloadCount()
        {
            try
            {
                DataTable objDataTable = new DataTable();
                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;
                DbParameter[] objDbParameter = new DbParameter[0];


                objDataTable = DataHelper.ExecuteDataTableForProcedure("SP_S_IDBI_Download_Log", null, objDbParameter);

                if (objDataTable.Rows.Count == 0)
                {
                    return null;
                }
                else
                {
                    return objDataTable;
                }
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return null;
            }
        }
        #endregion

        #region Sneha 26 Feb 2019
        public static DataTable SelectErrorDocs(string ErrorType)
        {
            try
            {
            
                DataTable dt = new DataTable();
                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;
                DbParameter[] objDbParameter = new DbParameter[1];
                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "ErrorType";

                dt = DataHelper.ExecuteDataTableForProcedure("SP_S_Doc_Error_IDBI_Ahm", null, objDbParameter);
                return dt;
                
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return null;
            }
        }
        public static DataTable CheckExcelEntry(string DocName)
        {
            try
            {

                DataTable dt = new DataTable();
                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;
                DbParameter[] objDbParameter = new DbParameter[1];
                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "DocName";
                objDbParameter[0].Value = DocName;

                dt = DataHelper.ExecuteDataTableForProcedure("SP_S_ExcelEntry_IDBI_Ahm", null, objDbParameter);
                return dt;

            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return null;
            }
        }

        #endregion

        public static Utility.ResultType DocumentDelete(int docid,int userid)
        {
            DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;
            DbTransaction objDbTransaction = Utility.GetTransaction;

            try
            {

                DbParameter[] objDbParameter = new DbParameter[3];

                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "docid";
                objDbParameter[0].Value = docid;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "deletedby";
                objDbParameter[1].Value = userid;

                objDbParameter[2] = objDbProviderFactory.CreateParameter();
                objDbParameter[2].ParameterName = "@deletedon";
                objDbParameter[2].Value = DateTime.Now;



                int result =  DataHelper.ExecuteNonQuery("SP_DeleteDocument", objDbTransaction, objDbParameter);

                if (result > 0)
                {

                    objDbTransaction.Commit();
                    return Utility.ResultType.Success;
                }
                else
                {
                    objDbTransaction.Rollback();
                    return Utility.ResultType.Failure;
                }

            }
            catch (Exception ex)
            {
                objDbTransaction.Rollback();
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return Utility.ResultType.Error;
            }
        }

        public static void InsertDocMoveHistory(string IPAddress, string MacAddress, int UserId, int DocId, int OldMetadataID, int NewMetadataID, string Entity, string from, string to)
        {
            DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;
            DbTransaction objDbTransaction = Utility.GetTransaction;
            try
            {
                DbParameter[] objDbParameter = new DbParameter[9];

                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "@IPAddress";
                objDbParameter[0].Value = IPAddress;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "@MacAddress";
                objDbParameter[1].Value = MacAddress;

                objDbParameter[2] = objDbProviderFactory.CreateParameter();
                objDbParameter[2].ParameterName = "@TransferedBy";
                objDbParameter[2].Value = UserId;

                objDbParameter[3] = objDbProviderFactory.CreateParameter();
                objDbParameter[3].ParameterName = "@DocID";
                objDbParameter[3].Value = DocId;

                objDbParameter[4] = objDbProviderFactory.CreateParameter();
                objDbParameter[4].ParameterName = "@OldMetadataID";
                objDbParameter[4].Value = OldMetadataID;

                objDbParameter[5] = objDbProviderFactory.CreateParameter();
                objDbParameter[5].ParameterName = "@NewMetadataID";
                objDbParameter[5].Value = NewMetadataID;

                objDbParameter[6] = objDbProviderFactory.CreateParameter();
                objDbParameter[6].ParameterName = "@Entity";
                objDbParameter[6].Value = Entity;

                objDbParameter[7] = objDbProviderFactory.CreateParameter();
                objDbParameter[7].ParameterName = "@from";
                objDbParameter[7].Value = from;

                objDbParameter[8] = objDbProviderFactory.CreateParameter();
                objDbParameter[8].ParameterName = "@to";
                objDbParameter[8].Value = to;


                DataHelper.ExecuteNonQuery("SP_I_DocumentMoveHistory", objDbTransaction, objDbParameter);
                objDbTransaction.Commit();
            }
            catch (Exception ex)
            {
                objDbTransaction.Rollback();
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                //return Utility.ResultType.Error;
            }
        }


        public static DataSet CheckDocumentVirescent(string Docname, int RepId, int metatemplateId)
        {
            try
            {
                //string strQuery = "select ID from DOCUMENT where DocumentName = '" + Docname + "' and Status=1";
                string strQuery = @"select D.ID from DOCUMENT D inner join MetaData M 
                                  on D.MetaDataID=M.ID where D.DocumentName = '" + Docname + "' and M.RepositoryID='" + RepId + "' and M.MetaTemplateID='" + metatemplateId + "' and D.Status=1";
                DataSet ds = new DataSet();
                ds = DataHelper.ExecuteDataSet(strQuery);
                return ds;
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return null;
            }
        }

        public static Utility.ResultType UpdateDocumentForMergingNEW(Document objDocument, DbTransaction objDbTransaction)
        {
            DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

            try
            {
                DbParameter[] objDbParameter = new DbParameter[11];

                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "DocId";
                objDbParameter[0].Value = objDocument.DocumentID;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "DocumentName";
                objDbParameter[1].Value = objDocument.DocumentName;

                objDbParameter[2] = objDbProviderFactory.CreateParameter();
                objDbParameter[2].ParameterName = "DocumentGuid";
                objDbParameter[2].Value = objDocument.DocumentGuid;

                objDbParameter[3] = objDbProviderFactory.CreateParameter();
                objDbParameter[3].ParameterName = "DocumentPath";
                objDbParameter[3].Value = objDocument.DocumentPath;


                objDbParameter[4] = objDbProviderFactory.CreateParameter();
                objDbParameter[4].ParameterName = "CreatedOn";
                objDbParameter[4].Value = DateTime.Now;

                objDbParameter[5] = objDbProviderFactory.CreateParameter();
                objDbParameter[5].ParameterName = "UpdatedBy";
                objDbParameter[5].Value = objDocument.UpdatedBy;

                objDbParameter[6] = objDbProviderFactory.CreateParameter();
                objDbParameter[6].ParameterName = "MergedPageCount";
                objDbParameter[6].Value = objDocument.MergedPageCount;

                objDbParameter[7] = objDbProviderFactory.CreateParameter();
                objDbParameter[7].ParameterName = "DocumentSize";
                objDbParameter[7].Value = objDocument.Size;

                objDbParameter[8] = objDbProviderFactory.CreateParameter();
                objDbParameter[8].ParameterName = "IPAddress";
                objDbParameter[8].Value = objDocument.IPAddress;


                objDbParameter[9] = objDbProviderFactory.CreateParameter();
                objDbParameter[9].ParameterName = "V_OLDPAGECOUNT";
                objDbParameter[9].Value = objDocument.OldPageCount;

                objDbParameter[10] = objDbProviderFactory.CreateParameter();
                objDbParameter[10].ParameterName = "V_PAGECOUNT";
                objDbParameter[10].Value = objDocument.PageCount;

                DataHelper.ExecuteNonQuery("SP_U_DOCUMENTFORMERGING_NEW", objDbTransaction, objDbParameter);

                return Utility.ResultType.Success;
            }
            catch (Exception ex)
            {

                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return Utility.ResultType.Error;
            }
        }

        //public static Utility.ResultType UploadedDcoumentReport(out DataTable objDataTable, int RepositoryID, int MetatemplateId, int CategoryId, int FolderID, string fromdate, string todate)
        //{
        //    DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

        //    string FolderIDFinal = "";
        //    try
        //    {
        //        if (FolderID != 0)
        //        {
        //            string folderids = string.Empty;
        //            Utility.FolderHasChild(FolderID, ref folderids);
        //            FolderIDFinal = folderids + FolderID;
        //        }
        //        else
        //        {
        //            FolderIDFinal = "0";
        //        }

        //        DbParameter[] objDbParameter = new DbParameter[6];

        //        objDbParameter[0] = objDbProviderFactory.CreateParameter();
        //        objDbParameter[0].ParameterName = "RepositoryID";
        //        objDbParameter[0].Value = RepositoryID;

        //        objDbParameter[1] = objDbProviderFactory.CreateParameter();
        //        objDbParameter[1].ParameterName = "MetatemplateID";
        //        objDbParameter[1].Value = MetatemplateId;

        //        objDbParameter[2] = objDbProviderFactory.CreateParameter();
        //        objDbParameter[2].ParameterName = "CategoryID";
        //        objDbParameter[2].Value = CategoryId;

        //        objDbParameter[3] = objDbProviderFactory.CreateParameter();
        //        objDbParameter[3].ParameterName = "FolderID";
        //        objDbParameter[3].Value = FolderIDFinal;

        //        objDbParameter[4] = objDbProviderFactory.CreateParameter();
        //        objDbParameter[4].ParameterName = "FromDate";
        //        objDbParameter[4].Value = fromdate;

        //        objDbParameter[5] = objDbProviderFactory.CreateParameter();
        //        objDbParameter[5].ParameterName = "ToDate";
        //        objDbParameter[5].Value = todate;

        //        //objDataTable = DataHelper.ExecuteDataTable(strQuery, objDbParameter);

        //        objDataTable = DataHelper.ExecuteDataTableNew("Sp_S_UploadedDocReport", objDbParameter);
        //        if (objDataTable.Rows.Count == 0)
        //        {
        //            return Utility.ResultType.Failure;
        //        }
        //        return Utility.ResultType.Success;
        //    }
        //    catch (Exception ex)
        //    {
        //        LogManager.ErrorLog(Utility.LogFilePath, ex);
        //        objDataTable = null;
        //        return Utility.ResultType.Error;
        //    }

        //}

        public static Utility.ResultType UploadedDcoumentReport(out DataTable objDataTable, int RepositoryID, int MetatemplateId, int FolderID, string fromdate, string todate)
        {
            DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

            string FolderIDFinal = "";
            try
            {
                if (FolderID != 0)
                {
                    string folderids = string.Empty;
                    Utility.FolderHasChild(FolderID, ref folderids);
                    FolderIDFinal = folderids + FolderID;
                }
                else
                {
                    FolderIDFinal = "0";
                }

                DbParameter[] objDbParameter = new DbParameter[5];

                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "RepositoryID";
                objDbParameter[0].Value = RepositoryID;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "MetatemplateID";
                objDbParameter[1].Value = MetatemplateId;

                //objDbParameter[2] = objDbProviderFactory.CreateParameter();
                //objDbParameter[2].ParameterName = "CategoryID";
                //objDbParameter[2].Value = CategoryId;

                objDbParameter[2] = objDbProviderFactory.CreateParameter();
                objDbParameter[2].ParameterName = "FolderID";
                objDbParameter[2].Value = FolderIDFinal;

                objDbParameter[3] = objDbProviderFactory.CreateParameter();
                objDbParameter[3].ParameterName = "FromDate";
                objDbParameter[3].Value = fromdate;

                objDbParameter[4] = objDbProviderFactory.CreateParameter();
                objDbParameter[4].ParameterName = "ToDate";
                objDbParameter[4].Value = todate;

                //objDataTable = DataHelper.ExecuteDataTable(strQuery, objDbParameter);

                objDataTable = DataHelper.ExecuteDataTableNew("Sp_S_UploadedDocReport", objDbParameter);
                if (objDataTable.Rows.Count == 0)
                {
                    return Utility.ResultType.Failure;
                }
                return Utility.ResultType.Success;
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                objDataTable = null;
                return Utility.ResultType.Error;
            }

        }

        public static Utility.ResultType FieldSearch(out DataTable objDataTable, int RepositoryID, int MetatemplateId, int FolderID, string FieldData)
        {
            DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

            string FolderIDFinal = "";
            try
            {
                if (FolderID != 0)
                {
                    string folderids = string.Empty;
                    Utility.FolderHasChild(FolderID, ref folderids);
                    FolderIDFinal = folderids + FolderID;
                }
                else
                {
                    FolderIDFinal = "0";
                }

                DbParameter[] objDbParameter = new DbParameter[4];

                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "RepositoryID";
                objDbParameter[0].Value = RepositoryID;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "MetatemplateID";
                objDbParameter[1].Value = MetatemplateId;

                //objDbParameter[2] = objDbProviderFactory.CreateParameter();
                //objDbParameter[2].ParameterName = "CategoryID";
                //objDbParameter[2].Value = CategoryId;

                objDbParameter[2] = objDbProviderFactory.CreateParameter();
                objDbParameter[2].ParameterName = "FolderID";
                objDbParameter[2].Value = FolderIDFinal;

                objDbParameter[3] = objDbProviderFactory.CreateParameter();
                objDbParameter[3].ParameterName = "FieldData";
                objDbParameter[3].Value = FieldData;

                //objDataTable = DataHelper.ExecuteDataTable(strQuery, objDbParameter);

                objDataTable = DataHelper.ExecuteDataTableNew("Sp_S_FieldSearch", objDbParameter);
                if (objDataTable.Rows.Count == 0)
                {
                    return Utility.ResultType.Failure;
                }
                return Utility.ResultType.Success;
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                objDataTable = null;
                return Utility.ResultType.Error;
            }

        }

        public static Utility.ResultType SearchInAll(out DataTable objDataTable, string DocName, int userID, int Flag)
        {
            try
            {
                #region OldCodeTill_17_02_2021

                // Commented by Sanjay

                //string strQuery="";
                //if(Flag==1)//like cretiria

                //    strQuery = @"select * from vwDocumentDetails where DocumentName like'%" + DocName + "%'";

                //else if(Flag==2)//equal criteria

                //    strQuery = @"select * from vwDocumentDetails where DocumentName ='" + DocName + "'";

                //else if (Flag == 3)//equal criteria

                //    strQuery = @"select * from vwDocumentDetails where DocumentName not like'%" + DocName + "%'";

                //objDataTable = DataHelper.ExecuteDataTable(strQuery, null);
                //if (objDataTable.Rows.Count == 0)
                //{
                //    return Utility.ResultType.Failure;
                //}
                //return Utility.ResultType.Success;

                #endregion

                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory; //mayuresh
                DbParameter[] objDbParameter = new DbParameter[3];

                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "@DocName";
                objDbParameter[0].Value = DocName;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "@userID";
                objDbParameter[1].Value = userID;

                objDbParameter[2] = objDbProviderFactory.CreateParameter();
                objDbParameter[2].ParameterName = "@Flag";
                objDbParameter[2].Value = Flag;

                objDataTable = DataHelper.ExecuteDataTableNew("SP_S_SearchInAll", objDbParameter);
                if (objDataTable.Rows.Count == 0)
                {
                    return Utility.ResultType.Failure;
                }
                return Utility.ResultType.Success;

            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                objDataTable = null;
                return Utility.ResultType.Error;

            }
        }
        
        public static Utility.ResultType DocumentRename(string DocumentName, int DocumentID, DbTransaction objDbTransaction)
        {
            try
            {
                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;
                DbParameter[] objDbParameter = new DbParameter[3];

                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "@DocumentName";
                objDbParameter[0].Value = DocumentName;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "@DocumentID";
                objDbParameter[1].Value = DocumentID;

                objDbParameter[2] = objDbProviderFactory.CreateParameter();
                objDbParameter[2].ParameterName = "@UpdatedBy";
                objDbParameter[2].Value = 1;

                int result = DataHelper.ExecuteNonQuery("SP_U_DocumentRename", objDbTransaction, objDbParameter);

                if (result != 0)
                    objDbTransaction.Commit();
                else
                    objDbTransaction.Rollback();

                return Utility.ResultType.Success;
            }
            catch (Exception ex)
            {

                return Utility.ResultType.Error;
            }
        }
        
        public static Utility.ResultType AuditReports(out DataTable dt, string RoleID, string FromDate, string ToDate, string UserID) //submit multiple USer REport
        {
            try
            {
                string query = "";

                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;
                DbParameter[] objDbParameter = new DbParameter[4];

                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "Roleid";
                objDbParameter[0].Value = RoleID;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "FromDate";
                objDbParameter[1].Value = FromDate;

                objDbParameter[2] = objDbProviderFactory.CreateParameter();
                objDbParameter[2].ParameterName = "ToDate";
                objDbParameter[2].Value = ToDate;

                objDbParameter[3] = objDbProviderFactory.CreateParameter();
                objDbParameter[3].ParameterName = "UserID";
                objDbParameter[3].Value = UserID;

                dt = DataHelper.ExecuteDataTableForProcedure("Sp_S_GetAuditReport", null, objDbParameter);

                if (dt.Rows.Count > 0)
                {
                    return Utility.ResultType.Success;
                }
                else
                {
                    return Utility.ResultType.Error;
                }
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                dt = null;
                return Utility.ResultType.Error;
            }
        }
        
        public static Utility.ResultType PageHitCount(out DataTable dt, string username, string fromdate, string todate)
        {
            try
            {
                string query = "";
                query = "select distinct(PageName) ,count(*) as Counting from DMSAuditLog v inner join [User] u on u.Id=v.UserID where CAST(v.DateOfActivity as DATE) between '" + fromdate + "' and '" + todate + "' and v.UserID=" + username + " and  Activity like '%Visit%' group by PageName order by PageName asc";
                //query = "select v.ID, v.IPAddress,v.MacAddress , v.DateOfActivity, v.Activity,v.DocumentName,u.UserName from VDRAudit v inner join [User] u on u.Id=v.UserID where CAST(v.DateOfActivity as DATE) between '" + fromdate + "' and '" + todate + "' and v.UserID=" + username + " order by v.DateOfActivity asc";
                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

                dt = DataHelper.ExecuteDataTable(query, null);
                if (dt.Rows.Count > 0)
                {
                    return Utility.ResultType.Success;
                }
                else
                {
                    return Utility.ResultType.Error;
                }
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                dt = null;
                return Utility.ResultType.Error;
            }
        }
        
        public static Utility.ResultType LoadRecentDocument(out DataTable objDataTable, string Action, int roleid, int userid)
        {
            try
            {
                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

                DbParameter[] objDbParameter = new DbParameter[3];
                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "Action";
                objDbParameter[0].Value = Action;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "roleid";
                objDbParameter[1].Value = roleid;

                objDbParameter[2] = objDbProviderFactory.CreateParameter();
                objDbParameter[2].ParameterName = "UserId";
                objDbParameter[2].Value = userid;

                objDataTable = DataHelper.ExecuteDataTableNew("RecentlyUploadedDocument", objDbParameter);
                if (objDataTable.Rows.Count == 0)
                {
                    return Utility.ResultType.Failure;
                }
                return Utility.ResultType.Success;
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                objDataTable = null;
                return Utility.ResultType.Error;
            }

        }

        public static Utility.ResultType LoadCountDocumnet(out DataTable objDataTable, int roleid, int userid)
        {
            try
            {
                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

                DbParameter[] objDbParameter = new DbParameter[2];


                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "UserId";
                objDbParameter[0].Value = userid;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "RoleId";
                objDbParameter[1].Value = roleid;
                
                objDataTable = DataHelper.ExecuteDataTableNew("SP_DocumentCount", objDbParameter);
                if (objDataTable.Rows.Count == 0)
                {
                    return Utility.ResultType.Failure;
                }
                return Utility.ResultType.Success;
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                objDataTable = null;
                return Utility.ResultType.Error;
            }

        }
        
        public static void MonthlyChart(out DataTable objDataTable, string RoleId, int userid)
        {
            try
            {
                string strQuery = string.Empty;

                strQuery = "select  count(*) x, DATENAME(month,(d.createdon)) y from document d inner join MetaData md on md.ID=d.MetaDataID  where YEAR(d.createdon) = YEAR(GETDATE()) group by DATENAME(month,(d.createdon)) ";

                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;
                objDataTable = DataHelper.ExecuteDataTable(strQuery, null);
                if (objDataTable.Rows.Count == 0)
                {
                    //return Utility.ResultType.Failure;
                }
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                objDataTable = null;
                //return Utility.ResultType.Error;
            }
        }
        
        public static void Metatemplatewisechart(out DataTable objDataTable, string RoleId, int userid, string year)
        {
            try
            {
                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

                DbParameter[] objDbParameter = new DbParameter[3];

                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "RoleID";
                objDbParameter[0].Value = RoleId;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "UserID";
                objDbParameter[1].Value = userid;

                objDbParameter[2] = objDbProviderFactory.CreateParameter();
                objDbParameter[2].ParameterName = "Year";
                objDbParameter[2].Value = year;

                objDataTable = DataHelper.ExecuteDataTableNew("SP_S_MetaTemplateWiseChart", objDbParameter);
                if (objDataTable.Rows.Count == 0)
                {
                    // return Utility.ResultType.Failure;
                }
                // return Utility.ResultType.Success;
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                objDataTable = null;
                // return Utility.ResultType.Error;
            }
        }

        public static void GetYears(out DataTable objDataTable, string RoleId, int userid)
        {
            try
            {
                string strQuery = string.Empty;

                strQuery = "select distinct year(CreatedOn) as 'Years' from Document";
                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;
                objDataTable = DataHelper.ExecuteDataTable(strQuery, null);
                if (objDataTable.Rows.Count == 0)
                {
                    //return Utility.ResultType.Failure;
                }
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                objDataTable = null;
                //return Utility.ResultType.Error;
            }
        }
        
        #region download

        public static Utility.ResultType DownloadUserTrue(out DataTable dt, String id)
        {
            try
            {
                string query = "update [user] set Download = 1 where Id=" + id;
                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

                dt = DataHelper.ExecuteDataTable(query, null);
                //if (dt.Rows.Count > 0)
                //{
                return Utility.ResultType.Success;
                //}
                //else
                //{
                //    return Utility.ResultType.Error;
                //}
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                dt = null;
                return Utility.ResultType.Error;
            }
        }

        public static Utility.ResultType DownloadUserFalse(out DataTable dt, String id)
        {
            try
            {
                string query = "update [user] set Download = 0 where Id=" + id;
                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

                dt = DataHelper.ExecuteDataTable(query, null);
                //if (dt.Rows.Count > 0)
                //{
                return Utility.ResultType.Success;
                //}
                //else
                //{
                //    return Utility.ResultType.Error;
                //}
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                dt = null;
                return Utility.ResultType.Error;
            }
        }

        #endregion download
        
        # region View

        public static Utility.ResultType ViewTrue(out DataTable dt, String id)
        {
            try
            {
                string query = "update [user] set IsViewed = 1 where Id=" + id;
                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

                dt = DataHelper.ExecuteDataTable(query, null);
                // if (dt.Rows.Count < 0)
                //{
                return Utility.ResultType.Success;
                //}
                //else
                //{
                //    return Utility.ResultType.Error;
                //}
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                dt = null;
                return Utility.ResultType.Error;
            }
        }

        public static Utility.ResultType ViewFalse(out DataTable dt, String id)
        {
            try
            {
                string query = "update [user] set IsViewed = 0 where Id=" + id;
                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

                dt = DataHelper.ExecuteDataTable(query, null);
                // if (dt.Rows.Count > 0)
                //{
                return Utility.ResultType.Success;
                // }
                // else
                //{
                // return Utility.ResultType.Error;
                // }
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                dt = null;
                return Utility.ResultType.Error;
            }
        }

        #endregion

        # region Edit

        public static Utility.ResultType EditTrue(out DataTable dt, String id)
        {
            try
            {
                string query = "update [user] set IsEdit = 1 where Id=" + id;
                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

                dt = DataHelper.ExecuteDataTable(query, null);
                // if (dt.Rows.Count < 0)
                //{
                return Utility.ResultType.Success;
                //}
                //else
                //{
                //    return Utility.ResultType.Error;
                //}
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                dt = null;
                return Utility.ResultType.Error;
            }
        }

        public static Utility.ResultType EditFalse(out DataTable dt, String id)
        {
            try
            {
                string query = "update [user] set IsEdit = 0 where Id=" + id;
                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

                dt = DataHelper.ExecuteDataTable(query, null);
                // if (dt.Rows.Count > 0)
                //{
                return Utility.ResultType.Success;
                // }
                // else
                //{
                // return Utility.ResultType.Error;
                // }
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                dt = null;
                return Utility.ResultType.Error;
            }
        }

        #endregion

        # region Merge

        public static Utility.ResultType MergeTrue(out DataTable dt, String id)
        {
            try
            {
                string query = "update [user] set IsMerge = 1 where Id=" + id;
                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

                dt = DataHelper.ExecuteDataTable(query, null);
                // if (dt.Rows.Count < 0)
                //{
                return Utility.ResultType.Success;
                //}
                //else
                //{
                //    return Utility.ResultType.Error;
                //}
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                dt = null;
                return Utility.ResultType.Error;
            }
        }

        public static Utility.ResultType MergeFalse(out DataTable dt, String id)
        {
            try
            {
                string query = "update [user] set IsMerge = 0 where Id=" + id;
                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

                dt = DataHelper.ExecuteDataTable(query, null);
                // if (dt.Rows.Count > 0)
                //{
                return Utility.ResultType.Success;
                // }
                // else
                //{
                // return Utility.ResultType.Error;
                // }
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                dt = null;
                return Utility.ResultType.Error;
            }
        }

        #endregion

        # region Split

        public static Utility.ResultType SplitTrue(out DataTable dt, String id)
        {
            try
            {
                string query = "update [user] set IsSplit = 1 where Id=" + id;
                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

                dt = DataHelper.ExecuteDataTable(query, null);
                // if (dt.Rows.Count < 0)
                //{
                return Utility.ResultType.Success;
                //}
                //else
                //{
                //    return Utility.ResultType.Error;
                //}
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                dt = null;
                return Utility.ResultType.Error;
            }
        }

        public static Utility.ResultType SplitFalse(out DataTable dt, String id)
        {
            try
            {
                string query = "update [user] set IsSplit = 0 where Id=" + id;
                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

                dt = DataHelper.ExecuteDataTable(query, null);
                // if (dt.Rows.Count > 0)
                //{
                return Utility.ResultType.Success;
                // }
                // else
                //{
                // return Utility.ResultType.Error;
                // }
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                dt = null;
                return Utility.ResultType.Error;
            }
        }

        #endregion

        # region Delete

        public static Utility.ResultType DeleteTrue(out DataTable dt, String id)
        {
            try
            {
                string query = "update [user] set IsDelete = 1 where Id=" + id;
                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

                dt = DataHelper.ExecuteDataTable(query, null);
                // if (dt.Rows.Count < 0)
                //{
                return Utility.ResultType.Success;
                //}
                //else
                //{
                //    return Utility.ResultType.Error;
                //}
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                dt = null;
                return Utility.ResultType.Error;
            }
        }

        public static Utility.ResultType DeleteFalse(out DataTable dt, String id)
        {
            try
            {
                string query = "update [user] set IsDelete = 0 where Id=" + id;
                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

                dt = DataHelper.ExecuteDataTable(query, null);
                // if (dt.Rows.Count > 0)
                //{
                return Utility.ResultType.Success;
                // }
                // else
                //{
                // return Utility.ResultType.Error;
                // }
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                dt = null;
                return Utility.ResultType.Error;
            }
        }

        #endregion
        
        public static Utility.ResultType GetUserData(out DataTable dt, int UserId)
        {
            try
            {
                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;
                DbParameter[] objDbParameter = new DbParameter[1];

                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "UserId";
                objDbParameter[0].Value = UserId;

                dt = DataHelper.ExecuteDataTableForProcedure("SP_S_GetUserData", null, objDbParameter);
                if (dt.Rows.Count > 0)
                {
                    return Utility.ResultType.Success;
                }
                else
                {
                    return Utility.ResultType.Failure;
                }
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                dt = null;
                return Utility.ResultType.Error;
            }

        }
        
        public static Utility.ResultType ValidateUser(out DataTable dt, int UserID)
        {
            try
            {
                string query = " select download from [user] where id=" + UserID;
                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

                dt = DataHelper.ExecuteDataTable(query, null);
                if (dt.Rows.Count > 0)
                {
                    if (Convert.ToString(dt.Rows[0][0]) == "1")
                    {
                        return Utility.ResultType.Success;
                    }
                    else
                    {
                        return Utility.ResultType.Failure;
                    }
                }
                else
                {
                    return Utility.ResultType.Error;
                }
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                dt = null;
                return Utility.ResultType.Error;
            }

        }


    }
}


