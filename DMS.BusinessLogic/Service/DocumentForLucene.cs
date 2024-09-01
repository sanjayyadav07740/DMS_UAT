using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;

namespace DMS.BusinessLogic.Service
{
    public class DocumentForLucene
    {
        public Utility.ResultType GetAllDocumentToBeLucened(out DataTable objDataTable)
        {
            try
            {
                string strQuery = "SELECT * FROM vwDocument WHERE IsLucened = 0";
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

        public void UpdateDocumentIsLucenedValue(int intDocumentID, int intIsLucened)
        {
            DbTransaction objDbTransaction = Utility.GetTransaction;
            try
            {
                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

                DbParameter[] objDbParameter = new DbParameter[2];
                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "DocumentID";
                objDbParameter[0].Value = intDocumentID;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "IsLucened";
                objDbParameter[1].Value = intIsLucened;

                DataHelper.ExecuteNonQuery("SP_U_Document_IsLucened", objDbTransaction, objDbParameter);
                objDbTransaction.Commit();
            }
            catch (Exception ex)
            {
                objDbTransaction.Rollback();
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        public Utility.ResultType StartLucene()
        {
            try
            {
                DataTable objDataTable;
                Utility objUtility = new Utility();
                objUtility.Result = GetAllDocumentToBeLucened(out objDataTable);
                if (objUtility.Result == Utility.ResultType.Error)
                {
                    return Utility.ResultType.Error;
                }

                foreach (DataRow objDataRow in objDataTable.Rows)
                {
                    Lucene objLucene = new Lucene();
                    string strText = string.Empty;
                    if (objDataRow["Image"].ToString().Trim() != string.Empty)
                    {
                        objUtility.Result = objLucene.ReadPdfFile(objDataRow["DocumentPath"].ToString(), (byte[])objDataRow["Image"], out strText);
                    }
                    else
                    {
                        objUtility.Result = objLucene.ReadPdfFile(objDataRow["DocumentPath"].ToString(), new byte[0], out strText);
                    }
                    if (objUtility.Result == Utility.ResultType.Success)
                    {
                        objLucene.CreateLuceneIndex(int.Parse(objDataRow["ID"].ToString()), strText.ToLower().Trim());
                    }

                    switch (objUtility.Result)
                    {
                        case Utility.ResultType.Success:
                            UpdateDocumentIsLucenedValue(int.Parse(objDataRow["ID"].ToString()), 1);
                            break;
                        case Utility.ResultType.Failure:
                            UpdateDocumentIsLucenedValue(int.Parse(objDataRow["ID"].ToString()), 2);
                            break;
                        case Utility.ResultType.Error:
                            UpdateDocumentIsLucenedValue(int.Parse(objDataRow["ID"].ToString()), 3);
                            break;
                    }

                }
                return Utility.ResultType.Success;
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex + " ===== MESSAGE =====" + ex.Message);
                return Utility.ResultType.Error;
            }
        }
    }
}
