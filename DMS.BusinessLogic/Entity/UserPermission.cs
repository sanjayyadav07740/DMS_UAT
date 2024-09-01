using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data;

namespace DMS.BusinessLogic
{
    public class UserPermission
    {
        public int UserPermissionID { get; set; }
        public int UserID { get; set; }
        public int RepositoryID { get; set; }
        public int MetaTemplateID { get; set; }
        public int FolderID { get; set; }
        public int CategoryID { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public int UpdatedBy { get; set; }

        #region Enum
        public enum DeleteIDType { UserID, RoleID };
        #endregion

          public static Utility.ResultType Select(out DataTable objDataTable,int intUserID)
        {
           
                try
                {
                    string strQuery = "SELECT * FROM vwUserPermission WHERE UserID =@UserID";

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

          public static DataSet SelectByRepId(int intRepID)
          {

              try
              {
                  string strQuery = @"select ID,UserName from vwuser where roleid in (select distinct R.ID
                                      from Role R inner join RolePermission RP
                                      on R.ID=RP.RoleID  and RP.RepositoryID='"+intRepID+"' and R.id!=1) ";

                  DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

                  DataSet ds = new DataSet();
                  ds=DataHelper.ExecuteDataSet(strQuery);
                  return ds;
                 

              }
              catch (Exception ex)
              {
                  LogManager.ErrorLog(Utility.LogFilePath, ex);
                  return null;
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

           
                  DataHelper.ExecuteNonQuery("SP_D_UserPermission", objDbTransaction, objDbParameter);

                  return Utility.ResultType.Success;

              }
              catch (Exception ex)
              {
                  LogManager.ErrorLog(Utility.LogFilePath, ex);
                  return Utility.ResultType.Error;


              }
          }

          public static Utility.ResultType Insert(UserPermission objUserPermission, DbTransaction objDbTransaction)
          {
              try
              {
                  DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

                  DbParameter[] objDbParameter = new DbParameter[5];

                  objDbParameter[0] = objDbProviderFactory.CreateParameter();
                  objDbParameter[0].ParameterName = "UserID";
                  objDbParameter[0].Value = objUserPermission.UserID;

                  objDbParameter[1] = objDbProviderFactory.CreateParameter();
                  objDbParameter[1].ParameterName = "RepositoryID";
                  objDbParameter[1].Value = objUserPermission.RepositoryID;

                  objDbParameter[2] = objDbProviderFactory.CreateParameter();
                  objDbParameter[2].ParameterName = "MetaTemplateID";
                  objDbParameter[2].Value = objUserPermission.MetaTemplateID;


                  objDbParameter[3] = objDbProviderFactory.CreateParameter();
                  objDbParameter[3].ParameterName = "FolderID";
                  objDbParameter[3].Value = objUserPermission.FolderID;

                  objDbParameter[4] = objDbProviderFactory.CreateParameter();
                  objDbParameter[4].ParameterName = "CategoryID";
                  objDbParameter[4].Value = objUserPermission.CategoryID;



                  DataHelper.ExecuteNonQuery("SP_I_UserPermission", objDbTransaction, objDbParameter);

                  return Utility.ResultType.Success;
              }
              catch (Exception ex)
              {
                  LogManager.ErrorLog(Utility.LogFilePath, ex);
                  return Utility.ResultType.Error;

              }
          }
      
    }
}
