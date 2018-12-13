using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;

namespace MailServer
{
    public class Mail
    { //encapsulate info from trable
        public int mailID { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }
        public string Date { get; set; }
    }

    public class MailDataAccess
    {
        public static List<Mail> GetAllMails(string sortColumn)
        {
            List<Mail> listOverMails = new List<Mail>(); //variable

            String cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString; //read constring from web.config file
            using (SqlConnection con = new SqlConnection(cs))
            { //Build the con object
                string sqlQuery = "SELECT * from worknow"; //What to want from table
                if (!string.IsNullOrEmpty(sortColumn))
                { //If the ID arent null or tempty do this
                    sqlQuery += " order by " + sortColumn; //append
                }
                SqlCommand cmd = new SqlCommand(sqlQuery, con); //command object

                con.Open(); //Opens connection

                SqlDataReader sdr = cmd.ExecuteReader();
                while (sdr.Read()) //Find columns
                {
                    Mail m = new Mail();
                    m.mailID = Convert.ToInt32(sdr["ID"]);
                    m.Name = sdr["navn"].ToString();
                    m.Message = sdr["mail"].ToString();
                    m.Date = sdr["dato"].ToString();

                    listOverMails.Add(m);
                }
            }
            return listOverMails;
        }
    }
}