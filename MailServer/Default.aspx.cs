using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.IO;
using System.Web.UI;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.Net;
using MailServer.App_Start;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml;
using System.Threading;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Reflection;

namespace MailServer
{

    public partial class _Default : Page
    {
        //Objekter
        zkkaMailDataContext m = new zkkaMailDataContext();
        Controller c = new Controller();
        LoginResult r = new LoginResult();

        protected void Page_Load(object sender, EventArgs e)
        {

            c.LangChoice(DropDownList1.SelectedValue);
            ImageButton1.Visible = Session["WebMail"] != null ? true : false;
            lblSprog.Text = Thread.CurrentThread.CurrentUICulture.ToString();
        }
        protected async void Button1_Click(object sender, EventArgs e)
        {
            //As long as password is not empty, send mail info through the controller.
            if (TextBox2.Text != "")
            {
                await Task.Run(() => c.Mailorder(TextBox3.Text, TextBox2.Text, TextBox4.Text, "Mail besked", TextArea1.InnerText, Label1, CheckBox1.Checked));
                c.EntityMail(TextBox3.Text, TextBox4.Text, TextArea1.InnerText);
                c.TraceOut("Mail message: " + TextArea1.InnerText);
                TextArea1.InnerText = "";
            }
            else
            {
                lblMailServer.Text = "!!! Mail cannot be found or not exist!";
            }
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            if (m.Logins.Any(u => u.Name == TextBox5.Text && m.Logins.Any(msg => msg.Mail == TextBox9.Text))) // check if a record already exist in database
            {
                lblChatServerMessages.Text = ("User Name or mail Already existed! Pls, do choose another name");
            }
            else
            {
                c.EntityLogin(TextBox5.Text, TextBox6.Text, TextBox9.Text);
                lblSuccesChatServer.Text = ("Bruger Oprettet");
            }
        }
        protected void Button4_Click(object sender, EventArgs e)
        {

            r = c.LoginGruppe(TextBox7.Text, TextBox8.Text);
            if (r.Mode)
            {
                lblSuccesChatServer.Text = ("success");
                Response.Redirect("/MailBruger.aspx?WebNavn=" + Session["WebNavn"] + "&WebMail=" + Session["WebMail"]);
            }
            else
            {
                lblChatServerMessages.Text = ("error");
            }
        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            //sørger for at gridview ikke hopper ud af formen
        }

        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            c.ShowMsg(GridView1, (string)Session["WebMail"]);
        }

        protected void Button5_Click(object sender, EventArgs e)
        {
            r = c.LoginGruppe(TextBox7.Text, TextBox8.Text);
            if (r.Mode)
            {
                lblSuccesChatServer.Text = ("success");
                Response.Redirect("/default.aspx?WebNavn=" + Session["WebNavn"] + "&WebMail=" + Session["WebMail"]);

            }
            else
            {
                lblChatServerMessages.Text = ("error");
            }
        }


    }
}

