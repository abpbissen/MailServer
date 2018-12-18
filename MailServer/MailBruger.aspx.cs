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
                Button2.Visible = false;
                //Efter string metoden er igangsat en enkelt gang, kan ref string bruges i stedet
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
                lblChatBoxHilsen.Text = ("your e-mail address: " + (string)Session["WebMail"]);
               
            }
        }
        //Timer bruges til at opdatere gridview, som bruges til chat interface
        protected async void Timer1_Tick(object sender, EventArgs e)
        {
            //Lambda expression i stedet for method group, da der er parametre
            await Task.Run(() => c.SqlGV(GridView1));
        }

        protected async void Button1_Click(object sender, EventArgs e)  //chat button
        {
            await Task.Run(() => c.EntityctTbl(chatBox.Text, WebNavn(), (string)Session["WebMail"]));
            chatBox.Text = ""; // Gør tekstfelt tom 
        }
        //Delete button, kun synlig hvis der logges ind som Admin
        protected void Button2_Click(object sender, EventArgs e)
        {
            c.EntityDelete();
        }
        //ref string til navn session, da denne bruges mange gange
        public ref string WebNavn()
        {
            //Cast er bedre end tostring(), fordi cast kan vise variabel type
            WNStr = (string)Session["WebNavn"];
            return ref WNStr;
        }

        //metoden der begrænser antal character på gridview
        protected void GridViewLimit(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ViewState["Besked"] = e.Row.Cells[2].Text;
                if (e.Row.Cells[2].Text.Length >= 10)
                {
                    e.Row.Cells[2].Text = e.Row.Cells[2].Text.Substring(0, 10) + "...";
                    e.Row.Cells[2].ToolTip = ViewState["Besked"].ToString();
                }
            }
        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            //sørger for at gridview ikke hopper ud af formen
        }
    }
}
