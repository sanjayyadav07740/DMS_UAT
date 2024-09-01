using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace DMS.BusinessLogic
{    
    public class LogManager
    {

        public LogManager()
        {
            
        }

        public static void ErrorLog(string strPathName, string strErrMsg)
        {
            try
            {

                string strFileName = DateTime.Now.ToString("dd-MMM-yyyy");
                string strLogFormat = DateTime.Now.ToLongDateString().ToString() + " " + DateTime.Now.ToLongTimeString().ToString() + " ==> ";

                StreamWriter sw = new StreamWriter(strPathName + strFileName + ".txt", true);
                sw.WriteLine(strLogFormat + strErrMsg);
                sw.WriteLine("");
                sw.WriteLine("=======================================================================================================================================");
                sw.WriteLine("");
                sw.Flush();
                sw.Close();
            }
            catch (Exception ex)
            {

            }

        }

        public static void ErrorLog(string strPathName, Exception  objException)
        {
            try
            {

                string strFileName = DateTime.Now.ToString("dd-MMM-yyyy");
                string strLogFormat = DateTime.Now.ToLongDateString().ToString() + " " + DateTime.Now.ToLongTimeString().ToString() + " ==> ";

                StreamWriter sw = new StreamWriter(strPathName + strFileName + ".txt", true);
                sw.WriteLine(strLogFormat + objException.Message);
                sw.WriteLine("\t\t\t\t\t===========================================");
                sw.WriteLine("\t\t\t\t\tLocation");
                sw.WriteLine("\t\t\t\t\t===========================================");                
                sw.WriteLine("\t\t\t\t\t"+ objException.StackTrace);
                sw.WriteLine("=======================================================================================================================================");
                sw.WriteLine("");
                sw.Flush();
                sw.Close();
            }
            catch (Exception ex)
            {

            }

        }
    }   
}
