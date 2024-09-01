using DMS.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DMS.Shared
{
    public class FirstChartHome
    {
        public String x { get; set; }
        public int y { get; set; }
    }
   
    public partial class NewDashBoard : System.Web.UI.Page
    {
        Utility objUtility = new Utility();
        DocumentManager objDocumentManager = new DocumentManager();
       // DMS.BusinessLogic.Control.WorkFlowManager objWorkFlowManager = new BusinessLogic.Control.WorkFlowManager();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                   // InprogressGridBinding();
                    RecentDocumentGridBinding();
                    CountDocument();

                    int id = UserSession.RoleID;
                    if (id == 1)
                    {
                        DivCalendar.Visible = true;
                        DivDocChart.Visible = true;
                        //string RoleName = UserSession.RoleName;
                    }
                    Log.AuditLog(HttpContext.Current, "Visit", "DashBoard");
                }
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
            
        }

        private void InprogressGridBinding()
        {
            try
            {               
                DataTable objDataTable = new DataTable();
               // objUtility.Result = objWorkFlowManager.LoadInprogressDocument(out objDataTable, "TopTenWorkSpace", Convert.ToInt32(UserSession.RoleID), Convert.ToInt32(UserSession.UserID));
                if (objDataTable.Rows.Count > 0)
                {
                    gvInprogressDocument.DataSource = objDataTable;
                    gvInprogressDocument.DataBind();

                }
            }
            catch (Exception ex)
            {

                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        private void RecentDocumentGridBinding()
        {
            try
            {
                DataTable objDataTable = new DataTable();
                objUtility.Result = objDocumentManager.LoadRecentDocument(out objDataTable, "RecentlyUploaded", Convert.ToInt32(UserSession.RoleID), Convert.ToInt32(UserSession.UserID));
                if (objDataTable.Rows.Count > 0)
                {
                    gvRecentDocument.DataSource = objDataTable;
                    gvRecentDocument.DataBind();

                }
            }
            catch (Exception ex)
            {

                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }
        
        private void CountDocument()
        {
            DataTable objDataTable = new DataTable();
            objUtility.Result = objDocumentManager.LoadCountDocumnet(out objDataTable,  Convert.ToInt32(UserSession.RoleID), Convert.ToInt32(UserSession.UserID));
            DataRowCollection drc = objDataTable.Rows;
            if(drc.Count > 0)
            {
                DataRow dr = drc[0];
                lblNewDoc.Text = dr["RecentDocument"].ToString();
                lbltotalDoc.Text = dr["AllDocument"].ToString();
                lblPendingDoc.Text = dr["PendingDocument"].ToString();
                lblUploadDoc.Text = Convert.ToString( dr["CompletedEntry"]);
            }

           
        }

        protected void gvInprogressDocument_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string DocumentName = Convert.ToString(gvInprogressDocument.DataKeys[e.Row.RowIndex].Values["DocumentName"]);
                string DocumentType = Convert.ToString(gvInprogressDocument.DataKeys[e.Row.RowIndex].Values["Documenttype"]);

                System.Web.UI.WebControls.Image image = (System.Web.UI.WebControls.Image)e.Row.FindControl("imgIcon");

                if (DocumentType.ToLower().Contains("pdf"))
                    image.ImageUrl = "../Images/Icon-pdf.png";
                else
                    if (DocumentType.ToLower().Contains("doc") || DocumentType.ToLower().Contains("docx"))
                        image.ImageUrl = "../Images/Icon-dox.png";
                    else
                        if (DocumentType.ToLower().Contains("xls") || DocumentType.ToLower().Contains("xlsx"))
                            image.ImageUrl = "../Images/Icon-xlsx.png";
                        else
                            if (DocumentType.ToLower().Contains("pdf"))
                                image.ImageUrl = "../Images/Icon-pdf.png";
                            else
                                if (DocumentType.ToLower().Contains("fpx"))
                                    image.ImageUrl = "../Images/Icon-fpx.png";
                                else
                                    if (DocumentType.ToLower().Contains("pcd"))
                                        image.ImageUrl = "../Images/Icon-pcd.png";
                                    else
                                        if (DocumentType.ToLower().Contains("tif") || DocumentType.ToLower().Contains("tiff"))
                                            image.ImageUrl = "../Images/Icon-tif.png";
                                        else
                                            if (DocumentType.ToLower().Contains("jpeg") || DocumentType.ToLower().Contains("jpg") || DocumentType.ToLower().Contains("bmp") || DocumentType.ToLower().Contains("gif"))
                                                image.ImageUrl = "../Images/Icon-jpeg.png";
                                            else
                                                if (DocumentType.ToLower().Contains("mp3"))
                                                    image.ImageUrl = "../Images/Icon-Mp3.png";
                                                else
                                                    if (DocumentType.ToLower().Contains("ppt") || DocumentType.ToLower().Contains("pptx"))
                                                        image.ImageUrl = "../Images/Icon-pptx.png";
                                                    else
                                                        if (DocumentType.Contains("mp4") || DocumentType.Contains("avi") || DocumentType.Contains("flv") || DocumentType.Contains("wmv") || DocumentType.Contains("mov"))
                                                            image.ImageUrl = "../Images/Icon-Video.png";
                                                        else
                                                            if (DocumentType.Contains("zip") || DocumentType.Contains("rar") || DocumentType.Contains("tar") || DocumentType.Contains("iso"))
                                                                image.ImageUrl = "../Images/Icon-zip.png";
                                                            else
                                                                if (DocumentType.ToLower().Contains("png"))
                                                                    image.ImageUrl = "../Images/Icon-png.png";
                                                                else
                                                                image.ImageUrl = "../Images/Icon-other.png";                                             
                                                             
            }
        }

        protected void gvInprogressDocument_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int strDocumentID = 0;
            string strDocumentPath = string.Empty;
            int intRowIndex = Convert.ToInt32(e.CommandArgument);

             if (e.CommandName == "View")
             {
                 strDocumentID = Convert.ToInt32(gvInprogressDocument.DataKeys[intRowIndex].Values["ID"]);
                 strDocumentPath = (gvInprogressDocument.DataKeys[intRowIndex].Values["DocumentPath"]).ToString();
                 Session["DocID"] = strDocumentID;
                 //UserSession.workspacepopup(this, strDocumentID, strDocumentPath);
                 Response.Redirect("~/MetaData/ViewDocumentEntry.aspx?DocId=" + strDocumentID, false);
            }
        }

        protected void gvRecentDocument_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string DocumentName = Convert.ToString(gvRecentDocument.DataKeys[e.Row.RowIndex].Values["DocumentName"]);
                string DocumentType = Convert.ToString(gvRecentDocument.DataKeys[e.Row.RowIndex].Values["Documenttype"]);

                System.Web.UI.WebControls.Image image = (System.Web.UI.WebControls.Image)e.Row.FindControl("imgIcon");

                if (DocumentType.ToLower().Contains("pdf"))
                    image.ImageUrl = "../Images/Icon-pdf.png";
                else
                    if (DocumentType.ToLower().Contains("doc") || DocumentType.ToLower().Contains("docx"))
                        image.ImageUrl = "../Images/Icon-dox.png";
                    else
                        if (DocumentType.ToLower().Contains("xls") || DocumentType.ToLower().Contains("xlsx"))
                            image.ImageUrl = "../Images/Icon-xlsx.png";
                        else
                            if (DocumentType.ToLower().Contains("pdf"))
                                image.ImageUrl = "../Images/Icon-pdf.png";
                            else
                                if (DocumentType.ToLower().Contains("fpx"))
                                    image.ImageUrl = "../Images/Icon-fpx.png";
                                else
                                    if (DocumentType.ToLower().Contains("pcd"))
                                        image.ImageUrl = "../Images/Icon-pcd.png";
                                    else
                                        if (DocumentType.ToLower().Contains("tif") || DocumentType.ToLower().Contains("tiff"))
                                            image.ImageUrl = "../Images/Icon-tif.png";
                                        else
                                            if (DocumentType.ToLower().Contains("jpeg") || DocumentType.ToLower().Contains("jpg") ||                       DocumentType.ToLower().Contains("bmp") || DocumentType.ToLower().Contains("gif"))
                                                image.ImageUrl = "../Images/Icon-jpeg.png";
                                            else
                                                if (DocumentType.ToLower().Contains("mp3"))
                                                    image.ImageUrl = "../Images/Icon-Mp3.png";
                                                else
                                                    if (DocumentType.ToLower().Contains("ppt") || DocumentType.ToLower().Contains("pptx"))
                                                        image.ImageUrl = "../Images/Icon-pptx.png";
                                                    else
                                                        if (DocumentType.Contains("mp4") || DocumentType.Contains("avi") ||                                             DocumentType.Contains("flv") || DocumentType.Contains("wmv") ||                                             DocumentType.Contains("mov"))
                                                            image.ImageUrl = "../Images/Icon-Video.png";
                                                        else
                                                            if (DocumentType.Contains("zip") || DocumentType.Contains("rar") ||                                         DocumentType.Contains("tar") || DocumentType.Contains("iso"))
                                                                image.ImageUrl = "../Images/Icon-zip.png";
                                                            else
                                                                if (DocumentType.ToLower().Contains("png"))
                                                                    image.ImageUrl = "../Images/Icon-png.png";
                                                                else
                                                                image.ImageUrl = "../Images/Icon-other.png";

            }
        }

        [WebMethod]
        public static string monthwisechart()
        {
            try
            {
                DataTable dtSQL = new DataTable();
                Document.MonthlyChart(out dtSQL, Convert.ToString(UserSession.RoleID), Convert.ToInt32(UserSession.UserID));

                string a = "";
                DataTable dt = new DataTable();
                dt.Clear();
                dt.Columns.Add("x");
                dt.Columns.Add("y");
                DataRow dr = dt.NewRow();
                dr["x"] = "0";
                dr["y"] = "January";
                dt.Rows.Add(dr);
                dr = dt.NewRow();
                dr["x"] = "0";
                dr["y"] = "February";
                dt.Rows.Add(dr);
                dr = dt.NewRow();
                dr["x"] = "0";
                dr["y"] = "March";
                dt.Rows.Add(dr); dr = dt.NewRow();
                dr["x"] = "0";
                dr["y"] = "April";
                dt.Rows.Add(dr);
                dr = dt.NewRow();
                dr["x"] = "0";
                dr["y"] = "May";
                dt.Rows.Add(dr); dr = dt.NewRow();
                dr["x"] = "0";
                dr["y"] = "June";
                dt.Rows.Add(dr); dr = dt.NewRow();
                dr["x"] = "0";
                dr["y"] = "July";
                dt.Rows.Add(dr); dr = dt.NewRow();
                dr["x"] = "0";
                dr["y"] = "August";
                dt.Rows.Add(dr); dr = dt.NewRow();
                dr["x"] = "0";
                dr["y"] = "September";
                dt.Rows.Add(dr); dr = dt.NewRow();
                dr["x"] = "0";
                dr["y"] = "October";
                dt.Rows.Add(dr); dr = dt.NewRow();
                dr["x"] = "0";
                dr["y"] = "November";
                dt.Rows.Add(dr); dr = dt.NewRow();
                dr["x"] = "0";
                dr["y"] = "December"; dt.Rows.Add(dr);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    foreach (DataRow drSQL in dtSQL.Rows)
                        if (Convert.ToString(dt.Rows[i].ItemArray[1]) == Convert.ToString(drSQL.ItemArray[1]))
                            dt.Rows[i][0] = drSQL.ItemArray[0];
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    a += "," + dt.Rows[i]["y"] + "," + dt.Rows[i]["x"].ToString();

                }
                return a.Substring(1, a.Length - 1);

            }
            catch (Exception ex)
            {
                return "error";
            }
        }

        [WebMethod]
        public static string Metatemplatewisechart(string year)
        {
            try
            {
                DataTable dtSQL = new DataTable();
                Document.Metatemplatewisechart(out dtSQL, Convert.ToString(UserSession.RoleID), Convert.ToInt32(UserSession.UserID), year == null ? DateTime.Now.Year.ToString() : year);


                string a = "";

                for (int i = 0; i < dtSQL.Rows.Count; i++)
                {
                    a += "," + dtSQL.Rows[i]["y"] + "," + dtSQL.Rows[i]["x"].ToString();

                }
                return a.Substring(1, a.Length - 1);

            }
            catch (Exception ex)
            {
                return "No Data To Display";
            }
        }

        [WebMethod]
        public static List<string> GetYearsforDropdown()
        {
            try
            {
                List<string> years = new List<string>();
                DataTable dtSQL = new DataTable();
                Document.GetYears(out dtSQL, Convert.ToString(UserSession.RoleID), Convert.ToInt32(UserSession.UserID));
                for (int i = 0; i < dtSQL.Rows.Count; i++)
                {
                    years.Add(dtSQL.Rows[i]["Years"].ToString());
                }

                return years;

            }
            catch (Exception ex)
            {
                return new List<string>();
            }
        }

        #region Bind  Calendar data

        public class CalendarEvents
        {
            public Int64 EventId { get; set; }

            public DateTime EventStartDate { get; set; }

            public DateTime EventEndDate { get; set; }

            public string EventDescription { get; set; }

            public string Title { get; set; }

            public bool AllDayEvent { get; set; }
        }

       
        [WebMethod]
        public static List<CalendarEvents> GetCalendarData(DateTime StartDate, DateTime EndDate)
        {

            string constr = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("SP_Select_Document_ByDate"))
                {
                    List<CalendarEvents> cData = new List<CalendarEvents>();
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.AddWithValue("@iUserId", Convert.ToInt32(HttpContext.Current.Request.Cookies["leaduser"]["leaduserid"].ToString()));
                    cmd.Parameters.AddWithValue("@StartDate", StartDate);
                    cmd.Parameters.AddWithValue("@EndDate", EndDate);
                    cmd.Parameters.AddWithValue("@UserID", UserSession.UserID);
                    cmd.Parameters.AddWithValue("@RoleID", UserSession.RoleID);
                    cmd.Connection = con;

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet dsUser = new DataSet();
                    da.Fill(dsUser);
                    con.Close();
                    for (int i = 0; i < dsUser.Tables[0].Rows.Count; i++)
                    {
                        int year = Convert.ToDateTime(dsUser.Tables[0].Rows[i]["CreatedDate"].ToString()).Year;
                        int month = Convert.ToDateTime(dsUser.Tables[0].Rows[i]["CreatedDate"].ToString()).Month;
                        int day = Convert.ToDateTime(dsUser.Tables[0].Rows[i]["CreatedDate"].ToString()).Day;
                        int H = Convert.ToDateTime(dsUser.Tables[0].Rows[i]["CreatedDate"].ToString()).Hour;
                        int M = Convert.ToDateTime(dsUser.Tables[0].Rows[i]["CreatedDate"].ToString()).Minute;
                        cData.Add(new CalendarEvents
                        {
                            // EventId = Convert.ToInt32(dsUser.Tables[0].Rows[i]["iLeadId"].ToString()),
                            //EventStartDate = new DateTime(2018,8,8,9,0,0),
                            //EventEndDate = new DateTime(2018,8,8,9,0,0),

                            EventStartDate = new DateTime(year, month, day, H, M, 0),
                            EventEndDate = new DateTime(year, month, day, H, M, 0),
                            Title = dsUser.Tables[0].Rows[i]["Counts"].ToString(),
                            EventDescription = "Total Documents :" + dsUser.Tables[0].Rows[i]["Counts"].ToString(),
                            AllDayEvent = false,

                        });

                    }
                    return cData;
                }

            }
        }

        #endregion

        protected void gvInprogressDocument_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvInprogressDocument.PageIndex = e.NewPageIndex;
            InprogressGridBinding();
            DivCalendar.Visible = true;
        }
     
    }
}