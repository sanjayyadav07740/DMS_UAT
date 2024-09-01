using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DMS.BusinessLogic;
using System.Data.Common;
using DMS.BusinessLogic.Entity;
using System.IO;


namespace DMS.Shared
{
    public partial class DocumentDashBoard : System.Web.UI.Page
    {
        #region Private Memeber
        Utility objUtility = new Utility();
        #endregion

        #region Event
        protected void Page_Load(object sender, EventArgs e)
        {
            ((AjaxControlToolkit.ToolkitScriptManager)Master.FindControl("tsmManager")).RegisterPostBackControl(ibtnExportData);
            if (!IsPostBack)
            {
                if (Request["Type"] != null)
                {
                    if (Request["Type"].ToString().Trim() == "1")
                    {
                        UserSession.DisplayMessage(this, "Document Entry Has Been Done Successfully .", MainMasterPage.MessageType.Success);
                    }
                }
                if (Request["MetaDataCode"] != null)
                {
                    UserSession.DisplayMessage(this, "Document Are Uploaded Successfully With MetaData Code : "+Request["MetaDataCode"].ToString().Trim(), MainMasterPage.MessageType.Success);                   
                }

                UserSession.GridData = null;
                UserSession.MetaDataID = 0;
                totalfilediv.Visible = false;
                Log.AuditLog(HttpContext.Current, "Visit", "DocumentDashboard");
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

        protected void gvwDocument_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                Utility.SetGridHoverStyle(e);
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        protected void gvwDocument_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandSource is LinkButton)
                {
                    if (((LinkButton)e.CommandSource).Text.Trim() == "0")
                    {
                        UserSession.DisplayMessage(this, "No Data To Display.", MainMasterPage.MessageType.Warning);
                        return;
                    }
                }
                if (e.CommandName.ToLower().Trim() == "approved")
                {
                    int intRowIndex = Convert.ToInt32(e.CommandArgument);
                    UserSession.MetaDataID = Convert.ToInt32(gvwDocument.DataKeys[intRowIndex].Value);
                    string strDocumentID = gvwDocument.DataKeys[intRowIndex].Values["DOCID"].ToString().Trim();
                    Response.Redirect("../MetaData/ApprovedDocument.aspx?DOCID=" + strDocumentID, false);
                    DataTable dt1 = DataHelper.ExecuteDataTable("select * from vwDocumentversion where documentid=" + strDocumentID, null);
                    Session["sampletable"] = dt1;
                    
                }
                else if (e.CommandName.ToLower().Trim() == "rejected")
                {
                    int intRowIndex = Convert.ToInt32(e.CommandArgument);
                    UserSession.MetaDataID = Convert.ToInt32(gvwDocument.DataKeys[intRowIndex].Value);
                    string strDocumentID = gvwDocument.DataKeys[intRowIndex].Values["DOCID"].ToString().Trim();
                    Response.Redirect("../MetaData/RejectedDocument.aspx?DOCID=" + strDocumentID, false);
                    DataTable dt2 = DataHelper.ExecuteDataTable("select * from vwDocumentversion where documentid=" + strDocumentID, null);
                    Session["sampletable"] = dt2;
                    //Response.Redirect("../MetaData/RejectedDocument.aspx", false);
                }
                else if (e.CommandName.ToLower().Trim() == "pending")
                {
                    int intRowIndex = Convert.ToInt32(e.CommandArgument);
                    UserSession.MetaDataID = Convert.ToInt32(gvwDocument.DataKeys[intRowIndex].Value);
                    string strDocumentID = gvwDocument.DataKeys[intRowIndex].Values["DOCID"].ToString().Trim();
                    Response.Redirect("../MetaData/DocumentEntry.aspx?DOCID=" + strDocumentID, false);
                    DataTable dt3 = DataHelper.ExecuteDataTable("select * from vwDocumentversion where documentid=" + strDocumentID, null);
                    Session["sampletable"] = dt3;
                    //Response.Redirect("../MetaData/DocumentEntry.aspx", false);
                }
                else if (e.CommandName.ToLower().Trim() == "entrycompleted")
                {
                    int intRowIndex = Convert.ToInt32(e.CommandArgument);
                    UserSession.MetaDataID = Convert.ToInt32(gvwDocument.DataKeys[intRowIndex].Value);
                    string strDocumentID = gvwDocument.DataKeys[intRowIndex].Values["DOCID"].ToString().Trim();
                    Response.Redirect("../MetaData/DocumentVerification.aspx?DOCID=" + strDocumentID, false);
                    DataTable dt4 = DataHelper.ExecuteDataTable("select * from vwDocumentversion where documentid=" + strDocumentID, null);
                    Session["sampletable"] = dt4;
                    //Response.Redirect("../MetaData/DocumentVerification.aspx", false);
                }
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        protected void gvwDocument_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                if (UserSession.GridData != null)
                {
                    gvwDocument.PageIndex = e.NewPageIndex;
                    gvwDocument.DataSource = UserSession.GridData;
                    gvwDocument.DataBind();
                }
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        protected void gvwDocumentList_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                if (UserSession.GridData != null)
                {
                    if (ViewState[UserSession.SortExpression] == null || ViewState[UserSession.SortExpression].ToString() == "DESC")
                        ViewState[UserSession.SortExpression] = "ASC";
                    else
                        ViewState[UserSession.SortExpression] = "DESC";

                    gvwDocumentList.DataSource = UserSession.SortedGridData(e.SortExpression, ViewState[UserSession.SortExpression].ToString());
                    gvwDocumentList.DataBind();
                }
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        protected void gvwDocumentList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                Utility.SetGridHoverStyle(e);
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        protected void gvwDocumentList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.ToLower().Trim() == "documentview")
                {
                    int intRowIndex = Convert.ToInt32(e.CommandArgument);
                    UserSession.MetaDataID = Convert.ToInt32(gvwDocumentList.DataKeys[intRowIndex].Values["MetaDataID"]);
                    string strDocumentID = gvwDocumentList.DataKeys[intRowIndex].Values["ID"].ToString().Trim();
                    string strStatus = gvwDocumentList.DataKeys[intRowIndex].Values["DocumentStatusID"].ToString().Trim();
                    switch (strStatus)
                    {
                        case "1":
                            Response.Redirect("../MetaData/ApprovedDocument.aspx?DOCID=" + strDocumentID, false);
                            DataTable dt1 = DataHelper.ExecuteDataTable("select * from vwDocumentversion where documentid=" + strDocumentID, null);
                            Session["sampletable"] = dt1;
                            break;

                        case "2":
                            Response.Redirect("../MetaData/RejectedDocument.aspx?DOCID=" + strDocumentID, false);
                            DataTable dt2 = DataHelper.ExecuteDataTable("select * from vwDocumentversion where documentid=" + strDocumentID, null);
                            Session["sampletable"] = dt2;
                            break;

                        case "3":
                            Response.Redirect("../MetaData/DocumentEntry.aspx?DOCID=" + strDocumentID, false);
                            DataTable dt3 = DataHelper.ExecuteDataTable("select * from vwDocumentversion where documentid=" + strDocumentID, null);
                            Session["sampletable"] = dt3;
                            break;

                        case "4":
                            Response.Redirect("../MetaData/DocumentVerification.aspx?DOCID=" + strDocumentID, false);
                             DataTable dt4 = DataHelper.ExecuteDataTable("select * from vwDocumentversion where documentid=" + strDocumentID , null);
                             Session["sampletable"] = dt4;
                            break;
                    }

                }
              
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        protected void gvwDocumentList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                if (UserSession.GridData != null)
                {
                    gvwDocumentList.PageIndex = e.NewPageIndex;
                    gvwDocumentList.DataSource = UserSession.GridData;
                    gvwDocumentList.DataBind();
                }
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        protected void ibtnAddNew_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                Response.Redirect("../MetaData/UploadDocuments.aspx", false);
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
            }
        }

        protected void ibtnShow_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (UserSession.UserID == 956)
                {
                    ((RequiredFieldValidator)emodModule.FindControl("rfvMetaTemplateName")).Enabled = false;
                    gvwDocumentList.Visible = true;
                    totalfilediv.Visible = true;
                    gvwDocument.Visible = false;
                    if (emodModule.SelectedMetaTemplate == -1 || emodModule.SelectedMetaTemplate == 0)
                    {
                        BusinessLogic.MetaData objMetaData = new BusinessLogic.MetaData()
                        {
                            RepositoryID = emodModule.SelectedRepository,
                            MetaTemplateID = emodModule.SelectedMetaTemplate,
                            CategoryID = emodModule.SelectedCategory,
                            FolderID = emodModule.SelectedFolder,
                            MetaDataID = emodModule.SelectedMetaDataCode
                        };

                        DataTable objDataTable = new DataTable();
                        DocumentManager objDocumentManager = new DocumentManager();
                        objUtility.Result = objDocumentManager.SelectReportDocument(out objDataTable, objMetaData, txtFrom.Text, txtTo.Text);
                        switch (objUtility.Result)
                        {
                            case Utility.ResultType.Success:
                                    gvwDocumentList.Visible = true;
                                    totalfilediv.Visible = true;
                                    gvwDocument.Visible = false;
                                    lbldtcount.Text = objDataTable.Rows.Count.ToString();                 
                                    gvwDocumentList.DataSource = objDataTable;
                                    gvwDocumentList.DataBind();                                
                                UserSession.GridData = objDataTable;
                                break;

                            case Utility.ResultType.Failure:                                
                                    gvwDocumentList.DataSource = objDataTable;
                                    gvwDocumentList.DataBind();                                
                                UserSession.DisplayMessage(this, "No Data To Display .", MainMasterPage.MessageType.Warning);
                                break;

                            case Utility.ResultType.Error:                                
                                    gvwDocumentList.DataSource = objDataTable;
                                    gvwDocumentList.DataBind();                                
                                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                                break;
                        }
                    }
                    else
                    {
                        totalfilediv.Visible = false;
                        //gvwDocument.Visible = true;
                        LoadGridDataByCriteria();
                    }

                }
                else
                {
                    ((RequiredFieldValidator)emodModule.FindControl("rfvMetaTemplateName")).Enabled = true;

                    if(emodModule.SelectedMetaTemplate == -1)
                    {
                        UserSession.DisplayMessage(this, "Please Select MetaTemplate Name .", MainMasterPage.MessageType.Warning);
                        return;
                    }
                    else
                    {
                        LoadGridDataByCriteria();
                    }
                    
                }
                
                // LoadGridDataByCriteria();
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
            }
        }

        protected void ibtnExportData_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (emodModule.SelectedMetaDataCode == -1)
                {
                    UserSession.DisplayMessage(this, "Please Select The MetaDataCode .", MainMasterPage.MessageType.Warning);
                    return;
                }
                
                BusinessLogic.DocumentManager objDocumentManager = new BusinessLogic.DocumentManager();
                BusinessLogic.MetaTemplateManager objMetaTemplateManager = new BusinessLogic.MetaTemplateManager();

                DataTable objDataTableMetaData = new DataTable();
                if (emodModule.SelectedMetaDataCode != 0)
                {
                    objUtility.Result = objDocumentManager.SelectMetaData(out objDataTableMetaData, emodModule.SelectedMetaDataCode);
                    switch (objUtility.Result)
                    {
                        case BusinessLogic.Utility.ResultType.Failure:
                            UserSession.DisplayMessage(this, "No MetaData To Display .", MainMasterPage.MessageType.Warning);
                            return;
                            break;

                        case BusinessLogic.Utility.ResultType.Error:
                            UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                            return;
                            break;
                    }
                }

                DataTable objDataTableField = new DataTable();

                objUtility.Result = objDocumentManager.SelectField(out objDataTableField, Convert.ToInt32(objDataTableMetaData.Rows[0]["MetaTemplateID"]));

                switch (objUtility.Result)
                {
                    case BusinessLogic.Utility.ResultType.Failure:
                        UserSession.DisplayMessage(this, "No Fields To Display .", MainMasterPage.MessageType.Warning);
                        return;
                        break;

                    case BusinessLogic.Utility.ResultType.Error:
                        UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                        return;
                        break;
                }


                DataTable objDataTableDocument = new DataTable();
                objDataTableDocument.Columns.Add("DocumentName");
                objDataTableDocument.Columns.Add("DocumentType");
                objDataTableDocument.Columns.Add("DocumentSize");
                objDataTableDocument.Columns.Add("DocumentStaus");

                foreach (DataRow objDataRow in objDataTableField.Rows)
                {
                    objDataTableDocument.Columns.Add(objDataRow["ID"].ToString());
                }
                objDataTableDocument.AcceptChanges();

                string strDocumentID = string.Empty;

                DataTable objDataTableMetaDataDocument = new DataTable();

                objUtility.Result = objDocumentManager.SelectDocument(out objDataTableMetaDataDocument, emodModule.SelectedMetaDataCode);

                switch (objUtility.Result)
                {
                    case BusinessLogic.Utility.ResultType.Failure:
                        UserSession.DisplayMessage(this, "No Document To Display .", MainMasterPage.MessageType.Warning);
                        return;
                        break;

                    case BusinessLogic.Utility.ResultType.Error:
                        UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                        return;
                        break;
                }



                DataTable objDataTableDocumentData = new DataTable();
                objUtility.Result = objDocumentManager.SelectDocumentData(out objDataTableDocumentData, emodModule.SelectedMetaDataCode);
                switch (objUtility.Result)
                {
                    case BusinessLogic.Utility.ResultType.Failure:
                        UserSession.DisplayMessage(this, "No Document Data To Display .", MainMasterPage.MessageType.Warning);
                        return;
                        break;

                    case BusinessLogic.Utility.ResultType.Error:
                        UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                        return;
                        break;
                }

                foreach (DataRow objDataRowMetaDataDocument in objDataTableMetaDataDocument.Rows)
                {
                    DataRow objDataRowNew = objDataTableDocument.NewRow();
                    objDataRowNew["DocumentName"] = objDataRowMetaDataDocument["DocumentName"].ToString();
                    objDataRowNew["DocumentType"] = objDataRowMetaDataDocument["DocumentType"].ToString();
                    objDataRowNew["DocumentSize"] = objDataRowMetaDataDocument["Size"].ToString();
                    objDataRowNew["DocumentStaus"] = objDataRowMetaDataDocument["DocumentStatus"].ToString();

                    DataRow[] objDataRowDocumentData = objDataTableDocumentData.Select("DocumentID=" + Convert.ToInt32(objDataRowMetaDataDocument["ID"]));

                    int intDocumentID = Convert.ToInt32(objDataRowMetaDataDocument["ID"]);
                    foreach (DataRow objDataRow in objDataTableField.Rows)
                    {
                        int intFieldID = Convert.ToInt32(objDataRow["ID"]);
                        DataRow[] objDataRowCollection = objDataTableDocumentData.Select("DocumentID=" + intDocumentID + " AND FieldID=" + intFieldID);
                        if (objDataRowCollection.Length > 0)
                        {
                            objDataRowNew[objDataRow["ID"].ToString()] = objDataRowCollection[0]["FieldData"].ToString();
                        }
                    }

                    objDataTableDocument.Rows.Add(objDataRowNew);
                }
                objDataTableDocument.AcceptChanges();

                DataTable objDataTableList = new DataTable();
                objUtility.Result = objMetaTemplateManager.SelectListItem(out objDataTableList, Convert.ToInt32(objDataTableMetaData.Rows[0]["MetaTemplateID"]));
                switch (objUtility.Result)
                {
                    case BusinessLogic.Utility.ResultType.Error:
                        UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                        return;
                        break;
                }

                objDataTableList.AcceptChanges();

                DataRow[] objDataRowList = objDataTableField.Select("FieldDataTypeID IN (4,9)");
                foreach (DataRow objDataRow in objDataRowList)
                {
                    if (objDataRow["FieldDataTypeID"].ToString().Trim() == "4")
                    {
                        foreach (DataRow objDataRowDocument in objDataTableDocument.Rows)
                        {
                            string strDocumentFieldValue = objDataRowDocument[objDataRow["ID"].ToString().Trim()].ToString().Trim();
                            if (strDocumentFieldValue != string.Empty)
                            {
                                if (objDataTableList.Select("FieldID=" + objDataRow["ID"].ToString() + " AND ID=" + strDocumentFieldValue).Length > 0)
                                {
                                    objDataRowDocument[objDataRow["ID"].ToString().Trim()] = objDataTableList.Select("FieldID=" + objDataRow["ID"].ToString() + " AND ID=" + strDocumentFieldValue)[0]["ListItemText"].ToString();
                                    objDataTableDocument.AcceptChanges();
                                }
                            }
                        }
                    }
                    if (objDataRow["FieldDataTypeID"].ToString().Trim() == "9")
                    {
                        foreach (DataRow objDataRowDocument in objDataTableDocument.Rows)
                        {
                            string strDocumentFieldValue = objDataRowDocument[objDataRow["ID"].ToString().Trim()].ToString().Trim();
                            if (strDocumentFieldValue != string.Empty)
                            {
                                string[] strArray = strDocumentFieldValue.Split(',');
                                string strSelectedItem = string.Empty;
                                foreach (string strItem in strArray)
                                {
                                    if (objDataTableList.Select("FieldID=" + objDataRow["ID"].ToString() + " AND ID=" + strItem).Length > 0)
                                    {
                                        strSelectedItem = strSelectedItem + objDataTableList.Select("FieldID=" + objDataRow["ID"].ToString() + " AND ID=" + strItem)[0]["ListItemText"].ToString() + ",";
                                    }

                                }
                                if (strSelectedItem != string.Empty)
                                    strSelectedItem = strSelectedItem.Remove(strSelectedItem.LastIndexOf(','));

                                objDataRowDocument[objDataRow["ID"].ToString().Trim()] = strSelectedItem;
                                objDataTableDocument.AcceptChanges();
                            }
                        }
                    }

                }
                foreach (DataRow objDataRow in objDataTableField.Rows)
                {
                    objDataTableDocument.Columns[objDataRow["ID"].ToString().Trim()].ColumnName = objDataRow["FieldName"].ToString().Trim();
                    objDataTableDocument.AcceptChanges();
                }

                Utility.ExportToExcel(objDataTableDocument,((DropDownList)emodModule.FindControl("ddlMetaDataCode")).SelectedItem.Text.Trim() );
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
            }
        }

        protected void ibtnExportDataNew_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                BusinessLogic.MetaData objMetaData = new BusinessLogic.MetaData()
                {
                    RepositoryID = emodModule.SelectedRepository,
                    MetaTemplateID = emodModule.SelectedMetaTemplate,
                    CategoryID = emodModule.SelectedCategory,
                    FolderID = emodModule.SelectedFolder,
                    MetaDataID = emodModule.SelectedMetaDataCode
                };

                DataTable objDataTable = new DataTable();
                DocumentManager objDocumentManager = new DocumentManager();

                if (UserSession.UserID == 956 )
                {
                    objUtility.Result = objDocumentManager.SelectReportDocument(out objDataTable, objMetaData, txtFrom.Text, txtTo.Text);
                    switch (objUtility.Result)
                    {
                        case BusinessLogic.Utility.ResultType.Failure:
                            UserSession.DisplayMessage(this, "No Data To Export .", MainMasterPage.MessageType.Warning);
                            return;
                            break;

                        case BusinessLogic.Utility.ResultType.Error:
                            UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                            return;
                            break;
                    }
                    if (ddlExport.SelectedValue == "Excel")
                        ExportToExcel(objDataTable, string.Format("DocumentReport{0:dd MMMM yyyy HH mm tt}", DateTime.Now));
                    else
                        csv(objDataTable);

                }
                else
                {
                    if (emodModule.SelectedMetaTemplate == -1)
                    {
                        UserSession.DisplayMessage(this, "Please Select MetaTemplate Name .", MainMasterPage.MessageType.Warning);
                        return;
                    }
                    else
                    {
                        if (objMetaData.RepositoryID == 1)//require merge page count and merging date for MHADA
                        {
                            objUtility.Result = objDocumentManager.SelectReportDocumentMHADA(out objDataTable, objMetaData, txtFrom.Text, txtTo.Text);
                        }
                        else
                        {
                            objUtility.Result = objDocumentManager.SelectReportDocument(out objDataTable, objMetaData, txtFrom.Text, txtTo.Text);

                        }
                        switch (objUtility.Result)
                        {
                            case BusinessLogic.Utility.ResultType.Failure:
                                UserSession.DisplayMessage(this, "No Data To Export .", MainMasterPage.MessageType.Warning);
                                return;
                                break;

                            case BusinessLogic.Utility.ResultType.Error:
                                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                                return;
                                break;
                        }
                        if (ddlExport.SelectedValue == "Excel")
                            ExportToExcel(objDataTable, string.Format("DocumentReport{0:dd MMMM yyyy HH mm tt}", DateTime.Now));
                        else
                            csv(objDataTable);

                    }
                }
                
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
            }
        }
        #endregion

        #region Method

        private void ExportToExcel(DataTable objTable, string fileName)
        {
            string attachment = "attachment; filename=" + fileName + ".xls";
            Response.ClearContent();
            Response.AddHeader("content-disposition", attachment);
            Response.ContentType = "application/vnd.ms-excel";

            string tab = "";
            foreach (DataColumn dc in objTable.Columns)
            {
                Response.Write(tab + dc.ColumnName);
                tab = "\t";
            }
            Response.Write("\n");
            int i;
            foreach (DataRow dr in objTable.Rows)
            {
                tab = "";
                int docid = Convert.ToInt32(dr["ID"]);
                for (i = 0; i < objTable.Columns.Count; i++)
                {
                    Response.Write(tab + (dr[i].ToString().Replace("\r\n", "")));
                    tab = "\t";
                }
                Response.Write("\n");

            }
            if (UserSession.RoleID == 1 || UserSession.RoleID == 131)
            {
                Log.AuditLog(HttpContext.Current, "Download Document Report", "DocumentDashBaord");
            }
            else
            {
                DataTable copyDataTable;
                copyDataTable = objTable.Copy();
                copyDataTable.Columns.Remove("RepositoryName");
                copyDataTable.Columns.Remove("DepartmentName");
                copyDataTable.Columns.Remove("MetaTemplateName");
                copyDataTable.Columns.Remove("FolderPath");
                copyDataTable.Columns.Remove("FolderName");
                copyDataTable.Columns.Remove("MetaDataCode");
                copyDataTable.Columns.Remove("DocumentName");
                copyDataTable.Columns.Remove("Size");
                copyDataTable.Columns.Remove("PageCount");
                copyDataTable.Columns.Remove("CreatedOn");
                copyDataTable.Columns.Remove("CreatedBy");

                Log._DocumentAuditLog(HttpContext.Current, "Download Document Report", "DocumentDashBaord", copyDataTable);

            }

            Response.End();
        }


        private void csv(DataTable dt)
        {
            if (dt.Rows.Count > 0)
            {
                HttpContext context = HttpContext.Current;
                context.Response.Clear();
                foreach (DataColumn column in dt.Columns)
                {
                    context.Response.Write(column.ColumnName + ",");
                }
                context.Response.Write(Environment.NewLine);
                foreach (DataRow row in dt.Rows)
                {
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        context.Response.Write(row[i].ToString().Replace(",", string.Empty) + ",");
                    }
                    context.Response.Write(Environment.NewLine);
                }
                context.Response.ContentType = "text/csv";
                string Filename = string.Format("DocumentReport{0:dd MMMM yyyy HH mm tt}", DateTime.Now);
                context.Response.AppendHeader("Content-Disposition", "attachment; filename="+Filename+".csv");
                HttpContext.Current.Response.Flush(); // Sends all currently buffered output to the client.
                HttpContext.Current.Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
                HttpContext.Current.ApplicationInstance.CompleteRequest(); // Causes ASP.NET to bypass all events and filtering in the HTTP pipeline chain of execution and directly execute the EndRequest event.
            }
        }
        private void LoadGridDataByCriteria()
        {
            try
            {
                DocumentManager objDocumentManager = new DocumentManager();
                DataTable objDataTable = new DataTable();
                string FromDate = txtFrom.Text;
                string ToDate = txtTo.Text;

                
                if (emodModule.SelectedMetaDataCode == -1)
                {
                    gvwDocumentList.Visible = false;
                    gvwDocument.Visible = true;
                    BusinessLogic.MetaData objMetaData = new BusinessLogic.MetaData()
                                            {
                                                RepositoryID = emodModule.SelectedRepository,
                                                MetaTemplateID = emodModule.SelectedMetaTemplate,
                                                CategoryID = emodModule.SelectedCategory,
                                                FolderID = emodModule.SelectedFolder
                                            };
                    
                    objUtility.Result = objDocumentManager.SelectMetaDataForGridNew(out objDataTable, DocumentManager.Status.TotalUploaded, objMetaData,FromDate,ToDate);
                }
                else
                {
                    gvwDocumentList.Visible = true;
                    gvwDocument.Visible = false;
                    objUtility.Result = objDocumentManager.SelectAllDocument(out objDataTable, emodModule.SelectedMetaDataCode, DocumentManager.Status.TotalUploaded);
                }

                switch (objUtility.Result)
                {
                    case Utility.ResultType.Success:
                        if (emodModule.SelectedMetaDataCode == -1)
                        {
                            gvwDocument.DataSource = objDataTable;
                            gvwDocument.DataBind();
                        }
                        else
                        {
                            gvwDocumentList.DataSource = objDataTable;
                            gvwDocumentList.DataBind();
                        }
                        UserSession.GridData = objDataTable;
                        break;

                    case Utility.ResultType.Failure:
                        if (emodModule.SelectedMetaDataCode == -1)
                        {
                            gvwDocument.DataSource = objDataTable;
                            gvwDocument.DataBind();
                        }
                        else
                        {
                            gvwDocumentList.DataSource = objDataTable;
                            gvwDocumentList.DataBind();
                        }
                        UserSession.DisplayMessage(this, "No Data To Display .", MainMasterPage.MessageType.Warning);
                        break;

                    case Utility.ResultType.Error:
                        if (emodModule.SelectedMetaDataCode == -1)
                        {
                            gvwDocument.DataSource = objDataTable;
                            gvwDocument.DataBind();
                        }
                        else
                        {
                            gvwDocumentList.DataSource = objDataTable;
                            gvwDocumentList.DataBind();
                        }
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