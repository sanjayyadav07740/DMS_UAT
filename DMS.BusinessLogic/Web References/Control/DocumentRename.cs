using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DMS.BusinessLogic;
using System.Data.Common;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
namespace DMS.BusinessLogic
{
    public class DocumentRename1
    {
        public string NewDocumentName { get; set; }
        public string OldDocumentName { get; set; }
        public int DocumentID { get; set; }
        public int CreatedBy { get; set; }
        public DateTime createdOn { get; set; }
        public string Tag { get; set; }
        public static string Extention { get; set; }


       
        public static Utility.ResultType DocumentRenameInsert(DocumentRename1 ObjDocument, DbTransaction objDbTransaction)
        {
            DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

            try
            {

                DbParameter[] objDbParameter = new DbParameter[7];

                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "NewDocumentName";
                objDbParameter[0].Value = ObjDocument.NewDocumentName;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "OldDocumentName";
                objDbParameter[1].Value = ObjDocument.OldDocumentName;

                objDbParameter[2] = objDbProviderFactory.CreateParameter();
                objDbParameter[2].ParameterName = "DocumentID";
                objDbParameter[2].Value = ObjDocument.DocumentID;

                objDbParameter[3] = objDbProviderFactory.CreateParameter();
                objDbParameter[3].ParameterName = "CreatedBy";
                objDbParameter[3].Value = ObjDocument.CreatedBy;

                objDbParameter[4] = objDbProviderFactory.CreateParameter();
                objDbParameter[4].ParameterName = "createdOn";
                objDbParameter[4].Value = ObjDocument.createdOn;

                objDbParameter[5] = objDbProviderFactory.CreateParameter();
                objDbParameter[5].ParameterName = "Tag";
                objDbParameter[5].Value = ObjDocument.Tag;

                objDbParameter[6] = objDbProviderFactory.CreateParameter();
                objDbParameter[6].ParameterName = "@DocumentRenameID";
                objDbParameter[6].Size = 50;
                objDbParameter[6].Direction = ParameterDirection.Output;

                DataHelper.ExecuteNonQuery("Rename_SP_Document", objDbTransaction, objDbParameter);
                int DocumentRenameID = 0;
                DocumentRenameID = Convert.ToInt32(objDbParameter[6].Value);
               // ObjDocument.NewDocumentName = objDbParameter[7].Value.ToString();

                return Utility.ResultType.Success;
            }
            catch (Exception ex)
            {

                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return Utility.ResultType.Error;
            }
        }

        public static Utility.ResultType DocumentRename(DocumentRename1 ObjDocument, DbTransaction objDbTransaction)
        {
            DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

            try
            {

                DbParameter[] objDbParameter = new DbParameter[7];

                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "NewDocumentName";
                objDbParameter[0].Value = ObjDocument.NewDocumentName;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "OldDocumentName";
                objDbParameter[1].Value = ObjDocument.OldDocumentName;

                objDbParameter[2] = objDbProviderFactory.CreateParameter();
                objDbParameter[2].ParameterName = "DocumentID";
                objDbParameter[2].Value = ObjDocument.DocumentID;

                objDbParameter[3] = objDbProviderFactory.CreateParameter();
                objDbParameter[3].ParameterName = "CreatedBy";
                objDbParameter[3].Value = ObjDocument.CreatedBy;

                objDbParameter[4] = objDbProviderFactory.CreateParameter();
                objDbParameter[4].ParameterName = "createdOn";
                objDbParameter[4].Value = ObjDocument.createdOn;

                objDbParameter[5] = objDbProviderFactory.CreateParameter();
                objDbParameter[5].ParameterName = "Tag";
                objDbParameter[5].Value = ObjDocument.Tag;

                objDbParameter[6] = objDbProviderFactory.CreateParameter();
                objDbParameter[6].ParameterName = "@DocumentRenameID";
                objDbParameter[6].Size = 50;
                objDbParameter[6].Direction = ParameterDirection.Output;

                DataHelper.ExecuteNonQuery("SP_RenameDocument", objDbTransaction, objDbParameter);
                int DocumentRenameID = 0;
                DocumentRenameID = Convert.ToInt32(objDbParameter[6].Value);
                // ObjDocument.NewDocumentName = objDbParameter[7].Value.ToString();

                return Utility.ResultType.Success;
            }
            catch (Exception ex)
            {

                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return Utility.ResultType.Error;
            }
        }

        public static int updateDocument(DocumentRename1 ObjDocument , SqlConnection newConn, SqlTransaction Newtrans)
        {
          
           int RowAffected = 0;
           SqlTransaction trans = Newtrans;
            SqlConnection conn = newConn;
            try
            {
               // using ( conn = new SqlConnection(Utility.DHSConnectionString))
                {
                 
                    using (SqlCommand cmd = new SqlCommand("DocumentRenameFromDMS", conn,trans))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@DocumentID", ObjDocument.DocumentID);
                        cmd.Parameters.AddWithValue("@NewDocumentName", ObjDocument.NewDocumentName);
                        cmd.Parameters.AddWithValue("@DocumentName", ObjDocument.OldDocumentName);
                        if (conn.State != ConnectionState.Open)
                        {
                            conn.Open();
                        }
                       RowAffected= cmd.ExecuteNonQuery();

                       if (RowAffected > 0)
                       {
                         //  trans.Commit();
                       }
                        //conn.Close();
                        cmd.Dispose();
                    }
                }
               
            }
            catch (Exception ex)
            {
                trans.Rollback();
                conn.Close();
               
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                
            }
            return RowAffected;
        }


      
    }  
}
