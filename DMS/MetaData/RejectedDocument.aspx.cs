using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using PdfViewer;
using AjaxControlToolkit;
using System.Data;
using System.Data.Common;
using System.Web.UI.HtmlControls;
using DMS.BusinessLogic;

namespace DMS.Shared
{
    public partial class RejectedDocument : System.Web.UI.Page
    {
        #region Private Member
        BusinessLogic.Utility objUtility = new BusinessLogic.Utility();
        BusinessLogic.DocumentManager objDocumentManager = new BusinessLogic.DocumentManager();
        BusinessLogic.MetaTemplateManager objMetaTemplateManager = new BusinessLogic.MetaTemplateManager();
        #endregion

        #region Event
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //DataTable dt2 = (DataTable)Session["sampletable"];
                //gvwDocumentVersion.DataSource = dt2;
                //gvwDocumentVersion.DataBind();
                if (Request.UrlReferrer != null)
                {
                    ViewState["LASTPAGEURL"] = Request.UrlReferrer.ToString().ToLower();
                }
            }
        }

        protected void ibtnBack_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (ViewState["LASTPAGEURL"] != null)
                {
                    if (ViewState["LASTPAGEURL"].ToString().Contains("documententry"))
                    {
                        Response.Redirect("../MetaData/ViewForDocumentEntry.aspx", false);
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
                }

            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(BusinessLogic.Utility.LogFilePath, ex);
            }
        }
        #endregion

        protected void gvwDocumentVersion_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        protected void gvwDocumentVersion_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void gvwDocumentVersion_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }
    }
}