using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;

namespace DMS.BusinessLogic
{
    public class Report
    {
        public static Utility.ResultType Select(out DataTable objDataTable)
        {
            try
            {
               // for first report
                string strQuery = "select d.DocumentName as DocumentName, u.UserName as CreatedUser,z.UserName as UpdatedUser,ds.StatusName as DocStatus from vwDocument d left outer Join  vwUser u on d.CreatedBy=u.ID left outer Join vwUser z on d.UpdatedBy=z.ID left outer Join vwDocumentStatus ds on d.DocumentStatusID=ds.ID";

               // string strQuery = "select ds.StatusName as DocStatus from vwDocument d left outer join DocumentStatus ds on d.DocumentStatusID=ds.ID";
               
                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;


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
        public void InsertAuditLog(string IPAddress,string MacAddress,string Activity,string DocumentName,int UserId)
        {
            DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;
            DbTransaction objDbTransaction = Utility.GetTransaction;
            try
            {
                DbParameter[] objDbParameter = new DbParameter[5];

                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "@IPAddress";
                objDbParameter[0].Value = IPAddress;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "@MacAddress";
                objDbParameter[1].Value = MacAddress;

                //objDbParameter[2] = objDbProviderFactory.CreateParameter();
                //objDbParameter[2].ParameterName = "@DateOfActivity";
                //objDbParameter[2].Value = DateOfActivity;

                objDbParameter[2] = objDbProviderFactory.CreateParameter();
                objDbParameter[2].ParameterName = "@Activity";
                objDbParameter[2].Value = Activity;

                objDbParameter[3] = objDbProviderFactory.CreateParameter();
                objDbParameter[3].ParameterName = "@DocumentName";

                //if (DocumentName != null)
                    objDbParameter[3].Value = DocumentName;
                //else
                //    objDbParameter[3].Value = null;

                objDbParameter[4] = objDbProviderFactory.CreateParameter();
                objDbParameter[4].ParameterName = "@UserId";
                objDbParameter[4].Value = UserId;

                DataHelper.ExecuteNonQuery("SP_I_AuditLog", objDbTransaction, objDbParameter);
                objDbTransaction.Commit();
            }
            catch (Exception ex)
            {
                objDbTransaction.Rollback();
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                //return Utility.ResultType.Error;
            }
        }

        public void UpdateAuditLog(DateTime LogoutTime,string IpAddress)
        {
            DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;
            DbTransaction objDbTransaction = Utility.GetTransaction;
            try
            {
                DbParameter[] objDbParameter = new DbParameter[2];

                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "@Logout";
                objDbParameter[0].Value = LogoutTime;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "@IPAddress";
                objDbParameter[1].Value = IpAddress;

                DataHelper.ExecuteNonQuery("SP_U_AuditLog", objDbTransaction, objDbParameter);
                objDbTransaction.Commit();
            }
            catch (Exception ex)
            {
                objDbTransaction.Rollback();
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                //return Utility.ResultType.Error;
            }
        }

        public static DataSet SelectAuditLog(int RepositoryID)
        {
            DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;
            DbTransaction objDbTransaction = Utility.GetTransaction;
            string strQuery="";

                
//                string strQuery = @"select IPAddress,CONVERT(date,LoginDate)as LoginDate,CONVERT(varchar(8),LoginTime,108) as LoginTime,
//                                    CONVERT(varchar(8),LogOutTime,108) as LogOutTime from AuditLog;";
            if (RepositoryID != 0)
            {
                strQuery = @"select distinct AL.IPAddress,AL.MacAddress,AL.DateOfActivity,AL.Activity,AL.DocumentName,U.UserName 
                                    from AuditLog AL inner join  vwUser U 
                                    on AL.UserID=U.ID inner join Role R
                                    on U.RoleID=R.ID inner join RolePermission RP
                                    on R.ID=RP.RoleID inner join Document D
                                    on AL.DocumentName=D.DocumentName inner join MetaData M
                                    on D.MetaDataID=M.ID
                                    where RP.RepositoryID='" + RepositoryID + "' and M.RepositoryID='" + RepositoryID + "'";
            }
            else
            {
                strQuery = @"select distinct AL.IPAddress,AL.MacAddress,AL.DateOfActivity,AL.Activity,AL.DocumentName,U.UserName 
                                    from AuditLog AL inner join  vwUser U 
                                    on AL.UserID=U.ID inner join Role R
                                    on U.RoleID=R.ID inner join RolePermission RP
                                    on R.ID=RP.RoleID inner join Document D
                                    on AL.DocumentName=D.DocumentName inner join MetaData M
                                    on D.MetaDataID=M.ID";
            }

                DataSet ds = new DataSet();
                ds = DataHelper.ExecuteDataSet(strQuery);
                return ds;
        }

        public static DataSet SelectAuditLogUser(int UserID)
        {
            DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;
            DbTransaction objDbTransaction = Utility.GetTransaction;
            string strQuery = "";


            //                string strQuery = @"select IPAddress,CONVERT(date,LoginDate)as LoginDate,CONVERT(varchar(8),LoginTime,108) as LoginTime,
            //                                    CONVERT(varchar(8),LogOutTime,108) as LogOutTime from AuditLog;";

            strQuery = @"select distinct AL.IPAddress,AL.MacAddress,AL.DateOfActivity,AL.Activity,AL.DocumentName,U.UserName 
                         from AuditLog AL inner join  vwUser U 
                         on AL.UserID=U.ID where AL.UserID='" + UserID + "'";
           

            DataSet ds = new DataSet();
            ds = DataHelper.ExecuteDataSet(strQuery);
            return ds;
        }

        public static Utility.ResultType SelectCentrumBillingReport(out DataTable objDataTable, string SelectedFromDate, string SelectedToDate, int MetatemplateID)
        {
            try
            {

                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

                DbParameter[] objDbParameter = new DbParameter[3];
                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "@DateFrom";
                objDbParameter[0].Value = SelectedFromDate;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "@DateTo";
                objDbParameter[1].Value = SelectedToDate;

                 objDbParameter[2] = objDbProviderFactory.CreateParameter();
                 objDbParameter[2].ParameterName = "@MetatemplateID";
                objDbParameter[2].Value = MetatemplateID;

                DataSet ds = new DataSet();

                objDataTable = DataHelper.ExecuteDataTableForProcedure("SP_S_CentrumBillingData", null, objDbParameter);

                //                objDataTable = DataHelper.ExecuteDataTable("sp_User_Productivity_Report",objDbParameter);
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

        //inserting audit log for Mhada WEbsite search module
        public void InsertAuditLog_MhadaWebsite(string IPAddress, string MacAddress, string Activity, int DocID)
        {
            DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;
            DbTransaction objDbTransaction = Utility.GetTransaction;
            try
            {
                DbParameter[] objDbParameter = new DbParameter[4];

                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "@IPAddress";
                objDbParameter[0].Value = IPAddress;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "@MacAddress";
                objDbParameter[1].Value = MacAddress;

                //objDbParameter[2] = objDbProviderFactory.CreateParameter();
                //objDbParameter[2].ParameterName = "@DateOfActivity";
                //objDbParameter[2].Value = DateOfActivity;

                objDbParameter[2] = objDbProviderFactory.CreateParameter();
                objDbParameter[2].ParameterName = "@Activity";
                objDbParameter[2].Value = Activity;

                objDbParameter[3] = objDbProviderFactory.CreateParameter();
                objDbParameter[3].ParameterName = "@DocumentID";

                //if (DocumentName != null)
                objDbParameter[3].Value = DocID;
                //else
                //    objDbParameter[3].Value = null;


                DataHelper.ExecuteNonQuery("SP_I_AuditLog_MhadaWebsite", objDbTransaction, objDbParameter);
                objDbTransaction.Commit();
            }
            catch (Exception ex)
            {
                objDbTransaction.Rollback();
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                //return Utility.ResultType.Error;
            }
        }

        public void InsertAuditLogNew(string IPAddress, string MacAddress, string Activity, string PageName, int UserId)
        {
            DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;
            DbTransaction objDbTransaction = Utility.GetTransaction;
            try
            {
                DbParameter[] objDbParameter = new DbParameter[5];

                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "@IPAddress";
                objDbParameter[0].Value = IPAddress;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "@MacAddress";
                objDbParameter[1].Value = MacAddress;

                objDbParameter[2] = objDbProviderFactory.CreateParameter();
                objDbParameter[2].ParameterName = "@Activity";
                objDbParameter[2].Value = Activity;

                objDbParameter[3] = objDbProviderFactory.CreateParameter();
                objDbParameter[3].ParameterName = "@PageName";
                objDbParameter[3].Value = PageName;

                objDbParameter[4] = objDbProviderFactory.CreateParameter();
                objDbParameter[4].ParameterName = "@UserId";
                objDbParameter[4].Value = UserId;

                DataHelper.ExecuteNonQuery("SP_I_DMSAuditLog", objDbTransaction, objDbParameter);
                objDbTransaction.Commit();
            }
            catch (Exception ex)
            {
                objDbTransaction.Rollback();
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                //return Utility.ResultType.Error;
            }
        }

        public void InsertAuditLogDoc(string IPAddress, string MacAddress, string Activity, string PageName, int UserId, int DocID)
        {
            DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;
            DbTransaction objDbTransaction = Utility.GetTransaction;
            try
            {
                DbParameter[] objDbParameter = new DbParameter[6];

                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "@IPAddress";
                objDbParameter[0].Value = IPAddress;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "@MacAddress";
                objDbParameter[1].Value = MacAddress;

                objDbParameter[2] = objDbProviderFactory.CreateParameter();
                objDbParameter[2].ParameterName = "@Activity";
                objDbParameter[2].Value = Activity;

                objDbParameter[3] = objDbProviderFactory.CreateParameter();
                objDbParameter[3].ParameterName = "@PageName";
                objDbParameter[3].Value = PageName;

                objDbParameter[4] = objDbProviderFactory.CreateParameter();
                objDbParameter[4].ParameterName = "@UserId";
                objDbParameter[4].Value = UserId;

                objDbParameter[5] = objDbProviderFactory.CreateParameter();
                objDbParameter[5].ParameterName = "@DocID";
                objDbParameter[5].Value = DocID;

                DataHelper.ExecuteNonQuery("SP_I_DMSAudit_DocID", objDbTransaction, objDbParameter);
                objDbTransaction.Commit();
            }
            catch (Exception ex)
            {
                objDbTransaction.Rollback();
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                //return Utility.ResultType.Error;
            }
        }

        public void _InsertAuditLogDoc(string IPAddress, string MacAddress, string Activity, string PageName, int UserId, DataTable dtDocID)
        {
            DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;
            DbTransaction objDbTransaction = Utility.GetTransaction;
            try
            {
                DbParameter[] objDbParameter = new DbParameter[6];

                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "@IPAddress";
                objDbParameter[0].Value = IPAddress;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "@MacAddress";
                objDbParameter[1].Value = MacAddress;

                objDbParameter[2] = objDbProviderFactory.CreateParameter();
                objDbParameter[2].ParameterName = "@Activity";
                objDbParameter[2].Value = Activity;

                objDbParameter[3] = objDbProviderFactory.CreateParameter();
                objDbParameter[3].ParameterName = "@PageName";
                objDbParameter[3].Value = PageName;

                objDbParameter[4] = objDbProviderFactory.CreateParameter();
                objDbParameter[4].ParameterName = "@UserId";
                objDbParameter[4].Value = UserId;

                objDbParameter[5] = objDbProviderFactory.CreateParameter();
                objDbParameter[5].ParameterName = "@dtDocID";
                objDbParameter[5].Value = dtDocID;

                DataHelper.ExecuteNonQuery("SP_I_DMSAudit_DocID_New", objDbTransaction, objDbParameter);
                objDbTransaction.Commit();
            }
            catch (Exception ex)
            {
                objDbTransaction.Rollback();
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                //return Utility.ResultType.Error;
            }
        }

        public void InsertDocRenameLog(int DocId, string OlDDocumentName, string NewDocumentName, string IPAddress, int UserId)
        {
            DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;
            DbTransaction objDbTransaction = Utility.GetTransaction;
            try
            {
                DbParameter[] objDbParameter = new DbParameter[5];

                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "@DocID";
                objDbParameter[0].Value = DocId;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "@OlDDocumentName";
                objDbParameter[1].Value = OlDDocumentName;

                objDbParameter[2] = objDbProviderFactory.CreateParameter();
                objDbParameter[2].ParameterName = "@NewDocumentName";
                objDbParameter[2].Value = NewDocumentName;

                objDbParameter[3] = objDbProviderFactory.CreateParameter();
                objDbParameter[3].ParameterName = "@IPAddress";
                objDbParameter[3].Value = IPAddress;

                objDbParameter[4] = objDbProviderFactory.CreateParameter();
                objDbParameter[4].ParameterName = "@UpdatedBy";
                objDbParameter[4].Value = UserId;

                DataHelper.ExecuteNonQuery("SP_I_DocumentRenameLog", objDbTransaction, objDbParameter);
                objDbTransaction.Commit();
            }
            catch (Exception ex)
            {
                objDbTransaction.Rollback();
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                //return Utility.ResultType.Error;
            }
        }

    }
}
