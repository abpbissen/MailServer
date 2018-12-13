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
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Security.Cryptography;
using System.Configuration;
using System.Globalization;
using System.Web.Configuration;
using System.Diagnostics;

namespace MailServer.App_Start
{
    public class LoginResult
    {
        public bool Mode { get; set; }
        public string LoginUser { get; set; }
        public int LoginId { get; set; }
    }

    public class Controller : Page
    {
        zkkaMailDataContext db = new zkkaMailDataContext();
        public async Task Mailorder(string from, string pass, string to, string subject, string mailbody, string adGuid, bool cb)
        {
            //unik appdomain instans til hver tråd
            AppDomain domain = AppDomain.CreateDomain(adGuid);
            //Mail server
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com", 587);
            SmtpServer.EnableSsl = true;
            SmtpServer.UseDefaultCredentials = false;
            SmtpServer.Credentials = new NetworkCredential(from, pass);
            using (SmtpServer)
            {
                await SmtpServer.SendMailAsync(from, to, subject, mailbody);
            }
            //Mail info til xml og krypteret
            if (cb)
            {
                var filename = "MailFile.xml";
                var filenameCrypt = "MailFileCrypt.xml";
                var MailFilePath = Path.Combine(@"C:\Users\Administrator\Desktop\Projects\MailServer\MailServer\App_Data", filename);
                var MailFilePathCrypt = Path.Combine(@"C:\Users\Administrator\Desktop\Projects\MailServer\MailServer\App_Data", filenameCrypt);
                XElement newElement = new XElement("Message",
                new XElement("Mail_Body", mailbody));
                IEnumerable<XElement> LinqMail = from x in newElement.Descendants() select x;
                newElement.Save(MailFilePath);
                Rijndael rCrypt = Rijndael.Create();
                using (Rijndael myRijndael = Rijndael.Create())
                {
                    
                    //Krypterer strengen til array af bytes
                    byte[] encrypted = EncryptStringToBytes(mailbody, myRijndael.Key, myRijndael.IV);
                    XElement newElementCrypt = new XElement("Message",
                    new XElement("Mail_Body", BitConverter.ToString(encrypted).Replace("-", "")));
                    IEnumerable<XElement> MailCryptIEnum = from x in newElementCrypt.Descendants().ToList() select x;
                    newElementCrypt.Save(MailFilePathCrypt);
                }
            }
        }
        static byte[] EncryptStringToBytes(string plainText, byte[] Key, byte[] IV)
        {
            byte[] encrypted;
            //Operet krypterings objekt med key og IV
            using (Rijndael rijAlg = Rijndael.Create())
            {
                rijAlg.Key = Key;
                rijAlg.IV = IV;
                ICryptoTransform encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);
                //Opretter stream brugt til kryptering
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Udskriver al data til stream
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
            return encrypted;
        }
        public LoginResult LoginGruppe(string Name, string Password)
        {
            LoginResult r = new LoginResult();
            var logonBruger =
            (from x in db.Logins where x.Name == Name && x.Password == Password select x).FirstOrDefault();
            if (logonBruger.Id != 0)
            {
                Session["WebNavn"] = logonBruger.Name;
                Session["WebMail"] = logonBruger.Mail;
                r.Mode = true;
                r.LoginUser = logonBruger.Password;
                r.LoginId = logonBruger.Id;
            }
            else
            {
                Session["WebNavn"] = "";
                Session["WebMail"] = "";
                r.Mode = false;
                r.LoginUser = "";
                r.LoginId = 0;
            }
            return r;
        }
        public ctTbl EntityctTbl(string besked, string navn, string email)
        {
            //Table<TEntity> oprettes
            ctTbl insertTbl = new ctTbl { Besked = besked, Navn = navn, Email = email };
            //Tilføj den nye TEntity til ctTbls kollektionen
            db.ctTbls.InsertOnSubmit(insertTbl);
            //Prøver at tilføje ændringer
            try
            {
                db.SubmitChanges();
            }
            catch (Exception)
            {
                //prøv igen
                db.SubmitChanges();
            }
            return insertTbl;
        }
        public MailBesked EntityMail(string fra, string brugermail, string besked )
        {
            MailBesked insertMail = new MailBesked { Fra = fra, Besked = besked, BrugerMail = brugermail };
            //Tilføj den nye TEntity til ctTbls kollektionen
            db.MailBeskeds.InsertOnSubmit(insertMail);
            //Prøver at tilføje ændringer
            try
            {
                db.SubmitChanges();
            }
            catch (Exception)
            {
                //prøv igen
                db.SubmitChanges();
            }
            return insertMail;
        }
        public Login EntityLogin(string navn, string kode, string mail)
        {
            Login insertLogin = new Login { Name = navn, Password = kode, Mail = mail };
            db.Logins.InsertOnSubmit(insertLogin);
            try
            {
                db.SubmitChanges();
            }
            catch (Exception)
            {
                db.SubmitChanges();
            }
            return insertLogin;
        }
        public bool SqlGV(GridView gv)
        {
            gv.DataSource = (from chat in db.ctTbls orderby chat.ChatNr descending select chat).Take(10);
            gv.DataBind();
            return true;
        }
        public bool ShowMsg(GridView gv, string mail)
        {
            gv.DataSource = (from x in db.MailBeskeds where x.BrugerMail == mail select x);
            gv.DataBind();
            return true;
        }
        public void LinqDelete()
        {
            var delTable =
            from x in db.ctTbls
            where x.ChatNr >= 1
            select x;

            foreach (var d in delTable)
            {
                db.ctTbls.DeleteOnSubmit(d);
            }

            try
            {
                db.SubmitChanges();
            }
            catch (Exception e)
            {
                Response.Write(e);
            }
        }

        public string LangChoice(string sprog)
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(WebConfigurationManager.AppSettings[sprog]);
            return sprog;
        }
        public string TraceOut(string outStr)
        {
            System.Diagnostics.Trace.Listeners.Add(new TextWriterTraceListener("MailLog.log", "myListener"));
            System.Diagnostics.Trace.TraceInformation(outStr);
            System.Diagnostics.Trace.Flush();
            return outStr;
        }
    }
}