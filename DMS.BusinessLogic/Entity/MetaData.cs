using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace DMS.BusinessLogic
{
    public class MetaData
    {
        #region Enum
        public enum SearchType { EqualTo = 101, Like = 102, DateRange = 102, Exact_word = 4, Contains = 1, Begins_with =2 , Ends_with= 3, Any_of_these_words = 5 };
        public enum DashBoard { RepositoryLevel, MetaTemplateLevel, MetaDataLevel, DocumentStatusLevel }
        #endregion

        #region Properties
        public int MetaDataID { get; set; }
        public string MetaDataCode { get; set; }
        public int RepositoryID { get; set; }
        public int MetaTemplateID { get; set; }
        public int FolderID { get; set; }
        public int CategoryID { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public int UpdatedBy { get; set; }
        public int DocumentStatusID { get; set; }
        public int Status { get; set; }
        public int NewPageIndex { get; set; }
        public int IsRecursive { get; set; }
        #endregion

        #region Method

        public static Utility.ResultType Insert(MetaData objMetaData, DbTransaction objDbTransaction)
        {
            DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

            try
            {

                DbParameter[] objDbParameter = new DbParameter[13];

                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "MetaDataCode";
                objDbParameter[0].Value = objMetaData.MetaDataCode;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "RepositoryID";
                objDbParameter[1].Value = objMetaData.RepositoryID;

                objDbParameter[2] = objDbProviderFactory.CreateParameter();
                objDbParameter[2].ParameterName = "MetaTemplateID";
                objDbParameter[2].Value = objMetaData.MetaTemplateID;

                objDbParameter[3] = objDbProviderFactory.CreateParameter();
                objDbParameter[3].ParameterName = "FolderID";
                objDbParameter[3].Value = objMetaData.FolderID;

                objDbParameter[4] = objDbProviderFactory.CreateParameter();
                objDbParameter[4].ParameterName = "CategoryID";
                objDbParameter[4].Value = objMetaData.CategoryID;

                objDbParameter[5] = objDbProviderFactory.CreateParameter();
                objDbParameter[5].ParameterName = "CreatedOn";
                objDbParameter[5].Value = DateTime.Now;

                objDbParameter[6] = objDbProviderFactory.CreateParameter();
                objDbParameter[6].ParameterName = "CreatedBy";
                objDbParameter[6].Value = objMetaData.CreatedBy;

                objDbParameter[7] = objDbProviderFactory.CreateParameter();
                objDbParameter[7].ParameterName = "UpdatedOn";
                objDbParameter[7].Value = DBNull.Value;

                objDbParameter[8] = objDbProviderFactory.CreateParameter();
                objDbParameter[8].ParameterName = "UpdatedBy";
                objDbParameter[8].Value = DBNull.Value;

                objDbParameter[9] = objDbProviderFactory.CreateParameter();
                objDbParameter[9].ParameterName = "DocumentStatusID";
                objDbParameter[9].Value = 1;

                objDbParameter[10] = objDbProviderFactory.CreateParameter();
                objDbParameter[10].ParameterName = "Status";
                objDbParameter[10].Value = objMetaData.Status;

                objDbParameter[11] = objDbProviderFactory.CreateParameter();
                objDbParameter[11].ParameterName = "MetaDataID";
                objDbParameter[11].Size = 100;
                objDbParameter[11].Direction = ParameterDirection.Output;

                objDbParameter[12] = objDbProviderFactory.CreateParameter();
                objDbParameter[12].ParameterName = "MetaDataCodeToDisplay";
                objDbParameter[12].Size = 50;
                objDbParameter[12].Direction = ParameterDirection.Output;

                DataHelper.ExecuteNonQuery("SP_I_METADATA", objDbTransaction, objDbParameter);

                objMetaData.MetaDataID = Convert.ToInt32(objDbParameter[11].Value);
                objMetaData.MetaDataCode = objDbParameter[12].Value.ToString();

                return Utility.ResultType.Success;
            }
            catch (Exception ex)
            {

                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return Utility.ResultType.Error;
            }
        }

        public static Utility.ResultType Select(out DataTable objDataTable, int intMetaDataID)
        {
            try
            {
                string strQuery = "SELECT * FROM METADATA WHERE ID = @ID AND Status=1";

                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

                DbParameter[] objDbParameter = new DbParameter[1];
                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "ID";
                objDbParameter[0].Value = intMetaDataID;

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


        public static Utility.ResultType SelectForGrid(out DataTable objDataTable, DocumentManager.Status enumStatus, MetaData objMetaData)
        {
            try
            {
                string strQuery = string.Empty;
                char charQuote = '"';
                if (enumStatus == DocumentManager.Status.Uploaded)
                {
                    strQuery = @"SELECT MAX(DOCUMENT.ID)  AS DOCID,METADATA.ID,METADATA.MetaDataCode,count(DOCUMENT.DocumentStatusID) AS " + charQuote + "Uploaded Document Count" + charQuote +
                                " FROM METADATA INNER JOIN DOCUMENT ON METADATA.ID = DOCUMENT.MetaDataID " +
                                " WHERE DOCUMENT.Status=1 AND METADATA.RepositoryID = @RepositoryID AND METADATA.MetaTemplateID = @MetaTemplateID " +
                                " AND METADATA.CategoryID = @CategoryID AND METADATA.FolderID = @FolderID " +
                                " GROUP BY METADATA.MetaDataCode,DOCUMENT.DocumentStatusID,METADATA.ID HAVING DOCUMENT.DocumentStatusID=3 " +
                                " ORDER BY METADATA.ID DESC";
                }
                else if (enumStatus == DocumentManager.Status.EntryCompleted)
                {
                    strQuery = @"SELECT MAX(DOCUMENT.ID)  AS DOCID,METADATA.ID,METADATA.MetaDataCode,count(DOCUMENT.DocumentStatusID) AS " + charQuote + "Entry Completed Count" + charQuote +
                                " FROM METADATA INNER JOIN DOCUMENT ON METADATA.ID = DOCUMENT.MetaDataID " +
                                " WHERE DOCUMENT.Status=1 AND METADATA.RepositoryID = @RepositoryID AND METADATA.MetaTemplateID = @MetaTemplateID " +
                                " AND METADATA.CategoryID = @CategoryID AND METADATA.FolderID = @FolderID " +
                                " GROUP BY METADATA.MetaDataCode,DOCUMENT.DocumentStatusID,METADATA.ID HAVING DOCUMENT.DocumentStatusID=4 " +
                                " ORDER BY METADATA.ID DESC";
                }
                else if (enumStatus == DocumentManager.Status.Approved)
                {
                    strQuery = @"SELECT MAX(DOCUMENT.ID)  AS DOCID,METADATA.ID,METADATA.MetaDataCode,count(DOCUMENT.DocumentStatusID) AS " + charQuote + "Approved Document Count" + charQuote +
                                " FROM METADATA INNER JOIN DOCUMENT ON METADATA.ID = DOCUMENT.MetaDataID " +
                                " WHERE DOCUMENT.Status=1 AND METADATA.RepositoryID = @RepositoryID AND METADATA.MetaTemplateID = @MetaTemplateID " +
                                " AND METADATA.CategoryID = @CategoryID AND METADATA.FolderID = @FolderID " +
                                " GROUP BY METADATA.MetaDataCode,DOCUMENT.DocumentStatusID,METADATA.ID HAVING DOCUMENT.DocumentStatusID=1 " +
                                " ORDER BY METADATA.ID DESC";

                }
                else if (enumStatus == DocumentManager.Status.Rejected)
                {
                    strQuery = @"SELECT MAX(DOCUMENT.ID)  AS DOCID,METADATA.ID,METADATA.MetaDataCode,count(DOCUMENT.DocumentStatusID) AS " + charQuote + "Rejected Document Count" + charQuote +
                                " FROM METADATA INNER JOIN DOCUMENT ON METADATA.ID = DOCUMENT.MetaDataID " +
                                " WHERE DOCUMENT.Status=1 AND METADATA.RepositoryID = @RepositoryID AND METADATA.MetaTemplateID = @MetaTemplateID " +
                                " AND METADATA.CategoryID = @CategoryID AND METADATA.FolderID = @FolderID " +
                                " GROUP BY METADATA.MetaDataCode,DOCUMENT.DocumentStatusID,METADATA.ID HAVING DOCUMENT.DocumentStatusID=2 " +
                                " ORDER BY METADATA.ID DESC";
                }
                else if (enumStatus == DocumentManager.Status.TotalUploaded)
                {
                    strQuery = "SELECT METADATA.ID,METADATA.MetaDataCode," + charQuote + "Total Document" + charQuote + "," + charQuote + "Approved Document" + charQuote + "," + charQuote + "Rejected Document" + charQuote + "," + charQuote + "Pending For Entry" + charQuote + "," + charQuote + "Entry Completed" + charQuote + "," + charQuote + "DOCID" + charQuote + "" +
                                 " FROM METADATA   LEFT OUTER JOIN " +
                                 " (SELECT COUNT(*) AS " + charQuote + "Total Document" + charQuote + " ,MetaDataID FROM DOCUMENT WHERE Status=1 GROUP BY MetaDataID)  " + charQuote + "TotalDocument" + charQuote + "" +
                                 " ON METADATA.ID = " + charQuote + "TotalDocument" + charQuote + ".MetaDataID  LEFT OUTER JOIN " +
                                 " (SELECT COUNT(*) AS " + charQuote + "Approved Document" + charQuote + " ,MetaDataID FROM DOCUMENT WHERE DocumentStatusID = 1 AND Status=1 GROUP BY MetaDataID)  " + charQuote + "ApprovedDocument" + charQuote + "" +
                                 " ON METADATA.ID = " + charQuote + "ApprovedDocument" + charQuote + ".MetaDataID    LEFT OUTER JOIN " +
                                 " (SELECT COUNT(*) AS " + charQuote + "Rejected Document" + charQuote + ",MetaDataID FROM DOCUMENT WHERE DocumentStatusID = 2 AND Status=1 GROUP BY MetaDataID)  " + charQuote + "RejectedDocument" + charQuote + "" +
                                 " ON METADATA.ID = " + charQuote + "RejectedDocument" + charQuote + ".MetaDataID    LEFT OUTER JOIN " +
                                 " (SELECT COUNT(*) AS " + charQuote + "Pending For Entry" + charQuote + ",MetaDataID FROM DOCUMENT WHERE DocumentStatusID = 3 AND Status=1 GROUP BY MetaDataID)  " + charQuote + "PendingForEntry" + charQuote + "" +
                                 " ON METADATA.ID = " + charQuote + "PendingForEntry" + charQuote + ".MetaDataID    LEFT OUTER JOIN " +
                                 " (SELECT COUNT(*) AS " + charQuote + "Entry Completed" + charQuote + ",MetaDataID FROM DOCUMENT WHERE DocumentStatusID = 4 AND Status=1 GROUP BY MetaDataID)  " + charQuote + "EntryCompleted" + charQuote + "" +
                                 " ON METADATA.ID = " + charQuote + "EntryCompleted" + charQuote + ".MetaDataID  LEFT OUTER JOIN" +
                                 " (SELECT MAX (ID) AS " + charQuote + "DOCID" + charQuote + ",MetaDataID FROM DOCUMENT WHERE Status=1 GROUP BY MetaDataID)  " + charQuote + "DOCID" + charQuote + "" +
                                 " ON METADATA.ID = " + charQuote + "DOCID" + charQuote + ".MetaDataID  " +
                                 " WHERE METADATA.STATUS=1 AND METADATA.RepositoryID = @RepositoryID AND METADATA.MetaTemplateID = @MetaTemplateID AND METADATA.CategoryID = @CategoryID AND METADATA.FolderID = @FolderID " +
                                 " GROUP BY METADATA.ID, METADATA.MetaDataCode," + charQuote + "Total Document" + charQuote + "," + charQuote + "Approved Document" + charQuote + "," + charQuote + "Rejected Document" + charQuote + "," + charQuote + "Pending For Entry" + charQuote + "," + charQuote + "Entry Completed" + charQuote + "," + charQuote + "DOCID" + charQuote + " ORDER BY METADATA.ID DESC";

                }
                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

                DbParameter[] objDbParameter = new DbParameter[4];
                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "RepositoryID";
                objDbParameter[0].Value = objMetaData.RepositoryID;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "MetaTemplateID";
                objDbParameter[1].Value = objMetaData.MetaTemplateID;

                objDbParameter[2] = objDbProviderFactory.CreateParameter();
                objDbParameter[2].ParameterName = "CategoryID";
                objDbParameter[2].Value = objMetaData.CategoryID;

                objDbParameter[3] = objDbProviderFactory.CreateParameter();
                objDbParameter[3].ParameterName = "FolderID";
                objDbParameter[3].Value = objMetaData.FolderID;

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

        public static Utility.ResultType SelectForGridNew(out DataTable objDataTable, DocumentManager.Status enumStatus, MetaData objMetaData, string FromDate, string Todate)
        {
            try
            {
                string strQuery = string.Empty;
                char charQuote = '"';
                if (enumStatus == DocumentManager.Status.Uploaded)
                {
                    strQuery = @"SELECT MAX(DOCUMENT.ID)  AS DOCID,METADATA.ID,METADATA.MetaDataCode,count(DOCUMENT.DocumentStatusID) AS " + charQuote + "Uploaded Document Count" + charQuote +
                                " FROM METADATA INNER JOIN DOCUMENT ON METADATA.ID = DOCUMENT.MetaDataID " +
                                " WHERE DOCUMENT.Status=1 AND METADATA.RepositoryID = @RepositoryID AND METADATA.MetaTemplateID = @MetaTemplateID " +
                                " AND METADATA.CategoryID = @CategoryID AND METADATA.FolderID = @FolderID " +
                                " GROUP BY METADATA.MetaDataCode,DOCUMENT.DocumentStatusID,METADATA.ID HAVING DOCUMENT.DocumentStatusID=3 " +
                                " ORDER BY METADATA.ID DESC";
                }
                else if (enumStatus == DocumentManager.Status.EntryCompleted)
                {
                    strQuery = @"SELECT MAX(DOCUMENT.ID)  AS DOCID,METADATA.ID,METADATA.MetaDataCode,count(DOCUMENT.DocumentStatusID) AS " + charQuote + "Entry Completed Count" + charQuote +
                                " FROM METADATA INNER JOIN DOCUMENT ON METADATA.ID = DOCUMENT.MetaDataID " +
                                " WHERE DOCUMENT.Status=1 AND METADATA.RepositoryID = @RepositoryID AND METADATA.MetaTemplateID = @MetaTemplateID " +
                                " AND METADATA.CategoryID = @CategoryID AND METADATA.FolderID = @FolderID " +
                                " GROUP BY METADATA.MetaDataCode,DOCUMENT.DocumentStatusID,METADATA.ID HAVING DOCUMENT.DocumentStatusID=4 " +
                                " ORDER BY METADATA.ID DESC";
                }
                else if (enumStatus == DocumentManager.Status.Approved)
                {
                    strQuery = @"SELECT MAX(DOCUMENT.ID)  AS DOCID,METADATA.ID,METADATA.MetaDataCode,count(DOCUMENT.DocumentStatusID) AS " + charQuote + "Approved Document Count" + charQuote +
                                " FROM METADATA INNER JOIN DOCUMENT ON METADATA.ID = DOCUMENT.MetaDataID " +
                                " WHERE DOCUMENT.Status=1 AND METADATA.RepositoryID = @RepositoryID AND METADATA.MetaTemplateID = @MetaTemplateID " +
                                " AND METADATA.CategoryID = @CategoryID AND METADATA.FolderID = @FolderID " +
                                " GROUP BY METADATA.MetaDataCode,DOCUMENT.DocumentStatusID,METADATA.ID HAVING DOCUMENT.DocumentStatusID=1 " +
                                " ORDER BY METADATA.ID DESC";

                }
                else if (enumStatus == DocumentManager.Status.Rejected)
                {
                    strQuery = @"SELECT MAX(DOCUMENT.ID)  AS DOCID,METADATA.ID,METADATA.MetaDataCode,count(DOCUMENT.DocumentStatusID) AS " + charQuote + "Rejected Document Count" + charQuote +
                                " FROM METADATA INNER JOIN DOCUMENT ON METADATA.ID = DOCUMENT.MetaDataID " +
                                " WHERE DOCUMENT.Status=1 AND METADATA.RepositoryID = @RepositoryID AND METADATA.MetaTemplateID = @MetaTemplateID " +
                                " AND METADATA.CategoryID = @CategoryID AND METADATA.FolderID = @FolderID " +
                                " GROUP BY METADATA.MetaDataCode,DOCUMENT.DocumentStatusID,METADATA.ID HAVING DOCUMENT.DocumentStatusID=2 " +
                                " ORDER BY METADATA.ID DESC";
                }
                else if (enumStatus == DocumentManager.Status.TotalUploaded)
                {
                    if(FromDate=="" || Todate=="" || FromDate==string.Empty || Todate==string.Empty)
                    {
                        strQuery = "SELECT METADATA.ID,METADATA.MetaDataCode," + charQuote + "Total Document" + charQuote + "," + charQuote + "Approved Document" + charQuote + "," + charQuote + "Rejected Document" + charQuote + "," + charQuote + "Pending For Entry" + charQuote + "," + charQuote + "Entry Completed" + charQuote + "," + charQuote + "DOCID" + charQuote + "" +
                                 " FROM METADATA   LEFT OUTER JOIN " +
                                 " (SELECT COUNT(*) AS " + charQuote + "Total Document" + charQuote + " ,MetaDataID FROM DOCUMENT WHERE Status=1 GROUP BY MetaDataID)  " + charQuote + "TotalDocument" + charQuote + "" +
                                 " ON METADATA.ID = " + charQuote + "TotalDocument" + charQuote + ".MetaDataID  LEFT OUTER JOIN " +
                                 " (SELECT COUNT(*) AS " + charQuote + "Approved Document" + charQuote + " ,MetaDataID FROM DOCUMENT WHERE DocumentStatusID = 1 AND Status=1 GROUP BY MetaDataID)  " + charQuote + "ApprovedDocument" + charQuote + "" +
                                 " ON METADATA.ID = " + charQuote + "ApprovedDocument" + charQuote + ".MetaDataID    LEFT OUTER JOIN " +
                                 " (SELECT COUNT(*) AS " + charQuote + "Rejected Document" + charQuote + ",MetaDataID FROM DOCUMENT WHERE DocumentStatusID = 2 AND Status=1 GROUP BY MetaDataID)  " + charQuote + "RejectedDocument" + charQuote + "" +
                                 " ON METADATA.ID = " + charQuote + "RejectedDocument" + charQuote + ".MetaDataID    LEFT OUTER JOIN " +
                                 " (SELECT COUNT(*) AS " + charQuote + "Pending For Entry" + charQuote + ",MetaDataID FROM DOCUMENT WHERE DocumentStatusID = 3 AND Status=1 GROUP BY MetaDataID)  " + charQuote + "PendingForEntry" + charQuote + "" +
                                 " ON METADATA.ID = " + charQuote + "PendingForEntry" + charQuote + ".MetaDataID    LEFT OUTER JOIN " +
                                 " (SELECT COUNT(*) AS " + charQuote + "Entry Completed" + charQuote + ",MetaDataID FROM DOCUMENT WHERE DocumentStatusID = 4 AND Status=1 GROUP BY MetaDataID)  " + charQuote + "EntryCompleted" + charQuote + "" +
                                 " ON METADATA.ID = " + charQuote + "EntryCompleted" + charQuote + ".MetaDataID  LEFT OUTER JOIN" +
                                 " (SELECT MAX (ID) AS " + charQuote + "DOCID" + charQuote + ",MetaDataID FROM DOCUMENT WHERE Status=1 GROUP BY MetaDataID)  " + charQuote + "DOCID" + charQuote + "" +
                                 " ON METADATA.ID = " + charQuote + "DOCID" + charQuote + ".MetaDataID  " +
                                 " WHERE METADATA.STATUS=1 AND METADATA.RepositoryID = @RepositoryID AND METADATA.MetaTemplateID = @MetaTemplateID AND METADATA.CategoryID = @CategoryID AND METADATA.FolderID = @FolderID " +
                                 " GROUP BY METADATA.ID, METADATA.MetaDataCode," + charQuote + "Total Document" + charQuote + "," + charQuote + "Approved Document" + charQuote + "," + charQuote + "Rejected Document" + charQuote + "," + charQuote + "Pending For Entry" + charQuote + "," + charQuote + "Entry Completed" + charQuote + "," + charQuote + "DOCID" + charQuote + " ORDER BY METADATA.ID DESC";

                    }
                    else
                    {
                        strQuery = "SELECT METADATA.ID,METADATA.MetaDataCode," + charQuote + "Total Document" + charQuote + "," + charQuote + "Approved Document" + charQuote + "," + charQuote + "Rejected Document" + charQuote + "," + charQuote + "Pending For Entry" + charQuote + "," + charQuote + "Entry Completed" + charQuote + "," + charQuote + "DOCID" + charQuote + "" +
                                   " FROM METADATA   LEFT OUTER JOIN " +
                                   " (SELECT COUNT(*) AS " + charQuote + "Total Document" + charQuote + " ,MetaDataID FROM DOCUMENT WHERE Status=1 and (convert(date,CreatedOn) between '" + FromDate + "' and '" + Todate + "') GROUP BY MetaDataID)  " + charQuote + "TotalDocument" + charQuote + "" +
                                   " ON METADATA.ID = " + charQuote + "TotalDocument" + charQuote + ".MetaDataID  LEFT OUTER JOIN " +
                                   " (SELECT COUNT(*) AS " + charQuote + "Approved Document" + charQuote + " ,MetaDataID FROM DOCUMENT WHERE DocumentStatusID = 1 AND Status=1 GROUP BY MetaDataID)  " + charQuote + "ApprovedDocument" + charQuote + "" +
                                   " ON METADATA.ID = " + charQuote + "ApprovedDocument" + charQuote + ".MetaDataID    LEFT OUTER JOIN " +
                                   " (SELECT COUNT(*) AS " + charQuote + "Rejected Document" + charQuote + ",MetaDataID FROM DOCUMENT WHERE DocumentStatusID = 2 AND Status=1 GROUP BY MetaDataID)  " + charQuote + "RejectedDocument" + charQuote + "" +
                                   " ON METADATA.ID = " + charQuote + "RejectedDocument" + charQuote + ".MetaDataID    LEFT OUTER JOIN " +
                                   " (SELECT COUNT(*) AS " + charQuote + "Pending For Entry" + charQuote + ",MetaDataID FROM DOCUMENT WHERE DocumentStatusID = 3 AND Status=1 GROUP BY MetaDataID)  " + charQuote + "PendingForEntry" + charQuote + "" +
                                   " ON METADATA.ID = " + charQuote + "PendingForEntry" + charQuote + ".MetaDataID    LEFT OUTER JOIN " +
                                   " (SELECT COUNT(*) AS " + charQuote + "Entry Completed" + charQuote + ",MetaDataID FROM DOCUMENT WHERE DocumentStatusID = 4 AND Status=1 GROUP BY MetaDataID)  " + charQuote + "EntryCompleted" + charQuote + "" +
                                   " ON METADATA.ID = " + charQuote + "EntryCompleted" + charQuote + ".MetaDataID  LEFT OUTER JOIN" +
                                   " (SELECT MAX (ID) AS " + charQuote + "DOCID" + charQuote + ",MetaDataID FROM DOCUMENT WHERE Status=1 GROUP BY MetaDataID)  " + charQuote + "DOCID" + charQuote + "" +
                                   " ON METADATA.ID = " + charQuote + "DOCID" + charQuote + ".MetaDataID  " +
                                   " WHERE METADATA.STATUS=1 AND METADATA.RepositoryID = @RepositoryID AND METADATA.MetaTemplateID = @MetaTemplateID AND METADATA.CategoryID = @CategoryID AND METADATA.FolderID = @FolderID " +
                                   " GROUP BY METADATA.ID, METADATA.MetaDataCode," + charQuote + "Total Document" + charQuote + "," + charQuote + "Approved Document" + charQuote + "," + charQuote + "Rejected Document" + charQuote + "," + charQuote + "Pending For Entry" + charQuote + "," + charQuote + "Entry Completed" + charQuote + "," + charQuote + "DOCID" + charQuote + " ORDER BY METADATA.ID DESC";
                    }
                    
                }
                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

                DbParameter[] objDbParameter = new DbParameter[4];
                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "RepositoryID";
                objDbParameter[0].Value = objMetaData.RepositoryID;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "MetaTemplateID";
                objDbParameter[1].Value = objMetaData.MetaTemplateID;

                objDbParameter[2] = objDbProviderFactory.CreateParameter();
                objDbParameter[2].ParameterName = "CategoryID";
                objDbParameter[2].Value = objMetaData.CategoryID;

                objDbParameter[3] = objDbProviderFactory.CreateParameter();
                objDbParameter[3].ParameterName = "FolderID";
                objDbParameter[3].Value = objMetaData.FolderID;

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

        public static Utility.ResultType SelectForReport(out DataTable objDataTable)
        {
            try
            {
                char charQuote = '"';
                //string strQuery = @"select count(*) as DocUploaded,sum(pagecount)as TotalImages," + charQuote + "MetaTemplate" + charQuote + " as met from document" +
                //                  " left outer join  METADATA " +
                //                  "on document.metadataid=METADATA.id " +
                //                  "left outer join  (SELECT ID,MetaTemplateName AS " + charQuote + "MetaTemplate" + charQuote + " FROM vwMetaTemplate WHERE Status=1)   " + charQuote + "MetaTemplateName" + charQuote +
                //                  " ON METADATA.MetaTemplateID = " + charQuote + "MetaTemplateName" + charQuote + ".ID" +
                //                  " group by " + charQuote + "MetaTemplate" + charQuote + "";
                string strQuery = @"select count(*),c.metatemplatename,sum(d.pagecount) from documententry a inner join metatemplatefields b
                                    on a.fieldid=b.id inner join metatemplate c
                                    on b.metatemplateid=c.id inner join document d
                                    on a.documentid=d.id
                                    and a.fieldid in (select id from metatemplatefields where metatemplateid=c.id and fieldname='TYPE OF FILE')
                                    group by c.metatemplatename";
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

        public static Utility.ResultType SelectForDashBoard(out DataTable objDataTable, DashBoard enumDashBoard, MetaData objMetaData)
        {
            try
            {
                string strQuery = string.Empty;
                char charQuote = '"';
                if (enumDashBoard == DashBoard.RepositoryLevel)
                {
                    strQuery = " SELECT RepositoryID," + charQuote + "RepositoryName" + charQuote + ", " +
                                " SUM(" + charQuote + "DASHBOARD" + charQuote + "." + charQuote + "TotalDocument" + charQuote + ") AS  " + charQuote + "TotalDocument" + charQuote + ",  SUM(" + charQuote + "DASHBOARD" + charQuote + "." + charQuote + "ApprovedDocument" + charQuote + ") AS  " + charQuote + "ApprovedDocument" + charQuote + "," +
                                " SUM(" + charQuote + "DASHBOARD" + charQuote + "." + charQuote + "RejectedDocument" + charQuote + ") AS  " + charQuote + "RejectedDocument" + charQuote + ", SUM(" + charQuote + "DASHBOARD" + charQuote + "." + charQuote + "PendingForEntry" + charQuote + ") AS  " + charQuote + "PendingForEntry" + charQuote + ", SUM(" + charQuote + "DASHBOARD" + charQuote + "." + charQuote + "EntryCompleted" + charQuote + ") AS  " + charQuote + "EntryCompleted" + charQuote + " " +
                                " FROM  " +
                                " (SELECT RepositoryID,MetaTemplateID,CategoryID,FolderID, (SELECT RepositoryName FROM vwRepository WHERE ID = METADATA.RepositoryID AND Status=1) AS  " + charQuote + "RepositoryName" + charQuote + " ," +
                                " " + charQuote + "TotalDocument" + charQuote + "," + charQuote + "ApprovedDocument" + charQuote + "," + charQuote + "RejectedDocument" + charQuote + "," + charQuote + "PendingForEntry" + charQuote + "," + charQuote + "EntryCompleted" + charQuote + "" +
                                " FROM METADATA   LEFT OUTER JOIN " +
                                " (SELECT COUNT(*) AS " + charQuote + "TotalDocument" + charQuote + " ,MetaDataID FROM DOCUMENT WHERE Status=1 GROUP BY MetaDataID)  " + charQuote + "Total Document" + charQuote + "" +
                                " ON METADATA.ID = " + charQuote + "Total Document" + charQuote + ".MetaDataID  LEFT OUTER JOIN " +
                                " (SELECT COUNT(*) AS " + charQuote + "ApprovedDocument" + charQuote + " ,MetaDataID FROM DOCUMENT WHERE DocumentStatusID = 1 AND Status=1 GROUP BY MetaDataID)  " + charQuote + "Approved Document" + charQuote + "" +
                                " ON METADATA.ID = " + charQuote + "Approved Document" + charQuote + ".MetaDataID    LEFT OUTER JOIN " +
                                " (SELECT COUNT(*) AS " + charQuote + "RejectedDocument" + charQuote + ",MetaDataID FROM DOCUMENT WHERE DocumentStatusID = 2 AND Status=1 GROUP BY MetaDataID)  " + charQuote + "Rejected Document" + charQuote + "" +
                                " ON METADATA.ID = " + charQuote + "Rejected Document" + charQuote + ".MetaDataID    LEFT OUTER JOIN " +
                                " (SELECT COUNT(*) AS " + charQuote + "PendingForEntry" + charQuote + ",MetaDataID FROM DOCUMENT WHERE DocumentStatusID = 3 AND Status=1 GROUP BY MetaDataID)  " + charQuote + "Pending For Entry" + charQuote + "" +
                                " ON METADATA.ID = " + charQuote + "Pending For Entry" + charQuote + ".MetaDataID    LEFT OUTER JOIN " +
                                " (SELECT COUNT(*) AS " + charQuote + "EntryCompleted" + charQuote + ",MetaDataID FROM DOCUMENT WHERE DocumentStatusID = 4 AND Status=1 GROUP BY MetaDataID)  " + charQuote + "Entry Completed" + charQuote + "" +
                                " ON METADATA.ID = " + charQuote + "Entry Completed" + charQuote + ".MetaDataID  " +
                                " WHERE METADATA.STATUS=1 AND " +
                                " RepositoryID IN (" + Utility.GetAccessRight(Utility.AccessRight.Repository) + ") " +
                                " AND MetaTemplateID IN (" + Utility.GetAccessRight(Utility.AccessRight.MetaTemplate) + ")" +
                                " AND CategoryID IN (" + Utility.GetAccessRight(Utility.AccessRight.Category) + ",-1) " +
                                " AND FolderID IN (" + Utility.GetAccessRight(Utility.AccessRight.Folder) + ",0)) " +
                                " DASHBOARD GROUP BY DASHBOARD." + charQuote + "RepositoryName" + charQuote + ",RepositoryID ";
                }
                else if (enumDashBoard == DashBoard.MetaTemplateLevel)
                {
                    strQuery = " SELECT  METADATA.ID,MetaTemplateID,CategoryID,FolderID," + charQuote + "MetaTemplate" + charQuote + ", " + charQuote + "Category" + charQuote + ", " + charQuote + "Folder" + charQuote + ", MetaDataCode, " + charQuote + "TotalDocument" + charQuote + "," + charQuote + "ApprovedDocument" + charQuote + "," + charQuote + "RejectedDocument" + charQuote + "," + charQuote + "PendingForEntry" + charQuote + "," + charQuote + "EntryCompleted" + charQuote + " " +
                                " FROM METADATA   LEFT OUTER JOIN  " +
                                " (SELECT ID,MetaTemplateName AS " + charQuote + "MetaTemplate" + charQuote + " FROM vwMetaTemplate WHERE Status=1) " + charQuote + "MetaTemplateName" + charQuote + " " +
                                " ON METADATA.MetaTemplateID = " + charQuote + "MetaTemplateName" + charQuote + ".ID  LEFT OUTER JOIN  " +
                                " (SELECT ID,CategoryName AS " + charQuote + "Category" + charQuote + " FROM vwCategory WHERE Status=1) " + charQuote + "CategoryName" + charQuote + " " +
                                " ON METADATA.CategoryID = " + charQuote + "CategoryName" + charQuote + ".ID  LEFT OUTER JOIN  " +
                                " (SELECT ID,FolderName AS " + charQuote + "Folder" + charQuote + " FROM vwFolder WHERE Status=1) " + charQuote + "FolderName" + charQuote + " " +
                                " ON METADATA.FolderID = " + charQuote + "FolderName" + charQuote + ".ID  LEFT OUTER JOIN  " +
                                " (SELECT COUNT(*) AS " + charQuote + "TotalDocument" + charQuote + " ,MetaDataID FROM DOCUMENT WHERE Status=1 GROUP BY MetaDataID)  " + charQuote + "Total Document" + charQuote + " " +
                                " ON METADATA.ID = " + charQuote + "Total Document" + charQuote + ".MetaDataID  LEFT OUTER JOIN  " +
                                " (SELECT COUNT(*) AS " + charQuote + "ApprovedDocument" + charQuote + " ,MetaDataID FROM DOCUMENT WHERE DocumentStatusID = 1 AND Status=1 GROUP BY MetaDataID)  " + charQuote + "Approved Document" + charQuote + " " +
                                " ON METADATA.ID = " + charQuote + "Approved Document" + charQuote + ".MetaDataID    LEFT OUTER JOIN  " +
                                " (SELECT COUNT(*) AS " + charQuote + "RejectedDocument" + charQuote + ",MetaDataID FROM DOCUMENT WHERE DocumentStatusID = 2 AND Status=1 GROUP BY MetaDataID)  " + charQuote + "Rejected Document" + charQuote + " " +
                                " ON METADATA.ID = " + charQuote + "Rejected Document" + charQuote + ".MetaDataID    LEFT OUTER JOIN  " +
                                " (SELECT COUNT(*) AS " + charQuote + "PendingForEntry" + charQuote + ",MetaDataID FROM DOCUMENT WHERE DocumentStatusID = 3 AND Status=1 GROUP BY MetaDataID)  " + charQuote + "Pending For Entry" + charQuote + " " +
                                " ON METADATA.ID = " + charQuote + "Pending For Entry" + charQuote + ".MetaDataID    LEFT OUTER JOIN  " +
                                " (SELECT COUNT(*) AS " + charQuote + "EntryCompleted" + charQuote + ",MetaDataID FROM DOCUMENT WHERE DocumentStatusID = 4 AND Status=1 GROUP BY MetaDataID)  " + charQuote + "Entry Completed" + charQuote + " " +
                                " ON METADATA.ID = " + charQuote + "Entry Completed" + charQuote + ".MetaDataID " +
                                " WHERE METADATA.STATUS=1 AND " +
                                " RepositoryID = " + objMetaData.RepositoryID + "" +
                                " AND MetaTemplateID IN (" + Utility.GetAccessRight(Utility.AccessRight.MetaTemplate) + ")" +
                                " AND CategoryID IN (" + Utility.GetAccessRight(Utility.AccessRight.Category) + ",-1) " +
                                " AND FolderID IN (" + Utility.GetAccessRight(Utility.AccessRight.Folder) + ",0) ";

                }
                else if (enumDashBoard == DashBoard.MetaDataLevel)
                {
                    strQuery = "SELECT ID,MetaDataID,DocumentName," + charQuote + "Size" + charQuote + ",DocumentType,DocumentStatusID,Tag," +
                               " (SELECT StatusName FROM DOCUMENTSTATUS WHERE ID=DOCUMENT.DocumentStatusID) AS " + charQuote + "DocumentStatus" + charQuote + "" +
                               " FROM DOCUMENT WHERE MetaDataID=" + objMetaData.MetaDataID + " AND Status=1";
                }
                else if (enumDashBoard == DashBoard.DocumentStatusLevel)
                {
                    strQuery = " SELECT DOCUMENTSTATUS.StatusName AS " + charQuote + "DocumentStatus" + charQuote + ",Count(*) AS " + charQuote + "TotalDocument" + charQuote + " " +
                                " FROM DOCUMENT LEFT OUTER JOIN DOCUMENTSTATUS " +
                                " ON DOCUMENT.DocumentStatusID = DOCUMENTSTATUS.ID " +
                                " WHERE MetaDataID=" + objMetaData.MetaDataID + " AND Status=1 GROUP BY DOCUMENTSTATUS.StatusName";
                }

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

        //public static Utility.ResultType SelectForFieldSearch_Indepay()
        //{
        //    try
        //    {
        //        string strQuery = string.Empty;
        //        char charQuote = '"';
        //        if (objMetaData.MetaDataID == -1)
        //        {
        //            if (enumSearchType == SearchType.EqualTo)
        //            {
        //                strQuery = @" SELECT DOCUMENT.ID,MetaDataID,DOCUMENT.DocumentStatusID,METADATA.MetaDataCode,DocumentName," + charQuote + "Size" + charQuote + ",DocumentType,Tag," +
        //                            " (SELECT StatusName FROM DOCUMENTSTATUS WHERE ID= DOCUMENT.DocumentStatusID ) AS " + charQuote + "DocumentStatus" + charQuote + " " +
        //                            " FROM DOCUMENT INNER JOIN METADATA " +
        //                            " ON DOCUMENT.MetaDataID = METADATA.ID " +
        //                            " WHERE  DOCUMENT.ID IN (SELECT DocumentID FROM DOCUMENTENTRY WHERE FieldID=@FieldID AND LOWER(FieldData) = @FieldData) " +
        //                            " AND RepositoryID=@RepositoryID AND MetaTemplateID=@MetaTemplateID AND FolderID=@FolderID AND CategoryID=@CategoryID";
        //            }
        //            else if (enumSearchType == SearchType.Like)
        //            {
        //                strQuery = @" SELECT DOCUMENT.ID,MetaDataID,DOCUMENT.DocumentStatusID,METADATA.MetaDataCode,DocumentName," + charQuote + "Size" + charQuote + ",DocumentType,Tag," +
        //                            " (SELECT StatusName FROM DOCUMENTSTATUS WHERE ID= DOCUMENT.DocumentStatusID ) AS " + charQuote + "DocumentStatus" + charQuote + " " +
        //                            " FROM DOCUMENT INNER JOIN METADATA " +
        //                            " ON DOCUMENT.MetaDataID = METADATA.ID " +
        //                            " WHERE  DOCUMENT.ID IN (SELECT DocumentID FROM DOCUMENTENTRY WHERE FieldID=@FieldID AND LOWER(FieldData) LIKE @FieldData) " +
        //                            " AND RepositoryID=@RepositoryID AND MetaTemplateID=@MetaTemplateID AND FolderID=@FolderID AND CategoryID=@CategoryID";
        //            }
        //    }
        //    catch(Exception ex)
        //    {
        //        LogManager.ErrorLog(Utility.LogFilePath, ex);
        //        objDataTable = null;
        //        return Utility.ResultType.Error;
        //    }
        //}

        public static Utility.ResultType SelectForFieldSearch(out DataTable objDataTable, MetaData objMetaData, DocumentEntry objDocumentEntry, SearchType enumSearchType)
        {
            try
            {
                string strQuery = string.Empty;
                char charQuote = '"';
                if (objMetaData.MetaDataID == -1)
                {
                    if (enumSearchType == SearchType.EqualTo)
                    {
                        strQuery = @" SELECT DOCUMENT.ID,MetaDataID,DOCUMENT.DocumentStatusID,METADATA.MetaDataCode,DocumentName," + charQuote + "Size" + charQuote + ",DocumentType,Tag," +
                                    " (SELECT StatusName FROM DOCUMENTSTATUS WHERE ID= DOCUMENT.DocumentStatusID ) AS " + charQuote + "DocumentStatus" + charQuote + " " +
                                    " FROM DOCUMENT INNER JOIN METADATA " +
                                    " ON DOCUMENT.MetaDataID = METADATA.ID " +
                                    " WHERE  DOCUMENT.ID IN (SELECT DocumentID FROM DOCUMENTENTRY WHERE FieldID=@FieldID AND LOWER(FieldData) = @FieldData) " +
                                    " AND RepositoryID=@RepositoryID AND MetaTemplateID=@MetaTemplateID AND FolderID=@FolderID AND CategoryID=@CategoryID";
                    }
                    else if (enumSearchType == SearchType.Like)
                    {
                        strQuery = @" SELECT DOCUMENT.ID,MetaDataID,DOCUMENT.DocumentStatusID,METADATA.MetaDataCode,DocumentName," + charQuote + "Size" + charQuote + ",DocumentType,Tag," +
                                    " (SELECT StatusName FROM DOCUMENTSTATUS WHERE ID= DOCUMENT.DocumentStatusID ) AS " + charQuote + "DocumentStatus" + charQuote + " " +
                                    " FROM DOCUMENT INNER JOIN METADATA " +
                                    " ON DOCUMENT.MetaDataID = METADATA.ID " +
                                    " WHERE  DOCUMENT.ID IN (SELECT DocumentID FROM DOCUMENTENTRY WHERE FieldID=@FieldID AND LOWER(FieldData) LIKE @FieldData) " +
                                    " AND RepositoryID=@RepositoryID AND MetaTemplateID=@MetaTemplateID AND FolderID=@FolderID AND CategoryID=@CategoryID";
                    }
                    else if (enumSearchType == SearchType.DateRange)
                    {
                        if (Utility.GetProviderFactory.GetType().ToString() == "System.Data.OracleClient.OracleClientFactory")
                        {
                            strQuery = @" SELECT DOCUMENT.ID,MetaDataID,DOCUMENT.DocumentStatusID,METADATA.MetaDataCode,DocumentName," + charQuote + "Size" + charQuote + ",DocumentType,Tag," +
                                  " (SELECT StatusName FROM DOCUMENTSTATUS WHERE ID= DOCUMENT.DocumentStatusID ) AS " + charQuote + "DocumentStatus" + charQuote + " " +
                                  " FROM DOCUMENT INNER JOIN METADATA " +
                                  " ON DOCUMENT.MetaDataID = METADATA.ID " +
                                  " WHERE  DOCUMENT.ID IN (SELECT DocumentID FROM DOCUMENTENTRY WHERE FieldID=@FieldID AND (TO_DATE(FieldData,'mm/dd/yyyy') BETWEEN TO_DATE('" + objDocumentEntry.FromDate + "','mm/dd/yyyy') AND TO_DATE('" + objDocumentEntry.ToDate + "','mm/dd/yyyy'))) " +
                                  " AND RepositoryID=@RepositoryID AND MetaTemplateID=@MetaTemplateID AND FolderID=@FolderID AND CategoryID=@CategoryID ";
                        }
                        else if (Utility.GetProviderFactory.GetType().ToString() == "System.Data.SqlClient.SqlClientFactory")
                        {
                            strQuery = @" SELECT DOCUMENT.ID,MetaDataID,DOCUMENT.DocumentStatusID,METADATA.MetaDataCode,DocumentName," + charQuote + "Size" + charQuote + ",DocumentType,Tag," +
                                  " (SELECT StatusName FROM DOCUMENTSTATUS WHERE ID= DOCUMENT.DocumentStatusID ) AS " + charQuote + "DocumentStatus" + charQuote + " " +
                                  " FROM DOCUMENT INNER JOIN METADATA " +
                                  " ON DOCUMENT.MetaDataID = METADATA.ID " +
                                  " WHERE  DOCUMENT.ID IN (SELECT DocumentID FROM DOCUMENTENTRY WHERE FieldID=@FieldID AND (CONVERT(DATETIME,FieldData,101) BETWEEN CONVERT(DATETIME,'" + objDocumentEntry.FromDate + "',101) AND CONVERT(DATETIME,'" + objDocumentEntry.ToDate + "',101))) " +
                                  " AND RepositoryID=@RepositoryID AND MetaTemplateID=@MetaTemplateID AND FolderID=@FolderID AND CategoryID=@CategoryID ";
                        }
                    }
                }
                else
                {
                    if (enumSearchType == SearchType.EqualTo)
                    {
                        strQuery = @" SELECT DOCUMENT.ID,MetaDataID,DOCUMENT.DocumentStatusID,METADATA.MetaDataCode,DocumentName," + charQuote + "Size" + charQuote + ",DocumentType,Tag," +
                                    " (SELECT StatusName FROM DOCUMENTSTATUS WHERE ID= DOCUMENT.DocumentStatusID ) AS " + charQuote + "DocumentStatus" + charQuote + " " +
                                    " FROM DOCUMENT INNER JOIN METADATA " +
                                    " ON DOCUMENT.MetaDataID = METADATA.ID " +
                                    " WHERE  DOCUMENT.ID IN (SELECT DocumentID FROM DOCUMENTENTRY WHERE FieldID=@FieldID AND LOWER(FieldData) = @FieldData) " +
                                    " AND RepositoryID=@RepositoryID AND MetaTemplateID=@MetaTemplateID AND FolderID=@FolderID AND CategoryID=@CategoryID AND MetaDataID=@MetaDataID";
                    }
                    else if (enumSearchType == SearchType.Like)
                    {
                        strQuery = @" SELECT DOCUMENT.ID,MetaDataID,DOCUMENT.DocumentStatusID,METADATA.MetaDataCode,DocumentName," + charQuote + "Size" + charQuote + ",DocumentType,Tag," +
                                    " (SELECT StatusName FROM DOCUMENTSTATUS WHERE ID= DOCUMENT.DocumentStatusID ) AS " + charQuote + "DocumentStatus" + charQuote + " " +
                                    " FROM DOCUMENT INNER JOIN METADATA " +
                                    " ON DOCUMENT.MetaDataID = METADATA.ID " +
                                    " WHERE  DOCUMENT.ID IN (SELECT DocumentID FROM DOCUMENTENTRY WHERE FieldID=@FieldID AND LOWER(FieldData) LIKE @FieldData) " +
                                    " AND RepositoryID=@RepositoryID AND MetaTemplateID=@MetaTemplateID AND FolderID=@FolderID AND CategoryID=@CategoryID AND MetaDataID=@MetaDataID";
                    }
                    else if (enumSearchType == SearchType.DateRange)
                    {
                        if (Utility.GetProviderFactory.GetType().ToString() == "System.Data.OracleClient.OracleClientFactory")
                        {
                            strQuery = @" SELECT DOCUMENT.ID,MetaDataID,DOCUMENT.DocumentStatusID,METADATA.MetaDataCode,DocumentName," + charQuote + "Size" + charQuote + ",DocumentType,Tag," +
                                    " (SELECT StatusName FROM DOCUMENTSTATUS WHERE ID= DOCUMENT.DocumentStatusID ) AS " + charQuote + "DocumentStatus" + charQuote + " " +
                                    " FROM DOCUMENT INNER JOIN METADATA " +
                                    " ON DOCUMENT.MetaDataID = METADATA.ID " +
                                    " WHERE  DOCUMENT.ID IN (SELECT DocumentID FROM DOCUMENTENTRY WHERE FieldID=@FieldID AND (TO_DATE(FieldData,'mm/dd/yyyy') BETWEEN TO_DATE('" + objDocumentEntry.FromDate + "','mm/dd/yyyy') AND TO_DATE('" + objDocumentEntry.ToDate + "','mm/dd/yyyy'))) " +
                                    " AND RepositoryID=@RepositoryID AND MetaTemplateID=@MetaTemplateID AND FolderID=@FolderID AND CategoryID=@CategoryID AND MetaDataID=@MetaDataID ";
                        }
                        else if (Utility.GetProviderFactory.GetType().ToString() == "System.Data.SqlClient.SqlClientFactory")
                        {
                            strQuery = @" SELECT DOCUMENT.ID,MetaDataID,DOCUMENT.DocumentStatusID,METADATA.MetaDataCode,DocumentName," + charQuote + "Size" + charQuote + ",DocumentType,Tag," +
                                  " (SELECT StatusName FROM DOCUMENTSTATUS WHERE ID= DOCUMENT.DocumentStatusID ) AS " + charQuote + "DocumentStatus" + charQuote + " " +
                                  " FROM DOCUMENT INNER JOIN METADATA " +
                                  " ON DOCUMENT.MetaDataID = METADATA.ID " +
                                  " WHERE  DOCUMENT.ID IN (SELECT DocumentID FROM DOCUMENTENTRY WHERE FieldID=@FieldID AND (CONVERT(DATETIME,FieldData,101) BETWEEN CONVERT(DATETIME,'" + objDocumentEntry.FromDate + "',101) AND CONVERT(DATETIME,'" + objDocumentEntry.ToDate + "',101))) " +
                                  " AND RepositoryID=@RepositoryID AND MetaTemplateID=@MetaTemplateID AND FolderID=@FolderID AND CategoryID=@CategoryID  AND MetaDataID=@MetaDataID  ";
                        }
                    }
                }

                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

                DbParameter[] objDbParameter = null;
                if (objMetaData.MetaDataID == -1)
                    objDbParameter = new DbParameter[6];
                else
                    objDbParameter = new DbParameter[7];

                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "RepositoryID";
                objDbParameter[0].Value = objMetaData.RepositoryID;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "MetaTemplateID";
                objDbParameter[1].Value = objMetaData.MetaTemplateID;

                objDbParameter[2] = objDbProviderFactory.CreateParameter();
                objDbParameter[2].ParameterName = "CategoryID";
                objDbParameter[2].Value = objMetaData.CategoryID;

                objDbParameter[3] = objDbProviderFactory.CreateParameter();
                objDbParameter[3].ParameterName = "FolderID";
                objDbParameter[3].Value = objMetaData.FolderID;

                objDbParameter[4] = objDbProviderFactory.CreateParameter();
                objDbParameter[4].ParameterName = "FieldID";
                objDbParameter[4].Value = objDocumentEntry.FieldID;

                objDbParameter[5] = objDbProviderFactory.CreateParameter();
                objDbParameter[5].ParameterName = "FieldData";

                if (enumSearchType == SearchType.EqualTo)
                    objDbParameter[5].Value = objDocumentEntry.FieldData.ToLower();
                else if (enumSearchType == SearchType.Like)
                    objDbParameter[5].Value = string.Format("%{0}%", objDocumentEntry.FieldData.ToLower());
                else if (enumSearchType == SearchType.DateRange)
                    objDbParameter[5] = null;

                if (objMetaData.MetaDataID != -1)
                {
                    objDbParameter[6] = objDbProviderFactory.CreateParameter();
                    objDbParameter[6].ParameterName = "MetaDataID";
                    objDbParameter[6].Value = objMetaData.MetaDataID;
                }


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



        public static Utility.ResultType SelectForTagSearch(out DataTable objDataTable, MetaData objMetaData, Document objDocument, SearchType enumSearchType)
        {
            try
            {

                string FolId = string.Empty;
                if (objMetaData.FolderID != 0)
                    Utility.FolderHasChild(objMetaData.FolderID, ref FolId);
                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;
                string strQuery = string.Empty;
                char charQuote = '"';
                if (objMetaData.CategoryID == -1)
                {
                    if (objMetaData.FolderID == 0)
                    {
                        if (objMetaData.MetaDataID == -1)
                        {
                            if (enumSearchType == SearchType.EqualTo)
                            {
                                strQuery = @" SELECT vwDocument.ID,MetaDataID,vwDocument.DocumentStatusID,vwDocument.DocumentPath,vwMetaData.MetaDataCode,DocumentName," + charQuote + "Size" + charQuote + ",DocumentType,Tag," +
                                            " (SELECT StatusName FROM vwDocumentStatus WHERE ID= vwDocument.DocumentStatusID ) AS " + charQuote + "DocumentStatus" + charQuote + " " +
                                            " FROM vwDocument INNER JOIN vwMetaData " +
                                            " ON vwDocument.MetaDataID = vwMetaData.ID " +
                                            " WHERE  lower(vwDocument.Tag) = @Tag " +
                                            " AND RepositoryID=@RepositoryID AND MetaTemplateID=@MetaTemplateID and vwDocument.Status=1";
                            }
                            else if (enumSearchType == SearchType.Like)
                            {
                                strQuery = @" SELECT vwDocument.ID,MetaDataID,vwDocument.DocumentStatusID,vwDocument.DocumentPath,vwMetaData.MetaDataCode,DocumentName," + charQuote + "Size" + charQuote + ",DocumentType,Tag," +
                                            " (SELECT StatusName FROM vwDocumentStatus WHERE ID= vwDocument.DocumentStatusID ) AS " + charQuote + "DocumentStatus" + charQuote + " " +
                                            " FROM vwDocument INNER JOIN vwMetaData " +
                                            " ON vwDocument.MetaDataID = vwMetaData.ID " +
                                            " WHERE  lower(vwDocument.Tag) LIKE @Tag " +
                                            " AND RepositoryID=@RepositoryID AND MetaTemplateID=@MetaTemplateID and vwDocument.Status=1";
                            }

                        }
                        else
                        {
                            if (enumSearchType == SearchType.EqualTo)
                            {
                                strQuery = @" SELECT vwDocument.ID,MetaDataID,vwDocument.DocumentStatusID,vwDocument.DocumentPath,vwMetaData.MetaDataCode,DocumentName," + charQuote + "Size" + charQuote + ",DocumentType,Tag," +
                                            " (SELECT StatusName FROM vwDocumentStatus WHERE ID= vwDocument.DocumentStatusID ) AS " + charQuote + "DocumentStatus" + charQuote + " " +
                                            " FROM vwDocument INNER JOIN vwMetaData " +
                                            " ON vwDocument.MetaDataID = vwMetaData.ID " +
                                            " WHERE lower(vwDocument.Tag) = @Tag " +
                                            " AND RepositoryID=@RepositoryID AND MetaTemplateID=@MetaTemplateID AND MetaDataID=@MetaDataID and vwDocument.Status=1";
                            }
                            else if (enumSearchType == SearchType.Like)
                            {
                                strQuery = @" SELECT vwDocument.ID,MetaDataID,vwDocument.DocumentStatusID,vwDocument.DocumentPath,vwMetaData.MetaDataCode,DocumentName," + charQuote + "Size" + charQuote + ",DocumentType,Tag," +
                                            " (SELECT StatusName FROM vwDocumentStatus WHERE ID= vwDocument.DocumentStatusID ) AS " + charQuote + "DocumentStatus" + charQuote + " " +
                                            " FROM vwDocument INNER JOIN vwMetaData " +
                                            " ON vwDocument.MetaDataID = vwMetaData.ID " +
                                            " WHERE lower(vwDocument.Tag) LIKE @Tag " +
                                            " AND RepositoryID=@RepositoryID AND MetaTemplateID=@MetaTemplateID AND MetaDataID=@MetaDataID and vwDocument.Status=1";
                            }
                        }
                    }
                    else
                    {
                        if (objMetaData.MetaDataID == -1)
                        {
                            if (enumSearchType == SearchType.EqualTo)
                            {
                                strQuery = @" SELECT vwDocument.ID,MetaDataID,vwDocument.DocumentStatusID,vwDocument.DocumentPath,vwMetaData.MetaDataCode,DocumentName," + charQuote + "Size" + charQuote + ",DocumentType,Tag," +
                                            " (SELECT StatusName FROM vwDocumentStatus WHERE ID= vwDocument.DocumentStatusID ) AS " + charQuote + "DocumentStatus" + charQuote + " " +
                                            " FROM vwDocument INNER JOIN vwMetaData " +
                                            " ON vwDocument.MetaDataID = vwMetaData.ID " +
                                            " WHERE lower(vwDocument.Tag) = @Tag " +
                                            " AND RepositoryID=@RepositoryID AND MetaTemplateID=@MetaTemplateID AND FolderID in (" + FolId + objMetaData.FolderID + ") and vwDocument.Status=1";
                            }
                            else if (enumSearchType == SearchType.Like)
                            {
                                strQuery = @" SELECT vwDocument.ID,MetaDataID,vwDocument.DocumentStatusID,vwDocument.DocumentPath,vwMetaData.MetaDataCode,DocumentName," + charQuote + "Size" + charQuote + ",DocumentType,Tag," +
                                            " (SELECT StatusName FROM vwDocumentStatus WHERE ID= vwDocument.DocumentStatusID ) AS " + charQuote + "DocumentStatus" + charQuote + " " +
                                            " FROM vwDocument INNER JOIN vwMetaData " +
                                            " ON vwDocument.MetaDataID = vwMetaData.ID " +
                                            " WHERE lower(vwDocument.Tag) LIKE @Tag " +
                                            " AND RepositoryID=@RepositoryID AND MetaTemplateID=@MetaTemplateID AND FolderID in (" + FolId + objMetaData.FolderID + ") and vwDocument.Status=1";
                            }

                        }
                        else
                        {
                            if (enumSearchType == SearchType.EqualTo)
                            {
                                strQuery = @" SELECT vwDocument.ID,MetaDataID,vwDocument.DocumentStatusID,vwDocument.DocumentPath,vwMetaData.MetaDataCode,DocumentName," + charQuote + "Size" + charQuote + ",DocumentType,Tag," +
                                            " (SELECT StatusName FROM vwDocumentStatus WHERE ID= vwDocument.DocumentStatusID ) AS " + charQuote + "DocumentStatus" + charQuote + " " +
                                            " FROM vwDocument INNER JOIN vwMetaData " +
                                            " ON vwDocument.MetaDataID = vwMetaData.ID " +
                                            " WHERE lower(vwDocument.Tag) = @Tag " +
                                            " AND RepositoryID=@RepositoryID AND MetaTemplateID=@MetaTemplateID AND FolderID in (" + FolId + objMetaData.FolderID + ") AND MetaDataID=@MetaDataID and vwDocument.Status=1";
                            }
                            else if (enumSearchType == SearchType.Like)
                            {
                                strQuery = @" SELECT vwDocument.ID,MetaDataID,vwDocument.DocumentStatusID,vwDocument.DocumentPath,vwMetaData.MetaDataCode,DocumentName," + charQuote + "Size" + charQuote + ",DocumentType,Tag," +
                                            " (SELECT StatusName FROM vwDocumentStatus WHERE ID= vwDocument.DocumentStatusID ) AS " + charQuote + "DocumentStatus" + charQuote + " " +
                                            " FROM vwDocument INNER JOIN vwMetaData " +
                                            " ON vwDocument.MetaDataID = vwMetaData.ID " +
                                            " WHERE lower(vwDocument.Tag) LIKE @Tag " +
                                            " AND RepositoryID=@RepositoryID AND MetaTemplateID=@MetaTemplateID AND FolderID in (" + FolId + objMetaData.FolderID + ") AND MetaDataID=@MetaDataID and vwDocument.Status=1";
                            }
                        }
                    }
                }
                else
                {
                    if (objMetaData.FolderID == 0)
                    {
                        if (objMetaData.MetaDataID == -1)
                        {
                            if (enumSearchType == SearchType.EqualTo)
                            {
                                strQuery = @" SELECT vwDocument.ID,MetaDataID,vwDocument.DocumentStatusID,vwDocument.DocumentPath,vwMetaData.MetaDataCode,DocumentName," + charQuote + "Size" + charQuote + ",DocumentType,Tag," +
                                            " (SELECT StatusName FROM vwDocumentStatus WHERE ID= vwDocument.DocumentStatusID ) AS " + charQuote + "DocumentStatus" + charQuote + " " +
                                            " FROM vwDocument INNER JOIN vwMetaData " +
                                            " ON vwDocument.MetaDataID = vwMetaData.ID " +
                                            " WHERE lower(vwDocument.Tag) = @Tag " +
                                            " AND RepositoryID=@RepositoryID AND MetaTemplateID=@MetaTemplateID AND CategoryID=@CategoryID and vwDocument.Status=1";
                            }
                            else if (enumSearchType == SearchType.Like)
                            {
                                strQuery = @" SELECT vwDocument.ID,MetaDataID,vwDocument.DocumentStatusID,vwDocument.DocumentPath,vwMetaData.MetaDataCode,DocumentName," + charQuote + "Size" + charQuote + ",DocumentType,Tag," +
                                            " (SELECT StatusName FROM vwDocumentStatus WHERE ID= vwDocument.DocumentStatusID ) AS " + charQuote + "DocumentStatus" + charQuote + " " +
                                            " FROM vwDocument INNER JOIN vwMetaData " +
                                            " ON vwDocument.MetaDataID = vwMetaData.ID " +
                                            " WHERE lower(vwDocument.Tag) LIKE @Tag " +
                                            " AND RepositoryID=@RepositoryID AND MetaTemplateID=@MetaTemplateID AND CategoryID=@CategoryID and vwDocument.Status=1";
                            }

                        }
                        else
                        {
                            if (enumSearchType == SearchType.EqualTo)
                            {
                                strQuery = @" SELECT vwDocument.ID,MetaDataID,vwDocument.DocumentStatusID,vwDocument.DocumentPath,vwMetaData.MetaDataCode,DocumentName," + charQuote + "Size" + charQuote + ",DocumentType,Tag," +
                                            " (SELECT StatusName FROM vwDocumentStatus WHERE ID= vwDocument.DocumentStatusID ) AS " + charQuote + "DocumentStatus" + charQuote + " " +
                                            " FROM vwDocument INNER JOIN vwMetaData " +
                                            " ON vwDocument.MetaDataID = vwMetaData.ID " +
                                            " WHERE lower(vwDocument.Tag) = @Tag " +
                                            " AND RepositoryID=@RepositoryID AND MetaTemplateID=@MetaTemplateID AND CategoryID=@CategoryID AND MetaDataID=@MetaDataID and vwDocument.Status=1";
                            }
                            else if (enumSearchType == SearchType.Like)
                            {
                                strQuery = @" SELECT vwDocument.ID,MetaDataID,vwDocument.DocumentStatusID,vwDocument.DocumentPath,vwMetaData.MetaDataCode,DocumentName," + charQuote + "Size" + charQuote + ",DocumentType,Tag," +
                                            " (SELECT StatusName FROM vwDocumentStatus WHERE ID= vwDocument.DocumentStatusID ) AS " + charQuote + "DocumentStatus" + charQuote + " " +
                                            " FROM vwDocument INNER JOIN vwMetaData " +
                                            " ON vwDocument.MetaDataID = vwMetaData.ID " +
                                            " WHERE lower(vwDocument.Tag) LIKE @Tag " +
                                            " AND RepositoryID=@RepositoryID AND MetaTemplateID=@MetaTemplateID AND CategoryID=@CategoryID AND MetaDataID=@MetaDataID and vwDocument.Status=1";
                            }
                        }
                    }
                    else
                    {
                        if (objMetaData.MetaDataID == -1)
                        {
                            if (enumSearchType == SearchType.EqualTo)
                            {
                                strQuery = @" SELECT vwDocument.ID,MetaDataID,vwDocument.DocumentStatusID,vwDocument.DocumentPath,vwMetaData.MetaDataCode,DocumentName," + charQuote + "Size" + charQuote + ",DocumentType,Tag," +
                                            " (SELECT StatusName FROM vwDocumentStatus WHERE ID= vwDocument.DocumentStatusID ) AS " + charQuote + "DocumentStatus" + charQuote + " " +
                                            " FROM vwDocument INNER JOIN vwMetaData " +
                                            " ON vwDocument.MetaDataID = vwMetaData.ID " +
                                            " WHERE lower(vwDocument.Tag) = @Tag " +
                                            " AND RepositoryID=@RepositoryID AND MetaTemplateID=@MetaTemplateID AND FolderID in (" + FolId + objMetaData.FolderID + ") AND CategoryID=@CategoryID and vwDocument.Status=1";
                            }
                            else if (enumSearchType == SearchType.Like)
                            {
                                strQuery = @" SELECT vwDocument.ID,MetaDataID,vwDocument.DocumentStatusID,vwDocument.DocumentPath,vwMetaData.MetaDataCode,DocumentName," + charQuote + "Size" + charQuote + ",DocumentType,Tag," +
                                            " (SELECT StatusName FROM vwDocumentStatus WHERE ID= vwDocument.DocumentStatusID ) AS " + charQuote + "DocumentStatus" + charQuote + " " +
                                            " FROM vwDocument INNER JOIN vwMetaData " +
                                            " ON vwDocument.MetaDataID = vwMetaData.ID " +
                                            " WHERE lower(vwDocument.Tag) LIKE @Tag " +
                                            " AND RepositoryID=@RepositoryID AND MetaTemplateID=@MetaTemplateID AND FolderID in (" + FolId + objMetaData.FolderID + ") AND CategoryID=@CategoryID and vwDocument.Status=1";
                            }

                        }
                        else
                        {
                            if (enumSearchType == SearchType.EqualTo)
                            {
                                strQuery = @" SELECT vwDocument.ID,MetaDataID,vwDocument.DocumentStatusID,vwDocument.DocumentPath,vwMetaData.MetaDataCode,DocumentName," + charQuote + "Size" + charQuote + ",DocumentType,Tag," +
                                            " (SELECT StatusName FROM vwDocumentStatus WHERE ID= vwDocument.DocumentStatusID ) AS " + charQuote + "DocumentStatus" + charQuote + " " +
                                            " FROM vwDocument INNER JOIN vwMetaData " +
                                            " ON vwDocument.MetaDataID = vwMetaData.ID " +
                                            " WHERE lower(vwDocument.Tag) = @Tag " +
                                            " AND RepositoryID=@RepositoryID AND MetaTemplateID=@MetaTemplateID AND FolderID in (" + FolId + objMetaData.FolderID + ") AND CategoryID=@CategoryID AND MetaDataID=@MetaDataID and vwDocument.Status=1";
                            }
                            else if (enumSearchType == SearchType.Like)
                            {
                                strQuery = @" SELECT vwDocument.ID,MetaDataID,vwDocument.DocumentStatusID,vwDocument.DocumentPath,vwMetaData.MetaDataCode,DocumentName," + charQuote + "Size" + charQuote + ",DocumentType,Tag," +
                                            " (SELECT StatusName FROM vwDocumentStatus WHERE ID= vwDocument.DocumentStatusID ) AS " + charQuote + "DocumentStatus" + charQuote + " " +
                                            " FROM vwDocument INNER JOIN vwMetaData " +
                                            " ON vwDocument.MetaDataID = vwMetaData.ID " +
                                            " WHERE lower(vwDocument.Tag) LIKE @Tag " +
                                            " AND RepositoryID=@RepositoryID AND MetaTemplateID=@MetaTemplateID AND FolderID in (" + FolId + objMetaData.FolderID + ") AND CategoryID=@CategoryID AND MetaDataID=@MetaDataID and vwDocument.Status=1";
                            }
                        }
                    }

                }



                DbParameter[] objDbParameter = null;

                //if (objMetaData.MetaDataID == -1 && objMetaData.CategoryID==-1)
                //    objDbParameter = new DbParameter[4];
                //else if (objMetaData.MetaDataID == -1 && objMetaData.CategoryID!=-1)
                //    objDbParameter = new DbParameter[5];
                //else if (objMetaData.MetaDataID != -1 && objMetaData.CategoryID == -1)
                //    objDbParameter = new DbParameter[5];
                //else if (objMetaData.MetaDataID != -1 && objMetaData.CategoryID != -1)
                objDbParameter = new DbParameter[5];
                //else
                //    objDbParameter = new DbParameter[6];

                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "RepositoryID";
                objDbParameter[0].Value = objMetaData.RepositoryID;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "MetaTemplateID";
                objDbParameter[1].Value = objMetaData.MetaTemplateID;

                objDbParameter[2] = objDbProviderFactory.CreateParameter();
                objDbParameter[2].ParameterName = "CategoryID";
                objDbParameter[2].Value = objMetaData.CategoryID;

                //objDbParameter[3] = objDbProviderFactory.CreateParameter();
                //objDbParameter[3].ParameterName = "FolderID";
                //objDbParameter[3].Value = FolId+objMetaData.FolderID;

                objDbParameter[3] = objDbProviderFactory.CreateParameter();
                objDbParameter[3].ParameterName = "Tag";
                if (enumSearchType == SearchType.EqualTo)
                    objDbParameter[3].Value = objDocument.Tag.ToLower().Trim();
                else
                    objDbParameter[3].Value = string.Format("%{0}%", objDocument.Tag.ToLower().Trim());


                if (objMetaData.MetaDataID != -1)
                {
                    objDbParameter[4] = objDbProviderFactory.CreateParameter();
                    objDbParameter[4].ParameterName = "MetaDataID";
                    objDbParameter[4].Value = objMetaData.MetaDataID;
                }


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

        public static Utility.ResultType SelectForTagSearchForDocPermission(out DataTable objDataTable, MetaData objMetaData, Document objDocument)
        {
            try
            {
                string strQuery = string.Empty;
                char charQuote = '"';
                if (objMetaData.MetaDataID == -1)
                {
                    strQuery = @" SELECT DOCUMENT.ID,MetaDataID,DOCUMENT.DocumentStatusID,METADATA.MetaDataCode,DocumentName," + charQuote + "Size" + charQuote + ",DocumentType,Tag," +
                                  " (SELECT StatusName FROM DOCUMENTSTATUS WHERE ID= DOCUMENT.DocumentStatusID ) AS " + charQuote + "DocumentStatus" + charQuote + " " +
                                  " FROM DOCUMENT INNER JOIN METADATA " +
                                  " ON DOCUMENT.MetaDataID = METADATA.ID " +
                                  " WHERE  LOWER(DOCUMENT.Tag) LIKE @Tag " +
                                  " AND RepositoryID=@RepositoryID AND MetaTemplateID=@MetaTemplateID AND FolderID=@FolderID AND CategoryID=@CategoryID";
                }
                else
                {
                    strQuery = @" SELECT DOCUMENT.ID,MetaDataID,DOCUMENT.DocumentStatusID,METADATA.MetaDataCode,DocumentName," + charQuote + "Size" + charQuote + ",DocumentType,Tag," +
                                   " (SELECT StatusName FROM DOCUMENTSTATUS WHERE ID= DOCUMENT.DocumentStatusID ) AS " + charQuote + "DocumentStatus" + charQuote + " " +
                                   " FROM DOCUMENT INNER JOIN METADATA " +
                                   " ON DOCUMENT.MetaDataID = METADATA.ID " +
                                   "WHERE  LOWER(DOCUMENT.Tag) LIKE @Tag " +
                                   " AND RepositoryID=@RepositoryID AND MetaTemplateID=@MetaTemplateID AND FolderID=@FolderID AND CategoryID=@CategoryID AND MetaDataID=@MetaDataID";
                }
                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

                DbParameter[] objDbParameter = null;
                if (objMetaData.MetaDataID == -1)
                    objDbParameter = new DbParameter[5];
                else
                    objDbParameter = new DbParameter[6];

                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "RepositoryID";
                objDbParameter[0].Value = objMetaData.RepositoryID;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "MetaTemplateID";
                objDbParameter[1].Value = objMetaData.MetaTemplateID;

                objDbParameter[2] = objDbProviderFactory.CreateParameter();
                objDbParameter[2].ParameterName = "CategoryID";
                objDbParameter[2].Value = objMetaData.CategoryID;

                objDbParameter[3] = objDbProviderFactory.CreateParameter();
                objDbParameter[3].ParameterName = "FolderID";
                objDbParameter[3].Value = objMetaData.FolderID;

                objDbParameter[4] = objDbProviderFactory.CreateParameter();
                objDbParameter[4].ParameterName = "Tag";
                objDbParameter[4].Value = string.Format("%{0}%", objDocument.Tag.ToLower());


                if (objMetaData.MetaDataID != -1)
                {
                    objDbParameter[5] = objDbProviderFactory.CreateParameter();
                    objDbParameter[5].ParameterName = "MetaDataID";
                    objDbParameter[5].Value = objMetaData.MetaDataID;
                }


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

        public static Utility.ResultType SelectForContentSearch(out DataTable objDataTable, MetaData objMetaData, string strDocumentID)
        {
            try
            {

                StringBuilder sbDocumentIDQuery = new StringBuilder();
                string strQueryFormat = " DOCUMENT.ID = {0} OR";

                foreach (string strID in strDocumentID.Split(','))
                {
                    sbDocumentIDQuery.Append(string.Format(strQueryFormat, strID));
                }

                string strQuery = string.Empty;
                char charQuote = '"';
                if (objMetaData.MetaDataID == -1)
                {
                    strQuery = @" SELECT DOCUMENT.ID,MetaDataID,DOCUMENT.DocumentStatusID,METADATA.MetaDataCode,DocumentName," + charQuote + "Size" + charQuote + ",DocumentType,Tag," +
                                " (SELECT StatusName FROM DOCUMENTSTATUS WHERE ID= DOCUMENT.DocumentStatusID ) AS " + charQuote + "DocumentStatus" + charQuote + " " +
                                " FROM DOCUMENT INNER JOIN METADATA " +
                                " ON DOCUMENT.MetaDataID = METADATA.ID " +
                                " WHERE (" + sbDocumentIDQuery.ToString().Trim('R', 'O') + ") " +
                                " AND RepositoryID=@RepositoryID AND MetaTemplateID=@MetaTemplateID AND FolderID=@FolderID AND CategoryID=@CategoryID";
                }
                else
                {
                    strQuery = @" SELECT DOCUMENT.ID,MetaDataID,DOCUMENT.DocumentStatusID,METADATA.MetaDataCode,DocumentName," + charQuote + "Size" + charQuote + ",DocumentType,Tag," +
                                " (SELECT StatusName FROM DOCUMENTSTATUS WHERE ID= DOCUMENT.DocumentStatusID ) AS " + charQuote + "DocumentStatus" + charQuote + " " +
                                " FROM DOCUMENT INNER JOIN METADATA " +
                                " ON DOCUMENT.MetaDataID = METADATA.ID " +
                                " WHERE  (" + sbDocumentIDQuery.ToString().Trim('R', 'O') + ") " +
                                " AND RepositoryID=@RepositoryID AND MetaTemplateID=@MetaTemplateID AND FolderID=@FolderID AND CategoryID=@CategoryID AND MetaDataID=@MetaDataID";
                }

                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

                DbParameter[] objDbParameter = null;
                if (objMetaData.MetaDataID == -1)
                    objDbParameter = new DbParameter[4];
                else
                    objDbParameter = new DbParameter[5];

                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "RepositoryID";
                objDbParameter[0].Value = objMetaData.RepositoryID;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "MetaTemplateID";
                objDbParameter[1].Value = objMetaData.MetaTemplateID;

                objDbParameter[2] = objDbProviderFactory.CreateParameter();
                objDbParameter[2].ParameterName = "CategoryID";
                objDbParameter[2].Value = objMetaData.CategoryID;

                objDbParameter[3] = objDbProviderFactory.CreateParameter();
                objDbParameter[3].ParameterName = "FolderID";
                objDbParameter[3].Value = objMetaData.FolderID;

                if (objMetaData.MetaDataID != -1)
                {
                    objDbParameter[4] = objDbProviderFactory.CreateParameter();
                    objDbParameter[4].ParameterName = "MetaDataID";
                    objDbParameter[4].Value = objMetaData.MetaDataID;
                }


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

        public static Utility.ResultType SelectByMetaDataForDropDown(out DataTable objDataTable, MetaData objMetaData)
        {
            try
            {
                string strQuery = "SELECT ID,MetaDataCode  FROM Metadata WHERE RepositoryID=@RepositoryID AND MetaTemplateID = @MetaTemplateID " +
                                  " AND FolderID=@FolderID AND Status=1 ORDER BY ID DESC";
                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

                DbParameter[] objDbParameter = new DbParameter[3];
                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "RepositoryID";
                objDbParameter[0].Value = objMetaData.RepositoryID;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "MetaTemplateID";
                objDbParameter[1].Value = objMetaData.MetaTemplateID;

                //objDbParameter[2] = objDbProviderFactory.CreateParameter();
                //objDbParameter[2].ParameterName = "CategoryID";
                //objDbParameter[2].Value = objMetaData.CategoryID;

                objDbParameter[2] = objDbProviderFactory.CreateParameter();
                objDbParameter[2].ParameterName = "FolderID";
                objDbParameter[2].Value = objMetaData.FolderID;

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

        public static Utility.ResultType Update(int Status, int ID, DbTransaction objDbTransaction)
        {
            DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

            try
            {
                DbParameter[] objDbParameter = new DbParameter[2];

                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "Status";
                objDbParameter[0].Value = Status;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "ID";
                objDbParameter[1].Value = ID;


                DataHelper.ExecuteNonQuery("Sp_Update", objDbTransaction, objDbParameter);

                //objMetaData.MetaDataID = Convert.ToInt32(objDbParameter[1].Value);
                //objMetaData.MetaDataCode = objDbParameter[1].Value.ToString();

                return Utility.ResultType.Success;
            }
            catch (Exception ex)
            {

                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return Utility.ResultType.Error;
            }
        }

        #endregion

        public DataTable GetMetadatadetails()
        {
            DataTable dt = new DataTable();

            try
            {
                SqlConnection con = new SqlConnection(Utility.ConnectionString);
                if (con.State != ConnectionState.Open)
                    con.Open();
                SqlCommand cmd = new SqlCommand(@"SELECT METADATA.RepositoryID,METADATA.MetaTemplateID,DOCUMENT.ID,MetaDataID,DOCUMENT.DocumentStatusID,METADATA.MetaDataCode,
                DocumentName,  Size  , DocumentType,Tag, (SELECT StatusName FROM DOCUMENTSTATUS WHERE ID= DOCUMENT.DocumentStatusID ) AS   DocumentStatus FROM DOCUMENT INNER JOIN METADATA ON DOCUMENT.MetaDataID = METADATA.ID  
                WHERE 
               METADATA.RepositoryID='" + RepositoryID + "' AND METADATA.MetaTemplateID='" + MetaTemplateID + "' AND METADATA.FolderID='" + FolderID + "' AND METADATA.CategoryID='" + CategoryID + "'", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {

            }
            return dt;
        }

        public static Utility.ResultType SelectAll(out DataTable objDataTable, MetaData objMetaData, Document objDocument)
        {
            try
            {
                string strQuery = string.Empty;
                char charQuote = '"';
                if (objMetaData.MetaDataID == -1)
                {
                    strQuery = @" SELECT DOCUMENT.ID,MetaDataID,DOCUMENT.DocumentStatusID,METADATA.MetaDataCode,DocumentName," + charQuote + "Size" + charQuote + ",DocumentType,Tag," +
                                    " (SELECT StatusName FROM DOCUMENTSTATUS WHERE ID= DOCUMENT.DocumentStatusID ) AS " + charQuote + "DocumentStatus" + charQuote + " " +
                                    " FROM DOCUMENT INNER JOIN METADATA " +
                                    " ON DOCUMENT.MetaDataID = METADATA.ID " +
                                    " WHERE RepositoryID=@RepositoryID AND MetaTemplateID=@MetaTemplateID AND FolderID=@FolderID AND CategoryID=@CategoryID";

                }
                else
                {
                    strQuery = @" SELECT DOCUMENT.ID,MetaDataID,DOCUMENT.DocumentStatusID,METADATA.MetaDataCode,DocumentName," + charQuote + "Size" + charQuote + ",DocumentType,Tag," +
                                    " (SELECT StatusName FROM DOCUMENTSTATUS WHERE ID= DOCUMENT.DocumentStatusID ) AS " + charQuote + "DocumentStatus" + charQuote + " " +
                                    " FROM DOCUMENT INNER JOIN METADATA " +
                                    " ON DOCUMENT.MetaDataID = METADATA.ID " +
                                    " WHERE  RepositoryID=@RepositoryID AND MetaTemplateID=@MetaTemplateID AND FolderID=@FolderID AND CategoryID=@CategoryID AND MetaDataID=@MetaDataID";
                }

                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

                DbParameter[] objDbParameter = null;
                if (objMetaData.MetaDataID == -1)
                    objDbParameter = new DbParameter[4];
                else
                    objDbParameter = new DbParameter[5];

                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "RepositoryID";
                objDbParameter[0].Value = objMetaData.RepositoryID;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "MetaTemplateID";
                objDbParameter[1].Value = objMetaData.MetaTemplateID;

                objDbParameter[2] = objDbProviderFactory.CreateParameter();
                objDbParameter[2].ParameterName = "CategoryID";
                objDbParameter[2].Value = objMetaData.CategoryID;

                objDbParameter[3] = objDbProviderFactory.CreateParameter();
                objDbParameter[3].ParameterName = "FolderID";
                objDbParameter[3].Value = objMetaData.FolderID;

                if (objMetaData.MetaDataID != -1)
                {
                    objDbParameter[4] = objDbProviderFactory.CreateParameter();
                    objDbParameter[4].ParameterName = "MetaDataID";
                    objDbParameter[4].Value = objMetaData.MetaDataID;
                }


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

        public static Utility.ResultType SelectAllForDocPermission(out DataTable objDataTable, MetaData objMetaData, Document objDocument)
        {
            try
            {
                string strQuery = string.Empty;
                char charQuote = '"';
                if (objMetaData.MetaDataID == -1)
                {
                    strQuery = @" SELECT DOCUMENT.ID,MetaDataID,DOCUMENT.DocumentStatusID,METADATA.MetaDataCode,DocumentName," + charQuote + "Size" + charQuote + ",DocumentType,Tag," +
                                    " (SELECT StatusName FROM DOCUMENTSTATUS WHERE ID= DOCUMENT.DocumentStatusID ) AS " + charQuote + "DocumentStatus" + charQuote + " " +
                                    " FROM DOCUMENT INNER JOIN METADATA " +
                                    " ON DOCUMENT.MetaDataID = METADATA.ID " +
                                    " WHERE RepositoryID=@RepositoryID AND MetaTemplateID=@MetaTemplateID AND FolderID=@FolderID AND CategoryID=@CategoryID";

                }
                else
                {
                    strQuery = @" SELECT DOCUMENT.ID,MetaDataID,DOCUMENT.DocumentStatusID,METADATA.MetaDataCode,DocumentName," + charQuote + "Size" + charQuote + ",DocumentType,Tag," +
                                    " (SELECT StatusName FROM DOCUMENTSTATUS WHERE ID= DOCUMENT.DocumentStatusID ) AS " + charQuote + "DocumentStatus" + charQuote + " " +
                                    " FROM DOCUMENT INNER JOIN METADATA " +
                                    " ON DOCUMENT.MetaDataID = METADATA.ID " +
                                    " WHERE  RepositoryID=@RepositoryID AND MetaTemplateID=@MetaTemplateID AND FolderID=@FolderID AND CategoryID=@CategoryID AND MetaDataID=@MetaDataID";
                }

                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

                DbParameter[] objDbParameter = null;
                if (objMetaData.MetaDataID == -1)
                    objDbParameter = new DbParameter[4];
                else
                    objDbParameter = new DbParameter[5];

                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "RepositoryID";
                objDbParameter[0].Value = objMetaData.RepositoryID;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "MetaTemplateID";
                objDbParameter[1].Value = objMetaData.MetaTemplateID;

                objDbParameter[2] = objDbProviderFactory.CreateParameter();
                objDbParameter[2].ParameterName = "CategoryID";
                objDbParameter[2].Value = objMetaData.CategoryID;

                objDbParameter[3] = objDbProviderFactory.CreateParameter();
                objDbParameter[3].ParameterName = "FolderID";
                objDbParameter[3].Value = objMetaData.FolderID;

                if (objMetaData.MetaDataID != -1)
                {
                    objDbParameter[4] = objDbProviderFactory.CreateParameter();
                    objDbParameter[4].ParameterName = "MetaDataID";
                    objDbParameter[4].Value = objMetaData.MetaDataID;
                }


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

        public static DataSet SelectRepName(int MetadataID)
        {
            try
            {

                string strquery = "select R.Id,R.RepositoryName from Repository R inner join MetaData M on M.RepositoryID=R.ID where M.ID="+MetadataID+"";
                DataSet ds = new DataSet();
                ds = DataHelper.ExecuteDataSet(strquery);
                return ds;
            }
            catch(Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
               
                return null;
            }
        }

        public static DataSet SelectMetatemplateName(int MetadataID)
        {
            try
            {

                string strquery = "select R.RepositoryName from Repository R inner join MetaData M on M.RepositoryID=R.ID where M.ID=" + MetadataID + "";
                DataSet ds = new DataSet();
                ds = DataHelper.ExecuteDataSet(strquery);
                return ds;
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);

                return null;
            }
        }
        public static Utility.ResultType SelectForFolderSearch(out DataTable objDataTable, MetaData objMetaData)
        {
            DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;
            // DbTransaction objDbTransaction = Utility.GetTransaction;

            try
            {
                DbParameter[] objDbParameter = new DbParameter[1];

                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "FolderId";
                objDbParameter[0].Value = objMetaData.FolderID;
                //DataTable dt = new DataTable();
                //string 
                //  DataHelper.ExecuteScalarForProc("SP_S_SearchByFolder", objDbParameter);
                objDataTable = DataHelper.ExecuteDataTableForProcedure("SP_S_SearchByFolder", null, objDbParameter);
                //DataHelper.ExecuteNonQuery("U_DOC_INMETADATAID", objDbTransaction, objDbParameter);

                if (objDataTable.Rows.Count == 0)
                {
                    return Utility.ResultType.Failure;
                }
                return Utility.ResultType.Success;
            }
            catch (Exception ex)
            {

                LogManager.ErrorLog(Utility.LogFilePath, ex);
                //dt = null;
                // return dt;
                objDataTable = null;
                return Utility.ResultType.Failure;
            }
            finally
            {

            }
        }

        public static Utility.ResultType DocumentSearch_New(out DataTable objDataTable, out int RecordCount, MetaData objMetaData,
            Document objDocument, SearchType enumSearchType, int PageIndex,int PageSize)
        {
            RecordCount = 0;
            try
            {
                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

                DbParameter[] objDbParameter = null;
                objDbParameter = new DbParameter[11];

                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "RepositoryID";
                objDbParameter[0].Value = objMetaData.RepositoryID;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "MetaTemplateID";
                objDbParameter[1].Value = objMetaData.MetaTemplateID;

                objDbParameter[2] = objDbProviderFactory.CreateParameter();
                objDbParameter[2].ParameterName = "CategoryID";
                objDbParameter[2].Value = objMetaData.CategoryID;

                objDbParameter[3] = objDbProviderFactory.CreateParameter();
                objDbParameter[3].ParameterName = "FolderID";
                objDbParameter[3].Value = objMetaData.FolderID;

                objDbParameter[4] = objDbProviderFactory.CreateParameter();
                objDbParameter[4].ParameterName = "FromDate";
                objDbParameter[4].Value = null;

                objDbParameter[5] = objDbProviderFactory.CreateParameter();
                objDbParameter[5].ParameterName = "ToDate";
                objDbParameter[5].Value = null;

                objDbParameter[6] = objDbProviderFactory.CreateParameter();
                objDbParameter[6].ParameterName = "SearchText";

                if (enumSearchType == SearchType.Exact_word)
                    objDbParameter[6].Value = objDocument.Tag.ToLower();
                else if (enumSearchType == SearchType.Contains)
                    objDbParameter[6].Value = string.Format("%{0}%", objDocument.Tag.ToLower());
                else if (enumSearchType == SearchType.Begins_with)
                    objDbParameter[6].Value = string.Format("{0}%", objDocument.Tag.ToLower());
                else if (enumSearchType == SearchType.Ends_with)
                    objDbParameter[6].Value = string.Format("%{0}", objDocument.Tag.ToLower());
                else if (enumSearchType == SearchType.Any_of_these_words)
                    objDbParameter[6].Value = string.Format("%{0}%", objDocument.Tag.ToLower());

                objDbParameter[7] = objDbProviderFactory.CreateParameter();
                objDbParameter[7].ParameterName = "IsRecursive";
                objDbParameter[7].Value = objMetaData.IsRecursive;

                objDbParameter[8] = objDbProviderFactory.CreateParameter();
                objDbParameter[8].ParameterName = "PageIndex";
                objDbParameter[8].Value = PageIndex;

                objDbParameter[9] = objDbProviderFactory.CreateParameter();
                objDbParameter[9].ParameterName = "PageSize";
                objDbParameter[9].Value = PageSize;

                objDbParameter[10] = objDbProviderFactory.CreateParameter();
                objDbParameter[10].ParameterName = "RecordCount";
                objDbParameter[10].Size = 50;
                objDbParameter[10].Direction = ParameterDirection.Output;

                objDataTable = DataHelper.ExecuteDataTableForProcedure("proc_DocumentSearch", null, objDbParameter);

                RecordCount = Convert.ToInt32(objDbParameter[10].Value);
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


        public static Utility.ResultType SelectForContentOptFolderSearch(out DataTable objDataTable, MetaData objMetaData, string strDocumentID)
        {
            try
            {
                StringBuilder sbDocumentIDQuery = new StringBuilder();
                string strQueryFormat = " vwDocument.ID = {0} OR";

                foreach (string strID in strDocumentID.Split(','))
                {
                    sbDocumentIDQuery.Append(string.Format(strQueryFormat, strID));
                }

                string strQuery = string.Empty;
                char charQuote = '"';
                if (objMetaData.MetaDataID == -1)
                {
                    if (objMetaData.FolderID != 0)
                    {
                        strQuery = @" SELECT vwDocument.ID,MetaDataID,vwDocument.DocumentPath,vwDocument.DocumentStatusID,vwMetaData.MetaDataCode,DocumentName," + charQuote + "Size" + charQuote + ",DocumentType,Tag," +
                                    " (SELECT StatusName FROM vwDocumentStatus WHERE ID= vwDocument.DocumentStatusID ) AS " + charQuote + "DocumentStatus" + charQuote + " " +
                                    " FROM vwDocument INNER JOIN vwMetaData " +
                                    " ON vwDocument.MetaDataID = vwMetaData.ID " +

                                    " WHERE (" + sbDocumentIDQuery.ToString().Trim('R', 'O') + ") " +
                                    " AND vwMetaData.RepositoryID=@RepositoryID  AND MetaTemplateID=@MetaTemplateID AND FolderID=@FolderID AND CategoryID=@CategoryID AND vwDocument.DocumentStatusID not in(2)";
                    }
                    else
                    {
                        strQuery = @" SELECT vwDocument.ID,MetaDataID,vwDocument.DocumentPath,vwDocument.DocumentStatusID,vwMetaData.MetaDataCode,DocumentName," + charQuote + "Size" + charQuote + ",DocumentType,Tag," +
                                   " (SELECT StatusName FROM vwDocumentStatus WHERE ID= vwDocument.DocumentStatusID ) AS " + charQuote + "DocumentStatus" + charQuote + " " +
                                   " FROM vwDocument INNER JOIN vwMetaData " +
                                   " ON vwDocument.MetaDataID = vwMetaData.ID " +

                                   " WHERE (" + sbDocumentIDQuery.ToString().Trim('R', 'O') + ") " +
                                   " AND vwMetaData.RepositoryID=@RepositoryID  AND MetaTemplateID=@MetaTemplateID AND CategoryID=@CategoryID AND vwDocument.DocumentStatusID not in(2)";
                    }
                }
                else
                {
                    if (objMetaData.FolderID != 0)
                    {
                        strQuery = @" SELECT vwDocument.ID,MetaDataID,vwDocument.DocumentPath,vwDocument.DocumentStatusID,vwMetaData.MetaDataCode,DocumentName," + charQuote + "Size" + charQuote + ",DocumentType,Tag," +
                                    " (SELECT StatusName FROM vwDocumentStatus WHERE ID= vwDocument.DocumentStatusID ) AS " + charQuote + "DocumentStatus" + charQuote + " " +
                                    " FROM vwDocument INNER JOIN vwMetaData " +
                                    " ON vwDocument.MetaDataID = vwMetaData.ID " +

                                    " WHERE  (" + sbDocumentIDQuery.ToString().Trim('R', 'O') + ") " +
                                    " AND vwMetaData.RepositoryID=@RepositoryID  AND MetaTemplateID=@MetaTemplateID AND FolderID=@FolderID AND CategoryID=@CategoryID AND MetaDataID=@MetaDataID AND vwDocument.DocumentStatusID not in(2)";
                    }
                    else
                    {

                        strQuery = @" SELECT vwDocument.ID,MetaDataID,vwDocument.DocumentPath,vwDocument.DocumentStatusID,vwMetaData.MetaDataCode,DocumentName," + charQuote + "Size" + charQuote + ",DocumentType,Tag," +
                                          " (SELECT StatusName FROM vwDocumentStatus WHERE ID= vwDocument.DocumentStatusID ) AS " + charQuote + "DocumentStatus" + charQuote + " " +
                                          " FROM vwDocument INNER JOIN vwMetaData " +
                                          " ON vwDocument.MetaDataID = vwMetaData.ID " +

                                          " WHERE  (" + sbDocumentIDQuery.ToString().Trim('R', 'O') + ") " +
                                          " AND vwMetaData.RepositoryID=@RepositoryID AND MetaTemplateID=@MetaTemplateID AND CategoryID=@CategoryID AND MetaDataID=@MetaDataID AND vwDocument.DocumentStatusID not in(2)";
                    }
                }

                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

                DbParameter[] objDbParameter = null;
                if (objMetaData.MetaDataID == -1)
                    objDbParameter = new DbParameter[4];
                else
                    objDbParameter = new DbParameter[5];

                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "RepositoryID";
                objDbParameter[0].Value = objMetaData.RepositoryID;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "MetaTemplateID";
                objDbParameter[1].Value = objMetaData.MetaTemplateID;

                objDbParameter[2] = objDbProviderFactory.CreateParameter();
                objDbParameter[2].ParameterName = "CategoryID";
                objDbParameter[2].Value = objMetaData.CategoryID;
                if (objMetaData.FolderID != 0)
                {
                    objDbParameter[3] = objDbProviderFactory.CreateParameter();
                    objDbParameter[3].ParameterName = "FolderID";
                    objDbParameter[3].Value = objMetaData.FolderID;

                    if (objMetaData.MetaDataID != -1)
                    {
                        objDbParameter[4] = objDbProviderFactory.CreateParameter();
                        objDbParameter[4].ParameterName = "MetaDataID";
                        objDbParameter[4].Value = objMetaData.MetaDataID;
                    }
                }
                else
                {
                    if (objMetaData.MetaDataID != -1)
                    {
                        objDbParameter[3] = objDbProviderFactory.CreateParameter();
                        objDbParameter[3].ParameterName = "MetaDataID";
                        objDbParameter[3].Value = objMetaData.MetaDataID;
                    }
                }
                //objDbParameter[objDbParameter.Length - 1] = objDbProviderFactory.CreateParameter();
                //objDbParameter[objDbParameter.Length - 1].ParameterName = "DeptID";
                //objDbParameter[objDbParameter.Length - 1].Value = objMetaData.DepartmentID;
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

    }

}
