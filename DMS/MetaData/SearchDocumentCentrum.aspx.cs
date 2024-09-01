using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DMS.BusinessLogic;
using System.Data.Common;
using System.Data;
using System.Collections;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.ComponentModel;
using System.Text;
using System.Net;
using System.Net.NetworkInformation;

namespace DMS.Shared
{

    public partial class SearchDocumentCentrum : System.Web.UI.Page
    {
        Utility objUtility = new Utility();
        SqlConnection con = new SqlConnection(Utility.ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

            }
            if (Request.UrlReferrer != null)
            {
                ViewState["LASTPAGEURL"] = Request.UrlReferrer.ToString().ToLower();

                if (ViewState["LASTPAGEURL"].ToString().Contains("viewdocumentforsearch"))
                {
                    if ((Session["SelectedRB"]) != null)
                    {
                        int num = Convert.ToInt32(Session["SelectedRB"]);
                        if (num == 1 || num == 2)
                        {
                            gvDocSearch.DataSource = Session["Result"];
                            gvDocSearch.DataBind();
                            gvwDocument.DataSource = Session["SearchFinal"];
                            gvwDocument.DataBind();

                        }
                        if (num == 3)
                        {
                            gvDocSearch.Visible = false;
                            gvwDocument.Visible = false;
                            gvwContentSearch.DataSource = Session["ContentSearch"];
                            gvwContentSearch.DataBind();
                        }
                        if (num == 4)
                        {
                            gvDocSearch.Visible = false;
                            gvwDocument.Visible = false;
                            gvwContentSearch.DataSource = Session["FileSearch"];
                            gvwContentSearch.DataBind();
                        }
                    }
                }

            }
        }

        protected void ibtnShow_Click(object sender, ImageClickEventArgs e)
        {
            if (txtTextToSeach.Text == string.Empty)
            {
                UserSession.DisplayMessage(this, "Please Enter the text to Search.", MainMasterPage.MessageType.Error);
            }
            else
            {
                LoadGridDataByCriteria();
            }

        }

        private void LoadGridDataByCriteria()
        {
            try
            {

                DocumentManager objDocumentManager = new DocumentManager();
                BusinessLogic.MetaData objMetaData = new BusinessLogic.MetaData()
                {
                    RepositoryID = emodModuleCentrum.SelectedRepository,
                    MetaTemplateID = emodModuleCentrum.SelectedMetaTemplate,
                    CategoryID = emodModuleCentrum.SelectedCategory,
                    FolderID = emodModuleCentrum.SelectedFolder,
                    MetaDataID = emodModuleCentrum.SelectedMetaDataCode
                };
                Hashtable objTempList = new Hashtable();
                DataTable dtSearchResult = new DataTable();
                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;
                if (rdblSearchBy.SelectedValue == "1")
                {
                    //Pan no search 17-3-17
                    Session["SelectedRB"] = null;
                    Session["SelectedRB"] = 1;
                    gvDocSearch.Visible = true;
                    gvwDocument.Visible = false;
                    //pan no search in Centrum Master table
                    if (txtTextToSeach.Text != null)
                    {
                        string SearchTextPan = txtTextToSeach.Text;
                        DbParameter[] objDbParameter = new DbParameter[5];
                        objDbParameter[0] = objDbProviderFactory.CreateParameter();
                        objDbParameter[0].ParameterName = "@pan_no";
                        objDbParameter[0].Value = SearchTextPan;

                        objDbParameter[1] = objDbProviderFactory.CreateParameter();
                        objDbParameter[1].ParameterName = "@Repository";
                        if (emodModuleCentrum.SelectedRepository == -1)
                        {
                            objDbParameter[1].Value = null;
                        }
                        else
                        {
                            objDbParameter[1].Value = emodModuleCentrum.SelectedRepository;
                        }

                        objDbParameter[2] = objDbProviderFactory.CreateParameter();
                        objDbParameter[2].ParameterName = "@metatemplate";
                        if (emodModuleCentrum.SelectedMetaTemplate == -1)
                        {
                            objDbParameter[2].Value = null;
                        }
                        else
                        {
                            objDbParameter[2].Value = emodModuleCentrum.SelectedMetaTemplate;
                        }

                        objDbParameter[3] = objDbProviderFactory.CreateParameter();
                        objDbParameter[3].ParameterName = "@category";
                        if (emodModuleCentrum.SelectedCategory == -1)
                        {
                            objDbParameter[3].Value = null;
                        }
                        else
                        {
                            objDbParameter[3].Value = emodModuleCentrum.SelectedCategory;
                        }

                        objDbParameter[4] = objDbProviderFactory.CreateParameter();
                        objDbParameter[4].ParameterName = "@folder";
                        if (emodModuleCentrum.SelectedFolder == 0)
                        {
                            objDbParameter[4].Value = null;
                        }
                        else
                        {
                            objDbParameter[4].Value = emodModuleCentrum.SelectedFolder;
                        }

                        dtSearchResult = DataHelper.ExecuteDataTableForProcedure("SP_S_SelectSearchAllCentrum", null, objDbParameter);
                        Session["Result"] = dtSearchResult;
                        if (dtSearchResult.Rows.Count < 1)
                        {
                            UserSession.DisplayMessage(this, "Sorry, No Such Record Found.", MainMasterPage.MessageType.Error);
                        }
                        else
                        {
                            gvDocSearch.DataSource = dtSearchResult;
                            gvDocSearch.DataBind();
                        }
                    }
                }

                if (rdblSearchBy.SelectedValue == "2")
                {
                    //customer name search 20-03-17
                    Session["SelectedRB"] = null;
                    Session["SelectedRB"] = 2;
                    gvDocSearch.Visible = true;
                    gvwDocument.Visible = false;
                    string SearchTextCust = txtTextToSeach.Text;
                    DbParameter[] objDbParameter = new DbParameter[5];
                    objDbParameter[0] = objDbProviderFactory.CreateParameter();
                    objDbParameter[0].ParameterName = "@CustName";
                    objDbParameter[0].Value = SearchTextCust;

                    objDbParameter[1] = objDbProviderFactory.CreateParameter();
                    objDbParameter[1].ParameterName = "@Repository";
                    if (emodModuleCentrum.SelectedRepository == -1)
                    {
                        objDbParameter[1].Value = null;
                    }
                    else
                    {
                        objDbParameter[1].Value = emodModuleCentrum.SelectedRepository;
                    }

                    objDbParameter[2] = objDbProviderFactory.CreateParameter();
                    objDbParameter[2].ParameterName = "@metatemplate";
                    if (emodModuleCentrum.SelectedMetaTemplate == -1)
                    {
                        objDbParameter[2].Value = null;
                    }
                    else
                    {
                        objDbParameter[2].Value = emodModuleCentrum.SelectedMetaTemplate;
                    }

                    objDbParameter[3] = objDbProviderFactory.CreateParameter();
                    objDbParameter[3].ParameterName = "@category";
                    if (emodModuleCentrum.SelectedCategory == -1)
                    {
                        objDbParameter[3].Value = null;
                    }
                    else
                    {
                        objDbParameter[3].Value = emodModuleCentrum.SelectedCategory;
                    }

                    objDbParameter[4] = objDbProviderFactory.CreateParameter();
                    objDbParameter[4].ParameterName = "@folder";
                    if (emodModuleCentrum.SelectedFolder == 0)
                    {
                        objDbParameter[4].Value = null;
                    }
                    else
                    {
                        objDbParameter[4].Value = emodModuleCentrum.SelectedFolder;
                    }
                    dtSearchResult = DataHelper.ExecuteDataTableForProcedure("SP_S_SelectSearchAllCentrumCust", null, objDbParameter);
                    Session["Result"] = dtSearchResult;
                    if (dtSearchResult.Rows.Count < 1)
                    {
                        UserSession.DisplayMessage(this, "Sorry, No Such Record Found.", MainMasterPage.MessageType.Error);
                    }
                    else
                    {
                        gvDocSearch.DataSource = dtSearchResult;
                        gvDocSearch.DataBind();
                    }
                }
                             

                if (rdblSearchBy.SelectedValue == "3")
                {
                    #region old search
                    Session["SelectedRB"] = null;
                    Session["SelectedRB"] = 3;
                    if (txtTextToSeach.Text.Trim() == string.Empty)
                    {
                        UserSession.DisplayMessage(this, "Please Enter The Text To Search .", MainMasterPage.MessageType.Warning);
                        return;
                    }
                    string strDocumentID = string.Empty;
                    strDocumentID = Utility.SearchPageContent(txtTextToSeach.Text.Trim().ToLower());

                    if (strDocumentID.Trim() == string.Empty)
                    {
                        UserSession.DisplayMessage(this, "No Result To Display .", MainMasterPage.MessageType.Warning);
                        return;
                    }                   

                    //DataTable objDataTable = new DataTable();
                    //StringBuilder sbDocumentIDQuery = new StringBuilder();
                    //string strQueryFormat = " D.ID in ({0}) OR";

                    //foreach (string strID in strDocumentID.Split(','))
                    //{
                    //    sbDocumentIDQuery.Append(string.Format(strQueryFormat, strID));
                    //}
                    
//                    string Query = "select A.ID,A.DocumentName, A.Pan_no,k.Customer_Name,A.MetaTemplateName,a.CreatedOn,k.Account_Opening_Date, k.Account_Closing_Date from " +
//"(select D.DocumentName,D.ID,M.MetaDataCode, D.CreatedOn,C.CategoryName,MT.MetaTemplateName,f.FolderName, " +
//"(select FolderName from Folder where ID=F.ParentFolderID) as Pan_no from Document D " +
//                   "inner join MetaData M on  D.MetaDataID=M.ID " +
//                   "inner join Folder F on F.id=M.FolderID " +
//                   "inner join MetaTemplate MT on MT.ID=M.MetaTemplateID " +
//                   "inner join Category C on C.ID=M.CategoryID " +
//                   "inner join Repository R on R.ID=M.RepositoryID	" +
//                   "where " + sbDocumentIDQuery.ToString().Trim('R', 'O') + " and  R.ID=37) as A " +
//                   "inner join (select distinct(Pan_No),Customer_Name,Account_Opening_Date, Account_Closing_Date from Centrum_Master) as K on k.Pan_No=A.Pan_no ";
                  //  con.Open();
                    SqlCommand cmd = new SqlCommand();
                   // cmd.CommandText = string.Format(query, string.Join(",", pages.ToArray()));
                  //  cmd.Connection = con;
                   // cmd.ExecuteNonQuery();
                   
                    //DataTable objDataTable1 = new DataTable();
                    //dtSearchResult = DataHelper.ExecuteDataTable(Query, null);
                    DbParameter[] objDbParameter = new DbParameter[4];
                    objDbParameter[0] = objDbProviderFactory.CreateParameter();
                    objDbParameter[0].ParameterName = "@DocumentID";
                    objDbParameter[0].Value = strDocumentID;

                    objDbParameter[1] = objDbProviderFactory.CreateParameter();
                    objDbParameter[1].ParameterName = "@Repository";
                    if (emodModuleCentrum.SelectedRepository == -1)
                    {
                        objDbParameter[1].Value = null;
                    }
                    else
                    {
                        objDbParameter[1].Value = emodModuleCentrum.SelectedRepository;
                    }

                    objDbParameter[2] = objDbProviderFactory.CreateParameter();
                    objDbParameter[2].ParameterName = "@metatemplate";
                    if (emodModuleCentrum.SelectedMetaTemplate == -1)
                    {
                        objDbParameter[2].Value = null;
                    }
                    else
                    {
                        objDbParameter[2].Value = emodModuleCentrum.SelectedMetaTemplate;
                    }

                    objDbParameter[3] = objDbProviderFactory.CreateParameter();
                    objDbParameter[3].ParameterName = "@category";
                    if (emodModuleCentrum.SelectedCategory == -1)
                    {
                        objDbParameter[3].Value = null;
                    }
                    else
                    {
                        objDbParameter[3].Value = emodModuleCentrum.SelectedCategory;
                    }
                    dtSearchResult = DataHelper.ExecuteDataTableForProcedure("SP_S_SelectSearchAllCentrumContentDoc", null, objDbParameter);
                   // con.Close();
                    Session["ContentSearch"] = dtSearchResult;
                    gvwContentSearch.Visible = true;
                    gvwContentSearch.DataSource = dtSearchResult;
                    gvwContentSearch.DataBind();
                    //objUtility.Result = objDocumentManager.SelectMetaDataForContentSearch(out objDataTable, objMetaData, strDocumentID);
                    //objTempList.Add("SEARCHBY", rdblSearchBy.SelectedValue);
                    //objTempList.Add("METADATA", objMetaData);
                    //objTempList.Add("DOCUMENTID", strDocumentID);
                    //objTempList.Add("TEXT", txtTextToSeach.Text.Trim());
                    //UserSession.TemporaryList = objTempList;

                    //switch (objUtility.Result)
                    //{
                    //    case Utility.ResultType.Success:
                    //        gvDocSearch.Visible = false;
                    //        gvwDocument.DataSource = objDataTable;
                    //        gvwDocument.DataBind();
                    //        UserSession.FilterData = null;
                    //        UserSession.GridData = objDataTable;
                    //        break;

                    //    case Utility.ResultType.Failure:
                    //        gvDocSearch.Visible = false;
                    //        gvwDocument.DataSource = objDataTable;
                    //        gvwDocument.DataBind();
                    //        UserSession.FilterData = null;
                    //        UserSession.DisplayMessage(this, "No Data To Display .", MainMasterPage.MessageType.Warning);
                    //        break;

                    //    case Utility.ResultType.Error:
                    //        gvDocSearch.Visible = false;
                    //        gvwDocument.DataSource = objDataTable;
                    //        gvwDocument.DataBind();
                    //        UserSession.FilterData = null;
                    //        UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                    //        break;
                    //}
                    #endregion old search
                    //#region new search
                    //try
                    //{
                    //    //Content Search 22-03-17
                    //    Session["SelectedRB"] = null;
                    //    Session["SelectedRB"] = 3;
                    //    gvDocSearch.Visible = false;
                    //    gvwDocument.Visible = false;
                    //   // ContentSearchAll(emodModule.SelectedRepository, emodModule.SelectedMetaTemplate, emodModule.SelectedCategory, emodModule.SelectedFolder);
                    //    ContentSearch(objMetaData.RepositoryID);
                    //}
                    //catch (Exception ex)
                    //{
                    //    LogManager.ErrorLog(Utility.LogFilePath, ex);
                    //    UserSession.DisplayMessage(this, "PDF file does not contained search text entered by user.", MainMasterPage.MessageType.Error);
                    //}
                    //#endregion new search
                }

                if (rdblSearchBy.SelectedValue == "4")
                {
                    //filename search
                    Session["SelectedRB"] = null;
                    Session["SelectedRB"] = 4;
                    string SearchTextCust = txtTextToSeach.Text;
                    DbParameter[] objDbParameter = new DbParameter[5];
                    objDbParameter[0] = objDbProviderFactory.CreateParameter();
                    objDbParameter[0].ParameterName = "@filename";
                    objDbParameter[0].Value = SearchTextCust;

                    objDbParameter[1] = objDbProviderFactory.CreateParameter();
                    objDbParameter[1].ParameterName = "@Repository";
                    if (emodModuleCentrum.SelectedRepository == -1)
                    {
                        objDbParameter[1].Value = null;
                    }
                    else
                    {
                        objDbParameter[1].Value = emodModuleCentrum.SelectedRepository;
                    }

                    objDbParameter[2] = objDbProviderFactory.CreateParameter();
                    objDbParameter[2].ParameterName = "@metatemplate";
                    if (emodModuleCentrum.SelectedMetaTemplate == -1)
                    {
                        objDbParameter[2].Value = null;
                    }
                    else
                    {
                        objDbParameter[2].Value = emodModuleCentrum.SelectedMetaTemplate;
                    }

                    objDbParameter[3] = objDbProviderFactory.CreateParameter();
                    objDbParameter[3].ParameterName = "@category";
                    if (emodModuleCentrum.SelectedCategory == -1)
                    {
                        objDbParameter[3].Value = null;
                    }
                    else
                    {
                        objDbParameter[3].Value = emodModuleCentrum.SelectedCategory;
                    }

                    objDbParameter[4] = objDbProviderFactory.CreateParameter();
                    objDbParameter[4].ParameterName = "@folder";
                    if (emodModuleCentrum.SelectedFolder == 0)
                    {
                        objDbParameter[4].Value = null;
                    }
                    else
                    {
                        objDbParameter[4].Value = emodModuleCentrum.SelectedFolder;
                    }
                    dtSearchResult = DataHelper.ExecuteDataTableForProcedure("SP_S_SelectSearchAllCentrumFile", null, objDbParameter);
                    Session["FileSearch"] = dtSearchResult;
                    gvDocSearch.Visible = false;
                    gvwDocument.Visible = false;
                    gvwContentSearch.Visible = true;
                    gvwContentSearch.DataSource = dtSearchResult;
                    gvwContentSearch.DataBind();
                }       


            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
            }
        }

//        private void ContentSearchAll(int RepID, int metatemplate, int category, int folder)
//        {
//            string str = txtTextToSeach.Text;
//            DataTable ds = new DataTable();
//            ds = Document.SelectAllDocCentrum(RepID, metatemplate, category, folder);
//            List<int> pages = new List<int>();
//            if (ds.Rows.Count > 0)
//            {
//                for (int i = 0; i < ds.Rows.Count; i++)
//                {

//                    if (File.Exists(ds.Rows[i]["DocumentPath"].ToString()))
//                    {
//                        PdfReader pdfReader = new PdfReader(ds.Rows[i]["DocumentPath"].ToString());
//                        for (int page = 1; page <= pdfReader.NumberOfPages; page++)
//                        {
//                            ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();

//                            string currentPageText = PdfTextExtractor.GetTextFromPage(pdfReader, page, strategy);
//                            if (currentPageText.Contains(str))
//                            {
//                                pages.Add(Convert.ToInt32(ds.Rows[i]["id"]));
//                                break;
//                            }
//                        }
//                    }
//                }
//            }

//            //covert list to datatable
//            DataTable dtConvert = new DataTable();
//            dtConvert = ToDataTable(pages);
//            //passing list to query
//            string query = "select A.ID,A.DocumentName, A.Pan_no,k.Customer_Name,A.MetaTemplateName,a.CreatedOn,k.Account_Opening_Date, k.Account_Closing_Date from " +
//"(select D.DocumentName,D.ID,M.MetaDataCode, D.CreatedOn,C.CategoryName,MT.MetaTemplateName,f.FolderName, " +
// "(select FolderName from Folder where ID=F.ParentFolderID) as Pan_no from Document D " +
//                    "inner join MetaData M on  D.MetaDataID=M.ID " +
//                    "inner join Folder F on F.id=M.FolderID " +
//                    "inner join MetaTemplate MT on MT.ID=M.MetaTemplateID " +
//                    "inner join Category C on C.ID=M.CategoryID " +
//                    "inner join Repository R on R.ID=M.RepositoryID	" +
//                    "where d.ID in({0}) and  R.ID=37) as A " +
//                    "inner join (select distinct(Pan_No),Customer_Name,Account_Opening_Date, Account_Closing_Date from Centrum_Master) as K on k.Pan_No=A.Pan_no ";
//            con.Open();
//            SqlCommand cmd = new SqlCommand();
//            cmd.CommandText = string.Format(query, string.Join(",", pages.ToArray()));
//            cmd.Connection = con;
//            cmd.ExecuteNonQuery();
//            con.Close();
//            DataTable objDataTable = new DataTable();
//            objDataTable = DataHelper.ExecuteDataTable(cmd.CommandText, null);
//            Session["ContentSearch"] = objDataTable;
//            gvwContentSearch.Visible = true;
//            gvwContentSearch.DataSource = objDataTable;
//            gvwContentSearch.DataBind();

//        }


        //public static DataTable ToDataTable(IList<int> data)
        //{
        //    DataTable dt = new DataTable();
        //    DataColumn dc = new DataColumn("DocID", typeof(String));
        //    dt.Columns.Add(dc);

        //    foreach (var temp in data)
        //    {
        //        DataRow dr = dt.NewRow();
        //        dr[0] = temp;
        //        dt.Rows.Add(dr);
        //    }
          
        //    return dt;
        //}


        public void ContentSearch(int RepID)
        {
            string str = txtTextToSeach.Text;
            DataSet ds = new DataSet();
            ds = Document.SelectAllDocs(RepID);
            List<int> pages = new List<int>();
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {

                    if (File.Exists(ds.Tables[0].Rows[i]["DocumentPath"].ToString()))
                    {
                        PdfReader pdfReader = new PdfReader(ds.Tables[0].Rows[i]["DocumentPath"].ToString());
                        for (int page = 1; page <= pdfReader.NumberOfPages; page++)
                        {
                            ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();

                            string currentPageText = PdfTextExtractor.GetTextFromPage(pdfReader, page, strategy);
                            if (currentPageText.Contains(str))
                            {
                                pages.Add(Convert.ToInt32(ds.Tables[0].Rows[i]["id"]));
                                break;
                            }
                        }
                    }
                }
            }

            //passing list to query
            string query = "select A.ID,A.DocumentName, A.Pan_no,k.Customer_Name,A.MetaTemplateName,a.CreatedOn,k.Account_Opening_Date, k.Account_Closing_Date from " +
"(select D.DocumentName,D.ID,M.MetaDataCode, D.CreatedOn,C.CategoryName,MT.MetaTemplateName,f.FolderName, " +
 "(select FolderName from Folder where ID=F.ParentFolderID) as Pan_no from Document D " +
                    "inner join MetaData M on  D.MetaDataID=M.ID " +
                    "inner join Folder F on F.id=M.FolderID " +
                    "inner join MetaTemplate MT on MT.ID=M.MetaTemplateID " +
                    "inner join Category C on C.ID=M.CategoryID " +
                    "inner join Repository R on R.ID=M.RepositoryID	" +
                    "where d.ID in({0}) and  R.ID=37) as A " +
                    "inner join (select distinct(Pan_No),Customer_Name,Account_Opening_Date, Account_Closing_Date from Centrum_Master) as K on k.Pan_No=A.Pan_no ";
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = string.Format(query, string.Join(",", pages.ToArray()));
            cmd.Connection = con;
            cmd.ExecuteNonQuery();
            con.Close();
            DataTable objDataTable = new DataTable();
            objDataTable = DataHelper.ExecuteDataTable(cmd.CommandText, null);
            Session["ContentSearch"] = objDataTable;
            gvwContentSearch.Visible = true;
            gvwContentSearch.DataSource = objDataTable;
            gvwContentSearch.DataBind();
        }

        protected void gvDocSearch_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvDocSearch.PageIndex = e.NewPageIndex;
                gvDocSearch.DataSource = Session["Result"];
                gvDocSearch.DataBind();
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
            }
        }

        protected void gvDocSearch_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.ToLower().Trim() == "viewdetail")
            {
                int intRowIndex = Convert.ToInt32(e.CommandArgument);
                string PanNo = gvDocSearch.DataKeys[intRowIndex].Values["Pan_No"].ToString();
                string CustomerName = gvDocSearch.DataKeys[intRowIndex].Values["Customer_Name"].ToString();

                DataTable dtSearchFinal = new DataTable();
                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;
                DbParameter[] objDbParameter = new DbParameter[5];
                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "@pan_no";
                objDbParameter[0].Value = PanNo;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "@Repository";
                if (emodModuleCentrum.SelectedRepository == -1)
                {
                    objDbParameter[1].Value = null;
                }
                else
                {
                    objDbParameter[1].Value = emodModuleCentrum.SelectedRepository;
                }

                objDbParameter[2] = objDbProviderFactory.CreateParameter();
                objDbParameter[2].ParameterName = "@metatemplate";
                if (emodModuleCentrum.SelectedMetaTemplate == -1)
                {
                    objDbParameter[2].Value = null;
                }
                else
                {
                    objDbParameter[2].Value = emodModuleCentrum.SelectedMetaTemplate;
                }

                objDbParameter[3] = objDbProviderFactory.CreateParameter();
                objDbParameter[3].ParameterName = "@category";
                if (emodModuleCentrum.SelectedCategory == -1)
                {
                    objDbParameter[3].Value = null;
                }
                else
                {
                    objDbParameter[3].Value = emodModuleCentrum.SelectedCategory;
                }

                objDbParameter[4] = objDbProviderFactory.CreateParameter();
                objDbParameter[4].ParameterName = "@folder";
                if (emodModuleCentrum.SelectedFolder == 0)
                {
                    objDbParameter[4].Value = null;
                }
                else
                {
                    objDbParameter[4].Value = emodModuleCentrum.SelectedFolder;
                }


                dtSearchFinal = DataHelper.ExecuteDataTableForProcedure("SP_SelectSearchCentrum", null, objDbParameter);
                Session["SearchFinal"] = dtSearchFinal;
                if (dtSearchFinal.Rows.Count < 0)
                {
                    UserSession.DisplayMessage(this, "Sorry ,No Data Found.", MainMasterPage.MessageType.Error);
                }
                else
                {
                    gvwDocument.Visible = true;
                    gvwDocument.DataSource = dtSearchFinal;
                    gvwDocument.DataBind();
                }

            }
        }

        protected void gvwDocument_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvwDocument.PageIndex = e.NewPageIndex;
            gvwDocument.DataSource = Session["SearchFinal"];
            gvwDocument.DataBind();
        }

        protected void gvwDocument_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.ToLower().Trim() == "documentsearch")
            {
                  int intRowIndex = Convert.ToInt32(e.CommandArgument);
                string DocID = gvwDocument.DataKeys[intRowIndex].Values["ID"].ToString().Trim();
                string DocName=gvwDocument.DataKeys[intRowIndex].Values["DocumentName"].ToString().Trim();
                DMS.BusinessLogic.Report objReport = new Report();
                objReport.InsertAuditLog(GetIPAddress(), GetMacAddress(), "Document Viewing", DocName, UserSession.UserID);
                Session["DocID"] = DocID;
                Response.Redirect("../MetaData/ViewDocumentForSearch.aspx?DOCID=" + DocID, false);
                //Response.Redirect("../MetaData/DocumentViewer.aspx?DOCID=" + DocID, false);
            }


        }

        protected void gvwContentSearch_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvwContentSearch.PageIndex = e.NewPageIndex;
            gvwContentSearch.DataSource = Session["ContentSearch"];
            gvwContentSearch.DataBind();
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

        protected void gvwContentSearch_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.ToLower().Trim() == "documentsearch")
            {
                int intRowIndex = Convert.ToInt32(e.CommandArgument);
                string DocID = gvwContentSearch.DataKeys[intRowIndex].Values["ID"].ToString().Trim();
                string DocName = gvwContentSearch.DataKeys[intRowIndex].Values["DocumentName"].ToString().Trim();
                Session["DocID"] = DocID;
                //enter in audit log
                //string strHostName = "";
                //strHostName = System.Net.Dns.GetHostName();

                //IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(strHostName);

                //IPAddress[] addr = ipEntry.AddressList;
                Report objReport = new Report();
                string IPAddress = GetIPAddress();
                //DateTime Date = Convert.ToDateTime(DateTime.Today.ToShortDateString());
                //DateTime Logintime = Convert.ToDateTime(DateTime.Now);
                DateTime LoginDate = DateTime.Today;
                string Activity = "Document Viewed";

                string MacAddress = GetMacAddress();
                objReport.InsertAuditLog(IPAddress, MacAddress, Activity, DocName, UserSession.UserID);
                Response.Redirect("../MetaData/ViewDocumentForSearch.aspx?DOCID=" + DocID, false);
               // Response.Redirect("../MetaData/DocumentViewer.aspx?DOCID=" + DocID, false);
            }
        }

        protected void rdblSearchBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtTextToSeach.Text = string.Empty;
            gvDocSearch.Visible = false;
            gvwDocument.Visible = false;
            gvwContentSearch.Visible = false;
        }


    }
}