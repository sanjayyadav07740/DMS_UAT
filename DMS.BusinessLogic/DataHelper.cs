using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using DMS.BusinessLogic;

namespace DMS.BusinessLogic
{
    public class DataHelper
    {
        /// <summary>
        /// This method is created for the insert update table using  Stored procedure.
        /// </summary>
        /// <param name="strProcedureName"></param>
        /// <param name="objDbTransaction"></param>
        /// <param name="objDbParameterCollection"></param>
        public static int ExecuteNonQuery(string strProcedureName, DbTransaction objDbTransaction, DbParameter[] objDbParameter)
        {
            DbProviderFactory objDbProviderFactory = DbProviderFactories.GetFactory(Utility.ProviderName);

            DbCommand objDbCommand = objDbProviderFactory.CreateCommand();
            objDbCommand.CommandType = CommandType.StoredProcedure;
            objDbCommand.CommandText = strProcedureName;
            objDbCommand.Connection = objDbTransaction.Connection;
            objDbCommand.Transaction = objDbTransaction;

            foreach (DbParameter objDbParameterNew in objDbParameter)
            {
                if (objDbParameterNew != null)
                objDbCommand.Parameters.Add(objDbParameterNew);
            }


            return objDbCommand.ExecuteNonQuery();           
        }

        /// <summary>
        /// This method  is created for executing reader using query(Select query/(View)).
        /// </summary>
        /// <param name="strQuery"></param>
        /// <returns></returns>
        public static DbDataReader ExecuteReader(string strQuery, DbParameter[] objDbParameter)
        {
          
            DbConnection objDbConnection = Utility.GetConnection;
            try
            {
                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;
                
                objDbConnection.Open();

                DbCommand objDbCommand = objDbProviderFactory.CreateCommand();
                objDbCommand.CommandType = CommandType.Text;
                objDbCommand.Connection = objDbConnection;
                objDbCommand.CommandText = strQuery;

                if (objDbParameter != null)
                {
                    foreach (DbParameter objDbParameterNew in objDbParameter)
                    {
                        if (objDbParameterNew != null)
                        objDbCommand.Parameters.Add(objDbParameterNew);
                    }
                }
                return objDbCommand.ExecuteReader(CommandBehavior.CloseConnection);
            }
            finally
            {
                objDbConnection.Close();
            }
        }

        /// <summary>
        /// This method is created for select the single value 
        /// </summary>
        /// <param name="strQuery"></param>
        /// <returns></returns>
        public static object ExecuteScalar(string strQuery, DbParameter[] objDbParameter)
        {
            DbConnection objDbConnection = Utility.GetConnection;
            try
            {
                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

                objDbConnection.Open();

                DbCommand objDbCommand = objDbProviderFactory.CreateCommand();
                objDbCommand.CommandType = CommandType.Text;
                objDbCommand.CommandText = BuildQuery(strQuery);
                objDbCommand.Connection = objDbConnection;

                if (objDbParameter != null)
                {
                    foreach (DbParameter objDbParameterNew in objDbParameter)
                    {
                        if (objDbParameterNew != null)
                        objDbCommand.Parameters.Add(objDbParameterNew);
                    }
                }

                return objDbCommand.ExecuteScalar();
            }

            finally
            {
                objDbConnection.Close();
            }
        }

        /// <summary>
        /// This method returns datatable.
        /// </summary>
        /// <param name="strQuery"></param>
        /// <returns></returns>
        public static DataTable ExecuteDataTable(string strQuery, DbParameter[] objDbParameter)
        {
            DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;
            DbConnection objDbConnection = Utility.GetConnection;
            objDbConnection.Open();

            DbCommand objDbCommand = objDbProviderFactory.CreateCommand();
            objDbCommand.CommandType = CommandType.Text;
            objDbCommand.CommandText = BuildQuery(strQuery);
            objDbCommand.Connection = objDbConnection;
            objDbCommand.CommandTimeout = 0;
            if (objDbParameter != null)
            {
                foreach (DbParameter objDbParameterNew in objDbParameter)
                {
                    if(objDbParameterNew!=null)
                    objDbCommand.Parameters.Add(objDbParameterNew);
                }
            }

            DbDataAdapter objDbDataAdapter = objDbProviderFactory.CreateDataAdapter();
            objDbDataAdapter.SelectCommand = objDbCommand;
            DataTable objDataTable = new DataTable();
            objDbDataAdapter.Fill(objDataTable);
            objDbConnection.Close();
            objDbCommand.Parameters.Clear();
            return objDataTable;
        }

        /// <summary>
        /// This method returns dataset.
        /// </summary>
        /// <param name="strQuery"></param>
        /// <returns></returns>
        //public static DataSet ExecuteDataSet(string strQuery)
        //{
        //    DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;
        //    DbConnection objDbConnection = Utility.GetConnection;
        //    objDbConnection.Open();

        //    DbCommand objDbCommand = objDbProviderFactory.CreateCommand();
        //    objDbCommand.CommandType = CommandType.Text;
        //    objDbCommand.CommandText = BuildQuery(strQuery);
        //    objDbCommand.Connection = objDbConnection;

        //    DbDataAdapter objDbDataAdapter = objDbProviderFactory.CreateDataAdapter();
        //    objDbDataAdapter.SelectCommand = objDbCommand;

        //    DataSet objDataSet = new DataSet();
        //    objDbDataAdapter.Fill(objDataSet);
        //    objDbConnection.Close();
        //    return objDataSet;
        //}

        public static DataSet ExecuteDataSet(string strQuery)
        {
            DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;
            DbConnection objDbConnection = Utility.GetConnection;
            objDbConnection.Open();
            DbCommand objDbCommand = objDbProviderFactory.CreateCommand();
            objDbCommand.CommandType = CommandType.Text;
            objDbCommand.CommandText = BuildQuery(strQuery);
            objDbCommand.Connection = objDbConnection;

            DbDataAdapter objDbDataAdapter = objDbProviderFactory.CreateDataAdapter();
            objDbDataAdapter.SelectCommand = objDbCommand;

            DataSet objDataSet = new DataSet();
          
            objDbDataAdapter.Fill(objDataSet);
            objDbConnection.Close();
            return objDataSet;
        }


        public static DataTable ExecuteDataTableForProcedure(string strProcedureName, DbTransaction objDbTransaction, DbParameter[] objDbParameter)
        {
            DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;
            DbConnection objDbConnection = Utility.GetConnection;
            objDbConnection.Open();

            DbCommand objDbCommand = objDbProviderFactory.CreateCommand();
            objDbCommand.CommandType = CommandType.StoredProcedure;
            objDbCommand.CommandText = strProcedureName;
            objDbCommand.Connection = objDbConnection;
            objDbCommand.Transaction = objDbTransaction;

            foreach (DbParameter objDbParameterNew in objDbParameter)
            {
                if (objDbParameterNew != null)
                    objDbCommand.Parameters.Add(objDbParameterNew);
            }

            DbDataAdapter objDbDataAdapter = objDbProviderFactory.CreateDataAdapter();
            objDbDataAdapter.SelectCommand = objDbCommand;

            DataTable objDataTable = new DataTable();
            objDbDataAdapter.Fill(objDataTable);
            objDbConnection.Close();
            return objDataTable;
        }

        private static string BuildQuery(string strQuery)
        {
            if (Utility.GetProviderFactory.GetType().ToString() == "System.Data.SqlClient.SqlClientFactory")
            {
                strQuery = strQuery.Replace("@", "@");
            }
            else if (Utility.GetProviderFactory.GetType().ToString() == "System.Data.OracleClient.OracleClientFactory")
            {
                strQuery = strQuery.Replace("@", ":").Replace("dbo.", "").Replace("DBO.", ""); 
            }
            else
            {
                strQuery = strQuery.Replace("@", "?");
            }
            return strQuery;
        }

        public static DataTable ExecuteDataTable(string strQuery)
        {
            DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;
            DbConnection objDbConnection = Utility.GetConnection;
            objDbConnection.Open();
            DbCommand objDbCommand = objDbProviderFactory.CreateCommand();
            objDbCommand.CommandType = CommandType.Text;
            objDbCommand.CommandText = BuildQuery(strQuery);
            objDbCommand.Connection = objDbConnection;

            DbDataAdapter objDbDataAdapter = objDbProviderFactory.CreateDataAdapter();
            objDbDataAdapter.SelectCommand = objDbCommand;

            DataTable objDatatable = new DataTable();
            objDbDataAdapter.Fill(objDatatable);
            objDbConnection.Close();
            return objDatatable;
        }


        // Added by Sanjay
        public static DataTable ExecuteDataTableNew(string strQuery, DbParameter[] objDbParameter)
        {
            DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;
            DbConnection objDbConnection = Utility.GetConnection;

            if (objDbConnection.State == ConnectionState.Open)
            {
                objDbConnection.Close();
            }
            objDbConnection.Open();

            DbCommand objDbCommand = objDbProviderFactory.CreateCommand();
            objDbCommand.CommandType = CommandType.StoredProcedure;
            objDbCommand.CommandText = strQuery;

            // Increase time for Execution 
            objDbCommand.CommandTimeout = 1000;

            objDbCommand.Connection = objDbConnection;

            if (objDbParameter != null)
            {
                foreach (DbParameter objDbParameterNew in objDbParameter)
                {
                    if (objDbParameterNew != null)
                        objDbCommand.Parameters.Add(objDbParameterNew);
                }
            }


            DbDataAdapter objDbDataAdapter = objDbProviderFactory.CreateDataAdapter();
            objDbDataAdapter.SelectCommand = objDbCommand;
            DataTable objDataTable = new DataTable();
            objDbDataAdapter.Fill(objDataTable);
            objDbConnection.Close();
            return objDataTable;
        }

    }
}
