using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;


namespace DMS.BusinessLogic
{
  public   class CategoryManager
    {
        #region Private Member
        Utility objUtility = new Utility();
        #endregion

        #region Method
        public Utility.ResultType InsertCategory(Category objCategory)
        {
            try
            {
                return Category.Insert(objCategory);
            }
            catch(Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return Utility.ResultType.Error;
            }


        }

        public Utility.ResultType UpdateCategory(Category objCategory)
        {
            try
            {
                return Category.Update(objCategory);
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return Utility.ResultType.Error;
            }


        }

        public Utility.ResultType SelectCategory(out DataTable objDataTable)
        {
           
                return Category.Select(out objDataTable);
           
        }

        public Utility.ResultType SelectCategoryByCategoryID(out DataTable objDataTable, int intCategoryID)
        {
            try
            {
                return Category.SelectByCategoryID(out objDataTable, intCategoryID);
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                objDataTable = null;
                return Utility.ResultType.Error;
            }
        }

        public Utility.ResultType SelectCategory(out DataTable objDataTable, int intMetaTemplateID)
        {
            try
            {
                return Category.SelectByMetaTemplateID(out objDataTable, intMetaTemplateID);
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                objDataTable = null;
                return Utility.ResultType.Error;
            }
        }

        public Utility.ResultType SelectCategoryForDropDown(out DataTable objDataTable, int intMetaTemplateID)
        {
            try
            {
                return Category.SelectByMetaTemplateIDForDropDown(out objDataTable, intMetaTemplateID);
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                objDataTable = null;
                return Utility.ResultType.Error;
            }
        }

        public Utility.ResultType SelectCategory(string strCategoryName, int intMetaTemplateID)
        {
            try
            {
                return Category.SelectByCategoryName(strCategoryName, intMetaTemplateID);
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return Utility.ResultType.Error;
            }
        }

        public Utility.ResultType SelectCategoryForTreeview(out DataTable objDataTable, int intRoleID)
        {

            return Category.SelectAllByRoleID(out objDataTable, intRoleID);

        }
      //Added by Vivek on 29-11-17 for getting category for selected categories
        public Utility.ResultType SelectCategoryByRepositoryForTreeview(out DataTable objDataTable, int intRoleID,int intRepoID)
        {

            return Category.SelectAllByRoleIDAndRepoID(out objDataTable, intRoleID, intRepoID);

        }

        public Utility.ResultType GetAccesstoCategory(Category objCategory,int RoleId,int UserId)
        {
            return Category.GetAccesstoCategory(objCategory, RoleId, UserId);
        }
        #endregion

    }
}
