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
        //Instansierede objekter
        zkkaMailDataContext m = new zkkaMailDataContext();
        Controller c = new Controller();
        LoginResult r = new LoginResult();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                c.LangChoice(DropDownList1.SelectedValue);
                //betingelses operator(conditional operator), alternativt til if statements 
                //imagebutton er synlig, hvis mail sessionen ikke er null(betingelse ? konsekvens : alternativ)
                ImageButton1.Visible = Session["WebMail"] != null ? true : false;
                lblSprog.Text = Thread.CurrentThread.CurrentUICulture.ToString();
            }
        }
        protected async void Button1_Click(object sender, EventArgs e)
        {
            //Hvis textbox2 ikke er tom, kan afsendelse af mail forsøges
            if (TextBox2.Text != "")
            {
                //Multi tråd(Tasks er threads i async form)
                await c.Mailorder(TextBox3.Text, TextBox2.Text, TextBox4.Text, "Mail besked", TextArea1.InnerText, Label1, CheckBox1.Checked);
                await Task.Run(() => c.EntityMail(TextBox3.Text, TextBox4.Text, TextArea1.InnerText));
                await Task.Run(() => c.TraceOut("Mail message: " + TextArea1.InnerText));
                TextArea1.InnerText = "";
            }
            else
            {
                lblMailServer.Text = "!!! Mail cannot be found or not exist!";
            }
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            //Undersøger om navnet i databasen eksisterer
            if (m.Logins.Any(u => u.Name == TextBox5.Text && m.Logins.Any(msg => msg.Mail == TextBox9.Text))) 
            {
                lblChatServerMessages.Text = "User Name or mail Already existed! Pls, do choose another name";
            }
            else
            {
                c.EntityLogin(TextBox5.Text, TextBox6.Text, TextBox9.Text);
                lblSuccesChatServer.Text = "User created";
            }
        }
        //Chat logon
        protected void Button4_Click(object sender, EventArgs e)
        {
            r = c.LoginGruppe(TextBox7.Text, TextBox8.Text);
            if (r.Mode)
            {
                //Omdirigerer videre til mailbruger siden, med overførsel af navn og mail sessionvariabler
                Response.Redirect("/MailBruger.aspx?WebNavn=" + Session["WebNavn"] + "&WebMail=" + Session["WebMail"]);
            }
            else
            {
                lblChatServerMessages.Text = "error";
            }
        }
        //Mail logon
        protected void Button5_Click(object sender, EventArgs e)
        {
            r = c.LoginGruppe(TextBox7.Text, TextBox8.Text);
            if (r.Mode)
            {
                //Giver default siden navn og mail sessionvariabler
                Response.Redirect("/default.aspx?WebNavn=" + Session["WebNavn"] + "&WebMail=" + Session["WebMail"]);

            }
            else
            {
                lblChatServerMessages.Text = "error";
            }
        }
        //Vis mail icon, til at se mail inbox
        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            c.ShowMsg(GridView1, (string)Session["WebMail"]);
        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            //sørger for at gridview ikke hopper ud af formen
        }


    }
}

