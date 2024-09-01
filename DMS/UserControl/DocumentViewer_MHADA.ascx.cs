using DMS.BusinessLogic;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Management;
using System.Net.NetworkInformation;

namespace DMS.UserControl
{
    public partial class DocumentViewer_MHADA : System.Web.UI.UserControl
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
        protected void ConvertTifToPdf(string FilePath,string FileName)
        {
            try
            {
                // creation of the document with a certain size and certain margins
                iTextSharp.text.Document document = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 0, 0, 0, 0);

                // creation of the different writers
                iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(document, new System.IO.FileStream(Utility.DMSVersionPath.ToString() + FileName.Substring(0, FileName.IndexOf(".")) + ".pdf", System.IO.FileMode.Create));

                // load the tiff image and count the total pages
                System.Drawing.Bitmap bm = new System.Drawing.Bitmap(FilePath);
                int total = bm.GetFrameCount(System.Drawing.Imaging.FrameDimension.Page);

                document.Open();
                iTextSharp.text.pdf.PdfContentByte cb = writer.DirectContent;
                for (int k = 0; k < total; ++k)
                {
                    bm.SelectActiveFrame(System.Drawing.Imaging.FrameDimension.Page, k);
                    iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(bm, System.Drawing.Imaging.ImageFormat.Bmp);
                    // scale the image to fit in the page
                    img.ScalePercent(72f / img.DpiX * 100);
                    img.SetAbsolutePosition(0, 0);
                    cb.AddImage(img);
                    document.NewPage();
                }
                document.Close();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    if (Request.UrlReferrer != null)
                    {
                        ViewState["LASTPAGEURL"] = Request.UrlReferrer.ToString().ToLower();
                    }

                }
                DataTable objTable = new DataTable();
                objTable = DataHelper.ExecuteDataTable("SELECT DocumentName,DocumentPath,PageCount=ISNULL(PageCount,'')+ ' ' + ISNULL(MergedPageCount,'') FROM vwDocument WHERE ID = " + Request["DOCID"].ToString(), null);
                imgImageView.Attributes["src"] = @"../UserControl/PdfHandler.ashx?PageID=1";
                //Session["DocumentPath"] = objTable.Rows[0]["DocumentPath"].ToString();
                ////Utility.DMSVersionPath.ToString();
                //Session["DocumentName"] = objTable.Rows[0]["DocumentName"].ToString();
                string DocName = objTable.Rows[0]["DocumentName"].ToString();
                string FilePath = objTable.Rows[0]["DocumentPath"].ToString();
                if (DocName.Substring(DocName.IndexOf(".")).ToUpper() == ".TIF" || DocName.Substring(DocName.IndexOf(".")).ToUpper() == ".TIFF")
                {
                    ConvertTifToPdf(FilePath, DocName);
                    //if (ViewState["LASTPAGEURL"].ToString().ToLower().Contains("searchmsib"))
                    //{
                    Session["DocumentPath"] = Utility.DMSVersionPath.ToString() + DocName.Substring(0, DocName.IndexOf(".")) + ".pdf";
                    FilePath = Session["DocumentPath"].ToString();
                    Session["DocumentName"] = DocName.Substring(0, DocName.IndexOf(".")) + ".pdf";
                }
                else
                {
                    Session["DocumentPath"] = FilePath;
                    FilePath = Session["DocumentPath"].ToString();
                    Session["DocumentName"] = DocName;
                }

                //}
                if (File.Exists(FilePath))
                {
                    if (Path.GetExtension(FilePath).ToLower() == ".pdf")
                    {
                        PdfReader objPdfReader = new PdfReader(FilePath);
                        hdfTotalPageNumber.Value = objPdfReader.NumberOfPages.ToString();
                        objPdfReader.Close();
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
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage((Page)this.Parent.Page, "Sorry ,Some Error Has Been Occured While Processing File.", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }
        protected void ibtnBack_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                // DataTable objTable = DataHelper.ExecuteDataTable("update PACL_DEEDS set REMARKS='"+txtRemarks.Text+"' WHERE DOCUMENTID = " + Request["DOCID"].ToString(), null);
                if (ViewState["LASTPAGEURL"] != null)
                {
                    if (ViewState["LASTPAGEURL"].ToString().Contains("searchmsib"))
                    {
                        Response.Redirect("../MHADA Website Search/SearchMSIB.aspx", false);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetIPAddress()//gets the IP address of host(user in this case)
        {
            string Address="";
            IPHostEntry Host = default(IPHostEntry);
            string Hostname = null;
            Hostname = System.Environment.MachineName;
            Host = Dns.GetHostEntry(Hostname);
            foreach (IPAddress IP in Host.AddressList)
            {
                if (IP.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    Address = Convert.ToString(IP);
                }
            }
            return Address;
        }

        public string GetMACAddress()
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            String sMacAddress = string.Empty;
            foreach (NetworkInterface adapter in nics)
            {
                if (sMacAddress == String.Empty)// only return MAC Address from first card  
                {
                    IPInterfaceProperties properties = adapter.GetIPProperties();
                    sMacAddress = adapter.GetPhysicalAddress().ToString();
                }
            } return sMacAddress;
        }

        protected void ibtnSave_Click(object sender, ImageClickEventArgs e)
        {
            string IPAddress_Host = GetIPAddress();
            string MAC_Address = GetMACAddress();
            string Activity = "Save";
            int DocID =Convert.ToInt32(Request["DOCID"].ToString());
            DMS.BusinessLogic.ReportManager objReport = new ReportManager();
            objReport.InsertAuditLog_MhadaWebsite(IPAddress_Host, MAC_Address, Activity, DocID);
        }
    }
}