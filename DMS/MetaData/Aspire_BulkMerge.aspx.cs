using DMS.BusinessLogic;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace DMS.Shared
{
    public partial class Aspire_BulkMerge : System.Web.UI.Page
    {
        Utility objUtility = new Utility();
        DMS.BusinessLogic.Document objdocument = new DMS.BusinessLogic.Document();
          
        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
            scriptManager.RegisterPostBackControl(this.ibtnSubmit);
            //((AjaxControlToolkit.ToolkitScriptManager)Master.FindControl("tsmManager")).RegisterPostBackControl(ibtnSubmit);
          
        }

        private void MergeFilesNew(string[] fileNames, string outFile)
        {
            try
            {
                // step 1: creation of a document-object
                iTextSharp.text.Document document = new iTextSharp.text.Document();

                // step 2: we create a writer that listens to the document
                PdfCopy writer = new PdfCopy(document, new FileStream(outFile, FileMode.Create));
                if (writer == null)
                {
                    return;
                }

                // step 3: we open the document
                document.Open();

                foreach (string fileName in fileNames)
                {
                    // we create a reader for a certain document


                    PdfReader reader = new PdfReader(fileName);
                    reader.ConsolidateNamedDestinations();

                    // step 4: we add content
                    for (int i = 1; i <= reader.NumberOfPages; i++)
                    {
                        PdfImportedPage page = writer.GetImportedPage(reader, i);
                        writer.AddPage(page);
                    }

                    PRAcroForm form = reader.AcroForm;
                    if (form != null)
                    {
                       // writer.CopyAcroForm(reader);
                    }

                    reader.Close();


                }

                // step 5: we close the document and writer
                writer.Close();
                document.Close();
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                // return Utility.ResultType.Error;
            }
        }

        protected void ibtnSubmit_Click(object sender, ImageClickEventArgs e)
        {
            int i = 0;
            DataTable dt1 = new DataTable("NotMergedTable");
            dt1.Columns.Add(new DataColumn("DocName_Merged", typeof(string)));
            dt1.Columns.Add(new DataColumn("DocName_NotMerged", typeof(string)));
            dt1.Columns.Add(new DataColumn("DocName_SamePagecountEntry", typeof(string)));
            if (Path.GetExtension(flUpload.FileName).ToLower().Trim() == ".zip")
            {
                string file1 = Server.MapPath("~/Files/");
                string ExtractFolder = Server.MapPath("~/ExtratedFiles/");
                string[] Serverfilepath = { file1, ExtractFolder };
                foreach (string path in Serverfilepath)
                {
                    DeleteDirectory(path);
                }
                string filename = Path.GetFileName(flUpload.PostedFile.FileName);
                //string filepath = "~/Files/" + filename;    
                //string file1 = Server.MapPath("~/Files/");
                if (!Directory.Exists(file1))
                {
                    Directory.CreateDirectory(file1);
                }
                flUpload.SaveAs(Server.MapPath("~/Files/" + filename));


                string filepath = Server.MapPath("~/Files/" + filename);
                // string ExtractFolder = Server.MapPath("~/ExtratedFiles/");

                if (!Directory.Exists(ExtractFolder))
                {
                    Directory.CreateDirectory(ExtractFolder);
                }
                string Destination = Server.MapPath("~/ExtratedFiles/");


                using (Ionic.Zip.ZipFile zip1 = Ionic.Zip.ZipFile.Read(filepath))//"D:\\Faltu\\DMS_ABBOTT\\DMS\\DMS\\Files\\Files_17May.zip"
                {
                    //zip1.ExtractAll(flUpload.PostedFile.FileName.Substring(0, flUpload.PostedFile.FileName.IndexOf(".")),

                    //Ionic.Zip.ExtractExistingFileAction.DoNotOverwrite);
                    zip1.ExtractAll(Destination, Ionic.Zip.ExtractExistingFileAction.DoNotOverwrite);

                }
                string[] files = Directory.GetFiles(Destination + filename.Substring(0, filename.LastIndexOf(".")) + "\\", "*.pdf");
                foreach (string f in files)
                {

                    int DocId = 0;
                    DataSet ds = new DataSet();
                    var fileLength = new FileInfo(f).Length;
                    PdfReader readermerge = new PdfReader(f);
                    int MergePageCount = readermerge.NumberOfPages;
                    string strHostName = "";
                    strHostName = System.Net.Dns.GetHostName();
                    IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(strHostName);
                    IPAddress[] addr = ipEntry.AddressList;
                    string IPAddress = addr[addr.Length - 1].ToString();
                    char delimeter = '\\';
                    string[] filenameparts = f.Split(delimeter);
                    string flname = "";

                    foreach (string substr in filenameparts)
                     {
                         if (substr.Contains("."))
                         {
                             if (substr.Contains('('))
                             {
                                 flname = substr.Substring(0, substr.IndexOf("("));
                                 flname = flname.Trim() + ".pdf";
                             }
                             else
                                 flname = substr;
                         }

                     }
                    DataTable dtRep=new DataTable();
                    DMS.BusinessLogic.RepositoryManager objRepositoryManager = new RepositoryManager();
                    objRepositoryManager.SelectRepository(out dtRep, "Centrum");
                    int RepId = Convert.ToInt32(dtRep.Rows[0]["ID"].ToString());
                    ds = DMS.BusinessLogic.Document.CheckDocumentSingleUpload(flname, RepId);
                    if (ds.Tables[0].Rows.Count == 1)
                      {
                          //FileInfo fl = new FileInfo(f);
                          int bufferSize = 1024 * 1024;
                          
                         //string FilePath = Utility.ArchiveFilePath + f.Split('\\').Last() ;
                         //string FilePath = Utility.ArchiveFilePath + f.Substring(f.LastIndexOf("\\") + 1, 8) + ".pdf";
                         string FilePath = Utility.ArchiveFilePath + flname;
                          using (FileStream fileStream = new FileStream(FilePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite))
                          {
                              FileStream fs = new FileStream(f, FileMode.Open, FileAccess.ReadWrite);
                              fileStream.SetLength(fs.Length);
                              int bytesRead = -1;
                              byte[] bytes = new byte[bufferSize];
                              while ((bytesRead = fs.Read(bytes, 0, bufferSize)) > 0)
                              {
                                  fileStream.Write(bytes, 0, bytesRead);
                              }
                              fs.Close();
                              fs.Dispose();

                          }
                          i++;
                          DocId = Convert.ToInt32(ds.Tables[0].Rows[0]["ID"].ToString());
                          DataTable dt = new DataTable();
                          dt = DMS.BusinessLogic.Document.GetDocumentPath(DocId);
                          string OldFileName = dt.Rows[0]["DocumentName"].ToString();
                          string DocGuid = dt.Rows[0]["DOCUMENTGUID"].ToString();
                          int OldPageCount = Convert.ToInt32(dt.Rows[0]["PAGECOUNT"].ToString());
                          string OldFilePath = dt.Rows[0]["DocumentPath"].ToString();
                          string[] filenames = { OldFilePath, FilePath };
                          MergeFilesNew(filenames, Utility.VersionFilePath + OldFileName);
                          PdfReader reader = new PdfReader(Utility.VersionFilePath + OldFileName);
                          int No_Of_Pages = reader.NumberOfPages;

                          FileInfo info = new FileInfo(Utility.VersionFilePath + OldFileName);
                          decimal fileSize = (info.Length) / 1024;
                          //File.Move(Utility.VersionFilePath + OldFileName, Utility.DocumentPath + DocGuid);
                          string RepName = dtRep.Rows[0]["RepositoryName"].ToString();
                          File.Copy(Utility.VersionFilePath + OldFileName, Utility.DocumentPath + RepName + @"\" + DocGuid, true);

                          objdocument.DocumentID = DocId;
                          objdocument.Size = Convert.ToInt32(fileSize);
                          objdocument.OldPageCount = OldPageCount;
                          if (dt.Rows[0]["MergedPageCount"].ToString()!="")
                        {
                            int OriginalMergePageCount = Convert.ToInt32(dt.Rows[0]["MergedPageCount"].ToString());
                            int TotalMergedPageCount = OriginalMergePageCount + MergePageCount;
                            objdocument.MergedPageCount = TotalMergedPageCount;
                        }
                          else
                          {                                                        
                              objdocument.MergedPageCount = MergePageCount;
                          }
                          
                          objdocument.UpdatedBy = UserSession.UserID;
                          DocumentManager objDocumentManager = new DocumentManager();
                          DbTransaction objDbTransaction = Utility.GetTransaction;
                          objUtility.Result = objDocumentManager.UpdateDocumentForMerging(objdocument, objDbTransaction);
                          switch (objUtility.Result)
                          {
                              case Utility.ResultType.Success:

                                  #region Seema 1 Aug 2017

                                  objdocument.DocumentID = DocId;
                                  objdocument.DocumentName = OldFileName;
                                  objdocument.DocumentGuid = DocGuid;
                                  objdocument.DocumentPath = OldFilePath;
                                  objdocument.UpdatedBy = UserSession.UserID;
                                  objdocument.Size = Convert.ToInt32(fileLength);
                                  objdocument.MergedPageCount = MergePageCount;
                                  objdocument.IPAddress = IPAddress;
                                  objUtility.Result = objDocumentManager.MergeInsert(objdocument, objDbTransaction);

                                   #endregion
                                  objDbTransaction.Commit();
                                  UserSession.DisplayMessage(this, "Document has been merged", MainMasterPage.MessageType.Success);
                                      DataRow dr = dt1.NewRow();
                                      dr["DocName_Merged"] = (f.Split('\\').Last()).Replace(" ", "");
                                      dt1.Rows.Add(dr);
                                  break;
                              case Utility.ResultType.Failure:
                              case Utility.ResultType.Error:
                                  objDbTransaction.Rollback();
                                  //UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                                  break;
                          }

                      
                      }

                      else if (ds.Tables[0].Rows.Count > 1)
                      {
                          DataRow dr = dt1.NewRow();
                          dr["DocName_SamePagecountEntry"] = (f.Split('\\').Last()).Replace(" ", "");
                          dt1.Rows.Add(dr);
                      }
                      else
                      {
                         // DataSet ds1 = new DataSet();
                         
                         
                          DataRow dr = dt1.NewRow();
                         // dr["DocName"] = f.Substring(f.LastIndexOf("\\") + 1, 8) + ".pdf";
                          dr["DocName_NotMerged"] = (f.Split('\\').Last()).Replace(" ", "");
                          dt1.Rows.Add(dr);
                          
                         // ds.Tables.Add(dt);
                      }
                      
                 }
                // Directory.Delete(flUpload.PostedFile.FileName.Substring(0,flUpload.PostedFile.FileName.IndexOf(".")),true);
                 //ExportToExcel(dt1);

                 //if (Directory.Exists(file1))
                 //{
                 //    foreach (string file in Directory.GetFiles(file1))
                 //    {
                 //        File.Delete(file);
                 //    }
                 //}
                 //if (Directory.Exists(ExtractFolder))
                 //{
                 //    foreach (string directory in Directory.GetDirectories(ExtractFolder))
                 //        Directory.Delete(directory);
                 //}
                
                 UserSession.DisplayMessage(this, i + " files were merged!", MainMasterPage.MessageType.Success);
                 if (dt1.Rows.Count > 0)
                 {
                     Utility.ExportToExcel(dt1, string.Format("DocumentNotMergedReport{0:dd MMMM yyyy HH mm tt}", DateTime.Now));
                 }
                

            }
            else
            {
                UserSession.DisplayMessage(this, "Please upload zip file only!", MainMasterPage.MessageType.Error);
                return;
            }
        }


        private void DeleteDirectory(string path)
        {
            if (Directory.Exists(path))
            {
                //Delete all files from the Directory
                foreach (string file in Directory.GetFiles(path))
                {
                    File.Delete(file);
                }
                //Delete all child Directories
                foreach (string directory in Directory.GetDirectories(path))
                {
                    DeleteDirectory(directory);
                }
                //Delete a Directory
                Directory.Delete(path);
            }
        }
    }
}