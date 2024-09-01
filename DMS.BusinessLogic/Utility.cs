using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using PdfViewer;
using AjaxControlToolkit;
using System.Data;
using System.Data.Common;
using System.Web.UI.HtmlControls;
using DMS.BusinessLogic;
using iTextSharp;
using System.Configuration;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Security.Cryptography;
using System.Text;
using iTextSharp.text.pdf;
using System.Windows.Forms;
using System.Net.NetworkInformation;

namespace DMS.BusinessLogic
{
    public class Utility
    {
        #region Enum
        public enum Status { InActive = 0, Active = 1 };
        public enum ResultType { Success, Failure, Error };
        public enum FieldType { Null = 0, NotNull = 1 };
        public enum UserType { Named, Concurrent };
        public enum UserIS { General, Domain };
        public enum StorageType { FileSystem, DatabaseSystem };
        public enum Control { Menu, Treeview };
        public enum EnableOrDisable { Enable, Disable };
        public enum AccessRight { Repository, MetaTemplate, Category, Folder };
        #endregion

        #region Properties
        public ResultType Result { get; set; }
        public static bool IsAccessRights = true;

        public static StorageType FileStorageType
        {
            get
            {
                string strFileStorageType = ConfigurationSettings.AppSettings["FileStorageType"].ToString().Trim();

                if (strFileStorageType == "1")
                {
                    return StorageType.FileSystem;
                }
                else
                {
                    return StorageType.DatabaseSystem;
                }
            }
        }

        public static bool EnabledNamedConcurrentUser
        {
            get
            {
                string strEnabledNamedConcurrentUser = ConfigurationSettings.AppSettings["EnabledNamedConcurrentUser"].ToString().Trim();

                if (strEnabledNamedConcurrentUser == "1")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public static string ConnectionString
        {
            get
            {
                return ConfigurationSettings.AppSettings["ConnectionString"].ToString();
            }
        }

        public static string DHSConnectionString
        {
            get
            {
                return ConfigurationSettings.AppSettings["DHSConnection"].ToString();
            }
            
        }

        public static string ProviderName
        {
            get
            {
                return ConfigurationSettings.AppSettings["ProviderName"].ToString();
            }
        }

        public static string FilesPath
        {
            get
            {
                return ConfigurationSettings.AppSettings["FilesPath"].ToString();
            }
        }

        public static string LogFilePath
        {
            get
            {
                return ConfigurationSettings.AppSettings["DMSLog"].ToString();
            }
        }
        
        
        public static string DocumentPath
        {
            get
            {
                return ConfigurationSettings.AppSettings["DMSDocument"].ToString();
            }
        }
        public static string DMSTiffBackupDocument
        {
            get
            {
                return ConfigurationSettings.AppSettings["DMSTiffBackup"].ToString();
            }
        }
        public static string LuceneFilePath
        {
            get
            {
                return ConfigurationSettings.AppSettings["DMSLucene"].ToString();
            }
        }
       

        public static string DMSVersionPath
        {
            get
            {
                return ConfigurationSettings.AppSettings["DMSVersion"].ToString();
            }
        }

        public static string ArchiveFilePath
        {
            get
            {
                return ConfigurationSettings.AppSettings["DMSArchive"].ToString();
            }
        }
        public static string VersionFilePath
        {
            get
            {
                return ConfigurationSettings.AppSettings["DMSVersion"].ToString();
            }
        }
        public static string BulkFilePath
        {
            get
            {
                return ConfigurationSettings.AppSettings["DMSBulkUpload"].ToString();
            }
        }

        public static string SuccessErrorFilePath
        {
            get
            {
                return ConfigurationSettings.AppSettings["DMSSuccessError"].ToString();
            }
        }

        public static int MaxUsers
        {
            get
            {
                return Convert.ToInt32(ConfigurationSettings.AppSettings["MaxUsers"].ToString());
            }
        }

        public static double Interval
        {
            get
            {
                return Convert.ToDouble(ConfigurationSettings.AppSettings["Interval"]);
            }
        }

        public static int MaxNamedUser
        {
            get
            {
                return Convert.ToInt32(ConfigurationSettings.AppSettings["MaxNamedUser"]);
            }
        }

        public static int MaxConcurrentUser
        {
            get
            {
                return Convert.ToInt32(ConfigurationSettings.AppSettings["MaxConcurrentUser"]);
            }
        }

        public static DbTransaction GetTransaction
        {
            get
            {
                DbProviderFactory objDbProviderFactory = DbProviderFactories.GetFactory(Utility.ProviderName);
                DbConnection objDbConnection = objDbProviderFactory.CreateConnection();
                objDbConnection.ConnectionString = Utility.ConnectionString;
                objDbConnection.Open();
                DbTransaction objDbTransaction = objDbConnection.BeginTransaction();
                return objDbTransaction;
            }
        }

        public static DbProviderFactory GetProviderFactory
        {
            get
            {
                DbProviderFactory objDbProviderFactory = DbProviderFactories.GetFactory(Utility.ProviderName);
                return objDbProviderFactory;
            }
        }

        public static DbConnection GetConnection
        {
            get
            {
                DbProviderFactory objDbProviderFactory = DbProviderFactories.GetFactory(Utility.ProviderName);
                DbConnection objDbConnection = objDbProviderFactory.CreateConnection();
                objDbConnection.ConnectionString = Utility.ConnectionString;
                return objDbConnection;
            }
        }
        #endregion

        #region Method
        public static string GetUniqueFileName(string strExtension)
        {
            bool boolIsFileExist = true;
            while (boolIsFileExist == true)
            {
                string strDocumentGuid = System.Guid.NewGuid().ToString() + strExtension;
                string strDocumentPath = Utility.DocumentPath + strDocumentGuid;
                if (!File.Exists(strDocumentPath))
                {
                    return strDocumentGuid;
                }
            }
            return DateTime.Now.ToString("dd_MMM_yyyy_hh_mi_ss") + strExtension;
        }

        //public TableLogOnInfos setLogonInfo()
        //{
        //    TableLogOnInfo logOnInfo = new TableLogOnInfo();

        //    logOnInfo.ConnectionInfo.ServerName = ConfigurationSettings.AppSettings["ServerName"].ToString();
        //    logOnInfo.ConnectionInfo.DatabaseName = ConfigurationSettings.AppSettings["DatabaseName"].ToString();
        //    logOnInfo.ConnectionInfo.UserID = ConfigurationSettings.AppSettings["UserID"].ToString();
        //    logOnInfo.ConnectionInfo.Password = ConfigurationSettings.AppSettings["Password"].ToString();
        //    TableLogOnInfos infos = new TableLogOnInfos();
        //    infos.Add(logOnInfo);
        //    return infos;
        //}

        public  DataTable LoadDataGrid()
        {
            
                Utility objUtility = new Utility();
                DataTable objDataTable = DataHelper.ExecuteDataTable("select Id,DocumentName,case Status when 1 then 'Active' else 'Inactive' End as Status from Document", null);
                return objDataTable;  
        }

        public static bool IsPermission(string strSettingName, DataTable objDataTableMenu)
        {
            string strModuleID = AppSetting.SettingData.Select("SettingType='PERMISSION' AND SettingName='" + strSettingName + "'")[0]["SettingValue"].ToString();
            if (objDataTableMenu.Select("ID=" + strModuleID).Length > 0)
                return true;
            else
                return false;
        }

        public static void LoadUserType(DropDownList objDropDownList)
        {
            DataTable objDataTable = AppSetting.SettingData;
            objDataTable = objDataTable.Select("SettingType='USERTYPE'").CopyToDataTable();

            objDropDownList.DataSource = objDataTable;
            objDropDownList.DataTextField = "SettingName";
            objDropDownList.DataValueField = "SettingValue";
            objDropDownList.DataBind();
            objDropDownList.SelectedIndex = 0;
        }

        public static void LoadUserIS(DropDownList objDropDownList)
        {
            DataTable objDataTable = AppSetting.SettingData;
            objDataTable = objDataTable.Select("SettingType='USERIS'").CopyToDataTable();

            objDropDownList.DataSource = objDataTable;
            objDropDownList.DataTextField = "SettingName";
            objDropDownList.DataValueField = "SettingValue";
            objDropDownList.DataBind();
            objDropDownList.SelectedIndex = 0;
        }

        public static void LoadStatus(DropDownList objDropDownList)
        {
            try
            {
                DataTable objDataTable = AppSetting.SettingData;
                objDataTable = objDataTable.Select("SettingType='STATUS'").CopyToDataTable();

                objDropDownList.DataSource = objDataTable;
                objDropDownList.DataTextField = "SettingName";
                objDropDownList.DataValueField = "SettingValue";
                objDropDownList.DataBind();
                objDropDownList.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        public static void SetGridHoverStyle(GridViewRowEventArgs objGridViewCommandEventArgs)
        {
            if (objGridViewCommandEventArgs.Row.RowType == DataControlRowType.DataRow)
            {
                objGridViewCommandEventArgs.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#ceedfc'");
                objGridViewCommandEventArgs.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle");
                objGridViewCommandEventArgs.Row.Attributes.Add("style", "cursor:pointer;");
            }
        }

        public static void LoadCountry(DropDownList objDropDownList)
        {
            try
            {
                string strQuery = "SELECT * FROM vwCountry WHERE Status=1";
                DataTable objDataTable = DataHelper.ExecuteDataTable(strQuery, null);
                objDropDownList.DataSource = objDataTable;
                objDropDownList.DataTextField = "CountryName";
                objDropDownList.DataValueField = "ID";
                objDropDownList.DataBind();
                objDropDownList.Items.Insert(0, new ListItem("--SELECT--", "-1"));
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                objDropDownList.Items.Insert(0, new ListItem("--NONE--", "-1"));
            }
        }

        public static void LoadMetaTemplateListFields(DropDownList objDropDownList, int intMetaTemplateID)
        {
            try
            {
                string strQuery = "select * from vwMetaTemplateFields where (FieldDataTypeID=4 or FieldDataTypeID=9) and MetaTemplateID=" + intMetaTemplateID + " and Status=1";

                DataTable objDataTable = DataHelper.ExecuteDataTable(strQuery, null);

                //DataTable objDataTableField = RemoveDuplicateRows(objDataTable,"ID");
                if (objDataTable != null && objDataTable.Rows.Count > 0)
                {
                    objDropDownList.DataSource = objDataTable;
                    objDropDownList.DataTextField = "FieldName";
                    objDropDownList.DataValueField = "ID";
                    objDropDownList.DataBind();
                    objDropDownList.Items.Insert(0, new ListItem("--SELECT--", "-1"));
                }
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                objDropDownList.Items.Insert(0, new ListItem("--NONE--", "-1"));
            }
        }

        public static DataTable RemoveDuplicateRows(DataTable dTable, string colName)
        {
            try
            {
                Hashtable hTable = new Hashtable();
                ArrayList duplicateList = new ArrayList();

                foreach (DataRow drow in dTable.Rows)
                {
                    if (hTable.Contains(drow[colName]))
                        duplicateList.Add(drow);
                    else
                        hTable.Add(drow[colName], string.Empty);
                }

                foreach (DataRow dRow in duplicateList)
                    dTable.Rows.Remove(dRow);

                return dTable;
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return dTable = null;
            }
        }

        public static void LoadState(DropDownList objDropDownList, int intCountryID)
        {
            try
            {
                string strQuery = "SELECT * FROM vwState WHERE CountryID= @CountryID and Status=1";

                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

                DbParameter[] objDbParameter = new DbParameter[1];
                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "CountryID";
                objDbParameter[0].Value = intCountryID;

                DataTable objDataTable = DataHelper.ExecuteDataTable(strQuery, objDbParameter);
                objDropDownList.DataSource = objDataTable;
                objDropDownList.DataTextField = "StateName";
                objDropDownList.DataValueField = "ID";
                objDropDownList.DataBind();

                objDropDownList.Items.Insert(0, new ListItem("--SELECT--", "-1"));

            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                objDropDownList.Items.Insert(0, new ListItem("--NONE--", "-1"));

            }
        }

        public static void LoadRole(DropDownList objDropDownList)
        {
            try
            {
                string strQuery = "SELECT * FROM vwRole " + (Convert.ToInt32(HttpContext.Current.Session["RoleType"]) == 2 ? " WHERE ID IN (SELECT RepositoryUserID FROM vwRoleRights WHERE RepositoryAdminID = " + Convert.ToInt32(HttpContext.Current.Session["RoleID"]) + " )" : string.Empty) + " ";
                DataTable objDataTable = DataHelper.ExecuteDataTable(strQuery, null);
                objDropDownList.DataSource = objDataTable;
                objDropDownList.DataTextField = "RoleName";
                objDropDownList.DataValueField = "ID";
                objDropDownList.DataBind();
                objDropDownList.Items.Insert(0, new ListItem("--SELECT--", "-1"));
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                objDropDownList.Items.Insert(0, new ListItem("--NONE--", "-1"));
            }
        }

        public static void LoadField(DropDownList objDropDownList, int intMetaTemplateID)
        {
            try
            {
                objDropDownList.Items.Clear();
                DataTable objDataTable = new DataTable();
                Utility objUtility = new Utility();
                objUtility.Result = MetaTemplateManager.SelectFieldForDropDown(out objDataTable, intMetaTemplateID);
                switch (objUtility.Result)
                {
                    case ResultType.Success:
                        DataView objDataView = objDataTable.DefaultView;
                        objDataView.Sort = "FieldName";
                        foreach (DataRowView objDataRowView in objDataView)
                        {
                            ListItem objListItem = new ListItem(objDataRowView["FieldName"].ToString(), objDataRowView["ID"].ToString() + "-" + objDataRowView["FieldDataTypeID"].ToString());
                            objDropDownList.Items.Add(objListItem);
                        }
                        objDropDownList.Items.Insert(0, new ListItem("--SELECT--", "-1"));
                        break;

                    case ResultType.Failure:
                        objDropDownList.Items.Insert(0, new ListItem("--NONE--", "-1"));
                        break;

                    case ResultType.Error:
                        objDropDownList.Items.Insert(0, new ListItem("--ERROR--", "-1"));
                        break;
                }
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                objDropDownList.Items.Insert(0, new ListItem("--NONE--", "-1"));
            }
        }

        public static Utility.ResultType SelectStatusByStatusID(out string strStatus, int intStatusID)
        {
            try
            {
                strStatus = ((Status)intStatusID).ToString();
                return Utility.ResultType.Success;

            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                strStatus = null;
                return Utility.ResultType.Error;
            }

        }

        public static void LoadFieldType(DropDownList objDropDownList)
        {
            try
            {
                Array objArray = System.Enum.GetValues(typeof(FieldType));
                foreach (var varItem in objArray)
                {
                    int intValue = (int)(FieldType)Enum.Parse(typeof(FieldType), varItem.ToString(), true);
                    ListItem objListItem = new ListItem(varItem.ToString(), intValue.ToString());
                    objDropDownList.Items.Add(objListItem);
                }
                objDropDownList.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        public static Utility.ResultType LoadRepository(DropDownList objDropDownList)
        {
            try
            {
                objDropDownList.Items.Clear();
                Utility objUtility = new Utility();
                DataTable objDataTable = new DataTable();
                objUtility.Result = RepositoryManager.SelectRepositoryForDropDown(out objDataTable);

                switch (objUtility.Result)
                {
                    case ResultType.Success:
                        DataView objDataView = objDataTable.DefaultView;
                        objDataView.Sort = "RepositoryName ASC";

                        objDropDownList.DataSource = objDataView;
                        objDropDownList.DataTextField = "RepositoryName";
                        objDropDownList.DataValueField = "ID";
                        objDropDownList.DataBind();
                        objDropDownList.Items.Insert(0, new ListItem("--SELECT--", "-1"));
                        return ResultType.Success;
                        break;

                    case ResultType.Failure:
                        objDropDownList.Items.Insert(0, new ListItem("--NONE--", "-1"));
                        return ResultType.Failure;
                        break;

                    case ResultType.Error:
                        objDropDownList.Items.Insert(0, new ListItem("--ERROR--", "-1"));
                        return ResultType.Error;
                        break;
                }
                return ResultType.Success;
            }
            catch (Exception ex)
            {
                objDropDownList.Items.Insert(0, new ListItem("--ERROR--", "-1"));
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return ResultType.Error;
            }
        }


        public static Utility.ResultType LoadAccessType (DropDownList objDropDownList)
        {
            try
            {
                objDropDownList.Items.Clear();
                Utility objUtility = new Utility();
                DataTable objDataTable = new DataTable();
                objUtility.Result = RepositoryManager.SelectAccessTypeForDropDown(out objDataTable);

                switch (objUtility.Result)
                {
                    case ResultType.Success:
                        DataView objDataView = objDataTable.DefaultView;
                        objDataView.Sort = "Access ASC";

                        objDropDownList.DataSource = objDataView;
                        objDropDownList.DataTextField = "Access";
                        objDropDownList.DataValueField = "ID";
                        objDropDownList.DataBind();
                        objDropDownList.Items.Insert(0, new ListItem("--SELECT--", "-1"));
                        return ResultType.Success;
                        break;

                    case ResultType.Failure:
                        objDropDownList.Items.Insert(0, new ListItem("--NONE--", "-1"));
                        return ResultType.Failure;
                        break;

                    case ResultType.Error:
                        objDropDownList.Items.Insert(0, new ListItem("--ERROR--", "-1"));
                        return ResultType.Error;
                        break;
                }
                return ResultType.Success;
            }
            catch (Exception ex)
            {
                objDropDownList.Items.Insert(0, new ListItem("--ERROR--", "-1"));
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return ResultType.Error;
            }
        }


        public static Utility.ResultType LoadUserEmail(DropDownList objDropDownList)
        {
            try
            {
                objDropDownList.Items.Clear();
                Utility objUtility = new Utility();
                DataTable objDataTable = new DataTable();
                objUtility.Result = RepositoryManager.SelectUserEmailForDropDown(out objDataTable);

                switch (objUtility.Result)
                {
                    case ResultType.Success:
                        DataView objDataView = objDataTable.DefaultView;
                        objDataView.Sort = "EmailID ASC";

                        objDropDownList.DataSource = objDataView;
                        objDropDownList.DataTextField = "EmailID";
                        objDropDownList.DataValueField = "ID";
                        objDropDownList.DataBind();
                        objDropDownList.Items.Insert(0, new ListItem("--SELECT--", "-1"));
                        return ResultType.Success;
                        break;

                    case ResultType.Failure:
                        objDropDownList.Items.Insert(0, new ListItem("--NONE--", "-1"));
                        return ResultType.Failure;
                        break;

                    case ResultType.Error:
                        objDropDownList.Items.Insert(0, new ListItem("--ERROR--", "-1"));
                        return ResultType.Error;
                        break;
                }
                return ResultType.Success;
            }
            catch (Exception ex)
            {
                objDropDownList.Items.Insert(0, new ListItem("--ERROR--", "-1"));
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return ResultType.Error;
            }
        }

        public static Utility.ResultType LoadRepositoryWindow(System.Windows.Forms.ComboBox objComboBox, DataTable objDataTable)
        {
            try
            {
                objComboBox.Items.Clear();

                Utility objUtility = new Utility();

                if (objDataTable.Rows.Count > 0)
                {
                    foreach (DataRow objDataRow in objDataTable.Rows)
                    {
                        objComboBox.Items.Add(new ListItem(objDataRow["RepositoryName"].ToString(), objDataRow["ID"].ToString()));
                    }
                    objComboBox.Items.Insert(0, new ListItem("--SELECT--", "-1"));
                    objComboBox.SelectedIndex = 0;
                    return ResultType.Success;
                }
                else if (objDataTable.Rows.Count == 0)
                {
                    objComboBox.Items.Insert(0, new ListItem("--NONE--", "-1"));
                    objComboBox.SelectedIndex = 0;
                    return ResultType.Failure;
                }
                return ResultType.Success;
            }
            catch (Exception ex)
            {
                objComboBox.Items.Insert(0, new ListItem("--ERROR--", "-1"));
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return ResultType.Error;
            }
        }

        public static Utility.ResultType LoadMetaTemplate(DropDownList objDropDownList, int intRepositoryID)
        {
            try
            {
                objDropDownList.Items.Clear();
                Utility objUtility = new Utility();
                DataTable objDataTable = new DataTable();
                objUtility.Result = MetaTemplateManager.SelectMetaTemplateForDropDown(out objDataTable, intRepositoryID);

                switch (objUtility.Result)
                {
                    case ResultType.Success:
                        DataView objDataView = objDataTable.DefaultView;
                        objDataView.Sort = "MetaTemplateName ASC";

                        objDropDownList.DataSource = objDataView;
                        objDropDownList.DataTextField = "MetaTemplateName";
                        objDropDownList.DataValueField = "ID";
                        objDropDownList.DataBind();
                        if (System.Web.HttpContext.Current.Session["UserID"].ToString() == "956")
                        {
                            objDropDownList.Items.Insert(0, new ListItem("ALL", "0"));
                        }
                        else
                        {
                            objDropDownList.Items.Insert(0, new ListItem("--SELECT--", "-1"));
                        }

                        return objUtility.Result;
                        break;

                    case ResultType.Failure:
                        objDropDownList.Items.Insert(0, new ListItem("--NONE--", "-1"));
                        return ResultType.Failure;
                        break;

                    case ResultType.Error:
                        objDropDownList.Items.Insert(0, new ListItem("--ERROR--", "-1"));
                        return ResultType.Error;
                        break;
                }
                return ResultType.Success;
            }
            catch (Exception ex)
            {
                objDropDownList.Items.Insert(0, new ListItem("--ERROR--", "-1"));
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return ResultType.Error;
            }
        }

        public static Utility.ResultType LoadMetaTemplateWindow(System.Windows.Forms.ComboBox objComboBox, DataTable objDataTable)
        {
            try
            {
                objComboBox.DataSource = null;
                objComboBox.Items.Clear();                              
                
                if (objDataTable.Rows.Count > 0)
                {
                    foreach (DataRow objDataRow in objDataTable.Rows)
                    {
                        objComboBox.Items.Add(new ListItem(objDataRow["MetaTemplateName"].ToString(), objDataRow["ID"].ToString()));
                    }
                    objComboBox.Items.Insert(0, new ListItem("--SELECT--", "-1"));
                    objComboBox.SelectedIndex = 0;
                    return ResultType.Success;
                }
                else if (objDataTable.Rows.Count == 0)
                {
                    objComboBox.Items.Insert(0, new ListItem("--NONE--", "-1"));
                    objComboBox.SelectedIndex = 0;
                    return ResultType.Failure;
                }
                return ResultType.Success;
            }
            catch (Exception ex)
            {
                objComboBox.Items.Insert(0, new ListItem("--ERROR--", "-1"));
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return ResultType.Error;
            }
        }

        public static Utility.ResultType LoadCategory(DropDownList objDropDownList, int intMetaTemplateID)
        {
            try
            {
                CategoryManager objCategoryManager = new CategoryManager();
                objDropDownList.Items.Clear();
                Utility objUtility = new Utility();
                DataTable objDataTable = new DataTable();
                objUtility.Result = objCategoryManager.SelectCategoryForDropDown(out objDataTable, intMetaTemplateID);

                switch (objUtility.Result)
                {
                    case ResultType.Success:
                        DataView objDataView = objDataTable.DefaultView;
                        objDataView.Sort = "CategoryName ASC";

                        objDropDownList.DataSource = objDataView;
                        objDropDownList.DataTextField = "CategoryName";
                        objDropDownList.DataValueField = "ID";
                        objDropDownList.DataBind();
                        objDropDownList.Items.Insert(0, new ListItem("--SELECT--", "-1"));
                        return objUtility.Result;
                        break;

                    case ResultType.Failure:
                        objDropDownList.Items.Insert(0, new ListItem("--NONE--", "-1"));
                        return ResultType.Failure;
                        break;

                    case ResultType.Error:
                        objDropDownList.Items.Insert(0, new ListItem("--ERROR--", "-1"));
                        return ResultType.Error;
                        break;
                }
                return ResultType.Success;
            }
            catch (Exception ex)
            {
                objDropDownList.Items.Insert(0, new ListItem("--ERROR--", "-1"));
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return ResultType.Error;
            }
        }

        public static Utility.ResultType LoadCategoryWindow(System.Windows.Forms.ComboBox objComboBox, DataTable objDataTable)
        {
            try
            {
                objComboBox.DataSource = null;
                objComboBox.Items.Clear();

                if (objDataTable.Rows.Count > 0)
                {
                    foreach (DataRow objDataRow in objDataTable.Rows)
                    {
                        objComboBox.Items.Add(new ListItem(objDataRow["CategoryName"].ToString(), objDataRow["ID"].ToString()));
                    }
                    objComboBox.Items.Insert(0, new ListItem("--SELECT--", "-1"));
                   // objComboBox.SelectedIndex = 0;
                    return ResultType.Success;
                }
                else if (objDataTable.Rows.Count == 0)
                {
                    objComboBox.Items.Insert(0, new ListItem("--NONE--", "-1"));
                    objComboBox.SelectedIndex = 0;
                    return ResultType.Failure;
                }
                return ResultType.Success;
            }
            catch (Exception ex)
            {
                objComboBox.Items.Insert(0, new ListItem("--ERROR--", "-1"));
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return ResultType.Error;
            }
        }

        public static Utility.ResultType LoadDataType(DropDownList objDropDownList)
        {
            try
            {
                objDropDownList.Items.Clear();
                Utility objUtility = new Utility();
                DataTable objDataTable = new DataTable();
                objUtility.Result = MetaTemplateManager.SelectDataTypeForDropDown(out objDataTable);

                switch (objUtility.Result)
                {
                    case ResultType.Success:
                        objDropDownList.DataSource = objDataTable;
                        objDropDownList.DataTextField = "DataTypeName";
                        objDropDownList.DataValueField = "ID";
                        objDropDownList.DataBind();
                        objDropDownList.Items.Insert(0, new ListItem("--SELECT--", "-1"));
                        return objUtility.Result;
                        break;

                    case ResultType.Failure:
                        objDropDownList.Items.Insert(0, new ListItem("--NONE--", "-1"));
                        return ResultType.Failure;
                        break;

                    case ResultType.Error:
                        objDropDownList.Items.Insert(0, new ListItem("--NONE--", "-1"));
                        return ResultType.Error;
                        break;
                }
                return ResultType.Success;
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return ResultType.Error;
            }
        }

        public static Utility.ResultType LoadFolder(System.Web.UI.WebControls.TreeView objTreeView, int intMetaTemplateID)
        {
            try

            {
                objTreeView.Nodes.Clear();
                Utility objUtility = new Utility();
                DataTable objDataTable = new DataTable();
                objUtility.Result = FolderManager.SelectFolderByMetaTemplateID(out objDataTable, intMetaTemplateID);
                objTreeView.ExpandAll();
                System.Web.UI.WebControls.TreeNode objTreeNode = new System.Web.UI.WebControls.TreeNode("-- My Folders --", "0");
                objTreeNode.Selected = true;
                objTreeView.Nodes.Add(objTreeNode);

                switch (objUtility.Result)
                {
                    case ResultType.Success:
                        DataView objDataView = new DataView(objDataTable);
                        objDataView.RowFilter = "ParentFolderID = 0";
                        objDataView.Sort = "FolderName";
                            
                        foreach (DataRowView objDataRowView in objDataView)
                        {
                            System.Web.UI.WebControls.TreeNode objTreeNodeParent = new System.Web.UI.WebControls.TreeNode(objDataRowView["FolderName"].ToString(), objDataRowView["ID"].ToString());
                            objTreeNodeParent.ToolTip = objDataRowView["FolderDescription"].ToString();
                            objTreeNode.ChildNodes.Add(objTreeNodeParent);

                            AddFolderInTreeNode(objTreeNodeParent, Convert.ToInt32(objDataRowView["ID"]), objDataTable);
                        }

                        return objUtility.Result;
                        break;

                    case ResultType.Failure:
                        return objUtility.Result;
                        break;

                    case ResultType.Error:
                        return objUtility.Result;
                        break;
                }
                return ResultType.Success;
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return ResultType.Error;
            }
        }

        public static Utility.ResultType LoadFolder_Centrum(System.Web.UI.WebControls.TreeView objTreeView, int intMetaTemplateID,int CategoryID)
        {
            try
            {
                objTreeView.Nodes.Clear();
                Utility objUtility = new Utility();
                DataTable objDataTable = new DataTable();
                objUtility.Result = FolderManager.SelectFolderByCategoryID_Centrum(out objDataTable, intMetaTemplateID, CategoryID);
                objTreeView.ExpandAll();
                System.Web.UI.WebControls.TreeNode objTreeNode = new System.Web.UI.WebControls.TreeNode("--NONE--", "0");
                objTreeNode.Selected = true;
                objTreeView.Nodes.Add(objTreeNode);

                switch (objUtility.Result)
                {
                    case ResultType.Success:
                        DataView objDataView = new DataView(objDataTable);
                        objDataView.RowFilter = "ParentFolderID = 0";
                        objDataView.Sort = "FolderName";

                        foreach (DataRowView objDataRowView in objDataView)
                        {
                            System.Web.UI.WebControls.TreeNode objTreeNodeParent = new System.Web.UI.WebControls.TreeNode(objDataRowView["FolderName"].ToString(), objDataRowView["ID"].ToString());
                            objTreeNodeParent.ToolTip = objDataRowView["FolderDescription"].ToString();
                            objTreeNode.ChildNodes.Add(objTreeNodeParent);

                            AddFolderInTreeNode(objTreeNodeParent, Convert.ToInt32(objDataRowView["ID"]), objDataTable);
                        }

                        return objUtility.Result;
                        break;

                    case ResultType.Failure:
                        return objUtility.Result;
                        break;

                    case ResultType.Error:
                        return objUtility.Result;
                        break;
                }
                return ResultType.Success;
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return ResultType.Error;
            }
        }

        #region Seema 9Nov 2017

        public static Utility.ResultType LoadFolder_AxisTree(System.Web.UI.WebControls.TreeView objTreeView, int intMetaTemplateID, int CategoryID)
        {
            try
            {
                objTreeView.Nodes.Clear();
                Utility objUtility = new Utility();
                DataTable objDataTable = new DataTable();
                objUtility.Result = FolderManager.SelectFolderByCategoryID(out objDataTable, intMetaTemplateID, CategoryID);
                objTreeView.ExpandAll();
                System.Web.UI.WebControls.TreeNode objTreeNode = new System.Web.UI.WebControls.TreeNode("--NONE--", "0");
                objTreeNode.Selected = true;
                objTreeView.Nodes.Add(objTreeNode);

                switch (objUtility.Result)
                {
                    case ResultType.Success:
                        DataView objDataView = new DataView(objDataTable);
                        objDataView.RowFilter = "ParentFolderID = 0";
                        objDataView.Sort = "FolderName";

                        foreach (DataRowView objDataRowView in objDataView)
                        {
                            System.Web.UI.WebControls.TreeNode objTreeNodeParent = new System.Web.UI.WebControls.TreeNode(objDataRowView["FolderName"].ToString(), objDataRowView["ID"].ToString());
                            objTreeNodeParent.ToolTip = objDataRowView["FolderDescription"].ToString();
                            objTreeNode.ChildNodes.Add(objTreeNodeParent);

                            AddFolderInTreeNode(objTreeNodeParent, Convert.ToInt32(objDataRowView["ID"]), objDataTable);
                        }

                        return objUtility.Result;
                        break;

                    case ResultType.Failure:
                        return objUtility.Result;
                        break;

                    case ResultType.Error:
                        return objUtility.Result;
                        break;
                }
                return ResultType.Success;
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return ResultType.Error;
            }
        }

        #endregion

        public static void AddFolderInTreeNode(System.Web.UI.WebControls.TreeNode objTreeNodeParent, int intParentFolderID, DataTable objDataTable)
        {
            try
            {
                DataView objDataView = new DataView(objDataTable);
                objDataView.RowFilter = "ParentFolderID = " + intParentFolderID;
                objDataView.Sort = "FolderName";

                foreach (DataRowView objDataRowView in objDataView)
                {
                    System.Web.UI.WebControls.TreeNode objTreeNodeChild = new System.Web.UI.WebControls.TreeNode(objDataRowView["FolderName"].ToString(), objDataRowView["ID"].ToString());
                    objTreeNodeChild.ToolTip = objDataRowView["FolderDescription"].ToString();
                    objTreeNodeParent.ChildNodes.Add(objTreeNodeChild);

                    AddFolderInTreeNode(objTreeNodeChild, Convert.ToInt32(objDataRowView["ID"]), objDataTable);
                }
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        public static Utility.ResultType LoadFolderWindow(System.Windows.Forms.TreeView objTreeView, DataTable objDataTable)
        {
            try
            {
                objTreeView.Nodes.Clear();
                Utility objUtility = new Utility();
                
                objTreeView.ExpandAll();
                System.Windows.Forms.TreeNode objTreeNode = new System.Windows.Forms.TreeNode() { Text = "-- My Folders --", Tag = "0" };
                objTreeView.Nodes.Add(objTreeNode);


                DataView objDataView = new DataView(objDataTable);
                objDataView.RowFilter = "ParentFolderID = 0";
                objDataView.Sort = "FolderName";

                foreach (DataRowView objDataRowView in objDataView)
                {
                    System.Windows.Forms.TreeNode objTreeNodeParent = new System.Windows.Forms.TreeNode() { Text = objDataRowView["FolderName"].ToString(), Tag = objDataRowView["ID"].ToString() };
                    objTreeNodeParent.ToolTipText = objDataRowView["FolderDescription"].ToString();
                    objTreeNode.Nodes.Add(objTreeNodeParent);

                    AddFolderInTreeNodeWindow(objTreeNodeParent, Convert.ToInt32(objDataRowView["ID"]), objDataTable);
                }
                return ResultType.Success;
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return ResultType.Error;
            }
        }

        public static void AddFolderInTreeNodeWindow(System.Windows.Forms.TreeNode objTreeNodeParent, int intParentFolderID, DataTable objDataTable)
        {
            try
            {
                DataView objDataView = new DataView(objDataTable);
                objDataView.RowFilter = "ParentFolderID = " + intParentFolderID;
                objDataView.Sort = "FolderName";

                foreach (DataRowView objDataRowView in objDataView)
                {
                    System.Windows.Forms.TreeNode objTreeNodeChild = new System.Windows.Forms.TreeNode() { Text = objDataRowView["FolderName"].ToString(), Tag = objDataRowView["ID"].ToString() };
                    objTreeNodeChild.ToolTipText = objDataRowView["FolderDescription"].ToString();
                    objTreeNodeParent.Nodes.Add(objTreeNodeChild);

                    AddFolderInTreeNodeWindow(objTreeNodeChild, Convert.ToInt32(objDataRowView["ID"]), objDataTable);
                }
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        public static Utility.ResultType LoadMetaDataCode(DropDownList objDropDownList, MetaData objMetaData)
        {
            try
            {
                DocumentManager objDocumentManager = new DocumentManager();
                objDropDownList.Items.Clear();
                Utility objUtility = new Utility();
                DataTable objDataTable = new DataTable();
                objUtility.Result = objDocumentManager.SelectMetaDataForDropDown(out objDataTable, objMetaData);

                switch (objUtility.Result)
                {
                    case ResultType.Success:
                        DataView objDataView = objDataTable.DefaultView;
                        objDataView.Sort = "MetaDataCode DESC";

                        objDropDownList.DataSource = objDataView;
                        objDropDownList.DataTextField = "MetaDataCode";
                        objDropDownList.DataValueField = "ID";
                        objDropDownList.DataBind();
                        objDropDownList.Items.Insert(0, new ListItem("--SELECT--", "-1"));
                        return objUtility.Result;
                        break;

                    case ResultType.Failure:
                        objDropDownList.Items.Insert(0, new ListItem("--NONE--", "-1"));
                        return ResultType.Failure;
                        break;

                    case ResultType.Error:
                        objDropDownList.Items.Insert(0, new ListItem("--ERROR--", "-1"));
                        return ResultType.Error;
                        break;
                }
                return ResultType.Success;
            }
            catch (Exception ex)
            {
                objDropDownList.Items.Insert(0, new ListItem("--ERROR--", "-1"));
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return ResultType.Error;
            }
        }

        public static Utility.ResultType LoadMetaDataCodeWindow(System.Windows.Forms.ComboBox objComboBox, DataTable objDataTable)
        {
            try
            {
                objComboBox.DataSource = null;
                objComboBox.Items.Clear();

                DocumentManager objDocumentManager = new DocumentManager();

                Utility objUtility = new Utility();
                
                if (objDataTable.Rows.Count > 0)
                {
                    foreach (DataRow objDataRow in objDataTable.Rows)
                    {
                        objComboBox.Items.Add(new ListItem(objDataRow["MetaDataCode"].ToString(), objDataRow["ID"].ToString()));
                    }
                    objComboBox.Items.Insert(0, new ListItem("--SELECT--", "-1"));
                    objComboBox.SelectedIndex = 0;
                    return ResultType.Success;
                }
                else if (objDataTable.Rows.Count == 0)
                {
                    objComboBox.Items.Insert(0, new ListItem("--NONE--", "-1"));
                    objComboBox.SelectedIndex = 0;
                    return ResultType.Failure;
                }
                return ResultType.Success;
            }
            catch (Exception ex)
            {
                objComboBox.Items.Insert(0, new ListItem("--ERROR--", "-1"));
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return ResultType.Error;
            }
        }

        public static void ConvertImageToPdf(string inputFileName, string outputFileName, string fileType)
        {
            if (fileType == ".tiff" || fileType == ".tif" || fileType == ".TIFF" || fileType == ".TIF")
            {
                iTextSharp.text.Document document = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 0, 0, 0, 0);

                iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(document, new System.IO.FileStream(outputFileName, System.IO.FileMode.Create));

                System.Drawing.Bitmap bm = new System.Drawing.Bitmap(inputFileName);
                int total = bm.GetFrameCount(System.Drawing.Imaging.FrameDimension.Page);

                document.Open();
                iTextSharp.text.pdf.PdfContentByte cb = writer.DirectContent;
                for (int k = 0; k < total; ++k)
                {
                    bm.SelectActiveFrame(System.Drawing.Imaging.FrameDimension.Page, k);
                    iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(bm, System.Drawing.Imaging.ImageFormat.Bmp);
                    img.ScalePercent(72f / img.DpiX * 100);
                    img.SetAbsolutePosition(0, 0);
                    cb.AddImage(img);
                    document.NewPage();
                }
                document.Close();
            }
        }

        public static byte[] ConvertImageToPdfFromFileByte(string inputFileName, byte[] byteExistingFileByte)
        {
            try
            {
                if (Utility.FileStorageType == StorageType.FileSystem)
                {
                    if (File.Exists(inputFileName))
                    {

                        iTextSharp.text.Document document = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 0, 0, 0, 0);

                        byte[] byteFileByte = File.ReadAllBytes(inputFileName);

                        if (Path.GetExtension(inputFileName).ToLower() == ".pdf")
                        {
                            return byteFileByte;
                        }

                        MemoryStream objMemoryStream = new MemoryStream();
                        iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(document, objMemoryStream);

                        MemoryStream objMemoryStreamFileByte = new MemoryStream(byteFileByte);
                        System.Drawing.Bitmap bm = new System.Drawing.Bitmap(objMemoryStreamFileByte);
                        int total = bm.GetFrameCount(System.Drawing.Imaging.FrameDimension.Page);

                        document.Open();
                        iTextSharp.text.pdf.PdfContentByte cb = writer.DirectContent;
                        for (int k = 0; k < total; ++k)
                        {
                            bm.SelectActiveFrame(System.Drawing.Imaging.FrameDimension.Page, k);
                            iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(bm, System.Drawing.Imaging.ImageFormat.Bmp);
                            img.ScalePercent(72f / img.DpiX * 100);
                            img.SetAbsolutePosition(0, 0);
                            cb.AddImage(img);
                            document.NewPage();
                        }
                        document.Close();

                        return objMemoryStream.ToArray();
                    }
                }
                else if (Utility.FileStorageType == StorageType.DatabaseSystem)
                {
                    iTextSharp.text.Document document = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 0, 0, 0, 0);

                    if (byteExistingFileByte == null || byteExistingFileByte.Length == 0)
                    {
                        return File.ReadAllBytes(HttpContext.Current.Server.MapPath("~/Others/FileNotFound.pdf"));
                    }

                    if (Path.GetExtension(inputFileName).ToLower() == ".pdf")
                    {
                        return byteExistingFileByte;
                    }

                    MemoryStream objMemoryStream = new MemoryStream();
                    iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(document, objMemoryStream);

                    MemoryStream objMemoryStreamFileByte = new MemoryStream(byteExistingFileByte);
                    System.Drawing.Bitmap bm = new System.Drawing.Bitmap(objMemoryStreamFileByte);
                    int total = bm.GetFrameCount(System.Drawing.Imaging.FrameDimension.Page);

                    document.Open();
                    iTextSharp.text.pdf.PdfContentByte cb = writer.DirectContent;
                    for (int k = 0; k < total; ++k)
                    {
                        bm.SelectActiveFrame(System.Drawing.Imaging.FrameDimension.Page, k);
                        iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(bm, System.Drawing.Imaging.ImageFormat.Bmp);
                        img.ScalePercent(72f / img.DpiX * 100);
                        img.SetAbsolutePosition(0, 0);
                        cb.AddImage(img);
                        document.NewPage();
                    }
                    document.Close();

                    return objMemoryStream.ToArray();
                }

                return File.ReadAllBytes(HttpContext.Current.Server.MapPath("~/Others/FileNotFound.pdf"));
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return File.ReadAllBytes(HttpContext.Current.Server.MapPath("~/Others/FileNotFound.pdf"));
            }
        }

        public static void GenerateControlAndFillData(GridViewRowEventArgs objGridViewRowEventArgs, GridView objGridView, HiddenField objHiddenField, ref byte[] byteFileByte, DataTable objDataTableDocument, DataTable objDataTableField, DataTable objDataTableList, EnableOrDisable enumEnableOrDisable)
        {
            System.Web.UI.WebControls.Label objLabel;
            System.Web.UI.WebControls.TextBox objTextBox;
            CheckBoxList objCheckBoxList;
            RequiredFieldValidator objRequiredFieldValidator;
            AjaxControlToolkit.FilteredTextBoxExtender objFilteredTextBoxExtender;
            AjaxControlToolkit.MaskedEditExtender objMaskedEditExtender;
            AjaxControlToolkit.MaskedEditValidator objMaskedEditValidator;
            CalendarExtender objCalendarExtender;
            DropDownList objDropDownList;
            CompareValidator objCompareValidator;

            if (objGridViewRowEventArgs.Row.RowType == DataControlRowType.DataRow)
            {
                string strfilePath = DataBinder.Eval(objGridViewRowEventArgs.Row.DataItem, "FilePath").ToString();
                string strDocumentID = DataBinder.Eval(objGridViewRowEventArgs.Row.DataItem, "DocumentID").ToString();
                try
                {
                    if (Utility.FileStorageType == StorageType.DatabaseSystem)
                    {
                        if (Path.GetExtension(strfilePath).ToLower() == ".pdf")
                        {
                            byteFileByte = ConvertImageToPdfFromFileByte(DataBinder.Eval(objGridViewRowEventArgs.Row.DataItem, "FilePath").ToString(), (byte[])DataBinder.Eval(objGridViewRowEventArgs.Row.DataItem, "FileByte"));
                        }
                        else if (Path.GetExtension(strfilePath).ToLower() != ".pdf")
                        {
                            byteFileByte = ConvertImageToPdfFromFileByte(DataBinder.Eval(objGridViewRowEventArgs.Row.DataItem, "FilePath").ToString(), (byte[])DataBinder.Eval(objGridViewRowEventArgs.Row.DataItem, "FileByte"));
                            objDataTableDocument.Select("DocumentID='" + strDocumentID + "'")[0]["FileByte"] = byteFileByte;
                            objDataTableDocument.Select("DocumentID='" + strDocumentID + "'")[0]["FilePath"] = Path.ChangeExtension(strfilePath, ".pdf");
                            objDataTableDocument.AcceptChanges();
                        }
                    }
                    else if (Utility.FileStorageType == StorageType.FileSystem)
                    {
                        if (DataBinder.Eval(objGridViewRowEventArgs.Row.DataItem, "FileByte") != DBNull.Value)
                        {
                            byteFileByte = (byte[])DataBinder.Eval(objGridViewRowEventArgs.Row.DataItem, "FileByte");
                        }
                        else
                        {
                            byteFileByte = ConvertImageToPdfFromFileByte(DataBinder.Eval(objGridViewRowEventArgs.Row.DataItem, "FilePath").ToString(), null);

                            #region Temporary Code
                            //if (enumEnableOrDisable == EnableOrDisable.Enable)
                            //{
                            //    PdfReader objPdfReader = new PdfReader(byteFileByte);
                            //    if (objPdfReader.NumberOfPages > 1)
                            //    {
                            //        MemoryStream objMemoryStream = new MemoryStream();
                            //        using (iTextSharp.text.Document objDocument = new iTextSharp.text.Document())
                            //        {
                            //            using (PdfWriter w = PdfWriter.GetInstance(objDocument, objMemoryStream))
                            //            {
                            //                objDocument.Open();
                            //                int i=2;
                            //                while (i <= objPdfReader.NumberOfPages)
                            //                {
                            //                    objDocument.NewPage();
                            //                    w.DirectContent.AddTemplate(w.GetImportedPage(objPdfReader, i), 0, 0);
                            //                    i++;
                            //                }
                            //                objDocument.Close();
                            //            }
                            //        }
                            //        byteFileByte = null;
                            //        byteFileByte = objMemoryStream.ToArray();
                            //    }
                            //}
                            #endregion

                            objDataTableDocument.Select("DocumentID='" + strDocumentID + "'")[0]["FileByte"] = byteFileByte;
                            objDataTableDocument.AcceptChanges();
                        }

                    }

                    if (objHiddenField.Value == "1")
                    {
                        ((ShowPdf)objGridViewRowEventArgs.Row.FindControl("pdfViewer")).FilePath = "../Handler/ImageHandler1.ashx";
                        objHiddenField.Value = "2";
                    }
                    else if (objHiddenField.Value == "2")
                    {
                        ((ShowPdf)objGridViewRowEventArgs.Row.FindControl("pdfViewer")).FilePath = "../Handler/ImageHandler2.ashx";
                        objHiddenField.Value = "1";
                    }
                }
                catch (Exception ex)
                {
                    ((ShowPdf)objGridViewRowEventArgs.Row.FindControl("pdfViewer")).FilePath = "../Others/FileNotFound.pdf";
                    LogManager.ErrorLog(BusinessLogic.Utility.LogFilePath, ex);
                }


                DataRow[] objDataRowDocument = objDataTableDocument.Select("DocumentID='" + strDocumentID + "'");

                HtmlTable objHtmlTable = new HtmlTable();

                foreach (DataRow objDataRow in objDataTableField.Rows)
                {
                    HtmlTableRow objHtmlTableRow = new HtmlTableRow();
                    objHtmlTable.Rows.Add(objHtmlTableRow);

                    HtmlTableCell objHtmlTableCellLabel = new HtmlTableCell();
                    objLabel = new System.Web.UI.WebControls.Label();
                    objLabel.Text = objDataRow["FieldName"].ToString();
                    objLabel.Width = Unit.Pixel(200);
                    objHtmlTableCellLabel.Controls.Add(objLabel);



                    HtmlTableCell objHtmlTableCellControl = new HtmlTableCell();
                    objTextBox = new System.Web.UI.WebControls.TextBox();

                    if (objDataRow["FieldDataTypeID"].ToString().Trim() != "4" && objDataRow["FieldDataTypeID"].ToString().Trim() != "9")
                    {
                        objTextBox.ID = objDataRow["ID"].ToString();
                        objTextBox.Text = objDataRowDocument[0][objDataRow["ID"].ToString()].ToString();
                        objTextBox.MaxLength = Convert.ToInt32(objDataRow["FieldLength"]);
                        if (enumEnableOrDisable == EnableOrDisable.Disable)
                        {
                            objTextBox.Enabled = false;
                        }
                        objHtmlTableCellControl.Controls.Add(objTextBox);
                    }
                    else if (objDataRow["FieldDataTypeID"].ToString().Trim() == "9")
                    {
                        System.Web.UI.WebControls.Panel objPanelList = new System.Web.UI.WebControls.Panel();
                        objPanelList.ScrollBars = System.Web.UI.WebControls.ScrollBars.Auto;
                        objPanelList.Height = Unit.Pixel(75);

                        objCheckBoxList = new CheckBoxList();
                        objCheckBoxList.ID = objDataRow["ID"].ToString();
                        objCheckBoxList.Height = Unit.Pixel(50);

                        DataView objDataView = objDataTableList.DefaultView;
                        objDataView.RowFilter = "FieldID='" + objDataRow["ID"].ToString() + "'";
                        objDataView.Sort = "ListItemText ASC";
                        objCheckBoxList.DataSource = objDataView;
                        objCheckBoxList.DataTextField = "ListItemText";
                        objCheckBoxList.DataValueField = "ID";
                        objCheckBoxList.DataBind();

                        if (objDataView.Count == 0)
                        {
                            objCheckBoxList.Items.Insert(0, new ListItem("No Data Available", "0"));
                        }
                        if (objDataView.Count > 0)
                        {
                            string strSelectedItemValue = objDataRowDocument[0][objDataRow["ID"].ToString()].ToString();
                            string[] strSelectedItem = strSelectedItemValue.Split(',');
                            if (strSelectedItem.Length > 0)
                            {
                                foreach (ListItem objListItem in objCheckBoxList.Items)
                                {
                                    if (strSelectedItem.Contains(objListItem.Value))
                                    {
                                        objListItem.Selected = true;
                                    }
                                    else
                                    {
                                        objListItem.Selected = false;
                                    }
                                }
                            }
                        }
                        if (enumEnableOrDisable == EnableOrDisable.Disable)
                        {
                            objCheckBoxList.Enabled = false;
                        }
                        objPanelList.Controls.Add(objCheckBoxList);
                        objHtmlTableCellControl.Controls.Add(objPanelList);
                    }
                    else if (objDataRow["FieldDataTypeID"].ToString().Trim() == "4")
                    {
                        objDropDownList = new DropDownList();
                        objDropDownList.ID = objDataRow["ID"].ToString();

                        DataView objDataView = objDataTableList.DefaultView;
                        objDataView.RowFilter = "FieldID='" + objDataRow["ID"].ToString() + "'";
                        objDataView.Sort = "ListItemText ASC";
                        objDropDownList.DataSource = objDataView;
                        objDropDownList.DataTextField = "ListItemText";
                        objDropDownList.DataValueField = "ID";
                        objDropDownList.DataBind();
                        if (objDataView.Count == 0)
                        {
                            objDropDownList.Items.Insert(0, new ListItem("No Data Available", "0"));
                        }
                        objDropDownList.SelectedIndex = objDropDownList.Items.IndexOf(objDropDownList.Items.FindByValue(objDataRowDocument[0][objDataRow["ID"].ToString()].ToString()));

                        if (enumEnableOrDisable == EnableOrDisable.Disable)
                        {
                            objDropDownList.Enabled = false;
                        }
                        objHtmlTableCellControl.Controls.Add(objDropDownList);

                    }


                    HtmlTableCell objHtmlTableCellValidator = new HtmlTableCell();
                    if (objDataRow["FieldDataTypeID"].ToString().Trim() != "4" && objDataRow["FieldDataTypeID"].ToString().Trim() != "9")
                    {
                        if (objDataRow["FieldTypeID"].ToString().Trim() != "0")
                        {
                            objRequiredFieldValidator = new RequiredFieldValidator();
                            objRequiredFieldValidator.ControlToValidate = objTextBox.UniqueID;
                            objRequiredFieldValidator.Display = ValidatorDisplay.Static;
                            objRequiredFieldValidator.Text = "*";
                            objHtmlTableCellValidator.Controls.Add(objRequiredFieldValidator);
                        }
                    }

                    HtmlTableCell objHtmlTableCellDataType = new HtmlTableCell();
                    switch (objDataRow["FieldDataTypeID"].ToString().Trim())
                    {
                        case "1":
                            objFilteredTextBoxExtender = new FilteredTextBoxExtender();
                            objFilteredTextBoxExtender.TargetControlID = objTextBox.UniqueID;
                            objFilteredTextBoxExtender.FilterType = FilterTypes.UppercaseLetters | FilterTypes.LowercaseLetters | FilterTypes.Custom;
                            objFilteredTextBoxExtender.ValidChars = " ";
                            objHtmlTableCellDataType.Controls.Add(objFilteredTextBoxExtender);
                            break;

                        case "2":
                            objFilteredTextBoxExtender = new FilteredTextBoxExtender();
                            objFilteredTextBoxExtender.TargetControlID = objTextBox.UniqueID;
                            objFilteredTextBoxExtender.FilterType = FilterTypes.Numbers;
                            objHtmlTableCellDataType.Controls.Add(objFilteredTextBoxExtender);
                            break;

                        case "3":
                            objCalendarExtender = new CalendarExtender();
                            objCalendarExtender.TargetControlID = objTextBox.UniqueID;
                            objCalendarExtender.Format = "MM/dd/yyyy";
                            objCalendarExtender.PopupButtonID = objTextBox.UniqueID;

                            objMaskedEditExtender = new MaskedEditExtender();
                            objMaskedEditExtender.ID = "MEE" + objTextBox.UniqueID;
                            objMaskedEditExtender.TargetControlID = objTextBox.UniqueID;
                            objMaskedEditExtender.Mask = "99/99/9999";
                            objMaskedEditExtender.MaskType = MaskedEditType.Date;

                            objMaskedEditValidator = new MaskedEditValidator();
                            objMaskedEditValidator.ControlExtender = objMaskedEditExtender.UniqueID;
                            objMaskedEditValidator.ControlToValidate = objTextBox.UniqueID;
                            objMaskedEditValidator.InvalidValueMessage = "Invalid Date (mm/dd/yyyy)";

                            objHtmlTableCellDataType.Controls.Add(objCalendarExtender);
                            objHtmlTableCellDataType.Controls.Add(objMaskedEditExtender);
                            objHtmlTableCellDataType.Controls.Add(objMaskedEditValidator);
                            break;

                        case "10":
                            objMaskedEditExtender = new MaskedEditExtender();
                            objMaskedEditExtender.ID = "MEE" + objTextBox.UniqueID;
                            objMaskedEditExtender.TargetControlID = objTextBox.UniqueID;
                            objMaskedEditExtender.Mask = "99:99:99";
                            objMaskedEditExtender.MaskType = MaskedEditType.Time;
                            objMaskedEditExtender.AcceptAMPM = true;

                            objMaskedEditValidator = new MaskedEditValidator();
                            objMaskedEditValidator.ControlExtender = objMaskedEditExtender.UniqueID;
                            objMaskedEditValidator.ControlToValidate = objTextBox.UniqueID;
                            objMaskedEditValidator.InvalidValueMessage = "Invalid Date (hh:mm:ss AMPM)";

                            objHtmlTableCellDataType.Controls.Add(objMaskedEditExtender);
                            objHtmlTableCellDataType.Controls.Add(objMaskedEditValidator);
                            break;

                        case "5":
                            objMaskedEditExtender = new MaskedEditExtender();
                            objMaskedEditExtender.ID = "MEE" + objTextBox.UniqueID;
                            objMaskedEditExtender.TargetControlID = objTextBox.UniqueID;
                            objMaskedEditExtender.Mask = "99/99/9999 99:99:99";
                            objMaskedEditExtender.MaskType = MaskedEditType.DateTime;
                            objMaskedEditExtender.AcceptAMPM = true;

                            objMaskedEditValidator = new MaskedEditValidator();
                            objMaskedEditValidator.ControlExtender = objMaskedEditExtender.UniqueID;
                            objMaskedEditValidator.ControlToValidate = objTextBox.UniqueID;
                            objMaskedEditValidator.InvalidValueMessage = "Invalid Date (mm/dd/yyyy hh:mm:ss AMPM)";

                            objHtmlTableCellDataType.Controls.Add(objMaskedEditExtender);
                            objHtmlTableCellDataType.Controls.Add(objMaskedEditValidator);
                            break;

                        case "6":
                            objFilteredTextBoxExtender = new FilteredTextBoxExtender();
                            objFilteredTextBoxExtender.TargetControlID = objTextBox.UniqueID;
                            objFilteredTextBoxExtender.FilterType = FilterTypes.UppercaseLetters | FilterTypes.LowercaseLetters | FilterTypes.Custom | FilterTypes.Numbers;
                            objFilteredTextBoxExtender.ValidChars = " ";
                            objHtmlTableCellDataType.Controls.Add(objFilteredTextBoxExtender);
                            break;

                        case "7":
                            objFilteredTextBoxExtender = new FilteredTextBoxExtender();
                            objFilteredTextBoxExtender.TargetControlID = objTextBox.UniqueID;
                            objFilteredTextBoxExtender.FilterType = FilterTypes.Custom | FilterTypes.Numbers;
                            objFilteredTextBoxExtender.ValidChars = ".";

                            objCompareValidator = new CompareValidator();
                            objCompareValidator.ControlToValidate = objTextBox.UniqueID;
                            objCompareValidator.Type = ValidationDataType.Double;
                            objCompareValidator.ErrorMessage = "Invalid Decimal Number";

                            objHtmlTableCellDataType.Controls.Add(objFilteredTextBoxExtender);
                            objHtmlTableCellDataType.Controls.Add(objCompareValidator);
                            break;

                        case "8":
                            objFilteredTextBoxExtender = new FilteredTextBoxExtender();
                            objTextBox.TextMode = TextBoxMode.MultiLine;
                            objTextBox.Rows = 3;
                            objFilteredTextBoxExtender.TargetControlID = objTextBox.UniqueID;
                            objFilteredTextBoxExtender.FilterType = FilterTypes.UppercaseLetters | FilterTypes.LowercaseLetters | FilterTypes.Custom | FilterTypes.Numbers;
                            objFilteredTextBoxExtender.ValidChars = "., /*-+!@#$%&*()?;'{}[]";
                            objHtmlTableCellDataType.Controls.Add(objFilteredTextBoxExtender);
                            break;

                    }


                    objHtmlTableRow.Cells.Add(objHtmlTableCellLabel);
                    objHtmlTableRow.Cells.Add(objHtmlTableCellControl);
                    objHtmlTableRow.Cells.Add(objHtmlTableCellValidator);
                    objHtmlTableRow.Cells.Add(objHtmlTableCellDataType);

                }

                System.Web.UI.WebControls.Panel objPanel = (System.Web.UI.WebControls.Panel)objGridViewRowEventArgs.Row.FindControl("pnlHolderMain").FindControl("pnlHolder");
                objPanel.Controls.Add(objHtmlTable);
            }
            if (objGridViewRowEventArgs.Row.RowType == DataControlRowType.DataRow)
            {
                if (objGridView.PageIndex == 0)
                {
                    ((ImageButton)objGridViewRowEventArgs.Row.FindControl("pnlHolderMain").FindControl("pnlHolder").FindControl("btnFirst")).Enabled = false;
                    ((ImageButton)objGridViewRowEventArgs.Row.FindControl("pnlHolderMain").FindControl("pnlHolder").FindControl("btnPrevious")).Enabled = false;
                }
                if (objGridView.PageIndex != 0)
                {
                    ((ImageButton)objGridViewRowEventArgs.Row.FindControl("pnlHolderMain").FindControl("pnlHolder").FindControl("btnFirst")).Enabled = true;
                    ((ImageButton)objGridViewRowEventArgs.Row.FindControl("pnlHolderMain").FindControl("pnlHolder").FindControl("btnPrevious")).Enabled = true;
                }
                if (objGridView.PageIndex == objGridView.PageCount - 1)
                {
                    ((ImageButton)objGridViewRowEventArgs.Row.FindControl("pnlHolderMain").FindControl("pnlHolder").FindControl("btnNext")).Enabled = false;
                    ((ImageButton)objGridViewRowEventArgs.Row.FindControl("pnlHolderMain").FindControl("pnlHolder").FindControl("btnLast")).Enabled = false;
                }
                if (objGridView.PageIndex != objGridView.PageCount - 1)
                {
                    ((ImageButton)objGridViewRowEventArgs.Row.FindControl("pnlHolderMain").FindControl("pnlHolder").FindControl("btnNext")).Enabled = true;
                    ((ImageButton)objGridViewRowEventArgs.Row.FindControl("pnlHolderMain").FindControl("pnlHolder").FindControl("btnLast")).Enabled = true;
                }
            }
        }

        public static void GenerateControlForSearch(HtmlTableCell objHtmlTableCell, string strSelectedField)
        {
            System.Web.UI.WebControls.Label objLabel;
            System.Web.UI.WebControls.TextBox objTextBox;
            CheckBoxList objCheckBoxList;
            RequiredFieldValidator objRequiredFieldValidator;
            AjaxControlToolkit.FilteredTextBoxExtender objFilteredTextBoxExtender;
            AjaxControlToolkit.MaskedEditExtender objMaskedEditExtender;
            AjaxControlToolkit.MaskedEditValidator objMaskedEditValidator;
            CalendarExtender objCalendarExtender;
            DropDownList objDropDownList;
            CompareValidator objCompareValidator;
            PlaceHolder objPlaceHolder = new PlaceHolder();

            string[] strField = strSelectedField.Split('-');
            string strFieldID = strField[0].ToString();
            string strFieldDataType = strField[1].ToString();

            objTextBox = new System.Web.UI.WebControls.TextBox();

            if (strFieldDataType.Trim() != "4" && strFieldDataType.Trim() != "9")
            {
                objTextBox.ID = strFieldID;
                objPlaceHolder.Controls.Add(objTextBox);
            }
            else if (strFieldDataType.Trim() == "9")
            {
                System.Web.UI.WebControls.Panel objPanelList = new System.Web.UI.WebControls.Panel();
                objPanelList.ScrollBars = System.Web.UI.WebControls.ScrollBars.Auto;
                objPanelList.Height = Unit.Pixel(75);

                objCheckBoxList = new CheckBoxList();
                objCheckBoxList.ID = strFieldID;
                objCheckBoxList.Height = Unit.Pixel(50);

                DataTable objDataTable = new DataTable();
                Utility objUtility = new Utility();
                objUtility.Result = MetaTemplateItem.SelectByFieldID(out objDataTable, Convert.ToInt32(strFieldID));
                if (objUtility.Result != ResultType.Error)
                {
                    DataView objDataView = objDataTable.DefaultView;
                    objDataView.Sort = "ListItemText ASC";
                    objCheckBoxList.DataSource = objDataView;
                    objCheckBoxList.DataTextField = "ListItemText";
                    objCheckBoxList.DataValueField = "ID";
                    objCheckBoxList.DataBind();

                    if (objDataView.Count == 0)
                    {
                        objCheckBoxList.Items.Insert(0, new ListItem("No Data Available", "0"));
                    }
                }
                objPanelList.Controls.Add(objCheckBoxList);
                objPlaceHolder.Controls.Add(objPanelList);
            }
            else if (strFieldDataType.Trim() == "4")
            {
                objDropDownList = new DropDownList();
                objDropDownList.ID = strFieldID;

                DataTable objDataTable = new DataTable();
                Utility objUtility = new Utility();
                objUtility.Result = MetaTemplateItem.SelectByFieldID(out objDataTable, Convert.ToInt32(strFieldID));
                if (objUtility.Result != ResultType.Error)
                {
                    DataView objDataView = objDataTable.DefaultView;
                    objDataView.Sort = "ListItemText ASC";
                    objDropDownList.DataSource = objDataView;
                    objDropDownList.DataTextField = "ListItemText";
                    objDropDownList.DataValueField = "ID";
                    objDropDownList.DataBind();
                    if (objDataView.Count == 0)
                    {
                        objDropDownList.Items.Insert(0, new ListItem("No Data Available", "0"));
                    }

                }
                objPlaceHolder.Controls.Add(objDropDownList);

            }


            switch (strFieldDataType.Trim())
            {
                case "1":
                    objFilteredTextBoxExtender = new FilteredTextBoxExtender();
                    objFilteredTextBoxExtender.TargetControlID = objTextBox.UniqueID;
                    objFilteredTextBoxExtender.FilterType = FilterTypes.UppercaseLetters | FilterTypes.LowercaseLetters | FilterTypes.Custom;
                    objFilteredTextBoxExtender.ValidChars = " ";
                    objPlaceHolder.Controls.Add(objFilteredTextBoxExtender);
                    break;

                case "2":
                    objFilteredTextBoxExtender = new FilteredTextBoxExtender();
                    objFilteredTextBoxExtender.TargetControlID = objTextBox.UniqueID;
                    objFilteredTextBoxExtender.FilterType = FilterTypes.Numbers;
                    objPlaceHolder.Controls.Add(objFilteredTextBoxExtender);
                    break;

                case "3":
                    objCalendarExtender = new CalendarExtender();
                    objCalendarExtender.TargetControlID = objTextBox.UniqueID;
                    objCalendarExtender.Format = "MM/dd/yyyy";
                    objCalendarExtender.PopupButtonID = objTextBox.UniqueID;

                    objMaskedEditExtender = new MaskedEditExtender();
                    objMaskedEditExtender.ID = "MEE" + objTextBox.UniqueID;
                    objMaskedEditExtender.TargetControlID = objTextBox.UniqueID;
                    objMaskedEditExtender.Mask = "99/99/9999";
                    objMaskedEditExtender.MaskType = MaskedEditType.Date;

                    objMaskedEditValidator = new MaskedEditValidator();
                    objMaskedEditValidator.ControlExtender = objMaskedEditExtender.UniqueID;
                    objMaskedEditValidator.ControlToValidate = objTextBox.UniqueID;
                    objMaskedEditValidator.InvalidValueMessage = "Invalid Date (mm/dd/yyyy)";

                    objPlaceHolder.Controls.Add(objCalendarExtender);
                    objPlaceHolder.Controls.Add(objMaskedEditExtender);
                    objPlaceHolder.Controls.Add(objMaskedEditValidator);
                    break;

                case "10":
                    objMaskedEditExtender = new MaskedEditExtender();
                    objMaskedEditExtender.ID = "MEE" + objTextBox.UniqueID;
                    objMaskedEditExtender.TargetControlID = objTextBox.UniqueID;
                    objMaskedEditExtender.Mask = "99:99:99";
                    objMaskedEditExtender.MaskType = MaskedEditType.Time;
                    objMaskedEditExtender.AcceptAMPM = true;

                    objMaskedEditValidator = new MaskedEditValidator();
                    objMaskedEditValidator.ControlExtender = objMaskedEditExtender.UniqueID;
                    objMaskedEditValidator.ControlToValidate = objTextBox.UniqueID;
                    objMaskedEditValidator.InvalidValueMessage = "Invalid Date (hh:mm:ss AMPM)";

                    objPlaceHolder.Controls.Add(objMaskedEditExtender);
                    objPlaceHolder.Controls.Add(objMaskedEditValidator);
                    break;

                case "5":
                    objMaskedEditExtender = new MaskedEditExtender();
                    objMaskedEditExtender.ID = "MEE" + objTextBox.UniqueID;
                    objMaskedEditExtender.TargetControlID = objTextBox.UniqueID;
                    objMaskedEditExtender.Mask = "99/99/9999 99:99:99";
                    objMaskedEditExtender.MaskType = MaskedEditType.DateTime;
                    objMaskedEditExtender.AcceptAMPM = true;

                    objMaskedEditValidator = new MaskedEditValidator();
                    objMaskedEditValidator.ControlExtender = objMaskedEditExtender.UniqueID;
                    objMaskedEditValidator.ControlToValidate = objTextBox.UniqueID;
                    objMaskedEditValidator.InvalidValueMessage = "Invalid Date (mm/dd/yyyy hh:mm:ss AMPM)";

                    objPlaceHolder.Controls.Add(objMaskedEditExtender);
                    objPlaceHolder.Controls.Add(objMaskedEditValidator);
                    break;

                case "6":
                    objFilteredTextBoxExtender = new FilteredTextBoxExtender();
                    objFilteredTextBoxExtender.TargetControlID = objTextBox.UniqueID;
                    objFilteredTextBoxExtender.FilterType = FilterTypes.UppercaseLetters | FilterTypes.LowercaseLetters | FilterTypes.Custom | FilterTypes.Numbers;
                    objFilteredTextBoxExtender.ValidChars = " ";
                    objPlaceHolder.Controls.Add(objFilteredTextBoxExtender);
                    break;

                case "7":
                    objFilteredTextBoxExtender = new FilteredTextBoxExtender();
                    objFilteredTextBoxExtender.TargetControlID = objTextBox.UniqueID;
                    objFilteredTextBoxExtender.FilterType = FilterTypes.Custom | FilterTypes.Numbers;
                    objFilteredTextBoxExtender.ValidChars = ".";

                    objCompareValidator = new CompareValidator();
                    objCompareValidator.ControlToValidate = objTextBox.UniqueID;
                    objCompareValidator.Type = ValidationDataType.Double;
                    objCompareValidator.ErrorMessage = "Invalid Decimal Number";

                    objPlaceHolder.Controls.Add(objFilteredTextBoxExtender);
                    objPlaceHolder.Controls.Add(objCompareValidator);
                    break;

                case "8":
                    objFilteredTextBoxExtender = new FilteredTextBoxExtender();
                    objTextBox.TextMode = TextBoxMode.MultiLine;
                    objTextBox.Rows = 3;
                    objFilteredTextBoxExtender.TargetControlID = objTextBox.UniqueID;
                    objFilteredTextBoxExtender.FilterType = FilterTypes.UppercaseLetters | FilterTypes.LowercaseLetters | FilterTypes.Custom | FilterTypes.Numbers;
                    objFilteredTextBoxExtender.ValidChars = "., /*-+!@#$%&*()?;'{}[]";
                    objPlaceHolder.Controls.Add(objFilteredTextBoxExtender);
                    break;

            }
            objHtmlTableCell.Controls.Add(objPlaceHolder);

        }

        public static void DocumentNavigation(GridViewCommandEventArgs objGridViewCommandEventArgs, GridView objGridView, DataTable objDataTableDocument)
        {
            if (objGridViewCommandEventArgs.CommandName.ToLower() == "previous")
            {
                if (objGridView.PageIndex > 0)
                {
                    objGridView.PageIndex = objGridView.PageIndex - 1;
                    objGridView.DataSource = objDataTableDocument;
                    objGridView.DataBind();
                }
            }
            else if (objGridViewCommandEventArgs.CommandName.ToLower() == "next")
            {
                if (objGridView.PageIndex < objGridView.PageCount)
                {
                    objGridView.PageIndex = objGridView.PageIndex + 1;
                    objGridView.DataSource = objDataTableDocument;
                    objGridView.DataBind();
                }
            }
            else if (objGridViewCommandEventArgs.CommandName.ToLower() == "first")
            {
                objGridView.PageIndex = 0;
                objGridView.DataSource = objDataTableDocument;
                objGridView.DataBind();
            }
            else if (objGridViewCommandEventArgs.CommandName.ToLower() == "last")
            {
                objGridView.PageIndex = objGridView.PageCount - 1;
                objGridView.DataSource = objDataTableDocument;
                objGridView.DataBind();
            }
        }

        public static void FillFileByteInDocumentDataTable(DataTable objDataTableMetaDataDocument, DataTable objDataTableDocument)
        {
            foreach (DataRow objDataRowMetaDataDocument in objDataTableMetaDataDocument.Rows)
            {
                DataRow objDataRowNew = objDataTableDocument.NewRow();
                objDataRowNew["DocumentID"] = objDataRowMetaDataDocument["ID"].ToString();
                if (Utility.FileStorageType == StorageType.DatabaseSystem)
                {
                    byte[] byteFileByte = objDataRowMetaDataDocument["Image"] == DBNull.Value ? null : (byte[])objDataRowMetaDataDocument["Image"];
                    objDataRowNew["FileByte"] = (byte[])objDataRowMetaDataDocument["Image"];
                }
                objDataRowNew["FilePath"] = objDataRowMetaDataDocument["DocumentPath"].ToString();
                objDataRowNew["VerifyStatus"] = 0;
                objDataRowNew["IsCheck"] = 0;
                objDataTableDocument.Rows.Add(objDataRowNew);
            }
            objDataTableDocument.AcceptChanges();
        }

        public static void FillFileByteAndDataInDocumentDataTable(DataTable objDataTableMetaDataDocument, DataTable objDataTableDocument, DataTable objDataTableDocumentData, DataTable objDataTableField)
        {
            foreach (DataRow objDataRowMetaDataDocument in objDataTableMetaDataDocument.Rows)
            {
                DataRow objDataRowNew = objDataTableDocument.NewRow();
                objDataRowNew["DocumentID"] = objDataRowMetaDataDocument["ID"].ToString();

                if (Utility.FileStorageType == StorageType.DatabaseSystem)
                {
                    byte[] byteFileByte = objDataRowMetaDataDocument["Image"] == DBNull.Value ? null : (byte[])objDataRowMetaDataDocument["Image"];
                    objDataRowNew["FileByte"] = (byte[])objDataRowMetaDataDocument["Image"];
                }
                objDataRowNew["FilePath"] = objDataRowMetaDataDocument["DocumentPath"].ToString();
                objDataRowNew["VerifyStatus"] = 0;
                objDataRowNew["IsCheck"] = 0;

                DataRow[] objDataRowDocumentData = objDataTableDocumentData.Select("DocumentID='" + Convert.ToInt32(objDataRowMetaDataDocument["ID"]) + "'");

                int intDocumentID = Convert.ToInt32(objDataRowMetaDataDocument["ID"]);
                foreach (DataRow objDataRow in objDataTableField.Select("STATUS = 1"))
                {
                    int intFieldID = Convert.ToInt32(objDataRow["ID"]);
                    DataRow[] objDataRowCollection = objDataTableDocumentData.Select("DocumentID='" + intDocumentID + "' AND FieldID='" + intFieldID + "'");
                    if (objDataRowCollection.Length > 0)
                    {
                        objDataRowNew[objDataRow["ID"].ToString()] = objDataRowCollection[0]["FieldData"].ToString();
                    }
                }

                objDataTableDocument.Rows.Add(objDataRowNew);
            }
            objDataTableDocument.AcceptChanges();
        }

        public static void ExportToExcel(DataTable objDataTable, string strFileName)
        {
            try
            {
                HttpContext.Current.Response.ContentType = "application/octet-stream";
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=\"" + strFileName.Trim() + ".xls\"");
                System.Text.StringBuilder objStringBuilder = new System.Text.StringBuilder();
                objStringBuilder.Append("<table style='border:1px solid black;'>");
                objStringBuilder.Append("<tr>");
                foreach (DataColumn objDataColumn in objDataTable.Columns)
                {
                    objStringBuilder.Append("<th style='border:1px solid black;'>");
                    objStringBuilder.Append(objDataColumn.ColumnName);
                    objStringBuilder.Append("</th>");
                }
                objStringBuilder.Append("</tr>");


                foreach (DataRow objDataRow in objDataTable.Rows)
                {
                    objStringBuilder.Append("<tr>");
                    foreach (DataColumn objDataColumn in objDataTable.Columns)
                    {
                        objStringBuilder.Append("<td style='border:1px solid black;'>");
                        objStringBuilder.Append(objDataRow[objDataColumn.ColumnName].ToString() == string.Empty ? "N/A" : objDataRow[objDataColumn.ColumnName].ToString());
                        objStringBuilder.Append("</td>");
                    }
                    objStringBuilder.Append("</tr>");
                }
                objStringBuilder.Append("</table>");
                HttpContext.Current.Response.Write(objStringBuilder.ToString());
               // HttpContext.Current.Response.End();
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public static string SearchPageContent(string strDocumentData)
        {
            string MetaDataID = "";
            try
            {
                List<string> objDocumentID = new List<string>();
                List<string> objWordData = strDocumentData.ToLower().Split().Where(res => res.Trim() != string.Empty).ToList();

                foreach (string word in objWordData)
                {
                    Lucene.Net.Search.BooleanQuery bq = new Lucene.Net.Search.BooleanQuery();

                    Lucene.Net.Search.Query ql = new Lucene.Net.Search.TermQuery(new Lucene.Net.Index.Term("DocumentData", word.ToString()));

                    bq.Add(ql, Lucene.Net.Search.BooleanClause.Occur.SHOULD);

                    Lucene.Net.Search.IndexSearcher objSearcher = new Lucene.Net.Search.IndexSearcher(Utility.LuceneFilePath);

                    var oHitColl = objSearcher.Search(bq);
                    for (int i = 0; i < oHitColl.Length(); i++)
                    {
                        Lucene.Net.Documents.Document oDoc = oHitColl.Doc(i);
                        objDocumentID.Add(oDoc.Get("DocumentID"));
                    }
                    objSearcher.Close();
                }

                var result = from temp in objDocumentID
                             group temp by temp into g
                             select new { ID = g.Key, TotalCount = g.Count() };

                if (result.Count() > 0)
                {
                    int maxCount = result.Max(i => i.TotalCount);

                    foreach (var item in result.Where(i => i.TotalCount == maxCount))
                    {
                        MetaDataID = MetaDataID + item.ID + ",";
                    }
                }

                return MetaDataID.Trim(',');
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return "";
            }

        }

        public static Utility.ResultType ExportMetaTemplateDescription(int intMetaTemplateID)
        {
            try
            {
                DbConnection objDbConnection = Utility.GetConnection;
                objDbConnection.Open();
                char charQuote = '"';

                string strQuery = "SELECT ID,FieldName,"
                                + " (SELECT DataTypeName FROM vwDataType WHERE ID=vwMetaTemplateFields.FieldDataTypeID) AS " + charQuote + "DataType" + charQuote + ","
                                + " CASE  WHEN FieldTypeID =1 THEN 'Null' ELSE 'NotNull' END AS " + charQuote + "FieldType" + charQuote + ","
                                + " CASE WHEN IsPrimary=1 THEN 'Unique' ELSE 'NonUnique' END AS " + charQuote + "IsUnique" + charQuote + ","
                                + " FieldLength"
                                + " FROM vwMetaTemplateFields WHERE MetaTemplateID=@MetaTemplateID AND Status=1";

                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

                DbParameter[] objDbParameter = new DbParameter[1];
                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "MetaTemplateID";
                objDbParameter[0].Value = intMetaTemplateID;

                DataTable objDataTableField = DataHelper.ExecuteDataTable(strQuery, objDbParameter);

                if (objDataTableField.Rows.Count == 0)
                    return ResultType.Failure;

                strQuery = "SELECT ID,FieldID,ListItemText  FROM vwListItem WHERE FieldID "
                          + " IN (SELECT ID FROM vwMetaTemplateFields WHERE MetaTemplateID=@MetaTemplateID AND Status=1) AND Status=1 ORDER BY FieldID";

                objDbParameter = new DbParameter[1];
                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "MetaTemplateID";
                objDbParameter[0].Value = intMetaTemplateID;

                DataTable objDataTableListItem = DataHelper.ExecuteDataTable(strQuery, objDbParameter);
                if (objDataTableField.Select("DataType IN ('List','MultiList')").Length > 0)
                {
                    if (objDataTableListItem.Rows.Count == 0)
                        return ResultType.Failure;
                }

                objDbConnection.Close();

                ExportToExcelFormat(objDataTableField, objDataTableListItem, "MetaTemplateDescription");
                return ResultType.Success;
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return ResultType.Error;
            }

        }

        public static Utility.ResultType ExportMetaTemplateUploadPattern(int intMetaTemplateID)
        {
            try
            {
                DbConnection objDbConnection = Utility.GetConnection;
                objDbConnection.Open();
                char charQuote = '"';

                string strQuery = "SELECT ID,FieldName,"
                                + " (SELECT DataTypeName FROM vwDataType WHERE ID=vwMetaTemplateFields.FieldDataTypeID) AS " + charQuote + "DataType" + charQuote + ","
                                + " CASE  WHEN FieldTypeID =1 THEN 'Null' ELSE 'NotNull' END AS " + charQuote + "FieldType" + charQuote + ","
                                + " CASE WHEN IsPrimary=1 THEN 'Unique' ELSE 'NonUnique' END AS " + charQuote + "IsUnique" + charQuote + ","
                                + " FieldLength"
                                + " FROM vwMetaTemplateFields WHERE MetaTemplateID=@MetaTemplateID AND Status=1";

                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

                DbParameter[] objDbParameter = new DbParameter[1];
                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "MetaTemplateID";
                objDbParameter[0].Value = intMetaTemplateID;

                DataTable objDataTableField = DataHelper.ExecuteDataTable(strQuery, objDbParameter);

                if (objDataTableField.Rows.Count == 0)
                    return ResultType.Failure;

                objDbConnection.Close();

                DataTable objDataTableNew = new DataTable();
                foreach (DataRow objDataRow in objDataTableField.Rows)
                {
                    objDataTableNew.Columns.Add(objDataRow["FieldName"].ToString());
                }
                objDataTableNew.Columns.Add("Tag");
                objDataTableNew.AcceptChanges();


                ExportToExcelFormat(objDataTableNew, "MetaTemplateUploadPattern");
                return ResultType.Success;
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return ResultType.Error;
            }

        }

        public static void ExportToExcelFormat(DataTable objDataTable, DataTable objDataTableNew, string strFileName)
        {
            HttpContext.Current.Response.ContentType = "application/octet-stream";
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + strFileName.Trim() + ".xls");
            System.Text.StringBuilder objStringBuilder = new System.Text.StringBuilder();

            foreach (DataColumn objDataColumn in objDataTable.Columns)
            {
                objStringBuilder.Append(objDataColumn.ColumnName);
                objStringBuilder.Append("\t");
            }

            foreach (DataRow objDataRow in objDataTable.Rows)
            {
                objStringBuilder.Append("\n");
                foreach (DataColumn objDataColumn in objDataTable.Columns)
                {
                    objStringBuilder.Append(objDataRow[objDataColumn.ColumnName].ToString() == string.Empty ? "N/A" : objDataRow[objDataColumn.ColumnName].ToString());
                    objStringBuilder.Append("\t");
                }
            }


            objStringBuilder.Append("\n");
            objStringBuilder.Append("\n");
            objStringBuilder.Append("\n");
            objStringBuilder.Append("\n");
            objStringBuilder.Append("\n");

            foreach (DataColumn objDataColumn in objDataTableNew.Columns)
            {
                objStringBuilder.Append(objDataColumn.ColumnName);
                objStringBuilder.Append("\t");
            }

            foreach (DataRow objDataRow in objDataTableNew.Rows)
            {
                objStringBuilder.Append("\n");
                foreach (DataColumn objDataColumn in objDataTableNew.Columns)
                {
                    objStringBuilder.Append(objDataRow[objDataColumn.ColumnName].ToString() == string.Empty ? "N/A" : objDataRow[objDataColumn.ColumnName].ToString());
                    objStringBuilder.Append("\t");
                }
            }

            HttpContext.Current.Response.Write(objStringBuilder.ToString());
            HttpContext.Current.Response.End();
        }

        public static void ExportToExcelFormat(DataTable objDataTable, string strFileName)
        {
            HttpContext.Current.Response.ContentType = "application/octet-stream";
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + strFileName.Trim() + ".xls");
            System.Text.StringBuilder objStringBuilder = new System.Text.StringBuilder();

            foreach (DataColumn objDataColumn in objDataTable.Columns)
            {
                objStringBuilder.Append(objDataColumn.ColumnName);
                objStringBuilder.Append("\t");
            }

            foreach (DataRow objDataRow in objDataTable.Rows)
            {
                objStringBuilder.Append("\n");
                foreach (DataColumn objDataColumn in objDataTable.Columns)
                {
                    objStringBuilder.Append(objDataRow[objDataColumn.ColumnName].ToString() == string.Empty ? "N/A" : objDataRow[objDataColumn.ColumnName].ToString());
                    objStringBuilder.Append("\t");
                }
            }

            HttpContext.Current.Response.Write(objStringBuilder.ToString());
            HttpContext.Current.Response.End();
        }

        public static string GetAccessRight(AccessRight enumAccessRight)
        {
            if (HttpContext.Current.Session["AccessRights"] != null)
            {
                DataTable objDataTable = (DataTable)HttpContext.Current.Session["AccessRights"];
                string strResult = string.Empty;
                switch (enumAccessRight)
                {
                    case AccessRight.Repository:
                        foreach (DataRow objDataRow in objDataTable.Rows)
                        {
                            strResult = strResult + objDataRow["RepositoryID"] + ",";
                        }
                        break;

                    case AccessRight.MetaTemplate:
                        foreach (DataRow objDataRow in objDataTable.Rows)
                        {
                            strResult = strResult + objDataRow["MetaTemplateID"] + ",";
                        }
                        break;

                    case AccessRight.Category:
                        foreach (DataRow objDataRow in objDataTable.Rows)
                        {
                            strResult = strResult + objDataRow["CategoryID"] + ",";
                        }
                        break;

                    case AccessRight.Folder:
                        foreach (DataRow objDataRow in objDataTable.Rows)
                        {
                            strResult = strResult + objDataRow["FolderID"] + ",";
                        }
                        break;
                }
                strResult = strResult.Trim(',');
                if (strResult.Trim() == string.Empty)
                    return "0";
                else
                    return strResult;
            }
            return "0";
        }

        public static void InsertAccessRight(int intRepositoryID, int intMetaTemplateID, int intCategoryID, int intFolderID)
        {
            if (HttpContext.Current.Session["AccessRights"] != null)
            {
                ((DataTable)HttpContext.Current.Session["AccessRights"]).Rows.Add(null, null, intRepositoryID, intMetaTemplateID, intCategoryID, intFolderID);
            }
        }

        public static Bitmap MakeCaptchaImage(string txt, int width, int hight, string fontFamilyName)
        {
            Bitmap bm = new Bitmap(width, hight);
            Graphics gr = Graphics.FromImage(bm);
            gr.SmoothingMode = SmoothingMode.HighQuality;
            RectangleF recF = new RectangleF(0, 0, width, hight);
            Brush br;

            br = new HatchBrush(HatchStyle.SmallConfetti, Color.FromArgb(250, 243, 214), Color.FromArgb(254, 250, 239));
            gr.FillRectangle(br, recF);
            SizeF text_size;
            Font the_font;
            float font_size = hight + 7;
            do
            {
                font_size -= 1;
                the_font = new Font(fontFamilyName, font_size, FontStyle.Bold, GraphicsUnit.Pixel);

                text_size = gr.MeasureString(txt, the_font);
            }
            while ((text_size.Width == width) || (text_size.Height == hight));

            StringFormat string_format = new StringFormat();
            string_format.Alignment = StringAlignment.Center;
            string_format.LineAlignment = StringAlignment.Center;


            GraphicsPath graphics_path = new GraphicsPath();
            graphics_path.AddString(txt, the_font.FontFamily, 1, the_font.Size, recF, string_format);


            Random rnd = new Random();
            PointF[] pts = { new PointF((float)rnd.Next(width) / 4, (float)rnd.Next(hight) / 4), new PointF(width - (float)rnd.Next(width) / 4, (float)rnd.Next(hight) / 4), new PointF((float)rnd.Next(width) / 4, hight - (float)rnd.Next(hight) / 4), new PointF(width - (float)rnd.Next(width) / 4, hight - (float)rnd.Next(hight) / 4) };

            Matrix mat = new Matrix();
            graphics_path.Warp(pts, recF, mat, WarpMode.Perspective, 0);


            br = new HatchBrush(HatchStyle.LargeConfetti, Color.FromArgb(10, 150, 10), Color.FromArgb(10, 159, 10));
            gr.FillPath(br, graphics_path);


            int max_dimension = System.Math.Max(width, hight);
            for (int i = 0; i <= (int)width * hight / 30; i++)
            {
                int X = rnd.Next(width);
                int Y = rnd.Next(hight);
                int W = (int)rnd.Next(max_dimension) / 50;
                int H = (int)rnd.Next(max_dimension) / 50;
                gr.FillEllipse(br, X, Y, W, H);
            }

            for (int i = 1; i <= 5; i++)
            {
                int x1 = rnd.Next(width);
                int y1 = rnd.Next(hight);
                int x2 = rnd.Next(width);
                int y2 = rnd.Next(hight);
                gr.DrawLine(Pens.DarkGray, x1, y1, x2, y2);
            }
            for (int i = 1; i <= 5; i++)
            {
                int x1 = rnd.Next(width);
                int y1 = rnd.Next(hight);
                int x2 = rnd.Next(width);
                int y2 = rnd.Next(hight);
                gr.DrawLine(Pens.LightGray, x1, y1, x2, y2);
            }
            graphics_path.Dispose();
            br.Dispose();
            the_font.Dispose();
            gr.Dispose();
            return bm;
        }

        public static string Encrypt(string strEncrypt)
        {
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(strEncrypt);

            if (true)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes("123"));
            }
            else
                keyArray = UTF8Encoding.UTF8.GetBytes("123");

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        public static string Decrypt(string strDecrypt)
        {
            byte[] keyArray;
            byte[] toEncryptArray = Convert.FromBase64String(strDecrypt);

            if (true)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes("123"));
            }
            else
                keyArray = UTF8Encoding.UTF8.GetBytes("123");

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return UTF8Encoding.UTF8.GetString(resultArray);
        }

        public static string CalculateSHA256(string str)
        {
            SHA256 sha256 = SHA256Managed.Create();
            byte[] hashValue;
            UTF8Encoding objUtf8 = new UTF8Encoding();
            hashValue = sha256.ComputeHash(objUtf8.GetBytes(str));

            return BitConverter.ToString(hashValue);
        }

        public static string ComputeHash(string input, Byte[] salt)
        {
            SHA256 sha256 = SHA256Managed.Create();
            Byte[] inputBytes = Encoding.UTF8.GetBytes(input);

            // Combine salt and input bytes
            Byte[] saltedInput = new Byte[salt.Length + inputBytes.Length];
            salt.CopyTo(saltedInput, 0);
            inputBytes.CopyTo(saltedInput, salt.Length);

            Byte[] hashedBytes = sha256.ComputeHash(saltedInput);

            return BitConverter.ToString(hashedBytes);
        }

        public static string ComputeHash(string input, HashAlgorithm algorithm, Byte[] salt)
        {
            Byte[] inputBytes = Encoding.UTF8.GetBytes(input);

            // Combine salt and input bytes
            Byte[] saltedInput = new Byte[salt.Length + inputBytes.Length];
            salt.CopyTo(saltedInput, 0);
            inputBytes.CopyTo(saltedInput, salt.Length);

            Byte[] hashedBytes = algorithm.ComputeHash(saltedInput);

            return BitConverter.ToString(hashedBytes);
        }


        public static bool IsInternetExplorer(HttpContext Context)
        {
            return Context.Request.Browser.Browser == @"IE";
            throw new NotImplementedException();
        }
        public static string Trim(string strString, int intLength)
        {
            if (strString.Trim() == string.Empty)
            {
                return "N/A";
            }
            return strString.Substring(0, strString.Length > intLength ? intLength : strString.Length) + (strString.Length > intLength ? "..." : string.Empty);
        }
        public static void CreateDirectory(string strPath)
        {
            string[] strDirectories = (string[])strPath.Split('\\').Where(i => i.Trim() != string.Empty).ToArray();
            string strDirectoryPath = string.Empty;
            foreach (string strDirectory in strDirectories)
            {
                strDirectoryPath = strDirectoryPath + strDirectory + "\\";
                if (!Directory.Exists(strDirectoryPath))
                {
                    Directory.CreateDirectory(strDirectoryPath);
                }
            }
        }

        #region Seema 17 July 2017

        public static void LoadRoleIndepey(DropDownList objDropDownList)
        {
            try
            {
                string strQuery = "SELECT * FROM vwRole where id in (219,220,221)";
                DataTable objDataTable = DataHelper.ExecuteDataTable(strQuery, null);
                objDropDownList.DataSource = objDataTable;
                objDropDownList.DataTextField = "RoleName";
                objDropDownList.DataValueField = "ID";
                objDropDownList.DataBind();
                objDropDownList.Items.Insert(0, new ListItem("--SELECT--", "-1"));
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                objDropDownList.Items.Insert(0, new ListItem("--NONE--", "-1"));
            }
        }

        #endregion

        //Added by Vivek for Axis Trustee 11/03
        public static string DMSDocumentAxisTrusteePath
        {
            get
            {
                return ConfigurationSettings.AppSettings["DMSDocumentAxisTrustee"].ToString();
            }
        }
        //Added by Vivek for ftpFolderUpload 21/11/17
        public static string DMSDocumentGetFolderPath
        {
            get
            {
                return ConfigurationSettings.AppSettings["DMSDocumentFolderPath"].ToString();
            }
        }

        public static bool FolderHasChild(int FolderID, ref string ConcattedString) //added by Mayuresh
        {
            try
            {
                DbConnection objDbConnection = Utility.GetConnection;
                objDbConnection.Open();
                string strQuery = "select * from Folder where ParentFolderID = " + FolderID + " and ID in (" + GetAccessRight(AccessRight.Folder) + ")";
                DataTable objDataTableField = DataHelper.ExecuteDataTable(strQuery, null);
                objDbConnection.Close();
                if (objDataTableField.Rows.Count > 0)
                {
                    foreach (DataRow objDataRow in objDataTableField.Rows)
                    {
                        ConcattedString = ConcattedString + objDataRow["ID"] + ",";
                        if (FolderHasChild(Convert.ToInt32(objDataRow["ID"]), ref ConcattedString))
                        {
                            continue;
                        }
                    }
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                ConcattedString = string.Empty;
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return false;
            }

        }

        #endregion

        public static string GetIPAddress(HttpContext context)
        {
            string ipList = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipList))
            {
                return ipList.Split(',')[0];
            }

            return context.Request.ServerVariables["REMOTE_ADDR"];

        }

        public static string GetMacAddress()
        {
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                // Only consider Ethernet network interfaces
                if (nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet &&
                    nic.OperationalStatus == OperationalStatus.Up)
                {
                    return nic.GetPhysicalAddress().ToString();
                }
            }
            return null;
        }

        #region Get Drop down List Data
        public static void GetDropDownListData(out DataTable objDataTable, string Action,
            int UserId, int RepositoryId, int MetaTemplateId, int CategoryID, int FolderId)
        {
            try
            {
                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

                DbParameter[] objDbParameter = null;
                objDbParameter = new DbParameter[6];

                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "Action";
                objDbParameter[0].Value = Action;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "UserId";
                objDbParameter[1].Value = UserId;

                objDbParameter[2] = objDbProviderFactory.CreateParameter();
                objDbParameter[2].ParameterName = "RepositoryID";
                objDbParameter[2].Value = RepositoryId;

                objDbParameter[3] = objDbProviderFactory.CreateParameter();
                objDbParameter[3].ParameterName = "MetaTemplateID";
                objDbParameter[3].Value = MetaTemplateId;

                objDbParameter[4] = objDbProviderFactory.CreateParameter();
                objDbParameter[4].ParameterName = "CategoryID";
                objDbParameter[4].Value = CategoryID;

                objDbParameter[5] = objDbProviderFactory.CreateParameter();
                objDbParameter[5].ParameterName = "FolderID";
                objDbParameter[5].Value = FolderId;

                objDataTable = DataHelper.ExecuteDataTableForProcedure("GetDropdownListData", null, objDbParameter);
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                objDataTable = null;
            }
        }

        public static void LoadRepository(DropDownList objDropDownList, int UserID)
        {
            try
            {
                objDropDownList.Items.Clear();
                DataTable objDataTable = new DataTable();
                GetDropDownListData(out objDataTable, "GetRepository", UserID, 0, 0, 0, 0);

                objDropDownList.DataSource = objDataTable;
                objDropDownList.DataTextField = "RepositoryName";
                objDropDownList.DataValueField = "ID";
                objDropDownList.DataBind();
                objDropDownList.Items.Insert(0, new ListItem("--SELECT--", "-1"));
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        public static void LoadMetaTemplate(DropDownList objDropDownList, int UserID, int RepositoryId)
        {
            try
            {
                objDropDownList.Items.Clear();
                DataTable objDataTable = new DataTable();
                GetDropDownListData(out objDataTable, "GetMetaTemplate", UserID, RepositoryId, 0, 0, 0);

                objDropDownList.DataSource = objDataTable;
                objDropDownList.DataTextField = "MetaTemplateName";
                objDropDownList.DataValueField = "ID";
                objDropDownList.DataBind();
                objDropDownList.Items.Insert(0, new ListItem("--SELECT--", "-1"));
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        public static void LoadCategory(DropDownList objDropDownList, int UserID, int RepositoryId, int MetaTemplateID)
        {
            try
            {
                objDropDownList.Items.Clear();
                DataTable objDataTable = new DataTable();
                GetDropDownListData(out objDataTable, "GetCategory", UserID, RepositoryId, MetaTemplateID, 0, 0);

                objDropDownList.DataSource = objDataTable;
                objDropDownList.DataTextField = "CategoryName";
                objDropDownList.DataValueField = "ID";
                objDropDownList.DataBind();
                objDropDownList.Items.Insert(0, new ListItem("--SELECT--", "-1"));
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        public static void LoadFolder(System.Web.UI.WebControls.TreeView objTreeView, int UserID,
            int RepositoryID, int MetaTemplateID, int CategoryID)
        {
            try
            {
                objTreeView.Nodes.Clear();
                DataTable objDataTable = new DataTable();
                GetDropDownListData(out objDataTable, "GetFolder", UserID, RepositoryID, MetaTemplateID, CategoryID, 0);
                objTreeView.ExpandAll();
                System.Web.UI.WebControls.TreeNode objTreeNode = new System.Web.UI.WebControls.TreeNode("--NONE--", "0");
                objTreeNode.Selected = true;
                objTreeView.Nodes.Add(objTreeNode);

                DataView objDataView = new DataView(objDataTable);
                objDataView.RowFilter = "ParentFolderID = 0";
                objDataView.Sort = "FolderName";

                foreach (DataRowView objDataRowView in objDataView)
                {
                    System.Web.UI.WebControls.TreeNode objTreeNodeParent = new System.Web.UI.WebControls.TreeNode(objDataRowView["FolderName"].ToString(), objDataRowView["ID"].ToString());
                    objTreeNodeParent.ToolTip = objDataRowView["FolderDescription"].ToString();
                    objTreeNode.ChildNodes.Add(objTreeNodeParent);

                    AddFolderInTreeNode(objTreeNodeParent, Convert.ToInt32(objDataRowView["ID"]), objDataTable);
                }
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }
        #endregion

        public static List<String> DirSearch(string sDir, string Extension)
        {
            List<String> files = new List<String>();
            if (!string.IsNullOrEmpty(sDir))
            {
                try
                {
                    foreach (string f in Directory.GetFiles(sDir))
                    {
                        string ext = Path.GetExtension(f);
                        if (!string.IsNullOrEmpty(ext))
                        {
                            if (ext.ToLower() == Extension)
                                files.Add(f);
                        }
                    }
                    foreach (string d in Directory.GetDirectories(sDir))
                    {
                        files.AddRange(DirSearch(d, Extension));
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return files;
        }

        public static List<String> DirSearch_AllFiles(string sDir)
        {
            List<String> files = new List<String>();
            if (!string.IsNullOrEmpty(sDir))
            {
                try
                {
                    foreach (string f in Directory.GetFiles(sDir))
                    {
                        string ext = Path.GetExtension(f);
                        if (!string.IsNullOrEmpty(ext))
                        {
                            files.Add(f);
                        }
                    }
                    foreach (string d in Directory.GetDirectories(sDir))
                    {
                        files.AddRange(DirSearch_AllFiles(d));
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return files;
        }


        public static void GenerateControlAndFillData_Virescent(GridViewRowEventArgs objGridViewRowEventArgs, GridView objGridView, HiddenField objHiddenField, ref byte[] byteFileByte, DataTable objDataTableDocument, DataTable objDataTableField, DataTable objDataTableList, EnableOrDisable enumEnableOrDisable)
        {
            System.Web.UI.WebControls.Label objLabel;

            System.Web.UI.WebControls.TextBox objTextBox;
            CheckBoxList objCheckBoxList;
            RequiredFieldValidator objRequiredFieldValidator;
            AjaxControlToolkit.FilteredTextBoxExtender objFilteredTextBoxExtender;
            AjaxControlToolkit.MaskedEditExtender objMaskedEditExtender;
            AjaxControlToolkit.MaskedEditValidator objMaskedEditValidator;
            CalendarExtender objCalendarExtender;
            DropDownList objDropDownList;
            CompareValidator objCompareValidator;

            if (objGridViewRowEventArgs.Row.RowType == DataControlRowType.DataRow)
            {
                string strfilePath = DataBinder.Eval(objGridViewRowEventArgs.Row.DataItem, "FilePath").ToString();
                string strDocumentID = DataBinder.Eval(objGridViewRowEventArgs.Row.DataItem, "DocumentID").ToString();
                try
                {
                    if (Utility.FileStorageType == StorageType.DatabaseSystem)
                    {
                        if (Path.GetExtension(strfilePath).ToLower() == ".pdf")
                        {
                            byteFileByte = ConvertImageToPdfFromFileByte(DataBinder.Eval(objGridViewRowEventArgs.Row.DataItem, "FilePath").ToString(), (byte[])DataBinder.Eval(objGridViewRowEventArgs.Row.DataItem, "FileByte"));
                        }
                        else if (Path.GetExtension(strfilePath).ToLower() != ".pdf")
                        {
                            byteFileByte = ConvertImageToPdfFromFileByte(DataBinder.Eval(objGridViewRowEventArgs.Row.DataItem, "FilePath").ToString(), (byte[])DataBinder.Eval(objGridViewRowEventArgs.Row.DataItem, "FileByte"));
                            objDataTableDocument.Select("DocumentID='" + strDocumentID + "'")[0]["FileByte"] = byteFileByte;
                            objDataTableDocument.Select("DocumentID='" + strDocumentID + "'")[0]["FilePath"] = Path.ChangeExtension(strfilePath, ".pdf");
                            objDataTableDocument.AcceptChanges();
                        }
                    }
                    else if (Utility.FileStorageType == StorageType.FileSystem)
                    {
                        if (DataBinder.Eval(objGridViewRowEventArgs.Row.DataItem, "FileByte") != DBNull.Value)
                        {
                            byteFileByte = (byte[])DataBinder.Eval(objGridViewRowEventArgs.Row.DataItem, "FileByte");
                        }
                        else
                        {
                            byteFileByte = ConvertImageToPdfFromFileByte(DataBinder.Eval(objGridViewRowEventArgs.Row.DataItem, "FilePath").ToString(), null);

                            #region Temporary Code
                            //if (enumEnableOrDisable == EnableOrDisable.Enable)
                            //{
                            //    PdfReader objPdfReader = new PdfReader(byteFileByte);
                            //    if (objPdfReader.NumberOfPages > 1)
                            //    {
                            //        MemoryStream objMemoryStream = new MemoryStream();
                            //        using (iTextSharp.text.Document objDocument = new iTextSharp.text.Document())
                            //        {
                            //            using (PdfWriter w = PdfWriter.GetInstance(objDocument, objMemoryStream))
                            //            {
                            //                objDocument.Open();
                            //                int i=2;
                            //                while (i <= objPdfReader.NumberOfPages)
                            //                {
                            //                    objDocument.NewPage();
                            //                    w.DirectContent.AddTemplate(w.GetImportedPage(objPdfReader, i), 0, 0);
                            //                    i++;
                            //                }
                            //                objDocument.Close();
                            //            }
                            //        }
                            //        byteFileByte = null;
                            //        byteFileByte = objMemoryStream.ToArray();
                            //    }
                            //}
                            #endregion

                            objDataTableDocument.Select("DocumentID='" + strDocumentID + "'")[0]["FileByte"] = byteFileByte;
                            objDataTableDocument.AcceptChanges();
                        }

                    }

                    if (objHiddenField.Value == "1")
                    {
                        ((ShowPdf)objGridViewRowEventArgs.Row.FindControl("pdfViewer")).FilePath = "../Handler/ImageHandler1.ashx";
                        objHiddenField.Value = "2";
                    }
                    else if (objHiddenField.Value == "2")
                    {
                        ((ShowPdf)objGridViewRowEventArgs.Row.FindControl("pdfViewer")).FilePath = "../Handler/ImageHandler2.ashx";
                        objHiddenField.Value = "1";
                    }
                }
                catch (Exception ex)
                {
                    ((ShowPdf)objGridViewRowEventArgs.Row.FindControl("pdfViewer")).FilePath = "../Others/FileNotFound.pdf";
                    LogManager.ErrorLog(BusinessLogic.Utility.LogFilePath, ex);
                }


                DataRow[] objDataRowDocument = objDataTableDocument.Select("DocumentID='" + strDocumentID + "'");

                HtmlTable objHtmlTable = new HtmlTable();

                foreach (DataRow objDataRow in objDataTableField.Rows)
                {
                    HtmlTableRow objHtmlTableRow = new HtmlTableRow();
                    objHtmlTable.Rows.Add(objHtmlTableRow);

                    HtmlTableCell objHtmlTableCellLabel = new HtmlTableCell();
                    objLabel = new System.Web.UI.WebControls.Label();

                    objLabel.Text = objDataRow["FieldName"].ToString();

                    List<string> obj = new List<string>();
                    using (System.Drawing.Text.InstalledFontCollection col = new System.Drawing.Text.InstalledFontCollection())
                    {
                        foreach (FontFamily fa in col.Families)
                        {
                            obj.Add(fa.Name);
                        }
                    }

                    // objLabel.Font.Name = obj[106];
                    // objLabel.Font.Names[0] = obj[106];
                    objLabel.SkinID = "lblup";


                    objLabel.Width = Unit.Pixel(200);
                    objHtmlTableCellLabel.Controls.Add(objLabel);



                    HtmlTableCell objHtmlTableCellControl = new HtmlTableCell();
                    objTextBox = new System.Web.UI.WebControls.TextBox();

                    if (objDataRow["FieldDataTypeID"].ToString().Trim() != "4" && objDataRow["FieldDataTypeID"].ToString().Trim() != "9")
                    {
                        objTextBox.ID = objDataRow["ID"].ToString();

                        objTextBox.Text = objDataRowDocument[0][objDataRow["ID"].ToString()].ToString();
                        objTextBox.MaxLength = Convert.ToInt32(objDataRow["FieldLength"]);
                        //  objTextBox.Font.Name = "Kruti Dev 010";
                        //objTextBox.Font.Names[0] = "Kruti Dev 010";
                        objTextBox.SkinID = "txtengskin";
                        if (enumEnableOrDisable == EnableOrDisable.Disable)
                        {
                            objTextBox.Enabled = false;
                        }
                        objHtmlTableCellControl.Controls.Add(objTextBox);
                    }
                    else if (objDataRow["FieldDataTypeID"].ToString().Trim() == "9")
                    {
                        System.Web.UI.WebControls.Panel objPanelList = new System.Web.UI.WebControls.Panel();
                        objPanelList.ScrollBars = System.Web.UI.WebControls.ScrollBars.Auto;
                        objPanelList.Height = Unit.Pixel(75);

                        objCheckBoxList = new CheckBoxList();
                        objCheckBoxList.ID = objDataRow["ID"].ToString();
                        objCheckBoxList.Height = Unit.Pixel(50);

                        DataView objDataView = objDataTableList.DefaultView;
                        objDataView.RowFilter = "FieldID='" + objDataRow["ID"].ToString() + "'";
                        objDataView.Sort = "ListItemText ASC";
                        objCheckBoxList.DataSource = objDataView;
                        objCheckBoxList.DataTextField = "ListItemText";
                        objCheckBoxList.DataValueField = "ID";
                        objCheckBoxList.DataBind();

                        if (objDataView.Count == 0)
                        {
                            objCheckBoxList.Items.Insert(0, new ListItem("No Data Available", "0"));
                        }
                        if (objDataView.Count > 0)
                        {
                            string strSelectedItemValue = objDataRowDocument[0][objDataRow["ID"].ToString()].ToString();
                            string[] strSelectedItem = strSelectedItemValue.Split(',');
                            if (strSelectedItem.Length > 0)
                            {
                                foreach (ListItem objListItem in objCheckBoxList.Items)
                                {
                                    if (strSelectedItem.Contains(objListItem.Value))
                                    {
                                        objListItem.Selected = true;
                                    }
                                    else
                                    {
                                        objListItem.Selected = false;
                                    }
                                }
                            }
                        }
                        if (enumEnableOrDisable == EnableOrDisable.Disable)
                        {
                            objCheckBoxList.Enabled = false;
                        }
                        objPanelList.Controls.Add(objCheckBoxList);
                        objHtmlTableCellControl.Controls.Add(objPanelList);
                    }
                    else if (objDataRow["FieldDataTypeID"].ToString().Trim() == "4")
                    {
                        objDropDownList = new DropDownList();
                        objDropDownList.ID = objDataRow["ID"].ToString();

                        DataView objDataView = objDataTableList.DefaultView;
                        objDataView.RowFilter = "FieldID='" + objDataRow["ID"].ToString() + "'";
                        objDataView.Sort = "ListItemText ASC";
                        objDropDownList.DataSource = objDataView;
                        objDropDownList.DataTextField = "ListItemText";
                        objDropDownList.DataValueField = "ID";
                        objDropDownList.DataBind();
                        if (objDataView.Count == 0)
                        {
                            objDropDownList.Items.Insert(0, new ListItem("No Data Available", "0"));
                        }
                        objDropDownList.SelectedIndex = objDropDownList.Items.IndexOf(objDropDownList.Items.FindByValue(objDataRowDocument[0][objDataRow["ID"].ToString()].ToString()));

                        if (enumEnableOrDisable == EnableOrDisable.Disable)
                        {
                            objDropDownList.Enabled = false;
                        }
                        objHtmlTableCellControl.Controls.Add(objDropDownList);

                    }


                    HtmlTableCell objHtmlTableCellValidator = new HtmlTableCell();
                    if (objDataRow["FieldDataTypeID"].ToString().Trim() != "4" && objDataRow["FieldDataTypeID"].ToString().Trim() != "9")
                    {
                        if (objDataRow["FieldTypeID"].ToString().Trim() != "0")
                        {
                            objRequiredFieldValidator = new RequiredFieldValidator();
                            objRequiredFieldValidator.ControlToValidate = objTextBox.UniqueID;
                            objRequiredFieldValidator.Display = ValidatorDisplay.Static;
                            objRequiredFieldValidator.Text = "*";
                            // objHtmlTableCellValidator.Controls.Add(objRequiredFieldValidator);
                        }
                    }

                    HtmlTableCell objHtmlTableCellDataType = new HtmlTableCell();
                    switch (objDataRow["FieldDataTypeID"].ToString().Trim())
                    {
                        case "1":
                            objFilteredTextBoxExtender = new FilteredTextBoxExtender();
                            //objFilteredTextBoxExtender.TargetControlID = objTextBox.UniqueID;
                            objFilteredTextBoxExtender.FilterType = FilterTypes.UppercaseLetters | FilterTypes.LowercaseLetters | FilterTypes.Custom;
                            objFilteredTextBoxExtender.ValidChars = " ";
                            //objHtmlTableCellDataType.Controls.Add(objFilteredTextBoxExtender);
                            break;

                        case "2":
                            objFilteredTextBoxExtender = new FilteredTextBoxExtender();
                            //objFilteredTextBoxExtender.TargetControlID = objTextBox.UniqueID;
                            objFilteredTextBoxExtender.FilterType = FilterTypes.Numbers;
                            //objHtmlTableCellDataType.Controls.Add(objFilteredTextBoxExtender);
                            break;

                        case "3":
                            objCalendarExtender = new CalendarExtender();
                            //objCalendarExtender.TargetControlID = objTextBox.UniqueID;
                            objCalendarExtender.Format = "MM/dd/yyyy";
                            objCalendarExtender.PopupButtonID = objTextBox.UniqueID;

                            objMaskedEditExtender = new MaskedEditExtender();
                            objMaskedEditExtender.ID = "MEE" + objTextBox.UniqueID;
                            objMaskedEditExtender.TargetControlID = objTextBox.UniqueID;
                            objMaskedEditExtender.Mask = "99/99/9999";
                            objMaskedEditExtender.MaskType = MaskedEditType.Date;

                            objMaskedEditValidator = new MaskedEditValidator();
                            objMaskedEditValidator.ControlExtender = objMaskedEditExtender.UniqueID;
                            objMaskedEditValidator.ControlToValidate = objTextBox.UniqueID;
                            objMaskedEditValidator.InvalidValueMessage = "Invalid Date (mm/dd/yyyy)";

                            objHtmlTableCellDataType.Controls.Add(objCalendarExtender);
                            objHtmlTableCellDataType.Controls.Add(objMaskedEditExtender);
                            objHtmlTableCellDataType.Controls.Add(objMaskedEditValidator);
                            break;

                        case "10":
                            objMaskedEditExtender = new MaskedEditExtender();
                            objMaskedEditExtender.ID = "MEE" + objTextBox.UniqueID;
                            objMaskedEditExtender.TargetControlID = objTextBox.UniqueID;
                            objMaskedEditExtender.Mask = "99:99:99";
                            objMaskedEditExtender.MaskType = MaskedEditType.Time;
                            objMaskedEditExtender.AcceptAMPM = true;

                            objMaskedEditValidator = new MaskedEditValidator();
                            objMaskedEditValidator.ControlExtender = objMaskedEditExtender.UniqueID;
                            objMaskedEditValidator.ControlToValidate = objTextBox.UniqueID;
                            objMaskedEditValidator.InvalidValueMessage = "Invalid Date (hh:mm:ss AMPM)";

                            objHtmlTableCellDataType.Controls.Add(objMaskedEditExtender);
                            objHtmlTableCellDataType.Controls.Add(objMaskedEditValidator);
                            break;

                        case "5":
                            objMaskedEditExtender = new MaskedEditExtender();
                            objMaskedEditExtender.ID = "MEE" + objTextBox.UniqueID;
                            objMaskedEditExtender.TargetControlID = objTextBox.UniqueID;
                            objMaskedEditExtender.Mask = "99/99/9999 99:99:99";
                            objMaskedEditExtender.MaskType = MaskedEditType.DateTime;
                            objMaskedEditExtender.AcceptAMPM = true;

                            objMaskedEditValidator = new MaskedEditValidator();
                            objMaskedEditValidator.ControlExtender = objMaskedEditExtender.UniqueID;
                            objMaskedEditValidator.ControlToValidate = objTextBox.UniqueID;
                            objMaskedEditValidator.InvalidValueMessage = "Invalid Date (mm/dd/yyyy hh:mm:ss AMPM)";

                            objHtmlTableCellDataType.Controls.Add(objMaskedEditExtender);
                            objHtmlTableCellDataType.Controls.Add(objMaskedEditValidator);
                            break;

                        case "6":
                            objFilteredTextBoxExtender = new FilteredTextBoxExtender();
                            objFilteredTextBoxExtender.TargetControlID = objTextBox.UniqueID;
                            objFilteredTextBoxExtender.FilterType = FilterTypes.UppercaseLetters | FilterTypes.LowercaseLetters | FilterTypes.Custom | FilterTypes.Numbers;
                            objFilteredTextBoxExtender.ValidChars = " ";
                            // objHtmlTableCellDataType.Controls.Add(objFilteredTextBoxExtender);
                            break;

                        case "7":
                            objFilteredTextBoxExtender = new FilteredTextBoxExtender();
                            objFilteredTextBoxExtender.TargetControlID = objTextBox.UniqueID;
                            objFilteredTextBoxExtender.FilterType = FilterTypes.Custom | FilterTypes.Numbers;
                            objFilteredTextBoxExtender.ValidChars = ".";

                            objCompareValidator = new CompareValidator();
                            objCompareValidator.ControlToValidate = objTextBox.UniqueID;
                            objCompareValidator.Type = ValidationDataType.Double;
                            objCompareValidator.ErrorMessage = "Invalid Decimal Number";

                            objHtmlTableCellDataType.Controls.Add(objFilteredTextBoxExtender);
                            objHtmlTableCellDataType.Controls.Add(objCompareValidator);
                            break;

                        case "8":
                            objFilteredTextBoxExtender = new FilteredTextBoxExtender();
                            objTextBox.TextMode = TextBoxMode.MultiLine;
                            objTextBox.Rows = 3;
                            objFilteredTextBoxExtender.TargetControlID = objTextBox.UniqueID;
                            objFilteredTextBoxExtender.FilterType = FilterTypes.UppercaseLetters | FilterTypes.LowercaseLetters | FilterTypes.Custom | FilterTypes.Numbers;
                            objFilteredTextBoxExtender.ValidChars = "., /*-+!@#$%&*()?;'{}[]";
                            objHtmlTableCellDataType.Controls.Add(objFilteredTextBoxExtender);
                            break;

                    }


                    objHtmlTableRow.Cells.Add(objHtmlTableCellLabel);
                    objHtmlTableRow.Cells.Add(objHtmlTableCellControl);
                    objHtmlTableRow.Cells.Add(objHtmlTableCellValidator);
                    objHtmlTableRow.Cells.Add(objHtmlTableCellDataType);

                }

                System.Web.UI.WebControls.Panel objPanel = (System.Web.UI.WebControls.Panel)objGridViewRowEventArgs.Row.FindControl("pnlHolderMain").FindControl("pnlHolder");
                objPanel.Controls.Add(objHtmlTable);
            }
            if (objGridViewRowEventArgs.Row.RowType == DataControlRowType.DataRow)
            {
                if (objGridView.PageIndex == 0)
                {
                    ((ImageButton)objGridViewRowEventArgs.Row.FindControl("pnlHolderMain").FindControl("pnlHolder").FindControl("btnFirst")).Enabled = false;
                    ((ImageButton)objGridViewRowEventArgs.Row.FindControl("pnlHolderMain").FindControl("pnlHolder").FindControl("btnPrevious")).Enabled = false;
                }
                if (objGridView.PageIndex != 0)
                {
                    ((ImageButton)objGridViewRowEventArgs.Row.FindControl("pnlHolderMain").FindControl("pnlHolder").FindControl("btnFirst")).Enabled = true;
                    ((ImageButton)objGridViewRowEventArgs.Row.FindControl("pnlHolderMain").FindControl("pnlHolder").FindControl("btnPrevious")).Enabled = true;
                }
                if (objGridView.PageIndex == objGridView.PageCount - 1)
                {
                    ((ImageButton)objGridViewRowEventArgs.Row.FindControl("pnlHolderMain").FindControl("pnlHolder").FindControl("btnNext")).Enabled = false;
                    ((ImageButton)objGridViewRowEventArgs.Row.FindControl("pnlHolderMain").FindControl("pnlHolder").FindControl("btnLast")).Enabled = false;
                }
                if (objGridView.PageIndex != objGridView.PageCount - 1)
                {
                    ((ImageButton)objGridViewRowEventArgs.Row.FindControl("pnlHolderMain").FindControl("pnlHolder").FindControl("btnNext")).Enabled = true;
                    ((ImageButton)objGridViewRowEventArgs.Row.FindControl("pnlHolderMain").FindControl("pnlHolder").FindControl("btnLast")).Enabled = true;
                }
            }
        }

    }
}
