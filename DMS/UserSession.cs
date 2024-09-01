using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI;
using System.ComponentModel;
using System.Reflection;

namespace DMS
{
    public class UserSession : System.Web.UI.Page
    {
        public const string SortExpression = "SortExpression";

        public static DataTable AccessRights
        {
            get
            {
                if (HttpContext.Current.Session["AccessRights"] != null && HttpContext.Current.Session != null)
                {
                    return (DataTable)HttpContext.Current.Session["AccessRights"];
                }
                return null;
            }
            set
            {
                if (HttpContext.Current.Session != null)
                    HttpContext.Current.Session["AccessRights"] = value;
            }
        }

        public static DataTable Menu
        {
            get
            {
                if (HttpContext.Current.Session["Menu"] != null && HttpContext.Current.Session != null)
                {
                    return (DataTable)HttpContext.Current.Session["Menu"];
                }
                return null;
            }
            set
            {
                if (HttpContext.Current.Session != null)
                    HttpContext.Current.Session["Menu"] = value;
            }
        }

        public static DataTable GridData
        {
            get
            {
                if (HttpContext.Current.Session["GridData"] != null && HttpContext.Current.Session != null)
                {
                    return (DataTable)HttpContext.Current.Session["GridData"];
                }
                return null;
            }
            set
            {
                if (HttpContext.Current.Session != null)
                    HttpContext.Current.Session["GridData"] = value;
            }
        }

        public static DataTable FilterData
        {
            get
            {
                if (HttpContext.Current.Session["FilterData"] != null && HttpContext.Current.Session != null)
                {
                    return (DataTable)HttpContext.Current.Session["FilterData"];
                }
                return null;
            }
            set
            {
                if (HttpContext.Current.Session != null)
                    HttpContext.Current.Session["FilterData"] = value;
            }
        }

        public static DataView SortedGridData(string strSortExpression,string strLastSortDirection)
        {
            
                DataView objDataView = UserSession.GridData.DefaultView;
                objDataView.Sort = strSortExpression + " " + strLastSortDirection;
                return objDataView;
        }

        public static DataView SortedFilterGridData(string strSortExpression, string strLastSortDirection)
        {

            DataView objDataView = UserSession.FilterData.DefaultView;
            objDataView.Sort = strSortExpression + " " + strLastSortDirection;
            return objDataView;
        }

        public static DataTable Document
        {
            get
            {
                if (HttpContext.Current.Session["Document"] != null && HttpContext.Current.Session != null)
                {
                    return (DataTable)HttpContext.Current.Session["Document"];
                }
                return null;
            }
            set
            {
                if (HttpContext.Current.Session != null)
                    HttpContext.Current.Session["Document"] = value;
            }
        }

        public static DataTable Field
        {
            get
            {
                if (HttpContext.Current.Session["Field"] != null && HttpContext.Current.Session != null)
                {
                    return (DataTable)HttpContext.Current.Session["Field"];
                }
                return null;
            }
            set
            {
                if (HttpContext.Current.Session != null)
                    HttpContext.Current.Session["Field"] = value;
            }
        }

        public static DataTable List
        {
            get
            {
                if (HttpContext.Current.Session["List"] != null && HttpContext.Current.Session != null)
                {
                    return (DataTable)HttpContext.Current.Session["List"];
                }
                return null;
            }
            set
            {
                if (HttpContext.Current.Session != null)
                    HttpContext.Current.Session["List"] = value;
            }
        }

        public static DataTable ListItem
        {
            get
            {
                if (HttpContext.Current.Session["ListItem"] != null && HttpContext.Current.Session != null)
                {
                    return (DataTable)HttpContext.Current.Session["ListItem"];
                }
                return null;
            }
            set
            {
                if (HttpContext.Current.Session != null)
                    HttpContext.Current.Session["ListItem"] = value;
            }
        }

        public static DataTable MetaTemplateField
        {
            get
            {
                if (HttpContext.Current.Session["MetaTemplateField"] != null && HttpContext.Current.Session != null)
                {
                    return (DataTable)HttpContext.Current.Session["MetaTemplateField"];
                }
                return null;
            }
            set
            {
                if (HttpContext.Current.Session != null)
                    HttpContext.Current.Session["MetaTemplateField"] = value;
            }
        }

        public static DataTable MetaTemplateFieldListItem
        {
            get
            {
                if (HttpContext.Current.Session["MetaTemplateFieldListItem"] != null && HttpContext.Current.Session != null)
                {
                    return (DataTable)HttpContext.Current.Session["MetaTemplateFieldListItem"];
                }
                return null;
            }
            set
            {
                if (HttpContext.Current.Session != null)
                    HttpContext.Current.Session["MetaTemplateFieldListItem"] = value;
            }
        }

        public static void DisplayMessage(Page objPage, string strMessage, MainMasterPage.MessageType enumMessageType)
        {
            if (strMessage.Contains("Sorry ,Some Error Has Been Occured ."))
                strMessage = "The operation could not be completed because an unexpected error occurred.";

            MethodInfo objMethodInfo = objPage.Master.GetType().GetMethod("DisplayMessage");
            object result = objMethodInfo.Invoke(objPage.Master, new object[] { strMessage, enumMessageType });
        }

        public static int IsCreateCategory
        {
            get
            {
                if(HttpContext.Current.Session["IsCreateCategory"] !=null && HttpContext.Current.Session != null)
                {
                    return Convert.ToInt32(HttpContext.Current.Session["IsCreateCategory"]);
                }
                return 0;
            }
            set
            {
                if(HttpContext.Current.Session!= null)
                HttpContext.Current.Session["IsCreateCategory"] = value;
            }
        }

        public static int IsCreateFolder
        {
            get
            {
                if (HttpContext.Current.Session["IsCreateFolder"] != null && HttpContext.Current.Session != null)
                {
                    return Convert.ToInt32(HttpContext.Current.Session["IsCreateFolder"]);
                }
                return 0;
            }
            set
            {
                if (HttpContext.Current.Session != null)
                    HttpContext.Current.Session["IsCreateFolder"] = value;
            }
        }

        public static int IsCreateRepository
        {
            get
            {
                if (HttpContext.Current.Session["IsCreateRepository"] != null && HttpContext.Current.Session != null)
                {
                    return Convert.ToInt32(HttpContext.Current.Session["IsCreateRepository"]);
                }
                return 0;
            }
            set
            {
                if (HttpContext.Current.Session != null)
                    HttpContext.Current.Session["IsCreateRepository"] = value;
            }
        }

        public static int IsCreateMetaTemplate
        {
            get
            {
                if (HttpContext.Current.Session["IsCreateMetaTemplate"] != null && HttpContext.Current.Session != null)
                {
                    return Convert.ToInt32(HttpContext.Current.Session["IsCreateMetaTemplate"]);
                }
                return 0;
            }
            set
            {
                if (HttpContext.Current.Session != null)
                    HttpContext.Current.Session["IsCreateMetaTemplate"] = value;
            }
        }

        public static int RepositoryID
        {
            get
            {
                if (HttpContext.Current.Session["RepositoryID"] != null && HttpContext.Current.Session != null)
                {
                    return Convert.ToInt32(HttpContext.Current.Session["RepositoryID"]);
                }
                return 0;
            }
            set
            {
                if (HttpContext.Current.Session != null)
                    HttpContext.Current.Session["RepositoryID"] = value;
            }
        }

        public static int MetatemplateID
        {
            get
            {
                if (HttpContext.Current.Session["MetatemplateID"] != null && HttpContext.Current.Session != null)
                {
                    return Convert.ToInt32(HttpContext.Current.Session["MetatemplateID"]);
                }
                return 0;
            }
            set
            {
                if (HttpContext.Current.Session != null)
                    HttpContext.Current.Session["MetatemplateID"] = value;
            }
        } 

        public static int IsCreateUser
        {
            get
            {
                if (HttpContext.Current.Session["IsCreateUser"] != null && HttpContext.Current.Session != null)
                {
                    return Convert.ToInt32(HttpContext.Current.Session["IsCreateUser"]);
                }
                return 0;
            }
            set
            {
                if (HttpContext.Current.Session != null)
                    HttpContext.Current.Session["IsCreateUser"] = value;
            }
        }

        public static int IsCreateRole
        {
            get
            {
                if (HttpContext.Current.Session["IsCreateRole"] != null && HttpContext.Current.Session != null)
                {
                    return Convert.ToInt32(HttpContext.Current.Session["IsCreateRole"]);
                }
                return 0;
            }
            set
            {
                if (HttpContext.Current.Session != null)
                    HttpContext.Current.Session["IsCreateRole"] = value;
            }
        }

        public static int IsCreateRoleModule
        {
            get
            {
                if (HttpContext.Current.Session["IsCreateRoleModule"] != null && HttpContext.Current.Session != null)
                {
                    return Convert.ToInt32(HttpContext.Current.Session["IsCreateRoleModule"]);
                }
                return 0;
            }
            set
            {
                if (HttpContext.Current.Session != null)
                    HttpContext.Current.Session["IsCreateRoleModule"] = value;
            }
        }

        public static int UserID
        {
            get
            {
                if (HttpContext.Current.Session["UserID"] != null && HttpContext.Current.Session != null)
                {
                    return Convert.ToInt32(HttpContext.Current.Session["UserID"]);
                }
                return 0;
            }
            set
            {
                if (HttpContext.Current.Session != null)
                    HttpContext.Current.Session["UserID"] = value;
            }
        }

        public static int RoleID
        {
            get
            {
                if (HttpContext.Current.Session["RoleID"] != null && HttpContext.Current.Session != null)
                {
                    return Convert.ToInt32(HttpContext.Current.Session["RoleID"]);
                }
                return 0;
            }
            set
            {
                if (HttpContext.Current.Session != null)
                    HttpContext.Current.Session["RoleID"] = value;
            }
        }

        public static int RoleType
        {
            get
            {
                if (HttpContext.Current.Session["RoleType"] != null && HttpContext.Current.Session != null)
                {
                    return Convert.ToInt32(HttpContext.Current.Session["RoleType"]);
                }
                return 0;
            }
            set
            {
                if (HttpContext.Current.Session != null)
                    HttpContext.Current.Session["RoleType"] = value;
            }
        }

        public static string UserPassword
        {
            get
            {
                if (HttpContext.Current.Session["UserPassword"] != null && HttpContext.Current.Session != null)
                {
                    return HttpContext.Current.Session["UserPassword"].ToString();
                }
                return null;
            }
            set
            {
                if (HttpContext.Current.Session != null)
                    HttpContext.Current.Session["UserPassword"] = value;
            }
        }

        public static int IsCreateMetaTemplateField
        {
            get
            {
                if (HttpContext.Current.Session["IsCreateMetaTemplateField"] != null && HttpContext.Current.Session != null)
                {
                    return Convert.ToInt32(HttpContext.Current.Session["IsCreateMetaTemplateField"]);
                }
                return 0;
            }
            set
            {
                if (HttpContext.Current.Session != null)
                    HttpContext.Current.Session["IsCreateMetaTemplateField"] = value;
            }
        }

        public static int MetaDataID
        {
            get
            {
                if (HttpContext.Current.Session["MetaDataID"] != null && HttpContext.Current.Session != null)
                {
                    return Convert.ToInt32(HttpContext.Current.Session["MetaDataID"]);
                }
                return 0;
            }
            set
            {
                if (HttpContext.Current.Session != null)
                    HttpContext.Current.Session["MetaDataID"] = value;
            }
        }

        public static byte[] FileByte
        {
            get
            {
                if (HttpContext.Current.Session["FileByte"] != null && HttpContext.Current.Session != null)
                {
                    return (byte[])HttpContext.Current.Session["FileByte"];
                }
                return null;
            }
            set
            {
                if (HttpContext.Current.Session != null)
                    HttpContext.Current.Session["FileByte"] = value;
            }
        }

        public static object UserSelectionInformation
        {
            get
            {
                if (HttpContext.Current.Session["UserSelectionInformation"] != null && HttpContext.Current.Session != null)
                {
                    return (object)HttpContext.Current.Session["UserSelectionInformation"];
                }
                return null;
            }
            set
            {
                if (HttpContext.Current.Session != null)
                    HttpContext.Current.Session["UserSelectionInformation"] = value;
            }
        }

        public static System.Collections.Hashtable TemporaryList
        {
            get
            {
                if (HttpContext.Current.Session["TemporaryList"] != null && HttpContext.Current.Session != null)
                {
                    return (System.Collections.Hashtable)HttpContext.Current.Session["TemporaryList"];
                }
                return null;
            }
            set
            {
                if (HttpContext.Current.Session != null)
                    HttpContext.Current.Session["TemporaryList"] = value;
            }
        }

        public static int LoginDetailID
        {
            get
            {
                if (HttpContext.Current.Session["LoginDetailID"] != null && HttpContext.Current.Session != null)
                {
                    return Convert.ToInt32(HttpContext.Current.Session["LoginDetailID"]);
                }
                return 0;
            }
            set
            {
                if (HttpContext.Current.Session != null)
                    HttpContext.Current.Session["LoginDetailID"] = value;
            }
        }

        public static string LastLogin
        {
            get
            {
                if (HttpContext.Current.Session["LastLogin"] != null && HttpContext.Current.Session != null)
                {
                    return Convert.ToString(HttpContext.Current.Session["LastLogin"]);
                }
                return null;
            }
            set
            {
                if (HttpContext.Current.Session != null)
                    HttpContext.Current.Session["LastLogin"] = value;
            }
        }

        public static string SourceFilePath
        {
            get
            {
                if (HttpContext.Current.Session["SourceFilePath"] != null && HttpContext.Current.Session != null)
                {
                    return Convert.ToString(HttpContext.Current.Session["SourceFilePath"]);
                }
                return null;
            }
            set
            {
                if (HttpContext.Current.Session != null)
                    HttpContext.Current.Session["SourceFilePath"] = value;
            }
        }

        public static string TempFilePath
        {
            get
            {
                if (HttpContext.Current.Session["TempFilePath"] != null && HttpContext.Current.Session != null)
                {
                    return Convert.ToString(HttpContext.Current.Session["TempFilePath"]);
                }
                return null;
            }
            set
            {
                if (HttpContext.Current.Session != null)
                    HttpContext.Current.Session["TempFilePath"] = value;
            }
        }
    }
}