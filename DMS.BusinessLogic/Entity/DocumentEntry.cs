using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;

namespace DMS.BusinessLogic
{
    public class DocumentEntry
    {
        #region Properties
        public int DocumentEntryID { get; set; }
        public int DocumentID { get; set; }
        public int FieldID { get; set; }
        public string FieldData { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        #endregion

        #region Method
        public static Utility.ResultType Insert(DocumentEntry objDocumentEntry, DbTransaction objDbTransaction)
        {
            DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;
            try
            {
                DbParameter[] objDbParameter = new DbParameter[4];

                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "DocumentID";
                objDbParameter[0].Value = objDocumentEntry.DocumentID;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "FieldID";
                objDbParameter[1].Value = objDocumentEntry.FieldID;

                objDbParameter[2] = objDbProviderFactory.CreateParameter();
                objDbParameter[2].ParameterName = "FieldData";
                objDbParameter[2].Value = objDocumentEntry.FieldData;

                objDbParameter[3] = objDbProviderFactory.CreateParameter();
                objDbParameter[3].ParameterName = "CreatedBy";
                objDbParameter[3].Value = objDocumentEntry.CreatedBy;

                DataHelper.ExecuteNonQuery("SP_I_DocumentEntry_Virescent", objDbTransaction, objDbParameter);

                return Utility.ResultType.Success;
            }
            catch (Exception ex)
            {

                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return Utility.ResultType.Error;
            }
        }

        public static Utility.ResultType Update(DocumentEntry objDocumentEntry, DbTransaction objDbTransaction,out int intAffectedRows)
        {
            DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;
            try
            {
                DbParameter[] objDbParameter = new DbParameter[5];

                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "DocumentID";
                objDbParameter[0].Value = objDocumentEntry.DocumentID;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "FieldID";
                objDbParameter[1].Value = objDocumentEntry.FieldID;

                objDbParameter[2] = objDbProviderFactory.CreateParameter();
                objDbParameter[2].ParameterName = "FieldData";
                objDbParameter[2].Value = objDocumentEntry.FieldData;

                objDbParameter[3] = objDbProviderFactory.CreateParameter();
                objDbParameter[3].ParameterName = "UpdatedBy";
                objDbParameter[3].Value = objDocumentEntry.UpdatedBy;

                objDbParameter[4] = objDbProviderFactory.CreateParameter();
                objDbParameter[4].ParameterName = "AffectedRows";
                objDbParameter[4].Direction = ParameterDirection.Output;
                objDbParameter[4].DbType = DbType.Int32;

                DataHelper.ExecuteNonQuery("SP_U_DocumentEntry_New", objDbTransaction, objDbParameter);

                intAffectedRows = Convert.ToInt32(objDbParameter[4].Value);

                return Utility.ResultType.Success;
            }
            catch (Exception ex)
            {
                intAffectedRows = 0;
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return Utility.ResultType.Error;
            }
        }

        public static Utility.ResultType Delete(DocumentEntry objDocumentEntry, DbTransaction objDbTransaction)
        {
            DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;
            try
            {
                DbParameter[] objDbParameter = new DbParameter[1];

                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "DocumentID";
                objDbParameter[0].Value = objDocumentEntry.DocumentID;

                DataHelper.ExecuteNonQuery("SP_D_DocumentEntry", objDbTransaction, objDbParameter);

                return Utility.ResultType.Success;
            }
            catch (Exception ex)
            {

                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return Utility.ResultType.Error;
            }
        }

        public static Utility.ResultType Select(out DataTable objDataTable, int intMetaDataID)
        {
            try
            {
                string strQuery = "SELECT * FROM vwDocumentEntry WHERE DocumentID IN (SELECT ID FROM vwDocument WHERE MetaDataID = @MetaDataID)";

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

        public static Utility.ResultType SelectForValueExistance(DocumentEntry objDocumentEntry)
        {
            try
            {
                if (objDocumentEntry.DocumentID == 0)
                {
                    string strQuery = "  SELECT * FROM DocumentEntry WHERE FieldID = @FieldID AND FieldData = @FieldData";

                    DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

                    DbParameter[] objDbParameter = new DbParameter[2];
                    objDbParameter[0] = objDbProviderFactory.CreateParameter();
                    objDbParameter[0].ParameterName = "FieldID";
                    objDbParameter[0].Value = objDocumentEntry.FieldID;

                    objDbParameter[1] = objDbProviderFactory.CreateParameter();
                    objDbParameter[1].ParameterName = "FieldData";
                    objDbParameter[1].Value = objDocumentEntry.FieldData;

                    DataTable objDataTable = DataHelper.ExecuteDataTable(strQuery, objDbParameter);
                    if (objDataTable.Rows.Count == 0)
                    {
                        return Utility.ResultType.Failure;
                    }
                }
                else
                {
                    string strQuery = "  SELECT * FROM DocumentEntry WHERE FieldID = @FieldID AND FieldData = @FieldData  AND DocumentID <> @DocumentID";

                    DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

                    DbParameter[] objDbParameter = new DbParameter[3];
                    objDbParameter[0] = objDbProviderFactory.CreateParameter();
                    objDbParameter[0].ParameterName = "FieldID";
                    objDbParameter[0].Value = objDocumentEntry.FieldID;

                    objDbParameter[1] = objDbProviderFactory.CreateParameter();
                    objDbParameter[1].ParameterName = "FieldData";
                    objDbParameter[1].Value = objDocumentEntry.FieldData;

                    objDbParameter[2] = objDbProviderFactory.CreateParameter();
                    objDbParameter[2].ParameterName = "DocumentID";
                    objDbParameter[2].Value = objDocumentEntry.DocumentID;

                    DataTable objDataTable = DataHelper.ExecuteDataTable(strQuery, objDbParameter);
                    if (objDataTable.Rows.Count == 0)
                    {
                        return Utility.ResultType.Failure;
                    }
                }
                return Utility.ResultType.Success;
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return Utility.ResultType.Error;
            }
        }

        #endregion

    }
}
