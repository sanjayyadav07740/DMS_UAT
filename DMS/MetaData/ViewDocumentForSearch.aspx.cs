using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DMS.BusinessLogic;
using System.IO;
using iTextSharp.text.pdf;
using System.Drawing;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Net;
using System.Net.NetworkInformation;

namespace DMS.Shared
{
    public partial class ViewDocumentForSearch : System.Web.UI.Page
    {
        Utility objUtility = new Utility();
        Document objdocument = new Document();
        SqlConnection con = new SqlConnection(Utility.ConnectionString);
        DocumentManager objDocumentManager = new DocumentManager();

        protected void Page_Load(object sender, EventArgs e)
        {
            ((AjaxControlToolkit.ToolkitScriptManager)Master.FindControl("tsmManager")).RegisterPostBackControl(ibtnDownload);

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

                if (Session["DocId"] != null )
                {
                    //#region insert auditlog

                    ////string strHostName = "";
                    ////strHostName = System.Net.Dns.GetHostName();

                    ////IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(strHostName);

                    ////IPAddress[] addr = ipEntry.AddressList;
                    //Report objReport = new Report();
                    //string IPAddress = GetIPAddress();
                    ////DateTime Date = Convert.ToDateTime(DateTime.Today.ToShortDateString());
                    ////DateTime Logintime = Convert.ToDateTime(DateTime.Now);
                    //DateTime LoginDate = DateTime.Today;
                    //string Activity = "View Document";

                    //string MacAddress = GetMacAddress();
                     //objReport.InsertAuditLog(IPAddress, MacAddress, Activity, Session["DocumentName"].ToString(), UserSession.UserID);
                    //#endregion
                    //string Tagname =
                        pdfViewer.FilePath = "../Handler/PDFHandler.ashx?DocID=" + Session["DocId"].ToString() +"";

                    pdfViewer.Visible = true;
                    //pdfViewer.FilePath = "../UserControl/PDFHandler.ashx?DOCID=" + Session["DocId"].ToString() + "&DocumentName="+Session["DocName"].ToString()+"";

                }

                //if (Session["DocId"] != null && Session["PageNoFrom"] != null && Session["PageNoTo"] == null)
                //{
                //    pdfViewer.FilePath = "../UserControl/PDFHandler.ashx?DOCID=" + Session["DocId"].ToString() + "&FROM=" + Session["PageNoFrom"].ToString() + "&TO=" + Session["PageNoFrom"].ToString() + "";

                //}
                //if (Session["DocId"] != null && Session["PageNoFrom"] != null && Session["PageNoTo"] != null )
                //{
                //    pdfViewer.FilePath = "../UserControl/PDFHandler.ashx?DOCID=" + Session["DocId"].ToString() + "&FROM=" + Session["PageNoFrom"].ToString() + "&TO=" + Session["PageNoTo"].ToString() + "";

                //}

                #region Seema 27 June 2016
                if (Request.UrlReferrer != null)
                {
                    ViewState["Backbutton"] = Request.UrlReferrer.ToString().ToLower();
                }
                #endregion
            }

            if (ViewState["LASTPAGEURL"]!=null)
            {
                if (ViewState["LASTPAGEURL"].ToString().ToLower().Contains("documentverificationcentrum"))
                {
                    ibtnApprove.Visible = true;
                    ibtnReject.Visible = true;
                }
            }
        }
        public string GetIPAddress()
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
        protected void ibtnBack_Click(object sender, ImageClickEventArgs e)
        {
            #region Sushil; 31-july-2017
            try
            {
                if (ViewState["Backbutton"] != null)
                {
                    if (ViewState["LASTPAGEURL"].ToString().Contains("documententry"))
                    {
                        Response.Redirect("../MetaData/ViewForDocumentEntry.aspx", false);
                    }
                      else if (ViewState["LASTPAGEURL"].ToString().Contains("searchdocumentcentrum"))
                    {
                        Response.Redirect("../MetaData/SearchDocumentCentrum.aspx", false);
                    }
                    //else if (ViewState["LASTPAGEURL"].ToString().Contains("searchinallcentrum"))
                    //{
                    //    Response.Redirect("../MetaData/SearchInAllCentrum.aspx", false);
                    //}
                    else if (ViewState["LASTPAGEURL"].ToString().Contains("searchinallcentrumnew"))
                    {
                        Response.Redirect("../MetaData/SearchInAllCentrumNew.aspx", false);
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
                    else if (ViewState["LASTPAGEURL"].ToString().Contains("documentsearch_idbi_ahm"))
                    {
                        Response.Redirect("../MetaData/DocumentSearch_IDBI_Ahm.aspx", false);
                    }
                    else if (ViewState["LASTPAGEURL"].ToString().Contains("documentsearch"))
                    {
                        Response.Redirect("../MetaData/DocumentSearch.aspx", false);
                    }
                    else if (ViewState["LASTPAGEURL"].ToString().Contains("split"))
                    {
                        Response.Redirect("../MetaData/SplitDocument.aspx", false);
                    }
                    else if (ViewState["LASTPAGEURL"].ToString().Contains("inbetween"))
                    {
                        Response.Redirect("../MetaData/MergeDocumentInBetween.aspx", false);
                    }
                    else if (ViewState["LASTPAGEURL"].ToString().Contains("renamae"))
                    {
                        Response.Redirect("../MetaData/DocumentRename.aspx", false);
                    }
                                      
                }
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
            }
            #endregion

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
                        Session["Approve"] = "approve";
                        Response.Redirect("../MetaData/DocumentVerificationCentrum.aspx", false);
                        UserSession.DisplayMessage(this, "Document is Approved Successfully .", MainMasterPage.MessageType.Success);
                        break;
                    case Utility.ResultType.Failure:
                    case Utility.ResultType.Error:
                        objDbTransaction.Rollback();
                        UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                        break;
                }


            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
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
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
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
                        Session["Reject"] = "reject";
                        Response.Redirect("../MetaData/DocumentVerificationCentrum.aspx", false);
                      
                        break;
                    case Utility.ResultType.Failure:
                    case Utility.ResultType.Error:
                        objDbTransaction.Rollback();
                        UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                        break;
                }


            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
            }
        }

        protected void ibtnDownload_Click(object sender, ImageClickEventArgs e)
        {
            DataTable dt = new DataTable();
            dt = Document.GetDocumentPath(Convert.ToInt32(Request["DOCID"].ToString()));
            Session["DocumentName"] = dt.Rows[0]["DocumentName"].ToString();
            Session["DocumentPath"] = dt.Rows[0]["DocumentPath"].ToString();
            //string strDocumentPath = gvwDocumentVersion.DataKeys[intRowIndex].Values["DocumentPath"].ToString();
            //string strDocumentName = gvwDocumentVersion.DataKeys[intRowIndex].Values["DocumentName"].ToString();

            if (!File.Exists(Session["DocumentPath"].ToString()))
            {
                //UserSession.DisplayMessage(this.Parent.Page, "File Not Found .", MainMasterPage.MessageType.Warning);
                UserSession.DisplayMessage(this, "Sorry ,File Not Found .", MainMasterPage.MessageType.Warning);
                return;
            }

            Response.ContentType = "application/pdf";
            Response.AppendHeader("content-disposition", "attachment;filename=\"" + Session["DocumentName"].ToString() + "\"");
            Response.TransmitFile(Session["DocumentPath"].ToString());
            Response.Flush();
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
    }
}