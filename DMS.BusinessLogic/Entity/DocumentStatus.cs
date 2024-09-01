using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;

namespace DMS.BusinessLogic
{
    public class DocumentStatus
    {
        public int DocumentStatusID { get; set; }
        public string StatusName { get; set; }
        public int UserId { get; set; }
        public string statusAproveRej { get; set; }
        public string UserName { get; set; }
        public int DocId { get; set; }
        public DateTime ApprovedOn { get; set; }
        public DateTime RejectedOn { get; set; }

        public static Utility.ResultType InsertDocApproveRejectDetails(DocumentStatus objDocumentStaus, DbTransaction objDbTransaction)
        {
            DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;
            try
            {
                DbParameter[] objDbParameter = new DbParameter[5];

                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "DocId";
                objDbParameter[0].Value = objDocumentStaus.DocId;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "UserId";
                objDbParameter[1].Value = objDocumentStaus.UserId;

                objDbParameter[2] = objDbProviderFactory.CreateParameter();
                objDbParameter[2].ParameterName = "UserName";
                objDbParameter[2].Value = objDocumentStaus.UserName;

                objDbParameter[3] = objDbProviderFactory.CreateParameter();
                objDbParameter[3].ParameterName = "AppoveOn";
                objDbParameter[3].Value = DateTime.Now;

                objDbParameter[4] = objDbProviderFactory.CreateParameter();
                objDbParameter[4].ParameterName = "Status";
                objDbParameter[4].Value = objDocumentStaus.statusAproveRej;

                DataHelper.ExecuteNonQuery("SP_I_DocApproveRejDetails", objDbTransaction, objDbParameter);

                return Utility.ResultType.Success;
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return Utility.ResultType.Error;
            }
        }
    }
}
