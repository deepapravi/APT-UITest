using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataDrivenFramework
{

    public class ExtentManager
    {

        public static ExtentV3HtmlReporter htmlReporter;

        private static ExtentReports extent;

        private ExtentManager()
        {

        }

        public static ExtentReports getInstance()
        {
            if (extent == null)
            {
                String dir = AppDomain.CurrentDomain.BaseDirectory;
                FileInfo fileInfo = new FileInfo(dir);
                DirectoryInfo currentDir = fileInfo.Directory.Parent.Parent;
                string parentDirName = currentDir.FullName;
                //string reportPath = @"C:\Deepa\Reports\Report.html";
                string reportFile = DateTime.Now.ToString().Replace("/", "_").Replace(":", "_").Replace(" ", "_") + ".html";
                //   htmlReporter = new ExtentHtmlReporter(@"C:\Deepa\Reports\"+reportFile);




                //  htmlReporter = new ExtentV3HtmlReporter(parentDirName + "./Reports/"+ reportFile);

                //  htmlReporter = new ExtentV3HtmlReporter(@"\\mercury\Development\Testresults\Reports\"+ reportFile);

                htmlReporter = new ExtentV3HtmlReporter(@"\\mimas.fft.local\tfs-build-reports\Testresults\Reports\" + reportFile);

                htmlReporter.Config.Theme = AventStack.ExtentReports.Reporter.Configuration.Theme.Dark;

                extent = new ExtentReports();

                extent.AttachReporter(htmlReporter);
                extent.AddSystemInfo("OS", "Windows");
                extent.AddSystemInfo("Host Name", "FFT4");
                extent.AddSystemInfo("Environment", "Test");
                extent.AddSystemInfo("UserName", "Deepa");

                //string filePath = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory);
                //filePath = Directory.GetParent(Directory.GetParent(filePath).FullName).FullName;
                string filePath = @"\\mimas.fft.local\tfs-build-reports\Testresults\Reports\";
                // string extentConfigPath = @"C:\Users\DNair\source\repos\FFT.Aspire\FFT.Aspire.Test.UITests\workspace\AutomatedBuild\AutomatedBuild\extent-config.xml";
                // htmlReporter.LoadConfig(ConfigurationManager.AppSettings["extentConfigPath"]);
                htmlReporter.LoadConfig(filePath + "\\extent-config.xml");
                //htmlReporter.LoadConfig().setAutoCreateRelativePathMedia(true);
                //Console.WriteLine(filePath);
                //htmlReporter.LoadConfig(extentConfigPath);
            }
            return extent;
        }
    }
}
