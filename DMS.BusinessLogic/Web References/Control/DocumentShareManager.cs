using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
namespace DMS.BusinessLogic
{
    public class DocumentShareManager
    {
        #region Private Member
        Utility objUtility = new Utility();
        #endregion


        #region Method
        public Utility.ResultType InsertDocumentShare(DocumentShare objDocumentShare)
        {
            try
            {
                return DocumentShare.InsertDocumentShare(objDocumentShare);
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return Utility.ResultType.Error;
            }
        }

        public static DataSet CheckInternalUser(int UserID)
        {
            try
            {

                string strQuery = @"select * from Document_Share where UserID="+UserID;
                DataSet ds = new DataSet();
                ds = DataHelper.ExecuteDataSet(strQuery);
                return ds;
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return null;
            }
        }

        public static DataSet CheckExternalUser(string Email)
        {
            try
            {

                string strQuery = @"select * from Document_Share where EmailId='"+Email+"'";
                DataSet ds = new DataSet();
                ds = DataHelper.ExecuteDataSet(strQuery);
                return ds;
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return null;
            }
        }



        public Utility.ResultType InsertDocumentShareDetails(DocumentShare objDocumentShare)
        {
            try
            {
                return DocumentShare.InsertDocumentShareDetails(objDocumentShare);
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return Utility.ResultType.Error;
            }
        }

        public Utility.ResultType GetDocumentByDocumentShareID(out DataTable dt, int FolderID)
        {
            return DocumentShare.GetDocumentByDocumentShareID(out dt, FolderID);
        }


        public Utility.ResultType FolderWiseDocumentBind(out DataTable dt, int FolderID)
        {
            return DocumentShare.FolderWiseDocumentBind(out dt, FolderID);
        }

       
        #endregion
    }
}
