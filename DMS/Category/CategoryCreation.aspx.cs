using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DMS.BusinessLogic;
using System.Data;
using System.Text;

namespace DMS.Category
{
    public partial class CategoryCreation : System.Web.UI.Page
    {
        #region Private Member
        Utility objUtility = new Utility();
        CategoryManager objCategoryManager = new CategoryManager();
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
                    txtCategoryName.Focus();

                    if (UserSession.IsCreateCategory == 0)
                    {
                        ClearControl();
                        lblTitle.Text = "Create Category";
                    }
                    else if (UserSession.IsCreateCategory != 0)
                    {
                        LoadControl(UserSession.IsCreateCategory);
                        lblTitle.Text = "Update Category";
                    }
                    Log.AuditLog(HttpContext.Current, "Visit", "Category Creation");
                }
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        protected void ibtnSubmit_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                DMS.BusinessLogic.Category objCategory = new BusinessLogic.Category();

                if (UserSession.IsCreateCategory == 0 || hdfCategoryName.Value.Trim().ToUpper() != txtCategoryName.Text.Trim().ToUpper())
                {
                    if (UserSession.IsCreateCategory == 0)
                    {
                        objUtility.Result = objCategoryManager.SelectCategory(txtCategoryName.Text.Trim(), emodModule.SelectedMetaTemplate);
                    }
                    else if (UserSession.IsCreateCategory != 0)
                    {
                        objUtility.Result = objCategoryManager.SelectCategory(txtCategoryName.Text.Trim(), Convert.ToInt32(hdfMetaTemplateID.Value));
                    }

                    switch (objUtility.Result)
                    {
                        case Utility.ResultType.Success:
                            UserSession.DisplayMessage(this, "Category Name Is Already Exist .", MainMasterPage.MessageType.Warning);
                            return;
                            break;

                        case Utility.ResultType.Error:
                            UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                            return;
                            break;
                    }
                }

                if (UserSession.IsCreateCategory == 0)
                {

                    objCategory.CategoryName = txtCategoryName.Text.Trim();
                    objCategory.CategoryDescription = txtCategoryDescription.Text.Trim();
                    objCategory.RepositoryID = emodModule.SelectedRepository;
                    objCategory.MetaTemplateID = emodModule.SelectedMetaTemplate;
                    objCategory.CreatedBy = UserSession.UserID;
                    objCategory.Status = Convert.ToInt32(ddlStatus.SelectedItem.Value);

                    objUtility.Result = objCategoryManager.InsertCategory(objCategory);
                    if(objCategory.RepositoryID == 82)
                    {
                        objCategoryManager.GetAccesstoCategory(objCategory, UserSession.RoleID, UserSession.UserID);
                    }
                }
                else if (UserSession.IsCreateCategory != 0)
                {
                    objCategory.CategoryID = UserSession.IsCreateCategory;
                    objCategory.CategoryName = txtCategoryName.Text.Trim();
                    objCategory.CategoryDescription = txtCategoryDescription.Text.Trim();
                    objCategory.UpdatedBy = UserSession.UserID;
                    objCategory.Status = Convert.ToInt32(ddlStatus.SelectedItem.Value);

                    objUtility.Result = objCategoryManager.UpdateCategory(objCategory);
                }

                switch (objUtility.Result)
                {
                    case Utility.ResultType.Success:
                        if (UserSession.IsCreateCategory == 0)
                            Response.Redirect("../Category/CategoryView.aspx?Type=0&ID=1", false);
                        else
                            Response.Redirect("../Category/CategoryView.aspx?Type=1&ID=2", false);
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

        protected void ibtnBack_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (UserSession.IsCreateCategory == 0)
                    Response.Redirect("../Category/CategoryView.aspx?ID=1", false);
                else
                    Response.Redirect("../Category/CategoryView.aspx?ID=2", false);
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
            txtCategoryName.Text = string.Empty;
            txtCategoryDescription.Text = string.Empty;
            txtCategoryName.Focus();
        }

        private void LoadControl(int intCategoryID)
        {
            try
            {
                emodModule.Visible = false;

                DataTable objDataTable = new DataTable();
                objUtility.Result = objCategoryManager.SelectCategoryByCategoryID(out objDataTable, intCategoryID);
                
                switch (objUtility.Result)
                {
                    case Utility.ResultType.Success:
                        txtCategoryName.Text = objDataTable.Rows[0]["CategoryName"].ToString().Trim();
                        txtCategoryDescription.Text = objDataTable.Rows[0]["CategoryDescription"].ToString().Trim();
                        ddlStatus.SelectedIndex = ddlStatus.Items.IndexOf(ddlStatus.Items.FindByValue(objDataTable.Rows[0]["Status"].ToString()));
                        hdfCategoryName.Value = txtCategoryName.Text.Trim();
                        hdfMetaTemplateID.Value = objDataTable.Rows[0]["MetaTemplateID"].ToString().Trim();
                        break;
                    case Utility.ResultType.Failure:
                        UserSession.DisplayMessage(this, "Sorry ,No Data Is Available For This Category .", MainMasterPage.MessageType.Error);
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