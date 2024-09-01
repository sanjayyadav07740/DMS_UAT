using DMS.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Collections;

namespace DMS.Shared
{
    public partial class MutiFieldSearch : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(Utility.ConnectionString);
        string strDocumentID;
        string strMRNO;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillState();
                lblDistt.Visible = false;
                ddlDistt.Visible = false;
                if(UserSession.GridData!=null)
                {
                    gvwDocument.DataSource = UserSession.GridData;
                    gvwDocument.DataBind();
                }
            }
            
        }
        protected void FillState()
        {
            string str = "select distinct State from PACL_DEEDS";
            DataSet ds = new DataSet();
            ds = DataHelper.ExecuteDataSet(str);
            if(ds.Tables[0].Rows.Count>0)
            {
                ddlState.DataSource = ds.Tables[0];
                ddlState.DataTextField = "State";
                ddlState.DataBind();
            }
            ddlState.Items.Insert(0, "--Select--");
        }

        protected void ibtnShow_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                string str1="";
                string FinalStr = "";
                if (txtMrNo.Text == "" && txtSeizureWise.Text == "" && txtSDeedNo.Text == "" && txtDetailsOfBuyer.Text == "" && txtDetailsOfSeller.Text == "" && txtD_O_P.Text == "" && txtAmount.Text == "" && txtArea.Text == "" && ddlState.SelectedItem.Text == "--Select--" && ddlDistt.SelectedItem.Text == "--Select--" && txtTehsil.Text == "" && txtVillage.Text == "" && txtSyNo.Text == "" && txtMode.Text=="")
                {
                    UserSession.DisplayMessage(this, "Please enter value of alteast one field!", MainMasterPage.MessageType.Error);
                    return;
                }
                TextBox[] txts = new TextBox[] { txtMrNo, txtSeizureWise, txtSDeedNo, txtDetailsOfBuyer, txtDetailsOfSeller, txtD_O_P, txtAmount, txtArea, txtTehsil, txtVillage, txtSyNo, txtMode};
                DataSet dsResult = new DataSet();
                dsResult = FillGrid(txts, out dsResult);

               if (dsResult.Tables[0].Rows.Count > 0)
                {
                //    char charQuote = '"';
                //    for (int i = 0; i < dsResult.Tables[0].Rows.Count;i++ )
                //    {
                //        str1 = String.Join(",", dsResult.Tables[0].Rows[i]["DOCUMENTID"].ToString());
                //        FinalStr = FinalStr + "," + dsResult.Tables[0].Rows[i]["DOCUMENTID"].ToString();
                        
                //            //string.Join(",", newval.ToArray());
                //    }
                //    FinalStr = FinalStr.Substring(1);
                //    string str = "select MetaDataID,Document.ID,Document.DocumentStatusID,Document.DocumentGuid,MetaData.MetaDataCode,Document.DocumentName," + charQuote + "Size" + charQuote + ",DocumentType,Tag,PAGECOUNT,Document.CreatedOn," +
                //                        " (SELECT StatusName FROM vwDocumentStatus WHERE ID= Document.DocumentStatusID ) AS " + charQuote + "DocumentStatus" + charQuote + " from Document inner join MetaData on Document.MetaDataID=MetaData.id where Document.ID in(" + FinalStr + ")";
                //    DataSet ds = new DataSet();
                //    ds = DataHelper.ExecuteDataSet(str);
                //    if (ds.Tables[0].Rows.Count > 0)
                //    {
                        gvwDocument.Visible = true;
                        gvwDocument.DataSource = dsResult.Tables[0];
                        gvwDocument.DataBind();
                        UserSession.GridData = dsResult.Tables[0];
                        //ibtnExport.Visible = true;
                    //}
                    //UserSession.GridData = ds.Tables[0];
                }
                else
                {
                    UserSession.DisplayMessage(this, "No Data To Display .", MainMasterPage.MessageType.Warning);
                    gvwDocument.Visible = false;
                    //ibtnExport.Visible = false;
                }
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }
        public DataSet FillGrid(TextBox[] txtArray, out DataSet dsResult)
        {
            try
            {
                string query = "";
                //DocumentManager objDocumentManager = new DocumentManager();
                //BusinessLogic.MetaData objMetaData = new BusinessLogic.MetaData()
                //{
                //    RepositoryID = emodModule.SelectedRepository,
                //    MetaTemplateID = emodModule.SelectedMetaTemplate,
                //    CategoryID = emodModule.SelectedCategory,
                //    FolderID = emodModule.SelectedFolder,
                //    MetaDataID = emodModule.SelectedMetaDataCode
                //};
                char charQuote = '"';
                StringBuilder txtWhere = new StringBuilder();
                string str = "";
                txtWhere.Append("select * from PACL_DEEDS where ");
                foreach (TextBox txt in txtArray)
                {
                    switch (txt.ID)
                    {
                        case "txtMrNo":
                            if (!string.IsNullOrEmpty(txtMrNo.Text))
                            {
                                //string Filename = txtFileName.Text.ToUpper();
                                //if (Filename.Contains("."))
                                //    Filename = Filename.Remove(Filename.IndexOf("."));
                                 txtWhere.Append("MR_NO like '%"+txtMrNo.Text+"%' and ");
                                
                               
                            }
                            break;
                        case "txtSeizureWise":
                            if (!string.IsNullOrEmpty(txtSeizureWise.Text))
                            {
                                txtWhere.Append("SRNO_SEIZURE_WISE like '%" + txtSeizureWise.Text+ "%' and ");
                               
                            }
                            break;
                        case "txtSDeedNo":
                            if (!string.IsNullOrEmpty(txtSDeedNo.Text))
                            {
                                txtWhere.Append("SDEEDNO_AGREEMENT like '%" + txtSDeedNo.Text + "%' and ");

                            }
                            break;
                        case "txtDetailsOfBuyer":
                            if (!string.IsNullOrEmpty(txtDetailsOfBuyer.Text))
                            {
                                txtWhere.Append("DETAILS_OF_BUYER like '%" + txtDetailsOfBuyer.Text + "%' and ");

                            }
                            break;
                        case "txtDetailsOfSeller":
                            if (!string.IsNullOrEmpty(txtDetailsOfSeller.Text))
                            {
                                txtWhere.Append("DETAILS_OF_SELLER like '%" + txtDetailsOfSeller.Text + "%' and ");

                            }
                            break;
                        case "txtD_O_P":
                            if (!string.IsNullOrEmpty(txtD_O_P.Text))
                            {
                                txtWhere.Append("D_O_P ='" + txtD_O_P.Text + "' and ");

                            }
                            break;
                        case "txtAmount":
                            if (!string.IsNullOrEmpty(txtAmount.Text))
                            {
                                txtWhere.Append("AMOUNT ='" + txtAmount.Text + "' and ");

                            }
                            break;
                        case "txtArea":
                            if (!string.IsNullOrEmpty(txtArea.Text))
                            {
                                txtWhere.Append("AREA like '%" + txtArea.Text + "%' and ");

                            }
                            break;
                        //case "txtState":
                        //    if (!string.IsNullOrEmpty(txtState.Text))
                        //    {
                        //        txtWhere.Append("STATE like '%" + txtState.Text + "%' and ");

                        //    }
                        //    break;
                        //case "txtDistt":
                        //    if (!string.IsNullOrEmpty(txtDistt.Text))
                        //    {
                        //        txtWhere.Append("DISTT like '%" + txtDistt.Text + "%' and ");

                        //    }
                        //    break;
                        case "txtTehsil":
                            if (!string.IsNullOrEmpty(txtTehsil.Text))
                            {
                                txtWhere.Append("TEHSIL like '%" + txtTehsil.Text + "%' and ");

                            }
                            break;
                        case "txtVillage":
                            if (!string.IsNullOrEmpty(txtVillage.Text))
                            {
                                txtWhere.Append("VILLAGE like '%" + txtVillage.Text + "%' and ");

                            }
                            break;
                        case "txtSyNo":
                            if (!string.IsNullOrEmpty(txtSyNo.Text))
                            {
                                txtWhere.Append("SYNo_KhaasraNo like '%" + txtSyNo.Text + "%' and ");

                            }
                            break;
                        case "txtMode":
                            if (!string.IsNullOrEmpty(txtMode.Text))
                            {
                                txtWhere.Append("MODE_CASH_CHEQUE like '%" + txtMode.Text + "%' and ");

                            }
                            break;
                    }
                }
                if(ddlState.SelectedItem.Text!="--Select--")
                {
                    txtWhere.Append("STATE ='"+ddlState.SelectedItem.Text+"' and ");
                }
                if (ddlDistt.Visible == true)
                {
                    if (ddlDistt.SelectedItem.Text != "--Select--")
                    {
                        txtWhere.Append("DISTT ='" + ddlDistt.SelectedItem.Text + "' and ");
                    }
                }
                
                    str = txtWhere.ToString();
                    // ErrorLog1(@"D:\IFCITypeOfFile", str);
                
                if (txtWhere.Length == 0)
                {
                    dsResult = null;

                }
                else
                {
                    txtWhere = txtWhere.Remove(txtWhere.Length - 5, 5);
                    txtWhere.ToString();
                    //txtWhere.Append(")");
                    //txtWhere.ToString();
                    str = txtWhere.ToString();
                    //ErrorLog1(@"D:\IFCIFullQuery", str);
                    dsResult = DataHelper.ExecuteDataSet(txtWhere.ToString());

                }
                //  UserSession.GridData = dsResult.Tables[0];

                return dsResult;
            }
            catch (Exception ex)
            {
                //ErrorLog1(@"D:\IFCIFieldid", Convert.ToString(FieldId));
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);

                //LogManager.ErrorLog(Utility.LogFilePath, ex);

                LogManager.ErrorLog(Utility.LogFilePath, ex);
                dsResult = null;
                return dsResult;
            }

        }

        protected void gvwDocument_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.ToLower().Trim() == "documentsearch")
                {

                    int intRowIndex = Convert.ToInt32(e.CommandArgument);
                    //UserSession.MetaDataID = Convert.ToInt32(gvwDocument.DataKeys[intRowIndex].Values["MetaDataID"]);
                    strDocumentID = gvwDocument.DataKeys[intRowIndex].Values["DOCUMENTID"].ToString().Trim();
                    //string strStatus = gvwDocument.DataKeys[intRowIndex].Values["DocumentStatusID"].ToString().Trim();
                    Session["DocId"] = strDocumentID;
                    //strMRNO = gvwDocument.DataKeys[intRowIndex].Values["MR_NO"].ToString().Trim();
                    Response.Redirect("../MetaData/DocumentViewer.aspx?DOCID=" + strDocumentID , false);
                    //switch (strStatus)
                    //{
                    //    case "1":
                    //        Response.Redirect("../MetaData/ApprovedDocument.aspx?DOCID=" + strDocumentID, false);
                    //        break;

                    //    case "2":
                    //        Response.Redirect("../MetaData/RejectedDocument.aspx?DOCID=" + strDocumentID, false);
                    //        break;

                    //    case "3":
                    //        Response.Redirect("../MetaData/DocumentEntry.aspx?DOCID=" + strDocumentID, false);
                    //        break;

                    //    case "4":
                    //        Response.Redirect("../MetaData/DocumentVerification.aspx?DOCID=" + strDocumentID, false);
                    //        break;
                    //}
                    //Incrementcount();
                }
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        protected void FillDistrict(string State)
        {
            string str = "select distinct DISTT from PACL_DEEDS where State='"+State+"'";
            DataSet ds = new DataSet();
            ds = DataHelper.ExecuteDataSet(str);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlDistt.DataSource = ds.Tables[0];
                ddlDistt.DataTextField = "DISTT";
                ddlDistt.DataBind();
            }
            ddlDistt.Items.Insert(0, "--Select--");

        }

        protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
        {
           if(ddlState.SelectedItem.Text!="--Select--")
           {
               ddlDistt.Visible = true;
               lblDistt.Visible = true;
               FillDistrict(ddlState.SelectedItem.Text);
           }                                                                                                                        
          // ddlDistt.Items.Insert(0,"--Select--");
        }

        protected void gvwDocument_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                if (UserSession.GridData != null)
                {
                    gvwDocument.PageIndex = e.NewPageIndex;
                    if (UserSession.FilterData == null)
                        gvwDocument.DataSource = UserSession.GridData;
                    else
                        gvwDocument.DataSource = UserSession.FilterData;

                    gvwDocument.DataBind();
                }
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        

    }
}