using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DMS.BusinessLogic;
using System.Collections;

namespace DMS.UserControl
{
    public partial class ListItems : System.Web.UI.UserControl
    {
        #region Private Member
        Utility objUtility = new Utility();
        MetaTemplateManager objMetaTemplateManager = new MetaTemplateManager();
        MetaTemplateItem objMetaTemplateItem = new MetaTemplateItem();
        public bool boolUpdateFlag;
        #endregion

        #region Events
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadGridData(UserSession.MetatemplateID);

                switch (objUtility.Result)
                {
                    case Utility.ResultType.Failure:
                        UserSession.DisplayMessage((Page)this.Parent.Page, "No List Item To Display .", MainMasterPage.MessageType.Warning);

                        //Sandip N 16052012
                        DropDownList ddlListItem = gdvListItem.Controls[0].Controls[0].FindControl("ddlListItem") as DropDownList;
                        Utility.LoadMetaTemplateListFields(ddlListItem, UserSession.MetatemplateID);
                        if (ddlListItem.Items.Count < 1)
                        {
                            Response.Redirect("../MetaTemplate/MetaTemplateView.aspx?Type=NoListItem", false);
                        }
                        //Sandip N 16052012
                        break;

                    case Utility.ResultType.Error:
                        UserSession.DisplayMessage((Page)this.Parent.Page, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                        break;
                }
            }
        }
        #endregion

        #region Methods
        private void LoadGridData(int intMetaTemplateID)
        {
            try
            {
                DataTable objDataTable = new DataTable();
                objUtility.Result = objMetaTemplateManager.SelectMetaTemplateListItems(out objDataTable, intMetaTemplateID);
                if (objUtility.Result == Utility.ResultType.Success)
                {
                    if (objDataTable != null && objDataTable.Rows.Count > 0)
                    {
                        DataView objDataView = new DataView(objDataTable);
                        objDataView.Sort = "fieldName";
                        gdvListItem.DataSource = objDataView;
                        gdvListItem.DataBind();

                        //Utility.LoadMetaTemplateListFields(ddlSelectField, objDataTable);
                        //Utility.LoadMetaTemplateListFields(ddlSelectField,UserSession.MetatemplateID);

                        UserSession.MetaTemplateFieldListItem = objDataTable;
                        //GroupGridView(gdvListItem.Rows, 0, 2);
                    }
                }
                else if (objUtility.Result == Utility.ResultType.Failure)
                {
                    gdvListItem.DataSource = null;
                    gdvListItem.DataBind();

                    UserSession.DisplayMessage((Page)this.Parent.Page, "No Data To Display .", MainMasterPage.MessageType.Warning);
                    //Response.Redirect("../MetaTemplate/MetaTemplateView.aspx", false);
                }
                else if (objUtility.Result == Utility.ResultType.Error)
                {
                    gdvListItem.DataSource = null;
                    gdvListItem.DataBind();

                    UserSession.DisplayMessage((Page)this.Parent.Page, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                    //Response.Redirect("../MetaTemplate/MetaTemplateView.aspx", false);
                }
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                UserSession.DisplayMessage((Page)this.Parent.Page, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
            }
        }
        void GroupGridView(GridViewRowCollection gvrc, int startIndex, int total)
        {
            //gvrc: GridView Rows
            //startIndex: index of first column to be grouped(where to start grouping).
            //total: total number of columns to be grouped.

            if (total == 0)
                return;
            int i,
                count = 1;
            ArrayList lst = new ArrayList();
            lst.Add(gvrc[0]);
            //var ctrl = gvrc[0].Cells[startIndex];
            var ctrl = gdvListItem.Rows[0].Cells[startIndex];
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
        #endregion

        protected void gdvListItem_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //DataRowView objDataRowView = e.Row.DataItem as DataRowView;

            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    //DropDownList ddlListItem = (DropDownList)gdvListItem.FooterRow.FindControl("ddlListItem");
            //    DropDownList ddlListItem = (DropDownList)e.Row.FindControl("ddlListItem");
            //    Utility.LoadMetaTemplateListFields(ddlListItem, UserSession.MetaTemplateFieldListItem);

            //    ddlListItem.SelectedValue = objDataRowView[2].ToString();

            //}
        }

        protected void imgbtnAddItem_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                gdvListItem.ShowFooter = true;
                LoadGridData(UserSession.MetatemplateID);
                //DropDownList ddlListItem1 = gdvListItem.FooterRow.FindControl("ddlListItem") as DropDownList;
                //Utility.LoadMetaTemplateListFields(ddlListItem1, UserSession.MetatemplateID);
                
                switch (objUtility.Result)
                {
                    case Utility.ResultType.Success:
                          DropDownList ddlListItem1 = gdvListItem.FooterRow.FindControl("ddlListItem") as DropDownList;
                        Utility.LoadMetaTemplateListFields(ddlListItem1, UserSession.MetatemplateID);
                        break;

                    case Utility.ResultType.Failure:
                        UserSession.DisplayMessage((Page)this.Parent.Page, "No List Item To Display .", MainMasterPage.MessageType.Warning);

                        //Sandip N 16052012
                        DropDownList ddlListItem = gdvListItem.Controls[0].Controls[0].FindControl("ddlListItem") as DropDownList;
                        Utility.LoadMetaTemplateListFields(ddlListItem, UserSession.MetatemplateID);                        
                        //Sandip N 16052012
                        break;

                    case Utility.ResultType.Error:
                        UserSession.DisplayMessage((Page)this.Parent.Page, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                        break;
                }              


                //if (Convert.ToInt32(ddlSelectField.SelectedItem.Value.ToString()) != -1 && txtListItem.Text!=string.Empty)
                //{
                //    objMetaTemplateItem.ListItemText = txtListItem.Text;
                //    objMetaTemplateItem.FieldID = Convert.ToInt32(ddlSelectField.SelectedItem.Value);
                //    objMetaTemplateItem.CreatedBy = UserSession.UserID;
                //    objUtility.Result = objMetaTemplateManager.InsertMetaTemplateFieldItems(objMetaTemplateItem);
                //    txtListItem.Text = string.Empty;
                //    UserSession.DisplayMessage(this, "MetaTemplate Field Item Is Created Successfully .", MainMasterPage.MessageType.Success);
                //    LoadGridData(UserSession.MetatemplateID);

                //    //Response.Redirect("../MetaTemplate/MetaTemplateView.aspx?Type=1", false);
                //}
                //else
                //{
                //    ddlSelectField.Focus();
                //    UserSession.DisplayMessage(this, "Please Select Field From List Or Please Enter The List Item Text.", MainMasterPage.MessageType.Success);
                //}
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                UserSession.DisplayMessage((Page)this.Parent.Page, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
            }
        }

        protected void gdvListItem_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gdvListItem.PageIndex = e.NewPageIndex;
                LoadGridData(UserSession.MetatemplateID);
                if (gdvListItem.ShowFooter == true)
                {
                    DropDownList ddlListItem = gdvListItem.FooterRow.FindControl("ddlListItem") as DropDownList;
                    Utility.LoadMetaTemplateListFields(ddlListItem, UserSession.MetatemplateID);
                }
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                UserSession.DisplayMessage((Page)this.Parent.Page, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
            }
        }

        protected void gdvListItem_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("Insert"))//Enter the List Item For Fields
                {
                    TextBox txtFieldName = gdvListItem.FooterRow.FindControl("txtFieldName") as TextBox;
                    DropDownList ddlListItem = gdvListItem.FooterRow.FindControl("ddlListItem") as DropDownList;

                    if (ddlListItem.SelectedItem.Value != "-1")
                    {
                        if (txtFieldName.Text != string.Empty)
                        {
                            //Check List Item Is Exsist Or Not
                            DataTable objListItem = new DataTable();
                            IEnumerable<DataRow> query = from ListItems in UserSession.MetaTemplateFieldListItem.AsEnumerable() where ListItems.Field<string>("ListItemText") == txtFieldName.Text && ListItems.Field<Int32>("ID") == Convert.ToInt32(ddlListItem.SelectedItem.Value) select ListItems;
                            if (query.Count() > 0)
                                objListItem = query.CopyToDataTable();
                            if (objListItem != null && objListItem.Rows.Count > 0)
                            {
                                UserSession.DisplayMessage((Page)this.Parent.Page, "MetaTemplate Field Item Is Already Exist .", MainMasterPage.MessageType.Success);
                                txtFieldName.Text = string.Empty;
                                txtFieldName.Focus();
                                return;
                            }
                        }
                        else
                        {
                            UserSession.DisplayMessage((Page)this.Parent.Page, "Please Enter The List Item .", MainMasterPage.MessageType.Success);
                            txtFieldName.Focus();
                            return;
                        }
                    }
                    else
                    {
                        UserSession.DisplayMessage((Page)this.Parent.Page, "Please Select The MetaTemplate Field .", MainMasterPage.MessageType.Success);
                        ddlListItem.Focus();
                        return;
                    }
                    objMetaTemplateItem.ListItemText = txtFieldName.Text;
                    objMetaTemplateItem.FieldID = Convert.ToInt32(ddlListItem.SelectedItem.Value);
                    objMetaTemplateItem.CreatedBy = UserSession.UserID;
                    objUtility.Result = objMetaTemplateManager.InsertMetaTemplateFieldItems(objMetaTemplateItem);

                    UserSession.DisplayMessage((Page)this.Parent.Page, "MetaTemplate Field Item Is Created Successfully .", MainMasterPage.MessageType.Success);
                    LoadGridData(UserSession.MetatemplateID);
                    Utility.LoadMetaTemplateListFields(ddlListItem, UserSession.MetatemplateID);
                    gdvListItem.ShowFooter = false;
                    gdvListItem.FooterRow.Visible = false;
                }
                if (e.CommandName.Equals("CancelInsert"))
                {
                    gdvListItem.ShowFooter = false;
                    gdvListItem.FooterRow.Visible = false;
                }
                if (e.CommandName.Equals("EmptyInsert"))//Enter the first List Item For Field
                {
                    TextBox txtFieldName = gdvListItem.Controls[0].Controls[0].FindControl("txtFieldName") as TextBox;
                    DropDownList ddlListItem = gdvListItem.Controls[0].Controls[0].FindControl("ddlListItem") as DropDownList;

                    if (ddlListItem.SelectedItem.Value != "-1")
                    {
                        if (txtFieldName.Text != string.Empty)
                        {
                            objMetaTemplateItem.ListItemText = txtFieldName.Text;
                            objMetaTemplateItem.FieldID = Convert.ToInt32(ddlListItem.SelectedItem.Value);
                            objMetaTemplateItem.CreatedBy = UserSession.UserID;
                            objUtility.Result = objMetaTemplateManager.InsertMetaTemplateFieldItems(objMetaTemplateItem);

                            UserSession.DisplayMessage((Page)this.Parent.Page, "MetaTemplate Field Item Is Created Successfully .", MainMasterPage.MessageType.Success);
                            LoadGridData(UserSession.MetatemplateID);
                            Utility.LoadMetaTemplateListFields(ddlListItem, UserSession.MetatemplateID);
                            gdvListItem.ShowFooter = false;
                            gdvListItem.FooterRow.Visible = false;
                        }
                        else
                        {
                            UserSession.DisplayMessage((Page)this.Parent.Page, "Please Enter The List Item .", MainMasterPage.MessageType.Success);
                            txtFieldName.Focus();
                            return;
                        }
                    }
                    else
                    {
                        UserSession.DisplayMessage((Page)this.Parent.Page, "Please Select The MetaTemplate Field .", MainMasterPage.MessageType.Success);
                        ddlListItem.Focus();
                        return;
                    }

                    //objMetaTemplateItem.ListItemText = txtFieldName.Text;
                    //objMetaTemplateItem.FieldID = Convert.ToInt32(ddlListItem.SelectedItem.Value);
                    //objMetaTemplateItem.CreatedBy = UserSession.UserID;
                    //objUtility.Result = objMetaTemplateManager.InsertMetaTemplateFieldItems(objMetaTemplateItem);

                    //UserSession.DisplayMessage((Page)this.Parent.Page, "MetaTemplate Field Item Is Created Successfully .", MainMasterPage.MessageType.Success);
                    //LoadGridData(UserSession.MetatemplateID);
                    //Utility.LoadMetaTemplateListFields(ddlListItem, UserSession.MetatemplateID);
                    //gdvListItem.ShowFooter = false;
                    //gdvListItem.FooterRow.Visible = false;
                }
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                UserSession.DisplayMessage((Page)this.Parent.Page, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
            }
        }

        protected void imgbtnCancel_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("../MetaTemplate/MetaTemplateView.aspx?ID=2", false);
        }
    }
}