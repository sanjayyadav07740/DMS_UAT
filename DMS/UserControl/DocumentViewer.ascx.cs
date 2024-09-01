using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using iTextSharp.text.pdf;
using System.Drawing;
using DMS.BusinessLogic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Net;
using System.Net.NetworkInformation;
using System.Configuration;

namespace DMS.UserControl
{
    public partial class DocumentViewer : System.Web.UI.UserControl
    {
        public string Width
        {
            get
            {
                return ViewState["Width"] == null ? string.Empty : ViewState["Width"].ToString();
            }
            set
            {
                ViewState["Width"] = value;
            }
        }

        public string Height
        {
            get
            {
                return ViewState["Height"] == null ? string.Empty : ViewState["Height"].ToString();
            }
            set
            {
                ViewState["Height"] = value;
            }
        }

        Utility objUtility = new Utility();
        Document objdocument = new Document();
        DocumentManager objDocumentManager = new DocumentManager();
        SqlConnection con = new SqlConnection(Utility.ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                int roleId = UserSession.RoleID;
                RolePermission objrolepermission = new RolePermission();
               // DataTable objdatatable=new DataTable();
                DataSet ds = new DataSet();
                ds = DMS.BusinessLogic.MetaData.SelectRepName(UserSession.MetaDataID);
                //get no of documents downloaded by IDBI CPU
                DataTable dt=new DataTable();
                dt=DocumentManager.GetIDBIDownloadCount();
                
                DataTable objdatatable = new DataTable();
                objUtility.Result = objDocumentManager.ValidateUser(out objdatatable, Convert.ToInt32(UserSession.UserID));
                switch (objUtility.Result)
                {
                    case Utility.ResultType.Success:
                        divSave.Visible = true;
                        break;
                    case Utility.ResultType.Failure:
                        divSave.Visible = false;
                        break;
                }
               
                if (!IsPostBack)
                {
                    if (Request.UrlReferrer != null)
                    {
                        ViewState["LASTPAGEURL"] = Request.UrlReferrer.ToString().ToLower();
                    }

                }
                if(ViewState["LASTPAGEURL"].ToString().ToLower().Contains("documentverificationcentrum"))
                {
                    ibtnApprove.Visible = true;
                    ibtnReject.Visible = true;
                }
                

                #region Seema 7Dec 2017
                // For HGS international merge(pagecount for viewer,sum of merge pages and original page is total pagecount)
                DataTable objTable = new DataTable();
                if (ds.Tables[0].Rows[0]["RepositoryName"].ToString() == "HGS International")
                {
                    objTable = DataHelper.ExecuteDataTable("SELECT DocumentName,DocumentPath,PageCount=ISNULL(PageCount,'')+ ' ' + ISNULL(MergedPageCount,'') FROM vwDocument WHERE ID = " + Request["DOCID"].ToString(), null);
                }
                else
                {
                    objTable = DataHelper.ExecuteDataTable("SELECT DocumentName,DocumentPath,PageCount=ISNULL(PageCount,'')+ ' ' + ISNULL(MergedPageCount,'') FROM vwDocument WHERE ID = " + Request["DOCID"].ToString(), null);
                }
                #endregion
                imgImageView.Attributes["src"] = @"../UserControl/PdfHandler.ashx?PageID=1";
                Session["DocumentPath"] = objTable.Rows[0]["DocumentPath"].ToString();
                Session["DocumentName"] = objTable.Rows[0]["DocumentName"].ToString();
                string FilePath = objTable.Rows[0]["DocumentPath"].ToString();
                if (File.Exists(FilePath))
                {
                    if (Path.GetExtension(FilePath).ToLower() == ".pdf")
                    {
                       
                        PdfReader objPdfReader = new PdfReader(FilePath);
                        hdfTotalPageNumber.Value = objPdfReader.NumberOfPages.ToString();
                        objPdfReader.Close();
                        //iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance("../Images/watermark.jpg");
                        //img.SetAbsolutePosition(125, 300); // set the position in the document where you want the watermark to appear (0,0 = bottom left corner of the page)


                        //PdfStamper pdfStamper = new PdfStamper(objPdfReader, new FileStream(Utility.DocumentPath, FileMode.Create, FileAccess.Write, FileShare.None));
                        //PdfContentByte waterMark;
                        //for (int page = 1; page <= objPdfReader.NumberOfPages; page++)
                        //{
                        //    waterMark = pdfStamper.GetOverContent(page);
                        //    waterMark.AddImage(img);
                        //}
                    }
                    else if (Path.GetExtension(FilePath).ToLower() == ".docx" || Path.GetExtension(FilePath).ToLower() == ".doc" || Path.GetExtension(FilePath).ToLower() == ".xlsx" || Path.GetExtension(FilePath).ToLower() == ".xls" || Path.GetExtension(FilePath).ToLower() == ".ppt" || Path.GetExtension(FilePath).ToLower() == ".pptx") //Added by Mayuresh
                    {
                        UserSession.DisplayMessage((Page)this.Parent.Page, "Selected File is not viewable.", MainMasterPage.MessageType.Warning);
                    }
                    else
                    {
                        Bitmap objBitmap = new Bitmap(FilePath);
                        hdfTotalPageNumber.Value = objBitmap.GetFrameCount(System.Drawing.Imaging.FrameDimension.Page).ToString();
                            //objTable.Rows[0]["PageCount"].ToString();
                            //
                        objBitmap.Dispose();
                    }
                    hdfCurrentPageNumber.Value = "1";
                    txtCurrentPageNumber.Value = "1";
                    bTotalPageNumber.InnerText = hdfTotalPageNumber.Value;
                }
                #region insert auditlog

                //string strHostName = "";
                //strHostName = System.Net.Dns.GetHostName();

                //IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(strHostName);

                //IPAddress[] addr = ipEntry.AddressList;
                Report objReport = new Report();
                string IPAddress = GetIPAddress(HttpContext.Current);
                //DateTime Date = Convert.ToDateTime(DateTime.Today.ToShortDateString());
                //DateTime Logintime = Convert.ToDateTime(DateTime.Now);
                DateTime LoginDate = DateTime.Today;
                string Activity = "View Document";

                string MacAddress = GetMacAddress();
                objReport.InsertAuditLog(IPAddress, MacAddress, Activity, Session["DocumentName"].ToString(), UserSession.UserID);
                #endregion
               // lblRemarks.Visible = false;
                //txtRemarks.Visible = false;
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage((Page)this.Parent.Page, "Sorry ,Some Error Has Been Occured While Processing File.", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        public string GetIPAddress(HttpContext context)
        {
            string ipList = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipList))
            {
                return ipList.Split(',')[0];
            }

            return context.Request.ServerVariables["REMOTE_ADDR"];

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
        protected void ibtnBack_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
               // DataTable objTable = DataHelper.ExecuteDataTable("update PACL_DEEDS set REMARKS='"+txtRemarks.Text+"' WHERE DOCUMENTID = " + Request["DOCID"].ToString(), null);
                if (ViewState["LASTPAGEURL"] != null)
                {
                    if (ViewState["LASTPAGEURL"].ToString().Contains("documententry"))
                    {
                        Response.Redirect("../MetaData/ViewForDocumentEntry.aspx", false);
                    }
                    else if (ViewState["LASTPAGEURL"].ToString().Contains("searchdocumentcentrum"))
                    {
                        Response.Redirect("../MetaData/SearchDocumentCentrum.aspx", false);
                    }
                    else if (ViewState["LASTPAGEURL"].ToString().Contains("searchinallcentrum"))
                    {
                        Response.Redirect("../MetaData/SearchInAllCentrum.aspx", false);
                    }
                    else if (ViewState["LASTPAGEURL"].ToString().Contains("documentverificationcentrum"))
                    {
                        Response.Redirect("../MetaData/DocumentVerificationCentrum.aspx", false);
                    }
                    else if (ViewState["LASTPAGEURL"].ToString().Contains("documentverification"))
                    {
                        Response.Redirect("../MetaData/ViewForDocumentVerification.aspx", false);
                    }
                    else if (ViewState["LASTPAGEURL"].ToString().Contains("approveddocument"))
                    {
                        Response.Redirect("../MetaData/ViewForApprovedDocument.aspx", false);
                    }
                    else if (ViewState["LASTPAGEURL"].ToString().Contains("rejecteddocument"))
                    {
                        Response.Redirect("../MetaData/RejectedDocument.aspx", false);
                    }
                    else if (ViewState["LASTPAGEURL"].ToString().Contains("documentdashboard"))
                    {
                        Response.Redirect("../MetaData/DocumentDashBoard.aspx", false);
                    }
                    else if (ViewState["LASTPAGEURL"].ToString().Contains("searchdocument"))
                    {
                        Response.Redirect("../MetaData/SearchDocument.aspx", false);
                    }
                    else if (ViewState["LASTPAGEURL"].ToString().Contains("mutifieldsearch"))
                    {
                        Response.Redirect("../MetaData/MutiFieldSearch.aspx", false);
                    }
                    else if (ViewState["LASTPAGEURL"].ToString().Contains("rename"))
                    {
                        Response.Redirect("../MetaData/DocumentRename.aspx", false);
                    }
                    else if (ViewState["LASTPAGEURL"].ToString().Contains("inbetween"))
                    {
                        Response.Redirect("../MetaData/MergeDocumentInBetween.aspx", false);
                    }
                    else if (ViewState["LASTPAGEURL"].ToString().Contains("split"))
                    {
                        Response.Redirect("../MetaData/SplitDocument.aspx", false);
                    }
                    else if (ViewState["LASTPAGEURL"].ToString().Contains("viewshareddocuments"))
                    {
                        Response.Redirect("../Shared/SharedDocumentViewer.aspx", false);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        protected void ibtnApprove_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                // string username=SelectUserName();
                DbTransaction objDbTransaction = Utility.GetTransaction;
                DocumentManager objDocumentManager = new DocumentManager();
                DMS.BusinessLogic.DocumentStatus objDocumentStaus = new DMS.BusinessLogic.DocumentStatus();
                if (Session["DocIdVerify"] != null)
                {
                    objDocumentStaus.DocId = Convert.ToInt32(Session["DocIdVerify"]);
                }
                objDocumentStaus.UserName = SelectUserName();
                objDocumentStaus.statusAproveRej = "Approve";
                objDocumentStaus.UserId = UserSession.UserID;
                //objDocumentStaus.ApprovedOn=DateTime.Now;               
                objUtility.Result = objDocumentManager.InsertDocApproveRejectDetails(objDocumentStaus, objDbTransaction);

                switch (objUtility.Result)
                {
                    case Utility.ResultType.Success:
                        objDbTransaction.Commit();
                        UserSession.DisplayMessage((Page)this.Parent.Page, "Document is Approved Successfully .", MainMasterPage.MessageType.Success);
                        Response.Redirect("../MetaData/DocumentVerificationCentrum.aspx", false);
                        break;
                    case Utility.ResultType.Failure:
                    case Utility.ResultType.Error:
                        objDbTransaction.Rollback();
                        UserSession.DisplayMessage((Page)this.Parent.Page, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                        break;
                }


            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                UserSession.DisplayMessage((Page)this.Parent.Page, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
            }
        }

        public string SelectUserName()
        {
            try
            {
                int UserId = UserSession.UserID;
                string strQuery = "select UserName from vwUser where Status=1 and id=" + UserId;
                SqlCommand com = new SqlCommand(strQuery, con);
                SqlDataAdapter da = new SqlDataAdapter(com);
                DataTable dtUserName = new DataTable();
                da.Fill(dtUserName);
                string username = dtUserName.Rows[0][0].ToString();
                return username;
            }
            catch (Exception ex)
            {
                return null;
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                UserSession.DisplayMessage((Page)this.Parent.Page, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
            }
        }

        protected void ibtnReject_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                DbTransaction objDbTransaction = Utility.GetTransaction;
                DocumentManager objDocumentManager = new DocumentManager();
                DMS.BusinessLogic.DocumentStatus objDocumentStaus = new DMS.BusinessLogic.DocumentStatus();
                if (Session["DocIdVerify"] != null)
                {
                    objDocumentStaus.DocId = Convert.ToInt32(Session["DocIdVerify"]);
                }
                objDocumentStaus.UserName = SelectUserName();
                objDocumentStaus.statusAproveRej = "Reject";
                objDocumentStaus.UserId = UserSession.UserID;
                //objDocumentStaus.ApprovedOn=DateTime.Now;               
                objUtility.Result = objDocumentManager.InsertDocApproveRejectDetails(objDocumentStaus, objDbTransaction);

                switch (objUtility.Result)
                {
                    case Utility.ResultType.Success:
                        objDbTransaction.Commit();
                        UserSession.DisplayMessage((Page)this.Parent.Page, "Document is Rejected Successfully .", MainMasterPage.MessageType.Success);
                        Response.Redirect("../MetaData/DocumentVerificationCentrum.aspx", false);
                        break;
                    case Utility.ResultType.Failure:
                    case Utility.ResultType.Error:
                        objDbTransaction.Rollback();
                        UserSession.DisplayMessage((Page)this.Parent.Page, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                        break;
                }


            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                UserSession.DisplayMessage((Page)this.Parent.Page, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
            }
        }

        protected void ibtnSave_Click(object sender, ImageClickEventArgs e)
        {
            string Flag = "";
            //if(Request.QueryString["Points"]==null)
            //{
            //    Flag = Request.QueryString["Points"];
                #region insert IDBI Download Log
                DbTransaction objDbTransaction = Utility.GetTransaction;
                int DocID = Convert.ToInt32(Request["DOCID"].ToString());
                DocumentManager objDocumentmanager = new DocumentManager();
                objUtility.Result = objDocumentmanager.InsertIDBIDownloadLog(DocID, objDbTransaction);
                switch (objUtility.Result)
                {
                    case Utility.ResultType.Success:
                        objDbTransaction.Commit();

                        break;
                    case Utility.ResultType.Failure:
                    case Utility.ResultType.Error:
                        objDbTransaction.Rollback();

                        break;
                }
                #endregion

                #region insert auditlog

                //string strHostName = "";
                //strHostName = System.Net.Dns.GetHostName();

                //IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(strHostName);

                //IPAddress[] addr = ipEntry.AddressList;
                Report objReport = new Report();
                string IPAddress = GetIPAddress(HttpContext.Current);
                //DateTime Date = Convert.ToDateTime(DateTime.Today.ToShortDateString());
                //DateTime Logintime = Convert.ToDateTime(DateTime.Now);
                DateTime LoginDate = DateTime.Today;
                string Activity = "Download Document";

                string MacAddress = GetMacAddress();
                objReport.InsertAuditLog(IPAddress, MacAddress, Activity, Session["DocumentName"].ToString(), UserSession.UserID);
                #endregion
                       
        }

      
        //protected void imgSendMail_Click(object sender, ImageClickEventArgs e)
        //{
        //    Response.Redirect("../MetaData/SendMail.aspx", false);
        //}

        //protected void imgAddRemarks_Click(object sender, ImageClickEventArgs e)
        //{
          //  lblRemarks.Visible = true;
            //txtRemarks.Visible = true;
            //DataTable objTable = DataHelper.ExecuteDataTable("SELECT REMARKS FROM PACL_DEEDS WHERE DOCUMENTID = " + Request["DOCID"].ToString(), null);
            //txtRemarks.Text = objTable.Rows[0]["REMARKS"].ToString();
        //}
    }
}