using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DMS.BusinessLogic;
using System.Data;
using System.Configuration;
namespace DMS.Repository
{
    public partial class RepositoryCreation : System.Web.UI.Page
    {
        #region Private Member
        Utility objUtility = new Utility();
        RepositoryManager objRepositoryManager = new RepositoryManager();
        #endregion

        #region Event
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Page.Form.DefaultButton = ibtnSubmit.UniqueID;
            try
            {
                if (!IsPostBack)
                {
                    Utility.LoadStatus(ddlStatus);
                    txtRepositoryName.Focus();
                    if (UserSession.IsCreateRepository == 0)
                    {
                        ClearControl();
                        lblTitle.Text = "Create Repository";
                    }
                    else if (UserSession.IsCreateRepository != 0)
                    {
                        LoadControl(UserSession.IsCreateRepository);
                        lblTitle.Text = "Update Repository";
                    }
                    Log.AuditLog(HttpContext.Current, "Visit", "Repository Creation");
                }
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        protected void ibtnSubmit_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                DMS.BusinessLogic.Repository objRepository = new DMS.BusinessLogic.Repository();

                if (UserSession.IsCreateRepository == 0 || hdfRepositoryName.Value.Trim().ToUpper() != txtRepositoryName.Text.Trim().ToUpper())
                {
                    objUtility.Result = objRepositoryManager.SelectRepository(txtRepositoryName.Text.Trim());
                    switch (objUtility.Result)
                    {
                        case Utility.ResultType.Success:
                            UserSession.DisplayMessage(this, "Repository Name Is Already Exist .", MainMasterPage.MessageType.Warning);
                            return;
                            break;

                        case Utility.ResultType.Error:
                             UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                             return;
                            break;
                    }
                }

                if (UserSession.IsCreateRepository == 0)
                {
                    objRepository.RepositoryName = txtRepositoryName.Text.Trim();
                    objRepository.RepositoryDescription = txtRepositoryDescription.Text.Trim();
                    objRepository.CreatedBy = UserSession.UserID;
                    objRepository.Status = Convert.ToInt32(ddlStatus.SelectedItem.Value);
                    Session["NewRepName"] = objRepository.RepositoryName;
                    objUtility.Result = objRepositoryManager.InsertRepository(objRepository);
                }
                else if (UserSession.IsCreateRepository != 0)
                {
                    objRepository.RepositoryID = UserSession.IsCreateRepository;
                    objRepository.RepositoryName = txtRepositoryName.Text.Trim();
                    objRepository.RepositoryDescription = txtRepositoryDescription.Text.Trim();
                    objRepository.UpdatedBy = UserSession.UserID;
                    objRepository.Status = Convert.ToInt32(ddlStatus.SelectedItem.Value);
                    objUtility.Result = objRepositoryManager.UpdateRepository(objRepository);
                }

                switch (objUtility.Result)
                {
                    case Utility.ResultType.Success:
                        if (UserSession.IsCreateRepository == 0)
                        {
                            Response.Redirect("../Repository/RepositoryView.aspx?Type=0&ID=1", false);
                            MailSent();
                        }
                        else
                            Response.Redirect("../Repository/RepositoryView.aspx?Type=1&ID=2", false);
                        
                        break;

                    case Utility.ResultType.Error:
                        UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                        break;
                }
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        protected void MailSent()
        {
            //DataSet ds = new DataSet();
            //ds = RepositoryManager.SelectNewlyCreatedRep();
            System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
            System.Net.Mail.SmtpClient SmtpServer = new System.Net.Mail.SmtpClient(ConfigurationManager.AppSettings["SmtpServer"].ToString(), Convert.ToInt32(ConfigurationManager.AppSettings["SmtpServerPort"]));
            mail.To.Add("rmsit@stockholdingdms.com");
            mail.IsBodyHtml = true;
            mail.From = new System.Net.Mail.MailAddress("sneha.bakshi@stockholdingdms.com");
            // mail.Bcc.Add(ConfigurationManager.AppSettings["EmailIDFrom"].ToString());
            mail.CC.Add("sneha.bakshi@stockholdingdms.com");
            mail.Subject = "New Repository Created in EDMS";
            mail.Body = getHtml(Session["NewRepName"].ToString());
            SmtpServer.UseDefaultCredentials = false;
            SmtpServer.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["EmailIDFrom"].ToString(), ConfigurationManager.AppSettings["EmailIDFromCredentials"].ToString());
            SmtpServer.EnableSsl = true;
            SmtpServer.Send(mail);
        }

        public static string getHtml(string RepName)
        {
            try
            {
                string messageBody = "<font>The New Repository Created is </font><br><br>";

                // messageBody = messageBody + "<table style=\"border-collapse:collapse; text-align:center;\"><tr style =\"background-color:#6FA1D2; color:#ffffff;\"><td style=\" border-color:#5c87b2; border-style:solid; border-width:thin; padding: 5px;\">MR NO</td><td style=\" border-color:#5c87b2; border-style:solid; border-width:thin; padding: 5px;\">Transaction ID</td><td style=\" border-color:#5c87b2; border-style:solid; border-width:thin; padding: 5px;\">Transaction Date</td><td style=\" border-color:#5c87b2; border-style:solid; border-width:thin; padding: 5px;\">Amount</td></tr>";
                //foreach (DataRow Row in dataSet.Tables[0].Rows)
                //{
                    messageBody = messageBody + RepName;
                    //"<tr style =\"color:#555555;\"><td style=\" border-color:#5c87b2; border-style:solid; border-width:thin; padding: 5px;\">" + Row["mr_no"].ToString() + "</td><td style=\" border-color:#5c87b2; border-style:solid; border-width:thin; padding: 5px;\">" + Row["transaction_id"].ToString() + "</td><td style=\" border-color:#5c87b2; border-style:solid; border-width:thin; padding: 5px;\">" + Row["transaction_date"].ToString() + "</td><td style=\" border-color:#5c87b2; border-style:solid; border-width:thin; padding: 5px;\">" + Convert.ToInt32(ConfigurationManager.AppSettings["amount"].ToString()) + "</td></tr>";

               // }
                messageBody += "<br/><br/> Thank You,<br/>Sneha Bakshi";
                messageBody = messageBody + "<br /><br />*** This is an automatically generated email, please do not reply *** ";
                return messageBody;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        protected void ibtnBack_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (UserSession.IsCreateRepository == 0)
                    Response.Redirect("../Repository/RepositoryView.aspx?ID=1", false);
                else
                    Response.Redirect("../Repository/RepositoryView.aspx?ID=2", false);
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }
        #endregion

        #region Method
        private void ClearControl()
        {
            txtRepositoryName.Text = string.Empty;
            txtRepositoryDescription.Text = string.Empty;
            txtRepositoryName.Focus();
        }

        private void LoadControl(int intRepositoryID)
        {
            try
            {
                DataTable objDataTable = new DataTable();
                objUtility.Result = objRepositoryManager.SelectRepository(out objDataTable, intRepositoryID);

                switch (objUtility.Result)
                {
                    case Utility.ResultType.Success:
                        txtRepositoryName.Text = objDataTable.Rows[0]["RepositoryName"].ToString().Trim();
                        txtRepositoryDescription.Text = objDataTable.Rows[0]["RepositoryDescription"].ToString().Trim();
                        ddlStatus.SelectedIndex = ddlStatus.Items.IndexOf(ddlStatus.Items.FindByValue(objDataTable.Rows[0]["Status"].ToString()));
                        hdfRepositoryName.Value = txtRepositoryName.Text;
                        break;
                    case Utility.ResultType.Failure:
                        UserSession.DisplayMessage(this, "Sorry ,No Data Is Available For This Repository .", MainMasterPage.MessageType.Error);
                        break;
                    case Utility.ResultType.Error:
                        UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                        break;
                }
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
            }
        }
        #endregion 

        
    }
}