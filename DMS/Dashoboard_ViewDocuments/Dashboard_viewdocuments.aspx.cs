using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DMS.BusinessLogic;
using System.Data;

namespace DMS.Dashoboard_ViewDocuments
{
    public partial class Dashboard_viewdocuments : System.Web.UI.Page
    {
        
        #region Private Memeber
        Utility objUtility = new Utility();
        string SelectedFieldValue = "SelectedFieldValue";
        #endregion

        #region Properties
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
        #endregion
        DMS.BusinessLogic.Document objDocument = new DMS.BusinessLogic.Document();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGrid();
            }
        }

        protected void GrvView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrvView.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        protected void GrvView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            
        }

        protected void GrvView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "SHOWLOG")
                {
                    {
                        int intRowIndex = Convert.ToInt32(e.CommandArgument);
                        UserSession.MetaDataID = Convert.ToInt32(GrvView.DataKeys[intRowIndex].Values["METADATAID"]);
                        string strDocumentID = GrvView.DataKeys[intRowIndex].Values["ID"].ToString().Trim();
                        string strStatus = GrvView.DataKeys[intRowIndex].Values["DOCUMENTSTATUSID"].ToString().Trim();
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

        private void BindGrid()
        {
            DataTable dt = objDocument.MostViewedDocuments();
            if (dt.Rows.Count > 0)
            {
                GrvView.DataSource = dt;
                GrvView.DataBind();
            }
            else
            {
                GrvView.DataSource = null;
                GrvView.DataBind();
            }
        }
    }
}