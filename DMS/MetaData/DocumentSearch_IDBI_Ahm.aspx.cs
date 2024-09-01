using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Data;
using System.Text;
using DMS.BusinessLogic;
using System.Net.NetworkInformation;
using System.Net;
namespace DMS.Shared
{
    public partial class DocumentSearch_IDBI_Ahm : System.Web.UI.Page
    {
        string strDocumentID;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        
        protected void ibtnSearch_Click(object sender, ImageClickEventArgs e)
        {
           
            if(txtCustId.Text=="" && txtAccNo.Text=="" && txtBarCode.Text==""&& txtBoxNo.Text=="")
            {
                UserSession.DisplayMessage(this, "Please enter atleast one search criteria", MainMasterPage.MessageType.Warning);
                return;
            }
            TextBox[] txts = new TextBox[] { txtCustId, txtAccNo, txtBarCode,txtBoxNo };
            DataSet dsResult = new DataSet();
            dsResult = FillGrid(txts, out dsResult);
            if(dsResult.Tables[0].Rows.Count>0)
            {
                gvwDocument.DataSource = dsResult.Tables[0];
                gvwDocument.DataBind();
                gvwDocument.Visible = true;
            }
            else
            {
                UserSession.DisplayMessage(this, "No document exists for entered criteria!", MainMasterPage.MessageType.Error);
                gvwDocument.Visible = false;
            }
        }

        public DataSet FillGrid(TextBox[] txtArray, out DataSet dsResult)
        {
            string query = "";
            char charQuote = '"';
            StringBuilder txtWhere = new StringBuilder();
            txtWhere.Append("select D.ID,D.MetaDataID,D.DocumentName,M.MetaDataCode,D.DocumentType,D.DocumentStatusID,(SELECT StatusName FROM DocumentStatus WHERE ID= D.DocumentStatusID )as documentstatus " +
                            ",E.SHCIL_Barcode_Date,D.Tag,D.PageCount " +
                            "from Document D inner join ExcelEntry_IDBI_Ahm E " +
                            "on D.documentname=(E.shcil_barcode+'.pdf')inner join MetaData M " +
                            "on D.MetaDataID=M.ID where ");
            foreach (TextBox txt in txtArray)
            {
                switch (txt.ID)
                {
                    case "txtCustId":
                        if (!string.IsNullOrEmpty(txtCustId.Text))
                        {
                            txtWhere.Append("E.custid like '%" + txtCustId.Text+"%' and ");
                        }
                        break;
                    case "txtAccNo":
                        if (!string.IsNullOrEmpty(txtAccNo.Text))
                        {
                            txtWhere.Append("E.AccNo like '%" + txtAccNo.Text+"%' and ");
                        }
                        break;
                    case "txtBarCode":
                        if (!string.IsNullOrEmpty(txtBarCode.Text))
                        {
                            txtWhere.Append("E.SHCIL_Barcode like '%" + txtBarCode.Text+"%' and ");
                        }
                        break;
                    case "txtBoxNo":
                        if (!string.IsNullOrEmpty(txtBoxNo.Text))
                        {
                            txtWhere.Append("E.BoxNo like '%" + txtBoxNo.Text + "%' and ");
                        }
                        break;
                }
            }
            if (txtWhere.Length == 0)
            {
                dsResult = null;

            }
            else
            {
                txtWhere.Append("D.Status=1");
               // txtWhere = txtWhere.Remove(txtWhere.Length - 4, 4);
                txtWhere.ToString();
                //txtWhere.Append(")");
                //txtWhere.ToString();
                dsResult = DataHelper.ExecuteDataSet(txtWhere.ToString());
                UserSession.GridData = dsResult.Tables[0];
            }
            return dsResult;
        }

        protected void txtCustId_TextChanged(object sender, EventArgs e)
        {
            Regex regex = new Regex(@"[0-9]");
            if(!regex.IsMatch(txtCustId.Text))
           {
                UserSession.DisplayMessage(this, "Please enter only numbers", MainMasterPage.MessageType.Warning);
                return;
            }
        }

        protected void txtAccNo_TextChanged(object sender, EventArgs e)
        {
            Regex regex = new Regex(@"[0-9]");
            if (!regex.IsMatch(txtAccNo.Text))
            {
                UserSession.DisplayMessage(this, "Please enter only numbers", MainMasterPage.MessageType.Warning);
                return;
            }
        }

        protected void txtBoxNo_TextChanged(object sender, EventArgs e)
        {
            Regex regex = new Regex(@"[0-9]");
            if (!regex.IsMatch(txtBoxNo.Text))
            {
                UserSession.DisplayMessage(this, "Please enter only numbers", MainMasterPage.MessageType.Warning);
                return;
            }
        }

        protected void gvwDocument_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                if (UserSession.GridData != null)
                {
                    gvwDocument.PageIndex = e.NewPageIndex;
                    if (UserSession.FilterData == null)
                        gvwDocument.DataSource = UserSession.GridData;
                    else
                        gvwDocument.DataSource = UserSession.FilterData;

                    gvwDocument.DataBind();
                }
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
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

        protected void gvwDocument_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.ToLower().Trim() == "documentsearch")
                {

                    int intRowIndex = Convert.ToInt32(e.CommandArgument);
                    UserSession.MetaDataID = Convert.ToInt32(gvwDocument.DataKeys[intRowIndex].Values["MetaDataID"]);
                    strDocumentID = gvwDocument.DataKeys[intRowIndex].Values["ID"].ToString().Trim();
                    string strStatus = gvwDocument.DataKeys[intRowIndex].Values["DocumentStatusID"].ToString().Trim();
                    string DocName = gvwDocument.DataKeys[intRowIndex].Values["DocumentName"].ToString().Trim();
                    Session["DocId"] = strDocumentID;
                    Session["DocumentName"] = DocName;
                    DataSet ds = new DataSet();
                    ds = DMS.BusinessLogic.MetaData.SelectRepName(UserSession.MetaDataID);
                    #region insert auditlog

                    //string strHostName = "";
                    //strHostName = System.Net.Dns.GetHostName();

                    //IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(strHostName);

                    //IPAddress[] addr = ipEntry.AddressList;
                    Report objReport = new Report();
                    string IPAddress = GetIPAddress();
                    //DateTime Date = Convert.ToDateTime(DateTime.Today.ToShortDateString());
                    //DateTime Logintime = Convert.ToDateTime(DateTime.Now);
                    DateTime LoginDate = DateTime.Today;
                    string Activity = "Search Document";

                    string MacAddress = GetMacAddress();
                    objReport.InsertAuditLog(IPAddress, MacAddress, Activity, DocName, UserSession.UserID);
                    #endregion
                    //if (ds.Tables[0].Rows[0]["RepositoryName"].ToString() == "Centrum Wealth Management Ltd")
                    if (ds.Tables[0].Rows[0]["RepositoryName"].ToString() == "MHADA" || ds.Tables[0].Rows[0]["RepositoryName"].ToString() == "IDBI Bank Ltd" || ds.Tables[0].Rows[0]["RepositoryName"].ToString() == "IDBI CPU"||ds.Tables[0].Rows[0]["RepositoryName"].ToString() =="IDBI Ahmedabad")
                    {
                        //for pdf viewer

                        Response.Redirect("../MetaData/ViewDocumentForSearch.aspx?DOCID=" + strDocumentID + " DocumentName=" + DocName, false);

                    }
                    else
                    {
                        //for image viewer
                        Response.Redirect("../MetaData/ViewDocumentForSearch.aspx?DOCID=" + strDocumentID + " DocumentName=" + DocName, false);

                    }
                    
                }
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }
    }
}