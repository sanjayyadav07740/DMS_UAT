using DMS.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DMS.Shared
{
    public partial class ShareDocument : System.Web.UI.Page
    {

        Utility objUtility = new Utility();
        FolderManager objManager = new FolderManager();
        DocumentShareManager objDocumentShareManager = new DocumentShareManager();
        List<int> DocumentIDforCart = new List<int>();
        DataTable CartData = new DataTable();
        int DocumentID;
        string strPassword ;
        string Password;

        protected void Page_Load(object sender, EventArgs e)
        {
            divuseremail.Visible = false;
            divInternalUser.Visible = false;
            divExternaluser.Visible = false;

            if (!IsPostBack)
            {

                Utility.LoadAccessType(ddlaccesstype);
                Utility.LoadUserEmail(ddluserlist);
            }
            //calfromdate.Start = DateTime.Now;   //to dissable past Date

            ((TreeView)emodModule.FindControl("tvwFolder")).SelectedNodeChanged += new EventHandler(tvwFolder_SelectedNodeChanged);
        }

        public void BindAccessType()
        {

        }

        protected void gvwDocument_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.ToLower().Trim() == "documentsearch")
                {
                    int intRowIndex = Convert.ToInt32(e.CommandArgument);
                    UserSession.MetaDataID = Convert.ToInt32(gvwDocument.DataKeys[intRowIndex].Values["MetaDataID"]);
                    string strDocumentID = gvwDocument.DataKeys[intRowIndex].Values["ID"].ToString().Trim();
                    Session["DocId"] = strDocumentID;
                    string DocumentPath = gvwDocument.DataKeys[intRowIndex].Values["DocumentPath"].ToString().Trim();
                    string strStatus = gvwDocument.DataKeys[intRowIndex].Values["DocumentStatusID"].ToString().Trim();
                    switch (strStatus)
                    {
                        case "1":
                            Response.Redirect("../MetaData/ApprovedDocument_new.aspx?DOCID=" + strDocumentID, false);
                            break;

                        case "2":
                            //Response.Redirect("../MetaData/RejectedDocument.aspx?DOCID=" + strDocumentID, false);
                            UserSession.DisplayMessage(this, "Rejected Document.", MainMasterPage.MessageType.Warning);
                            break;

                        case "3":
                            Log.DocumentAuditLog(HttpContext.Current, "View Document", "SearchDocument", Convert.ToInt32(strDocumentID));
                            if (DocumentPath.EndsWith(".pdf"))
                                Response.Redirect("../MetaData/ViewDocumentForSearch.aspx?DOCID=" + strDocumentID, false);
                            else
                                Response.Redirect("../MetaData/DocumentViewer.aspx?DOCID=" + strDocumentID, false);
                            //Response.Redirect("../MetaData/DocumentEntry.aspx?DOCID=" + strDocumentID, false);

                            //if (rdblSearchBy.SelectedValue == "1" && ddlField.SelectedValue != "-1")
                            //{
                            //    Response.Redirect("../MetaData/DocumentVerification_new.aspx?DOCID=" + strDocumentID, false);
                            //}
                            //else
                            //    Response.Redirect("../MetaData/DocumentEntry.aspx?DOCID=" + strDocumentID, false);
                            break;

                        case "4":
                            Log.DocumentAuditLog(HttpContext.Current, "View Document", "SearchDocument", Convert.ToInt32(strDocumentID));
                            if (DocumentPath.EndsWith(".pdf"))
                            {
                                //Response.Redirect("../MetaData/ViewDocumentForSearch.aspx?DOCID=" + strDocumentID, false);
                                string url = "ViewDocumentForSearch.aspx?DOCID=" + strDocumentID;
                                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open('" + url + "','_blank')", true);

                            }
                            else
                                Response.Redirect("../MetaData/DocumentViewer.aspx?DOCID=" + strDocumentID, false);

                            //if(rdblSearchBy.SelectedValue=="1" && ddlField.SelectedValue != "-1")
                            //{
                            //    Response.Redirect("../MetaData/DocumentVerification_new.aspx?DOCID=" + strDocumentID, false);
                            //}
                            //else
                            //Response.Redirect("../MetaData/DocumentEntry.aspx?DOCID=" + strDocumentID, false);

                            //Response.Redirect("../MetaData/DocumentVerification.aspx?DOCID=" + strDocumentID, false);
                            break;
                    }

                }
                if (e.CommandName.ToLower().Trim() == "documentsearchimage")
                {

                    int intRowIndex = Convert.ToInt32(e.CommandArgument);
                    UserSession.MetaDataID = Convert.ToInt32(gvwDocument.DataKeys[intRowIndex].Values["MetaDataID"]);
                    string strDocumentID = gvwDocument.DataKeys[intRowIndex].Values["ID"].ToString().Trim();
                    string strStatus = gvwDocument.DataKeys[intRowIndex].Values["DocumentStatusID"].ToString().Trim();
                    Session["DocId"] = strDocumentID;

                    Response.Redirect("../MetaData/ViewDocument_ImageViewer.aspx?DOCID=" + strDocumentID, false);
                }
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        //protected void gvwDocument_RowDataBound(object sender, GridViewPageEventArgs e)
        //{
        //    try
        //    {
        //        gvwDocument.PageIndex = e.NewPageIndex;
        //        if (UserSession.GridData != null)
        //        {
        //            if (UserSession.FilterData == null)
        //                gvwDocument.DataSource = UserSession.GridData;
        //            else
        //                gvwDocument.DataSource = UserSession.FilterData;

        //            gvwDocument.DataBind();
        //        }
        //        else
        //        {
        //            DocumentBind();
        //        }
        //    }


        //    catch (Exception ex)
        //    {
        //        UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
        //        LogManager.ErrorLog(Utility.LogFilePath, ex);
        //    }
        //}


        protected void gvwDocument_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {

                Utility.SetGridHoverStyle(e);

                //if (((DMS.BusinessLogic.MetaData)UserSession.TemporaryList["METADATA"]).RepositoryID == 75 || ((DMS.BusinessLogic.MetaData)UserSession.TemporaryList["METADATA"]).RepositoryID == 76)
                //    if (emodModule.SelectedRepository == 75 || emodModule.SelectedRepository == 76)
                //    {
                //        if (e.Row.RowType == DataControlRowType.DataRow)
                //        {
                //            GridViewRow grv1 = gvwDocument.HeaderRow;
                //            CheckBox chkHeader = (CheckBox)grv1.FindControl("chkHeader");
                //            CheckBox chkChild = (e.Row.FindControl("chkChild") as CheckBox);
                //            chkHeader.Visible = true;
                //            chkChild.Visible = true;

                //        }
                //    }
                //    else
                //    {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    GridViewRow grv1 = gvwDocument.HeaderRow;
                    CheckBox chkHeader = (CheckBox)grv1.FindControl("chkHeader");
                    CheckBox chkChild = (e.Row.FindControl("chkChild") as CheckBox);
                    chkHeader.Visible = true;
                    chkChild.Visible = true;

                }
                //  }
                // }




                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    e.Row.Cells[0].ColumnSpan = 6;
                    for (int i = 1; i < 7; i++)
                    {
                        e.Row.Cells[i].Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        protected void tvwFolder_SelectedNodeChanged(object sender, EventArgs e)
        {
            try
            {
                DocumentBind();
                ibtnShow.Visible = true;
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }

        }

        protected void gvwDocument_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvwDocument.PageIndex = e.NewPageIndex;
                if (UserSession.GridData != null)
                {
                    if (UserSession.FilterData == null)
                        gvwDocument.DataSource = UserSession.GridData;
                    else
                        gvwDocument.DataSource = UserSession.FilterData;

                    gvwDocument.DataBind();
                }
                else
                {
                    DocumentBind();
                }
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        protected void gvwDocument_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                if (UserSession.GridData != null)
                {
                    if (ViewState[UserSession.SortExpression] == null || ViewState[UserSession.SortExpression].ToString() == "DESC")
                        ViewState[UserSession.SortExpression] = "ASC";
                    else
                        ViewState[UserSession.SortExpression] = "DESC";

                    if (UserSession.FilterData != null)
                        gvwDocument.DataSource = UserSession.SortedFilterGridData(e.SortExpression, ViewState[UserSession.SortExpression].ToString());
                    else
                        gvwDocument.DataSource = UserSession.SortedGridData(e.SortExpression, ViewState[UserSession.SortExpression].ToString());

                    gvwDocument.DataBind();
                }
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        public void DocumentBind()
        {
            int folderID = emodModule.SelectedFolder;
            DataTable dt = new DataTable();
            //objUtility.Result == Utility.ResultType.Success
            objUtility.Result = objManager.GetDocumentByFolderID(out dt, folderID);

            UserSession.GridData = null;
            UserSession.GridData = dt;
            gvwDocument.DataSource = dt;
            gvwDocument.DataBind();
        }

        protected void ibtnFilterGrid_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (UserSession.GridData != null)
                {
                    string strFilterBy = ((DropDownList)gvwDocument.FooterRow.FindControl("ddlFilterGrid")).SelectedValue.Trim();
                    string strFilterText = ((TextBox)gvwDocument.FooterRow.FindControl("txtFilterGrid")).Text.Trim();
                    if (strFilterText == string.Empty)
                    {
                        gvwDocument.DataSource = UserSession.GridData;
                        gvwDocument.DataBind();
                        UserSession.FilterData = null;
                    }
                    else
                    {
                        DataRow[] objRows = null;

                        if (strFilterBy == "1")
                            objRows = UserSession.GridData.Select("DocumentName LIKE '%" + strFilterText + "%'");
                        else if (strFilterBy == "2")
                            objRows = UserSession.GridData.Select("Tag LIKE '%" + strFilterText + "%'");

                        if (objRows.Length > 0)
                        {
                            UserSession.FilterData = objRows.CopyToDataTable();
                            gvwDocument.DataSource = UserSession.FilterData;
                            gvwDocument.DataBind();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
            }
        }

        public void DocumentBindForCart()
        {

            DataTable dt = new DataTable();
            DocumentIDforCart = (List<int>)Session["DocumentCartIDs"];

            dt = FolderManager.GetDocumentForCartByDocumentID(DocumentIDforCart);

            gvwcart.DataSource = dt;
            gvwcart.DataBind();
            IbtnSubmit.Visible = true;
        }



        protected void ibtnShow_Click(object sender, ImageClickEventArgs e)
        {

            int DocumentID;
            if ((List<int>)Session["DocumentCartIDs"] != null)
            {
                DocumentIDforCart = (List<int>)Session["DocumentCartIDs"];
            }

            try
            {
                foreach (GridViewRow row in gvwDocument.Rows)
                {
                    if ((row.FindControl("chkChild") as CheckBox).Checked)
                    {

                        DocumentID = (int)gvwDocument.DataKeys[row.RowIndex].Values["ID"];

                        DocumentIDforCart.Add(DocumentID);
                    }
                }
                Session["DocumentCartIDs"] = DocumentIDforCart;
                DocumentBindForCart();
            }
            catch (Exception ex)
            {

                throw;
            }





        }
        public string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }

        protected void rbtnsharewith_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (rbtnsharewith.SelectedValue == "InternalUser")
                {
                    divuseremail.Visible = true;
                    divInternalUser.Visible = true;
                    divExternaluser.Visible = false;
                }
                else if (rbtnsharewith.SelectedValue == "ExternalUser")
                {
                    divuseremail.Visible = true;
                    divInternalUser.Visible = false;
                    divExternaluser.Visible = true;
                }
                else
                {
                    divuseremail.Visible = false;
                    divInternalUser.Visible = false;
                    divExternaluser.Visible = false;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }

        protected void gvwcart_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        protected void IbtnSubmit_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                DMS.BusinessLogic.DocumentShare objDocumentShare = new DMS.BusinessLogic.DocumentShare();
                int DocumentID;
                int DocumentCount = gvwcart.Rows.Count;
                string WebLink = ConfigurationManager.AppSettings["SharedDocumentWebLink"];
                DataSet ds = new DataSet();
                if (rbtnsharewith.SelectedValue == "InternalUser")
                {
                    int UserID = int.Parse(ddluserlist.SelectedValue);
                    ds = DocumentShareManager.CheckInternalUser(UserID);
                }
                else if (rbtnsharewith.SelectedValue == "ExternalUser")
                {
                    string EmailID = txtemailId.Text;
                    ds = DocumentShareManager.CheckExternalUser(EmailID);
                   
                   
                }

                if (ds.Tables[0].Rows.Count > 0)
                {
                    strPassword = ds.Tables[0].Rows[0].Field<string>("Password");
                    Password = Utility.Decrypt(strPassword);
                    bool IsFolderSelected = chkboxselectedfolder.Checked;
                    if (IsFolderSelected == true)
                    {
                        objDocumentShare.FolderID = emodModule.SelectedFolder;
                        objDocumentShare.DocumentShareID = ds.Tables[0].Rows[0].Field<int>("ID");
                        objDocumentShare.FromDate = txtFrom.Text;
                        objDocumentShare.ToDate = txtend.Text;
                        objUtility.Result = objDocumentShareManager.InsertDocumentShareDetails(objDocumentShare);
                    }
                    else
                    {
                        foreach (GridViewRow row in gvwcart.Rows)
                        {

                            DocumentID = (int)gvwcart.DataKeys[row.RowIndex].Values["ID"];
                            objDocumentShare.Document_ID = DocumentID;
                            objDocumentShare.DocumentShareID = ds.Tables[0].Rows[0].Field<int>("ID");
                            objDocumentShare.FromDate = txtFrom.Text;
                            objDocumentShare.ToDate = txtend.Text;
                            objUtility.Result = objDocumentShareManager.InsertDocumentShareDetails(objDocumentShare);


                        }
                    }


                    if (objUtility.Result == Utility.ResultType.Success)
                    {
                        if (rbtnsharewith.SelectedValue == "ExternalUser")
                        {
                            if (IsFolderSelected == true)
                            {
                                SendMail("Dear User,<br /><br /> We have shared the Folders with you.<br /><br /><br/>This Folders are available to you from <strong> " + txtFrom.Text + "</strong> to <strong> " + txtend.Text + "</strong><br /><br />.Please Login into the DMS Application to view the Documents.<br /><br /><br /><br><br>Thank you,<br>Team DMS </strong> <br /><br /> <br /><br />   This is system generated mail <strong> DO NOT REPLY</strong>.", "Notification - Document Shared. ", "DMS Team", ddluserlist.SelectedItem.Text);
                            }
                            else
                            {
                                SendMail("Dear User,<br /><br /> We have shared the " + DocumentCount + " documents with you.<br /><br /><br/>This Documents are available to you from <strong> " + txtFrom.Text + "</strong> to <strong> " + txtend.Text + "</strong><br /><br />.Please find below Credentials to view the Documents.<br /><br /><br /> Please <STRONG> <a href='" + WebLink + "'><b> Click here</b> </a> </strong> </  <br /><br /> <br /><br />  Username - <strong> " + txtemailId.Text + " </strong> <br /><br /> Password - <strong> " + Utility.Decrypt(Password) + " </strong><br><br>Thank you,<br>Team DMS </strong> <br /><br /> <br /><br />   This is system generated mail <strong> DO NOT REPLY</strong>.", "Notification - Document Shared.", "DMS Team", Convert.ToString(txtemailId.Text));
                            }
                        }
                        else if (rbtnsharewith.SelectedValue == "InternalUser")
                        {
                            if (IsFolderSelected == true)
                            {
                                SendMail("Dear User,<br /><br /> We have shared the Folders with you.<br /><br /><br/>This Folders are available to you from <strong> " + txtFrom.Text + "</strong> to <strong> " + txtend.Text + "</strong><br /><br />.Please Login into the DMS Application to view the Documents.<br /><br /><br /><br><br>Thank you,<br>Team DMS </strong> <br /><br /> <br /><br />   This is system generated mail <strong> DO NOT REPLY</strong>.", "Notification - Document Shared. ", "DMS Team", ddluserlist.SelectedItem.Text);
                            }
                            else
                            {
                                SendMail("Dear User,<br /><br /> We have shared the " + DocumentCount + " documents with you.<br /><br /><br/>This Documents are available to you from <strong> " + txtFrom.Text + "</strong> to <strong> " + txtend.Text + "</strong><br /><br />.Please Login into the DMS Application to view the Documents.<br /><br /><br /><br><br>Thank you,<br>Team DMS </strong> <br /><br /> <br /><br />   This is system generated mail <strong> DO NOT REPLY</strong>.", "Notification - Document Shared. ", "DMS Team", ddluserlist.SelectedItem.Text);
                            }
                        }

                        UserSession.DisplayMessage(this, "Document Has been Shared Successfully. .", MainMasterPage.MessageType.Success);
                    }
                }
                else
                {
                    objDocumentShare.UserType = rbtnsharewith.SelectedValue;
                    objDocumentShare.UserID = int.Parse(ddluserlist.SelectedValue);
                    objDocumentShare.FromDate = txtFrom.Text;
                    objDocumentShare.ToDate = txtend.Text;
                    objDocumentShare.EmailID = txtemailId.Text;
                    strPassword = RandomString(6, true);
                    Password = Utility.Encrypt(strPassword);
                    objDocumentShare.Password = Password;
                    objDocumentShare.AccessType = int.Parse(ddlaccesstype.SelectedValue);
                    objDocumentShare.CreatedBy = UserSession.UserID;
                    objUtility.Result = objDocumentShareManager.InsertDocumentShare(objDocumentShare);
                    if (objUtility.Result == Utility.ResultType.Success)
                    {
                        bool IsFolderSelected = chkboxselectedfolder.Checked;
                        if (IsFolderSelected == true)
                        {
                            objDocumentShare.FolderID = emodModule.SelectedFolder;
                            objDocumentShare.DocumentShareID = objDocumentShare.DocumentShareID;
                            objUtility.Result = objDocumentShareManager.InsertDocumentShareDetails(objDocumentShare);
                        }
                        else
                        {
                            foreach (GridViewRow row in gvwcart.Rows)
                            {
                                DocumentID = (int)gvwcart.DataKeys[row.RowIndex].Values["ID"];
                                objDocumentShare.Document_ID = DocumentID;
                                objDocumentShare.DocumentShareID = objDocumentShare.DocumentShareID;
                                objUtility.Result = objDocumentShareManager.InsertDocumentShareDetails(objDocumentShare);


                            }
                        }


                        if (objUtility.Result == Utility.ResultType.Success)
                        {
                            if (rbtnsharewith.SelectedValue == "ExternalUser")
                            {
                                if (IsFolderSelected == true)
                                {
                                    SendMail("Dear User,<br /><br /> We have shared the Folders with you.<br /><br /><br/>This Folders are available to you from <strong> " + txtFrom.Text + "</strong> to <strong> " + txtend.Text + "</strong><br /><br />.Please Login into the DMS Application to view the Documents.<br /><br /><br /><br><br>Thank you,<br>Team DMS </strong> <br /><br /> <br /><br />   This is system generated mail <strong> DO NOT REPLY</strong>.", "Notification - Document Shared. ", "DMS Team", ddluserlist.SelectedItem.Text);
                                }
                                else
                                {
                                    SendMail("Dear User,<br /><br /> We have shared the " + DocumentCount + " documents with you.<br /><br /><br/>This Documents are available to you from <strong> " + txtFrom.Text + "</strong> to <strong> " + txtend.Text + "</strong><br /><br />.Please find below Credentials to view the Documents.<br /><br /><br /> Please <STRONG> <a href='" + WebLink + "'><b> Click here</b> </a> </strong> </  <br /><br /> <br /><br />  Username - <strong> " + txtemailId.Text + " </strong> <br /><br /> Password - <strong> " + Utility.Decrypt(Password) + " </strong><br><br>Thank you,<br>Team DMS </strong> <br /><br /> <br /><br />   This is system generated mail <strong> DO NOT REPLY</strong>.", "Notification - Document Shared.", "DMS Team", Convert.ToString(txtemailId.Text));
                                }
                            }
                            else if (rbtnsharewith.SelectedValue == "InternalUser")
                            {
                                if (IsFolderSelected == true)
                                {
                                    SendMail("Dear User,<br /><br /> We have shared the Folders with you.<br /><br /><br/>This Folders are available to you from <strong> " + txtFrom.Text + "</strong> to <strong> " + txtend.Text + "</strong><br /><br />.Please Login into the DMS Application to view the Documents.<br /><br /><br /><br><br>Thank you,<br>Team DMS </strong> <br /><br /> <br /><br />   This is system generated mail <strong> DO NOT REPLY</strong>.", "Notification - Document Shared. ", "DMS Team", ddluserlist.SelectedItem.Text);
                                }
                                else
                                {
                                    SendMail("Dear User,<br /><br /> We have shared the " + DocumentCount + " documents with you.<br /><br /><br/>This Documents are available to you from <strong> " + txtFrom.Text + "</strong> to <strong> " + txtend.Text + "</strong><br /><br />.Please Login into the DMS Application to view the Documents.<br /><br /><br /><br><br>Thank you,<br>Team DMS </strong> <br /><br /> <br /><br />   This is system generated mail <strong> DO NOT REPLY</strong>.", "Notification - Document Shared. ", "DMS Team", ddluserlist.SelectedItem.Text);
                                }
                            }



                            UserSession.DisplayMessage(this, "Document Has been Shared Successfully. .", MainMasterPage.MessageType.Success);
                        }
                    }

                }

            }
            catch (Exception ex)
            {

                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
            finally
            {
                Session["DocumentCartIDs"] = null;
            }


        }


        public bool SendMail(string body, string subject, string DisplayName, params string[] tosend)
        {
            try
            {
                MailMessage Mail = new MailMessage();
                MailAddress mailFrom = new MailAddress(ConfigurationManager.AppSettings["EmailIDFrom"].ToString(), DisplayName);
                Mail.From = mailFrom;
                foreach (string item in tosend)
                {
                    Mail.To.Add(item);
                }
                Mail.IsBodyHtml = true;
                Mail.Subject = subject;
                Mail.Body = body;
                SmtpClient client = new SmtpClient(ConfigurationManager.AppSettings["SmtpServer"].ToString(), Convert.ToInt32(ConfigurationManager.AppSettings["SmtpServerPort"]));
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["EmailIDFrom"].ToString(), ConfigurationManager.AppSettings["EmailIDFromCredentials"].ToString());
                client.EnableSsl = true;
                if (Bypasscertificate == 1)
                {
                    System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object s,
                System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                System.Security.Cryptography.X509Certificates.X509Chain chain,
                System.Net.Security.SslPolicyErrors sslPolicyErrors)
                    {
                        return true;
                    };
                }
                client.Send(Mail);
                return true;
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return false;
            }

        }

        public static int Bypasscertificate
        {
            get
            {
                return Convert.ToInt32(ConfigurationSettings.AppSettings["bypasscertificate"]);
            }
        }

        protected void chkboxselectedfolder_CheckedChanged(object sender, EventArgs e)
        {
            if (chkboxselectedfolder.Checked == true)
            {
                ibtnShow.Visible = false;
            }
            else if (chkboxselectedfolder.Checked == false)
            {
                ibtnShow.Visible = true;
            }
        }


    }
}