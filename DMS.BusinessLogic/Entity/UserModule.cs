using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data;

namespace DMS.BusinessLogic
{
    public class UserModule
    {
         #region Private Members
        public int UserModuleID { get; set; }
        public int UserID { get; set; }
        public int ModuleID { get; set; }
        #endregion;

         #region Enum
        public enum DeleteIDType {UserID,RoleID};
        #endregion

        #region Methods

        public static Utility.ResultType Insert(UserModule objUserModule, DbTransaction objDbTransaction)
        {
            try
            {                
                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;
            
                DbParameter[] objDbParameter = new DbParameter[2];

                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "UserID";
                objDbParameter[0].Value = objUserModule.UserID;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "ModuleID";
                objDbParameter[1].Value = objUserModule.ModuleID;

                DataHelper.ExecuteNonQuery("SP_I_UserModule", objDbTransaction, objDbParameter);
              
                return Utility.ResultType.Success;
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return Utility.ResultType.Error;

            }
        }

        public static Utility.ResultType Delete(int intID, DbTransaction objDbTransaction,DeleteIDType enumDeleteIDType)
        {

            DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

            try
            {
                DbParameter[] objDbParameter = new DbParameter[2];

                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "ID";
                objDbParameter[0].Value = intID;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "DeleteIDType";
                objDbParameter[1].Value = enumDeleteIDType;

                DataHelper.ExecuteNonQuery("SP_D_UserModule", objDbTransaction, objDbParameter);

                return Utility.ResultType.Success;

            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return Utility.ResultType.Error;


            }
        }
   
        public static List<int>  SelectByUserID(int intUserID)
        {
                List<int> UserModules = new List<int>();

                string strQuery = "SELECT * FROM vwUserModule WHERE UserID =@UserID";

                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

                DbParameter[] objDbParameter = new DbParameter[1];
                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "UserID";
                objDbParameter[0].Value = intUserID;

                DataTable objDataTable = DataHelper.ExecuteDataTable(strQuery, objDbParameter);
         
               
                foreach (DataRow objDataRow in objDataTable.Rows)
                {
                    UserModules.Add(Convert.ToInt32(objDataRow["ModuleID"]));
               
                   
                }

                return UserModules;

        }

        public static Utility.ResultType SelectAllByUserID(out DataTable objDataTable, int intUserID)
        {
            try
            {
                string strQuery = " SELECT *" +
                                    " FROM vwModule Module INNER JOIN vwUserModule UserModule"
                                  + " ON  Module.ID=UserModule.ModuleID"
                                  + " AND UserModule.UserID=@UserID"
                                  + " WHERE Module.Status=1";

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

        #endregion

    }
}
