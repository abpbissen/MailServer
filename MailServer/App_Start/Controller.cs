﻿using System;
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
using System.Text;

namespace MailServer.App_Start
{
    public class LoginResult
    {
        //Auto properties
        public bool Mode { get; set; }
        public string LoginUser { get; set; }
        public int LoginId { get; set; }
    }
    //Main
    public class Controller : Page
    {
        //Streng til mail kryptering
        public string strMail = "";
        // Matematisk udregning af kryptering
        public string strPermutation = "";
        public const int bytePermutation1 = 0x19;
        public const int bytePermutation2 = 0x59;
        public const int bytePermutation3 = 0x17;
        public const int bytePermutation4 = 0x41;

        //Instantiering af Linq to sql objekt
        zkkaMailDataContext db = new zkkaMailDataContext();
        public string errStr = "";
        //public int logonBruger;
        public async Task Mailorder(string from, string pass, string to, string subject, string mailbody, Label errLbl, bool cb)
        {
            //unik appdomain instans til hver tråd
            //AppDomain domain = AppDomain.CreateDomain(string.Concat(DStr));
            //Mail server
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com", 587);
            SmtpServer.EnableSsl = true;
            SmtpServer.UseDefaultCredentials = false;
            SmtpServer.Credentials = new NetworkCredential(from, pass);
            try
            {
                using (SmtpServer)
                {
                    await SmtpServer.SendMailAsync(from, to, subject, mailbody);
                }
                if (cb)
                {
                    //HttpRuntime klassen bruges til at finde xml fil, uden brug af systemets stifinder(C:\)
                    string MailFilePath = Path.Combine(HttpRuntime.AppDomainAppPath, "App_Data/MailFile.xml");
                    string MailFilePathCrypt = Path.Combine(HttpRuntime.AppDomainAppPath, "App_Data/MailFileCrypt.xml");

                    //Linq to xml
                    XElement newElement = new XElement("Message",
                    new XElement("Mail_Body", mailbody));
                    IEnumerable<XElement> LinqMail = from x in newElement.Descendants() select x;
                    newElement.Save(MailFilePath);

                    strMail = Encrypt(mailbody);

                    XElement newElementCrypt = new XElement("Message", new XElement("Mail_Body", strMail));
                    newElementCrypt.Save(MailFilePathCrypt);
                }
            }
            catch (SmtpException)
                {
                    errLbl.Text = "Incorrect Password!";
                }
            }
      

        //Linq metoder(7), Metoder som kan slette/tilføje er navngivet med "Entity"
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
                //prøver igen
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
        public Login EntityLogin(string name, string password, string mail)
        {  
            Login insertLogin = new Login { Name = name, Password = Encrypt(password), Mail = mail };
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
        public void EntityDelete()
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
        //Constructor af LoginResult klassen
        public LoginResult LoginGruppe(string Name, string password)
        {
            LoginResult r = new LoginResult();
  
            var logonBruger =
            (from x in db.Logins where x.Name == Name && x.Password == Encrypt(password) select x).FirstOrDefault();
            if (logonBruger != null && logonBruger.Name != "")
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
        public void SqlGV(GridView gv)
        {
            gv.DataSource = (from x in db.ctTbls orderby x.ChatNr descending select new { x.ChatNr, x.Navn, x.Besked }).Take(10);
           
            gv.DataBind();
        }
        public void ShowMsg(GridView gv)
        {
            gv.DataSource = (from x in db.MailBeskeds orderby x.Id descending select new { x.Id, x.Fra, x.Besked }).Take(10);
            gv.DataBind();
        }

        //string metode til sprogvalg
        public string LangChoice(string sprog)
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(WebConfigurationManager.AppSettings[sprog]);
            return sprog;
        }
        //Brug af Trace klassen til at se historik af mailbeskeder
        public string TraceOut(string outStr)
        {
            System.Diagnostics.Trace.Listeners.Add(new TextWriterTraceListener("MailOutput.log", "myListener"));
            System.Diagnostics.Trace.TraceInformation(outStr);
            System.Diagnostics.Trace.Flush();
            return outStr;
        }
        //Krypteringsmetoder(4): Kryptering, dekryptering, Kryptering af bytes og dekryptering af bytes
        public string Encrypt(string strData)
        {
            return Convert.ToBase64String(ByteEncrypt(Encoding.UTF8.GetBytes(strData)));
        }
        public string Decrypt(string strData)
        {
            return Encoding.UTF8.GetString(ByteDecrypt(Convert.FromBase64String(strData)));
        }

        public byte[] ByteEncrypt(byte[] strData)
        {
            PasswordDeriveBytes passbytes =
            new PasswordDeriveBytes(strPermutation,
            new byte[] { bytePermutation1,
                         bytePermutation2,
                         bytePermutation3,
                         bytePermutation4
            });

            MemoryStream memstream = new MemoryStream();
            Aes aes = new AesManaged();
            aes.Key = passbytes.GetBytes(aes.KeySize / 8);
            aes.IV = passbytes.GetBytes(aes.BlockSize / 8);

            CryptoStream cryptostream = new CryptoStream(memstream,
            aes.CreateEncryptor(), CryptoStreamMode.Write);
            cryptostream.Write(strData, 0, strData.Length);
            cryptostream.Close();
            return memstream.ToArray();
        }
        public byte[] ByteDecrypt(byte[] strData)
        {
            PasswordDeriveBytes passbytes =
            new PasswordDeriveBytes(strPermutation,
            new byte[] { bytePermutation1,
                         bytePermutation2,
                         bytePermutation3,
                         bytePermutation4
            });

            MemoryStream memstream = new MemoryStream();
            Aes aes = new AesManaged();
            aes.Key = passbytes.GetBytes(aes.KeySize / 8);
            aes.IV = passbytes.GetBytes(aes.BlockSize / 8);

            CryptoStream cryptostream = new CryptoStream(memstream,
            aes.CreateDecryptor(), CryptoStreamMode.Write);
            cryptostream.Write(strData, 0, strData.Length);
            cryptostream.Close();
            return memstream.ToArray();
        }
    }
}