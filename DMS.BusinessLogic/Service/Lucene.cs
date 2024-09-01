using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Lucene.Net.Index;
using System.Web;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Search;
using LuceneNameSpace = Lucene.Net;
using DMS.BusinessLogic;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
//using MODI;
using Lucene.Net.Store;

namespace DMS.BusinessLogic.Service
{
    class Lucene
    {
        public void CheckLuceneFileExistence()
        {
            try
            {
                if (!IndexReader.IndexExists(Utility.LuceneFilePath))
                {
                    DirectoryInfo indexDirInfo = new DirectoryInfo(Utility.LuceneFilePath);
                    FSDirectory indexFSDir = FSDirectory.Open(indexDirInfo, new LuceneNameSpace.Store.SimpleFSLockFactory(indexDirInfo));
                    IndexWriter.Unlock(indexFSDir);

                    IndexWriter writer = new IndexWriter(Utility.LuceneFilePath, new StandardAnalyzer(), IndexWriter.MaxFieldLength.UNLIMITED);
                    writer.Close();
                }
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);                
            }
        }

        public void CreateLuceneIndex(int intDocumentID, string strDocumentData)
        {
            try
            {
                CheckLuceneFileExistence();

                DirectoryInfo indexDirInfo = new DirectoryInfo(Utility.LuceneFilePath);
                FSDirectory indexFSDir = FSDirectory.Open(indexDirInfo, new LuceneNameSpace.Store.SimpleFSLockFactory(indexDirInfo));
                IndexWriter.Unlock(indexFSDir);

                IndexWriter writer = new IndexWriter(Utility.LuceneFilePath, new StandardAnalyzer(), IndexWriter.MaxFieldLength.UNLIMITED);

                IndexDocument(writer, intDocumentID, strDocumentData);               
                writer.Close();
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }

        }

        public void IndexDocument(IndexWriter writer, int intDocumentID, string strDocumentData)
        {
            try
            {
                LuceneNameSpace.Documents.Document doc = new LuceneNameSpace.Documents.Document();

                doc.Add(new Field("DocumentID", intDocumentID.ToString(), Field.Store.YES, Field.Index.UN_TOKENIZED));
                doc.Add(new Field("DocumentData", strDocumentData, Field.Store.YES, Field.Index.TOKENIZED));

                writer.AddDocument(doc);                
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        public Utility.ResultType ReadPdfFile(string strFilename, byte[] byteFileByte, out string strText)
        {
            try
            {
                strText = string.Empty;
                if (System.IO.Path.GetExtension(strFilename).ToLower().Trim() == ".pdf")
                {
                    PdfReader reader;
                    if (Utility.FileStorageType == Utility.StorageType.FileSystem)
                        reader = new PdfReader(strFilename);
                    else
                        reader = new PdfReader(byteFileByte);

                    for (int page = 1; page <= reader.NumberOfPages; page++)
                    {
                        ITextExtractionStrategy its = new iTextSharp.text.pdf.parser.SimpleTextExtractionStrategy();
                        String s = PdfTextExtractor.GetTextFromPage(reader, page, its);
                        s = Encoding.UTF8.GetString(ASCIIEncoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(s)));
                        strText = strText + s;
                    }
                    reader.Close();
                }
                else
                {
                    #region OCR
                    //DocumentClass doc = new DocumentClass();
                   // try
                   // {
                       
                   //     if (Utility.FileStorageType == Utility.StorageType.FileSystem)
                   //         doc.Create(strFilename);


                   //     doc.OCR(MiLANGUAGES.miLANG_ENGLISH, true, true);
                       
                   //     foreach (MODI.Image image in doc.Images)
                   //     {
                   //         strText = strText + image.Layout.Text;
                   //     }
                   //     doc.Close(false);
                   // }
                   // catch (Exception ex)
                   // {
                   //     doc.Close(false);
                   //     strText = string.Empty;
                   //     LogManager.ErrorLog(Utility.LogFilePath, ex + " ===== MESSAGE =====" + ex.Message);
                   //     return Utility.ResultType.Error;
                    // }
                    #endregion
                }

                if (strText.Trim() != string.Empty)
                    return Utility.ResultType.Success;
                else
                    return Utility.ResultType.Failure;
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex +" ===== MESSAGE ====="+ex.Message);
                strText = string.Empty;
                return Utility.ResultType.Error;
            }
        }

        public string SearchPageContent(string strDocumentData)
        {
            string MetaDataID = "";
            try
            {
                LuceneNameSpace.Search.BooleanQuery bq = new LuceneNameSpace.Search.BooleanQuery();

                LuceneNameSpace.Search.Query ql = new LuceneNameSpace.Search.TermQuery(new LuceneNameSpace.Index.Term("DocumentData", strDocumentData.ToString()));

                bq.Add(ql, LuceneNameSpace.Search.BooleanClause.Occur.SHOULD);

                LuceneNameSpace.Search.IndexSearcher objSearcher = new LuceneNameSpace.Search.IndexSearcher(Utility.LuceneFilePath);

                var oHitColl = objSearcher.Search(bq);
                for (int i = 0; i < oHitColl.Length(); i++)
                {
                    LuceneNameSpace.Documents.Document oDoc = oHitColl.Doc(i);
                    MetaDataID = MetaDataID + oDoc.Get("DocumentID") + ",";
                }
                objSearcher.Close();
                return MetaDataID.Trim(',');
            }
            catch (Exception ex)
            {
                return "";
            }

        }
    }
}
