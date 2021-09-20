using NPOI.SS.Formula.Functions;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace DataDrivenFramework.Testcases
{
    [SetUpFixture]
    public class EmailClass
    {
        [OneTimeTearDown]
        public void sendMail()
        {


            // string reportFolder = @"\\mercury\development\Testresults\Reports\";

            string reportFolder = @"\\mimas.fft.local\tfs-build-reports\Testresults\Reports\";
           // // string reportFolder = ConfigurationManager.AppSettings["reportFolder"];
            DirectoryInfo DirInfo = new DirectoryInfo(reportFolder);

            FileInfo[] files = DirInfo.GetFiles();
            DateTime lastWrite = DateTime.MinValue;
            FileInfo lastWritenFile = null;

            foreach (FileInfo file in files)
            {
                if (file.LastWriteTime > lastWrite)
                {
                    lastWrite = file.LastWriteTime;
                    lastWritenFile = file;
                }
            }
            Console.WriteLine(lastWritenFile);



            //string path = @"index.html";
            //FileInfo fi1 = new FileInfo(path);
            //Console.WriteLine(fi1);


            MailMessage m = new MailMessage();
            SmtpClient sc = new SmtpClient();
            try
            {
                m.From = new MailAddress("deepa.nair@fft.org.uk", "Deepa");
        //     m.To.Add(new MailAddress("Marion.Williams@fft.org.uk", "Marion"));
              
           m.To.Add(new MailAddress("deepa.nair@fft.org.uk", "Deepa"));

       //      m.CC.Add(new MailAddress("Suseela.Sarvepalli@fft.org.uk", "Suseela"));
               m.CC.Add(new MailAddress("deepa.nair@fft.org.uk", "Deepa"));
                //similarly BCC

        
                m.Subject = "APT/CT-Regression Test Report on Test Env";
                m.Body = "Please find the Automated Test report attached.\n\n Regards\nDeepa";

                m.Attachments.Add(new Attachment(reportFolder + lastWritenFile));
                //m.Attachments.Add(new Attachment(reportFolder + fi1));

                //m.Attachments.Add(new Attachment(reportFolder + lastWritenFile));

                // sc.Host = "outlook.office365.com";
                sc.Host = "smtp.office365.com";
                sc.Port = 587;
                sc.Credentials = new System.Net.NetworkCredential("DNair@fft.org.uk", "yjycqxhvdrxhrvzx");
                sc.EnableSsl = true; // runtime encrypt the SMTP communications using SSL
                sc.Send(m);
            }
            catch (Exception ex)
            {

                Assert.Fail("Fail the Test-" + ex.Message);
                // Console.WriteLine(ex.Message);
                // Console.ReadLine();
            }
        }

    }
}

