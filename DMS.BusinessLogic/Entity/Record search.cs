using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data;
using System.Data.SqlClient;

namespace DMS.BusinessLogic.Entity
{
    #region Enum
    public enum SearchType { EqualTo, Like, DateRange };
    public enum DashBoard { RepositoryLevel, MetaTemplateLevel, MetaDataLevel, DocumentStatusLevel }
    #endregion
   // #region Properties


    class Record_search
    {


        //        public static Utility.ResultType SelectForFieldSearch(out DataTable objDataTable, MetaData objMetaData, DocumentEntry objDocumentEntry, SearchType enumSearchType)
        //        {
        //            try
        //            {
        //                string strQuery = string.Empty;
        //                char charQuote = '"';
        //                if (objMetaData.MetaDataID == -1)
        //                {
        //                    if (enumSearchType == SearchType.EqualTo)
        //                    {
        //                        strQuery=@"SELECT vwMetaData.RepositoryID,vwMetaData.MetaTemplateID,vwDocument.ID,MetaDataID,vwDocument.DocumentStatusID,vwMetaData.MetaDataCode,DocumentName, Size  ,DocumentType,Tag, (SELECT StatusName FROM vwDocumentStatus WHERE ID= vwDocument.DocumentStatusID ) AS   DocumentStatus    FROM vwDocument INNER JOIN vwMetaData  ON vwDocument.MetaDataID = vwMetaData.ID  WHERE   vwMetaData.RepositoryID=@RepositoryID AND vwMetaData.MetaTemplateID=@MetaTemplateID AND vwMetaData.FolderID=@FolderID AND vwMetaData.CategoryID=@CategoryID";
        //    }
    }
}
            