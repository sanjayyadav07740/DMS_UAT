using DMS.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DMS.Shared
{
    public partial class Rename_doc : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(Utility.ConnectionString);

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
            scriptManager.RegisterPostBackControl(this.ibtnSubmit);

            if (!IsPostBack)
            {

            }
        }

        protected void ibtnSubmit_Click(object sender, ImageClickEventArgs e)
        {
            DataTable dt1 = new DataTable("notfound");
            dt1.Columns.Add(new DataColumn("DocName_count1", typeof(string)));
            dt1.Columns.Add(new DataColumn("DocName_count2", typeof(string)));
            dt1.Columns.Add(new DataColumn("DocName_error", typeof(string)));
            //dt1.Columns.Add(new DataColumn("DocName_renamed", typeof(string)));
           // dt1.Columns.Add(new DataColumn("DocName_Notrenamed", typeof(string)));
            dt1.Columns.Add(new DataColumn("DocName_notfound", typeof(string)));
            DataSet objTableExcel = new DataSet();
            if (!Path.GetExtension(FileUpload1.FileName).ToLower().Contains(".xls"))
            {
                UserSession.DisplayMessage(this, "Please upload Excel file only", MainMasterPage.MessageType.Warning);
                return;
            }

            string excelFilePath = Server.MapPath("~/Others/" + Guid.NewGuid() + Path.GetExtension(FileUpload1.FileName));
            FileUpload1.SaveAs(excelFilePath);
            string connectionString = string.Empty;
            if (Path.GetExtension(FileUpload1.FileName).ToLower() == ".xls")
                connectionString = string.Format(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;HDR=Yes;IMEX=1'", excelFilePath);
            else
                connectionString = string.Format(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 12.0 Xml;HDR=YES;IMEX=1';", excelFilePath);
            OleDbConnection objOleDbConnection = new OleDbConnection(connectionString);
            objOleDbConnection.Open();
            DataTable objDataTableSheet = objOleDbConnection.GetSchema("Tables");
            string query = string.Format("SELECT * FROM [{0}]", objDataTableSheet.Rows[0]["Table_Name"].ToString());
            OleDbDataAdapter objOleDbDataAdapter = new OleDbDataAdapter(query, objOleDbConnection);

            objOleDbDataAdapter.Fill(objTableExcel);
             
             foreach (DataRow row in objTableExcel.Tables[0].Rows)
             {
                 try
                 {
                     string strquery = " select d.id from Document d inner join metadata m on d.MetaDataID=m.id where d.documentname like '%" + row["Correct New File name"] + "%' and convert(date,d.createdon)='2018-08-04' and m.categoryid=293";
                     DataSet dsdownload = new DataSet();
                     dsdownload = DataHelper.ExecuteDataSet(strquery);
                     if (dsdownload.Tables[0].Rows.Count==1)
                     {
                     //foreach (DataRow rowds in dsdownload.Tables[0].Rows)
                     //{

                         DataRow dr = dt1.NewRow();
                         // dr["DocName"] = f.Substring(f.LastIndexOf("\\") + 1, 8) + ".pdf";
                         dr["DocName_count1"] = row["Correct New File name"];
                         dt1.Rows.Add(dr);
                         //string strquery1 = "update document set tag='" + row["tag"] + "' where id='" + rowds["id"] + "'";
                         //SqlCommand cmdresult1 = new SqlCommand(strquery1, con);
                         //con.Open();
                         //cmdresult1.ExecuteNonQuery();
                         //con.Close();



                    // }
                     }
                     else if (dsdownload.Tables[0].Rows.Count > 1)
                     {
                         DataRow dr = dt1.NewRow();
                         // dr["DocName"] = f.Substring(f.LastIndexOf("\\") + 1, 8) + ".pdf";
                         dr["DocName_count2"] = row["Correct New File name"];
                         dt1.Rows.Add(dr);
                     }

                     else
                     {

                         DataRow dr = dt1.NewRow();
                         // dr["DocName"] = f.Substring(f.LastIndexOf("\\") + 1, 8) + ".pdf";
                         dr["DocName_notfound"] = row["Correct New File name"];
                         dt1.Rows.Add(dr);
                     }
                 }
                 catch (Exception ex)
                 {

                     DataRow dr = dt1.NewRow();
                     // dr["DocName"] = f.Substring(f.LastIndexOf("\\") + 1, 8) + ".pdf";
                     dr["DocName_error"] = row["Correct New File name"];
                     dt1.Rows.Add(dr);
                 }  
             }
             if (dt1.Rows.Count > 0)
             {
                 Utility.ExportToExcel(dt1, string.Format("DocumentNotMergedReport{0:dd MMMM yyyy HH mm tt}", DateTime.Now));
             }
        }
    }
}