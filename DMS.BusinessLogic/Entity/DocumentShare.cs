using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;


namespace DMS.BusinessLogic
{
    public class DocumentShare
    {
        #region Properties
        public int DocumentShareID { get; set; }
        public string UserType { get; set; }
        public int UserID { get; set; }
        public string EmailID { get; set; }
        public string Password { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public int AccessType { get; set; }
        public int IsActive { get; set; }       
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public int UpdatedBy { get; set; }
       
        public int Document_ID { get; set; }
        public int FolderID { get; set; }
        #endregion

        #region Method

        



        public static Utility.ResultType InsertDocumentShare(DocumentShare objDocument)
        {
            DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;
            DbTransaction objDbTransaction = Utility.GetTransaction;
            try
            {
                DbParameter[] objDbParameter = new DbParameter[11];

                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "UserType";
                objDbParameter[0].Value = objDocument.UserType;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "UserID";
                objDbParameter[1].Value = objDocument.UserID;


                objDbParameter[2] = objDbProviderFactory.CreateParameter();
                objDbParameter[2].ParameterName = "EmailId";
                objDbParameter[2].Value = objDocument.EmailID;

                objDbParameter[3] = objDbProviderFactory.CreateParameter();
                objDbParameter[3].ParameterName = "Password";
                objDbParameter[3].Value = objDocument.Password;

               
                objDbParameter[4] = objDbProviderFactory.CreateParameter();
                objDbParameter[4].ParameterName = "AccessType";
                objDbParameter[4].Value = objDocument.AccessType;

                objDbParameter[5] = objDbProviderFactory.CreateParameter();
                objDbParameter[5].ParameterName = "IsActive";
                objDbParameter[5].Value = DBNull.Value;
                

                objDbParameter[6] = objDbProviderFactory.CreateParameter();
                objDbParameter[6].ParameterName = "CreatedOn";
                objDbParameter[6].Value = DateTime.Now;

                objDbParameter[7] = objDbProviderFactory.CreateParameter();
                objDbParameter[7].ParameterName = "CreatedBy";
                objDbParameter[7].Value = objDocument.CreatedBy;

                objDbParameter[8] = objDbProviderFactory.CreateParameter();
                objDbParameter[8].ParameterName = "UpdatedOn";
                objDbParameter[8].Value = DBNull.Value;

                objDbParameter[9] = objDbProviderFactory.CreateParameter();
                objDbParameter[9].ParameterName = "UpdatedBy";
                objDbParameter[9].Value = DBNull.Value;

              
                objDbParameter[10] = objDbProviderFactory.CreateParameter();
                objDbParameter[10].ParameterName = "DocumentShareID";
                objDbParameter[10].Size = 100;
                objDbParameter[10].Direction = ParameterDirection.Output;

                DataHelper.ExecuteNonQuery("SP_I_DocumentShare", objDbTransaction, objDbParameter);
                objDbTransaction.Commit();

                objDocument.DocumentShareID = Convert.ToInt32(objDbParameter[10].Value);


                return Utility.ResultType.Success;
            }
            catch (Exception ex)
            {

                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return Utility.ResultType.Error;
            }
        }



        public static Utility.ResultType InsertDocumentShareDetails(DocumentShare objDocument)
        {
            DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;
            DbTransaction objDbTransaction = Utility.GetTransaction;
            try
            {
                DbParameter[] objDbParameter = new DbParameter[6];

                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "DocumentShareID";
                objDbParameter[0].Value = objDocument.DocumentShareID;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "DocumentID";
                objDbParameter[1].Value = objDocument.Document_ID;

                objDbParameter[2] = objDbProviderFactory.CreateParameter();
                objDbParameter[2].ParameterName = "FolderId";
                objDbParameter[2].Value = objDocument.FolderID;

                objDbParameter[4] = objDbProviderFactory.CreateParameter();
                objDbParameter[4].ParameterName = "FromDate";
                objDbParameter[4].Value = objDocument.FromDate;

                objDbParameter[5] = objDbProviderFactory.CreateParameter();
                objDbParameter[5].ParameterName = "ToDate";
                objDbParameter[5].Value = objDocument.ToDate;
                
                DataHelper.ExecuteNonQuery("SP_I_DocumentShareDetails", objDbTransaction, objDbParameter);
                objDbTransaction.Commit();


                return Utility.ResultType.Success;
            }
            catch (Exception ex)
            {

                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return Utility.ResultType.Error;
            }
        }

        public static Utility.ResultType GetDocumentByDocumentShareID(out DataTable objDataTable, int DocumentShareID)
        {
            DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;
            DbTransaction objDbTransaction = Utility.GetTransaction;
            try
            {
                DbParameter[] objDbParameter = new DbParameter[1];

                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "@DocumentShareID";
                objDbParameter[0].Value = DocumentShareID;

                objDataTable = DataHelper.ExecuteDataTableNew("U_Sp_DocumentDetailsByShareDocumentId", objDbParameter);
                objDbTransaction.Commit();

                return Utility.ResultType.Success;
            }
            catch (Exception ex)
            {
                objDbTransaction.Rollback();
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                objDataTable = null;
                return Utility.ResultType.Error;
            }

        }

        public static Utility.ResultType FolderWiseDocumentBind(out DataTable objDataTable, int FolderID)
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

                DbParameter[] objDbParameter = new DbParameter[1];
                
                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "FolderID";
                objDbParameter[0].Value = FolderIDFinal;

                //objDataTable = DataHelper.ExecuteDataTable(strQuery, objDbParameter);

                objDataTable = DataHelper.ExecuteDataTableNew("Sp_S_FolderWiseDocumentBind", objDbParameter);
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

