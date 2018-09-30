using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SterlingForexService.com.sbp.utility;
 using System.IO;
using System.Net.Mail;
using System.Net.Mime;
using System.Net;
using System.Data;
using SterlingForexService;

namespace SterlingForexService.com.sbp.utility
{
    class Mailer
    {

        public string mailFrom = "e-statement@sterlingbankng.com";// Gizmo.AppSetting("mailsender");
        public string mailHost = "10.0.20.77"; //Gizmo.AppSetting("mailserver");
        public string mailSenderName = "STERLING"; //Gizmo.AppSetting("MAILNAME");
        public string mailBody = "";
        public string mailSubject = "";
        public MailMessage message = new MailMessage();

        public string errmsg;

        public Mailer()
        {
            MailAddress address = new MailAddress(mailFrom, mailSenderName);
            message.From = address;
        }

        public Mailer(string email)
        {
            MailAddress address = new MailAddress(email);
            message.From = address;
        }


        public String sendTheEmail(string destinationemail, string sourceemail,string messagebody,string mailsubject)
        {

            String response = "";

            Gadget g = new Gadget();

            if (g.checkEmail(destinationemail))
            {

                try
                {
                    
                    string body = messagebody;


                   // ISMJob.ServiceReference1.ServiceSoapClient ws = new ISMJob.ServiceReference1.ServiceSoapClient();
                    response = "";// ws.SendMail("Tunde.Ifafore@sterlingbankng.com", "Tunde.Ifafore@sterlingbankng.com", body, "Test Email Subject");
                }
                catch (Exception ex)
                {
                    errmsg = "ERR: " + ex.Message;
                    return "false";
                }
                return response;

            }
            else
            {
                return response = "-1";
            }
        }



        public int addTo(string email)
        {
            //test if only one email
            int cnt = 0;

            string s = email;
            s = s.Replace(" ", "");
            s = s.Replace(",", " ");
            s = s.Replace(";", " ");
            string[] words = s.Split(' ');
            Gadget g = new Gadget();
            foreach (string word in words)
            {
                if (g.checkEmail(word))
                {
                    try
                    {
                        MailAddress address = new MailAddress(word);
                        message.To.Add(address);
                        cnt++;
                    }
                    catch
                    {
                    }
                }
            }
            return cnt;
        }

        public int addCC(string email)
        {
            int cnt = 0;
            string s = email;
            s = s.Replace(" ", "");
            s = s.Replace(",", " ");
            s = s.Replace(";", " ");
            string[] words = s.Split(' ');
            Gadget g = new Gadget();
            foreach (string word in words)
            {
                if (g.checkEmail(word))
                {
                    try
                    {
                        MailAddress address = new MailAddress(word);
                        message.CC.Add(address);
                        cnt++;
                    }
                    catch
                    {
                    }
                }
            }
            return cnt;
        }

        public int addBCC(string email)
        {
            int cnt = 0;
            string s = email;
            s = s.Replace(" ", "");
            s = s.Replace(",", " ");
            s = s.Replace(";", " ");
            string[] words = s.Split(' ');
            Gadget g = new Gadget();
            foreach (string word in words)
            {
                if (g.checkEmail(word))
                {
                    try
                    {
                        MailAddress address = new MailAddress(word);
                        message.Bcc.Add(address);
                        cnt++;
                    }
                    catch
                    {
                    }
                }
            }
            return cnt;
        }

        public void AddAttachment(string attachmentFilename)
        {
            if (attachmentFilename != null)
            {
                Attachment attachment = new Attachment(attachmentFilename, MediaTypeNames.Application.Octet);
                ContentDisposition disposition = attachment.ContentDisposition;
                disposition.CreationDate = File.GetCreationTime(attachmentFilename);
                disposition.ModificationDate = File.GetLastWriteTime(attachmentFilename);
                disposition.ReadDate = File.GetLastAccessTime(attachmentFilename);
                disposition.FileName = Path.GetFileName(attachmentFilename);
                disposition.Size = new FileInfo(attachmentFilename).Length;
                disposition.DispositionType = DispositionTypeNames.Attachment;
                message.Attachments.Add(attachment);
            }
        }

        private string GetMime(string fle)
        {
            string ext = Path.GetExtension(fle).ToLower();
            string resp = "image/jpeg";
            switch (ext)
            {
                case ".jpg":
                case ".jpeg":
                    resp = "image/jpeg";
                    break;
                case ".png":
                    resp = "image/png";
                    break;
                case ".pdf":
                    resp = "application/pdf";
                    break;

            }
            //Response.Write(resp);
            return resp;
        }

        public bool sendTheMail()
        {



            SmtpClient smtpClient = new SmtpClient();
            string mailUser = @"sterlingbank\ifaforeet"; // ConfigurationManager.AppSettings["mailuser"];
            string mailPwd ="Scholar001#";// ConfigurationManager.AppSettings["mailpass"];  
            NetworkCredential nc = new NetworkCredential(mailUser, mailPwd);
            try
            {
                message.Subject = mailSubject;
                message.Body = mailBody;
                message.IsBodyHtml = true;
                smtpClient.UseDefaultCredentials = true;
                smtpClient.Credentials = nc;
                errmsg = "OK: MAIL SUCCESSFUL";
                smtpClient.Host = mailHost;
                smtpClient.Send(message);
                return true;
            }
            catch (Exception ex)
            {
                errmsg = "ERR: " + ex.Message;
                return false;
            }

        }

        public bool sendTheMailTest()
        {
            errmsg = "OK: MAIL NOT SENT";
            return true;
        }

        public int ReplyTo(string email)
        {
            int cnt = 0;
            string s = email;
            s = s.Replace(" ", "");
            s = s.Replace(",", " ");
            s = s.Replace(";", " ");
            string[] words = s.Split(' ');
            Gadget g = new Gadget();
            foreach (string word in words)
            {
                if (g.checkEmail(word))
                {
                    try
                    {
                        message.ReplyToList.Add(word);
                        cnt++;
                    }
                    catch
                    {
                    }
                }
            }
            return cnt;
        }
   
    }
}
