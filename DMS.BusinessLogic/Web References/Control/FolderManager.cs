using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DMS.BusinessLogic
{
   public class FolderManager
    {
        #region Private Member
        Utility objUtility = new Utility();
        #endregion

        #region Method
        public static Utility.ResultType InsertFolder(Folder objFolder)
        {
            try
            {
                return Folder.Insert(objFolder);
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return Utility.ResultType.Error;
            }
        }

        public static Utility.ResultType InsertFolderAxisTrustee(Folder objFolder)
        {
            try
            {
               return Folder.InsertAxisTrustee(objFolder);
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return Utility.ResultType.Error;
            }
        }

        public static Utility.ResultType InsertFolderForAll(Folder objFolder)
        {
            try
            {
                return Folder.InsertForAll(objFolder);
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return Utility.ResultType.Error;
            }
        }

        public static Utility.ResultType InsertFolderCentrum(Folder objFolder)
        {
            try
            {
                return Folder.InsertCentrum(objFolder);
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return Utility.ResultType.Error;
            }
        }

        public static Utility.ResultType UpdateFolder(Folder objFolder)
        {
            try
            {
                return Folder.Update(objFolder);
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return Utility.ResultType.Error;
            }
        }

        public  Utility.ResultType SelectFolder(out DataTable objDataTable)
        {
           
                return Folder.Select(out objDataTable);
           
        }

        public static Utility.ResultType SelectFolder(out DataTable objDataTable, int intFolderID)
        {
            try
            {
                return Folder.SelectByFolderID(out objDataTable, intFolderID);
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                objDataTable = null;
                return Utility.ResultType.Error;
            }
        }

      
        public static Utility.ResultType SelectFolderByParentFolderID(out DataTable objDataTable, int intParentFolderID, int intMetaTemplateID)
        {
            try
            {
                return Folder.SelectByParentFolderID(out objDataTable, intParentFolderID,intMetaTemplateID);
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                objDataTable = null;
                return Utility.ResultType.Error;
            }
        }
        public static Utility.ResultType selectFolderbyMetaID(out DataTable objDataTable, int RepositoryID, int intMetaTemplateID, int folderid, int doccount)
        {
            try
            {
                return Folder.selectFolderByMetaID(out objDataTable,RepositoryID, intMetaTemplateID,folderid,doccount);
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                objDataTable = null;
                return Utility.ResultType.Error;
            }
        }

        public static Utility.ResultType SelectFolder(out DataTable dt, string strFolderName, int intMetaTemplateID, int intParentFolderID, int intCategoryID)
        {
            try
            {
                return Folder.SelectByFolderName(out dt,strFolderName, intMetaTemplateID, intParentFolderID, intCategoryID);
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                dt = null;
                return Utility.ResultType.Error;
            }
        }


        public static DataTable SelectFolderCentrum(string strFolderName, int intMetaTemplateID, int intParentFolderID, int intCategoryID)
        {
            try
            {
                return Folder.SelectByFolderNameCentrum(strFolderName, intMetaTemplateID, intParentFolderID, intCategoryID);
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                //dt = null;
                return null;
            }
        }

        public static DataTable SelectFolderCentrum2(string strFolderName, int intMetaTemplateID, int intParentFolderID)
        {
            try
            {
                return Folder.SelectByFolderNameCentrum2(strFolderName, intMetaTemplateID, intParentFolderID);
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                //dt = null;
                return null;
            }
        }

        public static DataSet SearchFolderName(string strFolderName, int intMetaTemplateID)
        {
            try
            {
                return Folder.SearchFolderName(strFolderName, intMetaTemplateID);
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return null;
            }
        }

        public static Utility.ResultType SelectFolderByMetaTemplateID(out DataTable objDataTable, int intMetaTemplateID)
        {
            try
            {
                return Folder.SelectFolderForTreeView(out objDataTable, intMetaTemplateID);
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                objDataTable = null;
                return Utility.ResultType.Error;
            }
        }

        public static Utility.ResultType SelectFolderByCategoryID_Centrum(out DataTable objDataTable, int intMetaTemplateID,int CategoryID)
        {
            try
            {
                return Folder.SelectFolderForTreeView_Centrum(out objDataTable, intMetaTemplateID, CategoryID);
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                objDataTable = null;
                return Utility.ResultType.Error;
            }
        }

        #region Seema 9Nov 2017

        public static Utility.ResultType SelectFolderByCategoryID_AxisTrustee(out DataTable objDataTable, int intMetaTemplateID, int CategoryID)
        {
            try
            {
                return Folder.SelectFolderForTreeView_AxisTree(out objDataTable, intMetaTemplateID, CategoryID);
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                objDataTable = null;
                return Utility.ResultType.Error;
            }
        }

        #endregion


#region Added by vivek 14 Nov
        public static Utility.ResultType SelectFolderByCategoryID(out DataTable objDataTable, int intMetaTemplateID, int CategoryID)
        {
            try
            { 
                return Folder.SelectFolderCategoryWise(out objDataTable, intMetaTemplateID, CategoryID);
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                objDataTable = null;
                return Utility.ResultType.Error;
            }
        }

#endregion
        public Utility.ResultType SelectFolderForTreeView(out DataTable objDataTable, int intRoleID)
        {

            return Folder.SelectAllByRoleID(out objDataTable, intRoleID);

        }
        //public static int InsertNewFolder(Folder objFolder)
        //{
        //    try
        //    {
        //        return Folder.InsertFolder(objFolder);
        //    }
        //    catch (Exception ex)
        //    {
        //        LogManager.ErrorLog(Utility.LogFilePath, ex);
        //        return -1;
        //    }
        //}

        public static Utility.ResultType GetAccesstoFolder(Folder objFolder,int RepositoryId,int RoleId,int UserId)
        {
            try
            {
                return Folder.GetAccesstoFolder(objFolder, RepositoryId, RoleId, UserId);
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return Utility.ResultType.Error;
            }
        }
        #endregion

        public Utility.ResultType GetDocumentByFolderID(out DataTable dt, int FolderID)
        {
            return Folder.GetDocumentByFolderID(out dt, FolderID);
        }


        public static DataTable GetDocumentForCartByDocumentID(List<int> DocumentID)
        {
            return Folder.GetDocumentForCartByDocumentID(DocumentID);
        }

    }
}
