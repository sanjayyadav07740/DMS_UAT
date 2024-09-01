using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DMS.BusinessLogic
{
   public class ReportManager
    {
        #region Private Member
        Utility objUtility = new Utility();
        #endregion

        public Utility.ResultType SelectDocuments(out DataTable objDataTable)
        {

            return Report.Select(out objDataTable);

        }
       public DataSet SelectAuditLog(int RepositoryID)
        {
            return Report.SelectAuditLog(RepositoryID);
        }
       public DataSet SelectAuditLogUser(int UserID)
       {
           return Report.SelectAuditLogUser(UserID);
       }

       public Utility.ResultType SelectCentrumBillingReport(out DataTable objDataTable, string SelectedFromDate, string SelectedToDate, int MetatemplateID)
       {
            return Report.SelectCentrumBillingReport(out  objDataTable,  SelectedFromDate,  SelectedToDate,  MetatemplateID);
        
       }

       public void InsertAuditLog_MhadaWebsite(string IPAddress, string MacAddress, string Activity, int DocID)
       {
           Report objReport = new Report();
           objReport.InsertAuditLog_MhadaWebsite(IPAddress, MacAddress, Activity, DocID);
       }
    }
}
