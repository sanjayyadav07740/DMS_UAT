using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Data;

namespace DMS.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IDMSUploadDownloadWCFService" in both code and config file together.
    [ServiceContract]
    public interface IDMSUploadDownloadWCFService
    {
        [OperationContract(Name = "Upload Document To Server")]
        bool UploadDocumentToServer(string filePath, byte[] objFileByte, bool boolIsNew, int intContentLength);

        [OperationContract]
        byte[] DownloadDocumentFromServer(string filePath);

        [OperationContract(Name = "Upload Document To Server With Detail")]
        bool UploadDocumentToServer(byte[] objFileByte, DMS.BusinessLogic.Document objDocument);

        [OperationContract]
        string GetUniqueFileName(string strExtension);

        [OperationContract]
        int InsertMetaData(DMS.BusinessLogic.MetaData objMetaData);

        [OperationContract]
        bool InsertDocument(DMS.BusinessLogic.Document objDocument);

        [OperationContract]
        DataTable LoadRepository();

        [OperationContract]
        DataTable LoadMetaTemplate(int intRepositoryID);

        [OperationContract]
        DataTable LoadCategory(int intMetaTemplateID);

        [OperationContract]
        DataTable LoadFolder(int intMetaTemplateID);

        [OperationContract]
        DataTable LoadMetaDataCode(DMS.BusinessLogic.MetaData objMetaData);

        [OperationContract]
        DataTable LoadDocument(string query);

        [OperationContract]
        DataTable AuthenticateUser(string userName, string password);
    }
}
