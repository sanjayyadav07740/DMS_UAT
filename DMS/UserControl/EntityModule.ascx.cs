using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DMS.BusinessLogic;
using System.Data.SqlClient;

namespace DMS.UserControl
{
    public partial class EntityModule : System.Web.UI.UserControl
    {
        #region Private Memeber
        Utility objUtility = new Utility();
        bool boolDisplayCategory = true;
        bool boolDisplayFolder = true;
        bool boolDisplayMetaTemplate = true;
        bool boolDisplayMetaDataCode = false;
        SqlConnection con = new SqlConnection(Utility.ConnectionString);
        #endregion

        #region Properties
        public int SelectedRepository
        {
            get
            {
                return Convert.ToInt32(ddlRepositoryName.SelectedValue);
            }
        }

        public int SelectedMetaTemplate
        {
            get
            {
                return Convert.ToInt32(ddlMetaTemplateName.SelectedValue);
            }
        }

        public int SelectedCategory
        {
            get
            {
                return Convert.ToInt32(ddlCategoryName.SelectedValue);
            }
        }

        public int SelectedFolder
        {
            get
            {
                return Convert.ToInt32(tvwFolder.SelectedValue);
            }
        }

        public int SelectedMetaDataCode
        {
            get
            {
                return Convert.ToInt32(ddlMetaDataCode.SelectedValue);
            }
        }

        public bool DisplayCategory
        {
            get
            {
                return boolDisplayCategory;
            }
            set
            {
                boolDisplayCategory = value;
            }
        }

        public bool DisplayFolder
        {
            get
            {
                return boolDisplayFolder;
            }
            set
            {
                boolDisplayFolder = value;
            }
        }

        public bool DisplayMetaTemplate
        {
            get
            {
                return boolDisplayMetaTemplate;
            }
            set
            {
                boolDisplayMetaTemplate = value;
            }
        }

        public bool DisplayMetaDataCode
        {
            get
            {
                return boolDisplayMetaDataCode;
            }
            set
            {
                boolDisplayMetaDataCode = value;
            }
        }
        #endregion

        #region Event
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    if(UserSession.RoleID==146 || UserSession.RoleID==147)
                    {
                        lblMetaDataCode.Visible = false;
                        ddlMetaDataCode.Visible = false;
                    }
                    else
                    {
                        lblMetaDataCode.Visible = true;
                        ddlMetaDataCode.Visible = true;
                    }
                    if (UserSession.UserSelectionInformation == null)
                    {
                        objUtility.Result = Utility.LoadRepository(ddlRepositoryName);
                        ddlMetaTemplateName.Items.Insert(0, new ListItem("--SELECT--", "-1"));
                        ddlCategoryName.Items.Insert(0, new ListItem("--SELECT--", "-1"));
                        ddlMetaDataCode.Items.Insert(0, new ListItem("--SELECT--", "-1"));
                        TreeNode objTreeNode = new TreeNode("--NONE--", "0");
                        objTreeNode.Selected = true;
                        tvwFolder.Nodes.Add(objTreeNode);

                        if (ddlRepositoryName.SelectedValue == "-1" && ddlRepositoryName.Items.Count > 1)
                        {
                            ddlRepositoryName.SelectedIndex = 1;
                            Utility.LoadMetaTemplate(ddlMetaTemplateName, Convert.ToInt32(ddlRepositoryName.SelectedItem.Value));
                        }

                        switch (objUtility.Result)
                        {
                            case Utility.ResultType.Error:
                                UserSession.DisplayMessage((Page)this.Parent.Page, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                                break;
                        }
                    }
                    else
                    {
                        objUtility.Result = Utility.LoadRepository(ddlRepositoryName);
                        ddlRepositoryName.SelectedIndex = ddlRepositoryName.Items.IndexOf(ddlRepositoryName.Items.FindByValue(GetPropertiesValue("SelectedRepository")));

                        if (boolDisplayMetaTemplate == true)
                        {
                            objUtility.Result = Utility.LoadMetaTemplate(ddlMetaTemplateName, Convert.ToInt32(ddlRepositoryName.SelectedItem.Value));
                            ddlMetaTemplateName.SelectedIndex = ddlMetaTemplateName.Items.IndexOf(ddlMetaTemplateName.Items.FindByValue(GetPropertiesValue("SelectedMetaTemplate")));
                        }

                        if (boolDisplayCategory == true)
                        {
                            objUtility.Result = Utility.LoadCategory(ddlCategoryName, Convert.ToInt32(ddlMetaTemplateName.SelectedItem.Value));
                            ddlCategoryName.SelectedIndex = ddlCategoryName.Items.IndexOf(ddlCategoryName.Items.FindByValue(GetPropertiesValue("SelectedCategory")));
                        }

                        if (boolDisplayFolder == true)
                        {
                            objUtility.Result = Utility.LoadFolder(tvwFolder, Convert.ToInt32(ddlMetaTemplateName.SelectedItem.Value));
                            TreeNode objTreeNode = tvwFolder.FindNode(GetPropertiesValue("SelectedFolder"));
                            if(objTreeNode!=null)
                                objTreeNode.Selected = true;
                        }

                        if (boolDisplayMetaDataCode == true)
                        {
                            BusinessLogic.MetaData objMetaData = new BusinessLogic.MetaData()
                            {
                                RepositoryID = Convert.ToInt32(ddlRepositoryName.SelectedItem.Value),
                                MetaTemplateID = Convert.ToInt32(ddlMetaTemplateName.SelectedItem.Value),
                                CategoryID = Convert.ToInt32(ddlCategoryName.SelectedItem.Value),
                                FolderID = Convert.ToInt32(tvwFolder.SelectedNode.Value)
                            };
                            objUtility.Result = Utility.LoadMetaDataCode(ddlMetaDataCode, objMetaData);
                            ddlMetaDataCode.SelectedIndex = ddlMetaDataCode.Items.IndexOf(ddlMetaDataCode.Items.FindByValue(GetPropertiesValue("SelectedMetaDataCode")));
                        }
                    }
                }
                if (UserSession.RoleID == 146 || UserSession.RoleID == 147)
                {
                    //boolDisplayMetaDataCode = false;
                    ddlMetaDataCode.Visible = false;
                    lblMetaDataCode.Visible = false;
                }
                else
                {
                    ddlMetaDataCode.Visible = true;
                    lblMetaDataCode.Visible = true;
                }
                if (boolDisplayCategory == false)
                {
                    lblCategoryName.Visible = false;
                    ddlCategoryName.Visible = false;
                    uprsProgressCategory.Visible = false;
                }
                else
                {
                    lblCategoryName.Visible = true;
                    ddlCategoryName.Visible = true;
                    uprsProgressCategory.Visible = false;
                }

                if (boolDisplayFolder == false)
                {
                    panFolder.Visible = false;
                    lblSelectFolderName.Visible = false;
                    uprsProgressFolder.Visible = false;
                }
                else 
                {
                    panFolder.Visible = true;
                    lblSelectFolderName.Visible = true;
                    uprsProgressFolder.Visible = false;
                }

                if (boolDisplayMetaTemplate == false)
                {
                    ddlMetaTemplateName.Visible = false;
                    lblMetaTemplateName.Visible = false;
                    rfvMetaTemplateName.Enabled = false;
                    uprsProgressMetaTemplateName.Visible = false;
                }
                else
                {
                    ddlMetaTemplateName.Visible = true;
                    lblMetaTemplateName.Visible = true;
                    rfvMetaTemplateName.Enabled = true;
                    uprsProgressMetaTemplateName.Visible = false;
                }

                if (boolDisplayMetaDataCode == false)
                {
                    ddlMetaDataCode.Visible = false;
                    lblMetaDataCode.Visible = false;
                    uprsProgressMetaDataCode.Visible = false;
                    ddlCategoryName.AutoPostBack=false;
                }
                else
                {
                    ddlMetaDataCode.Visible = true;
                    lblMetaDataCode.Visible = true;
                    uprsProgressMetaDataCode.Visible = false;
                    ddlCategoryName.AutoPostBack = true;
                }
            }
            catch (Exception ex)
            {

                UserSession.DisplayMessage((Page)this.Parent.Page, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
            
        }

        protected void ddlRepositoryName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (boolDisplayMetaTemplate == true)
                {
                    objUtility.Result = Utility.LoadMetaTemplate(ddlMetaTemplateName, Convert.ToInt32(ddlRepositoryName.SelectedItem.Value));

                    ddlCategoryName.Items.Clear();
                    ddlCategoryName.Items.Insert(0, new ListItem("--NONE--", "-1"));
                    tvwFolder.Nodes.Clear();
                    TreeNode objTreeNode = new TreeNode("--NONE--", "0");
                    objTreeNode.Selected = true;
                    tvwFolder.Nodes.Add(objTreeNode);

                    switch (objUtility.Result)
                    {
                        case Utility.ResultType.Error:
                            UserSession.DisplayMessage((Page)this.Parent.Page, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                            break;
                    }
                }
                if (boolDisplayMetaDataCode == true)
                {
                    ddlMetaDataCode.Items.Clear();
                    ddlMetaDataCode.Items.Insert(0, new ListItem("--SELECT--", "-1"));
                }
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage((Page)this.Parent.Page, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }

        }

        protected void ddlMetaTemplateName_SelectedIndexChanged(object sender, EventArgs e)
        {
        #region oldcode
            try
            {
                             
                if (boolDisplayMetaDataCode == true)
                {
                    BusinessLogic.MetaData objMetaData = new BusinessLogic.MetaData()
                    {
                        RepositoryID = Convert.ToInt32(ddlRepositoryName.SelectedItem.Value),
                        MetaTemplateID = Convert.ToInt32(ddlMetaTemplateName.SelectedItem.Value),
                        CategoryID = Convert.ToInt32(ddlCategoryName.SelectedItem.Value),
                        FolderID = Convert.ToInt32(tvwFolder.SelectedNode.Value)
                    };
                    objUtility.Result = Utility.LoadMetaDataCode(ddlMetaDataCode, objMetaData);
                    if (objUtility.Result == Utility.ResultType.Error)
                    {
                        UserSession.DisplayMessage((Page)this.Parent.Page, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                    }
                }
            }
            #endregion
          
            catch (Exception ex)
            {
                UserSession.DisplayMessage((Page)this.Parent.Page, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }

            try
            {
                if (boolDisplayCategory == true)               
                {
                    objUtility.Result = Utility.LoadCategory(ddlCategoryName, Convert.ToInt32(ddlMetaTemplateName.SelectedItem.Value));

                    switch (objUtility.Result)
                    {
                        case Utility.ResultType.Error:
                            UserSession.DisplayMessage((Page)this.Parent.Page, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                            break;
                    }
                }
            // For checking category is exist for respected metatemplate
               if(objUtility.Result == Utility.ResultType.Success)
                
                {
                    // if category is not available(not mapped) in folder table 
                 
                    //string strquery = "select * from folder where MetaTemplateID ='" + Convert.ToInt32(ddlMetaTemplateName.SelectedItem.Value) + "' and CategoryID is null  and status=1";

                    string strquery = "select * from folder where MetaTemplateID ='" + Convert.ToInt32(ddlMetaTemplateName.SelectedItem.Value) + "' and CategoryID in(-1,null) and status=1";

                    SqlCommand cmd = new SqlCommand(strquery, con);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    con.Open();
                    da.Fill(dt);
                    con.Close();
                   
                    if (dt.Rows.Count > 0)
                    {
                        objUtility.Result = Utility.LoadFolder(tvwFolder, Convert.ToInt32(ddlMetaTemplateName.SelectedItem.Value));

                        switch (objUtility.Result)
                        {
                            case Utility.ResultType.Error:
                                UserSession.DisplayMessage((Page)this.Parent.Page, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                                break;
                        }
                    }
                   
                }
               // if category is not  exist for respected metatemplate
                else
                {
                    objUtility.Result = Utility.LoadFolder(tvwFolder, Convert.ToInt32(ddlMetaTemplateName.SelectedItem.Value));

                    switch (objUtility.Result)
                    {
                        case Utility.ResultType.Error:
                            UserSession.DisplayMessage((Page)this.Parent.Page, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                            break;
                    }
                }
               

              
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage((Page)this.Parent.Page, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }

        }

        protected void ddlCategoryName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                // If category select then folder display
                #region Seema 9 NOv 2017 Axis Trustee
               
                if(Convert.ToInt32(ddlCategoryName.SelectedItem.Value)>0)
                {
                    string strquery="select * from folder where MetaTemplateID ='"+Convert.ToInt32(ddlMetaTemplateName.SelectedItem.Value)+"' and CategoryID is null and status=1";
                     SqlCommand cmd = new SqlCommand(strquery, con);
                     SqlDataAdapter da = new SqlDataAdapter(cmd);
                      DataTable dt = new DataTable();
                       con.Open();
                      da.Fill(dt);
                     con.Close();
             
                    if(dt.Rows.Count>0)
                    {
                        objUtility.Result = Utility.LoadFolder(tvwFolder, Convert.ToInt32(ddlMetaTemplateName.SelectedItem.Value));

                        switch (objUtility.Result)
                        {
                            case Utility.ResultType.Error:
                                UserSession.DisplayMessage((Page)this.Parent.Page, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                                break;
                        }
                    }
                    else
                    {
                        objUtility.Result = Utility.LoadFolder_AxisTree(tvwFolder, Convert.ToInt32(ddlMetaTemplateName.SelectedItem.Value), Convert.ToInt32(ddlCategoryName.SelectedItem.Value));
                       

                        switch (objUtility.Result)
                        {
                            case Utility.ResultType.Error:
                                UserSession.DisplayMessage((Page)this.Parent.Page, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                                break;
                        }
                    }
                  
                }
               
                   

                #endregion
                if (boolDisplayMetaDataCode == true)
                {
                    BusinessLogic.MetaData objMetaData = new BusinessLogic.MetaData()
                    {
                        RepositoryID = Convert.ToInt32(ddlRepositoryName.SelectedItem.Value),
                        MetaTemplateID = Convert.ToInt32(ddlMetaTemplateName.SelectedItem.Value),
                        CategoryID = Convert.ToInt32(ddlCategoryName.SelectedItem.Value),
                        FolderID = Convert.ToInt32(tvwFolder.SelectedNode.Value)
                    };
                    objUtility.Result = Utility.LoadMetaDataCode(ddlMetaDataCode, objMetaData);
                    if (objUtility.Result == Utility.ResultType.Error)
                    {
                        UserSession.DisplayMessage(this.Parent.Page, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this.Parent.Page, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        protected void tvwFolder_SelectedNodeChanged(object sender, EventArgs e)
        {
            try
            {
                if (boolDisplayMetaDataCode == false)
                {
                    BusinessLogic.MetaData objMetaData = new BusinessLogic.MetaData()
                    {
                        RepositoryID = Convert.ToInt32(ddlRepositoryName.SelectedItem.Value),
                        MetaTemplateID = Convert.ToInt32(ddlMetaTemplateName.SelectedItem.Value),
                      //  CategoryID = Convert.ToInt32(ddlCategoryName.SelectedItem.Value),
                        FolderID = Convert.ToInt32(tvwFolder.SelectedNode.Value)
                    };
                    objUtility.Result = Utility.LoadMetaDataCode(ddlMetaDataCode, objMetaData);
                    if (objUtility.Result == Utility.ResultType.Error)
                    {
                        UserSession.DisplayMessage(this.Parent.Page, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this.Parent.Page, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            UserSession.UserSelectionInformation = new
            {
                SelectedRepository = Convert.ToInt32(ddlRepositoryName.SelectedValue == null || ddlRepositoryName.SelectedValue.Trim() == string.Empty ? "-1" : ddlRepositoryName.SelectedValue),
                SelectedMetaTemplate = Convert.ToInt32(ddlMetaTemplateName.SelectedValue == null || ddlMetaTemplateName.SelectedValue.Trim() == string.Empty ? "-1" : ddlMetaTemplateName.SelectedValue),
                SelectedCategory = Convert.ToInt32(ddlCategoryName.SelectedValue == null || ddlCategoryName.SelectedValue.Trim() == string.Empty ? "-1" : ddlCategoryName.SelectedValue),
                SelectedFolder = tvwFolder.SelectedNode == null || tvwFolder.SelectedNode.ValuePath ==string.Empty? "0" : tvwFolder.SelectedNode.ValuePath,
                SelectedMetaDataCode = Convert.ToInt32(ddlMetaDataCode.SelectedValue == null||ddlMetaDataCode.SelectedValue.Trim() == string.Empty? "-1" : ddlMetaDataCode.SelectedValue)
            };
            base.OnPreRender(e);
        }
        #endregion

        #region Method
        public static string GetPropertiesValue(string strPropertyName)
        {
            return UserSession.UserSelectionInformation.GetType().GetProperty(strPropertyName).GetValue(UserSession.UserSelectionInformation, null).ToString();
        }
        #endregion

        public DataTable DataSource { get; set; }
    }
}