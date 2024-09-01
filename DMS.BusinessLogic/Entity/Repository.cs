using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data;

namespace DMS.BusinessLogic
{
    public class Repository
    {
        #region Properties
        public int RepositoryID { get; set; }
        public string RepositoryName { get; set; }
        public string RepositoryDescription { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public int UpdatedBy { get; set; }
        public int Status { get; set; }
        #endregion

        #region Methods
        public static Utility.ResultType Insert(Repository objRepository)
        {
            DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;
            DbTransaction objDbTransaction = Utility.GetTransaction;
            try
            {

                DbParameter[] objDbParameter = new DbParameter[8];

                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "RepositoryName";
                objDbParameter[0].Value = objRepository.RepositoryName;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "RepositoryDescription";
                objDbParameter[1].Value = objRepository.RepositoryDescription;

                objDbParameter[2] = objDbProviderFactory.CreateParameter();
                objDbParameter[2].ParameterName = "CreatedOn";
                objDbParameter[2].Value = DateTime.Now;

                objDbParameter[3] = objDbProviderFactory.CreateParameter();
                objDbParameter[3].ParameterName = "CreatedBy";
                objDbParameter[3].Value = objRepository.CreatedBy;

                objDbParameter[4] = objDbProviderFactory.CreateParameter();
                objDbParameter[4].ParameterName = "UpdatedOn";
                objDbParameter[4].Value = DBNull.Value;

                objDbParameter[5] = objDbProviderFactory.CreateParameter();
                objDbParameter[5].ParameterName = "UpdatedBy";
                objDbParameter[5].Value = DBNull.Value;

                objDbParameter[6] = objDbProviderFactory.CreateParameter();
                objDbParameter[6].ParameterName = "Status";
                objDbParameter[6].Value = objRepository.Status;

                objDbParameter[7] = objDbProviderFactory.CreateParameter();
                objDbParameter[7].ParameterName = "RepositoryID";
                objDbParameter[7].Size = 50;
                objDbParameter[7].Direction = System.Data.ParameterDirection.Output;

                DataHelper.ExecuteNonQuery("SP_I_Repository", objDbTransaction, objDbParameter);
                objDbTransaction.Commit();
                objRepository.RepositoryID = Convert.ToInt32(objDbParameter[7].Value);

                Utility.InsertAccessRight(objRepository.RepositoryID, 0, 0, 0);

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
                char charQuote = '"';
                string strQuery = " SELECT ID,RepositoryName,CreatedOn," +
                                    " (SELECT FirstName FROM vwUser WHERE ID = vwRepository.CreatedBy) AS " + charQuote + "CreatedBy" + charQuote + "," +
                                    " UpdatedOn," +
                                    " (SELECT FirstName FROM vwUser WHERE ID = vwRepository.UpdatedBy) AS " + charQuote + "UpdatedBy" + charQuote + "," +
                                    " (SELECT SettingName FROM vwAppSetting WHERE SettingType='STATUS' AND SettingValue=vwRepository.Status) AS " + charQuote + "Status" + charQuote + "" +
                                    " FROM vwRepository WHERE ID IN (" + Utility.GetAccessRight(Utility.AccessRight.Repository) + ") ORDER BY ID DESC";

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
        #region Change by Vivek on 28-11-17
        //Edited By Vivek
        //   public static Utility.ResultType SelectAllByRoleID(out DataTable objDataTable, int intRoleID)
        //{
        //    try
        //    {
        //        char charQuote = '"';
        //        string strQuery = "";
        //        if (intRoleID == 1)
        //        {
        //            //strQuery = "  SELECT *" +
        //            //               " FROM vwRepository Inner Join (SELECT Distinct(RepositoryID) FROM vwRolePermission WHERE RoleID=@RoleID)  RolePermission" +
        //            //               " ON vwRepository.Id=RolePermission.RepositoryID" +
        //            //               " ORDER BY ID DESC";
        //            //Commented by Vivek 23-11-17
        //            strQuery = "  SELECT *" +
        //            " FROM vwRepository Left Outer Join (SELECT Distinct(RepositoryID) FROM vwRolePermission WHERE RoleID=@RoleID)  RolePermission" +
        //            " ON vwRepository.Id=RolePermission.RepositoryID" +
        //            " ORDER BY ID DESC";
        //        }
        //        else
        //        {
        //            //strQuery = "  SELECT *" +
        //            //               " FROM vwRepository Inner Join (SELECT Distinct(RepositoryID) FROM vwRolePermission WHERE RoleID=@RoleID)  RolePermission" +
        //            //               " ON vwRepository.Id=RolePermission.RepositoryID" +
        //            //               " ORDER BY ID DESC";

        //            strQuery = "  SELECT *" +
        //          " FROM vwRepository Left Outer Join (SELECT Distinct(RepositoryID) FROM vwRolePermission WHERE RoleID=@RoleID)  RolePermission" +
        //          " ON vwRepository.Id=RolePermission.RepositoryID" +
        //          " ORDER BY ID DESC";
        //        }

        //        DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

        //        DbParameter[] objDbParameter = new DbParameter[1];
        //        objDbParameter[0] = objDbProviderFactory.CreateParameter();
        //        objDbParameter[0].ParameterName = "RoleID";
        //        objDbParameter[0].Value = intRoleID;

        //        objDataTable = DataHelper.ExecuteDataTable(strQuery, objDbParameter);

        //        if (objDataTable.Rows.Count == 0)
        //        {
        //            return Utility.ResultType.Failure;
        //        }
        //        return Utility.ResultType.Success;
        //    }
        //    catch (Exception ex)
        //    {
        //        LogManager.ErrorLog(Utility.LogFilePath, ex);
        //        objDataTable = null;
        //        return Utility.ResultType.Error;
        //    }
        //}
        public static Utility.ResultType SelectAllByRoleID(out DataTable objDataTable, int intRoleID, int intRepositoryID)
        {
            try
            {
                char charQuote = '"';
                string strQuery = "";
                if (intRoleID == 1 || intRoleID == 131)
                {
                    //strQuery = "  SELECT *" +
                    //               " FROM vwRepository Inner Join (SELECT Distinct(RepositoryID) FROM vwRolePermission WHERE RoleID=@RoleID)  RolePermission" +
                    //               " ON vwRepository.Id=RolePermission.RepositoryID" +
                    //               " ORDER BY ID DESC";
                    //Commented by Vivek 23-11-17
                    strQuery = "  SELECT *" +
                    " FROM vwRepository Left Outer Join (SELECT Distinct(RepositoryID) FROM vwRolePermission WHERE RoleID=@RoleID)  RolePermission" +
                    " ON vwRepository.Id=RolePermission.RepositoryID" + " where vwRepository.ID=@RepositoryID" +
                    " ORDER BY ID DESC";
                }
                else
                {
                    //strQuery = "  SELECT *" +
                    //               " FROM vwRepository Inner Join (SELECT Distinct(RepositoryID) FROM vwRolePermission WHERE RoleID=@RoleID)  RolePermission" +
                    //               " ON vwRepository.Id=RolePermission.RepositoryID" +
                    //               " ORDER BY ID DESC";

                    strQuery = "  SELECT *" +
                  " FROM vwRepository inner join (SELECT Distinct(RepositoryID) FROM vwRolePermission WHERE RoleID=@RoleID)  RolePermission" +
                  " ON vwRepository.Id=RolePermission.RepositoryID" +
                  " ORDER BY ID DESC";
                }

                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

                DbParameter[] objDbParameter = new DbParameter[2];
                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "RoleID";
                objDbParameter[0].Value = intRoleID;
                //if (intRoleID == 1)
                //{
                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "RepositoryID";
                objDbParameter[1].Value = intRepositoryID;
                //}

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

        public static Utility.ResultType Update(Repository objRepository)
        {
            DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;
            DbTransaction objDbTransaction = Utility.GetTransaction;
            try
            {

                DbParameter[] objDbParameter = new DbParameter[6];

                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "RepositoryID";
                objDbParameter[0].Value = objRepository.RepositoryID;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "RepositoryName";
                objDbParameter[1].Value = objRepository.RepositoryName;

                objDbParameter[2] = objDbProviderFactory.CreateParameter();
                objDbParameter[2].ParameterName = "RepositoryDescription";
                objDbParameter[2].Value = objRepository.RepositoryDescription;

                objDbParameter[3] = objDbProviderFactory.CreateParameter();
                objDbParameter[3].ParameterName = "UpdatedOn";
                objDbParameter[3].Value = DateTime.Now; ;

                objDbParameter[4] = objDbProviderFactory.CreateParameter();
                objDbParameter[4].ParameterName = "UpdatedBy";
                objDbParameter[4].Value = objRepository.UpdatedBy;

                objDbParameter[5] = objDbProviderFactory.CreateParameter();
                objDbParameter[5].ParameterName = "Status";
                objDbParameter[5].Value = objRepository.Status;

                DataHelper.ExecuteNonQuery("SP_U_Repository", objDbTransaction, objDbParameter);
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

        public static Utility.ResultType SelectByRepositoryID(out DataTable objDataTable, int intRepositoryID)
        {
            try
            {
                string strQuery = "SELECT * FROM vwRepository WHERE ID =@ID";
                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

                DbParameter[] objDbParameter = new DbParameter[1];
                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "ID";
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

        public static Utility.ResultType SelectRepositoryID(out DataTable objDataTable, string RepName)
        {
            try
            {
                string strQuery = "SELECT * FROM vwRepository WHERE RepositoryName=@RepositoryName";
                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

                DbParameter[] objDbParameter = new DbParameter[1];
                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "RepositoryName";
                objDbParameter[0].Value = RepName;

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

        public static Utility.ResultType SelectByRepositoryName(string strRepositoryName)
        {
            try
            {
                string strQuery = "SELECT RepositoryName FROM vwRepository WHERE UPPER(RepositoryName) =@RepositoryName";
                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

                DbParameter[] objDbParameter = new DbParameter[1];
                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "RepositoryName";
                objDbParameter[0].Value = strRepositoryName.ToUpper().Trim();

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

        public static Utility.ResultType SelectRepositoryForDropDown(out DataTable objDataTable)
        {
            try
            {
                string strQuery = "SELECT ID,RepositoryName FROM vwRepository WHERE ID IN (" + Utility.GetAccessRight(Utility.AccessRight.Repository) + ") AND Status=1 Order by RepositoryName";
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


        public static Utility.ResultType SelectAccessTypeForDropDown(out DataTable objDataTable)
        {
            try
            {
                string strQuery = "SELECT ID,Access FROM Share_Access where Status=1 order by id Desc";
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

        public static Utility.ResultType SelectUserEmailForDropDown(out DataTable objDataTable)
        {
            try
            {
                string strQuery = "select ID, EmailID from [user] where Status=1";
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

        public DataTable LoadDataRepository()
        {

            DataTable objDataTable = DataHelper.ExecuteDataTable(" select ID as MainID,RepositoryName as Name,CreatedOn,UpdatedOn from vwRepository where ID in(" + Utility.GetAccessRight(Utility.AccessRight.Repository) + ")ORDER BY ID DESC;", null);
            return objDataTable;
        }
        public DataTable LoadDataMetaTemplate()
        {

            DataTable objDataTable = DataHelper.ExecuteDataTable("select ID as MainID,MetaTemplateName as Name,CreatedOn,UpdatedOn from vwMetaTemplate where ID in(" + Utility.GetAccessRight(Utility.AccessRight.MetaTemplate) + ")ORDER BY ID DESC;", null);
            return objDataTable;
        }
        public DataTable LoadDataCategory()
        {

            DataTable objDataTable = DataHelper.ExecuteDataTable("select ID as MainID,CategoryName as Name,CreatedOn,UpdatedOn from vwCategory where ID in(" + Utility.GetAccessRight(Utility.AccessRight.Category) + ")ORDER BY ID DESC;", null);
            return objDataTable;
        }
        public DataTable LoadDataFolder()
        {

            DataTable objDataTable = DataHelper.ExecuteDataTable("select ID as MainID,FolderName as Name,CreatedOn,UpdatedOn from vwFolder where ID in(" + Utility.GetAccessRight(Utility.AccessRight.Folder) + ")ORDER BY ID DESC;", null);
            return objDataTable;
        }

        /// <summary>
        /// For Log
        /// </summary>
        /// <param name="RepId"></param>
        /// <returns></returns>
        /// 
        public DataTable LoadDataRepositoryLog(int RepId)
        {

            DataTable objDataTable = DataHelper.ExecuteDataTable(" select ID as MainID,RepositoryName as Name,CreatedOn,UpdatedOn,ActionPerformed from vwRepositoryLog where ID in(" + RepId + ")ORDER BY ID DESC;", null);
            return objDataTable;
        }
        public DataTable LoadDataMetaTemplateLog(int MetaId)
        {

            DataTable objDataTable = DataHelper.ExecuteDataTable("select ID as MainID,MetaTemplateName as Name,CreatedOn,UpdatedOn,ActionPerformed from vwMetaTemplateLog where ID in(" + MetaId + ")ORDER BY ID DESC;", null);
            return objDataTable;
        }
        public DataTable LoadDataCategoryLog(int CatId)
        {

            DataTable objDataTable = DataHelper.ExecuteDataTable("select ID as MainID,CategoryName as Name,CreatedOn,UpdatedOn,ActionPerformed from vwCategoryLog where ID in(" + CatId + ")ORDER BY ID DESC;", null);
            return objDataTable;
        }
        public DataTable LoadDataFolderlog(int FolderId)
        {

            DataTable objDataTable = DataHelper.ExecuteDataTable("select ID as MainID,FolderName as Name,CreatedOn,UpdatedOn,ActionPerformed from vwFolderLog where ID in(" + FolderId + ")ORDER BY ID DESC;", null);
            return objDataTable;
        }


        #endregion
    }
}
