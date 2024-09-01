using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;

namespace DMS.BusinessLogic
{
    public class MetaTemplateFields
    {

        #region Properties
        public int MetaTemplateFieldsID { get; set; }
        public string FieldName { get; set; }
        public int FieldDataTypeID { get; set; }
        public int FieldTypeID { get; set; }
        public int FieldLength { get; set; }
        public int IsPrimary { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public int UpdatedBy { get; set; }
        public int Status { get; set; }
        public int RepositoryID { get; set; }       
        public int MetaTemplateID { get; set; }
        #endregion


        #region Method
        public static Utility.ResultType Insert(MetaTemplateFields objMetaTemplateFields)
        {
            DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;
            DbTransaction objDbTransaction = Utility.GetTransaction;
            try
            {

                DbParameter[] objDbParameter = new DbParameter[12];

                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "FieldName";
                objDbParameter[0].Value = objMetaTemplateFields.FieldName;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "FieldDataTypeID";
                objDbParameter[1].Value = objMetaTemplateFields.FieldDataTypeID;

                objDbParameter[2] = objDbProviderFactory.CreateParameter();
                objDbParameter[2].ParameterName = "FieldTypeID";
                objDbParameter[2].Value = objMetaTemplateFields.FieldTypeID;

                objDbParameter[3] = objDbProviderFactory.CreateParameter();
                objDbParameter[3].ParameterName = "FieldLength";
                objDbParameter[3].Value = objMetaTemplateFields.FieldLength;

                objDbParameter[4] = objDbProviderFactory.CreateParameter();
                objDbParameter[4].ParameterName = "IsPrimary";
                objDbParameter[4].Value = objMetaTemplateFields.IsPrimary;

                objDbParameter[5] = objDbProviderFactory.CreateParameter();
                objDbParameter[5].ParameterName = "RepositoryID";
                objDbParameter[5].Value = objMetaTemplateFields.RepositoryID;

                objDbParameter[6] = objDbProviderFactory.CreateParameter();
                objDbParameter[6].ParameterName = "MetaTemplateID";
                objDbParameter[6].Value = objMetaTemplateFields.MetaTemplateID;

                objDbParameter[7] = objDbProviderFactory.CreateParameter();
                objDbParameter[7].ParameterName = "CreatedOn";
                objDbParameter[7].Value = DateTime.Now;

                objDbParameter[8] = objDbProviderFactory.CreateParameter();
                objDbParameter[8].ParameterName = "CreatedBy";
                objDbParameter[8].Value = objMetaTemplateFields.CreatedBy;

                objDbParameter[9] = objDbProviderFactory.CreateParameter();
                objDbParameter[9].ParameterName = "UpdatedOn";
                objDbParameter[9].Value = DBNull.Value;

                objDbParameter[10] = objDbProviderFactory.CreateParameter();
                objDbParameter[10].ParameterName = "UpdatedBy";
                objDbParameter[10].Value = DBNull.Value;

                objDbParameter[11] = objDbProviderFactory.CreateParameter();
                objDbParameter[11].ParameterName = "Status";
                objDbParameter[11].Value = objMetaTemplateFields.Status;

                DataHelper.ExecuteNonQuery("SP_I_MetaTemplateFields", objDbTransaction, objDbParameter);
                objDbTransaction.Commit();
                objMetaTemplateFields.RepositoryID = Convert.ToInt32(objDbParameter[7].Value);

                return Utility.ResultType.Success;
            }
            catch (Exception ex)
            {
                objDbTransaction.Rollback();
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return Utility.ResultType.Error;
            }
        }
        // Update MetaTemplateFields
        public static Utility.ResultType Update(MetaTemplateFields objMetaTemplateFields)
        {
            DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;
            DbTransaction objDbTransaction = Utility.GetTransaction;
            try
            {

                DbParameter[] objDbParameter = new DbParameter[9];

                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "FieldName";
                objDbParameter[0].Value = objMetaTemplateFields.FieldName;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "FieldDataTypeID";
                objDbParameter[1].Value = objMetaTemplateFields.FieldDataTypeID;

                objDbParameter[2] = objDbProviderFactory.CreateParameter();
                objDbParameter[2].ParameterName = "FieldTypeID";
                objDbParameter[2].Value = objMetaTemplateFields.FieldTypeID;

                objDbParameter[3] = objDbProviderFactory.CreateParameter();
                objDbParameter[3].ParameterName = "FieldLength";
                objDbParameter[3].Value = objMetaTemplateFields.FieldLength;

                objDbParameter[4] = objDbProviderFactory.CreateParameter();
                objDbParameter[4].ParameterName = "IsPrimary";
                objDbParameter[4].Value = objMetaTemplateFields.IsPrimary;

                //objDbParameter[5] = objDbProviderFactory.CreateParameter();
                //objDbParameter[5].ParameterName = "RepositoryID";
                //objDbParameter[5].Value = objMetaTemplateFields.RepositoryID;

                //objDbParameter[6] = objDbProviderFactory.CreateParameter();
                //objDbParameter[6].ParameterName = "MetaTemplateID";
                //objDbParameter[6].Value = objMetaTemplateFields.MetaTemplateID;

                //objDbParameter[5] = objDbProviderFactory.CreateParameter();
                //objDbParameter[5].ParameterName = "CreatedOn";
                //objDbParameter[5].Value = DateTime.Now;

                //objDbParameter[6] = objDbProviderFactory.CreateParameter();
                //objDbParameter[6].ParameterName = "CreatedBy";
                //objDbParameter[6].Value = objMetaTemplateFields.CreatedBy;

                objDbParameter[5] = objDbProviderFactory.CreateParameter();
                objDbParameter[5].ParameterName = "UpdatedOn";
                objDbParameter[5].Value = DateTime.Now;

                objDbParameter[6] = objDbProviderFactory.CreateParameter();
                objDbParameter[6].ParameterName = "UpdatedBy";
                objDbParameter[6].Value = objMetaTemplateFields.UpdatedBy;

                objDbParameter[7] = objDbProviderFactory.CreateParameter();
                objDbParameter[7].ParameterName = "Status";
                objDbParameter[7].Value = objMetaTemplateFields.Status;

                objDbParameter[8] = objDbProviderFactory.CreateParameter();
                objDbParameter[8].ParameterName = "MetaTemplateFieldID";
                objDbParameter[8].Value = objMetaTemplateFields.MetaTemplateFieldsID;

                DataHelper.ExecuteNonQuery("SP_U_MetaTemplateFields", objDbTransaction, objDbParameter);
                objDbTransaction.Commit();
                objMetaTemplateFields.RepositoryID = Convert.ToInt32(objDbParameter[7].Value);

                return Utility.ResultType.Success;
            }
            catch (Exception ex)
            {
                objDbTransaction.Rollback();
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return Utility.ResultType.Error;
            }
        }        

        public static Utility.ResultType SelectDataTypeForDropDown(out DataTable objDataTable)
        {
            try
            {
                string strQuery = "SELECT ID,DataTypeName FROM vwDataType";
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

        public static Utility.ResultType Select(out DataTable objDataTable, int intMetaTemplateID)
        {
            try
            {
                //string strQuery = "SELECT ID,FieldName,FieldDataTypeID,FieldTypeID,FieldLength,IsPrimary FROM vwMetaTemplateFields WHERE MetaTemplateID = @MetaTemplateID AND Status=1";
                string strQuery = "SELECT * FROM vwMetaTemplateFields WHERE MetaTemplateID = @MetaTemplateID Order By ID ";// AND Status=1";

                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

                DbParameter[] objDbParameter = new DbParameter[1];
                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "MetaTemplateID";
                objDbParameter[0].Value = intMetaTemplateID;

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

        public static Utility.ResultType SelectFieldForDropDown(out DataTable objDataTable, int intMetaTemplateID)
        {
            try
            {
                string strQuery = "SELECT ID,FieldName,FieldDataTypeID FROM vwMetaTemplateFields WHERE MetaTemplateID = @MetaTemplateID AND Status=1";

                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

                DbParameter[] objDbParameter = new DbParameter[1];
                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "MetaTemplateID";
                objDbParameter[0].Value = intMetaTemplateID;

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
