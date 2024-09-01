using DMS.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.NetworkInformation;
using System.Web;
using System.Net;

namespace DMS
{
    public class Log
    {
        Report objRepot = new Report();

        public static void AuditLog(HttpContext context, string Activity, string PageName)
        {
            Log objlog = new Log();
            objlog.objRepot.InsertAuditLogNew(objlog.GetIPAddress(), GetMacAddress(), Activity, PageName, UserSession.UserID);
        }

        public static void DocumentAuditLog(HttpContext context, string Activity, string PageName, int DocId)
        {
            Log objlog = new Log();
            objlog.objRepot.InsertAuditLogDoc(objlog.GetIPAddress(), GetMacAddress(), Activity, PageName, UserSession.UserID, DocId);

        }

        public static void _DocumentAuditLog(HttpContext context, string Activity, string PageName, DataTable DocId)
        {
            Log objlog = new Log();
            objlog.objRepot._InsertAuditLogDoc(objlog.GetIPAddress(), GetMacAddress(), Activity, PageName, UserSession.UserID, DocId);

        }

        public string GetIPAddress(HttpContext context)
        {
            string ipList = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipList))
            {
                return ipList.Split(',')[0];
            }

            return context.Request.ServerVariables["REMOTE_ADDR"];

        }

        public string GetIPAddress()
        {
            string IPAddress = "";
            IPHostEntry Host = default(IPHostEntry);
            string Hostname = null;
            Hostname = System.Environment.MachineName;
            Host = Dns.GetHostEntry(Hostname);
            foreach (IPAddress IP in Host.AddressList)
            {
                if (IP.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    IPAddress = Convert.ToString(IP);
                }
            }
            return IPAddress;

        }


        public static string GetMacAddress()
        {
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                // Only consider Ethernet network interfaces
                if (nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet &&
                    nic.OperationalStatus == OperationalStatus.Up)
                {
                    return nic.GetPhysicalAddress().ToString();
                }
            }
            return null;
        }

        public static void DocumentRenameLog(int DocId, string OlDDocumentName, string NewDocumentName, HttpContext context)
        {
            Log objlog = new Log();
            objlog.objRepot.InsertDocRenameLog(DocId, OlDDocumentName, NewDocumentName, objlog.GetIPAddress(), UserSession.UserID);

        }

    }
}