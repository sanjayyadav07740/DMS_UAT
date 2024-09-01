using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using DMS.BusinessLogic;


namespace DMS.BusinessLogic
{
   
    public class Mail_IDBI
    {
        SqlConnection con = new SqlConnection(Utility.ConnectionString);
      
        #region property
        public string Mail_To{get; set;}
        public string CC { get; set; }
        public string Subject { get; set; }
        public string Mail_Body { get; set; }
        public string Attachment { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CretaedBy { get; set; }
        public int IsMailSent { get; set; }
        #endregion

        #region Method
        public void insertmail()
        {
            SqlCommand cmd=new SqlCommand("INSERT_IDBI_CIRCULAR_MAIL",con);
            cmd.CommandType=CommandType.StoredProcedure;
           SqlParameter[] objinsetmaildata=new SqlParameter[8];
            objinsetmaildata[0]=new SqlParameter("@Mail_To",Mail_To);
            objinsetmaildata[1]=new SqlParameter("@CC",CC);
            objinsetmaildata[2]=new SqlParameter("@Subject",Subject);
            objinsetmaildata[3]=new SqlParameter("@Mail_Body",Mail_Body);
            objinsetmaildata[4]=new SqlParameter("@Attachment",Attachment);
            objinsetmaildata[5]=new SqlParameter("@CreatedOn",CreatedOn);
            objinsetmaildata[6]=new SqlParameter("@CretaedBy",CretaedBy);
            objinsetmaildata[7]=new SqlParameter("@IsMailSent",IsMailSent);

            foreach (SqlParameter objSqlParameter in objinsetmaildata)
            {
                cmd.Parameters.Add(objSqlParameter);
            }
            
            
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();


        }



        #endregion

    }


}
