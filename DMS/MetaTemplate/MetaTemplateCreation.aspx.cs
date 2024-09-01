using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DMS.BusinessLogic;
using System.Data;

namespace DMS.MetaTemplate
{
    public partial class MetaTemplateCreation : System.Web.UI.Page
    {
        #region Private Members
        Utility objUtility = new Utility();
        MetaTemplateManager objMetaTemplateManager = new MetaTemplateManager();
        #endregion

        #region Events
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.Page.Form.DefaultButton = ibtnSave.UniqueID;
                if (!IsPostBack)
                {
                    Utility.LoadStatus(ddlStatus);
                    txtMetaTemplateName.Focus();
                    if (UserSession.IsCreateMetaTemplate == 0)
                    {
                        ClearControl();
                        lblTitle.Text = "Create MetaTemplate";
                    }
                    else
                    {
                        emodModule.Visible = false;
                        LoadControl();
                        lblTitle.Text = "Update MetaTemplate";
                    }
                    Log.AuditLog(HttpContext.Current, "Visit", "MetaTemplate Creation");
                }
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        protected void ibtnSave_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                DMS.BusinessLogic.MetaTemplate objMetaTemplate = new BusinessLogic.MetaTemplate();

                if (UserSession.IsCreateMetaTemplate == 0 || hdfMetaTemplateName.Value.Trim().ToUpper() != txtMetaTemplateName.Text.Trim().ToUpper())
                {
                    if (UserSession.IsCreateMetaTemplate == 0)
                    {
                        objUtility.Result = objMetaTemplateManager.SelectMetaTemplate(txtMetaTemplateName.Text.Trim(), Convert.ToInt32(emodModule.SelectedRepository));
                    }
                    else if (UserSession.IsCreateMetaTemplate != 0)
                    {
                        objUtility.Result = objMetaTemplateManager.SelectMetaTemplate(txtMetaTemplateName.Text.Trim(), Convert.ToInt32(hdfRepositoryID.Value));
                    }

                    switch (objUtility.Result)
                    {
                        case Utility.ResultType.Success:
                            UserSession.DisplayMessage(this, "MetaTemplate Name Already Exist .", MainMasterPage.MessageType.Warning);
                            return;
                            break;

                        case Utility.ResultType.Error:
                            UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                            return;
                            break;
                    }
                }

                if (UserSession.IsCreateMetaTemplate == 0)
                {
                    objMetaTemplate.MetaTemplateName = txtMetaTemplateName.Text.Trim();
                    objMetaTemplate.MetaTemplateDescription = txtMetaTemplateDescription.Text.Trim();
                    objMetaTemplate.RepositoryID = Convert.ToInt32(emodModule.SelectedRepository);
                    objMetaTemplate.CreatedBy = UserSession.UserID;
                    objMetaTemplate.Status = Convert.ToInt32(ddlStatus.SelectedItem.Value);
                    objUtility.Result = objMetaTemplateManager.InsertMetaTemplate(objMetaTemplate);
                    if (objMetaTemplate.RepositoryID == 82) //Capital Small Finance Bank Limited
                    {
                        objMetaTemplateManager.GetAccessToMetatemplate(objMetaTemplate, UserSession.RoleID,UserSession.UserID);

                    }
                }
                else if (UserSession.IsCreateMetaTemplate != 0)
                {
                    objMetaTemplate.MetaTemplateID = UserSession.IsCreateMetaTemplate;
                    objMetaTemplate.MetaTemplateName = txtMetaTemplateName.Text.Trim();
                    objMetaTemplate.MetaTemplateDescription = txtMetaTemplateDescription.Text.Trim();
                    objMetaTemplate.UpdatedBy = UserSession.UserID;
                    objMetaTemplate.Status = Convert.ToInt32(ddlStatus.SelectedItem.Value);
                    objUtility.Result = objMetaTemplateManager.UpdateMetaTemplate(objMetaTemplate);
                }

                switch (objUtility.Result)
                {
                    case Utility.ResultType.Success:
                        if (UserSession.IsCreateMetaTemplate == 0)
                        {
                            Log.AuditLog(HttpContext.Current, "MetaTemplateFields Created", "MetaTemplateView");
                            Response.Redirect("../MetaTemplate/MetaTemplateView.aspx?Type=0&ID=1", false);
                        }
                        else
                        {
                            Log.AuditLog(HttpContext.Current, "MetaTemplateFields Updated", "MetaTemplateView");
                            Response.Redirect("../MetaTemplate/MetaTemplateView.aspx?Type=1&ID=2", false);
                        }
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

        protected void ibtnCancel_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (UserSession.IsCreateMetaTemplate == 0)
                    Response.Redirect("../MetaTemplate/MetaTemplateView.aspx?ID=1", false);
                else
                    Response.Redirect("../MetaTemplate/MetaTemplateView.aspx?ID=2", false);
                //Response.Redirect("../MetaTemplate/MetaTemplateView.aspx?ID=2", false);
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }
        #endregion

        #region Methods
        public void ClearControl()
        {
            foreach (Control objControl in this.Page.Master.FindControl("cphMain").Controls)
            {
                if (objControl.GetType() == typeof(TextBox))
                {
                    ((TextBox)objControl).Text = "";
                }
                if (objControl.GetType() == typeof(CheckBox))
                {
                    ((CheckBox)objControl).Checked = false;
                }
                if (objControl.GetType() == typeof(RadioButton))
                {
                    ((RadioButton)objControl).Checked = false;
                }
            }
        }

        public void LoadControl()
        {
            try
            {
                DataTable objDataTable = new DataTable();
                DMS.BusinessLogic.MetaTemplateManager objMetaTemplateManager = new MetaTemplateManager();
                objUtility.Result = objMetaTemplateManager.SelectMetaTemplate(out objDataTable, UserSession.IsCreateMetaTemplate);

                switch (objUtility.Result)
                {
                    case Utility.ResultType.Success:
                        txtMetaTemplateName.Text = objDataTable.Rows[0]["MetaTemplateName"].ToString();
                        txtMetaTemplateDescription.Text = objDataTable.Rows[0]["MetaTemplateDescription"].ToString();
                        ddlStatus.SelectedIndex = ddlStatus.Items.IndexOf(ddlStatus.Items.FindByValue(objDataTable.Rows[0]["Status"].ToString()));
                        hdfMetaTemplateName.Value = txtMetaTemplateName.Text.Trim();
                        hdfRepositoryID.Value = objDataTable.Rows[0]["RepositoryID"].ToString();
                        break;
                    case Utility.ResultType.Failure:
                        UserSession.DisplayMessage(this, "Sorry ,No Data Is Available For This MetaTemplate .", MainMasterPage.MessageType.Error);
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