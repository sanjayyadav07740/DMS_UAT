using DMS.BusinessLogic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace DMS.Handler
{
    /// <summary>
    /// Summary description for PDFHandler
    /// </summary>
    public class PDFHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                if (context.Request["DOCID"] != null)
                {
                    string strFilePath = "";
                    int intDocumentID = Convert.ToInt32(context.Request["DOCID"]);
                    string strQuery = "SELECT DOCUMENTPATH FROM Document WHERE ID = " + intDocumentID;
                    object objDocumentPath = DataHelper.ExecuteScalar(strQuery, null);
                    if (objDocumentPath != null)
                    {
                        strFilePath = objDocumentPath.ToString();
                    }



                    if (File.Exists(strFilePath))
                    {
                        context.Response.ContentType = "application/pdf";
                        if (context.Request["FROM"] != null && context.Request["TO"] != null)
                        {
                            System.IO.MemoryStream objStream = new System.IO.MemoryStream();
                            iTextSharp.text.pdf.PdfReader objReader = new iTextSharp.text.pdf.PdfReader(strFilePath);
                            iTextSharp.text.Document objDocument = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 0, 0, 0, 0);
                            iTextSharp.text.pdf.PdfCopy objCopy = new iTextSharp.text.pdf.PdfCopy(objDocument, objStream);
                            objDocument.Open();
                            if (context.Request["FROM"] != "0" && context.Request["TO"] != "0")
                            {
                                int StartPage = Convert.ToInt32(context.Request["FROM"]) == 0 ? 1 : Convert.ToInt32(context.Request["FROM"]);
                                int EndPage = Convert.ToInt32(context.Request["TO"]) == 0 ? 1 : Convert.ToInt32(context.Request["TO"]);
                                for (int i = StartPage; i <= EndPage; i++)
                                {
                                    objCopy.AddPage(objCopy.GetImportedPage(objReader, i));
                                }
                            }
                            else
                            {
                                for (int i = 1; i <= objReader.NumberOfPages; i++)
                                    objCopy.AddPage(objCopy.GetImportedPage(objReader, i));
                            }
                            objDocument.Close();
                            objReader.Close();
                            context.Response.BinaryWrite(objStream.ToArray());
                        }
                        else
                        {
                            // context.Response.BinaryWrite(File.ReadAllBytes(strFilePath));
                            context.Response.BinaryWrite(ReadAllBytes(strFilePath));
                        }
                        context.Response.Flush();
                        // context.Response.End();
                    }
                    else
                    {
                        context.Response.ContentType = "application/pdf";
                        context.Response.BinaryWrite(File.ReadAllBytes(context.Server.MapPath("~/Others/FileNotFound.pdf")));
                        context.Response.Flush();
                        context.Response.End();
                    }
                }
            }

            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }


        public byte[] ReadAllBytes(string fileName)
        {
            byte[] buffer = null;
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                buffer = new byte[fs.Length];
                fs.Read(buffer, 0, (int)fs.Length);
            }
            return buffer;
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
#region Commented old code
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using DMS.BusinessLogic;
//using System.IO;
//using System.Data;

//namespace DMS.Handler
//{
//    /// <summary>
//    /// Summary description for PDFHandler
//    /// </summary>
//    public class PDFHandler : IHttpHandler
//    {

//        public void ProcessRequest(HttpContext context)
//        {
//            if (context.Request["DOCID"] != null)
//            {
//                try
//                {
//                    int documentID = Convert.ToInt32(context.Request["DOCID"]);
//                    DataTable objDataTable = Document.GetDocumentPath(documentID);
//                    if (objDataTable.Rows.Count > 0)
//                    {
//                        byte[] objFileByte = Utility.ConvertImageToPdfFromFileByte(objDataTable.Rows[0]["DocumentPath"].ToString(), (byte[])(objDataTable.Rows[0]["Image"] == DBNull.Value ? null : objDataTable.Rows[0]["Image"]));
//                        string Filename = objDataTable.Rows[0]["DocumentName"].ToString().Substring(0, objDataTable.Rows[0]["DocumentName"].ToString().IndexOf('.')) + ".pdf";
//                        context.Response.AddHeader("content-disposition", "inline; filename=" + objDataTable.Rows[0]["DocumentName"].ToString());
//                        context.Response.ContentType = "application/pdf";
//                        context.Response.BinaryWrite(objFileByte);
//                        //context.Response.Buffer = true;
//                        if (context.Response.IsClientConnected)
//                        {
//                           context.Response.Flush();
//                            context.Response.End();
//                        }
//                        //context.Response.Flush();
//                        //context.Response.End();
//                    }
//                    else
//                    {
//                        context.Response.Write("No File To Display");
//                    }
//                }
//                catch(Exception ex)
//                {
//                    throw ex;
//                }
//            }
//        }

//        public bool IsReusable
//        {
//            get
//            {
//                return false;
//            }
//        }
//    }
//}

#endregion