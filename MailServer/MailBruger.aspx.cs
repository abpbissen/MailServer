using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Net;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using MailServer.App_Start;
namespace MailServer
{
    public partial class MailBruger : Page
    {
        zkkaMailDataContext m = new zkkaMailDataContext();
        Controller c = new Controller();
        string WNStr;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                GridView2.Visible = false;
                if (WebNavn() != null)
                {   //Nested
                    if (WNStr == "Admin")
                    {
                        Label1.Text = $"Hello Admin, feel free to delete text from table";
                        Button2.Visible = true;
                    }
                    Label1.Text = $"Velkommen {WNStr}";
                }
                else
                {
                    Response.Redirect("/default");
                }
                Response.Write("Test if exit: " + (string)Session["WebMail"]);
            }
        }
        protected async void Timer1_Tick(object sender, EventArgs e)
        {
            //Lambda expression i stedet for method group, da der er parametre
            await Task.Run(() => c.SqlGV(GridView1));
            //await Task.Run(() => c.ShowMsg(GridView1, (string)Session["WebMail"]));
        }

        protected async void Button1_Click(object sender, EventArgs e)
        {
            await Task.Run(() => c.EntityctTbl(TextBox2.Text, WebNavn(), (string)Session["WebMail"]));
        }
        protected void Button2_Click(object sender, EventArgs e)
        {
            c.LinqDelete();
        }
        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
    

        }
        public ref string WebNavn()
        {
            //Cast er bedre end tostring(), fordi cast kan vise variabel type
            WNStr = (string)Session["WebNavn"];
            return ref WNStr;
        }



        public override void VerifyRenderingInServerForm(Control control)
        {
            //sørger for at gridview ikke hopper ud af formen
        }



 
    }
}
