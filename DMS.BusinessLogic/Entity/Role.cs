using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data;

namespace DMS.BusinessLogic
{
    public class RoleRights
    {
        public int RoleRightsID { get; set; }
        public int RepositoryAdminID { get; set; }
        public int RepositoryUserID { get; set; }
    }

    public class Role
    {
        #region Properties
        public int RoleID { get; set; }
        public string RoleName { get; set; }
        public string DisplayName { get; set; }
        public string Status { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public int UpdatedBy { get; set; }
        public int RoleType { get; set; }
        #endregion

        # region Methods

        public static Utility.ResultType Select(out DataTable objDataTable)
        {
            try
            {
             
                  char charQuote = '"';
                  string strQuery = "SELECT ID,RoleName,DisplayName,"
                                   +"(SELECT SettingName FROM vwAppSetting WHERE SettingType='STATUS' AND SettingValue=vwRole.Status) AS "+charQuote+"Status"+charQuote+","
                                   + "(SELECT SettingName FROM vwAppSetting WHERE SettingType='ROLETYPE' AND SettingValue=vwRole.RoleType) AS " + charQuote + "RoleType" + charQuote + ","
                                   +"CreatedOn,"
                                   +"(SELECT FirstName FROM vwUser WHERE ID = vwRole.CreatedBy) AS " + charQuote + "CreatedBy" + charQuote + "," 
                                   +"UpdatedOn,"
                                   + "(SELECT FirstName FROM vwUser WHERE ID = vwRole.UpdatedBy) AS " + charQuote + "UpdatedBy" + charQuote + ""
                                   +" FROM vwRole ORDER BY ID DESC";

                objDataTable = DataHelper.ExecuteDataTable(strQuery, null);

                if (objDataTable.Rows.Count == 0)
                {
                    return Utility.ResultType.Failure;
                }
                return Utility.ResultType.Success;
            }
            catch(Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                objDataTable = null;
                return Utility.ResultType.Error;

            }

        }

        public static DataSet SearchRoleByRep(int RepId)
        {
            try
            {
                char charQuote = '"';
                string strQuery = "SELECT distinct vwRole.ID,RoleName,DisplayName,"
                                   + "(SELECT SettingName FROM vwAppSetting WHERE SettingType='STATUS' AND SettingValue=vwRole.Status) AS " + charQuote + "Status" + charQuote + ","
                                   + "(SELECT SettingName FROM vwAppSetting WHERE SettingType='ROLETYPE' AND SettingValue=vwRole.RoleType) AS " + charQuote + "RoleType" + charQuote + ","
                                   + "CreatedOn,"
                                   + "(SELECT FirstName FROM vwUser WHERE ID = vwRole.CreatedBy) AS " + charQuote + "CreatedBy" + charQuote + ","
                                   + "UpdatedOn,"
                                   + "(SELECT FirstName FROM vwUser WHERE ID = vwRole.UpdatedBy) AS " + charQuote + "UpdatedBy" + charQuote + ""
                                   + " FROM vwRole inner join RolePermission RP on vwRole.ID=RP.roleid"
                                   + " where RP.repositoryid=" + RepId + " ORDER BY vwRole.ID DESC";

                DataSet ds = new DataSet();
                ds = DataHelper.ExecuteDataSet(strQuery);
                return ds;
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return null;
            }
        }

        public static DataSet SelectRole(int RoleId)
        {
            try
            {
                string strQuery = "select * from RolePermission where RoleID='"+RoleId+"'";
                DataSet ds = new DataSet();
                ds = DataHelper.ExecuteDataSet(strQuery);
                return ds;
            }
            catch(Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return null;
            }
        }
        public static DataSet SelectRoleLike(string RoleName)
        {
            try
            {
                string strQuery = @"select distinct R.RoleName,R.DisplayName,R.RoleType,R.CreatedOn,R.id,U.UserName as CreatedBy,
                                    R.UpdatedOn,R.UpdatedBy,R.Status
                                    from RolePermission RP inner join Role R on RP.RoleID=R.ID 
                                    inner join vwUser U on R.CreatedBy=U.ID
                                    and UPPER(R.RoleName) like'%" + RoleName+"%'";
                DataSet ds = new DataSet();
                ds = DataHelper.ExecuteDataSet(strQuery);
                return ds;
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return null;
            }
        }

        public static Utility.ResultType SelectByRoleID(out DataTable objDataTable, int intRoleID)
        {
            try
            {
                char charQuote = '"';
                string strQuery = "";
                if(intRoleID==202)
                {
                    strQuery = "SELECT distinct vwRole.ID,RoleName,DisplayName,"
                                  + "(SELECT SettingName FROM vwAppSetting WHERE SettingType='STATUS' AND SettingValue=vwRole.Status) AS " + charQuote + "Status" + charQuote + ","
                                  + "(SELECT SettingName FROM vwAppSetting WHERE SettingType='ROLETYPE' AND SettingValue=vwRole.RoleType) AS " + charQuote + "RoleType" + charQuote + ","
                                  + "CreatedOn,"
                                  + "(SELECT FirstName FROM vwUser WHERE ID = vwRole.CreatedBy) AS " + charQuote + "CreatedBy" + charQuote + ","
                                  + "UpdatedOn,"
                                  + "(SELECT FirstName FROM vwUser WHERE ID = vwRole.UpdatedBy) AS " + charQuote + "UpdatedBy" + charQuote + ""
                                  + " FROM vwRole inner join RolePermission on vwRole.ID=RolePermission.RoleID  where RolePermission.RepositoryID=53 ORDER BY vwRole.ID DESC";
                }
                else if (intRoleID == 131)
                {
                      strQuery = "SELECT ID,RoleName,DisplayName,"
                                   +"(SELECT SettingName FROM vwAppSetting WHERE SettingType='STATUS' AND SettingValue=vwRole.Status) AS "+charQuote+"Status"+charQuote+","
                                   + "(SELECT SettingName FROM vwAppSetting WHERE SettingType='ROLETYPE' AND SettingValue=vwRole.RoleType) AS " + charQuote + "RoleType" + charQuote + ","
                                   +"CreatedOn,"
                                   +"(SELECT FirstName FROM vwUser WHERE ID = vwRole.CreatedBy) AS " + charQuote + "CreatedBy" + charQuote + "," 
                                   +"UpdatedOn,"
                                   + "(SELECT FirstName FROM vwUser WHERE ID = vwRole.UpdatedBy) AS " + charQuote + "UpdatedBy" + charQuote + ""
                                   + " FROM vwRole ORDER BY ID DESC";
                }
                else
                strQuery = "SELECT * FROM vwRole LEFT OUTER JOIN vwRoleRights ON vwRole.ID = vwRoleRights.RepositoryAdminID WHERE vwRole.ID = @ID";

                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

                DbParameter[] objDbParameter = new DbParameter[1];
                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "ID";
                objDbParameter[0].Value = intRoleID;

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

        public static Utility.ResultType SelectByRoleName(string strRoleName)
        {
            try
            {
                string strQuery = "SELECT * FROM vwRole WHERE UPPER(RoleName)=@RoleName";
                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

                DbParameter[] objDbParameter = new DbParameter[1];
                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "RoleName";
                objDbParameter[0].Value = strRoleName.ToUpper().Trim();

                object objResult = DataHelper.ExecuteScalar(strQuery, objDbParameter);
                if (objResult == null)
                {
                    return Utility.ResultType.Failure;
                }
                return Utility.ResultType.Success;

            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return Utility.ResultType.Error;
            }
        }
              
        public static Utility.ResultType Insert(Role objRole)
        {
            DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;
            DbTransaction objDbTransaction = Utility.GetTransaction;
            try
            {
                DbParameter[] objDbParameter = new DbParameter[7];

                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "RoleName";
                objDbParameter[0].Value = objRole.RoleName;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "DisplayName";
                objDbParameter[1].Value = objRole.DisplayName;

                objDbParameter[2] = objDbProviderFactory.CreateParameter();
                objDbParameter[2].ParameterName = "Status";
                objDbParameter[2].Value = Convert.ToInt32(objRole.Status);
                                
                objDbParameter[3] = objDbProviderFactory.CreateParameter();
                objDbParameter[3].ParameterName = "CreatedOn";
                objDbParameter[3].Value = DateTime.Now;

                objDbParameter[4] = objDbProviderFactory.CreateParameter();
                objDbParameter[4].ParameterName = "CreatedBy";
                objDbParameter[4].Value = objRole.CreatedBy;

                objDbParameter[5] = objDbProviderFactory.CreateParameter();
                objDbParameter[5].ParameterName = "RoleType";
                objDbParameter[5].Value = objRole.RoleType;

                objDbParameter[6] = objDbProviderFactory.CreateParameter();
                objDbParameter[6].ParameterName = "RoleID";
                objDbParameter[6].Size = 100;
                objDbParameter[6].Direction = ParameterDirection.Output;

                DataHelper.ExecuteNonQuery("SP_I_Role", objDbTransaction, objDbParameter);

                objRole.RoleID = Convert.ToInt32(objDbParameter[6].Value);

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

        public static Utility.ResultType Update(Role objRole)
        {
            DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;
            DbTransaction objDbTransaction = Utility.GetTransaction;


            try
            {
                DbParameter[] objDbParameter = new DbParameter[7];

                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "RoleID";
                objDbParameter[0].Value = objRole.RoleID;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "RoleName";
                objDbParameter[1].Value = objRole.RoleName;

                objDbParameter[2] = objDbProviderFactory.CreateParameter();
                objDbParameter[2].ParameterName = "DisplayName";
                objDbParameter[2].Value = objRole.DisplayName;

                objDbParameter[3] = objDbProviderFactory.CreateParameter();
                objDbParameter[3].ParameterName = "Status";
                objDbParameter[3].Value = int.Parse(objRole.Status);

                objDbParameter[4] = objDbProviderFactory.CreateParameter();
                objDbParameter[4].ParameterName = "UpdatedOn";
                objDbParameter[4].Value = DateTime.Now;

                objDbParameter[5] = objDbProviderFactory.CreateParameter();
                objDbParameter[5].ParameterName = "UpdatedBy";
                objDbParameter[5].Value = objRole.UpdatedBy;

                objDbParameter[6] = objDbProviderFactory.CreateParameter();
                objDbParameter[6].ParameterName = "RoleType";
                objDbParameter[6].Value = objRole.RoleType;

                DataHelper.ExecuteNonQuery("SP_U_Role", objDbTransaction, objDbParameter);
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

        public static Utility.ResultType SelectRepositoryUsers(out DataTable objDataTable)
        {
            try
            {

                string strQuery = "SELECT ID,RoleName FROM vwRole WHERE RoleType=3 AND Status = 1";

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

        public static Utility.ResultType InsertRoleRights(RoleRights objRoleRights)
        {
            DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;
            DbTransaction objDbTransaction = Utility.GetTransaction;
            try
            {
                DbParameter[] objDbParameter = new DbParameter[2];

                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "RepositoryAdminID";
                objDbParameter[0].Value = objRoleRights.RepositoryAdminID;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "RepositoryUserID";
                objDbParameter[1].Value = objRoleRights.RepositoryUserID;

                DataHelper.ExecuteNonQuery("SP_I_RoleRights", objDbTransaction, objDbParameter);
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

        public static Utility.ResultType DeleteRoleRights(RoleRights objRoleRights)
        {
            DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;
            DbTransaction objDbTransaction = Utility.GetTransaction;
            try
            {
                DbParameter[] objDbParameter = new DbParameter[1];

                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "RepositoryAdminID";
                objDbParameter[0].Value = objRoleRights.RepositoryAdminID;

                DataHelper.ExecuteNonQuery("SP_D_RoleRights", objDbTransaction, objDbParameter);
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

        public static Utility.ResultType InsertRolePermission(int RoleID, int RepositoryID, int MetatemplateID, int CategoryID, int FolderID, int UpdatedBy)
        {
            DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;
            DbTransaction objDbTransaction = Utility.GetTransaction;
            try
            {
                DbParameter[] objDbParameter = new DbParameter[6];

                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "RoleID";
                objDbParameter[0].Value = RoleID;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "RepositoryID";
                objDbParameter[1].Value = RepositoryID;

                objDbParameter[2] = objDbProviderFactory.CreateParameter();
                objDbParameter[2].ParameterName = "MetaTemplateID";
                objDbParameter[2].Value = MetatemplateID;

                objDbParameter[3] = objDbProviderFactory.CreateParameter();
                objDbParameter[3].ParameterName = "CategoryID";
                objDbParameter[3].Value = CategoryID;

                objDbParameter[4] = objDbProviderFactory.CreateParameter();
                objDbParameter[4].ParameterName = "FolderID";
                objDbParameter[4].Value = FolderID;

                objDbParameter[5] = objDbProviderFactory.CreateParameter();
                objDbParameter[5].ParameterName = "UserId";
                objDbParameter[5].Value = UpdatedBy;

                DataHelper.ExecuteNonQuery("SP_I_RolePermission", objDbTransaction, objDbParameter);

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


        # endregion
    }
}
