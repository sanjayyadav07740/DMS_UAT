using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Diagnostics;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections;
using System.Runtime.CompilerServices;

namespace DMS.UserControl
{
    [StructLayout(LayoutKind.Sequential)]
    public struct GhostScriptRevision
    {
        public string ProductInformation;
        public string CopyrightInformations;
        public int intRevision;
        public int intRevisionDate;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct GS_Revision
    {
        public IntPtr strProduct;
        public IntPtr strCopyright;
        public int intRevision;
        public int intRevisionDate;
    }

    public delegate Int32 StdioCallBack(IntPtr handle, IntPtr strptr, Int32 count);

    public class PDFConvert
    {
        private bool _bFitPage;
        private bool _bForcePageSize;
        private bool _bRedirectIO;
        private bool _bThrowOnlyException;
        /// <summary>If true i will try to output everypage to a different file!</summary>
        private bool _didOutputToMultipleFile;
        /// <summary>The first page to convert in image</summary>
        private int _iFirstPageToConvert;
        /// <summary>This parameter is used to control subsample antialiasing of graphics</summary>
        private int _iGraphicsAlphaBit;
        private int _iHeight;
        private int _iJPEGQuality;
        /// <summary>The last page to conver in an image</summary>
        private int _iLastPageToConvert;
        private int _iMaxBitmap;
        private int _iMaxBuffer;
        /// <summary>In how many thread i should perform the conversion</summary>
        /// <remarks>This is a Major innovation since 8.63 NEVER use it with previous version!</remarks>
        private int _iRenderingThreads;
        private int _iResolutionX;
        private int _iResolutionY;
        /// <summary>This parameter is used to control subsample antialiasing of text</summary>
        private int _iTextAlphaBit;
        private int _iWidth;
        private IntPtr _objHandle;
        /// <summary>The pagesize of the output</summary>
        private string _sDefaultPageSize;
        private string _sDeviceFormat;
        private string _sParametersUsed;
        private string _sUserPassword;
        private const string BW_TIFF_G4 = "tiffg4";
        private const string BW_TIFF_LZW = "tifflzw";
        private const string COLOR_JPEG = "jpeg";
        private const string COLOR_PNG_RGB = "png16m";
        private const string COLOR_TIFF_CMYK = "tiff32nc";
        private const string COLOR_TIFF_CMYK_SEP = "tiffsep";
        private const string COLOR_TIFF_RGB = "tiff24nc";
        private const int e_NeedInput = -106;
        private const int e_Quit = -101;
        private const string GRAY_JPG = "jpeggray";
        private const string GRAY_PNG = "pnggray";
        private const string GRAY_TIFF_NC = "tiffgray";
        private const string GS_BufferSpace = "-dBufferSpace={0}";
        private const string GS_DefaultPaperSize = "-sPAPERSIZE={0}";
        private const string GS_DeviceFormat = "-sDEVICE={0}";
        private const string GS_FirstPageFormat = "-dFirstPage={0}";
        private const string GS_FirstParameter = "pdf2img";
        private const string GS_FitPage = "-dPDFFitPage";
        private const string GS_Fixed1stParameter = "-dNOPAUSE";
        private const string GS_Fixed2ndParameter = "-dBATCH";
        private const string GS_Fixed3rdParameter = "-dSAFER";
        private const string GS_FixedMedia = "-dFIXEDMEDIA";
        private const string GS_GraphicsAlphaBits = "-dGraphicsAlphaBits={0}";
        private const string GS_JpegQualityFormat = "-dJPEGQ={0}";
        private const string GS_LastPageFormat = "-dLastPage={0}";
        private const string GS_MaxBitmap = "-dMaxBitmap={0}";
        private const string GS_MultiplePageCharacter = "%";
        /// <summary>Thanks to 	tchu_2000 to remind that u should never hardcode strings! :)</summary>
        private const string GS_OutputFileFormat = "-sOutputFile={0}";
        private const string GS_PageSizeFormat = "-g{0}x{1}";
        private const string GS_Password = "-sPDFPassword={0}";
        private const string GS_QuiteOperation = "-q";
        private const string GS_RenderingThreads = "-dNumRenderingThreads={0}";
        private const string GS_ResolutionXFormat = "-r{0}";
        private const string GS_ResolutionXYFormat = "-r{0}x{1}";
        private const string GS_StandardOutputDevice = "-";
        private const string GS_TextAlphaBits = "-dTextAlphaBits={0}";
        private Process myProcess;
        public StringBuilder output;
        private const int PRINT_DPI = 300;
        /// <summary>Use to check for default transformation</summary>
        private static bool useSimpleAnsiConversion = true;
        private const int VIEW_DPI = 200;

        public PDFConvert()
        {
            this._iMaxBitmap = 0;
            this._iMaxBuffer = 0;
            this._iFirstPageToConvert = -1;
            this._iLastPageToConvert = -1;
            this._iGraphicsAlphaBit = -1;
            this._iTextAlphaBit = -1;
            this._iRenderingThreads = -1;
            this._bThrowOnlyException = false;
            this._bRedirectIO = false;
            this._bForcePageSize = false;
            this._didOutputToMultipleFile = false;
            this._objHandle = IntPtr.Zero;
        }

        public PDFConvert(IntPtr objHandle)
        {
            this._iMaxBitmap = 0;
            this._iMaxBuffer = 0;
            this._iFirstPageToConvert = -1;
            this._iLastPageToConvert = -1;
            this._iGraphicsAlphaBit = -1;
            this._iTextAlphaBit = -1;
            this._iRenderingThreads = -1;
            this._bThrowOnlyException = false;
            this._bRedirectIO = false;
            this._bForcePageSize = false;
            this._didOutputToMultipleFile = false;
            this._objHandle = objHandle;
        }

        /// <summary>Convert a Pointer to a string to a real string</summary>
        /// <param name="strz">the pointer to the string in memory</param>
        /// <returns>The string</returns>
        public static string AnsiZtoString(IntPtr strz)
        {
            if (strz != IntPtr.Zero)
            {
                return Marshal.PtrToStringAnsi(strz);
            }
            return string.Empty;
        }

        /// <summary>Remove the memory allocated</summary>
        /// <param name="aGCHandle"></param>
        /// <param name="gchandleArgs"></param>
        private void ClearParameters(ref GCHandle[] aGCHandle, ref GCHandle gchandleArgs)
        {
            int num2 = aGCHandle.Length - 1;
            for (int i = 0; i <= num2; i++)
            {
                aGCHandle[i].Free();
            }
            gchandleArgs.Free();
        }

        public Image Convert(string inputFile)
        {
            this._bRedirectIO = true;
            if (this.Convert(inputFile, "%stdout", System.Convert.ToString(this._bThrowOnlyException)) && ((this.output != null) && (this.output.Length > 0)))
            {
                return (Image.FromStream(this.myProcess.StandardOutput.BaseStream).Clone() as Image);
            }
            return null;
        }

        /// <summary>Convert a single file!</summary>
        /// <param name="inputFile">The file PDf to convert</param>
        /// <param name="outputFile">The image file that will be created</param>
        /// <remarks>You must pass all the parameter for the conversion
        /// as Proprieties of this class</remarks>
        /// <returns>True if the conversion succed!</returns>
        public bool Convert(string inputFile, string outputFile)
        {
            return this.Convert(inputFile, outputFile, this._bThrowOnlyException, null);
        }

        /// <summary>Convert a single file!</summary>
        /// <param name="inputFile">The file PDf to convert</param>
        /// <param name="outputFile">The image file that will be created</param>
        /// <param name="parameters">You must pass all the parameter for the conversion here</param>
        /// <remarks>Thanks to 	tchu_2000 for the help!</remarks>
        /// <returns>True if the conversion succed!</returns>
        public bool Convert(string inputFile, string outputFile, string parameters)
        {
            return this.Convert(inputFile, outputFile, this._bThrowOnlyException, parameters);
        }

        /// <summary>Convert a single file!</summary>
        /// <param name="inputFile">The file PDf to convert</param>
        /// <param name="outputFile">The image file that will be created</param>
        /// <param name="throwException">if the function should throw an exception
        /// or display a message box</param>
        /// <remarks>You must pass all the parameter for the conversion
        /// as Proprieties of this class</remarks>
        /// <returns>True if the conversion succed!</returns>
        private bool Convert(string inputFile, string outputFile, bool throwException, string options)
        {
            IntPtr ptr2 = IntPtr.Zero;
            if (string.IsNullOrEmpty(inputFile))
            {
                if (throwException)
                {
                    throw new ArgumentNullException("inputFile");
                }
                throw new Exception("The inputfile is missing");
                return false;
            }
            if (!File.Exists(inputFile))
            {
                if (throwException)
                {
                    throw new ArgumentException(string.Format("The file :'{0}' doesn't exist", inputFile), "inputFile");
                }
                throw new Exception(string.Format("The file :'{0}' doesn't exist", inputFile));
                return false;
            }
            if (string.IsNullOrEmpty(this._sDeviceFormat))
            {
                if (throwException)
                {
                    throw new ArgumentNullException("Device");
                }
                throw new Exception("You didn't provide a device for the conversion");
                return false;
            }
            string[] strArray = this.GetGeneratedArgs(inputFile, outputFile, options);
            int length = strArray.Length;
            object[] objArray = new object[(length - 1) + 1];
            IntPtr[] ptrArray = new IntPtr[(length - 1) + 1];
            GCHandle[] aGCHandle = new GCHandle[(length - 1) + 1];
            int num4 = length - 1;
            for (int i = 0; i <= num4; i++)
            {
                objArray[i] = StringToAnsiZ(strArray[i]);
                aGCHandle[i] = GCHandle.Alloc(RuntimeHelpers.GetObjectValue(objArray[i]), GCHandleType.Pinned);
                ptrArray[i] = aGCHandle[i].AddrOfPinnedObject();
            }
            GCHandle gchandleArgs = GCHandle.Alloc(ptrArray, GCHandleType.Pinned);
            IntPtr argv = gchandleArgs.AddrOfPinnedObject();
            int num3 = -1;
            try
            {
                if (gsapi_new_instance(ref ptr2, this._objHandle) < 0)
                {
                    this.ClearParameters(ref aGCHandle, ref gchandleArgs);
                    if (throwException)
                    {
                        throw new ApplicationException("I can't create a new istance of Ghostscript please verify no other istance are running!");
                    }
                    throw new Exception("I can't create a new istance of Ghostscript please verify no other istance are running!");
                    return false;
                }
            }
            catch (DllNotFoundException exception1)
            {
                DllNotFoundException exception = exception1;
                this.ClearParameters(ref aGCHandle, ref gchandleArgs);
                if (throwException)
                {
                    throw new ApplicationException("The gsdll32.dll wasn't found in default dlls search pathor is not in correct version (doesn't expose the required methods). Please download the version 8.64 from the original website");
                }
                throw new Exception("The gsdll32.dll wasn't found in default dlls search pathor is not in correct version (doesn't expose the required methods). Please download the version 8.64 from the original website");
                bool flag = false;
                return flag; ;
            }
            if (this._bRedirectIO)
            {
                StdioCallBack back2 = new StdioCallBack(this.gsdll_stdin);
                StdioCallBack back3 = new StdioCallBack(this.gsdll_stdout);
                StdioCallBack back = new StdioCallBack(this.gsdll_stderr);
                num3 = gsapi_set_stdio(ptr2, back2, back3, back);
                if (this.output == null)
                {
                    this.output = new StringBuilder();
                }
                else
                {
                    this.output.Remove(0, this.output.Length);
                }
                this.myProcess = Process.GetCurrentProcess();
                this.myProcess.OutputDataReceived += new DataReceivedEventHandler(this.SaveOutputToImage);
            }
            num3 = -1;
            try
            {
                num3 = gsapi_init_with_args(ptr2, length, argv);
            }
            catch (Exception exception3)
            {
                Exception innerException = exception3;
                if (throwException)
                {
                    throw new ApplicationException(innerException.Message, innerException);
                }
                throw new Exception(innerException.Message);
            }
            finally
            {
                this.ClearParameters(ref aGCHandle, ref gchandleArgs);
                gsapi_exit(ptr2);
                gsapi_delete_instance(ptr2);
                if (this.myProcess != null)
                {
                    this.myProcess.OutputDataReceived -= new DataReceivedEventHandler(this.SaveOutputToImage);
                }
            }
            return ((num3 == 0) | (num3 == -101));
        }

        public static string ConvertPdfToGraphic(string inputFileName, string outputFileName, string fileFormat, int DPI, int startPageNumber = 0, int endPageNumber = 0, bool ToPrinter = false, string Password = "")
        {
            PDFConvert convert = new PDFConvert();
            convert.RenderingThreads = Environment.ProcessorCount;
            if (fileFormat.Contains("tif"))
            {
                convert.OutputToMultipleFile = false;
            }
            else
            {
                convert.OutputToMultipleFile = true;
            }
            if (startPageNumber == 0)
            {
                convert.FirstPageToConvert = -1;
                convert.LastPageToConvert = -1;
            }
            else
            {
                convert.FirstPageToConvert = startPageNumber;
                convert.LastPageToConvert = endPageNumber;
            }
            convert.FitPage = false;
            convert.JPEGQuality = 70;
            if (ToPrinter)
            {
                convert.TextAlphaBit = -1;
                convert.GraphicsAlphaBit = -1;
            }
            else
            {
                convert.TextAlphaBit = 4;
                convert.GraphicsAlphaBit = 4;
            }
            convert.ResolutionX = DPI;
            convert.ResolutionY = DPI;
            convert.OutputFormat = fileFormat;
            convert.UserPassword = Password;
            FileInfo info = new FileInfo(inputFileName);
            while (File.Exists(outputFileName))
            {
                string str2 = Regex.Replace(outputFileName, @"^.+\.(.+$)", "$1");
                outputFileName = outputFileName.Replace("." + str2, "(" + System.Convert.ToString(DateTime.Now.Ticks) + ")." + str2);
            }
            if (convert.Convert(info.FullName, outputFileName))
            {
                return outputFileName;
            }
            return "";
        }

        /// <summary>Needed to copy memory from one location to another, used to fill the struct</summary>
        /// <param name="Destination"></param>
        /// <param name="Source"></param>
        /// <param name="Length"></param>
        [DllImport("kernel32.dll", EntryPoint = "RtlMoveMemory")]
        private static extern void CopyMemory(IntPtr Destination, IntPtr Source, uint Length);
        /// <summary>This function create the list of parameters to pass to the dll with parameters given directly from the program</summary>
        /// <param name="inputFile"></param>
        /// <param name="outputFile"></param>
        /// <param name="otherParameters">The other parameters i could be interested</param>
        /// <remarks>Be very Cautious using this! code provided and modified from tchu_2000</remarks>
        /// <returns></returns>
        private string[] GetGeneratedArgs(string inputFile, string outputFile, string otherParameters)
        {
            if (!string.IsNullOrEmpty(otherParameters))
            {
                return this.GetGeneratedArgs(inputFile, outputFile, otherParameters.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries));
            }
            return this.GetGeneratedArgs(inputFile, outputFile, (string[])null);
        }

        /// <summary>This function create the list of parameters to pass to the dll</summary>
        /// <param name="inputFile">the file to convert</param>
        /// <param name="outputFile">where to write the image</param>
        /// <returns>the list of the arguments</returns>
        private string[] GetGeneratedArgs(string inputFile, string outputFile, string[] presetParameters)
        {
            string[] strArray;
            ArrayList list = new ArrayList();
            if ((presetParameters == null) || (presetParameters.Length == 0))
            {
                if (((this._sDeviceFormat == "jpeg") && (this._iJPEGQuality > 0)) && (this._iJPEGQuality < 0x65))
                {
                    list.Add(string.Format("-dJPEGQ={0}", this._iJPEGQuality));
                }
                if ((this._iWidth > 0) && (this._iHeight > 0))
                {
                    list.Add(string.Format("-g{0}x{1}", this._iWidth, this._iHeight));
                }
                else if (!string.IsNullOrEmpty(this._sDefaultPageSize))
                {
                    list.Add(string.Format("-sPAPERSIZE={0}", this._sDefaultPageSize));
                    if (this._bForcePageSize)
                    {
                        list.Add("-dFIXEDMEDIA");
                    }
                }
                if (this._iGraphicsAlphaBit > 0)
                {
                    list.Add(string.Format("-dGraphicsAlphaBits={0}", this._iGraphicsAlphaBit));
                }
                if (this._iTextAlphaBit > 0)
                {
                    list.Add(string.Format("-dTextAlphaBits={0}", this._iTextAlphaBit));
                }
                if (this._bFitPage)
                {
                    list.Add("-dPDFFitPage");
                }
                if (this._iMaxBitmap > 0)
                {
                    list.Add(string.Format("-dMaxBitmap={0}", this._iMaxBitmap));
                }
                if (this._iMaxBuffer > 0)
                {
                    list.Add(string.Format("-dBufferSpace={0}", this._iMaxBuffer));
                }
                if (this._sUserPassword != "")
                {
                    list.Add(string.Format("-sPDFPassword={0}", this._sUserPassword));
                }
                if (this._iResolutionX > 0)
                {
                    if (this._iResolutionY > 0)
                    {
                        list.Add(string.Format("-r{0}x{1}", this._iResolutionX, this._iResolutionY));
                    }
                    else
                    {
                        list.Add(string.Format("-r{0}", this._iResolutionX));
                    }
                }
                if (this._iFirstPageToConvert > 0)
                {
                    list.Add(string.Format("-dFirstPage={0}", this._iFirstPageToConvert));
                }
                if (this._iLastPageToConvert > 0)
                {
                    if ((this._iFirstPageToConvert > 0) && (this._iFirstPageToConvert > this._iLastPageToConvert))
                    {
                        throw new ArgumentOutOfRangeException(string.Format("The 1st page to convert ({0}) can't be after then the last one ({1})", this._iFirstPageToConvert, this._iLastPageToConvert));
                    }
                    list.Add(string.Format("-dLastPage={0}", this._iLastPageToConvert));
                }
                if (this._iRenderingThreads > 0)
                {
                    list.Add(string.Format("-dNumRenderingThreads={0}", this._iRenderingThreads));
                }
                if (this._bRedirectIO)
                {
                    outputFile = "-";
                    list.Add("-q");
                }
                int num2 = 7;
                int count = list.Count;
                strArray = new string[(num2 + (list.Count - 1)) + 1];
                strArray[1] = "-dNOPAUSE";
                strArray[2] = "-dBATCH";
                strArray[3] = "-dSAFER";
                strArray[4] = string.Format("-sDEVICE={0}", this._sDeviceFormat);
                int num7 = count - 1;
                for (int j = 0; j <= num7; j++)
                {
                    strArray[5 + j] = (string)list[j];
                }
            }
            else
            {
                strArray = new string[(presetParameters.Length + 2) + 1];
                int num8 = presetParameters.Length - 1;
                for (int k = 1; k <= num8; k++)
                {
                    strArray[k] = presetParameters[k - 1];
                }
            }
            strArray[0] = "pdf2img";
            if (this._didOutputToMultipleFile && !outputFile.Contains("%"))
            {
                int startIndex = outputFile.LastIndexOf('.');
                if (startIndex > 0)
                {
                    outputFile = outputFile.Insert(startIndex, "%d");
                }
            }
            this._sParametersUsed = string.Empty;
            int num9 = strArray.Length - 3;
            for (int i = 1; i <= num9; i++)
            {
                this._sParametersUsed = this._sParametersUsed + " " + strArray[i];
            }
            strArray[strArray.Length - 2] = string.Format("-sOutputFile={0}", outputFile);
            strArray[strArray.Length - 1] = string.Format("{0}", inputFile);
            this._sParametersUsed = this._sParametersUsed + " " + string.Format("-sOutputFile={0}", string.Format("\"{0}\"", outputFile)) + " " + string.Format("\"{0}\"", inputFile);
            return strArray;
        }

        public static Image GetPageFromPDF(string filename, int PageNumber, int DPI = 200, string Password = "", bool forPrinting = false)
        {
            PDFConvert convert = new PDFConvert();
            convert.RenderingThreads = Environment.ProcessorCount;
            convert.OutputToMultipleFile = false;
            convert.MaxBitmap = 0x5f5e100;
            convert.MaxBuffer = 0xbebc200;
            if (PageNumber > 0)
            {
                convert.FirstPageToConvert = PageNumber;
                convert.LastPageToConvert = PageNumber;
            }
            else
            {
                return null;
            }
            convert.FitPage = false;
            convert.JPEGQuality = 70;
            convert.UserPassword = Password;
            if (DPI != 200)
            {
                convert.ResolutionX = DPI;
                convert.ResolutionY = DPI;
            }
            else
            {
                convert.ResolutionX = 200;
                convert.ResolutionY = 200;
            }
            if (forPrinting)
            {
                convert.TextAlphaBit = -1;
                convert.GraphicsAlphaBit = -1;
            }
            else
            {
                convert.TextAlphaBit = 4;
                convert.GraphicsAlphaBit = 4;
            }
            convert.OutputFormat = "png16m";
            string outputFile = Path.GetTempPath() + System.Convert.ToString(DateTime.Now.Ticks) + ".png";
            if (convert.Convert(filename, outputFile))
            {
                Image image = new Bitmap(outputFile);
                return image;
            }
            return null;
        }

        public GhostScriptRevision GetRevision()
        {
            GhostScriptRevision revision2;
            GS_Revision revision3 = new GS_Revision();
            GCHandle handle = GCHandle.Alloc(revision3, GCHandleType.Pinned);
            int num = gsapi_revision(ref revision3, 0x10);
            revision2.intRevision = revision3.intRevision;
            revision2.intRevisionDate = revision3.intRevisionDate;
            revision2.ProductInformation = AnsiZtoString(revision3.strProduct);
            revision2.CopyrightInformations = AnsiZtoString(revision3.strCopyright);
            handle.Free();
            return revision2;
        }

        /// <summary>
        /// Destroy an instance of Ghostscript. Before you call this, Ghostscript must have finished. If Ghostscript has been initialised, you must call gsapi_exit before gsapi_delete_instance. 
        /// </summary>
        /// <param name="instance"></param>
        [DllImport(@"gsdll32.dll")]
        private static extern void gsapi_delete_instance(IntPtr instance);
        /// <summary>
        /// Exit the interpreter. This must be called on shutdown if gsapi_init_with_args() has been called, and just before gsapi_delete_instance(). 
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        [DllImport(@"gsdll32.dll")]
        private static extern int gsapi_exit(IntPtr instance);
        /// <summary>This is the important function that will perform the conversion</summary>
        /// <param name="instance"></param>
        /// <param name="argc"></param>
        /// <param name="argv"></param>
        /// <returns></6>
        [DllImport(@"gsdll32.dll")]
        private static extern int gsapi_init_with_args(IntPtr instance, int argc, IntPtr argv);
        /// <summary>Create a new instance of Ghostscript. This instance is passed to most other gsapi functions. The caller_handle will be provided to callback functions.
        /// At this stage, Ghostscript supports only one instance. </summary>
        /// <param name="pinstance"></param>
        /// <param name="caller_handle"></param>
        /// <returns></returns>
        [DllImport(@"gsdll32.dll")]
        private static extern int gsapi_new_instance(ref IntPtr pinstance, IntPtr caller_handle);
        /// <summary>Get info about the version of Ghostscript i'm using</summary>
        /// <param name="pGSRevisionInfo"></param>
        /// <param name="intLen"></param>
        /// <returns></returns>
        [DllImport(@"gsdll32.dll")]
        private static extern int gsapi_revision(ref GS_Revision pGSRevisionInfo, int intLen);
        /// <summary>Use a different I/O</summary>
        /// <param name="lngGSInstance"></param>
        /// <param name="gsdll_stdin">Function that menage the Standard INPUT</param>
        /// <param name="gsdll_stdout">Function that menage the Standard OUTPUT</param>
        /// <param name="gsdll_stderr">Function that menage the Standard ERROR output</param>
        /// <returns></returns>
        [DllImport(@"gsdll32.dll")]
        private static extern int gsapi_set_stdio(IntPtr lngGSInstance, StdioCallBack gsdll_stdin, StdioCallBack gsdll_stdout, StdioCallBack gsdll_stderr);
        public int gsdll_stderr(IntPtr intGSInstanceHandle, IntPtr strz, int intBytes)
        {
            return intBytes;
        }

        public int gsdll_stdin(IntPtr intGSInstanceHandle, IntPtr strz, int intBytes)
        {
            int num = 0;
            return num;
        }

        public int gsdll_stdout(IntPtr intGSInstanceHandle, IntPtr strz, int intBytes)
        {
            if (intBytes > 0)
            {
            }
            return 0;
        }

        private void SaveOutputToImage(object sender, DataReceivedEventArgs e)
        {
            this.output.Append(e.Data);
        }

        /// <summary>
        /// Convert a Unicode string to a null terminated Ansi string for Ghostscript.
        /// The result is stored in a byte array
        /// </summary>
        /// <param name="str">The parameter i want to convert</param>
        /// <returns>the byte array that contain the string</returns>
        private static byte[] StringToAnsiZ(string str)
        {
            if (str == null)
            {
                str = string.Empty;
            }
            return Encoding.Default.GetBytes(str);
        }

        /// <summary>The pagesize of the output</summary>
        /// <remarks>Without this parameter the output should be letter, complain to USA for this :) if the document specify a different size it will take precedece over this!</remarks>
        public string DefaultPageSize
        {
            get
            {
                return this._sDefaultPageSize;
            }
            set
            {
                this._sDefaultPageSize = value;
            }
        }

        /// <summary>The first page to convert in image</summary>
        public int FirstPageToConvert
        {
            get
            {
                return this._iFirstPageToConvert;
            }
            set
            {
                this._iFirstPageToConvert = value;
            }
        }

        public bool FitPage
        {
            get
            {
                return this._bFitPage;
            }
            set
            {
                this._bFitPage = value;
            }
        }

        /// <summary>If set to true and page default page size will force the rendering in that output format</summary>
        public bool ForcePageSize
        {
            get
            {
                return this._bForcePageSize;
            }
            set
            {
                this._bForcePageSize = value;
            }
        }

        /// <summary>This parameter is used to control subsample antialiasing of graphics</summary>
        /// <value>Value MUST BE below or equal 0 if not set, or 1,2,or 4 NO OTHER VALUES!</value>
        public int GraphicsAlphaBit
        {
            get
            {
                return this._iGraphicsAlphaBit;
            }
            set
            {
                if ((value > 4) | (value == 3))
                {
                    throw new ArgumentOutOfRangeException("The Graphics Alpha Bit must have a value between 1 2 and 4, or <= 0 if not set");
                }
                this._iGraphicsAlphaBit = value;
            }
        }

        public int Height
        {
            get
            {
                return this._iHeight;
            }
            set
            {
                this._iHeight = value;
            }
        }

        /// <summary>Quality of compression of JPG</summary>
        public int JPEGQuality
        {
            get
            {
                return this._iJPEGQuality;
            }
            set
            {
                this._iJPEGQuality = value;
            }
        }

        /// <summary>The last page to conver in an image</summary>
        public int LastPageToConvert
        {
            get
            {
                return this._iLastPageToConvert;
            }
            set
            {
                this._iLastPageToConvert = value;
            }
        }

        public int MaxBitmap
        {
            get
            {
                return this._iMaxBitmap;
            }
            set
            {
                this._iMaxBitmap = value;
            }
        }

        public int MaxBuffer
        {
            get
            {
                return this._iMaxBuffer;
            }
            set
            {
                this._iMaxBuffer = value;
            }
        }

        /// <summary>
        /// What format to use to convert
        /// is suggested to use png256 instead of jpeg for document!
        /// they are smaller and better suited!
        /// </summary>
        public string OutputFormat
        {
            get
            {
                return this._sDeviceFormat;
            }
            set
            {
                this._sDeviceFormat = value;
            }
        }

        /// <summary>If true i will try to output everypage to a different file!</summary>
        public bool OutputToMultipleFile
        {
            get
            {
                return this._didOutputToMultipleFile;
            }
            set
            {
                this._didOutputToMultipleFile = value;
            }
        }

        public string ParametersUsed
        {
            get
            {
                return this._sParametersUsed;
            }
            set
            {
                this._sParametersUsed = value;
            }
        }

        /// <summary>If i should redirect the Output of Ghostscript library somewhere</summary>
        public bool RedirectIO
        {
            get
            {
                return this._bRedirectIO;
            }
            set
            {
                this._bRedirectIO = value;
            }
        }

        /// <summary>In how many thread i should perform the conversion</summary>
        /// <remarks>This is a Major innovation since 8.63 NEVER use it with previous version!</remarks>
        /// <value>Set it to 0 made the program set it to Environment.ProcessorCount HT machine could want to perform a check for this..</value>
        public int RenderingThreads
        {
            get
            {
                return this._iRenderingThreads;
            }
            set
            {
                if (value == 0)
                {
                    this._iRenderingThreads = Environment.ProcessorCount;
                }
                else
                {
                    this._iRenderingThreads = value;
                }
            }
        }

        public int ResolutionX
        {
            get
            {
                return this._iResolutionX;
            }
            set
            {
                this._iResolutionX = value;
            }
        }

        public int ResolutionY
        {
            get
            {
                return this._iResolutionY;
            }
            set
            {
                this._iResolutionY = value;
            }
        }

        /// <summary>This parameter is used to control subsample antialiasing of text</summary>
        /// <value>Value MUST BE below or equal 0 if not set, or 1,2,or 4 NO OTHER VALUES!</value>
        public int TextAlphaBit
        {
            get
            {
                return this._iTextAlphaBit;
            }
            set
            {
                if ((value > 4) | (value == 3))
                {
                    throw new ArgumentOutOfRangeException("The Text ALpha Bit must have a value between 1 2 and 4, or <= 0 if not set");
                }
                this._iTextAlphaBit = value;
            }
        }

        /// <summary>Set to True if u want the program to never display Messagebox
        /// but otherwise throw exception</summary>
        public bool ThrowOnlyException
        {
            get
            {
                return this._bThrowOnlyException;
            }
            set
            {
                this._bThrowOnlyException = value;
            }
        }

        public string UserPassword
        {
            get
            {
                return this._sUserPassword;
            }
            set
            {
                this._sUserPassword = value;
            }
        }

        public int Width
        {
            get
            {
                return this._iWidth;
            }
            set
            {
                this._iWidth = value;
            }
        }
    }
}