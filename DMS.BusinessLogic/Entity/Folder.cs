using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Net;
using System.Net.NetworkInformation;
using System.Data.SqlClient;

namespace DMS.BusinessLogic
{
    public class Folder
    {
        #region Properties
        public int FolderID { get; set; }
        public string FolderName { get; set; }
        public string FolderDescription { get; set; }
        public int MetaTemplateID { get; set; }
        public int ParentFolderID { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public int UpdatedBy { get; set; }
        public int Status { get; set; }
        public int CategoryID { get; set; }
        #endregion

        #region Method
        public static Utility.ResultType Insert(Folder objFolder)
        {
            DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;
            DbTransaction objDbTransaction = Utility.GetTransaction;
            try
            {

                DbParameter[] objDbParameter = new DbParameter[9];

                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "FolderName";
                objDbParameter[0].Value = objFolder.FolderName;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "FolderDescription";
                objDbParameter[1].Value = objFolder.FolderDescription;

                objDbParameter[2] = objDbProviderFactory.CreateParameter();
                objDbParameter[2].ParameterName = "MetaTemplateID";
                objDbParameter[2].Value = objFolder.MetaTemplateID;

                objDbParameter[3] = objDbProviderFactory.CreateParameter();
                objDbParameter[3].ParameterName = "ParentFolderID";
                objDbParameter[3].Value = objFolder.ParentFolderID;

                objDbParameter[4] = objDbProviderFactory.CreateParameter();
                objDbParameter[4].ParameterName = "CreatedOn";
                objDbParameter[4].Value = DateTime.Now;

                objDbParameter[5] = objDbProviderFactory.CreateParameter();
                objDbParameter[5].ParameterName = "CreatedBy";
                objDbParameter[5].Value = objFolder.CreatedBy;

                objDbParameter[6] = objDbProviderFactory.CreateParameter();
                objDbParameter[6].ParameterName = "Status";
                objDbParameter[6].Value = objFolder.Status;

                objDbParameter[7] = objDbProviderFactory.CreateParameter();
                objDbParameter[7].ParameterName = "CategoryID";
                objDbParameter[7].Value = objFolder.CategoryID;

                objDbParameter[8] = objDbProviderFactory.CreateParameter();
                objDbParameter[8].ParameterName = "FolderID";
                objDbParameter[8].Size = 50;
                objDbParameter[8].Direction = System.Data.ParameterDirection.Output;


                DataHelper.ExecuteNonQuery("SP_I_Folder", objDbTransaction, objDbParameter);
                objDbTransaction.Commit();
                objFolder.FolderID = Convert.ToInt32(objDbParameter[8].Value);
                //Utility.InsertAccessRight(0, 0, 0, objFolder.FolderID);
                return Utility.ResultType.Success;
            }
            catch (Exception ex)
            {
                objDbTransaction.Rollback();
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return Utility.ResultType.Error;
            }
        }

        #region Vivek 6 nov 2017

        public static Utility.ResultType InsertAxisTrustee(Folder objFolder)
        {
            DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;
            DbTransaction objDbTransaction = Utility.GetTransaction;
            try
            {

                DbParameter[] objDbParameter = new DbParameter[9];

                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "FolderName";
                objDbParameter[0].Value = objFolder.FolderName;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "FolderDescription";
                objDbParameter[1].Value = objFolder.FolderDescription;

                objDbParameter[2] = objDbProviderFactory.CreateParameter();
                objDbParameter[2].ParameterName = "MetaTemplateID";
                objDbParameter[2].Value = objFolder.MetaTemplateID;

                objDbParameter[3] = objDbProviderFactory.CreateParameter();
                objDbParameter[3].ParameterName = "ParentFolderID";
                objDbParameter[3].Value = "0"; //objFolder.ParentFolderID;//"0";//;

                objDbParameter[4] = objDbProviderFactory.CreateParameter();
                objDbParameter[4].ParameterName = "CreatedOn";
                objDbParameter[4].Value = DateTime.Now;

                objDbParameter[5] = objDbProviderFactory.CreateParameter();
                objDbParameter[5].ParameterName = "CreatedBy";
                objDbParameter[5].Value = objFolder.CreatedBy;

                objDbParameter[6] = objDbProviderFactory.CreateParameter();
                objDbParameter[6].ParameterName = "Status";
                objDbParameter[6].Value = objFolder.Status;

                objDbParameter[7] = objDbProviderFactory.CreateParameter();
                objDbParameter[7].ParameterName = "FolderID";
                objDbParameter[7].Size = 50;
                objDbParameter[7].Direction = System.Data.ParameterDirection.Output;

                objDbParameter[8] = objDbProviderFactory.CreateParameter();
                objDbParameter[8].ParameterName = "CategoryID";
                objDbParameter[8].Value = objFolder.CategoryID;


                DataHelper.ExecuteNonQuery("SP_I_Folder_AxisTrustee", objDbTransaction, objDbParameter);
                objDbTransaction.Commit();
                objFolder.FolderID = Convert.ToInt32(objDbParameter[7].Value);
                //Utility.InsertAccessRight(0, 0, 0, objFolder.FolderID);
                return Utility.ResultType.Success;
            }
            catch (Exception ex)
            {
                objDbTransaction.Rollback();
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return Utility.ResultType.Error;
            }
        }

        #endregion

        #region Vivek 21 nov 2017
        public static Utility.ResultType InsertForAll(Folder objFolder)
        {
            DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;
            DbTransaction objDbTransaction = Utility.GetTransaction;
            try
            {

                DbParameter[] objDbParameter = new DbParameter[9];

                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "FolderName";
                objDbParameter[0].Value = objFolder.FolderName;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "FolderDescription";
                objDbParameter[1].Value = objFolder.FolderDescription;

                objDbParameter[2] = objDbProviderFactory.CreateParameter();
                objDbParameter[2].ParameterName = "MetaTemplateID";
                objDbParameter[2].Value = objFolder.MetaTemplateID;

                objDbParameter[3] = objDbProviderFactory.CreateParameter();
                objDbParameter[3].ParameterName = "ParentFolderID";
                objDbParameter[3].Value = "0"; //objFolder.ParentFolderID;//"0";//;

                objDbParameter[4] = objDbProviderFactory.CreateParameter();
                objDbParameter[4].ParameterName = "CreatedOn";
                objDbParameter[4].Value = DateTime.Now;

                objDbParameter[5] = objDbProviderFactory.CreateParameter();
                objDbParameter[5].ParameterName = "CreatedBy";
                objDbParameter[5].Value = objFolder.CreatedBy;

                objDbParameter[6] = objDbProviderFactory.CreateParameter();
                objDbParameter[6].ParameterName = "Status";
                objDbParameter[6].Value = objFolder.Status;

                objDbParameter[7] = objDbProviderFactory.CreateParameter();
                objDbParameter[7].ParameterName = "FolderID";
                objDbParameter[7].Size = 50;
                objDbParameter[7].Direction = System.Data.ParameterDirection.Output;

                objDbParameter[8] = objDbProviderFactory.CreateParameter();
                objDbParameter[8].ParameterName = "CategoryID";
                objDbParameter[8].Value = objFolder.CategoryID;


                DataHelper.ExecuteNonQuery("SP_I_Folder_ForAll", objDbTransaction, objDbParameter);
                objDbTransaction.Commit();
                objFolder.FolderID = Convert.ToInt32(objDbParameter[7].Value);
                //Utility.InsertAccessRight(0, 0, 0, objFolder.FolderID);
                return Utility.ResultType.Success;
            }
            catch (Exception ex)
            {
                objDbTransaction.Rollback();
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return Utility.ResultType.Error;
            }
        }
        #endregion

        public static Utility.ResultType InsertCentrum(Folder objFolder)
        {
            DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;
            DbTransaction objDbTransaction = Utility.GetTransaction;
            try
            {

                DbParameter[] objDbParameter = new DbParameter[9];

                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "FolderName";
                objDbParameter[0].Value = objFolder.FolderName;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "FolderDescription";
                objDbParameter[1].Value = objFolder.FolderDescription;

                objDbParameter[2] = objDbProviderFactory.CreateParameter();
                objDbParameter[2].ParameterName = "MetaTemplateID";
                objDbParameter[2].Value = objFolder.MetaTemplateID;

                objDbParameter[3] = objDbProviderFactory.CreateParameter();
                objDbParameter[3].ParameterName = "ParentFolderID";
                objDbParameter[3].Value = objFolder.ParentFolderID;

                objDbParameter[4] = objDbProviderFactory.CreateParameter();
                objDbParameter[4].ParameterName = "CreatedOn";
                objDbParameter[4].Value = DateTime.Now;

                objDbParameter[5] = objDbProviderFactory.CreateParameter();
                objDbParameter[5].ParameterName = "CreatedBy";
                objDbParameter[5].Value = objFolder.CreatedBy;

                objDbParameter[6] = objDbProviderFactory.CreateParameter();
                objDbParameter[6].ParameterName = "Status";
                objDbParameter[6].Value = objFolder.Status;

                objDbParameter[7] = objDbProviderFactory.CreateParameter();
                objDbParameter[7].ParameterName = "CategoryID";
                objDbParameter[7].Value = objFolder.CategoryID;

                objDbParameter[8] = objDbProviderFactory.CreateParameter();
                objDbParameter[8].ParameterName = "FolderID";
                objDbParameter[8].Size = 50;
                objDbParameter[8].Direction = System.Data.ParameterDirection.Output;

                DataHelper.ExecuteNonQuery("SP_I_FolderCentrum", objDbTransaction, objDbParameter);
                objDbTransaction.Commit();
                objFolder.FolderID = Convert.ToInt32(objDbParameter[8].Value);
                //DMS.BusinessLogic.Report objReport = new DMS.BusinessLogic.Report();
                //string IPAddress = GetIPAddress();
                //string MACAddress = GetMacAddress();
                //objReport.InsertAuditLog(IPAddress, MACAddress, "Insert Folder", objFolder.FolderName, UserSession.UserID);
               // Utility.InsertAccessRight(0, 0, 0, objFolder.FolderID);
                return Utility.ResultType.Success;
            }
            catch (Exception ex)
            {
                objDbTransaction.Rollback();
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return Utility.ResultType.Error;
            }
        }

        public static string GetIPAddress()
        {
            string IPAddress = "";
            IPHostEntry Host = default(IPHostEntry);
            string Hostname = null;
            Hostname = System.Environment.MachineName;
            Host = Dns.GetHostEntry(Hostname);
            foreach (IPAddress IP in Host.AddressList)
            {
                if (IP.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    IPAddress = Convert.ToString(IP);
                }
            }
            return IPAddress;

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

        public static Utility.ResultType Update(Folder objFolder)
        {
            DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;
            DbTransaction objDbTransaction = Utility.GetTransaction;
            try
            {

                DbParameter[] objDbParameter = new DbParameter[6];

                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "FolderName";
                objDbParameter[0].Value = objFolder.FolderName;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "FolderDescription";
                objDbParameter[1].Value = objFolder.FolderDescription;

                objDbParameter[2] = objDbProviderFactory.CreateParameter();
                objDbParameter[2].ParameterName = "UpdatedOn";
                objDbParameter[2].Value = DateTime.Now;

                objDbParameter[3] = objDbProviderFactory.CreateParameter();
                objDbParameter[3].ParameterName = "UpdatedBy";
                objDbParameter[3].Value = objFolder.UpdatedBy;

                objDbParameter[4] = objDbProviderFactory.CreateParameter();
                objDbParameter[4].ParameterName = "Status";
                objDbParameter[4].Value = objFolder.Status;

                objDbParameter[5] = objDbProviderFactory.CreateParameter();
                objDbParameter[5].ParameterName = "FolderID";
                objDbParameter[5].Value = objFolder.FolderID;

                DataHelper.ExecuteNonQuery("SP_U_Folder", objDbTransaction, objDbParameter);
                objDbTransaction.Commit();

                return Utility.ResultType.Success;
            }
            catch (Exception ex)
            {
                objDbTransaction.Rollback();
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return Utility.ResultType.Error;
            }
        }

        public static Utility.ResultType Select(out DataTable objDataTable)
        {
            try
            {
                string strQuery = "SELECT * FROM vwFolder WHERE Status=1";
                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

           
                objDataTable = DataHelper.ExecuteDataTable(strQuery,null);
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

        public static Utility.ResultType SelectByFolderID(out DataTable objDataTable, int intFolderID)
        {
            try
            {
                string strQuery = "SELECT * FROM vwFolder WHERE ID =@ID";
                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

                DbParameter[] objDbParameter = new DbParameter[1];
                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "ID";
                objDbParameter[0].Value = intFolderID;

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
        public static Utility.ResultType selectFolderByMetaID(out DataTable objDataTable, int RepositoryID, int intMetaTemplateID, int FolderID, int DocCount)
        {
            try
            {

                //string strQuery = "SELECT t1.FolderName FolderName,t2.FolderName FolderParent FROM Folder t1 LEFT JOIN Folder t2 ON t1.ParentFolderID = t2.ID where t1.id not in (select folderid from vwDocumentDetails) and t1.MetaTemplateID=@MetaTemplateID";

                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

                DbParameter[] objDbParameter = new DbParameter[4];
                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "@RepositoryID";
                objDbParameter[0].Value = RepositoryID;

          
                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "MetaTemplateID";
                objDbParameter[1].Value = intMetaTemplateID;

                objDbParameter[2] = objDbProviderFactory.CreateParameter();
                objDbParameter[2].ParameterName = "@DocCount";
                objDbParameter[2].Value = DocCount;

                objDbParameter[3] = objDbProviderFactory.CreateParameter();
                objDbParameter[3].ParameterName = "@ParentFolderID";
                objDbParameter[3].Value = FolderID;

                objDataTable = DataHelper.ExecuteDataTableNew("Sp_LeastFolderCount_Final", objDbParameter);
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
              
        public static Utility.ResultType SelectByParentFolderID(out DataTable objDataTable, int intParentFolderID,int intMetaTemplateID)
        {
            try
            {   char charQuote = '"';
                string strQuery = " SELECT ID,FolderName,CreatedOn,"+
                                    " (SELECT FirstName FROM vwUser WHERE ID = vwFolder.CreatedBy) AS " + charQuote + "CreatedBy" + charQuote + "," +
                                    " UpdatedOn,"+
                                    " (SELECT FirstName FROM vwUser WHERE ID = vwFolder.UpdatedBy) AS " + charQuote + "UpdatedBy" + charQuote + "," +
                                    " (SELECT SettingName FROM vwAppSetting WHERE SettingType='STATUS' AND SettingValue=vwFolder.Status) AS " + charQuote + "Status" + charQuote + "" +
                                    " FROM vwFolder WHERE ParentFolderID =@ParentFolderID AND MetaTemplateID=@MetaTemplateID AND ID IN (" + Utility.GetAccessRight(Utility.AccessRight.Folder) + ") ORDER BY ID DESC ";

                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

                DbParameter[] objDbParameter = new DbParameter[2];
                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "ParentFolderID";
                objDbParameter[0].Value = intParentFolderID;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "MetaTemplateID";
                objDbParameter[1].Value = intMetaTemplateID;

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

        public static Utility.ResultType SelectByFolderName(out DataTable dt,string strFolderName, int intMetaTemplateID, int intParentFolderID,int intCategoryID)
        {
            try
            {
                string strQuery = "SELECT FolderName,ID FROM Folder WHERE UPPER(FolderName) = @FolderName AND MetaTemplateID = @MetaTemplateID AND ParentFolderID = @ParentFolderID AND CategoryID=@CategoryID";
                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

                DbParameter[] objDbParameter = new DbParameter[4];
                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "FolderName";
                objDbParameter[0].Value = strFolderName.ToUpper().Trim();

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "MetaTemplateID";
                objDbParameter[1].Value = intMetaTemplateID;

                objDbParameter[2] = objDbProviderFactory.CreateParameter();
                objDbParameter[2].ParameterName = "ParentFolderID";
                objDbParameter[2].Value = intParentFolderID;

                objDbParameter[3] = objDbProviderFactory.CreateParameter();
                objDbParameter[3].ParameterName = "CategoryID";
                objDbParameter[3].Value = intCategoryID;

                dt = DataHelper.ExecuteDataTable(strQuery, objDbParameter);
                //object objResult = DataHelper.ExecuteScalar(strQuery, objDbParameter);
                if (dt.Rows.Count==0)
                {
                    return Utility.ResultType.Failure;
                }
                return Utility.ResultType.Success;
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                dt = null;
                return Utility.ResultType.Error;
            }
        }


        public static DataTable SelectByFolderNameCentrum2(string strFolderName, int intMetaTemplateID, int intParentFolderID)
        {
            try
            {
                DataTable dt;
                string strQuery = "SELECT FolderName,ID FROM vwFolder WHERE UPPER(FolderName) = @FolderName AND MetaTemplateID = @MetaTemplateID AND ParentFolderID = @ParentFolderID and Status=1";
                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

                DbParameter[] objDbParameter = new DbParameter[3];
                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "FolderName";
                objDbParameter[0].Value = strFolderName.ToUpper().Trim();

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "MetaTemplateID";
                objDbParameter[1].Value = intMetaTemplateID;

                objDbParameter[2] = objDbProviderFactory.CreateParameter();
                objDbParameter[2].ParameterName = "ParentFolderID";
                objDbParameter[2].Value = intParentFolderID;

                dt = DataHelper.ExecuteDataTable(strQuery, objDbParameter);
                //object objResult = DataHelper.ExecuteScalar(strQuery, objDbParameter);
                return dt;
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                //dt = null;
                return null;
            }
        }



        public static DataTable SelectByFolderNameCentrum(string strFolderName, int intMetaTemplateID, int intParentFolderID,int intCategoryID)
        {
            try
            {
                //DataTable dt = new DataTable();
                DataTable dtInvoice = new DataTable();
                using (SqlConnection scon = new SqlConnection(Utility.ConnectionString))
                {
                    SqlCommand scmd = new SqlCommand("SP_S_MissingEntryDocs", scon);
                    
                    scmd.CommandType = CommandType.StoredProcedure;

                    scmd.Parameters.AddWithValue("@FolderName", strFolderName);
                    scmd.Parameters.AddWithValue("@MetatemplateID", intMetaTemplateID);
                    scmd.Parameters.AddWithValue("@ParentFolderID", intParentFolderID);
                    scmd.Parameters.AddWithValue("@categoryID", intCategoryID);
                    scmd.CommandTimeout = 0;
                    if (scon.State == ConnectionState.Open)
                        scon.Close();
                    scon.Open();
                    SqlDataAdapter da = new SqlDataAdapter(scmd);
                    da.Fill(dtInvoice);
                    scon.Close();
                }
                return dtInvoice;
                //DataTable dt;
                ////string strQuery = "SELECT FolderName,ID FROM Folder WHERE UPPER(FolderName) = '" + strFolderName + "' AND MetaTemplateID = '" + intMetaTemplateID + "' AND ParentFolderID = '" + intParentFolderID + "' AND CategoryID='" + intCategoryID + "'";
                //string strQuery = "SELECT FolderName,ID FROM Folder WHERE UPPER(FolderName) = @FolderName AND MetaTemplateID = @MetaTemplateID AND ParentFolderID = @ParentFolderID AND CategoryID=@CategoryID and Status=1";
                //DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

                //DbParameter[] objDbParameter = new DbParameter[4];
                //objDbParameter[0] = objDbProviderFactory.CreateParameter();
                //objDbParameter[0].ParameterName = "FolderName";
                //objDbParameter[0].Value = strFolderName.ToUpper().Trim();

                //objDbParameter[1] = objDbProviderFactory.CreateParameter();
                //objDbParameter[1].ParameterName = "MetaTemplateID";
                //objDbParameter[1].Value = intMetaTemplateID;

                //objDbParameter[2] = objDbProviderFactory.CreateParameter();
                //objDbParameter[2].ParameterName = "ParentFolderID";
                //objDbParameter[2].Value = intParentFolderID;

                //objDbParameter[3] = objDbProviderFactory.CreateParameter();
                //objDbParameter[3].ParameterName = "CategoryID";
                //objDbParameter[3].Value = intCategoryID;

                //dt = DataHelper.ExecuteDataTable(strQuery, objDbParameter);
                ////object objResult = DataHelper.ExecuteScalar(strQuery, objDbParameter);
                //return dt;
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
                DataSet ds = new DataSet();
                string strQuery = "SELECT FolderName FROM vwFolder WHERE UPPER(FolderName) LIKE @FolderName AND MetaTemplateID = @MetaTemplateID";
                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

                DbParameter[] objDbParameter = new DbParameter[2];
                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "FolderName";
                objDbParameter[0].Value = string.Format("%{0}%", strFolderName.ToUpper().Trim());

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "MetaTemplateID";
                objDbParameter[1].Value = intMetaTemplateID;

               // objDbParameter[4].Value = string.Format("%{0}%", strFolderName.ToUpper().Trim());

                ds = DataHelper.ExecuteDataSet(strQuery);
                return ds;
                //object objResult = DataHelper.ExecuteScalar(strQuery, objDbParameter);
                //if (objResult == null)
                //{
                //    return Utility.ResultType.Failure;
                //}
                //return Utility.ResultType.Success;
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return null;
            }
        }

        public static Utility.ResultType SelectFolderForTreeView(out DataTable objDataTable, int intMetaTemplateID)
        {
            try
            {
                //string strQuery = "SELECT ID,FolderName,FolderDescription,ParentFolderID FROM vwFolder WHERE Status=1 AND MetaTemplateID=@MetaTemplateID AND ID IN (" + Utility.GetAccessRight(Utility.AccessRight.Folder) + ")";
                //AND ID IN (" + Utility.GetAccessRight(Utility.AccessRight.Folder) + ")";
                string strQuery = @"SELECT distinct F.ID,F.FolderName,F.FolderDescription,F.ParentFolderID 
                                    FROM Folder F inner join MetaTemplate M
                                    on M.ID=F.MetaTemplateID
                                  WHERE F.Status=1 
                                  AND F.MetaTemplateID=@MetaTemplateID AND F.ID IN (" + Utility.GetAccessRight(Utility.AccessRight.Folder) + ")";
                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

                DbParameter[] objDbParameter = new DbParameter[1];
                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "MetaTemplateID";
                objDbParameter[0].Value = intMetaTemplateID;

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

        public static Utility.ResultType SelectFolderForTreeView_Centrum(out DataTable objDataTable, int intMetaTemplateID, int CategoryID)
        {
            try
            {
                //string strQuery = "SELECT ID,FolderName,FolderDescription,ParentFolderID FROM vwFolder WHERE Status=1 AND MetaTemplateID=@MetaTemplateID AND ID IN (" + Utility.GetAccessRight(Utility.AccessRight.Folder) + ")";
//                string strQuery = @"SELECT F.ID,F.FolderName,F.FolderDescription,F.ParentFolderID 
//                                  FROM vwFolder F inner join MetaData M
//                                  on M.FolderID=F.ID
//                                  WHERE F.Status=1 AND M.CategoryID=@CategoryID
//                                  AND F.MetaTemplateID=@MetaTemplateID AND F.ID IN (" + Utility.GetAccessRight(Utility.AccessRight.Folder) + ")";
                string strQuery = @"select ID,FolderName,FolderDescription,ParentFolderID
                                    from Folder where MetaTemplateID='" + intMetaTemplateID + "' and categoryID='" + CategoryID + "'";
                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

                DbParameter[] objDbParameter = new DbParameter[2];
                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "MetaTemplateID";
                objDbParameter[0].Value = intMetaTemplateID;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "CategoryID";
                objDbParameter[1].Value = CategoryID;

                objDataTable = DataHelper.ExecuteDataTable(strQuery, objDbParameter);
                DataRow dr1 = objDataTable.NewRow();
                dr1[0] = 2450;
                dr1[1] = "PAN NO";
                dr1[2] = "";
                dr1[3] = 0;
                objDataTable.Rows.Add(dr1);
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

        #region Seema 9Nov 2017

        public static Utility.ResultType SelectFolderForTreeView_AxisTree(out DataTable objDataTable, int intMetaTemplateID, int CategoryID)
        {
            try
            {
                //string strQuery = "SELECT ID,FolderName,FolderDescription,ParentFolderID FROM vwFolder WHERE Status=1 AND MetaTemplateID=@MetaTemplateID AND ID IN (" + Utility.GetAccessRight(Utility.AccessRight.Folder) + ")";
                //                string strQuery = @"SELECT F.ID,F.FolderName,F.FolderDescription,F.ParentFolderID 
                //                                  FROM vwFolder F inner join MetaData M
                //                                  on M.FolderID=F.ID
                //                                  WHERE F.Status=1 AND M.CategoryID=@CategoryID
                //                                  AND F.MetaTemplateID=@MetaTemplateID AND F.ID IN (" + Utility.GetAccessRight(Utility.AccessRight.Folder) + ")";
                string strQuery = @"select ID,FolderName,FolderDescription,ParentFolderID
                                    from Folder where MetaTemplateID='" + intMetaTemplateID + "' and categoryID='" + CategoryID + "'";
                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

                DbParameter[] objDbParameter = new DbParameter[2];
                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "MetaTemplateID";
                objDbParameter[0].Value = intMetaTemplateID;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "CategoryID";
                objDbParameter[1].Value = CategoryID;

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

        #endregion

        #region vivek 14 Nov 2017
        public static Utility.ResultType SelectFolderCategoryWise(out DataTable objDataTable, int intMetaTemplateID, int CategoryID)
        {
            try
            {
                //string strQuery = "SELECT ID,FolderName,FolderDescription,ParentFolderID FROM vwFolder WHERE Status=1 AND MetaTemplateID=@MetaTemplateID AND ID IN (" + Utility.GetAccessRight(Utility.AccessRight.Folder) + ")";
                //                string strQuery = @"SELECT F.ID,F.FolderName,F.FolderDescription,F.ParentFolderID 
                //                                  FROM vwFolder F inner join MetaData M
                //                                  on M.FolderID=F.ID
                //                                  WHERE F.Status=1 AND M.CategoryID=@CategoryID
                //                                  AND F.MetaTemplateID=@MetaTemplateID AND F.ID IN (" + Utility.GetAccessRight(Utility.AccessRight.Folder) + ")";
                string strQuery = @"select ID,FolderName,FolderDescription,ParentFolderID
                                    from Folder where MetaTemplateID='" + intMetaTemplateID + "' and categoryID='" + CategoryID + "' AND Status=1 AND ID IN (" + Utility.GetAccessRight(Utility.AccessRight.Folder) + ")";
                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

                DbParameter[] objDbParameter = new DbParameter[2];
                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "MetaTemplateID";
                objDbParameter[0].Value = intMetaTemplateID;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "CategoryID";
                objDbParameter[1].Value = CategoryID;

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
        #endregion
        public static Utility.ResultType SelectAllByRoleID(out DataTable objDataTable, int intRoleID)
        {
            try
            {
                char charQuote = '"';
                string strQuery = "SELECT *" +
                                    " FROM Folder Left Outer Join (SELECT Distinct(FolderID) FROM vwRolePermission WHERE RoleID=@RoleID)  RolePermission" +
                                    " ON Folder.Id=RolePermission.FolderID" +
                                    " ORDER BY ID DESC";


                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

                DbParameter[] objDbParameter = new DbParameter[1];
                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "RoleID";
                objDbParameter[0].Value = intRoleID;

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


        public static Utility.ResultType GetAccesstoFolder(Folder objFolder,int RepositoryID, int RoleId, int UserId)
        {
            DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;
            DbTransaction objDbTransaction = Utility.GetTransaction;
            try
            {

                DbParameter[] objDbParameter = new DbParameter[6];

                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "RoleID";
                objDbParameter[0].Value = RoleId;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "RepositoryID";
                objDbParameter[1].Value = RepositoryID;

                objDbParameter[2] = objDbProviderFactory.CreateParameter();
                objDbParameter[2].ParameterName = "MetaTemplateID";
                objDbParameter[2].Value = objFolder.MetaTemplateID;

                objDbParameter[3] = objDbProviderFactory.CreateParameter();
                objDbParameter[3].ParameterName = "CategoryID";
                objDbParameter[3].Value = objFolder.CategoryID;

                objDbParameter[4] = objDbProviderFactory.CreateParameter();
                objDbParameter[4].ParameterName = "FolderID";
                objDbParameter[4].Value = objFolder.FolderID;

                objDbParameter[5] = objDbProviderFactory.CreateParameter();
                objDbParameter[5].ParameterName = "UpdatedBy";
                objDbParameter[5].Value = UserId;

                DataHelper.ExecuteNonQuery("SP_AccessPermission", objDbTransaction, objDbParameter);
                objDbTransaction.Commit();

                return Utility.ResultType.Success;
            }
            catch (Exception ex)
            {
                objDbTransaction.Rollback();
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return Utility.ResultType.Error;
            }
        }
        #endregion

        public static Utility.ResultType GetDocumentByFolderID(out DataTable objDataTable, int folderId)
        {
            DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;
            DbTransaction objDbTransaction = Utility.GetTransaction;
            try
            {
                DbParameter[] objDbParameter = new DbParameter[1];

                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "@FolderId";
                objDbParameter[0].Value = folderId;

                objDataTable = DataHelper.ExecuteDataTableNew("U_Sp_DocumentDetailsByFolder", objDbParameter);
                objDbTransaction.Commit();

                return Utility.ResultType.Success;
            }
            catch (Exception ex)
            {
                objDbTransaction.Rollback();
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                objDataTable = null;
                return Utility.ResultType.Error;
            }

        }


        public static DataTable GetDocumentForCartByDocumentID(List<int> DocumentId)
        {
            DataTable objDataTable = new DataTable();
            try
            {
                SqlConnection con = new SqlConnection(Utility.ConnectionString);
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                string DocumentIDs = string.Join(",", DocumentId);
                SqlCommand cmd = new SqlCommand(@"select ID,DocumentName,Tag,[dbo].[udf_GetFullFolderPath](RepositoryId,MetaTemplateId,FolderId) as DocumentPath from vwDocumentDetails where ID in (" + DocumentIDs + ")", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(objDataTable);
            }
            catch (Exception ex)
            {
                
                throw;
            }
            return objDataTable;
        }
       
    }
}
