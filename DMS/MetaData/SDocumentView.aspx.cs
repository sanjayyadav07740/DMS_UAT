using DMS.BusinessLogic;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Data;
using System.Configuration;
using System.Drawing;
using System.Web.Script.Services;
using Newtonsoft.Json;
using System.Text;
using System.Security.Cryptography;
using System.Net.NetworkInformation;
using iTextSharp.text.html.simpleparser;
using System.Net;



namespace DMS.SinglePage
{
    public partial class SDocumentView : System.Web.UI.Page
    {
        // SinglePageManager objsinglePage = new SinglePageManager();
        Utility objUtility = new Utility();
        StringBuilder sb = new StringBuilder();
        string temp;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                ((AjaxControlToolkit.ToolkitScriptManager)Master.FindControl("tsmManager")).RegisterPostBackControl(ibtnSave);

                sb.Append(DateTime.Now.ToString("dd/MMM/yyyy") + " " + DateTime.Now.ToShortTimeString());
                if (!Page.IsPostBack)
                {

                    if (Session["RepositoryName"].ToString() == "CIDCO" || Session["RepositoryName"].ToString() == "Pratik Agarwal")
                    {
                        ibtnSave.Visible = false;
                    }
                    else
                        ibtnSave.Visible = true;

                    //DataTable dt = new DataTable();
                    //DataTable dtTemplateCheck = new DataTable();
                    //DataTable dtPartialDispose = new DataTable();
                    //DataTable dtServiceDetails = new DataTable();

                    //objUtility.Result = objsinglePage.getServiceDetails(out dtServiceDetails, hdRefId.Value, Convert.ToInt32(Session["ServiceType"]));
                    DataTable objTable = new DataTable();
                    objTable = DataHelper.ExecuteDataTable("SELECT DocumentName,DocumentPath,documentguid,PageCount=ISNULL(PageCount,'')+ ' ' + ISNULL(MergedPageCount,'') FROM vwDocument WHERE ID = " + Session["DocId"].ToString(), null);

                    if (objTable.Rows.Count > 0)
                    {
                        ViewState["strfilePathNoting"] = null;
                        ViewState["strfilePathSupplementary"] = null;
                        //if (objTable.Rows.Count == 1)
                        //{
                        //ViewState["strfilePathSupplementary"] = Convert.ToString(dt.Rows[0]["DocumentPath"]);
                        ViewState["strfilePathNoting"] = objTable.Rows[0]["DocumentPath"].ToString();
                        //ViewState["strfilePathNoting"] = @"F:\DMS\DMSDocument\28f14c9a-6002-4cd5-926b-2dd28eb7abf1.pdf";
                        ViewState["DocName"] = objTable.Rows[0]["DocumentName"].ToString();
                        ViewState["DocGuid"] = objTable.Rows[0]["documentguid"].ToString();
                        //}
                        //else
                        //{
                        //    DataRow[] notingvalue = dt.Select("foldername = 'Noting'");
                        //    DataRow[] suplimentaryvalue = dt.Select("foldername = 'Supplementary'");
                        //    ViewState["strfilePathNoting"] = notingvalue[0]["DocumentPath"].ToString();
                        //    //ViewState["strfilePathSupplementary"] = suplimentaryvalue[0]["DocumentPath"].ToString();
                        //    ViewState["DocumentName"] = Convert.ToString(dt.Rows[0]["DocumentName"]);
                        //    //int DocCount = dt.Rows.Count;
                        //    //List<string> firstlist = new List<string>();
                        //    //for(int i=0;i<DocCount-2;i++)
                        //    //{
                        //    //    firstlist.Add(dt.Rows[i]["DocumentPath"].ToString());                            
                        //    //}
                        //}


                        FileInfo objFile = new FileInfo(ViewState["strfilePathNoting"].ToString());
                        if (!objFile.Exists)
                        {
                            // ScriptManager.RegisterStartupScript(this, this.GetType(), "onclick", "alert(' Noting File Not Found.');", true);
                        }
                        else
                        {
                            load();
                        }

                    }
                }






            }

                //if (UserSession.RoleID == 145 || UserSession.RoleID == 146)
            //{
            //    divAttachto.Visible = false;
            //}



            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);

            }

        }


        //protected void ibtnBack_Click(object sender, ImageClickEventArgs e)
        //{

        //    Response.Redirect("../SinglePage/WorkFlowSinglePage.aspx");
        //}



        public void load()
        {

            Report objReport = new Report();
            objReport.InsertAuditLog(GetIPAddress(), GetMacAddress(), "File Fiew", "null", UserSession.UserID);

            //if (Session["upload"] == "documentsearch")
            //{
            byte[] byteFileByte = null;
            //string strfilePath = Convert.ToString(ViewState["strfilePathSupplementary"].ToString());

            FileInfo objFile = new FileInfo(ViewState["strfilePathNoting"].ToString());


            string Destination = Server.MapPath("../PDFVIEWERRAW/") + objFile.Name;
            //string Destination = Server.MapPath("../PDFVIEWERRAW/") + ViewState["DocName"].ToString();
            File.Copy(ViewState["strfilePathNoting"].ToString(), Destination, true);
            //DecryptFile(ViewState["strfilePathNoting"].ToString(), Destination);

            byte[] pdf = File.ReadAllBytes(ViewState["strfilePathNoting"].ToString());
            BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);



            //   if (File.Exists(Destination))
            //File.Copy(Convert.ToString(Session["DocumentPath"].ToString()), Destination, true);
            hdnFile.Value = "../PDFVIEWERRAW/" + objFile.Name;
            //ViewState["strfilePathNoting"].ToString();
            //"../PDFVIEWERRAW/" + objFile.Name;

            hdnFile.Value = "../PDFVIEWERRAW/" + objFile.Name;
            //pdfViewer.FilePath = "../Handler/ImageHandler1.ashx";



            //}

        }












        //protected void ibtnBack_Click1(object sender, EventArgs e)
        //{
        //    Session["FilePath"] = null;
        //    Response.Redirect(ViewState["LASTPAGEURL"].ToString(),false);
        //}



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

        public string GetIPAddress()
        {
            string ipList = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipList))
            {
                return ipList.Split(',')[0];
            }

            return Request.ServerVariables["REMOTE_ADDR"];

        }

        private void DecryptFile(string inputFile, string outputFile)
        {

            {
                string password = @"myKey123"; // Your Key Here

                UnicodeEncoding UE = new UnicodeEncoding();
                byte[] key = UE.GetBytes(password);

                FileStream fsCrypt = new FileStream(inputFile, FileMode.Open);

                RijndaelManaged RMCrypto = new RijndaelManaged();
                RMCrypto.Padding = PaddingMode.None;
                CryptoStream cs = new CryptoStream(fsCrypt,
                    RMCrypto.CreateDecryptor(key, key),
                    CryptoStreamMode.Read);

                FileStream fsOut = new FileStream(outputFile, FileMode.Create);

                int data;
                while ((data = cs.ReadByte()) != -1)
                    fsOut.WriteByte((byte)data);

                fsOut.Close();
                cs.Close();
                fsCrypt.Close();

            }
        }

        private void EncryptFile(string inputFile, string outputFile)
        {

            try
            {
                string password = @"myKey123"; // Your Key Here
                UnicodeEncoding UE = new UnicodeEncoding();
                byte[] key = UE.GetBytes(password);

                string cryptFile = outputFile;
                using (FileStream fsCrypt = new FileStream(cryptFile, FileMode.Create))
                {
                    // FileStream fsCrypt = new FileStream(cryptFile, FileMode.Create);

                    RijndaelManaged RMCrypto = new RijndaelManaged();

                    CryptoStream cs = new CryptoStream(fsCrypt,
                        RMCrypto.CreateEncryptor(key, key),
                        CryptoStreamMode.Write);

                    using (FileStream fsIn = new FileStream(inputFile, FileMode.Open))
                    {
                        int data;
                        while ((data = fsIn.ReadByte()) != -1)
                            cs.WriteByte((byte)data);


                        fsIn.Close();
                        cs.Close();
                        fsCrypt.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void ibtnSave_Click(object sender, ImageClickEventArgs e)
        {
            System.IO.FileInfo file = new System.IO.FileInfo(ViewState["strfilePathNoting"].ToString());
            if (file.Exists)
            {
                Response.Clear();
                Response.ContentType = "application/octet-stream";
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + ViewState["DocName"].ToString());
                // Response.TransmitFile(ViewState["strfilePathNoting"].ToString());
                Response.WriteFile(ViewState["strfilePathNoting"].ToString());
                Report objReport = new Report();
                string IPAddress = GetIPAddress();
                DateTime LoginDate = DateTime.Today;
                // string Activity = "Document Download";
                string MacAddress = GetMacAddress();
                objReport.InsertAuditLog(IPAddress, MacAddress, "File Download", "null", UserSession.UserID);
                //objReport.InsertAuditLogDoc(IPAddress, MacAddress, Activity, Convert.ToString(objDataTable.Rows[0]["DocumentName"]), UserSession.UserID, Convert.ToInt32(Request["DOCID"]));
                Response.Flush();
                Response.End();
            }
            else
            {
                UserSession.DisplayMessage(this, "This file does not exist.", MainMasterPage.MessageType.Error);
            }
        }

        //protected void ibtnBack_Click(object sender, ImageClickEventArgs e)
        //{
        //    if (ViewState["LASTPAGEURL"].ToString().Contains("searchdocument"))
        //    {
        //        Response.Redirect("../MetaData/SearchDocument.aspx", false);
        //    }
        //}




        //protected void ibtnCancle_Click(object sender, ImageClickEventArgs e)
        //{
        //    mpeTemplateForm.Hide();
        //}







        //protected void drpDocumentType_SelectedIndexChanged(object sender, EventArgs e)
        //{


        //}
    }
}