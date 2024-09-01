using DMS.BusinessLogic;
using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DMS.Shared
{
    public partial class SBI_Mutual_Fund_Trustee_Company_Private_limited_Download : System.Web.UI.Page

    {
        #region Seema 13 June 2018

        #region Private Members

        SqlConnection con = new SqlConnection(Utility.ConnectionString);

        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            ((AjaxControlToolkit.ToolkitScriptManager)Master.FindControl("tsmManager")).RegisterPostBackControl(ibtn_download);
            ibtn_download.Visible = false;
            ibtn_delete.Visible = false;

            if (!IsPostBack)
            {
                LoadFolder();
            }

        }

        protected void ibtnShow_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                string strquery = " Select distinct convert(date,d.createdon) from Document d inner join metadata m on d.metadataid=m.id where  m.folderid='" + drpfolder.SelectedItem.Value + "' and d.status=1";
                DataSet dsdownload = new DataSet();
                dsdownload = DataHelper.ExecuteDataSet(strquery);
             
                if (dsdownload.Tables[0].Rows.Count > 0)
                {
                    Session["datecount"] = dsdownload.Tables[0];
                    lbldate.Visible = true;
                    drpdate.Visible = true;
                    drpdate.Items.Clear();
                    drpdate.Items.Add(new ListItem("--Select--", "0"));
                    for (int i = 0; i < dsdownload.Tables[0].Rows.Count; i++)
                        drpdate.Items.Add(new ListItem(
                        Convert.ToString(dsdownload.Tables[0].Rows[i][0]).Trim(),
                        Convert.ToString(dsdownload.Tables[0].Rows[i][0])));                 
                }
                else
                {
                   
                    UserSession.DisplayMessage(this, "No Data Found", MainMasterPage.MessageType.Warning);
                }
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);

            }
        }

        protected void drpdate_SelectedIndexChanged(object sender, EventArgs e)
        {
            ibtn_download.Visible = true;
            string strquery = "select d.id,d.DocumentName,d.createdon,d.pagecount,d.documentpath from Document d inner join metadata m on d.metadataid=m.id where  m.folderid='" + drpfolder.SelectedItem.Value + "' and d.status=1 and convert(date,d.createdon)='" + drpdate.SelectedItem.Text + "' ";
            DataSet dsdownloaddate = new DataSet();
            dsdownloaddate = DataHelper.ExecuteDataSet(strquery);

            Session["download"] = dsdownloaddate.Tables[0];
            if (UserSession.UserID == 223)
           {
               ibtn_delete.Visible = true;
           }
           else
           {
               ibtn_delete.Visible = false;
           }

        }

        private static List<DataTable> SplitTable(DataTable originalTable, int batchSize)
        {
            List<DataTable> tables = new List<DataTable>();
            int i = 0;
            int j = 1;
            DataTable newDt = originalTable.Clone();
            newDt.TableName = "Table_" + j;
            newDt.Clear();
            foreach (DataRow row in originalTable.Rows)
            {
                DataRow newRow = newDt.NewRow();
                newRow.ItemArray = row.ItemArray;
                newDt.Rows.Add(newRow);
                i++;
                if (i == batchSize)
                {
                    tables.Add(newDt);
                    j++;
                    newDt = originalTable.Clone();
                    newDt.TableName = "Table_" + j;
                    newDt.Clear();
                    i = 0;
                }
            }
            return tables;
        }
 

        protected void ibtn_download_Click(object sender, ImageClickEventArgs e)
        {
            if (Session["download"] != null)
            {
                //List<DataTable> dtList = new List<DataTable>();
                //dtList = SplitTable((DataTable)Session["download"], 1000);
                //foreach (var dtLst in dtList)
                //{
                
                DownloadFiles((DataTable)Session["download"]);
                    
                //}
            }
                
        }

        protected void ibtn_delete_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = (DataTable)Session["download"];

                DataTable dtdate = new DataTable();
                dtdate = (DataTable)Session["datecount"];

                if (Session["download"] != null)
                {     
            
                    foreach (DataRow dr in dt.Rows)
                    {
                        try
                        {
                            string strquery1 = "update document set status='0',updatedon='" + DateTime.Now.ToString() + "',updatedby='" + UserSession.UserID + "' where id='" + dr[0] + "'";
                            SqlCommand cmdresult1 = new SqlCommand(strquery1, con);
                            con.Open();
                            cmdresult1.ExecuteNonQuery();
                            con.Close();

                            if (Session["datecount"] != null)
                            {
                            if (dtdate.Rows.Count==1)
                           {
                               string strquery = "update folder set status='0',updatedon='" + DateTime.Now.ToString() + "',updatedby='" + UserSession.UserID + "' where id='" + drpfolder.SelectedItem.Value + "'";
                               SqlCommand cmdresult = new SqlCommand(strquery, con);
                               con.Open();
                               cmdresult.ExecuteNonQuery();
                               con.Close();
                           }      
                            }
                            string FilePath = Convert.ToString(dr[4]);
                            File.Delete(FilePath);
                            UserSession.DisplayMessage(this, "Data deleted Successfully", MainMasterPage.MessageType.Success);
                        }
                        catch (Exception ex)
                        {
                            UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                            LogManager.ErrorLog(Utility.LogFilePath, ex);
                        }
                     
                        
                    }
                }
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);

            }
        }

        #endregion

        #region Methods

        public void LoadFolder()
        {
            string strQuery1 = "select distinct ID,foldername from vwfolder where status=1 and metatemplateid=177 order by foldername asc";
            DataTable objDataTable = DataHelper.ExecuteDataTable(strQuery1, null);

            if (objDataTable.Rows.Count > 0)
            {
                drpfolder.DataSource = objDataTable;
                drpfolder.DataTextField = "foldername";
                drpfolder.DataValueField = "ID";
                drpfolder.DataBind();
            }
            if (objDataTable.Rows.Count == 0)
            {
                drpfolder.Items.Clear();
            }
            drpfolder.Items.Insert(0, new ListItem("--SELECT--", "-1"));
        }

        protected void DownloadFiles(DataTable dt)
        {
            try
            {
               
                using (ZipFile zip = new ZipFile())
                {
                  //  zip.AlternateEncodingUsage = ZipOption.AsNecessary;
                    //zip.AddDirectoryByName("Files");
                    foreach (DataRow row in dt.Rows)
                    {
                        //if ((row.FindControl("chkRow") as CheckBox).Checked)
                        //{

                        //string filePath = (string)gvwTreeViewData.DataKeys[row.RowIndex].Values["DocumentPath"];

                        ////(row.FindControl("lblFilePath") as Label).Text;
                        //zip.AddFile(filePath).FileName = (string)gvwTreeViewData.DataKeys[row.RowIndex].Values["DocumentName"];
                        //string inputpath = Utility.DocumentPath + "SBI Mutual Fund Trustee Company Private limited" + @"\";
                        //string outputpath = Utility.DocumentPath + "SBI Mutual Fund Trustee Company Private limited" + @"\";

                        string FilePath = Convert.ToString(row[4]);
                        // string filePath = (string)grvsearch.DataKeys[row.RowIndex].Values["DocumentPath"];
                        FileInfo objFile = new FileInfo(FilePath);

                        //decrypt the file from FilePath
                        // string PathFolder = @"D:\Intermidiate";
                        //if (!Directory.Exists(PathFolder))
                        //{
                        //    Directory.CreateDirectory(PathFolder);
                        //}
                        // FilePath = objTable.Rows[0]["DocumentPath"].ToString();
                        // DecryptFile(Session["DocumentPath"].ToString(), FilePath);
                        //if (File.Exists(outputpath + objFile.Name) && objFile.Extension.ToLower().Contains("pdf"))
                        //{
                        //Temp Commented
                        // DecryptFile(inputpath + objFile.Name, outputpath + objFile.Name);
                        if (File.Exists(FilePath))
                        zip.AddFile(FilePath).FileName = (string)Convert.ToString(row[1]);

                        //}
                        //else
                        //{

                        //    zip.AddFile(outputpath + objFile.Name).FileName = (string)grvsearch.DataKeys[row.RowIndex].Values["DocumentName"];

                        //    //UserSession.DisplayMessage((Page)this.Parent.Page, "Try again to view document", MainMasterPage.MessageType.Warning);
                        //    //return;

                        //}
                        //}
                        //else
                        //{

                        //}
                    }
                    Response.Clear();
                    Response.BufferOutput = false;
                    string zipName = String.Format("Zip_{0}.zip", DateTime.Now.ToString("yyyy-MMM-dd-HHmmss"));
                    
                    Response.ContentType = "application/zip";
                    Response.AddHeader("content-disposition", "attachment; filename=" + zipName);
                    zip.Save(Response.OutputStream);
                    Response.End();
                }

            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);

            }
        }

        #endregion     

        protected void drpfolder_SelectedIndexChanged(object sender, EventArgs e)
        {
            lbldate.Visible = false;
            drpdate.Visible = false;
        }

        #endregion

    }
}