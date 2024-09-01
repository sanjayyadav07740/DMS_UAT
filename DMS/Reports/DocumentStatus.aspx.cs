using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DMS.BusinessLogic;
using System.Data;

namespace DMS.Reports
{
    public partial class DocumentStatus : System.Web.UI.Page
    
    {
        #region Private Member
        Utility objUtility = new Utility();
        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["GridData"] = null;
            }
         
        }

        protected void btnDocsStatus_Click(object sender, EventArgs e)
        {
          

            string strwhereCondition = "";

            int intRepositoryID = Convert.ToInt32(emodModule.SelectedRepository);
            int intMetaTemplateID = Convert.ToInt32(emodModule.SelectedMetaTemplate);
            int intCategoryID = Convert.ToInt32(emodModule.SelectedCategory);
            int intFolderID = Convert.ToInt32(emodModule.SelectedFolder);
      
            if (intRepositoryID>0)
                 strwhereCondition = " RepositoryID = " + intRepositoryID.ToString();

            if (intMetaTemplateID>0)
                strwhereCondition += " AND MetaTemplateID = " + intMetaTemplateID.ToString();
            else
                strwhereCondition += " AND MetaTemplateID IN (" + Utility.GetAccessRight(Utility.AccessRight.MetaTemplate) + ")";

            if (intCategoryID > 0)
                strwhereCondition += " AND CategoryID = " + intCategoryID.ToString();
            else
                strwhereCondition += " AND CategoryID  IN (" + Utility.GetAccessRight(Utility.AccessRight.Category) + ",-1)";


            if (intFolderID>0)
                strwhereCondition += " AND FolderID = " + intFolderID.ToString();
            else
                strwhereCondition += " AND FolderID IN (" + Utility.GetAccessRight(Utility.AccessRight.Folder) + ",0)";

            LoadGridData(strwhereCondition);
        }
          
        protected void Page_PreRender(object sender, EventArgs e)
        {
            ((RequiredFieldValidator)emodModule.FindControl("rfvMetaTemplateName")).Enabled = false;
        }
        
        protected void gvwDocsStatus_RowDataBound(object sender, GridViewRowEventArgs e)
        {


            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Create a new GridView for displaying the expanded details
            }
        }

        protected void gvwDocsStatus_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

            try
            {
                if (ViewState["GridData"] != null)
                {
                    gvwDocsStatus.PageIndex = e.NewPageIndex;
                    gvwDocsStatus.DataSource = (DataTable)ViewState["GridData"];
                    gvwDocsStatus.DataBind();
                }
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);

            }

        }

        protected void gvwDocsStatus_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                if (ViewState["GridData"] != null)
                {
                  

                    if (ViewState[UserSession.SortExpression] == null || ViewState[UserSession.SortExpression].ToString() == "DESC")
                        ViewState[UserSession.SortExpression] = "ASC";

                    else
                        ViewState[UserSession.SortExpression] = "DESC";


                    DataView objDataView = ((DataTable)ViewState["GridData"]).DefaultView;

                    objDataView.Sort = e.SortExpression + " " + ViewState[UserSession.SortExpression].ToString();

                    gvwDocsStatus.DataSource = objDataView;
                    gvwDocsStatus.DataBind();



                }
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);

            }

        }

        #endregion

        #region Methods

        private void LoadGridData(string strwhereCondition)
        {
            DocumentManager objDocumentManager = new DocumentManager();
            DataTable objDataTable = new DataTable();
            objUtility.Result = objDocumentManager.SelectDocument(out objDataTable, strwhereCondition);

            switch (objUtility.Result)
            {
                case Utility.ResultType.Success:
                    gvwDocsStatus.DataSource = objDataTable;
                    gvwDocsStatus.DataBind();
                    ViewState["GridData"] = objDataTable;
                    break;
                case Utility.ResultType.Failure:
                    UserSession.DisplayMessage(this, "No Data To Display .", MainMasterPage.MessageType.Warning);
                    break;

                case Utility.ResultType.Error:
                    UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                    break;
            }
        }

        #endregion

       
    }
}