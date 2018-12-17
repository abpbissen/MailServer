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
                lblChatBoxHilsen.Text = ("your e-mail address: " + (string)Session["WebMail"]);
                //GridView1.Columns[2].ItemStyle.Width = Unit.Pixel(100);
                //GridView1.RowStyle.Width = Unit.Pixel(10);
               
            }
        }

   
        protected async void Timer1_Tick(object sender, EventArgs e)
        {
            //Lambda expression i stedet for method group, da der er parametre
            await Task.Run(() => c.SqlGV(GridView1));
            //await Task.Run(() => c.ShowMsg(GridView1, (string)Session["WebMail"]));
        }

        protected async void Button1_Click(object sender, EventArgs e)  //chat button
        {
            await Task.Run(() => c.EntityctTbl(chatBox.Text, WebNavn(), (string)Session["WebMail"]));
            chatBox.Text = ""; // clear text after button click or enter
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
    }
}
