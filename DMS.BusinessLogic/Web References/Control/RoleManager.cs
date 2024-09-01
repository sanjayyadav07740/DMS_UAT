using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace DMS.BusinessLogic
{
    public class RoleManager
    {
       
        # region Methods

        public Utility.ResultType SelectRole(out DataTable objDataTable)
        {
            return Role.Select(out objDataTable);
        }

        public Utility.ResultType SelectRole(out DataTable objDataTable, int intRoleID)
        {

          return Role.SelectByRoleID(out objDataTable, intRoleID);
            
        }

        public DataSet SearchRoleByRep(int RepId)
        {

            return Role.SearchRoleByRep(RepId);

        }

        public DataSet Select(int intRoleID)
        {

            return Role.SelectRole(intRoleID);

        }

        public DataSet SelectRoleLike(string RoleName)
        {

            return Role.SelectRoleLike(RoleName);

        }

        public Utility.ResultType SelectRoleModule(out DataTable objDataTable,int intRoleID,Utility.Control enumControl)
        {

            return RoleModule.SelectAllByRoleID(out objDataTable, intRoleID, enumControl);

        }

        public Utility.ResultType SelectRole(string strRoleName)
        {
            return Role.SelectByRoleName(strRoleName);
        }

        public Utility.ResultType InsertRole(Role objRole)
        {
            return Role.Insert(objRole);

        }

        public Utility.ResultType InsertRolePermission(int RoleID,int RepositoryID,int MetatemplateID,int CategoryID,int FolderID ,int UpdatedBy)
        {
            return Role.InsertRolePermission( RoleID, RepositoryID, MetatemplateID, CategoryID, FolderID , UpdatedBy);

        }

        public Utility.ResultType InsertRoleModule(RoleModule objRoleModule, DbTransaction objDbTransaction)
        {
            return RoleModule.Insert(objRoleModule,objDbTransaction);
        }

        public Utility.ResultType UpdateRole(Role objRole)
        {
            return Role.Update(objRole);
        }

        public Utility.ResultType DeleteRoleModule(int intRoleID, DbTransaction objDbTransaction)
        {
            return RoleModule.DeleteByRoleId(intRoleID, objDbTransaction);
        }

        public Utility.ResultType SelectRolePermission(out DataTable objDataTable)
        {
            return RolePermission.Select(out objDataTable);
        }

        public Utility.ResultType SelectRolePermission(out DataTable objDataTable,int intRoleID)
        {
            return RolePermission.SelectByRoleID(out objDataTable, intRoleID);
        }

        public Utility.ResultType InsertRolePermission(RolePermission objRolePermission, DbTransaction objDbTransaction)
        {
            return RolePermission.Insert(objRolePermission, objDbTransaction);
        }

        public Utility.ResultType DeleteRolePermission(int intRoleID,int intRepositoryID, DbTransaction objDbTransaction)
        {
            return RolePermission.DeleteByRoleId(intRoleID, intRepositoryID, objDbTransaction);
        }
        public Utility.ResultType UpdateRolePermission(int intRoleID,int RepositoryId, int intUserID, DbTransaction objDbTransaction)
        {
            return RolePermission.UpdateByRoleId(intRoleID, RepositoryId, intUserID, objDbTransaction);
        }
        # endregion

        public int InsertRepositoryRolepermission(string Action,int RoleID ,int ReposssitoryID, int CreatedOn)
        {
            Role obj1 = new Role();
            string Constr = Utility.ConnectionString.ToString();

            SqlConnection con = new SqlConnection(Constr);
            SqlCommand cmd = new SqlCommand("Get_Repository_for_Role", con);
            con.Open();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Action", Action);
            cmd.Parameters.AddWithValue("@RoleID",RoleID);
            cmd.Parameters.AddWithValue("@RepositoryID",ReposssitoryID);
            cmd.Parameters.AddWithValue("@UpdatedBy", CreatedOn);

            int result=0;
            result = cmd.ExecuteNonQuery();
            con.Close();
            return result;
        }

    }
}
