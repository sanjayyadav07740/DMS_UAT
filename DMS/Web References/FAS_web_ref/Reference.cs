﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.42000.
// 
#pragma warning disable 1591

namespace DMS.FAS_web_ref {
    using System.Diagnostics;
    using System;
    using System.Xml.Serialization;
    using System.ComponentModel;
    using System.Web.Services.Protocols;
    using System.Web.Services;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.9032.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="SaleInvoiceSoap", Namespace="http://tempuri.org/")]
    public partial class SaleInvoice : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback HelloWorldOperationCompleted;
        
        private System.Threading.SendOrPostCallback ImportSaleInvoiceOperationCompleted;
        
        private System.Threading.SendOrPostCallback ImportSaleInvoiceByLocationOperationCompleted;
        
        private System.Threading.SendOrPostCallback ImportSaleInvoiceByLocation1OperationCompleted;
        
        private System.Threading.SendOrPostCallback CreateCreditNoteOperationCompleted;
        
        private System.Threading.SendOrPostCallback AddPrintTextOperationCompleted;
        
        private System.Threading.SendOrPostCallback AddDebitNotePrintTextOperationCompleted;
        
        private System.Threading.SendOrPostCallback AddCreditNotePrintTextOperationCompleted;
        
        private System.Threading.SendOrPostCallback HIOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public SaleInvoice() {
            this.Url = global::DMS.Properties.Settings.Default.DMS_FAS_web_ref_SaleInvoice;
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event HelloWorldCompletedEventHandler HelloWorldCompleted;
        
        /// <remarks/>
        public event ImportSaleInvoiceCompletedEventHandler ImportSaleInvoiceCompleted;
        
        /// <remarks/>
        public event ImportSaleInvoiceByLocationCompletedEventHandler ImportSaleInvoiceByLocationCompleted;
        
        /// <remarks/>
        public event ImportSaleInvoiceByLocation1CompletedEventHandler ImportSaleInvoiceByLocation1Completed;
        
        /// <remarks/>
        public event CreateCreditNoteCompletedEventHandler CreateCreditNoteCompleted;
        
        /// <remarks/>
        public event AddPrintTextCompletedEventHandler AddPrintTextCompleted;
        
        /// <remarks/>
        public event AddDebitNotePrintTextCompletedEventHandler AddDebitNotePrintTextCompleted;
        
        /// <remarks/>
        public event AddCreditNotePrintTextCompletedEventHandler AddCreditNotePrintTextCompleted;
        
        /// <remarks/>
        public event HICompletedEventHandler HICompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/HelloWorld", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string HelloWorld() {
            object[] results = this.Invoke("HelloWorld", new object[0]);
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void HelloWorldAsync() {
            this.HelloWorldAsync(null);
        }
        
        /// <remarks/>
        public void HelloWorldAsync(object userState) {
            if ((this.HelloWorldOperationCompleted == null)) {
                this.HelloWorldOperationCompleted = new System.Threading.SendOrPostCallback(this.OnHelloWorldOperationCompleted);
            }
            this.InvokeAsync("HelloWorld", new object[0], this.HelloWorldOperationCompleted, userState);
        }
        
        private void OnHelloWorldOperationCompleted(object arg) {
            if ((this.HelloWorldCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.HelloWorldCompleted(this, new HelloWorldCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/ImportSaleInvoice", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public void ImportSaleInvoice(System.DateTime SIDate, int CustomerID, System.Xml.XmlNode ItemIDs) {
            this.Invoke("ImportSaleInvoice", new object[] {
                        SIDate,
                        CustomerID,
                        ItemIDs});
        }
        
        /// <remarks/>
        public void ImportSaleInvoiceAsync(System.DateTime SIDate, int CustomerID, System.Xml.XmlNode ItemIDs) {
            this.ImportSaleInvoiceAsync(SIDate, CustomerID, ItemIDs, null);
        }
        
        /// <remarks/>
        public void ImportSaleInvoiceAsync(System.DateTime SIDate, int CustomerID, System.Xml.XmlNode ItemIDs, object userState) {
            if ((this.ImportSaleInvoiceOperationCompleted == null)) {
                this.ImportSaleInvoiceOperationCompleted = new System.Threading.SendOrPostCallback(this.OnImportSaleInvoiceOperationCompleted);
            }
            this.InvokeAsync("ImportSaleInvoice", new object[] {
                        SIDate,
                        CustomerID,
                        ItemIDs}, this.ImportSaleInvoiceOperationCompleted, userState);
        }
        
        private void OnImportSaleInvoiceOperationCompleted(object arg) {
            if ((this.ImportSaleInvoiceCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ImportSaleInvoiceCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/ImportSaleInvoiceByLocation", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string ImportSaleInvoiceByLocation(System.DateTime SIDate, int CustomerID, int LocationID, int OfficeID, int CompanyID, int CompanyType, int BusinessDivisionID, string SIType, string Message1, string SystemSource, int BranchID, string TypeOfExtension, System.Xml.XmlNode ItemIDs, string FilePath, string InvoiceNumber) {
            object[] results = this.Invoke("ImportSaleInvoiceByLocation", new object[] {
                        SIDate,
                        CustomerID,
                        LocationID,
                        OfficeID,
                        CompanyID,
                        CompanyType,
                        BusinessDivisionID,
                        SIType,
                        Message1,
                        SystemSource,
                        BranchID,
                        TypeOfExtension,
                        ItemIDs,
                        FilePath,
                        InvoiceNumber});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void ImportSaleInvoiceByLocationAsync(System.DateTime SIDate, int CustomerID, int LocationID, int OfficeID, int CompanyID, int CompanyType, int BusinessDivisionID, string SIType, string Message1, string SystemSource, int BranchID, string TypeOfExtension, System.Xml.XmlNode ItemIDs, string FilePath, string InvoiceNumber) {
            this.ImportSaleInvoiceByLocationAsync(SIDate, CustomerID, LocationID, OfficeID, CompanyID, CompanyType, BusinessDivisionID, SIType, Message1, SystemSource, BranchID, TypeOfExtension, ItemIDs, FilePath, InvoiceNumber, null);
        }
        
        /// <remarks/>
        public void ImportSaleInvoiceByLocationAsync(
                    System.DateTime SIDate, 
                    int CustomerID, 
                    int LocationID, 
                    int OfficeID, 
                    int CompanyID, 
                    int CompanyType, 
                    int BusinessDivisionID, 
                    string SIType, 
                    string Message1, 
                    string SystemSource, 
                    int BranchID, 
                    string TypeOfExtension, 
                    System.Xml.XmlNode ItemIDs, 
                    string FilePath, 
                    string InvoiceNumber, 
                    object userState) {
            if ((this.ImportSaleInvoiceByLocationOperationCompleted == null)) {
                this.ImportSaleInvoiceByLocationOperationCompleted = new System.Threading.SendOrPostCallback(this.OnImportSaleInvoiceByLocationOperationCompleted);
            }
            this.InvokeAsync("ImportSaleInvoiceByLocation", new object[] {
                        SIDate,
                        CustomerID,
                        LocationID,
                        OfficeID,
                        CompanyID,
                        CompanyType,
                        BusinessDivisionID,
                        SIType,
                        Message1,
                        SystemSource,
                        BranchID,
                        TypeOfExtension,
                        ItemIDs,
                        FilePath,
                        InvoiceNumber}, this.ImportSaleInvoiceByLocationOperationCompleted, userState);
        }
        
        private void OnImportSaleInvoiceByLocationOperationCompleted(object arg) {
            if ((this.ImportSaleInvoiceByLocationCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ImportSaleInvoiceByLocationCompleted(this, new ImportSaleInvoiceByLocationCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/ImportSaleInvoiceByLocation1", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string ImportSaleInvoiceByLocation1(
                    System.DateTime SIDate, 
                    int CustomerID, 
                    int LocationID, 
                    int OfficeID, 
                    int CompanyID, 
                    int CompanyType, 
                    int BusinessDivisionID, 
                    string SIType, 
                    string Message1, 
                    string SystemSource, 
                    int BranchID, 
                    string TypeOfExtension, 
                    System.Xml.XmlNode ItemIDs, 
                    string FilePath, 
                    string InvoiceNumber, 
                    ReceiptTagged[] lstSaleInvoiceTagged, 
                    int DebitNoteBy) {
            object[] results = this.Invoke("ImportSaleInvoiceByLocation1", new object[] {
                        SIDate,
                        CustomerID,
                        LocationID,
                        OfficeID,
                        CompanyID,
                        CompanyType,
                        BusinessDivisionID,
                        SIType,
                        Message1,
                        SystemSource,
                        BranchID,
                        TypeOfExtension,
                        ItemIDs,
                        FilePath,
                        InvoiceNumber,
                        lstSaleInvoiceTagged,
                        DebitNoteBy});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void ImportSaleInvoiceByLocation1Async(
                    System.DateTime SIDate, 
                    int CustomerID, 
                    int LocationID, 
                    int OfficeID, 
                    int CompanyID, 
                    int CompanyType, 
                    int BusinessDivisionID, 
                    string SIType, 
                    string Message1, 
                    string SystemSource, 
                    int BranchID, 
                    string TypeOfExtension, 
                    System.Xml.XmlNode ItemIDs, 
                    string FilePath, 
                    string InvoiceNumber, 
                    ReceiptTagged[] lstSaleInvoiceTagged, 
                    int DebitNoteBy) {
            this.ImportSaleInvoiceByLocation1Async(SIDate, CustomerID, LocationID, OfficeID, CompanyID, CompanyType, BusinessDivisionID, SIType, Message1, SystemSource, BranchID, TypeOfExtension, ItemIDs, FilePath, InvoiceNumber, lstSaleInvoiceTagged, DebitNoteBy, null);
        }
        
        /// <remarks/>
        public void ImportSaleInvoiceByLocation1Async(
                    System.DateTime SIDate, 
                    int CustomerID, 
                    int LocationID, 
                    int OfficeID, 
                    int CompanyID, 
                    int CompanyType, 
                    int BusinessDivisionID, 
                    string SIType, 
                    string Message1, 
                    string SystemSource, 
                    int BranchID, 
                    string TypeOfExtension, 
                    System.Xml.XmlNode ItemIDs, 
                    string FilePath, 
                    string InvoiceNumber, 
                    ReceiptTagged[] lstSaleInvoiceTagged, 
                    int DebitNoteBy, 
                    object userState) {
            if ((this.ImportSaleInvoiceByLocation1OperationCompleted == null)) {
                this.ImportSaleInvoiceByLocation1OperationCompleted = new System.Threading.SendOrPostCallback(this.OnImportSaleInvoiceByLocation1OperationCompleted);
            }
            this.InvokeAsync("ImportSaleInvoiceByLocation1", new object[] {
                        SIDate,
                        CustomerID,
                        LocationID,
                        OfficeID,
                        CompanyID,
                        CompanyType,
                        BusinessDivisionID,
                        SIType,
                        Message1,
                        SystemSource,
                        BranchID,
                        TypeOfExtension,
                        ItemIDs,
                        FilePath,
                        InvoiceNumber,
                        lstSaleInvoiceTagged,
                        DebitNoteBy}, this.ImportSaleInvoiceByLocation1OperationCompleted, userState);
        }
        
        private void OnImportSaleInvoiceByLocation1OperationCompleted(object arg) {
            if ((this.ImportSaleInvoiceByLocation1Completed != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ImportSaleInvoiceByLocation1Completed(this, new ImportSaleInvoiceByLocation1CompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/CreateCreditNote", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string CreateCreditNote(
                    System.DateTime SIDate, 
                    int CustomerID, 
                    int LocationID, 
                    int OfficeID, 
                    int CompanyID, 
                    int CompanyType, 
                    int BusinessDivisionID, 
                    string SIType, 
                    string Message1, 
                    string SystemSource, 
                    int BranchID, 
                    string TypeOfExtension, 
                    System.Xml.XmlNode ItemIDs, 
                    string FilePath, 
                    string InvoiceNumber, 
                    ReceiptTagged[] lstSaleInvoiceTagged, 
                    int DebitNoteBy) {
            object[] results = this.Invoke("CreateCreditNote", new object[] {
                        SIDate,
                        CustomerID,
                        LocationID,
                        OfficeID,
                        CompanyID,
                        CompanyType,
                        BusinessDivisionID,
                        SIType,
                        Message1,
                        SystemSource,
                        BranchID,
                        TypeOfExtension,
                        ItemIDs,
                        FilePath,
                        InvoiceNumber,
                        lstSaleInvoiceTagged,
                        DebitNoteBy});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void CreateCreditNoteAsync(
                    System.DateTime SIDate, 
                    int CustomerID, 
                    int LocationID, 
                    int OfficeID, 
                    int CompanyID, 
                    int CompanyType, 
                    int BusinessDivisionID, 
                    string SIType, 
                    string Message1, 
                    string SystemSource, 
                    int BranchID, 
                    string TypeOfExtension, 
                    System.Xml.XmlNode ItemIDs, 
                    string FilePath, 
                    string InvoiceNumber, 
                    ReceiptTagged[] lstSaleInvoiceTagged, 
                    int DebitNoteBy) {
            this.CreateCreditNoteAsync(SIDate, CustomerID, LocationID, OfficeID, CompanyID, CompanyType, BusinessDivisionID, SIType, Message1, SystemSource, BranchID, TypeOfExtension, ItemIDs, FilePath, InvoiceNumber, lstSaleInvoiceTagged, DebitNoteBy, null);
        }
        
        /// <remarks/>
        public void CreateCreditNoteAsync(
                    System.DateTime SIDate, 
                    int CustomerID, 
                    int LocationID, 
                    int OfficeID, 
                    int CompanyID, 
                    int CompanyType, 
                    int BusinessDivisionID, 
                    string SIType, 
                    string Message1, 
                    string SystemSource, 
                    int BranchID, 
                    string TypeOfExtension, 
                    System.Xml.XmlNode ItemIDs, 
                    string FilePath, 
                    string InvoiceNumber, 
                    ReceiptTagged[] lstSaleInvoiceTagged, 
                    int DebitNoteBy, 
                    object userState) {
            if ((this.CreateCreditNoteOperationCompleted == null)) {
                this.CreateCreditNoteOperationCompleted = new System.Threading.SendOrPostCallback(this.OnCreateCreditNoteOperationCompleted);
            }
            this.InvokeAsync("CreateCreditNote", new object[] {
                        SIDate,
                        CustomerID,
                        LocationID,
                        OfficeID,
                        CompanyID,
                        CompanyType,
                        BusinessDivisionID,
                        SIType,
                        Message1,
                        SystemSource,
                        BranchID,
                        TypeOfExtension,
                        ItemIDs,
                        FilePath,
                        InvoiceNumber,
                        lstSaleInvoiceTagged,
                        DebitNoteBy}, this.CreateCreditNoteOperationCompleted, userState);
        }
        
        private void OnCreateCreditNoteOperationCompleted(object arg) {
            if ((this.CreateCreditNoteCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.CreateCreditNoteCompleted(this, new CreateCreditNoteCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/AddPrintText", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public void AddPrintText(System.Xml.XmlNode ItemIDs) {
            this.Invoke("AddPrintText", new object[] {
                        ItemIDs});
        }
        
        /// <remarks/>
        public void AddPrintTextAsync(System.Xml.XmlNode ItemIDs) {
            this.AddPrintTextAsync(ItemIDs, null);
        }
        
        /// <remarks/>
        public void AddPrintTextAsync(System.Xml.XmlNode ItemIDs, object userState) {
            if ((this.AddPrintTextOperationCompleted == null)) {
                this.AddPrintTextOperationCompleted = new System.Threading.SendOrPostCallback(this.OnAddPrintTextOperationCompleted);
            }
            this.InvokeAsync("AddPrintText", new object[] {
                        ItemIDs}, this.AddPrintTextOperationCompleted, userState);
        }
        
        private void OnAddPrintTextOperationCompleted(object arg) {
            if ((this.AddPrintTextCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.AddPrintTextCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/AddDebitNotePrintText", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public void AddDebitNotePrintText(System.Xml.XmlNode ItemIDs) {
            this.Invoke("AddDebitNotePrintText", new object[] {
                        ItemIDs});
        }
        
        /// <remarks/>
        public void AddDebitNotePrintTextAsync(System.Xml.XmlNode ItemIDs) {
            this.AddDebitNotePrintTextAsync(ItemIDs, null);
        }
        
        /// <remarks/>
        public void AddDebitNotePrintTextAsync(System.Xml.XmlNode ItemIDs, object userState) {
            if ((this.AddDebitNotePrintTextOperationCompleted == null)) {
                this.AddDebitNotePrintTextOperationCompleted = new System.Threading.SendOrPostCallback(this.OnAddDebitNotePrintTextOperationCompleted);
            }
            this.InvokeAsync("AddDebitNotePrintText", new object[] {
                        ItemIDs}, this.AddDebitNotePrintTextOperationCompleted, userState);
        }
        
        private void OnAddDebitNotePrintTextOperationCompleted(object arg) {
            if ((this.AddDebitNotePrintTextCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.AddDebitNotePrintTextCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/AddCreditNotePrintText", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public void AddCreditNotePrintText(System.Xml.XmlNode ItemIDs) {
            this.Invoke("AddCreditNotePrintText", new object[] {
                        ItemIDs});
        }
        
        /// <remarks/>
        public void AddCreditNotePrintTextAsync(System.Xml.XmlNode ItemIDs) {
            this.AddCreditNotePrintTextAsync(ItemIDs, null);
        }
        
        /// <remarks/>
        public void AddCreditNotePrintTextAsync(System.Xml.XmlNode ItemIDs, object userState) {
            if ((this.AddCreditNotePrintTextOperationCompleted == null)) {
                this.AddCreditNotePrintTextOperationCompleted = new System.Threading.SendOrPostCallback(this.OnAddCreditNotePrintTextOperationCompleted);
            }
            this.InvokeAsync("AddCreditNotePrintText", new object[] {
                        ItemIDs}, this.AddCreditNotePrintTextOperationCompleted, userState);
        }
        
        private void OnAddCreditNotePrintTextOperationCompleted(object arg) {
            if ((this.AddCreditNotePrintTextCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.AddCreditNotePrintTextCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/HI", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string HI() {
            object[] results = this.Invoke("HI", new object[0]);
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void HIAsync() {
            this.HIAsync(null);
        }
        
        /// <remarks/>
        public void HIAsync(object userState) {
            if ((this.HIOperationCompleted == null)) {
                this.HIOperationCompleted = new System.Threading.SendOrPostCallback(this.OnHIOperationCompleted);
            }
            this.InvokeAsync("HI", new object[0], this.HIOperationCompleted, userState);
        }
        
        private void OnHIOperationCompleted(object arg) {
            if ((this.HICompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.HICompleted(this, new HICompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.9032.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/")]
    public partial class ReceiptTagged {
        
        private int idField;
        
        private int sourceJournalIDField;
        
        private int destinationJournalIDField;
        
        private int accountIDField;
        
        private int subLedgerIDField;
        
        private decimal amountField;
        
        private string journalReferenceField;
        
        private System.DateTime journalDateField;
        
        private int journalIDField;
        
        private decimal taggedAmountField;
        
        /// <remarks/>
        public int ID {
            get {
                return this.idField;
            }
            set {
                this.idField = value;
            }
        }
        
        /// <remarks/>
        public int SourceJournalID {
            get {
                return this.sourceJournalIDField;
            }
            set {
                this.sourceJournalIDField = value;
            }
        }
        
        /// <remarks/>
        public int DestinationJournalID {
            get {
                return this.destinationJournalIDField;
            }
            set {
                this.destinationJournalIDField = value;
            }
        }
        
        /// <remarks/>
        public int AccountID {
            get {
                return this.accountIDField;
            }
            set {
                this.accountIDField = value;
            }
        }
        
        /// <remarks/>
        public int SubLedgerID {
            get {
                return this.subLedgerIDField;
            }
            set {
                this.subLedgerIDField = value;
            }
        }
        
        /// <remarks/>
        public decimal Amount {
            get {
                return this.amountField;
            }
            set {
                this.amountField = value;
            }
        }
        
        /// <remarks/>
        public string JournalReference {
            get {
                return this.journalReferenceField;
            }
            set {
                this.journalReferenceField = value;
            }
        }
        
        /// <remarks/>
        public System.DateTime JournalDate {
            get {
                return this.journalDateField;
            }
            set {
                this.journalDateField = value;
            }
        }
        
        /// <remarks/>
        public int JournalID {
            get {
                return this.journalIDField;
            }
            set {
                this.journalIDField = value;
            }
        }
        
        /// <remarks/>
        public decimal TaggedAmount {
            get {
                return this.taggedAmountField;
            }
            set {
                this.taggedAmountField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.9032.0")]
    public delegate void HelloWorldCompletedEventHandler(object sender, HelloWorldCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.9032.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class HelloWorldCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal HelloWorldCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.9032.0")]
    public delegate void ImportSaleInvoiceCompletedEventHandler(object sender, System.ComponentModel.AsyncCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.9032.0")]
    public delegate void ImportSaleInvoiceByLocationCompletedEventHandler(object sender, ImportSaleInvoiceByLocationCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.9032.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ImportSaleInvoiceByLocationCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal ImportSaleInvoiceByLocationCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.9032.0")]
    public delegate void ImportSaleInvoiceByLocation1CompletedEventHandler(object sender, ImportSaleInvoiceByLocation1CompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.9032.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ImportSaleInvoiceByLocation1CompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal ImportSaleInvoiceByLocation1CompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.9032.0")]
    public delegate void CreateCreditNoteCompletedEventHandler(object sender, CreateCreditNoteCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.9032.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class CreateCreditNoteCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal CreateCreditNoteCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.9032.0")]
    public delegate void AddPrintTextCompletedEventHandler(object sender, System.ComponentModel.AsyncCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.9032.0")]
    public delegate void AddDebitNotePrintTextCompletedEventHandler(object sender, System.ComponentModel.AsyncCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.9032.0")]
    public delegate void AddCreditNotePrintTextCompletedEventHandler(object sender, System.ComponentModel.AsyncCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.9032.0")]
    public delegate void HICompletedEventHandler(object sender, HICompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.9032.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class HICompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal HICompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591