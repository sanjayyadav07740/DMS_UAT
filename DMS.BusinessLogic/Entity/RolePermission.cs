using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;

namespace DMS.BusinessLogic
{
   public class RolePermission
   {
       # region Properties
       public int ID { get; set; }
       public int RoleID { get; set; }
       public int RepositoryID { get; set; }
       public int MetaTemplateID { get; set; }
       public int FolderID { get; set; }
       public int CategoryID { get; set; }
       public int UserId { get; set; }
       
       
       # endregion

       # region Methods

       public static Utility.ResultType Select(out DataTable objDataTable)
       {
           try
           {
               string strQuery = "SELECT * FROM vwRolePermission";
               
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

       public static Utility.ResultType SelectByRoleID(out DataTable objDataTable,int intRoleID)
       {
           try
           {
               string strQuery = "SELECT * FROM vwRolePermission WHERE RoleID =@RoleID";

               DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

               DbParameter[] objDbParameter = new DbParameter[1];
               objDbParameter[0] = objDbProviderFactory.CreateParameter();
               objDbParameter[0].ParameterName = "RoleID";
               objDbParameter[0].Value = intRoleID;

               objDataTable = DataHelper.ExecuteDataTable(strQuery,objDbParameter);
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

       public static Utility.ResultType Insert(RolePermission objRolePermission, DbTransaction objDbTransaction)
       {
           try
           {
               DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

               DbParameter[] objDbParameter = new DbParameter[6];

               objDbParameter[0] = objDbProviderFactory.CreateParameter();
               objDbParameter[0].ParameterName = "RoleID";
               objDbParameter[0].Value = objRolePermission.RoleID;

               objDbParameter[1] = objDbProviderFactory.CreateParameter();
               objDbParameter[1].ParameterName = "RepositoryID";
               objDbParameter[1].Value = objRolePermission.RepositoryID;

               objDbParameter[2] = objDbProviderFactory.CreateParameter();
               objDbParameter[2].ParameterName = "MetaTemplateID";
               objDbParameter[2].Value = objRolePermission.MetaTemplateID;


               objDbParameter[3] = objDbProviderFactory.CreateParameter();
               objDbParameter[3].ParameterName = "FolderID";
               objDbParameter[3].Value = objRolePermission.FolderID;

               objDbParameter[4] = objDbProviderFactory.CreateParameter();
               objDbParameter[4].ParameterName = "CategoryID";
               objDbParameter[4].Value = objRolePermission.CategoryID;

               objDbParameter[5] = objDbProviderFactory.CreateParameter();
               objDbParameter[5].ParameterName = "UserId";
               objDbParameter[5].Value = objRolePermission.UserId;


               DataHelper.ExecuteNonQuery("SP_I_RolePermission", objDbTransaction, objDbParameter);

               return Utility.ResultType.Success;
           }
           catch (Exception ex)
           {
               LogManager.ErrorLog(Utility.LogFilePath, ex);
               return Utility.ResultType.Error;

           }
       }

       public static Utility.ResultType DeleteByRoleId(int intRoleID,int intRepositoryID, DbTransaction objDbTransaction)
       {

           DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

           try
           {
               DbParameter[] objDbParameter = new DbParameter[2];

               objDbParameter[0] = objDbProviderFactory.CreateParameter();
               objDbParameter[0].ParameterName = "RoleID";
               objDbParameter[0].Value = intRoleID;

               objDbParameter[1] = objDbProviderFactory.CreateParameter();
               objDbParameter[1].ParameterName = "RepositoryID";
               objDbParameter[1].Value = intRepositoryID;

               DataHelper.ExecuteNonQuery("SP_D_RolePermission", objDbTransaction, objDbParameter);

               return Utility.ResultType.Success;

           }
           catch (Exception ex)
           {
               LogManager.ErrorLog(Utility.LogFilePath, ex);
               return Utility.ResultType.Error;


           }
       }
       #region added by Vivek 11-5-2018 for updating user who change roles
       public static Utility.ResultType UpdateByRoleId(int intRoleID,int RepositoryId, int intUserID, DbTransaction objDbTransaction)
       {

           DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

           try
           {
               DbParameter[] objDbParameter = new DbParameter[3];

               objDbParameter[0] = objDbProviderFactory.CreateParameter();
               objDbParameter[0].ParameterName = "RoleID";
               objDbParameter[0].Value = intRoleID;

               objDbParameter[1] = objDbProviderFactory.CreateParameter();
               objDbParameter[1].ParameterName = "UserId";
               objDbParameter[1].Value = intUserID;

               objDbParameter[2] = objDbProviderFactory.CreateParameter();
               objDbParameter[2].ParameterName = "RepoId";
               objDbParameter[2].Value = RepositoryId;

               DataHelper.ExecuteNonQuery("SP_U_RolePermission", objDbTransaction, objDbParameter);

               return Utility.ResultType.Success;

           }
           catch (Exception ex)
           {
               LogManager.ErrorLog(Utility.LogFilePath, ex);
               return Utility.ResultType.Error;


           }
       }
       #endregion
       # endregion

   }
}
