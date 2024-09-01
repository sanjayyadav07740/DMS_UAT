using DMS.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DMS.MHADA_Website_Search
{
    public partial class SearchMSIB : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                FillYear();
            }
        }

        public void FillYear()//adds year in the drop down for year
        {
            ddlYear.Items.Clear();
            ddlYear.Items.Add("--Select--");
            var currentYear = DateTime.Today.Year;
            for (int i = 28; i >= 0; i--)
            {
                // Now just add an entry that's the current year minus the counter
                ddlYear.Items.Add((currentYear - i).ToString());
            }
        }

        public void TotalDocs()
        {
            try
            {
                 string Zone = ddlZone.SelectedItem.Text;
                DataSet dsTotal=new DataSet();
                string strQuery = "";
                switch(Zone)
                {
                    case "East":
                        strQuery = "select count(1) as total from document where documentname like 'MSIB EE East%' and status=1 ";
                        dsTotal = DataHelper.ExecuteDataSet(strQuery);
                        break;
                    case "West":
                        strQuery = "select count(1) as total from document where documentname like 'MSIB EE West%' and status=1 ";
                        dsTotal = DataHelper.ExecuteDataSet(strQuery);
                        break;
                    case "City":
                        strQuery = "select count(1) as total from document where documentname like 'MSIB EE City%' and status=1 ";
                        dsTotal = DataHelper.ExecuteDataSet(strQuery);
                        break;
                }
                if (dsTotal.Tables[0].Rows.Count > 0)
                {
                    lblTotal.Text = dsTotal.Tables[0].Rows[0]["total"].ToString();
                    //UserSession.GridData = dsResult.Tables[0];
                    //grvDocument.DataSource = dsResult.Tables[0];
                    //grvDocument.DataBind();
                }
                else
                    lblTotal.Text = "";
                
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public void Search()//searches document
        {
            try
            {
                string strQueryEast = "";
                string strQueryWest = "";
                string strQueryCity = "";
                string Zone = ddlZone.SelectedItem.Text;
                DataSet dsResult = new DataSet();
               
                
             
                switch(Zone)
                {
                    case "East":
                        strQueryEast = "select id,documentname from document where documentname like 'MSIB EE East%' and status=1 ";
                        
                        if (txtAggNo.Text != "" && txtSubName.Text == "" && ddlYear.SelectedItem.Text == "--Select--" && txtArea.Text == "")
                        {
                            strQueryEast = strQueryEast + "and documentname like '%" + txtAggNo.Text + "%'";
                        }
                        if (txtAggNo.Text == "" && txtSubName.Text != "" && ddlYear.SelectedItem.Text == "--Select--" && txtArea.Text == "")
                        {
                            strQueryEast = strQueryEast + "and documentname like '%" + txtSubName.Text + "%'";
                        }
                        if (txtAggNo.Text == "" && txtSubName.Text == "" && ddlYear.SelectedItem.Text != "--Select--" && txtArea.Text == "")
                        {
                            strQueryEast = strQueryEast + "and documentname like '%" + ddlYear.SelectedItem.Text + "%'";
                        }
                        if (txtAggNo.Text == "" && txtSubName.Text == "" && ddlYear.SelectedItem.Text == "--Select--" && txtArea.Text != "")
                        {
                            strQueryEast = strQueryEast + "and documentname like '%" + txtArea.Text + "%'";
                        }
                        if (txtAggNo.Text != "" && txtSubName.Text != "" && ddlYear.SelectedItem.Text == "--Select--" && txtArea.Text == "")
                        {
                            strQueryEast = strQueryEast + "and documentname like '%" + txtAggNo.Text + "%' and documentname like '%" + txtSubName.Text + "%'";
                        }
                        if (txtAggNo.Text != "" && txtSubName.Text == "" && ddlYear.SelectedItem.Text != "--Select--" && txtArea.Text == "")
                        {
                            strQueryEast = strQueryEast + "and documentname like '%" + txtAggNo.Text + "%' and documentname like '%" + ddlYear.SelectedItem.Text + "%'";
                        }
                        if (txtAggNo.Text != "" && txtSubName.Text == "" && ddlYear.SelectedItem.Text == "--Select--" && txtArea.Text != "")
                        {
                            strQueryEast = strQueryEast + "and documentname like '%" + txtAggNo.Text + "%' and documentname like '%" + txtArea.Text + "%'";
                        }
                        if (txtAggNo.Text == "" && txtSubName.Text != "" && ddlYear.SelectedItem.Text != "--Select--" && txtArea.Text == "")
                        {
                            strQueryEast = strQueryEast + "and documentname like '%" + txtSubName.Text + "%' and documentname like '%" + ddlYear.SelectedItem.Text + "%'";
                        }
                        if (txtAggNo.Text == "" && txtSubName.Text != "" && ddlYear.SelectedItem.Text == "--Select--" && txtArea.Text != "")
                        {
                            strQueryEast = strQueryEast + "and documentname like '%" + txtSubName.Text + "%' and documentname like '%" + txtArea.Text + "%'";
                        }
                        if (txtAggNo.Text == "" && txtSubName.Text == "" && ddlYear.SelectedItem.Text != "--Select--" && txtArea.Text != "")
                        {
                            strQueryEast = strQueryEast + "and documentname like '%" + ddlYear.SelectedItem.Text + "%' and documentname like '%" + txtArea.Text + "%'";
                        }
                       dsResult = DataHelper.ExecuteDataSet(strQueryEast);
                        break;

                    case "West":
                        strQueryWest = "select id,documentname from document where documentname like 'MSIB EE West%' and status=1 ";
                        
                        if (txtAggNo.Text != "" && txtSubName.Text == "" && ddlYear.SelectedItem.Text == "--Select--" && txtArea.Text == "")
                        {
                            strQueryWest = strQueryWest + "and documentname like '%" + txtAggNo.Text + "%'";
                        }
                        if (txtAggNo.Text == "" && txtSubName.Text != "" && ddlYear.SelectedItem.Text == "--Select--" && txtArea.Text == "")
                        {
                            strQueryWest = strQueryWest + "and documentname like '%" + txtSubName.Text + "%'";
                        }
                        if (txtAggNo.Text == "" && txtSubName.Text == "" && ddlYear.SelectedItem.Text != "--Select--" && txtArea.Text == "")
                        {
                            strQueryWest = strQueryWest + "and documentname like '%" + ddlYear.SelectedItem.Text + "%'";
                        }
                        if (txtAggNo.Text == "" && txtSubName.Text == "" && ddlYear.SelectedItem.Text == "--Select--" && txtArea.Text != "")
                        {
                            strQueryWest = strQueryWest + "and documentname like '%" + txtArea.Text + "%'";
                        }
                        if (txtAggNo.Text != "" && txtSubName.Text != "" && ddlYear.SelectedItem.Text == "--Select--" && txtArea.Text == "")
                        {
                            strQueryWest = strQueryWest + "and documentname like '%" + txtAggNo.Text + "%' and documentname like '%" + txtSubName.Text + "%'";
                        }
                        if (txtAggNo.Text != "" && txtSubName.Text == "" && ddlYear.SelectedItem.Text != "--Select--" && txtArea.Text == "")
                        {
                            strQueryWest = strQueryWest + "and documentname like '%" + txtAggNo.Text + "%' and documentname like '%" + ddlYear.SelectedItem.Text + "%'";
                        }
                        if (txtAggNo.Text != "" && txtSubName.Text == "" && ddlYear.SelectedItem.Text == "--Select--" && txtArea.Text != "")
                        {
                            strQueryWest = strQueryWest + "and documentname like '%" + txtAggNo.Text + "%' and documentname like '%" + txtArea.Text + "%'";
                        }
                        if (txtAggNo.Text == "" && txtSubName.Text != "" && ddlYear.SelectedItem.Text != "--Select--" && txtArea.Text == "")
                        {
                            strQueryWest = strQueryWest + "and documentname like '%" + txtSubName.Text + "%' and documentname like '%" + ddlYear.SelectedItem.Text + "%'";
                        }
                        if (txtAggNo.Text == "" && txtSubName.Text != "" && ddlYear.SelectedItem.Text == "--Select--" && txtArea.Text != "")
                        {
                            strQueryWest = strQueryWest + "and documentname like '%" + txtSubName.Text + "%' and documentname like '%" + txtArea.Text + "%'";
                        }
                        if (txtAggNo.Text == "" && txtSubName.Text == "" && ddlYear.SelectedItem.Text != "--Select--" && txtArea.Text != "")
                        {
                            strQueryWest = strQueryWest + "and documentname like '%" + ddlYear.SelectedItem.Text + "%' and documentname like '%" + txtArea.Text + "%'";
                        }
                        dsResult = DataHelper.ExecuteDataSet(strQueryWest);
                        break;

                    case "City":
                        strQueryCity = "select id,documentname from document where documentname like 'MSIB EE City%' and status=1 ";
                      
                        if (txtAggNo.Text != "" && txtSubName.Text == "" && ddlYear.SelectedItem.Text == "--Select--" && txtArea.Text == "")
                        {
                            strQueryCity = strQueryCity + "and documentname like '%" + txtAggNo.Text + "%'";
                        }
                        if (txtAggNo.Text == "" && txtSubName.Text != "" && ddlYear.SelectedItem.Text == "--Select--" && txtArea.Text == "")
                        {
                            strQueryCity = strQueryCity + "and documentname like '%" + txtSubName.Text + "%'";
                        }
                        if (txtAggNo.Text == "" && txtSubName.Text == "" && ddlYear.SelectedItem.Text != "--Select--" && txtArea.Text == "")
                        {
                            strQueryCity = strQueryCity + "and documentname like '%" + ddlYear.SelectedItem.Text + "%'";
                        }
                        if (txtAggNo.Text == "" && txtSubName.Text == "" && ddlYear.SelectedItem.Text == "--Select--" && txtArea.Text != "")
                        {
                            strQueryCity = strQueryCity + "and documentname like '%" + txtArea.Text + "%'";
                        }
                        if (txtAggNo.Text != "" && txtSubName.Text != "" && ddlYear.SelectedItem.Text == "--Select--" && txtArea.Text == "")
                        {
                            strQueryCity = strQueryCity + "and documentname like '%" + txtAggNo.Text + "%' and documentname like '%" + txtSubName.Text + "%'";
                        }
                        if (txtAggNo.Text != "" && txtSubName.Text == "" && ddlYear.SelectedItem.Text != "--Select--" && txtArea.Text == "")
                        {
                            strQueryCity = strQueryCity + "and documentname like '%" + txtAggNo.Text + "%' and documentname like '%" + ddlYear.SelectedItem.Text + "%'";
                        }
                        if (txtAggNo.Text != "" && txtSubName.Text == "" && ddlYear.SelectedItem.Text == "--Select--" && txtArea.Text != "")
                        {
                            strQueryCity = strQueryCity + "and documentname like '%" + txtAggNo.Text + "%' and documentname like '%" + txtArea.Text + "%'";
                        }
                        if (txtAggNo.Text == "" && txtSubName.Text != "" && ddlYear.SelectedItem.Text != "--Select--" && txtArea.Text == "")
                        {
                            strQueryCity = strQueryCity + "and documentname like '%" + txtSubName.Text + "%' and documentname like '%" + ddlYear.SelectedItem.Text + "%'";
                        }
                        if (txtAggNo.Text == "" && txtSubName.Text != "" && ddlYear.SelectedItem.Text == "--Select--" && txtArea.Text != "")
                        {
                            strQueryCity = strQueryCity + "and documentname like '%" + txtSubName.Text + "%' and documentname like '%" + txtArea.Text + "%'";
                        }
                        if (txtAggNo.Text == "" && txtSubName.Text == "" && ddlYear.SelectedItem.Text != "--Select--" && txtArea.Text != "")
                        {
                            strQueryCity = strQueryCity + "and documentname like '%" + ddlYear.SelectedItem.Text + "%' and documentname like '%" + txtArea.Text + "%'";
                        }
                        dsResult = DataHelper.ExecuteDataSet(strQueryCity);
                        break;
                }
               
               
                    if (dsResult.Tables[0].Rows.Count>0)
                    {
                        UserSession.GridData = dsResult.Tables[0];
                        grvDocument.DataSource = dsResult.Tables[0];
                        grvDocument.DataBind();
                    }
                    else
                    {
                        string script = "alert(\"No Data To Display!\");";
                        ScriptManager.RegisterStartupScript(this, GetType(),
                                              "ServerControlScript", script, true);
                        return;
                    }
                }
                
              
            
            catch(Exception ex)
            {
                throw ex;
            }
        }

        protected void txtSearch_Click(object sender, EventArgs e)
        {
            if (ddlZone.SelectedItem.Text=="--Select--" && txtAggNo.Text == "" && txtSubName.Text == "" && ddlYear.SelectedItem.Text == "--Select--" && txtArea.Text == "")
            {
                string script = "alert(\"Please enter atleast one search criteria!\");";
                ScriptManager.RegisterStartupScript(this, GetType(),
                                      "ServerControlScript", script, true);
               // UserSession.DisplayMessage(this, "Please enter one more search criteria!", MainMasterPage.MessageType.Warning);
                return;
            }
            else
            {
                Search();
                TotalDocs();
            }
        }

        protected void txtCancel_Click(object sender, EventArgs e)
        {
            ddlZone.SelectedIndex = 0;
            txtAggNo.Text = "";
            ddlYear.SelectedIndex = 0;
            txtArea.Text = "";
        }

        public string GetIPAddress()//gets the IP address of host(user in this case)
        {
            string Address = "";
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

        protected void grvDocument_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.ToLower().Trim() == "view")
                {
                    int intRowIndex = Convert.ToInt32(e.CommandArgument);
                    string strDocumentID = intRowIndex.ToString();
                    Session["DocId"] = strDocumentID;
                   string IPAddress_Host = GetIPAddress();
                   string MAC_Address = GetMACAddress();
                   string Activity = "View";
                   DMS.BusinessLogic.ReportManager objReport = new ReportManager();
                   objReport.InsertAuditLog_MhadaWebsite(IPAddress_Host, MAC_Address, Activity, Convert.ToInt32(strDocumentID));
                   //Response.Redirect("../MHADA Website Search/ViewDocument.aspx?DOCID=" + strDocumentID, false);
                   Response.Redirect("../MetaData/ViewDocumentForSearch_Website.aspx?DOCID=" + Convert.ToInt32(strDocumentID), false);
            }
        }

        protected void grvDocument_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                if (UserSession.GridData != null)
                {
                    grvDocument.PageIndex = e.NewPageIndex;
                    if (UserSession.FilterData == null)
                        grvDocument.DataSource = UserSession.GridData;
                    else
                        grvDocument.DataSource = UserSession.FilterData;

                    grvDocument.DataBind();
                }
            }
            catch (Exception ex)
            {
                string script = "alert(\"Some error has occurred!\");";
                ScriptManager.RegisterStartupScript(this, GetType(),
                                      "ServerControlScript", script, true);
            }
        }
    }
}