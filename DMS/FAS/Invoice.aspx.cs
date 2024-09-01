using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using DMS.BusinessLogic.Entity;
using DMS.BusinessLogic;
using System.Configuration;
using System.IO;
using System.Text;

namespace DMS.FAS
{
    public partial class Invoice : System.Web.UI.Page
    {
      
        SqlConnection con = new SqlConnection(Utility.ConnectionString);
        string Msg = "";
        DataTable dtMetatemplateMhada = new DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {
            ((AjaxControlToolkit.ToolkitScriptManager)Master.FindControl("tsmManager")).RegisterPostBackControl(ibtnPolicyReport);

            if (!IsPostBack)
                {
                    client();
                   
                
                }

                  
        }

        
        
        protected void client()
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM Repository where Status=1", con);
            SqlDataAdapter adp = new SqlDataAdapter(cmd);
            DataTable dt1 = new DataTable();
            adp.Fill(dt1);

            ddlClient.DataSource = dt1;
            ddlClient.DataTextField = "RepositoryName";
            ddlClient.DataValueField = "ID";
            ddlClient.DataBind();
            ddlClient.Items.Insert(0, new ListItem("--SELECT--", "0"));
            lblDept.Visible = false;
            ddlDept.Visible = false;
            //ddlLocation.Items.Insert(0, new ListItem("--SELECT--", "0"));
            //ddlBranch.Items.Insert(0, new ListItem("--SELECT--", "0"));
 
        }
        protected void Department()
        {
            SqlCommand cmd = new SqlCommand("select distinct DepartmentName from Department_Mhada", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            ddlDept.DataSource = dt;
            ddlDept.DataTextField = "DepartmentName";
            ddlDept.DataBind();
            ddlDept.Items.Insert(0, "--Select--");
        }

        protected DataTable MetatemplateFill()
        {
            SqlCommand cmd = new SqlCommand("select MetaTemplateName from Department_Mhada where DepartmentName='"+ddlDept.SelectedItem.Text+"'",con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dtMetatemplateMhada);
            return dtMetatemplateMhada;
        }
        

        protected void btnShow_Click1(object sender, ImageClickEventArgs e)
        {
            try
            {

                if (ddlClient.SelectedItem.Text != "MHADA")
                {


//                    string Query = @"select Repository.repositoryname as ClientName,FAS_ClientTable.ClientFasID,'Mahape'Location,'200'LocationFasID,
//                                    'Hosting'Sevice,'2601'ServiceFasID,'Mahape'Branch,'7126'BranchFasID, Null CreatedOn,sum(PageCount) as Quantity from metadata
//                                    inner join document on metadata.id = document.metadataid
//                                    inner join repository on repository.id =  metadata.repositoryid
//                                    inner join FAS_ClientTable on Repository.ID=FAS_ClientTable.RepositoryID
//                                    where Repository.repositoryname='" + ddlClient.SelectedItem.Text + "' and Convert(date,document.createdon,101) < '" + txtFrom.Text + "'" +
//                                        @" group by Repository.ID,Repository.repositoryname,FAS_ClientTable.ClientFasID
//
//                                    union all
//
//                                    select Repository.repositoryname as ClientName,FAS_ClientTable.ClientFasID,'Mahape'Location,'200'LocationFasID,
//                                    'Hosting'Sevice,'2601'ServiceFasID,'Mahape'Branch,'7126'BranchFasID,Document.CreatedOn,sum(PageCount) as Quantity from metadata
//                                    inner join document on metadata.id = document.metadataid
//                                    inner join repository on repository.id =  metadata.repositoryid
//                                    inner join FAS_ClientTable on Repository.ID=FAS_ClientTable.RepositoryID
//                                    where Repository.repositoryname='" + ddlClient.SelectedItem.Text + "'" +
//                                        @"and Convert(date,document.createdon,101)
//                                    between '" + txtFrom.Text + "' and '" + txtTo.Text + "'" +
//                                        @"group by Repository.repositoryname,FAS_ClientTable.ClientFasID,
//                                    Document.CreatedOn";
                    //
                    ////for Mhada Authority
                    ////for generating old stock
                    //SqlCommand cmd = new SqlCommand(" select ID from MetaTemplate where RepositoryID=3 and MetaTemplateName='Mhada Authority'", con);
                    //SqlDataAdapter da = new SqlDataAdapter(cmd);
                    //DataTable dt = new DataTable();
                    //da.Fill(dt);
                    //int MetatemplateId = Convert.ToInt16(dt.Rows[0].ToString());
                    //SqlCommand cmd1 = new SqlCommand("select SUM(PageCount),Document.CreatedOn from Document,MetaTemplate,MetaData where MetaTemplate.ID=MetaData.MetaTemplateID and MetaData.ID=Document.MetaDataID and MetaTemplateID='"+MetatemplateId+"'  group by Document.CreatedOn", con);
                    //SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
                    //DataSet ds = new DataSet();

                    string Query = @"select  MetaData.RepositoryID,Repository.RepositoryName,
                                    sum(Document.PageCount)Quantity,CAST(Document.CreatedOn as DATE)CreatedOn
                                    from MetaData inner join Document 
                                    on MetaData.ID = Document.MetaDataID
                                    inner join Repository
                                    on MetaData.RepositoryID = Repository.ID
                                    where Document.Status=1 and MetaData.Status =1 and
                                    Document.CreatedOn>='" + Convert.ToDateTime(txtFrom.Text) + "' and Document.CreatedOn<='"+Convert.ToDateTime(txtTo.Text)+"' and Repository.ID='"+Convert.ToInt32(ddlClient.SelectedValue)+"' group by  MetaData.RepositoryID,Repository.RepositoryName, CAST(Document.CreatedOn as DATE);";

                    
                    SqlCommand showgrid1 = new SqlCommand(Query, con);

                    SqlDataAdapter showadp1 = new SqlDataAdapter(showgrid1);
                    DataTable showDT = new DataTable();

                    showadp1.Fill(showDT);
                    if(showDT.Rows.Count==0)
                    {
                        Query = @"select  MetaData.RepositoryID,Repository.RepositoryName,
                                sum(Document.PageCount)Quantity
                                from MetaData inner join Document 
                                on MetaData.ID = Document.MetaDataID
                                inner join Repository
                                on MetaData.RepositoryID = Repository.ID
                                where Document.Status=1 and MetaData.Status =1
                                and Document.CreatedOn<='" + Convert.ToDateTime(txtTo.Text) + "' and Repository.ID='" + Convert.ToInt32(ddlClient.SelectedValue) + "' group by  MetaData.RepositoryID,Repository.RepositoryName";
                   
                      SqlCommand showgrid2 = new SqlCommand(Query, con);

                    SqlDataAdapter showadp2 = new SqlDataAdapter(showgrid2);
                    showadp2.Fill(showDT);
                        grvOpeningStock.DataSource=showDT;
                       
                        btnGenerate.Visible = true;
                        grvOpeningStock.DataBind();
                        grvShowbill.Visible = false;
                        grvOpeningStock.Visible = true;
                    }
                    
                    //Session["showDT"] = showDT;
                    else
                    {
                    grvShowbill.DataSource = showDT;

                   
                        btnGenerate.Visible = true;
                        grvShowbill.DataBind();
                        grvShowbill.Visible = true;
                        grvOpeningStock.Visible = false;
                    }
                    ViewState["showDT"] = showDT;
                    //else
                    //{
                    //    btnGenerate.Visible = false;
                    //}
                    //if (showDT.Rows.Count == 0)
                    //{
                    //    UserSession.DisplayMessage(this, "No Data To Display .", MainMasterPage.MessageType.Warning);
                    //}


                }

                else
                {
                    //SqlCommand cmd = new SqlCommand("select MetaTemplateName from Department_Mhada where DepartmentName='" + ddlDept.SelectedItem.Text + "'", con);
                    //SqlDataAdapter da = new SqlDataAdapter(cmd);
                    //da.Fill(dtMetatemplateMhada);
                   
                    //string subquery ="";
                    //string subquery1 = "";
//                    string query = @"select Repository.repositoryname as ClientName,FAS_ClientTable.ClientFasID,'Mahape'Location,'200'LocationFasID,
//                                    'Hosting'Sevice,'2601'ServiceFasID,'Mahape'Branch,'7126'BranchFasID, Null CreatedOn,sum(PageCount) as Quantity 
//                                   from Document,MetaData,MetaTemplate,Repository,FAS_ClientTable
// where Repository.repositoryname='Mhada' and
//  Repository.ID=FAS_ClientTable.RepositoryID and
// Document.MetaDataID=MetaData.ID and MetaData.MetaTemplateID=MetaTemplate.ID and
// MetaData.RepositoryID=(select ID from Repository where RepositoryName='Mhada')
// and";
                    if(ddlDept.SelectedItem.Text=="--Select--")
                    {
                        Response.Write("<script>alert('Please select a department!')</script>");
                        return;
                    }
                    string Query = @"select  MetaData.RepositoryID,Repository.RepositoryName,
                                     sum(Document.PageCount)Quantity,CAST(Document.CreatedOn as DATE)CreatedOn
                                     from MetaData inner join Document 
                                     on MetaData.ID = Document.MetaDataID
                                     inner join Repository
                                     on MetaData.RepositoryID = Repository.ID
                                     inner join MetaTemplate 
                                     on MetaTemplate.RepositoryID=Repository.ID
                                     where Document.Status=1 and MetaData.Status =1 and
                                     Document.CreatedOn>='" + Convert.ToDateTime(txtFrom.Text) + "' and Document.CreatedOn<='"+Convert.ToDateTime(txtTo.Text)+"' and Repository.ID=1 and MetaTemplate.ID='"+Convert.ToInt32(ddlDept.SelectedValue)+"' group by  MetaData.RepositoryID,Repository.RepositoryName, CAST(Document.CreatedOn as DATE)";
                    SqlCommand showgrid1 = new SqlCommand(Query, con);

                    SqlDataAdapter showadp1 = new SqlDataAdapter(showgrid1);
                    DataTable showDT = new DataTable();

                    showadp1.Fill(showDT);
                    if(showDT.Rows.Count==0)
                    {
                        Query = @"select  MetaData.RepositoryID,Repository.RepositoryName,
                                     sum(Document.PageCount)Quantity
                                     from MetaData inner join Document 
                                     on MetaData.ID = Document.MetaDataID
                                     inner join Repository
                                     on MetaData.RepositoryID = Repository.ID
                                     inner join MetaTemplate 
                                     on MetaTemplate.RepositoryID=Repository.ID
                                     where Document.Status=1 and MetaData.Status =1 and
                                     Document.CreatedOn<='" + Convert.ToDateTime(txtTo.Text) + "' and Repository.ID=1 and MetaTemplate.ID='" + Convert.ToInt32(ddlDept.SelectedValue) + "' group by  MetaData.RepositoryID,Repository.RepositoryName";
                        SqlCommand showgrid2 = new SqlCommand(Query, con);

                        SqlDataAdapter showadp2 = new SqlDataAdapter(showgrid2);
                        DataTable showDT2 = new DataTable();
                        
                        showadp1.Fill(showDT2);
                        if(showDT2.Rows.Count>0)
                        {
                            grvOpeningStock.DataSource = showDT2;
                            grvOpeningStock.DataBind();
                        }
                    }
                    else
                    {
                        grvShowbill.DataSource = showDT;
                        grvShowbill.DataBind();
                    }
                    btnGenerate.Visible = true;
                    //if (dtMetatemplateMhada.Rows.Count > 1)
                    //{
                    //    for (int i = 0; i <= dtMetatemplateMhada.Rows.Count; i=i+2)
                    //    {
                    //        subquery = @"(MetaTemplate.MetaTemplateName='" + dtMetatemplateMhada.Rows[i]["MetaTemplateName"] + "'or MetaTemplate.MetaTemplateName='" + dtMetatemplateMhada.Rows[i + 1]["MetaTemplateName"] + "')";
                    //        subquery1 = subquery1 + "or"+subquery;
                    //    }
                    //}
                    //else
                    //{
                    //    subquery1 = @"(MetaTemplate.MetaTemplateName='" + dtMetatemplateMhada.Rows[0]["MetaTemplateName"] + "')";
                    //}
                    //query = query + subquery1;
                }
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
            }
        
           
        }

      

        //protected void ddlClient_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    con.Open();
        //    SqlCommand cmd1 = new SqlCommand("SELECT * FROM FAS_Loation WHERE RepositoryID=" + ddlClient.SelectedValue,con);
        //    SqlDataAdapter adp1 = new SqlDataAdapter(cmd1);
        //    DataTable dt1 = new DataTable();
        //    adp1.Fill(dt1);
           
        //    ddlLocation.DataSource = dt1;
           
        //    ddlLocation.DataTextField = "LocationName";
        //    ddlLocation.DataValueField = "ID";
        //    ddlLocation.DataBind();
        //    ddlLocation.Items.Insert(0, new ListItem("--SELECT--","0"));
        //    con.Close();

        //    if (ddlLocation.SelectedValue == "0")
        //    {

        //        //ddlBranch.Items.Clear();
        //        //ddlBranch.Items.Insert(0, new ListItem("--SELECT--", "0"));
        //    }

        //}

      

        protected void btnGenerate_Click1(object sender, ImageClickEventArgs e)
        {
            //if(ddlClient.SelectedItem.Text!="MHADA")
            //{

            //DateTime SIDate = Convert.ToDateTime(txtTo.Text);

            int CutomerID = Convert.ToInt32(ddlClient.SelectedValue);
            //int Locationid = Convert.ToInt32(ddlLocation.SelectedValue);
            //int BranchId = Convert.ToInt32(ddlBranch.SelectedValue);

            DateTime SIDate_FAS = Convert.ToDateTime(txtTo.Text);
            DateTime SIDate = new DateTime(SIDate_FAS.Year, SIDate_FAS.Month, DateTime.DaysInMonth(SIDate_FAS.Year, SIDate_FAS.Month));

            DateTime FromDate = new DateTime(SIDate_FAS.Year, SIDate_FAS.Month, 1);
            DateTime ToDate = new DateTime(SIDate_FAS.Year, SIDate_FAS.Month, DateTime.DaysInMonth(SIDate_FAS.Year, SIDate_FAS.Month));

            DataTable dtMidmonthActivity = new DataTable();
            dtMidmonthActivity = MidMonthQuantity(FromDate);

            DataTable dtOpeningStockActivity = new DataTable();
            dtOpeningStockActivity = OpeningStockQuantity(ToDate);

            string query1 = "select FasID from FAS_Loation where RepositoryID='"+Convert.ToInt32(ddlClient.SelectedValue)+"'";
            //DataSet ds = new DataSet();
            //ds = DataHelper.ExecuteDataSet(query1);
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand(query1, con);
            SqlDataAdapter adpt = new SqlDataAdapter(cmd);
            adpt.Fill(dt);
           
            string query2 = "select FasActivityID from Fas_Activity where DMSActivity='Hosting'";
            DataTable dt2 = new DataTable();
            SqlCommand cmd2 = new SqlCommand(query2, con);
            SqlDataAdapter adpt2 = new SqlDataAdapter(cmd2);
            adpt2.Fill(dt2);

            string query4 = "select FasActivityID from Fas_Activity where DMSActivity='New-Hosting'";
            DataTable dt4 = new DataTable();
            SqlCommand cmd4 = new SqlCommand(query4, con);
            SqlDataAdapter adpt4 = new SqlDataAdapter(cmd4);
            adpt4.Fill(dt4);

            XmlDocument xDoc = new XmlDocument();

            XmlNode nodeRoot = xDoc.CreateNode(XmlNodeType.Element, "ROOT", null);
            xDoc.AppendChild(nodeRoot);

            XmlNode nodeRows = xDoc.CreateNode(XmlNodeType.Element, "ROWS", null);
            nodeRoot.AppendChild(nodeRows);

            for (int i = 0; i < dtMidmonthActivity.Rows.Count; i++)
            {

                XmlNode nodeRow = xDoc.CreateNode(XmlNodeType.Element, "ROW", null);
                nodeRows.AppendChild(nodeRow);

                XmlNode locationID = xDoc.CreateNode(XmlNodeType.Element, "LocationID", null);
                locationID.InnerText = dt.Rows[0]["FasID"].ToString();
                nodeRow.AppendChild(locationID);

                XmlNode itemID1 = xDoc.CreateNode(XmlNodeType.Element, "ItemID", null);
                itemID1.InnerText = dt4.Rows[0]["FasActivityID"].ToString();
                nodeRow.AppendChild(itemID1);


                XmlNode invoicequantity = xDoc.CreateNode(XmlNodeType.Element, "InvoiceQuantity", null);
                invoicequantity.InnerText = dtMidmonthActivity.Rows[i]["Quantity"].ToString();
                nodeRow.AppendChild(invoicequantity);
               



                XmlNode dateOfActivity = xDoc.CreateNode(XmlNodeType.Element, "DateOfActivity", null);
                dateOfActivity.InnerText = Convert.ToDateTime(((DataTable)ViewState["showDT"]).Rows[i]["CreatedOn"]).ToString("dd-MMM-yyyy");
                nodeRow.AppendChild(dateOfActivity);
                
                XmlNode activityEndDate = xDoc.CreateNode(XmlNodeType.Element, "DOAEnd", null);
                activityEndDate.InnerText = null;
                nodeRow.AppendChild(activityEndDate);
            }   
                   
                DateTime dateTime = DateTime.Parse(txtFrom.Text);
            string Month=dateTime.Month.ToString();
            string Year = dateTime.Year.ToString();
            for (int i = 0; i < dtOpeningStockActivity.Rows.Count; i++)
            {

                XmlNode nodeRow = xDoc.CreateNode(XmlNodeType.Element, "ROW", null);
                nodeRows.AppendChild(nodeRow);

                XmlNode locationID = xDoc.CreateNode(XmlNodeType.Element, "LocationID", null);
                locationID.InnerText = dt.Rows[0]["FasID"].ToString();
                nodeRow.AppendChild(locationID);

                XmlNode itemID1 = xDoc.CreateNode(XmlNodeType.Element, "ItemID", null);
                itemID1.InnerText = dt2.Rows[0]["FasActivityID"].ToString();
                nodeRow.AppendChild(itemID1);

                XmlNode invoicequantity = xDoc.CreateNode(XmlNodeType.Element, "InvoiceQuantity", null);
                if (Convert.ToInt32(ddlClient.SelectedValue) == 29 && Month == "4" && Year == "2015")
                {
                    //invoicequantity.InnerText = "27148";
                    invoicequantity.InnerText = "0";
                }
                else
                {
                    //int OpeningStock=27148+
                    int InvoiceQuantity = Convert.ToInt32(dtOpeningStockActivity.Rows[i]["Quantity"].ToString()) + 27148;
                    //invoicequantity.InnerText = Convert.ToString(InvoiceQuantity);
                    invoicequantity.InnerText = Convert.ToString(InvoiceQuantity);
                }

                nodeRow.AppendChild(invoicequantity);

                XmlNode dateOfActivity = xDoc.CreateNode(XmlNodeType.Element, "DateOfActivity", null);
                dateOfActivity.InnerText = null;
                nodeRow.AppendChild(dateOfActivity);

                XmlNode activityEndDate = xDoc.CreateNode(XmlNodeType.Element, "DOAEnd", null);
                activityEndDate.InnerText = null;
                nodeRow.AppendChild(activityEndDate);



            }
                xDoc.Save("d:\\DMS_Invoice.xml");

#region comment

                //DataTable objTable = ((DataTable)ViewState["showDT"]);
//                DataTable objTable = new DataTable();
//                string query1 = @"select Repository.repositoryname as ClientName,FAS_ClientTable.ClientFasID,'Mahape'Location,'200'LocationFasID,
//                                'Hosting'Sevice,'2601'ServiceFasID,'Mahape'Branch,'7126'BranchFasID,
//                                Document.CreatedOn,PageCount 
//                                as Quantity from metadata
//                                inner join document on metadata.id = document.metadataid
//                                inner join repository on repository.id =  metadata.repositoryid
//                                inner join FAS_ClientTable on Repository.ID=FAS_ClientTable.RepositoryID
//                                where Repository.repositoryname='" + ddlClient.SelectedItem.Text+"'"+ 
//                                @"and Convert(date,document.createdon,101) <='"+txtTo.Text+"'"+
//                                @"group by Repository.RepositoryName,FAS_ClientTable.ClientFasID,
//                                Document.CreatedOn,PageCount ";
//                SqlCommand cmd = new SqlCommand(query1,con);
//                SqlDataAdapter adp = new SqlDataAdapter(cmd);
//                adp.Fill(objTable);
//                StringBuilder objStringBuilder = new StringBuilder();

//                objStringBuilder.Append("<table>");

//                objStringBuilder.Append("<tr>");
//                foreach (DataColumn objColumn in objTable.Columns)
//                {
//                    objStringBuilder.Append("<td>" + objColumn.ColumnName + "</td>");
//                }
//                objStringBuilder.Append("</tr>");

//                foreach (DataRow objRow in objTable.Rows)
//                {
//                    objStringBuilder.Append("<tr>");
//                    foreach (DataColumn objColumn in objTable.Columns)
//                    {
//                        if (objColumn.ColumnName.ToLower() != "createdon")
//                        {
//                            objStringBuilder.Append(@"<td>" + objRow[objColumn.ColumnName] + "</td>");
//                        }
//                        else
//                        {
//                            if (objRow[objColumn.ColumnName].ToString() == string.Empty)
//                            {
//                                objStringBuilder.Append(@"<td>" + objRow[objColumn.ColumnName] + "</td>");
//                            }
//                            else
//                            {
//                                objStringBuilder.Append(@"<td>" + Convert.ToDateTime(objRow[objColumn.ColumnName]).ToString("dd-MMM-yyyy") + "</td>");
//                            }
//                        }
//                    }
//                    objStringBuilder.Append("</tr>");
//                }
//                objStringBuilder.Append("</table>");

//                StreamWriter objWriter = new StreamWriter("d:\\DMS_Invoice.xls");
//                objWriter.Write(objStringBuilder.ToString());
//                objWriter.Close();
//                objWriter.Dispose();
               

#endregion


                FAS_web_ref.SaleInvoice objSaleInvoice = new FAS_web_ref.SaleInvoice();

                FAS_invoice fi = new FAS_invoice();
                DataTable dt3 = new DataTable();
                dt3 = fi.otherParam();
                int offcID = 0, cmpnyID = 0, cmpnyType = 0, BussdivID = 0;
                string siType = "";

                foreach (DataRow dr in dt3.Rows)
                {
                    offcID = Convert.ToInt32(dr["OfficeID"]);
                    cmpnyID = Convert.ToInt32(dr["ComponyID"]);
                    cmpnyType = Convert.ToInt32(dr["ComponyType"]);
                    BussdivID = Convert.ToInt32(dr["BusinessDivisionID"]);
                    siType = dr["SIType"].ToString();
                }



                int customerID = fi.CustomerID1(CutomerID);

                int locationID1 = fi.LocationID1(CutomerID);
                //int BranchID1 = fi.branchID1(BranchId);

                //Msg = objSaleInvoice.ImportSaleInvoiceByLocation(SIDate, customerID, locationID1, offcID, xDoc, cmpnyID, cmpnyType, BussdivID, siType, Msg);
         
                Msg = objSaleInvoice.ImportSaleInvoiceByLocation(SIDate, customerID, locationID1, offcID, cmpnyID, cmpnyType, BussdivID, siType, Msg, "DMS", 7126, "", xDoc, "", "");
                UserSession.DisplayMessage(this, "  " + Msg, MainMasterPage.MessageType.Success);
            //}

            }

        public DataTable otherParam()
        {
            DataTable dt = new DataTable();
            string query = "SELECT * FROM FasOther";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataAdapter adp = new SqlDataAdapter(cmd);

            adp.Fill(dt);
            return (dt);
        }
        //protected void ddlLocation_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    con.Open();
        //    SqlCommand cmdbranch = new SqlCommand("SELECT * FROM FAS_Branches where LocationID=" + ddlLocation.SelectedValue, con);

        //    SqlDataAdapter adpbranch = new SqlDataAdapter(cmdbranch);
        //    DataTable dtbranch = new DataTable();
        //    adpbranch.Fill(dtbranch);

        //    ddlBranch.DataSource = dtbranch;
        //    ddlBranch.DataTextField = "BranchName";
        //    ddlBranch.DataValueField = "ID";

        //    ddlBranch.DataBind();
        //    ddlBranch.Items.Insert(0, new ListItem("--SELECT--", "0"));
        //    con.Close();


        //}

        

        protected void btnCancel_Click1(object sender, ImageClickEventArgs e)
        {
            try
            {
                Response.Redirect("~/Shared/HomePage.aspx", false);
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }

        }

        protected void grvShowbill_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                Utility.SetGridHoverStyle(e);
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        protected void grvShowbill_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            if (ViewState["showDT"] != null)
            {
                grvShowbill.PageIndex = e.NewPageIndex;
                grvShowbill.DataSource = (DataTable)ViewState["showDT"];
                grvShowbill.DataBind();
            }
        }

        protected void grvShowbill_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (ViewState["showDT"] != null)
            {
                if (ViewState[UserSession.SortExpression] == null || ViewState[UserSession.SortExpression].ToString() == "DESC")
                    ViewState[UserSession.SortExpression] = "ASC";

                else

                    ViewState[UserSession.SortExpression] = "DESC";
                DataView objView = ((DataTable)ViewState["showDT"]).DefaultView;
                objView.Sort = e.SortExpression + " " + ViewState[UserSession.SortExpression].ToString();
                grvShowbill.DataSource = objView;
                grvShowbill.DataBind();
            }

        }

        protected void ibtnPolicyReport_Click(object sender, ImageClickEventArgs e)
        {
            DataTable objdatatable = (DataTable)ViewState["showDT"];
            DataGrid objgrid = new DataGrid();
            objgrid.DataSource = objdatatable;
            objgrid.DataBind();

            StringWriter objstringwriter = new StringWriter();
            HtmlTextWriter objtextwriter = new HtmlTextWriter(objstringwriter);
            objgrid.RenderControl(objtextwriter);
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "DMS_Invoice.xls"));
            Response.ContentType = "application/ms-excel";
            Response.Write(objstringwriter.ToString());
            Response.End();


        }
       
        protected void ddlClient_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlClient.SelectedItem.Text == "MHADA")
            {
                lblDept.Visible = true;
                ddlDept.Visible = true;
                //Department();
            }
            else
            {
                lblDept.Visible = false;
                ddlDept.Visible = false;
            }
        }

        protected void ddlDept_SelectedIndexChanged(object sender, EventArgs e)
        {
           // dtMetatemplateMhada = MetatemplateFill();
        }


        public DataTable OpeningStockQuantity(DateTime formatteddate)
        {
            DateTime formatteddateFrom = new DateTime(formatteddate.Year, formatteddate.Month, 1, 0, 0, 0);
            DateTime formatteddateTo = new DateTime(formatteddate.Year, formatteddate.Month, formatteddate.Day, 0, 0, 0);
            formatteddateTo = formatteddateTo.AddDays(1);
            string query = "";
            if (ddlClient.SelectedItem.Text == "MHADA")
            {
                query = @"select  MetaData.RepositoryID,Repository.RepositoryName,
                                    sum(Document.PageCount)Quantity
                                    from MetaData inner join Document 
                                    on MetaData.ID = Document.MetaDataID
                                    inner join Repository
                                    on MetaData.RepositoryID = Repository.ID
                                    where Document.Status=1 and MetaData.Status =1 and
                                    Document.CreatedOn<='" + Convert.ToDateTime(txtTo.Text) + "' and Repository.ID='" + Convert.ToInt32(ddlClient.SelectedValue) + "' group by  MetaData.RepositoryID,Repository.RepositoryName";
            }
            else
            {
                query = @"select  MetaData.RepositoryID,Repository.RepositoryName,
                                    sum(Document.PageCount)Quantity
                                    from MetaData inner join Document 
                                    on MetaData.ID = Document.MetaDataID
                                    inner join Repository
                                    on MetaData.RepositoryID = Repository.ID
                                    where Document.Status=1 and MetaData.Status =1 and
                                    Document.CreatedOn>='01-Apr-2015' and Document.CreatedOn<='" + Convert.ToDateTime(txtTo.Text) + "' and Repository.ID='" + Convert.ToInt32(ddlClient.SelectedValue) + "' group by  MetaData.RepositoryID,Repository.RepositoryName";
//                query = @"select  MetaData.RepositoryID,Repository.RepositoryName,
//                                     sum(Document.PageCount)Quantity
//                                     from MetaData inner join Document 
//                                     on MetaData.ID = Document.MetaDataID
//                                     inner join Repository
//                                     on MetaData.RepositoryID = Repository.ID
//                                     inner join MetaTemplate 
//                                     on MetaTemplate.RepositoryID=Repository.ID
//                                     where Document.Status=1 and MetaData.Status =1 and
//                                      Document.CreatedOn<='" + Convert.ToDateTime(txtTo.Text) + "' and Repository.ID=1 and MetaTemplate.ID='" + Convert.ToInt32(ddlDept.SelectedValue) + "' group by  MetaData.RepositoryID,Repository.RepositoryName";
            }

            //string query = @" select (select FasActivityID from Fas_Activity where DMSActivity='Hosting') as ActivityID " +
            //                 ",sum(PageCount) as Quantity from metadata inner join document on metadata.id = document.metadataid inner join repository on repository.id =  metadata.repositoryid inner join FAS_ClientTable on Repository.ID=FAS_ClientTable.RepositoryID where Repository.repositoryname='" + ddlClient.SelectedItem.Text + "'  and Convert(date, Document.CreatedOn,101)<= '" + txtFrom.Text + "'";

            SqlDataAdapter adpt = new SqlDataAdapter(query, con);

            DataTable dt = new DataTable();
            adpt.Fill(dt);

            return dt;
        }
        public DataTable MidMonthQuantity(DateTime formatteddate)
        {
            DateTime formatteddateFrom = new DateTime(formatteddate.Year, formatteddate.Month, 1, 0, 0, 0);
            DateTime formatteddateTo = new DateTime(formatteddate.Year, formatteddate.Month, formatteddate.Day, 0, 0, 0);
            //formatteddateTo = formatteddateTo.AddDays(1);
            string query = "";
            if(ddlClient.SelectedItem.Text=="MHADA")
            {
                 query = @"select  MetaData.RepositoryID,Repository.RepositoryName,
                                    sum(Document.PageCount)Quantity,CAST(Document.CreatedOn as DATE)CreatedOn
                                    from MetaData inner join Document 
                                    on MetaData.ID = Document.MetaDataID
                                    inner join Repository
                                    on MetaData.RepositoryID = Repository.ID
                                    where Document.Status=1 and MetaData.Status =1 and
                                    Document.CreatedOn>='" + Convert.ToDateTime(txtFrom.Text) + "' and Document.CreatedOn<='" + Convert.ToDateTime(txtTo.Text) + "' and Repository.ID='" + Convert.ToInt32(ddlClient.SelectedValue) + "' group by  MetaData.RepositoryID,Repository.RepositoryName, CAST(Document.CreatedOn as DATE);";
            }
            else
            {
                query = @"select  MetaData.RepositoryID,Repository.RepositoryName,
                                     sum(Document.PageCount)Quantity,CAST(Document.CreatedOn as DATE)CreatedOn
                                     from MetaData inner join Document 
                                     on MetaData.ID = Document.MetaDataID
                                     inner join Repository
                                     on MetaData.RepositoryID = Repository.ID
                                     inner join MetaTemplate 
                                     on MetaTemplate.RepositoryID=Repository.ID
                                     where Document.Status=1 and MetaData.Status =1 and
                                     Document.CreatedOn>='" + Convert.ToDateTime(txtFrom.Text) + "' and Document.CreatedOn<='" + Convert.ToDateTime(txtTo.Text) + "' and Repository.ID='" + Convert.ToInt32(ddlClient.SelectedValue) + "' group by  MetaData.RepositoryID,Repository.RepositoryName, CAST(Document.CreatedOn as DATE)";
            }
//            string query = @"  select Repository.repositoryname as ClientName,FAS_ClientTable.ClientFasID,'Mahape'Location,'200'LocationFasID,
//                                    'Hosting'Sevice,'2601'ServiceFasID,'Mahape'Branch,'7126'BranchFasID,CONVERT(varchar(12),Document.CreatedOn,101) as CREATEDON,sum(PageCount) as Quantity from document
//                                    inner join metadata on metadata.id = document.metadataid
//                                    inner join repository on repository.id =  metadata.repositoryid
//                                    inner join FAS_ClientTable on Repository.ID=FAS_ClientTable.RepositoryID
//                                    where Repository.repositoryname='"+ddlClient.SelectedItem.Text+"'  and Convert(date,document.CreatedOn) between '"+txtFrom.Text+"' and '"+txtTo.Text+"' group by Repository.repositoryname,FAS_ClientTable.ClientFasID, CONVERT(varchar(12),Document.CreatedOn,101)";

            SqlDataAdapter adpt = new SqlDataAdapter(query, con);

            DataTable dt = new DataTable();
            adpt.Fill(dt);

            return dt;
        }
       
       
        

       
        
    }
}