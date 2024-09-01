using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
namespace DMS.BusinessLogic
{
    public class RepositoryManager
    {
        #region Private Member
        Utility objUtility = new Utility();
        #endregion


        #region Method
        public Utility.ResultType InsertRepository(Repository objRepository)
        {
            try
            {
                return Repository.Insert(objRepository);
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return Utility.ResultType.Error;
            }
        }

        public Utility.ResultType UpdateRepository(Repository objRepository)
        {
            try
            {
                return Repository.Update(objRepository);
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return Utility.ResultType.Error;
            }
        }

        public Utility.ResultType SelectRepository(out DataTable objDataTable)
        {
            try
            {
                return Repository.Select(out objDataTable);
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                objDataTable = null;
                return Utility.ResultType.Error;
            }
        }

        public Utility.ResultType SelectRepository(out DataTable objDataTable,int intRepositoryID)
        {
            try
            {
                return Repository.SelectByRepositoryID(out objDataTable, intRepositoryID);
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                objDataTable = null;
                return Utility.ResultType.Error;
            }
        }

        public Utility.ResultType SelectRepository(out DataTable objDataTable, string RepName)
        {
            try
            {
                return Repository.SelectRepositoryID(out objDataTable, RepName);
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                objDataTable = null;
                return Utility.ResultType.Error;
            }
        }
        public Utility.ResultType SelectRepository(string strRepositoryName)
        {
            try
            {
                return Repository.SelectByRepositoryName(strRepositoryName);
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return Utility.ResultType.Error;
            }
        }

        #region Change by Vivek on 28-11-17
        public Utility.ResultType SelectRepositoryForTreeview(out DataTable objDataTable, int intRoleID, int RepositoryId)
        {
            try
            {

                return Repository.SelectAllByRoleID(out objDataTable, intRoleID, RepositoryId);
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                objDataTable = null;
                return Utility.ResultType.Error;
            }
        }

        //Commented by Vivek
        //public Utility.ResultType SelectRepositoryForTreeview(out DataTable objDataTable, int intRoleID)
        //{
        //    try
        //    {

        //        return Repository.SelectAllByRoleID(out objDataTable, intRoleID);
        //    }
        //    catch (Exception ex)
        //    {
        //        LogManager.ErrorLog(Utility.LogFilePath, ex);
        //        objDataTable = null;
        //        return Utility.ResultType.Error;
        //    }
        //}
        #endregion

        public static Utility.ResultType SelectRepositoryForDropDown(out DataTable objDataTable)
        {
            try
            {
                return Repository.SelectRepositoryForDropDown(out objDataTable);
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
                return Repository.SelectAccessTypeForDropDown(out objDataTable);
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
                return Repository.SelectUserEmailForDropDown(out objDataTable);
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
