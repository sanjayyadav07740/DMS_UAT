using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.Odbc;
using DMS.BusinessLogic.Entity;

namespace DMS.BusinessLogic.Service
{
    public class DocumentForBulkUpload
    {
        Utility objUtility = new Utility();
        DataTable objDataTableUpload, objDataTableField, objDataTableListItem, objDataTableStorage;
        BusinessLogic.DocumentManager objDocumentManager = new BusinessLogic.DocumentManager();

        #region Method
        public Utility.ResultType GetAllDocumentToBeBulkUpload(out DataTable objDataTable)
        {
            try
            {
                string strQuery = "SELECT * FROM vwBulkUpload WHERE UploadStatus = 1 AND Status=1";
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

        private string ValidateData(string strValue, DataColumn objDataColumn)
        {
            if (objDataColumn.Caption.Trim().Split('=')[3].Trim() == "1" && strValue.Trim() == string.Empty)
            {
                throw new Exception("Can Not Insert Empty Value In " + objDataColumn.ColumnName);
            }

            if (strValue.Length > Convert.ToInt32(objDataColumn.Caption.Trim().Split('=')[2]))
            {
                throw new Exception("Invalid Length Of " + objDataColumn.ColumnName);
            }

            if (objDataColumn.Caption.Trim().Split('=')[4].Trim() == "1")
            {
                BusinessLogic.DocumentEntry objDocumentEntryExist = new BusinessLogic.DocumentEntry();
                
                objDocumentEntryExist.FieldID = Convert.ToInt32(objDataColumn.Caption.Trim().Split('=')[1].Trim());
                objDocumentEntryExist.FieldData = strValue.Trim().ToUpper();
                objUtility.Result = objDocumentManager.SelectDocumentData(objDocumentEntryExist);

                switch (objUtility.Result)
                {
                    case BusinessLogic.Utility.ResultType.Success:
                        throw new Exception("Value Of " + objDataColumn.ColumnName + " Field Is Already Exist .");
                }
            }

            switch (objDataColumn.Caption.Trim().Split('=')[0])
            {
                case "1"://STRING
                    return strValue.ToString().Trim();

                case "2"://NUMBER
                    return Convert.ToInt32(strValue).ToString();

                case "3"://DATE
                    return Convert.ToDateTime(strValue).ToString("MM/dd/yyyy");

                case "4"://LIST
                    string strExpression = string.Format("FieldID={0} AND ID={1}", objDataColumn.Caption.Trim().Split('=')[1], strValue.Trim() == string.Empty ? "0" : strValue.Trim());
                    if (objDataTableListItem.Select(strExpression).Length > 0)
                        return strValue.ToString().Trim();
                    throw new Exception(objDataColumn.ColumnName + " Value Of ListItem Does Not Exist.");
                    break;

                case "5"://DATETIME
                    return Convert.ToDateTime(strValue).ToString("MM/dd/yyyy hh:mm:ss tt");

                case "6"://VARCHAR
                    return strValue.ToString().Trim();

                case "7"://DECIMAL
                    return Convert.ToDecimal(strValue).ToString();

                case "8"://MULTILINE
                    return strValue.ToString().Trim();

                case "9"://MULTILIST
                    bool boolFlag = true;
                    strValue = strValue.Trim(',');
                    string[] strSelectedListItem = strValue.Split(',');
                    Array.Sort(strSelectedListItem);
                    if (strSelectedListItem.Length == 0)
                        return strValue;
                    else
                    {

                        foreach (string strItem in strSelectedListItem)
                        {
                            string strExpressionItem = string.Format("FieldID={0} AND ID={1}", objDataColumn.Caption.Trim().Split('=')[1], strItem.Trim());
                            if (objDataTableListItem.Select(strExpressionItem).Length == 0)
                            {
                                boolFlag = false;
                                break;
                            }
                        }
                    }
                    if (boolFlag == true)
                    {
                        strValue = string.Empty;
                        foreach (string strItem in strSelectedListItem)
                        {
                            strValue = strValue + strItem + ",";
                        }
                        strValue = strValue.Trim(',');
                        return strValue;
                    }

                    if (boolFlag == false)
                        throw new Exception(objDataColumn.ColumnName + " Value Of ListItem Does Not Exist.");
                    break;

                case "10"://TIME
                    return Convert.ToDateTime(strValue).ToString("hh:mm:ss tt");
            }
            return strValue.Trim();
        }

        private void UpdateBulkUploadAndGenerateTextFile(string strMessage, int intBulkUploadID, int intUploadStatus)
        {
            string strFilePath = Utility.SuccessErrorFilePath + "" + Guid.NewGuid() + ".txt";

            StreamWriter objStreamWriter = new StreamWriter(strFilePath, true);
            objStreamWriter.WriteLine(strMessage);
            objStreamWriter.Close();

            objUtility.Result = new DocumentManager().UpdateBulkUpload(new Entity.BulkUpload() { MetaDataCode = "N/A", DownloadPath = strFilePath, UploadStatus = intUploadStatus, BulkUploadID = intBulkUploadID });
        }

        private void UpdateBulkUploadAndGenerateExcelFile(DataTable objDataTable, int intBulkUploadID, int intUploadStatus, string strMetaDataCode)
        {
            string strFilePath = Utility.SuccessErrorFilePath + "" + Guid.NewGuid() + ".xls";

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

            StreamWriter objStreamWriter = new StreamWriter(strFilePath, true);
            objStreamWriter.WriteLine(objStringBuilder.ToString());
            objStreamWriter.Close();

            objUtility.Result = new DocumentManager().UpdateBulkUpload(new Entity.BulkUpload() { MetaDataCode = strMetaDataCode, DownloadPath = strFilePath, UploadStatus = intUploadStatus, BulkUploadID = intBulkUploadID });
        }

        public Utility.ResultType StartBulkUpload()
        {
            try
            {
                DataTable objDataTable;

                objUtility.Result = GetAllDocumentToBeBulkUpload(out objDataTable);
                if (objUtility.Result == Utility.ResultType.Error)
                {
                    return Utility.ResultType.Error;
                }

                foreach (DataRow objDataRow in objDataTable.Rows)
                {
                    objDataTableUpload = null; objDataTableField = null; objDataTableListItem = null; objDataTableStorage = null;
                    objDataTableUpload = new DataTable(); objDataTableField = new DataTable(); objDataTableListItem = new DataTable(); objDataTableStorage = new DataTable();
                    string strErrorFileText = string.Empty;
                    if (File.Exists(objDataRow["DocumentPath"].ToString().Trim()))
                    {
                        #region Open Excel File
                        try
                        {
                            OdbcConnection objOdbcConnection = new OdbcConnection(string.Format(@"Dsn=Excel Files;dbq={0};defaultdir={1};driverid=1046;maxbuffersize=2048;pagetimeout=5;uid=abc", objDataRow["DocumentPath"].ToString().Trim(), Utility.BulkFilePath));
                            objOdbcConnection.Open();
                            DataTable objDataTableSheet = objOdbcConnection.GetSchema("Tables");
                            if (objDataTableSheet.Rows.Count == 0)
                            {
                                UpdateBulkUploadAndGenerateTextFile("No Sheet Is Available.", int.Parse(objDataRow["ID"].ToString()), (int)BulkUpload.UploadStatusType.Error);
                                continue;
                            }

                            OdbcCommand objOdbcCommand = new OdbcCommand(string.Format("SELECT * FROM [{0}]", objDataTableSheet.Rows[0]["Table_Name"].ToString()), objOdbcConnection);

                            objDataTableUpload.Load(objOdbcCommand.ExecuteReader());

                            objOdbcConnection.Close();

                            foreach (DataColumn objDataColumn in objDataTableUpload.Columns)
                            {
                                objDataColumn.ColumnName = objDataColumn.ColumnName.Trim().ToUpper();
                            }
                            objDataTableUpload.Columns.Add("Success-Error");
                            objDataTableUpload.Columns.Add("Remark-For-Record");
                            objDataTableUpload.AcceptChanges();
                        }
                        catch (Exception ex)
                        {
                            strErrorFileText = ex.Message + " => " + objDataRow["DocumentPath"].ToString() + "=>" + objDataRow["BulkUploadCode"].ToString();
                            UpdateBulkUploadAndGenerateTextFile(strErrorFileText, int.Parse(objDataRow["ID"].ToString()), (int)BulkUpload.UploadStatusType.Error);
                            LogManager.ErrorLog(Utility.LogFilePath, ex);
                            continue;
                        }
                        #endregion

                        #region LoadMetaTemplateField
                        MetaTemplateManager objMetaTemplateManager = new MetaTemplateManager();
                        objUtility.Result = objMetaTemplateManager.SelectMetaTemplateFeildsByMetaTemplateID(out objDataTableField, Convert.ToInt32(objDataRow["MetaTemplateID"]));

                        if (objUtility.Result == Utility.ResultType.Error)
                        {
                            strErrorFileText = "Error In MetaTemplate Field Selection => " + objDataRow["DocumentPath"].ToString() + "=>" + objDataRow["BulkUploadCode"].ToString();
                            UpdateBulkUploadAndGenerateTextFile(strErrorFileText, int.Parse(objDataRow["ID"].ToString()), (int)BulkUpload.UploadStatusType.Error);
                            continue;
                        }
                        objDataTableListItem = null;
                        objUtility.Result = objMetaTemplateManager.SelectListItem(out objDataTableListItem, Convert.ToInt32(objDataRow["MetaTemplateID"]));
                        if (objUtility.Result == Utility.ResultType.Error)
                        {
                            strErrorFileText = "Error In MetaTemplate ListItem Selection => " + objDataRow["DocumentPath"].ToString() + "=>" + objDataRow["BulkUploadCode"].ToString();
                            UpdateBulkUploadAndGenerateTextFile(strErrorFileText, int.Parse(objDataRow["ID"].ToString()), (int)BulkUpload.UploadStatusType.Error);
                            continue;
                        }
                        #endregion

                        #region Check Column Name In Excel File
                        bool boolFlag = true;
                        foreach (DataRow objDataRowField in objDataTableField.Rows)
                        {
                            if (!objDataTableUpload.Columns.Contains(objDataRowField["FieldName"].ToString().Trim().ToUpper()))
                            {
                                boolFlag = false;
                            }
                        }
                        if (boolFlag == false)
                        {
                            strErrorFileText = "Column Mismatch => " + objDataRow["DocumentPath"].ToString() + "=>" + objDataRow["BulkUploadCode"].ToString();
                            UpdateBulkUploadAndGenerateTextFile(strErrorFileText, int.Parse(objDataRow["ID"].ToString()), (int)BulkUpload.UploadStatusType.Error);
                            continue;
                        }
                        #endregion

                        #region Assign The Details Of Column In DataTable
                        foreach (DataRow objDataRowField in objDataTableField.Rows)
                        {
                            DataColumn objDataColumn = objDataTableUpload.Columns[objDataRowField["FieldName"].ToString().Trim().ToUpper()];
                            objDataColumn.Caption = objDataRowField["FieldDataTypeID"].ToString().Trim() + "=" + objDataRowField["ID"].ToString().Trim() + "=" + objDataRowField["FieldLength"].ToString().Trim() + "=" + objDataRowField["FieldTypeID"].ToString().Trim() + "=" + objDataRowField["IsPrimary"].ToString().Trim();
                        }
                        #endregion

                        #region Filling The Data In Database From Uploaded Excel
                        try
                        {
                            foreach (DataRow objDataRowUpload in objDataTableUpload.Rows)
                            {
                                boolFlag = true;
                                foreach (DataColumn objDataColumn in objDataTableUpload.Columns)
                                {
                                    try
                                    {
                                        if (objDataColumn.ColumnName.ToLower() != "remark-for-record" && objDataColumn.ColumnName.ToLower() != "tag" && objDataColumn.ColumnName.ToLower() != "success-error")
                                            ValidateData(objDataRowUpload[objDataColumn.ColumnName].ToString(), objDataTableUpload.Columns[objDataColumn.ColumnName]);
                                    }
                                    catch (Exception ex)
                                    {
                                        objDataRowUpload["Remark-For-Record"] = objDataRowUpload["Remark-For-Record"] + "," + ex.Message;
                                        objDataRowUpload["Remark-For-Record"] = objDataRowUpload["Remark-For-Record"].ToString().Trim(',');

                                        boolFlag = false;
                                    }
                                }
                                if (boolFlag == false)
                                {
                                    objDataRowUpload["Success-Error"] = "ERROR";
                                    objDataTableUpload.AcceptChanges();
                                }
                                else if (boolFlag == true)
                                {
                                    objDataRowUpload["Success-Error"] = "SUCCESS";

                                    DbTransaction objDbTransaction = Utility.GetTransaction;
                                    DMS.BusinessLogic.Document objDocument = new BusinessLogic.Document();
                                    BusinessLogic.DocumentManager objDocumentManager = new BusinessLogic.DocumentManager();

                                    objDocument.MetaDataID = 0;
                                    objDocument.DocumentName = "N/A";
                                    objDocument.DocumentGuid = "N/A";
                                    objDocument.DocumentPath = "N/A";
                                    objDocument.DocumentType = "N/A";
                                    objDocument.Size = 0;
                                    objDocument.Image = new byte[0];
                                    objDocument.Tag = objDataRowUpload["Tag"].ToString();
                                    objDocument.IsLucened = 4;
                                    objDocument.CreatedBy = 1;
                                    objDocument.DocumentStatusID = 5;
                                    objDocument.Status = (int)Utility.Status.Active;

                                    bool boolDocumentEntry = true;
                                    objUtility.Result = objDocumentManager.InsertDocument(objDocument, objDbTransaction);
                                    if (objUtility.Result == Utility.ResultType.Error)
                                    {
                                        objDbTransaction.Rollback();
                                        boolDocumentEntry = false;
                                    }

                                    objDataRowUpload["Remark-For-Record"] = objDocument.DocumentID.ToString();

                                    BusinessLogic.DocumentEntry objDocumentEntry = new BusinessLogic.DocumentEntry();
                                    objDocumentEntry.DocumentID = objDocument.DocumentID;

                                    foreach (DataRow objDataRowField in objDataTableField.Rows)
                                    {
                                        objDocumentEntry.FieldID = Convert.ToInt32(objDataRowField["ID"]);
                                        objDocumentEntry.FieldData = objDataRowUpload[objDataRowField["FieldName"].ToString().Trim().ToUpper()].ToString();

                                        objUtility.Result = objDocumentManager.InsertDocumentEntry(objDocumentEntry, objDbTransaction);
                                        if (objUtility.Result == Utility.ResultType.Error)
                                        {
                                            objDbTransaction.Rollback();
                                            boolDocumentEntry = false;
                                        }
                                    }
                                    if (boolDocumentEntry)
                                    {
                                        objDbTransaction.Commit();
                                    }

                                    objDataTableUpload.AcceptChanges();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            strErrorFileText = ex.Message + " => " + objDataRow["DocumentPath"].ToString() + "=>" + objDataRow["BulkUploadCode"].ToString();
                            UpdateBulkUploadAndGenerateTextFile(strErrorFileText, int.Parse(objDataRow["ID"].ToString()), (int)BulkUpload.UploadStatusType.Error);
                            LogManager.ErrorLog(Utility.LogFilePath, ex);
                            continue;
                        }

                        try
                        {
                            if (objDataTableUpload.Select("[Success-Error]='SUCCESS'").Length > 0)
                            {
                                DbTransaction objDbTransaction = Utility.GetTransaction;
                                DMS.BusinessLogic.MetaData objMetaData = new BusinessLogic.MetaData();

                                objMetaData.MetaDataCode = "DMS-000001234";
                                objMetaData.RepositoryID = Convert.ToInt32(objDataRow["RepositoryID"]);
                                objMetaData.MetaTemplateID = Convert.ToInt32(objDataRow["MetaTemplateID"]);
                                objMetaData.FolderID = Convert.ToInt32(objDataRow["FolderID"]);
                                objMetaData.CategoryID = Convert.ToInt32(objDataRow["CategoryID"]);
                                objMetaData.CreatedBy = 1;
                                objMetaData.DocumentStatusID = 1;
                                objMetaData.Status = (int)Utility.Status.Active;

                                DocumentManager objDocumentManager = new DocumentManager();
                                objUtility.Result = objDocumentManager.InsertMetaData(objMetaData, objDbTransaction);

                                switch (objUtility.Result)
                                {
                                    case Utility.ResultType.Success:
                                        {
                                            foreach (DataRow objDataRowSuccess in objDataTableUpload.Select("[Success-Error]='SUCCESS'"))
                                            {
                                                objDocumentManager.UpdateDocumentForBulkUploadInsertMetaDataID(new Document() { MetaDataID = objMetaData.MetaDataID, DocumentID = Convert.ToInt32(objDataRowSuccess["Remark-For-Record"]) }, objDbTransaction);
                                            }
                                            objDbTransaction.Commit();
                                        }
                                        break;

                                    default:
                                        objDbTransaction.Rollback();
                                        break;
                                }

                                UpdateBulkUploadAndGenerateExcelFile(objDataTableUpload, int.Parse(objDataRow["ID"].ToString()), (int)BulkUpload.UploadStatusType.Success, objMetaData.MetaDataCode);
                                continue;
                            }
                            UpdateBulkUploadAndGenerateExcelFile(objDataTableUpload, int.Parse(objDataRow["ID"].ToString()), (int)BulkUpload.UploadStatusType.Success, "UNSUCCESSFUL");
                        }
                        catch (Exception ex)
                        {
                            strErrorFileText = ex.Message + " => " + objDataRow["DocumentPath"].ToString() + "=>" + objDataRow["BulkUploadCode"].ToString();
                            UpdateBulkUploadAndGenerateTextFile(strErrorFileText, int.Parse(objDataRow["ID"].ToString()), (int)BulkUpload.UploadStatusType.Error);
                            LogManager.ErrorLog(Utility.LogFilePath, ex);
                            continue;
                        }
                        #endregion
                       
                    }
                    else
                    {
                        strErrorFileText = "File Not Found " + objDataRow["DocumentPath"].ToString() + "=>" + objDataRow["BulkUploadCode"].ToString();
                        UpdateBulkUploadAndGenerateTextFile(strErrorFileText, int.Parse(objDataRow["ID"].ToString()), (int)BulkUpload.UploadStatusType.Error);
                        continue;
                    }
                }

                return Utility.ResultType.Success;
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return Utility.ResultType.Error;
            }
        }       
        
        #endregion
    }
}
