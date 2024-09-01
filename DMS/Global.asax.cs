using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.IO;
using System.Threading;
using ASPNETChat;
using DMS.BusinessLogic;
using System.Web.Routing;


namespace DMS
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            var routeCollection = RouteTable.Routes;
            routeCollection.MapPageRoute("DefaultRoute", string.Empty, "~/Shared/LoginForm.aspx");
            // Decleared For Chat Applications
            System.Threading.Timer ChatRoomsCleanerTimer = new System.Threading.Timer(new TimerCallback(ChatEngine.CleanChatRooms), null, 1200000, 1200000);

            if(!Directory.Exists(System.Configuration.ConfigurationManager.AppSettings["DMSMain"].ToString()))
                Directory.CreateDirectory(System.Configuration.ConfigurationManager.AppSettings["DMSMain"].ToString());

            if(!Directory.Exists(DMS.BusinessLogic.Utility.DocumentPath))
                Directory.CreateDirectory(DMS.BusinessLogic.Utility.DocumentPath);

            if(!Directory.Exists(DMS.BusinessLogic.Utility.LogFilePath))
                Directory.CreateDirectory(DMS.BusinessLogic.Utility.LogFilePath);

            if(!Directory.Exists(DMS.BusinessLogic.Utility.LuceneFilePath))
                Directory.CreateDirectory(DMS.BusinessLogic.Utility.LuceneFilePath);

            if (!Directory.Exists(DMS.BusinessLogic.Utility.BulkFilePath))
                Directory.CreateDirectory(DMS.BusinessLogic.Utility.BulkFilePath);

            if (!Directory.Exists(DMS.BusinessLogic.Utility.SuccessErrorFilePath))
                Directory.CreateDirectory(DMS.BusinessLogic.Utility.SuccessErrorFilePath);
            
            if (!Directory.Exists(DMS.BusinessLogic.Utility.DMSTiffBackupDocument))
                Directory.CreateDirectory(DMS.BusinessLogic.Utility.DMSTiffBackupDocument);



            if (Application["LoginUsers"] == null)
            {
                System.Data.DataTable objDataTable = new System.Data.DataTable();
                objDataTable.Columns.Add("UserID", typeof(Int32));
                objDataTable.Columns.Add("LastRequest", typeof(DateTime));
                objDataTable.Columns.Add("TotalLoginCount", typeof(Int32)).DefaultValue = 0;
                objDataTable.Columns.Add("MaximumLoginCount", typeof(Int32)).DefaultValue = 0;
                objDataTable.AcceptChanges();
                Application["LoginUsers"] = objDataTable;
            }            
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {
        //    HttpException lastErrorWrapper =
        //Server.GetLastError() as HttpException;
        //    Exception lastError = lastErrorWrapper;
        //    //    if (lastErrorWrapper.InnerException != null)
        //    //        lastError = lastErrorWrapper.InnerException;

        //    string lastErrorTypeName = lastError.GetType().ToString();
        //    string lastErrorMessage = lastError.Message;
        //    string lastErrorStackTrace = lastError.StackTrace;
        }

        protected void Session_End(object sender, EventArgs e)
        {
            try
            {
                if (Session["UserID"] != null)
                {
                    if (((System.Data.DataTable)Application["LoginUsers"]).Select("UserID=" + Convert.ToInt32(Session["UserID"])).Length > 0)
                    {
                        if (Convert.ToInt32(((System.Data.DataTable)Application["LoginUsers"]).Select("UserID=" + Convert.ToInt32(Session["UserID"]))[0]["TotalLoginCount"]) != 0)
                        {
                            ((System.Data.DataTable)Application["LoginUsers"]).Select("UserID=" + Convert.ToInt32(Session["UserID"]))[0]["TotalLoginCount"] = Convert.ToInt32(((System.Data.DataTable)Application["LoginUsers"]).Select("UserID=" + Convert.ToInt32(Session["UserID"]))[0]["TotalLoginCount"]) - 1;
                            if (Convert.ToInt32(((System.Data.DataTable)Application["LoginUsers"]).Select("UserID=" + Convert.ToInt32(Session["UserID"]))[0]["TotalLoginCount"]) == 0)
                            {
                                ((System.Data.DataTable)Application["LoginUsers"]).Select("UserID=" + Convert.ToInt32(Session["UserID"]))[0].Delete();
                            }
                            ((System.Data.DataTable)Application["LoginUsers"]).AcceptChanges();
                        }
                        else
                        {
                            ((System.Data.DataTable)Application["LoginUsers"]).Select("UserID=" + Convert.ToInt32(Session["UserID"]))[0].Delete();
                            ((System.Data.DataTable)Application["LoginUsers"]).AcceptChanges();
                        }
                    }

                    UserManager objUserManager = new UserManager();
                    LoginDetail objLoginDetail = new LoginDetail();
                    objLoginDetail.LoginDetailID = Convert.ToInt32(Session["LoginDetailID"]);
                    objLoginDetail.LogoutTime = DateTime.Now;
                    objLoginDetail.Remark = "Logged Out";
                    objUserManager.UpdateLoginDetail(objLoginDetail);
                }
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}