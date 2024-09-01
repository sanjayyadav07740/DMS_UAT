using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using DMS.BusinessLogic;
using System.Data.SqlClient;

namespace DMS.UserControl
{
    public class PdfHandler : IHttpHandler, IRequiresSessionState, IReadOnlySessionState
    {
        SqlConnection con = new SqlConnection(Utility.ConnectionString);

        public void ProcessRequest(HttpContext context)
        {
            if (context.Request["PageID"] != null)
             {
                try
                {
                    System.Drawing.Image objImage =null;
                    if (context.Request["PageID"].ToString() == "download")
                    {
                        context.Response.Clear();
                        context.Response.ContentType = "application/octet-stream";
                        context.Response.AppendHeader("content-disposition", "attachment;filename=\"" + context.Session["DocumentName"].ToString() + "\"");
                        
                        context.Response.TransmitFile(context.Session["DocumentPath"].ToString());
                        
                        HttpContext.Current.ApplicationInstance.CompleteRequest();

                        con.Open();
                        SqlCommand cmd = new SqlCommand("Select ID from Document where DocumentName="+"'"+ context.Session["DocumentName"].ToString()+"'", con);
                        int DocumentID = Convert.ToInt32(cmd.ExecuteScalar());
                        con.Close();

                        Log.DocumentAuditLog(HttpContext.Current, "Download Document", "PdfHandler", DocumentID);

                    }
                    if(context.Request["PageID"].ToString() == "download")
                    {
                        objImage = ExtractImage(context.Session["DocumentPath"].ToString(), 1);
                    }
                    else
                    {
                        objImage = ExtractImage(context.Session["DocumentPath"].ToString(), Convert.ToInt32(context.Request["PageID"].ToString()));
                    }
                    
                    //System.Drawing.Image objImage = ExtractImage(context.Session["DocumentPath"].ToString(), 1);
                    if (objImage != null)
                    {
                        if (context.Request["RotateID"] != null)
                        {
                            int rotateID = 0;
                            if (context.Request["RotateID"].ToString().Split(',').Length > 0)
                            {
                                rotateID = Convert.ToInt32(context.Request["RotateID"].ToString().Split(',')[context.Request["RotateID"].ToString().Split(',').Length - 1].ToString());
                            }
                            switch (rotateID)
                            {
                                case 1:
                                    objImage.RotateFlip(System.Drawing.RotateFlipType.RotateNoneFlipNone);
                                    break;

                                case 2:
                                    objImage.RotateFlip(System.Drawing.RotateFlipType.Rotate90FlipNone);
                                    break;

                                case 3:
                                    objImage.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipNone);
                                    break;

                                case 4:
                                    objImage.RotateFlip(System.Drawing.RotateFlipType.Rotate270FlipNone);
                                    break;
                            }
                        }
                        Bitmap objBitmapTemp = (Bitmap)objImage.GetThumbnailImage(1000, 1000, () => false, IntPtr.Zero);
                        objBitmapTemp.Save(context.Response.OutputStream, ImageFormat.Jpeg);
                        objBitmapTemp.Dispose();
                        objImage.Dispose();
                      // context.Response.Flush();
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                        
                    }
                }
                catch (Exception ex)
                {

                    LogManager.ErrorLog(Utility.LogFilePath, ex);
                    //objDataTable = null;
                    //return Utility.ResultType.Error;
                }
            }
        }

        public static Bitmap ExtractImage(string filePath, int pageNumber)
        {
            if (Path.GetExtension(filePath).ToLower() == ".pdf")
            {
                try
                {
                    PDFConvert converter = new PDFConvert();
                    bool Converted = false;
                    converter.RenderingThreads = Environment.ProcessorCount;
                    converter.OutputToMultipleFile = false;
                    converter.MaxBitmap = 100000000;
                    converter.MaxBuffer = 200000000;

                    converter.FirstPageToConvert = pageNumber;
                    converter.LastPageToConvert = pageNumber;

                    converter.FitPage = false;
                    converter.JPEGQuality = 70;
                    converter.UserPassword = string.Empty;

                    converter.ResolutionX = 200;
                    converter.ResolutionY = 200;

                    converter.TextAlphaBit = 4;
                    converter.GraphicsAlphaBit = 4;

                    converter.OutputFormat = "png16m";
                    string output = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".png";
                    Converted = converter.Convert(filePath, output);
                    if (Converted)
                    {
                        MemoryStream objStream = new MemoryStream(File.ReadAllBytes(output));
                        System.Drawing.Bitmap objBitmap = new System.Drawing.Bitmap(objStream);
                        if (File.Exists(output))
                        {
                            File.Delete(output);
                        }
                        return objBitmap;
                    }
                    else
                    {
                        return null;
                    }

                }
                catch (Exception ex)
                {

                    LogManager.ErrorLog(Utility.LogFilePath, ex);
                    // objDataTable = null;
                    // return Utility.ResultType.Error;
                }
            }
            else
            {
                try
                {
                    Bitmap objBitmap = new Bitmap(filePath);
                        //ConvertTiffToJpeg(filePath);
                        
                    objBitmap.SelectActiveFrame(FrameDimension.Page, pageNumber - 1);
                    MemoryStream objStream = new MemoryStream();
                    objBitmap.Save(objStream, ImageFormat.Jpeg);
                    objBitmap.Dispose();
                    return new Bitmap(objStream);
                }
                catch(Exception ex)
                {
                    throw ex;
                }
            }
            return null;

        }

        public static Bitmap ConvertTiffToJpeg(string fileName)
        {
            using (Image imageFile = Image.FromFile(fileName))
            {
                Bitmap bmp=null;
                FrameDimension frameDimensions = new FrameDimension(
                    imageFile.FrameDimensionsList[0]);

                // Gets the number of pages from the tiff image (if multipage) 
                int frameNum = imageFile.GetFrameCount(frameDimensions);
                string[] jpegPaths = new string[frameNum];

                for (int frame = 0; frame < frameNum; frame++)
                {
                    // Selects one frame at a time and save as jpeg. 
                    imageFile.SelectActiveFrame(frameDimensions, frame);
                    using (bmp = new Bitmap(imageFile))
                    {
                        jpegPaths[frame] = String.Format("{0}\\{1}{2}.jpg",
                            Path.GetDirectoryName(fileName),
                            Path.GetFileNameWithoutExtension(fileName),
                            frame);
                        bmp.Save(jpegPaths[frame], ImageFormat.Jpeg);
                    }
                }

                return bmp;
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