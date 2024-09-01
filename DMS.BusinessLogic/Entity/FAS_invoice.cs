using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;


namespace DMS.BusinessLogic
{
    public class FAS_invoice
    {
        SqlConnection con = new SqlConnection(Utility.ConnectionString);

        public DataTable otherParam()
        {
            DataTable dt = new DataTable();
            string query = "select * from FAS_Other";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataAdapter adpt = new SqlDataAdapter(cmd);
            adpt.Fill(dt);
            return dt;
        }


        public int CustomerID1(int custID)
        {
            int customerID = 0;
            string query = @"select ClientFasID from FAS_ClientTable where RepositoryID=" + custID;
            con.Open();
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                customerID = rdr.GetInt32(0);
            }
            con.Close();
            return customerID;
        }


        public int LocationID1(int custID)
        {
            int locationID = 0;
            string query = @"select FasId from FAS_Loation where RepositoryID=" + custID;
            con.Open();
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                locationID = rdr.GetInt32(0);
            }
            con.Close();
            return locationID;
        }

        //public int branchID1(int branID)
        //{
        //    int BranchId = 0;
        //    string query = @"SELECT FasID FROM FAS_Branches WHERE ID=" + branID;
        //    con.Open();
        //    SqlCommand cmd = new SqlCommand(query,con);
        //    SqlDataReader rdr = cmd.ExecuteReader();
        //    while (rdr.Read())
        //    {
        //        BranchId = rdr.GetInt32(0);

        //    }
        //    con.Close();
        //    return BranchId;


        //}



    }
}
