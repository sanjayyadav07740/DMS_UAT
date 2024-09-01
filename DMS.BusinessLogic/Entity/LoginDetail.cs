using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data;

namespace DMS.BusinessLogic
{
    public class LoginDetail
    {
        #region Properties
        public int LoginDetailID { get; set; }
        public int UserID { get; set; }
        public DateTime LoginTime { get; set; }
        public DateTime LogoutTime { get; set; }
        public string Remark { get; set; }
        #endregion Properties

        #region Method
        public static void Insert(LoginDetail objLoginDetail)
        {
            DbTransaction objDbTransaction = Utility.GetTransaction;
            try
            {
                
                 DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;
                 DbParameter[] objDbParameter = new DbParameter[5];

                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "LoginDetailID";
                objDbParameter[0].Size=50;
                objDbParameter[0].Direction= ParameterDirection.Output;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "UserID";
                objDbParameter[1].Value = objLoginDetail.UserID;

                objDbParameter[2] = objDbProviderFactory.CreateParameter();
                objDbParameter[2].ParameterName = "LoginTime";
                objDbParameter[2].Value = objLoginDetail.LoginTime;

                objDbParameter[3] = objDbProviderFactory.CreateParameter();
                objDbParameter[3].ParameterName = "LogoutTime";
                objDbParameter[3].Value = DBNull.Value;

                objDbParameter[4] = objDbProviderFactory.CreateParameter();
                objDbParameter[4].ParameterName = "Remark";
                objDbParameter[4].Value = objLoginDetail.Remark;

                DataHelper.ExecuteNonQuery("SP_I_LoginDetail", objDbTransaction, objDbParameter);

                objLoginDetail.LoginDetailID = Convert.ToInt32(objDbParameter[0].Value);

                objDbTransaction.Commit();
            }
            catch(Exception ex)
            {
                objDbTransaction.Rollback();
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        public static void Update(LoginDetail objLoginDetail)
        {
            DbTransaction objDbTransaction = Utility.GetTransaction;
            try
            {

                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;
                DbParameter[] objDbParameter = new DbParameter[3];

                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "LoginDetailID";
                objDbParameter[0].Value = objLoginDetail.LoginDetailID;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "LogoutTime";
                objDbParameter[1].Value = objLoginDetail.LogoutTime;

                objDbParameter[2] = objDbProviderFactory.CreateParameter();
                objDbParameter[2].ParameterName = "Remark";
                objDbParameter[2].Value = objLoginDetail.Remark;

                DataHelper.ExecuteNonQuery("SP_U_LoginDetail", objDbTransaction, objDbParameter);

                objDbTransaction.Commit();
            }
            catch (Exception ex)
            {
                objDbTransaction.Rollback();
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        public static string SelectLastLogin(LoginDetail objLoginDetail)
        {
            try
            {
                string strQuery = @"SELECT RESULT.LoginTime FROM 
                                    (SELECT ROW_NUMBER() OVER(ORDER BY ID DESC) AS RowNumber,LoginTime FROM vwLoginDetail WHERE UserID = @UserID) RESULT
                                    WHERE RESULT.RowNumber = 2";

                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

                DbParameter[] objDbParameter = new DbParameter[1];
                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "UserID";
                objDbParameter[0].Value = objLoginDetail.UserID;

                object objResult = DataHelper.ExecuteScalar(strQuery, objDbParameter);
                if (objResult == null)
                    return "First Login";
                else
                    return Convert.ToDateTime(objResult).ToString("f");
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return "Error";
            }
        }
        #endregion Method


    }
}
