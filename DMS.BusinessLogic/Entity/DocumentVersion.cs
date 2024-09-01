using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data;

namespace DMS.BusinessLogic.Entity
{
   public  class DocumentVersion
    {
        #region Properties
        public int DocumentVersionID { get; set; }
        public int DocumentID { get; set; }
        public string DocumentName { get; set; }
        public int DocumentSize { get; set; }
        public string DocumentPath { get; set; }
        public string DocumentType { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public int Status { get; set; }
        #endregion

        #region Method
        public static Utility.ResultType Insert(DocumentVersion objDocumentVersion, DbTransaction objDbTransaction)
        {
            DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

            try
            {
                DbParameter[] objDbParameter = new DbParameter[9];

                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "DocumentID";
                objDbParameter[0].Value = objDocumentVersion.DocumentID;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "DocumentName";
                objDbParameter[1].Value = objDocumentVersion.DocumentName;

                objDbParameter[2] = objDbProviderFactory.CreateParameter();
                objDbParameter[2].ParameterName = "DocumentPath";
                objDbParameter[2].Value = objDocumentVersion.DocumentPath;

                objDbParameter[3] = objDbProviderFactory.CreateParameter();
                objDbParameter[3].ParameterName = "DocumentSize";
                objDbParameter[3].Value = objDocumentVersion.DocumentSize;

                objDbParameter[4] = objDbProviderFactory.CreateParameter();
                objDbParameter[4].ParameterName = "DocumentType";
                objDbParameter[4].Value = objDocumentVersion.DocumentType;

                objDbParameter[5] = objDbProviderFactory.CreateParameter();
                objDbParameter[5].ParameterName = "CreatedOn";
                objDbParameter[5].Value = DateTime.Now;

                objDbParameter[6] = objDbProviderFactory.CreateParameter();
                objDbParameter[6].ParameterName = "CreatedBy";
                objDbParameter[6].Value = objDocumentVersion.CreatedBy;

                objDbParameter[7] = objDbProviderFactory.CreateParameter();
                objDbParameter[7].ParameterName = "Status";
                objDbParameter[7].Value = objDocumentVersion.Status;

                objDbParameter[8] = objDbProviderFactory.CreateParameter();
                objDbParameter[8].ParameterName = "DocumentVersionID";
                objDbParameter[8].Size = 100;
                objDbParameter[8].Direction = ParameterDirection.Output;

                DataHelper.ExecuteNonQuery("SP_I_DocumentVersion", objDbTransaction, objDbParameter);

                objDocumentVersion.DocumentVersionID = Convert.ToInt32(objDbParameter[8].Value);

                return Utility.ResultType.Success;
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return Utility.ResultType.Error;
            }
        }

        public static Utility.ResultType Select(out DataTable objDataTable, int intDocumentID)
        {
            try
            {
                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

                DbParameter[] objDbParameter = new DbParameter[1];
                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "DocumentID";
                objDbParameter[0].Value = intDocumentID;

                objDataTable = DataHelper.ExecuteDataTableForProcedure("SP_S_DocumentVersion", null, objDbParameter);

                return Utility.ResultType.Success;
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                objDataTable = null;
                return Utility.ResultType.Error;
            }
        }
        #endregion Method
    }
}
