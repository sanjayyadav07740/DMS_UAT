using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;

namespace DMS.BusinessLogic
{
   public class AppSetting
   {
       #region Private Member
       private static DataTable objDataTable;
       #endregion

       #region Properties
        public static DataTable SettingData
        {
            get
            {
                if (objDataTable != null)
                {
                    return objDataTable;
                }
                else
                {
                    objDataTable = LoadAppSettingData();
                    return objDataTable;
                }
            }
        }
        #endregion

        #region Method
        private static DataTable LoadAppSettingData()
        {
            string strQuery = "SELECT * FROM vwAppSetting WHERE Status = 1";
            DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;
            return DataHelper.ExecuteDataTable(strQuery, null);
        }
        #endregion

    }
}
