using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DMS.BusinessLogic;
using System.Data;
using System.Collections;
using System.Data.SqlClient;
using System.Data.Common;
using System.Net;
using System.Net.NetworkInformation;
using System.IO;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using Ionic.Zip;


namespace DMS.Shared
{
    public partial class SearchDocument2 : System.Web.UI.Page
    {
        private int PageSize = 100;
        Utility objUtility = new Utility();
        Document objdocument = new Document();
        string SelectedFieldValue = "SelectedFieldValue";

        public string FieldValue
        {
            get
            {
                if (Session[SelectedFieldValue] != null)
                    return Session[SelectedFieldValue].ToString();
                else
                    return null;
            }
            set
            {
                Session[SelectedFieldValue] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Utility.LoadRepository(ddlRepository, UserSession.UserID);
                ddlMetatemplate.Items.Insert(0, new ListItem("--SELECT--", "-1"));
                ddlCategory.Items.Insert(0, new ListItem("--SELECT--", "-1"));
                TreeNode objTreeNode = new TreeNode("--NONE--", "0");
                objTreeNode.Selected = true;
                tvwFolder.Nodes.Add(objTreeNode);
            }
        }

        protected void ddlRepository_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlRepository.SelectedValue != "-1")
            {
                Utility.LoadMetaTemplate(ddlMetatemplate, UserSession.UserID, int.Parse(ddlRepository.SelectedValue));
            }
            else
            {
                ddlMetatemplate.Items.Clear();
                ddlMetatemplate.Items.Insert(0, new ListItem("--SELECT--", "-1"));
                ddlCategory.Items.Clear();
                ddlCategory.Items.Insert(0, new ListItem("--SELECT--", "-1"));
                tvwFolder.Nodes.Clear();
            }
        }

        protected void ddlMetatemplate_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlMetatemplate.SelectedValue != "-1")
            {
                Utility.LoadCategory(ddlCategory, UserSession.UserID, int.Parse(ddlRepository.SelectedValue), int.Parse(ddlMetatemplate.SelectedValue));
                if (ddlCategory.Items.Count == 1)
                {
                    Utility.LoadFolder(tvwFolder, UserSession.UserID, int.Parse(ddlRepository.SelectedValue), int.Parse(ddlMetatemplate.SelectedValue), 0);
                }
            }
            else
            {
                ddlCategory.Items.Clear();
                ddlCategory.Items.Insert(0, new ListItem("--SELECT--", "-1"));
                tvwFolder.Nodes.Clear();
            }
        }

        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlMetatemplate.SelectedValue != "-1")
            {
                Utility.LoadFolder(tvwFolder, UserSession.UserID, int.Parse(ddlRepository.SelectedValue), int.Parse(ddlMetatemplate.SelectedValue), int.Parse(ddlCategory.SelectedValue));
            }
            else
            {
                tvwFolder.Nodes.Clear();
            }
        }

        protected void tvwFolder_SelectedNodeChanged(object sender, EventArgs e)
        {

        }

        protected void ibtnShow_Click(object sender, EventArgs e)
        {
            try
            {
                LoadGridDataByCriteria(1);
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
            }
        }

        private void LoadGridDataByCriteria(int PageIndex)
        {
            try
            {
                Hashtable objTempList = new Hashtable();
                ibtnDownloadFiles.Visible = false;

                int RepositoryID = int.Parse(ddlRepository.SelectedValue);
                int MetaTemplateID = int.Parse(ddlMetatemplate.SelectedValue);
                int CategoryID = int.Parse(ddlCategory.SelectedValue);
                int FolderID = int.Parse(tvwFolder.SelectedNode.Value);
                int IsRecursive = chkRecurse.Checked ? 1 : 0;

                DataTable objDataTable = new DataTable();
                DocumentManager objDocumentManager = new DocumentManager();
                BusinessLogic.MetaData objMetaData = new BusinessLogic.MetaData()
                {
                    RepositoryID = RepositoryID,
                    MetaTemplateID = MetaTemplateID,
                    CategoryID = CategoryID,
                    FolderID = FolderID,
                    MetaDataID = -1,
                    IsRecursive = IsRecursive
                };
                BusinessLogic.Document objDocument = null;
                int RecordCount = 0;
                if (rdblSearchBy.SelectedValue == "1") // Using MetaTemplate Field
                {
                    #region MetaTemplate Field Search
                    if (ddlField.SelectedValue == "-1")
                    {
                        UserSession.DisplayMessage(this, "Please Select The Field .", MainMasterPage.MessageType.Warning);
                        return;
                    }

                    string strDataTypeID = ddlField.SelectedValue.Split('-')[1].ToString();
                    string strEnterFieldValue = string.Empty;
                    if (strDataTypeID.Trim() != "4" && strDataTypeID.Trim() != "9" && strDataTypeID.Trim() != "3")
                    {
                        //strEnterFieldValue = ((TextBox)tdDataToSearch.FindControl(ddlField.SelectedValue.Split('-')[0].ToString())).Text;
                    }
                    else if (strDataTypeID.Trim() == "4")
                    {
                        //strEnterFieldValue = ((DropDownList)tdDataToSearch.FindControl(ddlField.SelectedValue.Split('-')[0].ToString())).SelectedItem.Value;
                    }
                    else if (strDataTypeID.Trim() == "9")
                    {
                        CheckBoxList objCheckBoxList = null; //((CheckBoxList)tdDataToSearch.FindControl(ddlField.SelectedValue.Split('-')[0].ToString()));
                        foreach (ListItem objListItem in objCheckBoxList.Items)
                        {
                            if (objListItem.Selected)
                                strEnterFieldValue = strEnterFieldValue + objListItem.Value + ",";
                        }
                        if (strEnterFieldValue != string.Empty)
                            strEnterFieldValue = strEnterFieldValue.Remove(strEnterFieldValue.LastIndexOf(','));

                        string[] strArray = strEnterFieldValue.Split(',');
                        if (strArray.Length > 0)
                        {
                            strEnterFieldValue = string.Empty;
                            Array.Sort(strArray);
                        }

                        foreach (string strItem in strArray)
                        {
                            strEnterFieldValue = strEnterFieldValue + strItem + ",";
                        }
                        if (strEnterFieldValue != string.Empty)
                            strEnterFieldValue = strEnterFieldValue.Remove(strEnterFieldValue.LastIndexOf(','));
                    }

                    BusinessLogic.DocumentEntry objDocumentEntry = new BusinessLogic.DocumentEntry()
                    {
                        FieldID = Convert.ToInt32(ddlField.SelectedValue.Split('-')[0].ToString()),
                        FieldData = strEnterFieldValue,
                        FromDate = txtFromDate.Text,
                        ToDate = txtToDate.Text
                    };

                    if (strDataTypeID.Trim() != "3")
                    {

                    }
                    else
                    {

                    }


                    switch (objUtility.Result)
                    {
                        case Utility.ResultType.Success:
                            gvwDocument.DataSource = objDataTable;
                            if (RepositoryID == 75 || RepositoryID == 76)
                            {
                                gvwDocument.AllowPaging = false;
                                ibtnDownloadFiles.Visible = true;

                            }
                            else
                            {
                                gvwDocument.AllowPaging = true;
                                ibtnDownloadFiles.Visible = false;
                                gvwDocument.PageSize = 10;
                            }
                            gvwDocument.DataBind();
                            if (RepositoryID == 75 || RepositoryID == 76)
                                ibtnDownloadFiles.Visible = true;
                            UserSession.TemporaryList.Add("DOWNLOADBTN", ibtnDownloadFiles.Visible);
                            UserSession.FilterData = null;
                            UserSession.GridData = objDataTable;
                            break;

                        case Utility.ResultType.Failure:
                            gvwDocument.DataSource = objDataTable;
                            gvwDocument.DataBind();
                            UserSession.FilterData = null;
                            UserSession.DisplayMessage(this, "No Data To Display .", MainMasterPage.MessageType.Warning);
                            break;

                        case Utility.ResultType.Error:
                            gvwDocument.DataSource = objDataTable;
                            gvwDocument.DataBind();
                            UserSession.FilterData = null;
                            UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                            break;
                    }
                    #endregion
                }
                else if (rdblSearchBy.SelectedValue == "2") // using Tag Search
                {
                    #region Tag Search
                    if (txtTextToSeach.Text.Trim() == string.Empty)
                    {
                        UserSession.DisplayMessage(this, "Please Enter The Text To Search .", MainMasterPage.MessageType.Warning);
                        return;
                    }

                    objDocument = new BusinessLogic.Document()
                    {
                        Tag = txtTextToSeach.Text.Trim()
                    };

                    int intSearchType = int.Parse(ddlCriteria.SelectedValue);
                    BusinessLogic.MetaData.SearchType enumSearchType = (BusinessLogic.MetaData.SearchType)Enum.ToObject(typeof(BusinessLogic.MetaData.SearchType), intSearchType);

                    objUtility.Result = objDocumentManager.DocumentSearch_New(out objDataTable, out RecordCount, objMetaData, objDocument, enumSearchType, PageIndex, PageSize);

                    #endregion
                }
                else if (rdblSearchBy.SelectedValue == "3") // Content Search
                {
                    #region Content Search
                    if (txtTextToSeach.Text.Trim() == string.Empty)
                    {
                        UserSession.DisplayMessage(this, "Please Enter The Text To Search .", MainMasterPage.MessageType.Warning);
                        return;
                    }
                    objDocument = new BusinessLogic.Document()
                    {
                        Tag = txtTextToSeach.Text.Trim()
                    };

                    objUtility.Result = objDocumentManager.DocumentSearch_New(out objDataTable, out RecordCount, objMetaData, objDocument, BusinessLogic.MetaData.SearchType.Contains, PageIndex, PageSize);
                    DataTable dtFilteredDocument = ReadPdfFile(objDataTable, objDocument.Tag);
                    if (dtFilteredDocument != null)
                    {
                        DataTable dtFinalResult = new DataTable();
                        var result = (from dataRows1 in objDataTable.AsEnumerable()
                                      join dataRows2 in dtFilteredDocument.AsEnumerable()
                                      on dataRows1.Field<int>("ID") equals dataRows2.Field<int>("DocID")
                                      select dtFinalResult.LoadDataRow(new object[] { 
                                            dataRows1.Field<int>("ID"), 
                                            dataRows1.Field<string>("DocumentName"),
                                            dataRows1.Field<string>("FolderName"), 
                                            dataRows1.Field<string>("DocumentPath"), 
                                            dataRows1.Field<string>("Size"), 
                                            dataRows1.Field<string>("DocumentType"), 
                                            dataRows1.Field<string>("DocumentStatus"), 
                                            dataRows1.Field<string>("CreatedOn"), 
                                            dataRows1.Field<string>("CretedBy")
                                      }, true)).ToList();

                        objDataTable = result.CopyToDataTable();
                    }
                    else
                    {
                        gvwDocument.Visible = false;
                        UserSession.DisplayMessage(this, "No Data To Display .", MainMasterPage.MessageType.Warning);
                    }
                    #endregion
                }
                else if (rdblSearchBy.SelectedValue == "4") // Folder BAsed Search
                {
                    #region Folder BAsed Search
                    objDocument = new BusinessLogic.Document()
                    {
                        Tag = ""
                    };

                    objUtility.Result = objDocumentManager.DocumentSearch_New(out objDataTable, out RecordCount, objMetaData, objDocument, BusinessLogic.MetaData.SearchType.Contains, PageIndex, PageSize);
                    #endregion
                }

                #region Display Result
                gvwDocument.Visible = true;
                gvwDocument.DataSource = objDataTable;
                gvwDocument.DataBind();

                //dlDocument.DataSource = objDataTable;
                //dlDocument.DataBind();

                LblCount.Text = "No. of records :" + objDataTable.Rows.Count;
                //this.PopulatePager(RecordCount, PageIndex);


                if (RepositoryID == 75 || RepositoryID == 76 || RepositoryID == 55)
                    ibtnDownloadFiles.Visible = true;
                else
                    ibtnDownloadFiles.Visible = false;

                switch (objUtility.Result)
                {
                    case Utility.ResultType.Success:
                        #region insert auditlog
                        Report objReport = new Report();
                        objReport.InsertAuditLog(Utility.GetIPAddress(HttpContext.Current), Utility.GetMacAddress(),
                            "Search Document by Folder", "NULL", UserSession.UserID);
                        #endregion
                        break;
                    case Utility.ResultType.Failure:
                        UserSession.DisplayMessage(this, "No Data To Display.", MainMasterPage.MessageType.Warning);
                        break;
                    case Utility.ResultType.Error:
                        UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                        break;
                }
                #endregion
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured ." + ex.Message.ToString(), MainMasterPage.MessageType.Error);
            }
        }

        private DataTable ReadPdfFile(DataTable dtDocumentDetails, String searthText)
        {
            try
            {
                DataTable dtFilteredDocument = new DataTable();
                dtFilteredDocument.Columns.Add(new DataColumn("DocName", typeof(string)));
                dtFilteredDocument.Columns.Add(new DataColumn("DocID", typeof(int)));

                List<int> pages = new List<int>();
                for (int i = 0; i < dtDocumentDetails.Rows.Count; i++)
                {
                    if (File.Exists(dtDocumentDetails.Rows[i]["DocumentPath"].ToString()))
                    {
                        PdfReader pdfReader = new PdfReader(dtDocumentDetails.Rows[i]["DocumentPath"].ToString());
                        for (int page = 1; page <= pdfReader.NumberOfPages; page++)
                        {
                            ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();

                            string currentPageText = PdfTextExtractor.GetTextFromPage(pdfReader, page, strategy);

                            if (currentPageText.ToUpper().Contains(searthText.ToUpper()))
                            {
                                DataRow dr = dtFilteredDocument.NewRow();
                                dr["DocName"] = dtDocumentDetails.Rows[i]["DocumentName"].ToString();
                                dr["DocID"] = dtDocumentDetails.Rows[i]["ID"].ToString();
                                dtFilteredDocument.Rows.Add(dr);

                                page = pdfReader.NumberOfPages;
                                break;
                            }
                        }
                    }
                }
                if (dtFilteredDocument.Rows.Count > 0)
                    return dtFilteredDocument;
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void rdblSearchBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rdblSearchBy.SelectedValue == "1") // MetaTemplate Field
            {
                trField.Visible = true;
                trCriteria.Visible = true;
                trSearchText.Visible = true;
                trFromDate.Visible = false;
                trToDate.Visible = false;
            }
            else if (rdblSearchBy.SelectedValue == "2") // Document NAme / Tag
            {
                trField.Visible = false;
                trCriteria.Visible = true;
                trSearchText.Visible = true;
                trFromDate.Visible = false;
                trToDate.Visible = false;
            }
            else if (rdblSearchBy.SelectedValue == "3") // Content Search
            {
                trField.Visible = false;
                trCriteria.Visible = false;
                trSearchText.Visible = true;
                trFromDate.Visible = false;
                trToDate.Visible = false;
            }
            else if (rdblSearchBy.SelectedValue == "4") // All Folders
            {
                trField.Visible = false;
                trCriteria.Visible = false;
                trSearchText.Visible = false;
                trFromDate.Visible = false;
                trToDate.Visible = false;
            }
        }

        protected void gvwDocument_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvwDocument.PageIndex = e.NewPageIndex;
                LoadGridDataByCriteria(e.NewPageIndex);
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        protected void gvwDocument_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                string strDocumentID = "";
                if (e.CommandName.ToLower().Trim() == "documentsearch")
                {
                    int intRowIndex = Convert.ToInt32(e.CommandArgument);
                    UserSession.MetaDataID = Convert.ToInt32(gvwDocument.DataKeys[intRowIndex].Values["MetaDataID"]);
                    strDocumentID = gvwDocument.DataKeys[intRowIndex].Values["ID"].ToString().Trim();
                    string DocName = gvwDocument.DataKeys[intRowIndex].Values["DocumentName"].ToString().Trim();
                    DataSet ds = new DataSet();
                    ds = DMS.BusinessLogic.MetaData.SelectRepName(UserSession.MetaDataID);

                    Session["DocId"] = strDocumentID;
                    Session["DocumentName"] = DocName;
                    Session["RepositoryName"] = ds.Tables[0].Rows[0]["RepositoryName"].ToString();

                    //#region insert auditlog
                    //Report objReport = new Report();
                    //string IPAddress = Utility.GetIPAddress(HttpContext.Current);
                    //string Activity = "View Document";
                    //string MacAddress = Utility.GetMacAddress();
                    //objReport.InsertAuditLog(IPAddress, MacAddress, Activity, DocName, UserSession.UserID);
                    //#endregion

                    switch (ds.Tables[0].Rows[0]["RepositoryName"].ToString())
                    {
                        case "IDBI Bank Ltd":
                        case "IDBI CPU":
                        case "Indepay Networks Pvt Ltd":
                            //for image viewer
                            Response.Redirect("../MetaData/DocumentViewer.aspx?DOCID=" + strDocumentID, false);
                            break;
                        case "MHADA":
                            if (DocName.Substring(DocName.LastIndexOf(".")) == ".pdf")//for pdf viewer
                                Response.Redirect("../MetaData/ViewDocumentForSearch.aspx?DOCID=" + strDocumentID + " DocumentName=" + DocName, false);
                            else
                                Response.Redirect("../MetaData/DocumentViewer.aspx?DOCID=" + strDocumentID, false);//for image viewer
                            break;
                        case "CIDCO":
                        case "Fasttrack Housing Finance Limited":
                            Response.Redirect("../MetaData/SDocumentView.aspx?", false);// for JQuery Viewer
                            break;
                        default:
                            if (DocName.Substring(DocName.LastIndexOf(".")) == ".pdf")//for pdf viewer
                                Response.Redirect("../MetaData/ViewDocumentForSearch.aspx?DOCID=" + strDocumentID + " DocumentName=" + DocName, false);
                            else
                                Response.Redirect("../MetaData/DocumentViewer.aspx?DOCID=" + strDocumentID, false);//for image viewers
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        #region Pager Functionality
        //private void PopulatePager(int recordCount, int currentPage)
        //{
        //    lblPager.Text = "Page " + currentPage + " of " + PageSize; 
        //    List<ListItem> pages = new List<ListItem>();
        //    int startIndex, endIndex;
        //    int pagerSpan = 5;

        //    //Calculate the Start and End Index of pages to be displayed.
        //    double dblPageCount = (double)((decimal)recordCount / Convert.ToDecimal(PageSize));
        //    int pageCount = (int)Math.Ceiling(dblPageCount);
        //    startIndex = currentPage > 1 && currentPage + pagerSpan - 1 < pagerSpan ? currentPage : 1;
        //    endIndex = pageCount > pagerSpan ? pagerSpan : pageCount;
        //    if (currentPage > pagerSpan % 2)
        //    {
        //        if (currentPage == 2)
        //        {
        //            endIndex = 5;
        //        }
        //        else
        //        {
        //            endIndex = currentPage + 2;
        //        }
        //    }
        //    else
        //    {
        //        endIndex = (pagerSpan - currentPage) + 1;
        //    }

        //    if (endIndex - (pagerSpan - 1) > startIndex)
        //    {
        //        startIndex = endIndex - (pagerSpan - 1);
        //    }

        //    if (endIndex > pageCount)
        //    {
        //        endIndex = pageCount;
        //        startIndex = ((endIndex - pagerSpan) + 1) > 0 ? (endIndex - pagerSpan) + 1 : 1;
        //    }

        //    //Add the First Page Button.
        //    if (currentPage > 1)
        //    {
        //        pages.Add(new ListItem("First", "1"));
        //    }

        //    //Add the Previous Button.
        //    if (currentPage > 1)
        //    {
        //        pages.Add(new ListItem("<<", (currentPage - 1).ToString()));
        //    }

        //    for (int i = startIndex; i <= endIndex; i++)
        //    {
        //        pages.Add(new ListItem(i.ToString(), i.ToString(), i != currentPage));
        //    }

        //    //Add the Next Button.
        //    if (currentPage < pageCount)
        //    {
        //        pages.Add(new ListItem(">>", (currentPage + 1).ToString()));
        //    }

        //    //Add the Last Button.
        //    if (currentPage != pageCount)
        //    {
        //        pages.Add(new ListItem("Last", pageCount.ToString()));
        //    }
        //    rptPager.DataSource = pages;
        //    rptPager.DataBind();
        //}

        //protected void Page_Changed(object sender, EventArgs e)
        //{
        //    int pageIndex = int.Parse((sender as LinkButton).CommandArgument);
        //    this.LoadGridDataByCriteria(pageIndex);
        //} 
        #endregion

        //private void LoadGridDataFromTemporaryList()
        //{
        //    try
        //    {
        //        if (UserSession.TemporaryList != null)
        //        {
        //            if (UserSession.TemporaryList["SEARCHBY"] != null)
        //            {
        //                DataTable objDataTable = new DataTable();
        //                DocumentManager objDocumentManager = new DocumentManager();
        //                if (UserSession.TemporaryList["SEARCHBY"].ToString().Trim() == "1")
        //                {
        //                    if (UserSession.TemporaryList["METADATA"] != null && UserSession.TemporaryList["DOCUMENTENTRY"] != null && UserSession.TemporaryList["SEARCHTYPE"] != null)
        //                    {
        //                        if (((BusinessLogic.MetaData.SearchType)UserSession.TemporaryList["SEARCHTYPE"]) == BusinessLogic.MetaData.SearchType.EqualTo)
        //                            ddlCriteria.SelectedIndex = 0;
        //                        else
        //                            ddlCriteria.SelectedIndex = 1;

        //                        rdblSearchBy.Items.FindByValue("1").Selected = true;
        //                        objUtility.Result = objDocumentManager.SelectMetaDataForSearch(out objDataTable, ((DMS.BusinessLogic.MetaData)UserSession.TemporaryList["METADATA"]), ((DMS.BusinessLogic.DocumentEntry)UserSession.TemporaryList["DOCUMENTENTRY"]), ((BusinessLogic.MetaData.SearchType)UserSession.TemporaryList["SEARCHTYPE"]));
        //                    }
        //                }
        //                else if (UserSession.TemporaryList["SEARCHBY"].ToString().Trim() == "2")
        //                {
        //                    if (UserSession.TemporaryList["METADATA"] != null && UserSession.TemporaryList["DOCUMENT"] != null && UserSession.TemporaryList["SEARCHTYPE"] != null && UserSession.TemporaryList["TEXT"] != null)
        //                    {
        //                        if (((BusinessLogic.MetaData.SearchType)UserSession.TemporaryList["SEARCHTYPE"]) == BusinessLogic.MetaData.SearchType.EqualTo)
        //                            ddlCriteria.SelectedIndex = 0;
        //                        else
        //                            ddlCriteria.SelectedIndex = 1;

        //                        rdblSearchBy.Items.FindByValue("2").Selected = true;
        //                        txtTextToSeach.Text = UserSession.TemporaryList["TEXT"].ToString();
        //                        objUtility.Result = objDocumentManager.SelectMetaDataForTagSearch(out objDataTable, ((DMS.BusinessLogic.MetaData)UserSession.TemporaryList["METADATA"]), ((DMS.BusinessLogic.Document)UserSession.TemporaryList["DOCUMENT"]), ((BusinessLogic.MetaData.SearchType)UserSession.TemporaryList["SEARCHTYPE"]));
        //                    }
        //                }
        //                else if (UserSession.TemporaryList["SEARCHBY"].ToString().Trim() == "3")
        //                {
        //                    if (UserSession.TemporaryList["METADATA"] != null && UserSession.TemporaryList["DOCUMENTID"] != null && UserSession.TemporaryList["TEXT"] != null)
        //                    {
        //                        rdblSearchBy.Items.FindByValue("3").Selected = true;
        //                        txtTextToSeach.Text = UserSession.TemporaryList["TEXT"].ToString();
        //                        objUtility.Result = objDocumentManager.SelectMetaDataForContentSearch(out objDataTable, ((DMS.BusinessLogic.MetaData)UserSession.TemporaryList["METADATA"]), UserSession.TemporaryList["DOCUMENTID"].ToString().ToLower());
        //                    }
        //                }

        //                else if (UserSession.TemporaryList["SEARCHBY"].ToString().Trim() == "4")
        //                {
        //                    if (UserSession.TemporaryList["METADATA"] != null && UserSession.TemporaryList["DOCUMENT"] != null && UserSession.TemporaryList["SEARCHTYPE"] != null)
        //                    {
        //                        if (((BusinessLogic.MetaData.SearchType)UserSession.TemporaryList["SEARCHTYPE"]) == BusinessLogic.MetaData.SearchType.EqualTo)
        //                            ddlCriteria.SelectedIndex = 0;
        //                        else
        //                            ddlCriteria.SelectedIndex = 1;
        //                        rdblSearchBy.Items.FindByValue("4").Selected = true;
        //                        objUtility.Result = objDocumentManager.SelectMetaDataForFolderSearch(out objDataTable, ((DMS.BusinessLogic.MetaData)UserSession.TemporaryList["METADATA"]));
        //                        ////(emodModule.FindControl("ddlCategoryName") as DropDownList).SelectedValue = Convert.ToString(((BusinessLogic.MetaData)UserSession.TemporaryList["METADATA"]).CategoryID);
        //                        //(emodModule.FindControl("ddlCategoryName") as DropDownList).Items.FindByValue(Convert.ToString(((BusinessLogic.MetaData)UserSession.TemporaryList["METADATA"]).CategoryID)).Selected = true;
        //                        //emodModule.ddlCategoryName_SelectedIndexChanged(null, null);

        //                        //txtTextToSeach.Text = "";
        //                        //txtTextToSeach.Text = UserSession.TemporaryList["TEXT"].ToString();
        //                        //objUtility.Result = objDocumentManager.SelectMetaDataForFolderSearch (out objDataTable, ((DMS.BusinessLogic.MetaData)UserSession.TemporaryList["METADATA"]));
        //                        //SelectMetaDataForTagSearch(out objDataTable, ((DMS.BusinessLogic.MetaData)UserSession.TemporaryList["METADATA"]), ((DMS.BusinessLogic.Document)UserSession.TemporaryList["DOCUMENT"]), ((BusinessLogic.MetaData.SearchType)UserSession.TemporaryList["SEARCHTYPE"]));
        //                    }
        //                }

        //                if (objUtility.Result == Utility.ResultType.Success)
        //                {
        //                    //  int i = UserSession.RepositoryID;

        //                    gvwDocument.DataSource = objDataTable;
        //                    if (((DMS.BusinessLogic.MetaData)UserSession.TemporaryList["METADATA"]).RepositoryID == 75 || ((DMS.BusinessLogic.MetaData)UserSession.TemporaryList["METADATA"]).RepositoryID == 76)
        //                    {
        //                        gvwDocument.AllowPaging = false;
        //                        ibtnDownloadFiles.Visible = true;

        //                    }
        //                    else
        //                    {
        //                        gvwDocument.AllowPaging = true;
        //                        ibtnDownloadFiles.Visible = false;
        //                        gvwDocument.PageSize = 10;
        //                    }
        //                    gvwDocument.DataBind();
        //                    //emodModule.SelectedRepository = ((DMS.BusinessLogic.MetaData)UserSession.TemporaryList["METADATA"]).RepositoryID;
        //                    //if (emodModule.SelectedRepository == 75 || emodModule.SelectedRepository == 76)
        //                    //    ibtnDownloadFiles.Visible = true;
        //                    ibtnDownloadFiles.Visible = (bool)UserSession.TemporaryList["DOWNLOADBTN"];
        //                    UserSession.FilterData = null;
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogManager.ErrorLog(Utility.LogFilePath, ex);
        //    }
        //}

        //public void Incrementcount()
        //{
        //    DbTransaction objDbTransaction = Utility.GetTransaction;
        //    try
        //    {
        //        // int Value = totalcount();

        //        objdocument.OutPutValuues = totalcount() + 1;
        //        objdocument.MetaDataID = UserSession.MetaDataID;
        //        objUtility.Result = Document.Increasecount(objdocument, objDbTransaction);
        //        objDbTransaction.Commit();
        //    }
        //    catch (Exception ex)
        //    {
        //        objDbTransaction.Rollback();
        //        LogManager.ErrorLog(Utility.LogFilePath, ex);
        //    }
        //}

        //public int totalcount()
        //{
        //    int count = 0;
        //    DbTransaction objDbTransaction = Utility.GetTransaction;
        //    try
        //    {
        //        objdocument.MetaDataID = UserSession.MetaDataID;
        //        objUtility.Result = Document.LastTotalCount(objdocument, objDbTransaction, out count);

        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return count;
        //}

        //protected void ibtnDownloadFiles_Click(object sender, EventArgs e)
        //{
        //    DownloadFiles();
        //}

        //public void DownloadFiles()
        //{
        //    int Id = 0;
        //    DataTable objTable = new DataTable();
        //    string Filepath = string.Empty;

        //    string FileName = string.Empty;
        //    using (ZipFile zip = new ZipFile())
        //    {
        //        zip.AlternateEncodingUsage = ZipOption.AsNecessary;
        //        foreach (GridViewRow row in gvwDocument.Rows)
        //        {
        //            if ((row.FindControl("chkChild") as CheckBox).Checked)
        //            {
        //                Id = (int)gvwDocument.DataKeys[row.RowIndex].Values["ID"];
        //                objTable = DataHelper.ExecuteDataTable("SELECT DocumentName,DocumentPath,PageCount=ISNULL(PageCount,'')+ ' ' + ISNULL(MergedPageCount,'') FROM vwDocument WHERE ID = " + Id, null);
        //                Filepath = objTable.Rows[0]["DocumentPath"].ToString();
        //                FileName = objTable.Rows[0]["DocumentName"].ToString();
        //                zip.AddFile(Filepath).FileName = FileName;

        //            }
        //        }
        //        if (zip.Count <= 0)
        //        {
        //            return;
        //        }
        //        Response.Clear();
        //        Response.BufferOutput = false;
        //        string zipName = String.Format("Zip_{0}.zip", DateTime.Now.ToString("yyyy-MMM-dd-HHmmss"));
        //        Response.ContentType = "application/zip";
        //        Response.AddHeader("content-disposition", "attachment; filename=" + zipName);
        //        zip.Save(Response.OutputStream);
        //        Response.End();
        //    }
        //}

        //protected void gvwDocument_Sorting(object sender, GridViewSortEventArgs e)
        //{
        //    try
        //    {
        //        if (UserSession.GridData != null)
        //        {
        //            if (ViewState[UserSession.SortExpression] == null || ViewState[UserSession.SortExpression].ToString() == "DESC")
        //                ViewState[UserSession.SortExpression] = "ASC";
        //            else
        //                ViewState[UserSession.SortExpression] = "DESC";

        //            if (UserSession.FilterData != null)
        //                gvwDocument.DataSource = UserSession.SortedFilterGridData(e.SortExpression, ViewState[UserSession.SortExpression].ToString());
        //            else
        //                gvwDocument.DataSource = UserSession.SortedGridData(e.SortExpression, ViewState[UserSession.SortExpression].ToString());

        //            gvwDocument.DataBind();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
        //        LogManager.ErrorLog(Utility.LogFilePath, ex);
        //    }
        //}

        //protected void gvwDocument_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    try
        //    {
        //        Utility.SetGridHoverStyle(e);
        //        //this.gvwDocument.Columns[0].Visible = false;

        //        // CheckBox chkHeader = (e.Row.FindControl("chkHeader") as CheckBox);
        //        //GridViewRow grv1 = gvwDocument.HeaderRow;
        //        //CheckBox chkHeader = (CheckBox)grv1.FindControl("chkHeader");

        //        //chkHeader.Visible = true;
        //        if (((DMS.BusinessLogic.MetaData)UserSession.TemporaryList["METADATA"]).RepositoryID == -1)
        //        //if (emodModule.SelectedRepository != -1)
        //        {
        //            if (((DMS.BusinessLogic.MetaData)UserSession.TemporaryList["METADATA"]).RepositoryID == 75 || ((DMS.BusinessLogic.MetaData)UserSession.TemporaryList["METADATA"]).RepositoryID == 76)
        //                if (emodModule.SelectedRepository == 75 || emodModule.SelectedRepository == 76)
        //                {

        //                    if (e.Row.RowType == DataControlRowType.DataRow)
        //                    {
        //                        GridViewRow grv1 = gvwDocument.HeaderRow;
        //                        CheckBox chkHeader = (CheckBox)grv1.FindControl("chkHeader");
        //                        CheckBox chkChild = (e.Row.FindControl("chkChild") as CheckBox);
        //                        chkHeader.Visible = true;
        //                        chkChild.Visible = true;

        //                    }
        //                }
        //                else
        //                {
        //                    if (e.Row.RowType == DataControlRowType.DataRow)
        //                    {
        //                        GridViewRow grv1 = gvwDocument.HeaderRow;
        //                        CheckBox chkHeader = (CheckBox)grv1.FindControl("chkHeader");
        //                        CheckBox chkChild = (e.Row.FindControl("chkChild") as CheckBox);
        //                        chkHeader.Visible = false;
        //                        chkChild.Visible = false;

        //                    }
        //                }
        //        }
        //        if (e.Row.RowType == DataControlRowType.Footer)
        //        {
        //            e.Row.Cells[0].ColumnSpan = 7;
        //            for (int i = 1; i < 10; i++)
        //            {
        //                e.Row.Cells[i].Visible = false;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
        //        LogManager.ErrorLog(Utility.LogFilePath, ex);
        //    }
        //}



        //protected void gvwDocument_PageIndexChanging(object sender, GridViewPageEventArgs e)
        //{
        //    try
        //    {
        //        if (UserSession.GridData != null)
        //        {
        //            gvwDocument.PageIndex = e.NewPageIndex;
        //            if (UserSession.FilterData == null)
        //                gvwDocument.DataSource = UserSession.GridData;
        //            else
        //                gvwDocument.DataSource = UserSession.FilterData;

        //            gvwDocument.DataBind();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
        //        LogManager.ErrorLog(Utility.LogFilePath, ex);
        //    }
        //}

        //protected void ibtnFilterGrid_Click(object sender, ImageClickEventArgs e)
        //{
        //    try
        //    {
        //        if (UserSession.GridData != null)
        //        {
        //            string strFilterBy = ((DropDownList)gvwDocument.FooterRow.FindControl("ddlFilterGrid")).SelectedValue.Trim();
        //            string strFilterText = ((TextBox)gvwDocument.FooterRow.FindControl("txtFilterGrid")).Text.Trim();
        //            if (strFilterText == string.Empty)
        //            {
        //                gvwDocument.DataSource = UserSession.GridData;
        //                gvwDocument.DataBind();
        //                UserSession.FilterData = null;
        //            }
        //            else
        //            {
        //                DataRow[] objRows = null;

        //                if (strFilterBy == "1")
        //                    objRows = UserSession.GridData.Select("DocumentName LIKE '%" + strFilterText + "%'");
        //                else if (strFilterBy == "2")
        //                    objRows = UserSession.GridData.Select("Tag LIKE '%" + strFilterText + "%'");

        //                if (objRows.Length > 0)
        //                {
        //                    UserSession.FilterData = objRows.CopyToDataTable();
        //                    gvwDocument.DataSource = UserSession.FilterData;
        //                    gvwDocument.DataBind();
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogManager.ErrorLog(Utility.LogFilePath, ex);
        //        UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
        //    }
        //}

        //protected void OnRowEditing(object sender, GridViewEditEventArgs e)
        //{
        //    gvwDocument.EditIndex = e.NewEditIndex;
        //    gvwDocument.DataSource = UserSession.GridData;
        //    gvwDocument.DataBind();
        //}

        //protected void gvwDocument_RowUpdating(object sender, GridViewUpdateEventArgs e)
        //{
        //    try
        //    {
        //        int intRowIndex = Convert.ToInt32(e.RowIndex);
        //        GridViewRow Row = gvwDocument.Rows[e.RowIndex];
        //        int DocId = Convert.ToInt32(gvwDocument.DataKeys[e.RowIndex]["ID"].ToString());
        //        string DocumentName = (Row.FindControl("txtDocName") as TextBox).Text;
        //        string strDocTag = DocumentName.Substring(0, DocumentName.LastIndexOf("."));
        //        Session["DocumentName"] = strDocTag;

        //        bool transanction = false;

        //        DocumentManager objDocumentmanager = new DocumentManager();
        //        DbTransaction objDBTransaction = BusinessLogic.Utility.GetTransaction;
        //        objUtility.Result = objDocumentmanager.UpdateDocumentName(DocId, DocumentName, strDocTag, objDBTransaction);
        //        switch (objUtility.Result)
        //        {
        //            case Utility.ResultType.Success:
        //                objDBTransaction.Commit();
        //                transanction = true;
        //                UserSession.DisplayMessage(this, "Document Name is Updated SuccessFully", MainMasterPage.MessageType.Success);
        //                break;
        //            case Utility.ResultType.Error:
        //                objDBTransaction.Rollback();
        //                UserSession.DisplayMessage(this, "Sorry, Some Error Has Been Occured.", MainMasterPage.MessageType.Error);
        //                break;
        //        }
        //        if (transanction == true)
        //        {
        //            //Get MacAddress of Machine
        //            DbTransaction objDBTransaction1 = BusinessLogic.Utility.GetTransaction;
        //            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
        //            // UserManager objUserManager=new UserManager();
        //            Report objReport = new Report();
        //            string MacAdd = nics[0].GetPhysicalAddress().ToString();
        //            string IpAddress = Utility.GetIPAddress(HttpContext.Current);
        //            string Activity = "Document Renamed";
        //            int UserId = UserSession.UserID;
        //            DateTime DateOfActivity = DateTime.Now;
        //            string DocName = Convert.ToString(Session["DocumentName"]);
        //            string IPAddress = Utility.GetIPAddress(HttpContext.Current);
        //            string MacAddress = Utility.GetMacAddress();
        //            objReport.InsertAuditLog(IPAddress, MacAddress, Activity, "null", UserSession.UserID);
        //            // objUtility.Result = objUserManager.
        //            //AuditLogDetails(IpAddress, MacAdd, Activity, DocName, UserId, objDBTransaction1);
        //            switch (objUtility.Result)
        //            {
        //                case Utility.ResultType.Success:
        //                    objDBTransaction1.Commit();
        //                    break;
        //                case Utility.ResultType.Error:
        //                    objDBTransaction.Rollback();
        //                    break;
        //            }

        //        }
        //        gvwDocument.EditIndex = -1;
        //        LoadGridDataByCriteria();
        //    }
        //    catch (Exception ex)
        //    {
        //        LogManager.ErrorLog(Utility.LogFilePath, ex);
        //        UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured.", MainMasterPage.MessageType.Error);
        //    }
        //}

        //protected void gvwDocument_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        //{
        //    try
        //    {
        //        gvwDocument.EditIndex = -1;
        //        LoadGridDataByCriteria();
        //    }
        //    catch (Exception ex)
        //    {
        //        LogManager.ErrorLog(Utility.LogFilePath, ex);
        //        UserSession.DisplayMessage(this, "Sorry, Some Error Has Been Occured.", MainMasterPage.MessageType.Error);
        //        throw;
        //    }
        //}


    }
}