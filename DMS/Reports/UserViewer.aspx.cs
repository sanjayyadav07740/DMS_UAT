using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using DMS.BusinessLogic;


namespace DMS.Reports
{
    public partial class UserViewer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                UserSession.GridData = null;
                UserSession.FilterData = null;
                Txtfrom.Text = DateTime.Now.ToString("dd/mmm/yyyy");
                Txtto.Text = DateTime.Now.ToString("dd/mm/yyyy");
            }

        }

        protected void btnsubmit_Click(object sender, EventArgs e)
        {
            try
            {

                Convert.ToDateTime(Txtfrom.Text);
                Convert.ToDateTime(Txtto.Text);
            }
            catch(Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), "msg", "alert('Invalid date .');", true);
                return;
            }

            if (Convert.ToDateTime(Txtto) > Convert.ToDateTime(Txtfrom))
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), "msg", "alert('To date must be greater than From date .');", true);
                return;
            }
            ViewState["selectusername"] = null;
            loadgriddata();

        }


        protected void loadusername()
        {
            if (UserSession.GridData != null && UserSession.GridData.Rows.Count > 0)
            {
                
                DataView objView = UserSession.GridData.DefaultView;
                
                objView.Sort = "UserName";

                Ddluserviewerlist.DataSource = objView.ToTable(true, "UserName");
                Ddluserviewerlist.DataTextField = "UserName";
                Ddluserviewerlist.Items.Insert(0,new ListItem("--ALL--","-1"));
                Ddluserviewerlist.DataBind();

                if (ViewState["selectusername"] != null)
                {
                    Ddluserviewerlist.SelectedIndex = Ddluserviewerlist.Items.IndexOf(Ddluserviewerlist.Items.FindByText(ViewState["selectusername"].ToString()));
                }




            }
 
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            DropDownList Ddrlusername = Grvuserviewerlist.FooterRow.FindControl("Ddrlusername") as DropDownList;
            ViewState["selectusername"] = Ddrlusername.SelectedItem.Text;

            if (Ddrlusername.SelectedValue != "-1")
            {
                DataRow[] objdatarow = UserSession.GridData.Select("UserName", Ddrlusername.SelectedItem.Text);

                if (objdatarow.Count() == 0)
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "msg", "alert('No Data Found .');", true);
                    return;

                }
                UserSession.FilterData = objdatarow.CopyToDataTable();
                Grvuserviewerlist.DataSource = UserSession.FilterData;
                Grvuserviewerlist.DataBind();
                loadusername();

                
             }
          

            else
            {
                UserSession.FilterData = null;
                Grvuserviewerlist.DataSource = UserSession.GridData;
                Grvuserviewerlist.DataBind();
                loadusername();
            }
            

          
        }

        protected void loadgriddata()
        {
            SqlConnection con = new SqlConnection(Utility.ConnectionString);

            string query = "select UserName from User inner join on UserViewer User.UserID=UserViewer.UserID";

            SqlDataAdapter adp = new SqlDataAdapter(query,con);
            DataTable td = new DataTable();
            adp.Fill(td);

            UserSession.GridData=td;
            UserSession.FilterData=null;
            Grvuserviewerlist.DataSource = UserSession.GridData;
            Grvuserviewerlist.DataBind();
            loadusername();

            
        }

        protected void Grvuserviewerlist_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

            if (UserSession.GridData != null)
            {
                DropDownList Ddrlusername = Grvuserviewerlist.FooterRow.FindControl("Ddrlusername") as DropDownList;
                Grvuserviewerlist.PageIndex = e.NewPageIndex;
                Grvuserviewerlist.DataBind();
                loadusername();
            }
        }

      



    }

   
}