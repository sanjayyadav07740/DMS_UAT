using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data;

namespace DMS.BusinessLogic
{
    public class MetaTemplate
    {
        #region Properties
        public int MetaTemplateID { get; set; }
        public string MetaTemplateName { get; set; }
        public string MetaTemplateDescription { get; set; }
        public int RepositoryID { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public int UpdatedBy { get; set; }
        public int Status { get; set; }
        #endregion

        #region Method
        public static Utility.ResultType Insert(MetaTemplate objMetaTemplate)
        {
            DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;
            DbTransaction objDbTransaction = Utility.GetTransaction;
            try
            {

                DbParameter[] objDbParameter = new DbParameter[9];

                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "MetaTemplateName";
                objDbParameter[0].Value = objMetaTemplate.MetaTemplateName;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "MetaTemplateDescription";
                objDbParameter[1].Value = objMetaTemplate.MetaTemplateDescription;

                objDbParameter[2] = objDbProviderFactory.CreateParameter();
                objDbParameter[2].ParameterName = "RepositoryID";
                objDbParameter[2].Value = objMetaTemplate.RepositoryID;

                objDbParameter[3] = objDbProviderFactory.CreateParameter();
                objDbParameter[3].ParameterName = "CreatedOn";
                objDbParameter[3].Value = DateTime.Now;

                objDbParameter[4] = objDbProviderFactory.CreateParameter();
                objDbParameter[4].ParameterName = "CreatedBy";
                objDbParameter[4].Value = objMetaTemplate.CreatedBy;

                objDbParameter[5] = objDbProviderFactory.CreateParameter();
                objDbParameter[5].ParameterName = "UpdatedOn";
                objDbParameter[5].Value = DBNull.Value;

                objDbParameter[6] = objDbProviderFactory.CreateParameter();
                objDbParameter[6].ParameterName = "UpdatedBy";
                objDbParameter[6].Value = DBNull.Value;

                objDbParameter[7] = objDbProviderFactory.CreateParameter();
                objDbParameter[7].ParameterName = "Status";
                objDbParameter[7].Value = objMetaTemplate.Status;

                objDbParameter[8] = objDbProviderFactory.CreateParameter();
                objDbParameter[8].ParameterName = "MetaTemplateID";
                objDbParameter[8].Size = 50;
                objDbParameter[8].Direction = System.Data.ParameterDirection.Output;

                DataHelper.ExecuteNonQuery("SP_I_MetaTemplate", objDbTransaction, objDbParameter);
                objDbTransaction.Commit();
                objMetaTemplate.MetaTemplateID = Convert.ToInt32(objDbParameter[8].Value);
                Utility.InsertAccessRight(0, objMetaTemplate.MetaTemplateID, 0, 0);
                return Utility.ResultType.Success;
            }
            catch (Exception ex)
            {
                objDbTransaction.Rollback();
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return Utility.ResultType.Error;
            }
        }

        public static Utility.ResultType Update(MetaTemplate objMetaTemplate)
        {
            DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;
            DbTransaction objDbTransaction = Utility.GetTransaction;
            try
            {

                DbParameter[] objDbParameter = new DbParameter[6];

                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "MetaTemplateName";
                objDbParameter[0].Value = objMetaTemplate.MetaTemplateName;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "MetaTemplateDescription";
                objDbParameter[1].Value = objMetaTemplate.MetaTemplateDescription;

                objDbParameter[2] = objDbProviderFactory.CreateParameter();
                objDbParameter[2].ParameterName = "UpdatedOn";
                objDbParameter[2].Value = DateTime.Now;

                objDbParameter[3] = objDbProviderFactory.CreateParameter();
                objDbParameter[3].ParameterName = "UpdatedBy";
                objDbParameter[3].Value = objMetaTemplate.UpdatedBy;

                objDbParameter[4] = objDbProviderFactory.CreateParameter();
                objDbParameter[4].ParameterName = "Status";
                objDbParameter[4].Value = objMetaTemplate.Status;

                objDbParameter[5] = objDbProviderFactory.CreateParameter();
                objDbParameter[5].ParameterName = "MetaTemplateID";
                objDbParameter[5].Value = objMetaTemplate.MetaTemplateID;

                DataHelper.ExecuteNonQuery("SP_U_MetaTemplate", objDbTransaction, objDbParameter);
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

        public static Utility.ResultType Select(out DataTable objDataTable)
        {
            try
            {
                string strQuery = "SELECT * FROM vwMetaTemplate WHERE Status=1";
                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

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
                string strQuery = "SELECT * FROM vwMetaTemplate WHERE ID =@ID";
                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

                DbParameter[] objDbParameter = new DbParameter[1];
                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "ID";
                objDbParameter[0].Value = intMetaTemplateID;
                objDataTable = DataHelper.ExecuteDataTable(strQuery, objDbParameter);

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

        public static Utility.ResultType SelectByRepositoryID(out DataTable objDataTable, int intRepositoryID)
        {
            try
            {
                //string strQuery = "SELECT * FROM vwMetaTemplate WHERE RepositoryID =@RepositoryID ORDER BY ID DESC";

                //string strQuery = "SELECT ID,MetaTemplateName,MetaTemplateDescription,RepositoryID,CreatedOn,dbo.getUserName(CreatedBy) as CreatedBy,UpdatedOn,dbo.getUserName(UpdatedBy) as UpdatedBy,dbo.getStatus(Status) as Status FROM vwMetaTemplate WHERE RepositoryID =@RepositoryID AND ID IN (" + Utility.GetAccessRight(Utility.AccessRight.MetaTemplate) + ") ORDER BY ID DESC";
                char charQuote = '"';
                string strQuery = "SELECT ID,MetaTemplateName,MetaTemplateDescription,RepositoryID, "
                                + "(SELECT SettingName FROM vwAppSetting WHERE SettingType='STATUS' AND SettingValue=vwMetaTemplate.Status) AS " + charQuote + "Status" + charQuote + ","
                                + "CreatedOn,"
                                + "(SELECT FirstName FROM vwUser WHERE ID = vwMetaTemplate.CreatedBy) AS " + charQuote + "CreatedBy" + charQuote + ","
                                + "UpdatedOn,"
                                + "(SELECT FirstName FROM vwUser WHERE ID = vwMetaTemplate.UpdatedBy) AS " + charQuote + "UpdatedBy" + charQuote + " FROM vwMetaTemplate WHERE RepositoryID =@RepositoryID AND ID IN (" + Utility.GetAccessRight(Utility.AccessRight.MetaTemplate) + ") ORDER BY ID DESC";
                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

                DbParameter[] objDbParameter = new DbParameter[1];
                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "RepositoryID";
                objDbParameter[0].Value = intRepositoryID;
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

        public static Utility.ResultType SelectByMetaTemplateName(string strMetaTemplateName, int intRepositoryID)
        {
            try
            {
                string strQuery = "SELECT MetaTemplateName FROM vwMetaTemplate WHERE UPPER(MetaTemplateName) =@MetaTemplateName AND RepositoryID = @RepositoryID";
                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

                DbParameter[] objDbParameter = new DbParameter[2];
                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "MetaTemplateName";
                objDbParameter[0].Value = strMetaTemplateName.ToUpper().Trim();

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "RepositoryID";
                objDbParameter[1].Value = intRepositoryID;

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

        public static Utility.ResultType SelectMetaTemplateForDropDown(out DataTable objDataTable, int intRepositoryID)
        {
            try
            {
                string strQuery = "SELECT ID,MetaTemplateName FROM vwMetaTemplate WHERE Status=1 AND RepositoryID=@RepositoryID AND ID IN (" + Utility.GetAccessRight(Utility.AccessRight.MetaTemplate) + ")";
                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

                DbParameter[] objDbParameter = new DbParameter[1];
                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "RepositoryID";
                objDbParameter[0].Value = intRepositoryID;

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

        public static Utility.ResultType SelectAllByRoleID(out DataTable objDataTable, int intRoleID)
        {
            try
            {
                char charQuote = '"';
                string strQuery = "SELECT *" +
                                    " FROM vwMetaTemplate left outer Join (SELECT Distinct(MetaTemplateID) FROM vwRolePermission WHERE RoleID=@RoleID)  RolePermission" +
                                    " ON vwMetaTemplate.Id=RolePermission.MetaTemplateID" +
                                    " ORDER BY ID DESC";


                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

                DbParameter[] objDbParameter = new DbParameter[1];
                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "RoleID";
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

        //Added by vivek for getting metatemplate for perticular repository 28-11-2017
        public static Utility.ResultType SelectMetadataByRoleIDForRepository(out DataTable objDataTable, int intRoleID, int intRepositoryID)
        {
            try
            {
                char charQuote = '"';
                string strQuery = "SELECT *" +
                                    " FROM vwMetaTemplate left outer join (SELECT Distinct(MetaTemplateID) FROM vwRolePermission WHERE RoleID=@RoleID)  RolePermission" +
                                    " ON vwMetaTemplate.Id=RolePermission.MetaTemplateID" + " where vwMetaTemplate.RepositoryID=@RepositoryID" +
                                    " ORDER BY ID DESC";

                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

                DbParameter[] objDbParameter = new DbParameter[2];
                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "RoleID";
                objDbParameter[0].Value = intRoleID;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "RepositoryID";
                objDbParameter[1].Value = intRepositoryID;

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

        public static Utility.ResultType GetAccessToMetatemplate(MetaTemplate objMetaTemplate, int RoleId,int UserId)
        {
            DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;
            DbTransaction objDbTransaction = Utility.GetTransaction;
            try
            {

                DbParameter[] objDbParameter = new DbParameter[6];

                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "RoleID";
                objDbParameter[0].Value = RoleId;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "RepositoryID";
                objDbParameter[1].Value = objMetaTemplate.RepositoryID;

                objDbParameter[2] = objDbProviderFactory.CreateParameter();
                objDbParameter[2].ParameterName = "MetaTemplateID";
                objDbParameter[2].Value = objMetaTemplate.MetaTemplateID;

                objDbParameter[3] = objDbProviderFactory.CreateParameter();
                objDbParameter[3].ParameterName = "CategoryID";
                objDbParameter[3].Value = 0;

                objDbParameter[4] = objDbProviderFactory.CreateParameter();
                objDbParameter[4].ParameterName = "FolderID";
                objDbParameter[4].Value = 0;

                objDbParameter[5] = objDbProviderFactory.CreateParameter();
                objDbParameter[5].ParameterName = "UpdatedBy";
                objDbParameter[5].Value = UserId;

                DataHelper.ExecuteNonQuery("SP_AccessPermission", objDbTransaction, objDbParameter);
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
