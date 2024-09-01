using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace SEBIPDFSearch
{
    public static class Logger
    {
        //public static string Log(string logtext)
        //{
        //    string filename = "D:\\Log\\Log.txt";
        //    StringBuilder sb = new StringBuilder();
        //    sb.Append(File.ReadAllText(filename));
        //    sb.Append("\n");
        //    sb.Append(logtext);

        //    File.WriteAllText(filename, sb.ToString());

        //    return sb.ToString();
        //}

        //public static void WriteToFile(string Message)
        //{
        //    string path = AppDomain.CurrentDomain.BaseDirectory + "\\Logs";
        //    if (!Directory.Exists(path))
        //    {
        //        Directory.CreateDirectory(path);
        //    }
        //    string filepath = AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\Log_" + DateTime.Now.Date.ToShortDateString().Replace('/', '_') + ".txt";
        //    if (!File.Exists(filepath))
        //    {
        //        // Create a file to write to.   
        //        using (StreamWriter sw = File.CreateText(filepath))
        //        {
        //            sw.WriteLine(Message+" : "+DateTime.Now);
        //        }
        //    }
        //    else
        //    {
        //        using (StreamWriter sw = File.AppendText(filepath))
        //        {
        //            sw.WriteLine(Message + " : " + DateTime.Now);
        //        }
        //    }
        //}



        public static void WriteToFile(Exception ex){

             string message = string.Format("Time: {0}", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"));
             message += Environment.NewLine;
             message += "-----------------------------------------------------------";
             message += Environment.NewLine;
             message += string.Format("Message: {0}", ex.Message);
             message += Environment.NewLine;
             message += string.Format("StackTrace: {0}", ex.StackTrace);
             message += Environment.NewLine;
             message += string.Format("Source: {0}", ex.Source);
             message += Environment.NewLine;
             message += string.Format("TargetSite: {0}", ex.TargetSite.ToString());
             message += Environment.NewLine;
             message += "-----------------------------------------------------------";
             message += Environment.NewLine;

            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Logs";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filepath = AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\ErrorLog_" + DateTime.Now.Date.ToShortDateString().Replace('/', '_') + ".txt";
            if (!File.Exists(filepath))
            {
                // Create a file to write to.   
                using (StreamWriter sw = File.CreateText(filepath))
                {
                    sw.WriteLine(message);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    sw.WriteLine(message);
                }
            }
        }

       // public static void WriteMsgToFile(string msg, [CallerLineNumber] int lineNumber = 0,[CallerMemberName] string caller = null)
        public static void WriteMsgToFile(string msg)
        {
            StackFrame callStack = new StackFrame(1, true);
            string message = string.Format("Time: {0}", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"));
            message += Environment.NewLine;
            message += "-----------------------------------------------------------";
            message += Environment.NewLine;
            message += string.Format("Message: {0}", msg);
            message += Environment.NewLine;
            message += string.Format("File Name: {0}", callStack.GetFileName().ToString());
            message += string.Format(" Line: {0}", callStack.GetFileLineNumber().ToString());
            message += Environment.NewLine;
            message += string.Format("Method Name: {0}", callStack.GetMethod().ToString());
            message += Environment.NewLine;
            message += "-----------------------------------------------------------";
            message += Environment.NewLine;

            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Logs";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filepath = AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\MessageLog_" + DateTime.Now.Date.ToShortDateString().Replace('/', '_') + ".txt";
            if (!File.Exists(filepath))
            {
                // Create a file to write to.   
                using (StreamWriter sw = File.CreateText(filepath))
                {
                    sw.WriteLine(message);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    sw.WriteLine(message);
                }
            }
        }
    }

}

