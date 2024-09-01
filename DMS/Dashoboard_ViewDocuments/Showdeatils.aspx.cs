using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DMS.BusinessLogic;

namespace DMS.Dashoboard_ViewDocuments
{
    public partial class Showdeatils : System.Web.UI.Page
    {
        #region ObjectCreate
        Document objDocument = new Document();
        #endregion

        #region Event
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGrid();
                BindMostview();
                BindGridRecentAdd();
            }
        }

        #region MostViewDocument
        protected void GrvViewMostview_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrvViewMostview.PageIndex = e.NewPageIndex;
            BindMostview();
        }

        protected void GrvViewMostview_RowCommand1(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "SHOWLOG")
                {
                    {
                        int intRowIndex = Convert.ToInt32(e.CommandArgument);
                        UserSession.MetaDataID = Convert.ToInt32(GrvViewMostview.DataKeys[intRowIndex].Values["METADATAID"]);
                        string strDocumentID = GrvViewMostview.DataKeys[intRowIndex].Values["ID"].ToString().Trim();
                        string strStatus = GrvViewMostview.DataKeys[intRowIndex].Values["DOCUMENTSTATUSID"].ToString().Trim();
                        switch (strStatus)
                        {
                            case "1":
                                Response.Redirect("../MetaData/ApprovedDocument.aspx?DOCID=" + strDocumentID, false);
                                break;

                            case "2":
                                Response.Redirect("../MetaData/RejectedDocument.aspx?DOCID=" + strDocumentID, false);
                                break;

                            case "3":
                                Response.Redirect("../MetaData/DocumentEntry.aspx?DOCID=" + strDocumentID, false);
                                break;

                            case "4":
                                Response.Redirect("../MetaData/DocumentVerification.aspx?DOCID=" + strDocumentID, false);
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }
        #endregion

        #region CountDocument

        protected void GridCount_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridCount.PageIndex = e.NewPageIndex;
            BindGrid();

        }

      
        #endregion
       

        #region RecentlyAddedDocument

        protected void GridViewRecentAdd_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewRecentAdd.PageIndex = e.NewPageIndex;
            BindGridRecentAdd();
        }
            
        #endregion

        #endregion
       
        #region Method
        /// <summary>
        /// Date Wise Document to show in Gridview
        /// </summary>
        public void BindGrid()
        {
            DataTable dt = objDocument.DatewiseDocuments();
            if (dt.Rows.Count > 0)
            {
                GridCount.DataSource = dt;
                GridCount.DataBind();
            }
            else
            {
                GridCount.DataSource = null;
                GridCount.DataBind();
            }
        }

        /// <summary>
        /// bind data To gridview to show Most View only
        /// </summary>
        public void BindMostview()
        {
            DataTable dt = objDocument.MostViewedDocuments();
            if (dt.Rows.Count > 0)
            {
                GrvViewMostview.DataSource = dt;
                GrvViewMostview.DataBind();
            }
            else
            {
                GrvViewMostview.DataSource = null;
                GrvViewMostview.DataBind();
            }
        }

        /// <summary>
        /// bind Data to Grideview for Recently Added Documents
        /// </summary>
        public void BindGridRecentAdd()
        {

            DataTable dt = objDocument.RecentlyAddedDocuments();
            if (dt.Rows.Count > 0)
            {
                GridViewRecentAdd.DataSource = dt;
                GridViewRecentAdd.DataBind();
            }
            else
            {
                GridViewRecentAdd.DataSource = null;
                GridViewRecentAdd.DataBind();
            }

        }


        protected void btnShow_Click(object sender, EventArgs e)
        {
            if (txtFrom.Text != "" && txtTO.Text != "")
            {

                DataTable dt = objDocument.FilterDatewiseDocuments(txtFrom.Text, txtTO.Text);
                if (dt.Rows.Count > 0)
                {
                    GridCount.DataSource = dt;
                    GridCount.DataBind();
                }
                else
                {
                    GridCount.DataSource = null;
                    GridCount.DataBind();
                }

                txtFrom.Text = "";
                txtTO.Text = "";
            }
        }
        #endregion
    }
}