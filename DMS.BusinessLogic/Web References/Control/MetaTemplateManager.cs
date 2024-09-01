using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DMS.BusinessLogic;
using System.Data;

namespace DMS.BusinessLogic
{
    public class MetaTemplateManager
    {
        #region Private Members
        Utility objUtility = new Utility();
        #endregion


        #region Methods

        public Utility.ResultType InsertMetaTemplate(MetaTemplate objMetaTemplate)
        {
            try
            {
                return MetaTemplate.Insert(objMetaTemplate);
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return Utility.ResultType.Error;
            }
        }
        // Insert MetaTemplateFields
        public Utility.ResultType InsertMetaTemplateFields(MetaTemplateFields objMetaTemplateFields)
        {
            try
            {
                return MetaTemplateFields.Insert(objMetaTemplateFields);
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return Utility.ResultType.Error;
            }
        }
        // Update MetaTemplateFields
        public Utility.ResultType UpdateMetaTemplateFields(MetaTemplateFields objMetaTemplateFields)
        {
            try
            {
                return MetaTemplateFields.Update(objMetaTemplateFields);
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return Utility.ResultType.Error;
            }
        }


        public Utility.ResultType SelectMetaTemplate(out DataTable objDataTable)
        {

            return MetaTemplate.Select(out objDataTable);

        }

        public Utility.ResultType SelectMetaTemplate(out DataTable objDataTable, int intMetaTemplateID)
        {
            try
            {
                return MetaTemplate.Select(out objDataTable, intMetaTemplateID);
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                objDataTable = null;
                return Utility.ResultType.Error;
            }
        }

        public Utility.ResultType SelectMetaTemplateByRepositoryID(out DataTable objDataTable, int intRepositoryID)
        {
            try
            {
                return MetaTemplate.SelectByRepositoryID(out objDataTable, intRepositoryID);
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                objDataTable = null;
                return Utility.ResultType.Error;
            }
        }
        public static Utility.ResultType SelectMetaTemplateForDropDown(out DataTable objDataTable, int intRepositoryID)
        {
            try
            {
                return MetaTemplate.SelectMetaTemplateForDropDown(out objDataTable, intRepositoryID);
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                objDataTable = null;
                return Utility.ResultType.Error;
            }
        }

        public Utility.ResultType UpdateMetaTemplate(MetaTemplate objMetaTemplate)
        {
            try
            {
                return MetaTemplate.Update(objMetaTemplate);
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return Utility.ResultType.Error;
            }
        }

        public Utility.ResultType SelectMetaTemplate(string strMetaTemplateName, int intRepositoryID)
        {
            try
            {
                return MetaTemplate.SelectByMetaTemplateName(strMetaTemplateName, intRepositoryID);
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return Utility.ResultType.Error;
            }
        }

        public static Utility.ResultType SelectDataTypeForDropDown(out DataTable objDataTable)
        {
            try
            {
                return MetaTemplateFields.SelectDataTypeForDropDown(out objDataTable);
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                objDataTable = null;
                return Utility.ResultType.Error;
            }
        }
        //Load MetaTemplateFields By MetaTemplateID
        public Utility.ResultType SelectMetaTemplateFeildsByMetaTemplateID(out DataTable objDataTable, int intMetaTemplateID)
        {
            try
            {
                return MetaTemplateFields.Select(out objDataTable, intMetaTemplateID);
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                objDataTable = null;
                return Utility.ResultType.Error;
            }
        }
        public Utility.ResultType SelectListItem(out DataTable objDataTable, int intMetaTemplateID)
        {
            try
            {
                return MetaTemplateItem.Select(out objDataTable, intMetaTemplateID);
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                objDataTable = null;
                return Utility.ResultType.Error;
            }
        }
         //Load MetaTemplate Fields ListItems By MetaTemplateID
        public Utility.ResultType SelectMetaTemplateListItems(out DataTable objDataTable, int intMetaTemplateID)
        {
            try
            {
                return MetaTemplateItem.SelectListItems(out objDataTable, intMetaTemplateID);
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                objDataTable = null;
                return Utility.ResultType.Error;
            }
        }

        //Insert MetaTemplateFields Items
        public Utility.ResultType InsertMetaTemplateFieldItems(MetaTemplateItem objMetaTemplateItem)
        {
            try
            {
                return MetaTemplateItem.Insert(objMetaTemplateItem);
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return Utility.ResultType.Error;
            }
        }
        
        public static Utility.ResultType SelectFieldForDropDown(out DataTable objDataTable, int intMetaTemplateID)
        {
            try
            {
                return MetaTemplateFields.SelectFieldForDropDown(out objDataTable, intMetaTemplateID);
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                objDataTable = null;
                return Utility.ResultType.Error;
            }
        }


        public Utility.ResultType SelectMetaTemplateForTreeview(out DataTable objDataTable,int intRoleID)
        {

            return MetaTemplate.SelectAllByRoleID(out objDataTable,intRoleID);

        }

        public Utility.ResultType SelectMetaTemplateForTreeviewForRepository(out DataTable objDataTable, int intRoleID,int intRepoId)
        {

            return MetaTemplate.SelectMetadataByRoleIDForRepository(out objDataTable, intRoleID, intRepoId);

        }

       public Utility.ResultType GetAccessToMetatemplate(MetaTemplate objmetatemplate,int RoleId,int UserId)
        {
            return MetaTemplate.GetAccessToMetatemplate(objmetatemplate, RoleId, UserId);
        }
        #endregion
    }
}
