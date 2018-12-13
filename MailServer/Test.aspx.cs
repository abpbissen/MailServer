using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Drawing;
using System.Configuration;

namespace MailServer
{
    public partial class Test : System.Web.UI.Page
    {

        string Con = @"Server= localhost; Data Source = DESKTOP-89UA16K; Initial Catalog = TestKelly ; Integrated Security = SSPI;"; //Connection
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack) //
            {
                BrugerInfo.DataSource = MailDataAccess.GetAllMails("ID"); //To sort data after ID
                BrugerInfo.DataBind(); //Binds the data gotten from the class MailDataAcces to the table
            }


            using (SqlConnection sqlcon = new SqlConnection(Con)) {
                sqlcon.Open();
                SqlDataAdapter sqlData = new SqlDataAdapter("SELECT * from workNow", sqlcon); //Get data from 
                DataSet ds = new DataSet();
                sqlData.Fill(ds); //Fills the adapter with data from db
                BrugerInfo.DataSource = ds; //Assign
                BrugerInfo.DataBind(); //Shows it
                sqlcon.Close();
            }

        }
           //Sort ASC or DESC
        protected void gv_Sorting(object sender, GridViewSortEventArgs e) {
            Response.Write("Sort Expression" + e.SortExpression);
            Response.Write("<br/>");
            Response.Write("Sort Direction" + e.SortExpression.ToString());

            string strSortDirection = e.SortDirection == SortDirection.Ascending ? "ASC" : "DESC";
            


        }
    }
}