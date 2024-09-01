using DMS.BusinessLogic;
using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DMS.Shared
{
    public partial class Reliance_BulkDownload : System.Web.UI.Page
    {

        #region Seema 13 June 2018

        #region Private Members

        SqlConnection con = new SqlConnection(Utility.ConnectionString);
        string FileName = "";

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            ((AjaxControlToolkit.ToolkitScriptManager)Master.FindControl("tsmManager")).RegisterPostBackControl(ibtn_download);
            ibtn_download.Visible = false;
            ibtn_delete.Visible = false;

            ((AjaxControlToolkit.ToolkitScriptManager)Master.FindControl("tsmManager")).RegisterPostBackControl(cmdDownload);

            if (!IsPostBack)
            {
                LoadFolder();
            }
        }


        protected void ibtnShow_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                string strquery = " Select distinct convert(date,d.createdon) from Document d inner join metadata m on d.metadataid=m.id where  m.MetaTemplateID='" + drpfolder.SelectedItem.Value + "' and d.status=1";
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
            string strquery = "select d.id,d.DocumentName,CONCAT(FORMAT(d.size / 1048576.0, 'N3'), ' MB') as Size,d.createdon,d.pagecount,d.documentpath from Document d inner join metadata m on d.metadataid=m.id where m.CategoryId='" + ddlSubFolder.SelectedItem.Value + "' and m.MetaTemplateID='" + drpfolder.SelectedItem.Value + "' and d.status=1 and convert(date,d.createdon)='" + drpdate.SelectedItem.Text + "'  ";
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

        public static List<DataTable> SplitTable(DataTable originalTable, int batchSize)
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
                                if (dtdate.Rows.Count == 1)
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



        public void LoadFolder()
        {
             string strQuery1=string.Empty;
             strQuery1 = "select ID,MetatemplateName from MetaTemplate where RepositoryID=29 and Status=1";
             DataTable objDataTable = DataHelper.ExecuteDataTable(strQuery1, null);

            if (objDataTable.Rows.Count > 0)
            {
                drpfolder.DataSource = objDataTable;
                drpfolder.DataTextField = "MetatemplateName";
                drpfolder.DataValueField = "ID";
                drpfolder.DataBind();
            }
            if (objDataTable.Rows.Count == 0)
            {
                drpfolder.Items.Clear();
            }
            drpfolder.Items.Insert(0, new ListItem("--SELECT--", "-1"));
        }

        // =============== CODE CHANGE BY RUSHIKESH ============== 18_11_2019

        protected void DownloadFiles(DataTable dt)
        {

            if (dt.Rows.Count > 0)
            {

                GridView1.DataSource = dt;
                Session["Data"] = dt;
                GridView1.DataBind();
                GridView1.Visible = true;
                cmdDownload.Visible = true;
                cmdCancel.Visible = true;
                //try
                //{
                //    int i = 0;
                //    int j = 0;
                //    DataTable dtFull = dt;
                //    List<DataTable> splitdt = new List<DataTable>();
                //    splitdt = SplitTable(dtFull, 100);
                //    using (ZipFile zip = new ZipFile())
                //    {
                //        for (int k = 0; k < splitdt.Count; k++)
                //        {

                //            foreach (var item in splitdt)
                //            {
                //                if (item.Rows.Count == 100)
                //                {
                //                    zip.AlternateEncodingUsage = ZipOption.AsNecessary;
                //                    foreach (DataRow row in item.Rows)
                //                    {
                //                        string FilePath = Convert.ToString(row[4]);
                //                        FileInfo objFile = new FileInfo(FilePath);

                //                        if (File.Exists(FilePath))
                //                        {
                //                            zip.AddFile(FilePath).FileName = (string)Convert.ToString(row[1]);
                //                        }

                //                        else
                //                        {
                //                        }
                //                    }

                //                    j++;
                //                    Response.Clear();
                //                    Response.Buffer = false;
                //                    Response.BufferOutput = false;
                //                    //string zipName = String.Format("Zip_{0}.zip", DateTime.Now.ToString("yyyy-MMM-dd-HHmmss"));
                //                    string zipName = String.Format(drpfolder.SelectedItem.Text + "_" + ddlSubFolder.SelectedItem.Text + j + ".zip");
                //                    Response.AddHeader("content-disposition", "attachment; filename=" + zipName);
                //                    Response.ContentType = "application/zip";                              
                //                    zip.Save(Response.OutputStream);


                //                }

                //            }
                //            Response.End();
                //        }


                //    }
                //}
                //catch (Exception ex)
                //{
                //    UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                //    LogManager.ErrorLog(Utility.LogFilePath, ex);

                //}
            }
            else
            {
                cmdDownload.Visible = false;
                cmdCancel.Visible = false;
            }
           
        }







        protected void drpfolder_SelectedIndexChanged(object sender, EventArgs e)
        {
            //lbldate.Visible = false;
            //drpdate.Visible = false;

            string strQuery1 = "select ID,Categoryname from Category where MetaTemplateID='" + drpfolder.SelectedValue + "' and Status=1";
            //            string strQuery1 = "select distinct ID,foldername from vwfolder where status=1 and metatemplateid=177 order by foldername asc";
            DataTable objDataTable = DataHelper.ExecuteDataTable(strQuery1, null);

            if (objDataTable.Rows.Count > 0)
            {
                ddlSubFolder.DataSource = objDataTable;
                ddlSubFolder.DataTextField = "Categoryname";
                ddlSubFolder.DataValueField = "ID";
                ddlSubFolder.DataBind();
            }
            if (objDataTable.Rows.Count == 0)
            {
                ddlSubFolder.Items.Clear();
            }
            ddlSubFolder.Items.Insert(0, new ListItem("--SELECT--", "-1"));
        }

        protected void ddlSubFolder_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string strquery = " Select distinct convert(date,d.createdon) cdate from Document d inner join metadata m on d.metadataid=m.id where  m.MetaTemplateID='" + drpfolder.SelectedItem.Value + "' and  m.CategoryID='" + ddlSubFolder.SelectedItem.Value + "' and d.status=1 and d.CreatedOn  > '2019-12-23'  order by cdate desc";
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
                    lbldate.Visible = false;
                    drpdate.Visible = false;
                    GridView1.Visible = false;
                    cmdCancel.Visible = false;
                    cmdDownload.Visible = false;


                }
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);

            }
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

            GridView1.PageIndex = e.NewPageIndex;
            GridView1.DataSource = Session["Data"];
            GridView1.DataBind();

        }

        protected void cmdDownload_Click(object sender, EventArgs e)
        {
            try
            {
                using (ZipFile zf = new ZipFile())
                {
                    zf.AlternateEncodingUsage = ZipOption.AsNecessary;
                    zf.AddDirectoryByName("Files");
                    foreach (GridViewRow gvr in GridView1.Rows)
                    {
                        CheckBox ChkAction = (CheckBox)gvr.FindControl("ChkAction");
                        //Label lblFileName = (Label)gvr.FindControl("lblFileName");
                        Label lblFilePath = (Label)gvr.FindControl("lblFilePathAll");
                        if (ChkAction.Checked)
                        {
                            FileName = lblFilePath.Text; //+ "\\" + lblFileName.Text;
                            //zf.AddFile(FileName, "Files");
                            zf.AddFile(FileName, "Files").FileName = (string)GridView1.DataKeys[gvr.RowIndex].Values["DocumentName"];
                        }
                    }
                    Response.Clear();
                    Response.BufferOutput = false;
                    string zipName = String.Format(drpfolder.SelectedItem.Text + "_" + ddlSubFolder.SelectedItem.Text + "_" + DateTime.Now.ToString("yyyy-MMM-dd-HHmmss") + ".zip");
                    Response.ContentType = "application/zip";
                    Response.AddHeader("content-disposition", "attachment; filename=" + zipName);
                    zf.Save(Response.OutputStream);
                    Response.End();
                }
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }



        }

        protected void cmdCancel_Click(object sender, EventArgs e)
        {

        }


    }
}


        #endregion