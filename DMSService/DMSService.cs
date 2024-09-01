using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using DMS.BusinessLogic;
using DMS.BusinessLogic.Service;

namespace DMSService
{
    public partial class DMSService : ServiceBase
    {
        System.Timers.Timer objTimer;
        public DMSService()
        {
            try
            {
                InitializeComponent();
                objTimer = new System.Timers.Timer();
                objTimer.Elapsed += new System.Timers.ElapsedEventHandler(objTimer_Elapsed);
                objTimer.Enabled = true;
                objTimer.Interval = DMS.BusinessLogic.Utility.Interval;
                LogManager.ErrorLog(Utility.LogFilePath, "SERVICE IS INITIALIZED");
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                objTimer.Start();
                LogManager.ErrorLog(Utility.LogFilePath, "SERVICE HAS BEEN STARTED");
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        protected override void OnStop()
        {
            try
            {
                objTimer.Stop();
                LogManager.ErrorLog(Utility.LogFilePath, "SERVICE HAS BEEN STOPED");
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        private void objTimer_Elapsed(object sender, EventArgs e)
        {
            try
            {
                objTimer.Stop();

                DMS.BusinessLogic.Service.DocumentForLucene objDocumentForLucene = new DMS.BusinessLogic.Service.DocumentForLucene();
                objDocumentForLucene.StartLucene();
                DMS.BusinessLogic.Service.DocumentForBulkUpload objDocumentForBulkUpload = new DMS.BusinessLogic.Service.DocumentForBulkUpload();
                objDocumentForBulkUpload.StartBulkUpload();
               // DMS.BusinessLogic.Service.DocumentExpiry objDocumentExpiry = new DMS.BusinessLogic.Service.DocumentExpiry();
                //objDocumentExpiry.SendMail();
                objTimer.Start();
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }
    }
}
