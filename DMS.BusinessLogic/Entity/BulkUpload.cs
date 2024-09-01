using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data;

namespace DMS.BusinessLogic.Entity
{
    public class BulkUpload
    {
        #region Enum
        public enum UploadStatusType { InProcess=1,Success=2,Error=3}
        #endregion

        #region Properties
        public int BulkUploadID { get; set; }
        public string BulkUploadCode { get; set; }
        public string MetaDataCode { get; set; }
        public string DocumentGuid { get; set; }
        public string DocumentPath { get; set; }
        public string DownloadPath { get; set; }
        public int RepositoryID { get; set; }
        public int MetaTemplateID { get; set; }
        public int FolderID { get; set; }
        public int CategoryID { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public int UpdatedBy { get; set; }
        public int UploadStatus { get; set; }
        public int Status { get; set; }
        #endregion

        #region Method

        public static Utility.ResultType Insert(BulkUpload objBulkUpload)
        {
            DbTransaction objDbTransaction = Utility.GetTransaction;
            DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

            try
            {
                DbParameter[] objDbParameter = new DbParameter[13];

                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "BulkUploadCode";
                objDbParameter[0].Size = 100;
                objDbParameter[0].Direction = System.Data.ParameterDirection.Output;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "MetaDataCode";
                objDbParameter[1].Value = DBNull.Value;

                objDbParameter[2] = objDbProviderFactory.CreateParameter();
                objDbParameter[2].ParameterName = "DocumentGuid";
                objDbParameter[2].Value = objBulkUpload.DocumentGuid;

                objDbParameter[3] = objDbProviderFactory.CreateParameter();
                objDbParameter[3].ParameterName = "DocumentPath";
                objDbParameter[3].Value = objBulkUpload.DocumentPath;

                objDbParameter[4] = objDbProviderFactory.CreateParameter();
                objDbParameter[4].ParameterName = "DownloadPath";
                objDbParameter[4].Value = DBNull.Value;

                objDbParameter[5] = objDbProviderFactory.CreateParameter();
                objDbParameter[5].ParameterName = "RepositoryID";
                objDbParameter[5].Value = objBulkUpload.RepositoryID;

                objDbParameter[6] = objDbProviderFactory.CreateParameter();
                objDbParameter[6].ParameterName = "MetaTemplateID";
                objDbParameter[6].Value = objBulkUpload.MetaTemplateID;

                objDbParameter[7] = objDbProviderFactory.CreateParameter();
                objDbParameter[7].ParameterName = "FolderID";
                objDbParameter[7].Value = objBulkUpload.FolderID;

                objDbParameter[8] = objDbProviderFactory.CreateParameter();
                objDbParameter[8].ParameterName = "CategoryID";
                objDbParameter[8].Value = objBulkUpload.CategoryID;
                
                objDbParameter[9] = objDbProviderFactory.CreateParameter();
                objDbParameter[9].ParameterName = "CreatedOn";
                objDbParameter[9].Value = DateTime.Now;

                objDbParameter[10] = objDbProviderFactory.CreateParameter();
                objDbParameter[10].ParameterName = "CreatedBy";
                objDbParameter[10].Value = objBulkUpload.CreatedBy;

                objDbParameter[11] = objDbProviderFactory.CreateParameter();
                objDbParameter[11].ParameterName = "UploadStatus";
                objDbParameter[11].Value = objBulkUpload.UploadStatus;

                objDbParameter[12] = objDbProviderFactory.CreateParameter();
                objDbParameter[12].ParameterName = "Status";
                objDbParameter[12].Value = objBulkUpload.Status;

                DataHelper.ExecuteNonQuery("SP_I_BulkUpload", objDbTransaction, objDbParameter);

                objBulkUpload.BulkUploadCode = Convert.ToString(objDbParameter[0].Value);

                objDbTransaction.Commit();
                return Utility.ResultType.Success;
            }
            catch (Exception ex)
            {
                objDbTransaction.Rollback();
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return Utility.ResultType.Error;
            }
        }

        public static Utility.ResultType Update(BulkUpload objBulkUpload)
        {
            DbTransaction objDbTransaction = Utility.GetTransaction;
            DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

            try
            {
                DbParameter[] objDbParameter = new DbParameter[4];

                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "MetaDataCode";
                objDbParameter[0].Value = objBulkUpload.MetaDataCode;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "DownloadPath";
                objDbParameter[1].Value = objBulkUpload.DownloadPath;

                objDbParameter[2] = objDbProviderFactory.CreateParameter();
                objDbParameter[2].ParameterName = "UploadStatus";
                objDbParameter[2].Value = objBulkUpload.UploadStatus;

                objDbParameter[3] = objDbProviderFactory.CreateParameter();
                objDbParameter[3].ParameterName = "BulkUploadID";
                objDbParameter[3].Value = objBulkUpload.BulkUploadID;
                
                DataHelper.ExecuteNonQuery("SP_U_BulkUpload", objDbTransaction, objDbParameter);

                objDbTransaction.Commit();
                return Utility.ResultType.Success;
            }
            catch (Exception ex)
            {
                objDbTransaction.Rollback();
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return Utility.ResultType.Error;
            }
        }

        public static Utility.ResultType Select( out DataTable objDataTable,BulkUpload objBulkUpload)
        {
            try
            {
                string strQuery = string.Empty;
                char charQuote = '"';

                strQuery = @"SELECT ID,BulkUploadCode,MetaDataCode,DownloadPath,CreatedOn," +
                            " (SELECT FirstName FROM vwUser WHERE ID = vwBulkUpload.CreatedBy) AS " + charQuote + "CreatedBy" + charQuote + "," +
                            " (SELECT SettingName FROM vwAppSetting WHERE SettingType='UPLOADSTATUS' AND SettingValue=vwBulkUpload.UploadStatus) AS "+charQuote+"UPLOADSTATUS"+charQuote+""+
                            " FROM vwBulkUpload WHERE Status=1 AND vwBulkUpload.RepositoryID = @RepositoryID AND vwBulkUpload.MetaTemplateID = @MetaTemplateID " +
                            " AND vwBulkUpload.CategoryID = @CategoryID AND vwBulkUpload.FolderID = @FolderID " +
                            " ORDER BY vwBulkUpload.ID DESC";

                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

                DbParameter[] objDbParameter = new DbParameter[4];
                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "RepositoryID";
                objDbParameter[0].Value = objBulkUpload.RepositoryID;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "MetaTemplateID";
                objDbParameter[1].Value = objBulkUpload.MetaTemplateID;

                objDbParameter[2] = objDbProviderFactory.CreateParameter();
                objDbParameter[2].ParameterName = "CategoryID";
                objDbParameter[2].Value = objBulkUpload.CategoryID;

                objDbParameter[3] = objDbProviderFactory.CreateParameter();
                objDbParameter[3].ParameterName = "FolderID";
                objDbParameter[3].Value = objBulkUpload.FolderID;

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
        #endregion
    }
}
