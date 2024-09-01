using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DMS.BusinessLogic;
using System.Drawing;
using System.Drawing.Imaging;

namespace DMS.Handler
{
    /// <summary>
    /// Summary description for CaptchaHandler
    /// </summary>
    public class CaptchaHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            if(context.Request["Code"]!=null)
            {
               Bitmap objBitmap = Utility.MakeCaptchaImage(context.Request["Code"].ToString().Trim(), 120, 30, "Arial");
               objBitmap.Save(context.Response.OutputStream,ImageFormat.Jpeg);
               objBitmap.Dispose();
               context.Response.Flush();
            }
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