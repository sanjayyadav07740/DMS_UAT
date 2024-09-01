using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;

namespace DMS.BusinessLogic
{
    public class RoleModule
    {
        #region Properties
        public int RoleModuleID { get; set; }
        public int RoleID { get; set; }
        public int ModuleID { get; set; }
        #endregion Properties

        # region Methods

        public static Utility.ResultType SelectAllByRoleID(out DataTable objDataTable,int intRoleID,Utility.Control enumControl)
        {
            try
            {
                string strQuery="";

                if (enumControl== Utility.Control.Treeview)
                {
                 strQuery = " SELECT *" +
                                   " FROM vwModule Module LEFT OUTER JOIN vwRoleModule RoleModule"
                                  + " ON  Module.ID=RoleModule.ModuleID "
                                  + " AND RoleModule.RoleID=@RoleID"
                                  + " WHERE Module.Status=1";
                }
                else if(enumControl== Utility.Control.Menu)
                {

                 strQuery = " SELECT *" +
                                  " FROM vwModule Module INNER JOIN vwRoleModule RoleModule"
                                 + " ON  Module.ID=RoleModule.ModuleID "
                                 + " AND RoleModule.RoleID=@RoleID"
                                 + " WHERE Module.Status=1";
                }

                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

                DbParameter[] objDbParameter = new DbParameter[1];
                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "RoleID";
                objDbParameter[0].Value = intRoleID;

                objDataTable = DataHelper.ExecuteDataTable(strQuery, objDbParameter);

                if (objDataTable.Rows.Count == 0)
                {
                    return Utility.ResultType.Failure;
                }
                return Utility.ResultType.Success;
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                objDataTable = null;
                return Utility.ResultType.Error;

            }
        }

        public static Utility.ResultType Insert(RoleModule objRoleModule, DbTransaction objDbTransaction)
        {
            try
            {                
                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;
            
                DbParameter[] objDbParameter = new DbParameter[2];

                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "RoleID";
                objDbParameter[0].Value = objRoleModule.RoleID;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "ModuleID";
                objDbParameter[1].Value = objRoleModule.ModuleID;

                DataHelper.ExecuteNonQuery("SP_I_RoleModule", objDbTransaction, objDbParameter);
              
                return Utility.ResultType.Success;
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return Utility.ResultType.Error;

            }
        }

        public static Utility.ResultType DeleteByRoleId(int intRoleID, DbTransaction objDbTransaction)
        {
         
            DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

            try
            {
                DbParameter[] objDbParameter = new DbParameter[1];

                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "RoleID";
                objDbParameter[0].Value = intRoleID;

                DataHelper.ExecuteNonQuery("SP_D_RoleModule", objDbTransaction, objDbParameter);
            
                return Utility.ResultType.Success;
    
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return Utility.ResultType.Error;

            
            }
        }

        #endregion
    }
  
}
