using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DMS.BusinessLogic;
using System.Data;
using System.Collections;

namespace DMS.MetaTemplate
{
    public partial class MetaTemplateFieldCreation : System.Web.UI.Page
    {
        #region Private Member
        Utility objUtility = new Utility();
        MetaTemplateManager objMetaTemplateManager = new MetaTemplateManager();
        MetaTemplateFields objMetaTemplateFields = new MetaTemplateFields();
        public DataTable objMetaTemplateFielsTemp;
        public bool boolUpdateFlag;
        #endregion

        #region Events
        protected void Page_Load(object sender, EventArgs e)
        {
            //lblTitle.Text = "MetaTemplate Field Creation";
            //lblTitle.ForeColor = System.Drawing.Color.Black;
            if (!IsPostBack)
            {
                Utility.LoadStatus(ddlStatus);
                Utility.LoadFieldType(ddlFieldType);
                Utility.LoadDataType(ddlFieldDataType);                
                objUtility.Result = Utility.LoadRepository(ddlRepository);
                ddlMetaTemplate.Items.Insert(0, new ListItem("--SELECT--", "-1"));

                objMetaTemplateFielsTemp = new DataTable();
                DataColumn objDataColumn = new DataColumn("ID");
                objDataColumn.AutoIncrement = true;
                objDataColumn.AutoIncrementSeed = 1;
                objDataColumn.AutoIncrementStep = 1;

                objMetaTemplateFielsTemp.Columns.Add(objDataColumn);
                objMetaTemplateFielsTemp.Columns.Add("FieldName");
                objMetaTemplateFielsTemp.Columns.Add("FieldDataTypeID");
                objMetaTemplateFielsTemp.Columns.Add("FieldTypeID");
                objMetaTemplateFielsTemp.Columns.Add("FieldLength");
                objMetaTemplateFielsTemp.Columns.Add("IsPrimary");
                objMetaTemplateFielsTemp.Columns.Add("RepositoryID");
                objMetaTemplateFielsTemp.Columns.Add("MetaTemplateID");
                objMetaTemplateFielsTemp.Columns.Add("CreatedOn");
                objMetaTemplateFielsTemp.Columns.Add("CreatedBy");
                objMetaTemplateFielsTemp.Columns.Add("UpdatedOn");
                objMetaTemplateFielsTemp.Columns.Add("UpdatedBy");
                objMetaTemplateFielsTemp.Columns.Add("Status");               
                objMetaTemplateFielsTemp.AcceptChanges();
                UserSession.MetaTemplateField = objMetaTemplateFielsTemp;

                LoadGridData(UserSession.MetatemplateID);
                boolUpdateFlag = false;
                switch (objUtility.Result)
                {
                    case Utility.ResultType.Failure:
                        //UserSession.DisplayMessage(this, "No Fields To Display .", MainMasterPage.MessageType.Warning);
                        break;

                    case Utility.ResultType.Error:
                        UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                        break;
                }                
            }
            Log.AuditLog(HttpContext.Current, "Visit", "MetaTemplate FieldCreation");
        }        

        protected void ddlRepository_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                objUtility.Result = Utility.LoadMetaTemplate(ddlMetaTemplate, Convert.ToInt32(ddlRepository.SelectedItem.Value));
                UserSession.MetaTemplateField.Rows.Clear();
                gdvMetaTemplateFields.DataSource = UserSession.MetaTemplateField;
                gdvMetaTemplateFields.DataBind();

                switch (objUtility.Result)
                {
                    case Utility.ResultType.Failure:
                        UserSession.DisplayMessage(this, "No MetaTemplate To Display .", MainMasterPage.MessageType.Warning);
                        break;

                    case Utility.ResultType.Error:
                        UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                        break;
                }
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }       

        protected void ibtnCancel_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                Response.Redirect("", false);
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        protected void ibtnAdd_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if(UserSession.IsCreateMetaTemplateField==0)
                {
                    if (txtFieldName.Text == string.Empty)
                    {
                        UserSession.DisplayMessage(this, "Please Enter Field Name", MainMasterPage.MessageType.Warning);
                        txtFieldName.Focus();
                        return;
                    }
                    else if (ddlFieldDataType.SelectedItem.Value=="-1")
                    {
                        UserSession.DisplayMessage(this, "Please Select Data Type", MainMasterPage.MessageType.Warning);
                        ddlFieldDataType.Focus();
                        return;
                    }
                    else if (txtFieldLength.Text == string.Empty)
                    {
                        UserSession.DisplayMessage(this, "Please Enter Field Size", MainMasterPage.MessageType.Warning);
                        txtFieldLength.Focus();
                        return;
                    }
                    DataTable objDataTable = UserSession.MetaTemplateField;
                    if (objDataTable != null)
                    {
                        bool boolIsColumnExist = IsColumnExist(txtFieldName.Text.Trim());                        
                        if (!boolIsColumnExist)
                        {
                            if (chkIsPrimary.Checked == false)
                            {
                                objMetaTemplateFields.IsPrimary = 0;
                                //UserSession.DisplayMessage(this, "", MainMasterPage.MessageType.Error);
                            }
                            else
                            {                               
                                bool IsPrimaryKey = IsPrimaryKeyExist(UserSession.MetaTemplateField);
                                if (!IsPrimaryKey)
                                {                                  
                                    objMetaTemplateFields.IsPrimary = 1;
                                    ddlStatus.SelectedValue = "1";
                                }
                                else
                                {
                                    //chkIsPrimary.Checked = false;
                                    objMetaTemplateFields.IsPrimary = 0;
                                    //lblError.Text = "Primary Key Is Already Set...";
                                    UserSession.DisplayMessage(this, "Primary Key Is Already Set...", MainMasterPage.MessageType.Warning);
                                    return;
                                }
                            }
                            //objDataTable.Rows.Add(null, txtFieldName.Text, Convert.ToInt32(ddlFieldDataType.SelectedValue), Convert.ToInt32(ddlFieldType.SelectedValue), Convert.ToInt32(txtFieldLength.Text), objMetaTemplateFields.IsPrimary, Convert.ToInt32(ddlRepository.SelectedValue), Convert.ToInt32(ddlMetaTemplate.SelectedValue), null, UserSession.UserID, null, UserSession.UserID, Convert.ToInt32(ddlStatus.SelectedValue));
                            objDataTable.Rows.Add(null, txtFieldName.Text, Convert.ToInt32(ddlFieldDataType.SelectedValue), Convert.ToInt32(ddlFieldType.SelectedValue), Convert.ToInt32(txtFieldLength.Text), objMetaTemplateFields.IsPrimary, UserSession.RepositoryID, UserSession.MetatemplateID, null, UserSession.UserID, null, UserSession.UserID, Convert.ToInt32(ddlStatus.SelectedValue));
                            objDataTable.AcceptChanges();
                            UserSession.MetaTemplateField = objDataTable;
                            gdvMetaTemplateFields.DataSource = objDataTable;
                            gdvMetaTemplateFields.DataBind();

                            ibtnSave.Visible = true;
                            ibtnCancelAll.Visible = true;
                            //GroupGridView(gdvMetaTemplateFields.Rows, 2, 1);  
                            //ddlRepository.SelectedIndex = -1;
                            //ddlMetaTemplate.SelectedIndex = -1;
                            ddlFieldDataType.SelectedIndex = -1;
                            ddlFieldType.SelectedIndex = 0;
                            ddlStatus.SelectedIndex = 0;
                            txtFieldLength.Text = "";
                            txtFieldName.Text = "";
                            chkIsPrimary.Checked = false;

                            UserSession.DisplayMessage(this, "MetaTemplate Field Added....", MainMasterPage.MessageType.Success);
                        }
                        else
                        {
                            UserSession.DisplayMessage(this, "Column Name Already Exists...", MainMasterPage.MessageType.Warning);
                        }
                    }
                }
                else
                {                
                
                }
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }
        #endregion        

        protected void ibtnSave_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                bool IsAddedOrUpdate = false;
                //bool IsPrimaryKey = IsPrimaryKeyExist(UserSession.MetaTemplateField);
                //if (IsPrimaryKey)
                //{
                    for (int i = 0; i <= UserSession.MetaTemplateField.Rows.Count - 1; i++)
                    {
                        //DataTable objDataTable = (DataTable)ViewState["myTable"];
                       
                        if (UserSession.MetaTemplateField.Rows[i]["ID"].ToString() == "")
                        {
                            objMetaTemplateFields.FieldName = UserSession.MetaTemplateField.Rows[i]["FieldName"].ToString();//txtFieldName.Text.Trim();
                            objMetaTemplateFields.FieldDataTypeID = Convert.ToInt32(UserSession.MetaTemplateField.Rows[i]["FieldDataTypeID"].ToString());//Convert.ToInt32(ddlFieldDataType.SelectedItem.Value);
                            objMetaTemplateFields.FieldTypeID = Convert.ToInt32(UserSession.MetaTemplateField.Rows[i]["FieldTypeID"].ToString());//Convert.ToInt32(ddlFieldType.SelectedItem.Value);
                            objMetaTemplateFields.FieldLength = Convert.ToInt32(UserSession.MetaTemplateField.Rows[i]["FieldLength"].ToString());//Convert.ToInt32(txtFieldLength.Text.Trim());
                            objMetaTemplateFields.RepositoryID = Convert.ToInt32(UserSession.MetaTemplateField.Rows[i]["RepositoryID"].ToString());//Convert.ToInt32(ddlRepository.SelectedItem.Value);
                            objMetaTemplateFields.MetaTemplateID = Convert.ToInt32(UserSession.MetaTemplateField.Rows[i]["MetaTemplateID"].ToString());//Convert.ToInt32(ddlMetaTemplate.SelectedItem.Value);
                            objMetaTemplateFields.CreatedBy = UserSession.UserID;
                            objMetaTemplateFields.Status = Convert.ToInt32(UserSession.MetaTemplateField.Rows[i]["Status"].ToString());//Convert.ToInt32(ddlStatus.SelectedItem.Value);
                            objMetaTemplateFields.IsPrimary = Convert.ToInt32(UserSession.MetaTemplateField.Rows[i]["IsPrimary"].ToString());
                            objUtility.Result = objMetaTemplateManager.InsertMetaTemplateFields(objMetaTemplateFields);

                            IsAddedOrUpdate = true;
                        }
                        else if (UserSession.MetaTemplateField.Rows[i]["EditFlag"].ToString() == "1")
                        {
                            objMetaTemplateFields.FieldName = UserSession.MetaTemplateField.Rows[i]["FieldName"].ToString();//txtFieldName.Text.Trim();
                            objMetaTemplateFields.FieldDataTypeID = Convert.ToInt32(UserSession.MetaTemplateField.Rows[i]["FieldDataTypeID"].ToString());//Convert.ToInt32(ddlFieldDataType.SelectedItem.Value);
                            objMetaTemplateFields.FieldTypeID = Convert.ToInt32(UserSession.MetaTemplateField.Rows[i]["FieldTypeID"].ToString());//Convert.ToInt32(ddlFieldType.SelectedItem.Value);
                            objMetaTemplateFields.FieldLength = Convert.ToInt32(UserSession.MetaTemplateField.Rows[i]["FieldLength"].ToString());//Convert.ToInt32(txtFieldLength.Text.Trim());
                            //objMetaTemplateFields.RepositoryID = Convert.ToInt32(UserSession.MetaTemplateField.Rows[i]["RepositoryID"].ToString());//Convert.ToInt32(ddlRepository.SelectedItem.Value);
                            //objMetaTemplateFields.MetaTemplateID = Convert.ToInt32(UserSession.MetaTemplateField.Rows[i]["MetaTemplateID"].ToString());//Convert.ToInt32(ddlMetaTemplate.SelectedItem.Value);
                            objMetaTemplateFields.UpdatedBy = UserSession.UserID;
                            objMetaTemplateFields.Status = Convert.ToInt32(UserSession.MetaTemplateField.Rows[i]["Status"].ToString());//Convert.ToInt32(ddlStatus.SelectedItem.Value);
                            objMetaTemplateFields.IsPrimary = Convert.ToInt32(UserSession.MetaTemplateField.Rows[i]["IsPrimary"].ToString());
                            objMetaTemplateFields.MetaTemplateFieldsID = Convert.ToInt32(UserSession.MetaTemplateField.Rows[i]["ID"].ToString());
                            objUtility.Result = objMetaTemplateManager.UpdateMetaTemplateFields(objMetaTemplateFields);
                            boolUpdateFlag = true;

                            IsAddedOrUpdate = true;
                        }
                    }
                    if (IsAddedOrUpdate == false)
                    {
                        return;
                    }
                    
                    //UserSession.DisplayMessage(this, "MetaTemplate Field Info Submitted Successfully", MainMasterPage.MessageType.Success);
                    UserSession.MetaTemplateField = null;
                    gdvMetaTemplateFields.DataSource = UserSession.MetaTemplateField;
                    gdvMetaTemplateFields.DataBind();

                    if (boolUpdateFlag == true)
                    {
                        Response.Redirect("../MetaTemplate/MetaTemplateView.aspx?Type=1&ID=2", false);
                    }
                    else
                    {
                        Response.Redirect("../MetaTemplate/MetaTemplateView.aspx?Type=0&ID=2", false);
                    }
                //}
                //else
                //{                   
                //    UserSession.DisplayMessage(this, "Primary Key Is Not Set...", MainMasterPage.MessageType.Warning);
                //}
            }
            catch(Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        protected void ibtnCancelAll_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("../MetaTemplate/MetaTemplateView.aspx?ID=2", false);           
        }
        protected void chkIsPrimary_CheckedChanged(object sender, EventArgs e)
        {
            if (chkIsPrimary.Checked==true)
            {                           
                bool IsPrimaryKey= IsPrimaryKeyExist(UserSession.MetaTemplateField);
                if (!IsPrimaryKey)
                {                   
                    ddlFieldType.SelectedIndex = 1;
                }
                else
                {
                    UserSession.DisplayMessage(this, "Primary Key Is Already Set...", MainMasterPage.MessageType.Error);                    
                }
            }
            else
            {
                
            }
        }

        protected void ddlFieldType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (chkIsPrimary.Checked == true)
            {
                ddlFieldType.SelectedIndex = 1;
            }
            //else
            //{
            //    ddlFieldType.SelectedIndex = 0;
            //}
        }

        #region Methods
        private static bool IsColumnExist(string strColumnName)
        {            
            try
            {
                foreach(DataRow  objDataRow in UserSession.MetaTemplateField.Rows)
                {
                    if (objDataRow["FieldName"].ToString() == strColumnName)
                        {
                            return true;
                        }                        
                }
                return false;
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath,ex);
                return false;
            }
        }
        private static bool IsPrimaryKeyExist(DataTable objDataTable)
        {
            try
            {
                foreach (DataRow objDataRow in objDataTable.Rows)
                {
                    if (objDataRow["IsPrimary"].ToString() == "1")
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return false;
            }
        }
        public void clearControls()
        {
            foreach (Control objControl in this.Page.Master.FindControl("cphMain").Controls)
            {
                if (objControl.GetType() == typeof(TextBox))
                {
                    ((TextBox)objControl).Text = "";
                }
                if (objControl.GetType() == typeof(DropDownList))
                {
                    ((DropDownList)objControl).SelectedIndex = 0;
                }
                if (objControl.GetType() == typeof(CheckBox))
                {
                    ((CheckBox)objControl).Checked = false;
                }
            }
        }
        #endregion

        protected void ddlMetaTemplate_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                UserSession.MetaTemplateField.Rows.Clear();
                gdvMetaTemplateFields.DataSource = UserSession.MetaTemplateField;
                gdvMetaTemplateFields.DataBind();              
            }
            catch(Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath,ex);
            }
        }

        protected void gdvMetaTemplateFields_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //int intRowIndex = Convert.ToInt32(e.CommandArgument);
            //UserSession.IsCreateMetaTemplateField = Convert.ToInt32(gdvMetaTemplateFields.DataKeys[intRowIndex].Value);

            //int intRowIndex = int.Parse(e.CommandArgument.ToString()); 
            //string val = gdvMetaTemplateFields.DataKeys[intRowIndex]["ID"].ToString(); 

            if (e.CommandName.ToLower() == "edit")
            {
                //int intRowIndex = Convert.ToInt32(e.CommandArgument);
                //UserSession.IsCreateMetaTemplateField = Convert.ToInt32(gdvMetaTemplateFields.DataKeys[intRowIndex].Value);
            }
            else if (e.CommandName.ToLower() == "delete")
            {
            }
        }

        protected void gdvMetaTemplateFields_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gdvMetaTemplateFields.EditIndex = e.NewEditIndex;
            gdvMetaTemplateFields.DataSource = UserSession.MetaTemplateField;
            gdvMetaTemplateFields.DataBind();           
        }

        protected void gdvMetaTemplateFields_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gdvMetaTemplateFields.EditIndex = -1;
            gdvMetaTemplateFields.DataSource = UserSession.MetaTemplateField;
            gdvMetaTemplateFields.DataBind();
        }

        protected void gdvMetaTemplateFields_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gdvMetaTemplateFields.PageIndex = e.NewPageIndex;
            gdvMetaTemplateFields.DataSource = UserSession.MetaTemplateField;
            gdvMetaTemplateFields.DataBind();
        }

        protected void gdvMetaTemplateFields_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            UserSession.MetaTemplateField.Rows.RemoveAt(e.RowIndex);
            gdvMetaTemplateFields.EditIndex = -1;
            gdvMetaTemplateFields.DataSource = UserSession.MetaTemplateField;
            gdvMetaTemplateFields.DataBind();
            if (UserSession.MetaTemplateField == null || UserSession.MetaTemplateField.Rows.Count == 0)
            {
                ibtnSave.Visible = false;
                ibtnCancelAll.Visible = false;
            }
        }

        protected void gdvMetaTemplateFields_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            //Data Key Value
            //int intRowIndex = Convert.ToInt32(gdvMetaTemplateFields.DataKeys[e.RowIndex].Value); 

            TextBox txtFieldName = (TextBox)(gdvMetaTemplateFields.Rows[gdvMetaTemplateFields.EditIndex].FindControl("txtFieldName"));
            TextBox txtFieldLength = (TextBox)(gdvMetaTemplateFields.Rows[gdvMetaTemplateFields.EditIndex].FindControl("txtFieldLength"));
            DropDownList ddlFieldDataType = (DropDownList)(gdvMetaTemplateFields.Rows[gdvMetaTemplateFields.EditIndex].FindControl("ddlFieldDataType"));
            DropDownList ddlFieldType = (DropDownList)(gdvMetaTemplateFields.Rows[gdvMetaTemplateFields.EditIndex].FindControl("ddlFieldType"));
            DropDownList ddlStatus = (DropDownList)(gdvMetaTemplateFields.Rows[gdvMetaTemplateFields.EditIndex].FindControl("ddlStatus"));
            CheckBox chkIsPrimary = (CheckBox)(gdvMetaTemplateFields.Rows[gdvMetaTemplateFields.EditIndex].FindControl("chkIsPrimary"));

            if (txtFieldName.Text==string.Empty)
            {
                UserSession.DisplayMessage(this, "Field Name Cannot Be Null.........", MainMasterPage.MessageType.Warning);
                return;               
            }
            else if (txtFieldLength.Text == string.Empty ||Convert.ToInt32(txtFieldLength.Text) <= 0)
            {
                UserSession.DisplayMessage(this, "Field Length Cannot Be Null Or Zero.........", MainMasterPage.MessageType.Warning);
                return;
            }
            else if (ddlFieldDataType.SelectedIndex==0)
            {
                UserSession.DisplayMessage(this, "Select The Data Type.........", MainMasterPage.MessageType.Warning);
                return;
            }
           
            GridViewRow row = gdvMetaTemplateFields.Rows[e.RowIndex];

            //UserSession.MetaTemplateField.Rows[row.DataItemIndex]["FieldName"] = txtFieldName.Text;
            UserSession.MetaTemplateField.Rows[row.DataItemIndex]["FieldDataTypeID"] = ddlFieldDataType.SelectedValue;
            UserSession.MetaTemplateField.Rows[row.DataItemIndex]["FieldTypeID"] = ddlFieldType.SelectedValue;
            UserSession.MetaTemplateField.Rows[row.DataItemIndex]["FieldLength"] = txtFieldLength.Text;
            UserSession.MetaTemplateField.Rows[row.DataItemIndex]["Status"] = ddlStatus.SelectedValue;

            if (UserSession.MetaTemplateField.Rows[row.DataItemIndex]["ID"].ToString() != "")
            {
            //UserSession.MetaTemplateField.Columns.Add("EditFlag");
                UserSession.MetaTemplateField.Rows[row.DataItemIndex]["EditFlag"] = "1";
            //UserSession.MetaTemplateField.AcceptChanges();
            }

            bool boolIsColumnExist = false;
            if (UserSession.MetaTemplateField.Rows[row.DataItemIndex]["FieldName"].ToString() != txtFieldName.Text.Trim())
            {
                boolIsColumnExist = IsColumnExist(txtFieldName.Text.Trim());
            }
            if (!boolIsColumnExist)
            {                
                if (chkIsPrimary.Checked == false)
                {
                    objMetaTemplateFields.IsPrimary = 0;
                    UserSession.DisplayMessage(this, "Field is updated successfully .", MainMasterPage.MessageType.Success);
                }
                else
                {
                    bool IsPrimaryKey = IsPrimaryKeyExist(UserSession.MetaTemplateField);
                    if (!IsPrimaryKey)
                    {
                        objMetaTemplateFields.IsPrimary = 1;
                        UserSession.MetaTemplateField.Rows[row.DataItemIndex]["FieldTypeID"] = 1;
                        UserSession.MetaTemplateField.Rows[row.DataItemIndex]["Status"] = 1;
                    }
                    else
                    {
                        objMetaTemplateFields.IsPrimary = 0;
                        UserSession.DisplayMessage(this, "Primary Key Is Already Set...", MainMasterPage.MessageType.Warning);
                    }
                }
                UserSession.MetaTemplateField.Rows[row.DataItemIndex]["FieldName"] = txtFieldName.Text;
            }
            else
            {
                UserSession.DisplayMessage(this, "Column Name Already Exists...", MainMasterPage.MessageType.Warning);
            }
            UserSession.MetaTemplateField.Rows[row.DataItemIndex]["IsPrimary"] = objMetaTemplateFields.IsPrimary;

            gdvMetaTemplateFields.EditIndex = -1;
            gdvMetaTemplateFields.DataSource = UserSession.MetaTemplateField;
            gdvMetaTemplateFields.DataBind();
        }

        protected void gdvMetaTemplateFields_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            DataRowView objDataRowView = e.Row.DataItem as DataRowView;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddlFieldDataType = (DropDownList)e.Row.FindControl("ddlFieldDataType");
                DropDownList ddlFieldType = (DropDownList)e.Row.FindControl("ddlFieldType");
                DropDownList ddlStatus = (DropDownList)e.Row.FindControl("ddlStatus");
                CheckBox chkIsPrimary = (CheckBox)e.Row.FindControl("chkIsPrimary");
                
                if (objDataRowView["ID"].ToString() == "")
                {
                    e.Row.Cells[10].Visible = true; 
                }
                else
                {
                    e.Row.Cells[10].Visible = false; 
                }                    
                

                if (objDataRowView[5].ToString() == "1")
                {
                    chkIsPrimary.Checked = true;
                }

                Utility.LoadStatus(ddlStatus);
                Utility.LoadFieldType(ddlFieldType);
                Utility.LoadDataType(ddlFieldDataType);

                ddlFieldDataType.SelectedValue = objDataRowView[2].ToString();
                ddlFieldType.SelectedValue = objDataRowView[3].ToString();
                ddlStatus.SelectedValue = objDataRowView[12].ToString();

                if ((e.Row.RowState & DataControlRowState.Edit) > 0)
                {
                    //DropDownList ddlFieldDataType = (DropDownList)e.Row.FindControl("ddlFieldDataType");
                    //DropDownList ddlFieldType = (DropDownList)e.Row.FindControl("ddlFieldType");
                    //DropDownList ddlStatus = (DropDownList)e.Row.FindControl("ddlStatus");

                    //Utility.LoadStatus(ddlStatus);
                    //Utility.LoadFieldType(ddlFieldType);
                    //Utility.LoadDataType(ddlFieldDataType);      

                    //ddlFieldDataType.SelectedValue = objDataRowView[2].ToString();
                    //ddlFieldType.SelectedValue = objDataRowView[3].ToString();
                    //ddlStatus.SelectedValue = objDataRowView[12].ToString();

                    //CheckBox chkIsPrimary = (CheckBox)e.Row.FindControl("chkIsPrimary");
                    //if(objDataRowView[5].ToString()=="1")
                    //{
                    //    chkIsPrimary.Checked = true;
                    //}
                }
            }
        }

        void GroupGridView(GridViewRowCollection gvrc, int startIndex, int total) 
        {
            if (total == 0) 
                return; 
            int i, 
                count = 1; 
            ArrayList lst = new ArrayList(); 
            lst.Add(gvrc[0]); 
            var ctrl = gvrc[0].Cells[startIndex]; 
            for (i = 1; i < gvrc.Count; i++) 
            { 
                TableCell nextCell = gvrc[i].Cells[startIndex]; 
                if (ctrl.Text == nextCell.Text) 
                { 
                    count++; 
                    nextCell.Visible = false; 
                    lst.Add(gvrc[i]); 
                } 
                else 
                { 
                    if (count > 1) 
                    { 
                        ctrl.RowSpan = count; 
                        GroupGridView(new GridViewRowCollection(lst), startIndex + 1, total - 1); 
                    } 
                    count = 1; 
                    lst.Clear(); 
                    ctrl = gvrc[i].Cells[startIndex]; 
                    lst.Add(gvrc[i]); 
                } 
            } 
            if (count > 1) 
            { 
                ctrl.RowSpan = count; 
                GroupGridView(new GridViewRowCollection(lst), startIndex + 1, total - 1); 
            } 
            count = 1; 
            lst.Clear(); 
        }

        protected void ddlFieldDataType_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }
        //Load MetaTemplateFields By MetaTemplateID
        private void LoadGridData(int intMetaTemplateID)
        {
            try
            {
                DataTable objDataTable = new DataTable();
                objDataTable=UserSession.MetaTemplateField;
                objUtility.Result = objMetaTemplateManager.SelectMetaTemplateFeildsByMetaTemplateID(out objDataTable, intMetaTemplateID);
                if (objUtility.Result == Utility.ResultType.Success)
                {
                    if (objDataTable != null && objDataTable.Rows.Count > 0)
                    {
                        gdvMetaTemplateFields.DataSource = objDataTable;
                        gdvMetaTemplateFields.DataBind();
                        UserSession.MetaTemplateField = objDataTable;

                        UserSession.MetaTemplateField.Columns.Add("EditFlag");                       
                        UserSession.MetaTemplateField.AcceptChanges();

                        ibtnSave.Visible = true;
                        ibtnCancelAll.Visible = true;

                        //gdvMetaTemplateFields.Columns[11].Visible = false;                       
                        //ViewState["myTable"] = objDataTable;   

                        //if (hasData == false)
                        //{ 
                        //    gdvMetaTemplateFields.Columns[10].Visible=false;
                        //}

                        //Boolean hasData = false;
                        //for (int col = 0; col < gdvMetaTemplateFields.Columns.Count; col++)
                        //{
                        //    for (int row = 0; col < gdvMetaTemplateFields.Rows.Count; row++)
                        //    {
                        //        if (gdvMetaTemplateFields.Columns[10] != "")
                        //        {
                        //            hasData = true;
                        //            break;
                        //        } 
                        //    }
                        //    gdvMetaTemplateFields.Columns[10].Visible = hasData;
                        //    hasData = false;
                        //}
  
                    }
                }
                else if (objUtility.Result == Utility.ResultType.Failure)
                {
                    gdvMetaTemplateFields.DataSource = null;
                    gdvMetaTemplateFields.DataBind();
                    UserSession.MetaTemplateField = objDataTable;

                    UserSession.MetaTemplateField.Columns.Add("EditFlag");
                    UserSession.MetaTemplateField.AcceptChanges();

                    UserSession.DisplayMessage(this, "No Data To Display .", MainMasterPage.MessageType.Warning);
                }
                else if (objUtility.Result == Utility.ResultType.Error)
                {
                    gdvMetaTemplateFields.DataSource = null;
                    gdvMetaTemplateFields.DataBind();
                    UserSession.MetaTemplateField = objDataTable;

                    UserSession.MetaTemplateField.Columns.Add("EditFlag");
                    UserSession.MetaTemplateField.AcceptChanges();

                    UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                }
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
            }
        }
    }
}