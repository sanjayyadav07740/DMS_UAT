using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DMS.BusinessLogic;
using System.Data.Common;

namespace DMS.BusinessLogic
{
   public class MetaTemplateItem
    {
        #region Properties
        public int ListItemID { get; set; }
        public int FieldID { get; set; }
        public string ListItemText { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public int UpdatedBy { get; set; }
        //public string ListItemValue { get; set; }
        #endregion

        #region Method
        public static Utility.ResultType Select(out DataTable objDataTable, int intMetaTemplateID)
        {
            try
            {
                string strQuery = "SELECT * FROM vwListItem WHERE FieldID IN (SELECT ID FROM vwMetaTemplateFields WHERE MetaTemplateID = @MetaTemplateID) AND Status=1";

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

        //Load Select MetaTemplate ListItems By MetaTemplateID
        public static Utility.ResultType SelectListItems(out DataTable objDataTable, int intMetaTemplateID)
        {
            try
            {
                string strQuery = "select vwMTF.fieldName,vwLI.ListItemText,vwMTF.FieldDataTypeID,vwMTF.ID from vwListItem vwLI "+
                                    "inner join vwMetaTemplateFields vwMTF on vwMTF.ID=vwLI.FieldID where vwMTF.MetaTemplateID=@MetaTemplateID ";

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

        public static Utility.ResultType SelectByFieldID(out DataTable objDataTable, int intFieldID)
        {
            try
            {
                string strQuery = "SELECT * FROM vwListItem WHERE FieldID =@FieldID";

                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

                DbParameter[] objDbParameter = new DbParameter[1];
                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "FieldID";
                objDbParameter[0].Value = intFieldID;

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

        public static Utility.ResultType Insert(MetaTemplateItem objMetaTemplateItem)
        {
            DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;
            DbTransaction objDbTransaction = Utility.GetTransaction;
            try
            {

                DbParameter[] objDbParameter = new DbParameter[6];

                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "FieldID";
                objDbParameter[0].Value = objMetaTemplateItem.FieldID;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "ListItemText";
                objDbParameter[1].Value = objMetaTemplateItem.ListItemText;            

                objDbParameter[2] = objDbProviderFactory.CreateParameter();
                objDbParameter[2].ParameterName = "CreatedOn";
                objDbParameter[2].Value = DateTime.Now;

                objDbParameter[3] = objDbProviderFactory.CreateParameter();
                objDbParameter[3].ParameterName = "CreatedBy";
                objDbParameter[3].Value = objMetaTemplateItem.CreatedBy;

                objDbParameter[4] = objDbProviderFactory.CreateParameter();
                objDbParameter[4].ParameterName = "UpdatedOn";
                objDbParameter[4].Value = DBNull.Value;

                objDbParameter[5] = objDbProviderFactory.CreateParameter();
                objDbParameter[5].ParameterName = "UpdatedBy";
                objDbParameter[5].Value = DBNull.Value;                

                DataHelper.ExecuteNonQuery("SP_I_MetaTemplateFieldsItems", objDbTransaction, objDbParameter);
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
       
        #endregion
    }
}
