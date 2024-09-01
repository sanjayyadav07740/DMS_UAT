using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DMS.Handler
{
    /// <summary>
    /// Summary description for ImageHandler1
    /// </summary>
    public class ImageHandler1 : IHttpHandler, System.Web.SessionState.IReadOnlySessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/pdf";
            context.Response.AddHeader("content-disposition", "inline; filename=" + context.Session["DocumentName"].ToString());
            context.Response.BinaryWrite(UserSession.FileByte);
            context.Response.Flush();
            context.Response.End();
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}