using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data;

namespace DMS.BusinessLogic
{
    public class Category
    {
        #region Properties
        public int RepositoryID { get; set; }
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string CategoryDescription { get; set; }
        public int MetaTemplateID { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public int UpdatedBy { get; set; }
        public int Status { get; set; }
        #endregion

        #region Method
        public static Utility.ResultType Insert(Category objCategory)
        {
            DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;
            DbTransaction objDbTransaction = Utility.GetTransaction;
            try
            {

                DbParameter[] objDbParameter = new DbParameter[10];

                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "CategoryName";
                objDbParameter[0].Value = objCategory.CategoryName;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "CategoryDescription";
                objDbParameter[1].Value = objCategory.CategoryDescription;


                objDbParameter[2] = objDbProviderFactory.CreateParameter();
                objDbParameter[2].ParameterName = "RepositoryID";
                objDbParameter[2].Value = objCategory.RepositoryID;

                objDbParameter[3] = objDbProviderFactory.CreateParameter();
                objDbParameter[3].ParameterName = "MetaTemplateID";
                objDbParameter[3].Value = objCategory.MetaTemplateID;



                objDbParameter[4] = objDbProviderFactory.CreateParameter();
                objDbParameter[4].ParameterName = "CreatedOn";
                objDbParameter[4].Value = DateTime.Now;

                objDbParameter[5] = objDbProviderFactory.CreateParameter();
                objDbParameter[5].ParameterName = "CreatedBy";
                objDbParameter[5].Value = objCategory.CreatedBy;

                objDbParameter[6] = objDbProviderFactory.CreateParameter();
                objDbParameter[6].ParameterName = "UpdatedOn";
                objDbParameter[6].Value = DBNull.Value;

                objDbParameter[7] = objDbProviderFactory.CreateParameter();
                objDbParameter[7].ParameterName = "UpdatedBy";
                objDbParameter[7].Value = DBNull.Value;

                objDbParameter[8] = objDbProviderFactory.CreateParameter();
                objDbParameter[8].ParameterName = "Status";
                objDbParameter[8].Value = objCategory.Status;

                objDbParameter[9] = objDbProviderFactory.CreateParameter();
                objDbParameter[9].ParameterName = "CategoryID";
                objDbParameter[9].Size = 50;
                objDbParameter[9].Direction = System.Data.ParameterDirection.Output;

                DataHelper.ExecuteNonQuery("SP_I_Category", objDbTransaction, objDbParameter);
                objDbTransaction.Commit();
                objCategory.CategoryID = Convert.ToInt32(objDbParameter[9].Value);
                Utility.InsertAccessRight(0, 0, objCategory.CategoryID, 0);
                return Utility.ResultType.Success;
            }
            catch (Exception ex)
            {
                objDbTransaction.Rollback();
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return Utility.ResultType.Error;
            }
        }

        public static Utility.ResultType Update(Category objCategory)
        {
            DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;
            DbTransaction objDbTransaction = Utility.GetTransaction;
            try
            {

                DbParameter[] objDbParameter = new DbParameter[6];

                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "CategoryName";
                objDbParameter[0].Value = objCategory.CategoryName;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "CategoryDescription";
                objDbParameter[1].Value = objCategory.CategoryDescription;

                objDbParameter[2] = objDbProviderFactory.CreateParameter();
                objDbParameter[2].ParameterName = "UpdatedOn";
                objDbParameter[2].Value = DateTime.Now;

                objDbParameter[3] = objDbProviderFactory.CreateParameter();
                objDbParameter[3].ParameterName = "UpdatedBy";
                objDbParameter[3].Value = objCategory.UpdatedBy;

                objDbParameter[4] = objDbProviderFactory.CreateParameter();
                objDbParameter[4].ParameterName = "Status";
                objDbParameter[4].Value = objCategory.Status;

                objDbParameter[5] = objDbProviderFactory.CreateParameter();
                objDbParameter[5].ParameterName = "CategoryID";
                objDbParameter[5].Value = objCategory.CategoryID;

                DataHelper.ExecuteNonQuery("SP_U_Category", objDbTransaction, objDbParameter);
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
                string strQuery = "SELECT * FROM vwCategory WHERE Status=1";
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

        public static Utility.ResultType SelectByCategoryID(out DataTable objDataTable, int intCategoryID)
        {
            try
            {
                string strQuery = "SELECT * FROM vwCategory WHERE ID =@ID";
                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

                DbParameter[] objDbParameter = new DbParameter[1];
                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "ID";
                objDbParameter[0].Value = intCategoryID;

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

        public static Utility.ResultType SelectByMetaTemplateID(out DataTable objDataTable, int intMetaTemplateID)
        {
            try
            {
                char charQuote = '"';
                string strQuery = " SELECT ID,CategoryName,CreatedOn," +
                                 " (SELECT FirstName FROM vwUser WHERE ID = vwCategory.CreatedBy) AS " + charQuote + "CreatedBy" + charQuote + "," +
                                 " UpdatedOn," +
                                 " (SELECT FirstName FROM vwUser WHERE ID = vwCategory.UpdatedBy) AS " + charQuote + "UpdatedBy" + charQuote + "," +
                                 " (SELECT SettingName FROM vwAppSetting WHERE SettingType='STATUS' AND SettingValue=vwCategory.Status) AS " + charQuote + "Status" + charQuote + "" +
                                 " FROM vwCategory WHERE MetaTemplateID = @MetaTemplateID AND ID IN (" + Utility.GetAccessRight(Utility.AccessRight.Category) + ") ORDER BY ID DESC";

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

        public static Utility.ResultType SelectByMetaTemplateIDForDropDown(out DataTable objDataTable, int intMetaTemplateID)
        {
            try
            {
                string strQuery = "SELECT ID,CategoryName  FROM vwCategory WHERE MetaTemplateID = @MetaTemplateID AND Status=1 AND ID IN (" + Utility.GetAccessRight(Utility.AccessRight.Category) + ")";
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

        public static Utility.ResultType SelectByCategoryName(string strCategoryName, int intMetaTemplateID)
        {
            try
            {
                string strQuery = "SELECT CategoryName FROM vwCategory WHERE UPPER(CategoryName) = @CategoryName AND MetaTemplateID = @MetaTemplateID";
                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

                DbParameter[] objDbParameter = new DbParameter[2];
                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "CategoryName";
                objDbParameter[0].Value = strCategoryName.ToUpper().Trim();

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "MetaTemplateID";
                objDbParameter[1].Value = intMetaTemplateID;

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

        public static Utility.ResultType SelectAllByRoleID(out DataTable objDataTable, int intRoleID)
        {
            try
            {
                char charQuote = '"';
                string strQuery = "SELECT *" +
                                    " FROM vwCategory Left Outer Join (SELECT Distinct(CategoryID) FROM vwRolePermission WHERE RoleID=@RoleID) RolePermission" +
                                    " ON vwCategory.Id=RolePermission.CategoryID" +
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

        public static Utility.ResultType SelectAllByRoleIDAndRepoID(out DataTable objDataTable, int intRoleID, int intRepositoryId)
        {
            try
            {
                char charQuote = '"';
                string strQuery = "SELECT *" +
                                    " FROM vwCategory Left Outer Join (SELECT Distinct(CategoryID) FROM vwRolePermission WHERE RoleID=@RoleID) RolePermission" +
                                    " ON vwCategory.Id=RolePermission.CategoryID" + " where vwCategory.RepositoryID=@intRepositoryId" +
                                    " ORDER BY ID DESC";


                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

                DbParameter[] objDbParameter = new DbParameter[2];
                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "RoleID";
                objDbParameter[0].Value = intRoleID;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "intRepositoryId";
                objDbParameter[1].Value = intRepositoryId;

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

        public static Utility.ResultType GetAccesstoCategory(Category objCategory,int RoleId,int UserId)
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
                objDbParameter[1].Value = objCategory.RepositoryID;

                objDbParameter[2] = objDbProviderFactory.CreateParameter();
                objDbParameter[2].ParameterName = "MetaTemplateID";
                objDbParameter[2].Value = objCategory.MetaTemplateID;

                objDbParameter[3] = objDbProviderFactory.CreateParameter();
                objDbParameter[3].ParameterName = "CategoryID";
                objDbParameter[3].Value = objCategory.CategoryID;

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
