using NUnit.Framework;
using AventStack.ExtentReports;
using System.Collections.Generic;
using DataDrivenFramework.Util;
using System.Configuration;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using Newtonsoft.Json;
using System.Threading;
using OpenQA.Selenium;

namespace DataDrivenFramework.Testcases
{
    [TestFixture]

    public class AC_APTHodderImport : BaseTest
    {
        static ExcelReaderFile xls = new ExcelReaderFile(ConfigurationManager.AppSettings["xlsPath"]);
        static string testCaseName = "HodderImportTest";
        [Test, TestCaseSource("getData")]
        public void CheckforUpdate(Dictionary<string, string> data)
        {
            rep = ExtentManager.getInstance();
            test = rep.CreateTest("APT HodderImportTest-" + data["TestId"] + "", "This test will describe APT Hodder Import functionality");
            if (!DataUtil.isTestRunnable(testCaseName, xls) || data["Runmode"].Equals("N"))
            {

                test.Log(Status.Skip, "Skipping the Test as the runmode is No");
                Assert.Ignore("Skipping the test as runmode is No");

            }

            openBrowser(data["Browser"]);
            navigate("appurl");
            var username = string.Empty;
            if (data.ContainsKey("Username"))
            {
                username = data["Username"];
            }

            var password = GetPassword(username);
            bool actualResult = doLogin(username, password);
            if (actualResult.Equals(false))
            {

                reportFailure("Login has failed");
                test.Log(Status.Info, "Create Assessment has failed because of invalid Login: " + data["Username"]);
                takeScreenshot();

            }
            else
            {
                if (CheckforElement("Dontshow_link"))
                {
                    click("Dontshow_link");
                }

                click("Span_Xpath");
                click("AssessmentTab_Xpath");
                Thread.Sleep(1000);
                click("ImportAssessment_link");
                explicitWait("choosetestproviderLabel_Xpath");
                Thread.Sleep(2000);
                click("Choosebutton_Xpath");
                Thread.Sleep(3000);
                click("Importbutton_Xpath");
                explicitWait("ImportassessmentLabel_Xpath");
                if (waitforElementPresent("ImportPanel_Xpath").Equals(false))
                {
                    reportFailure("Import assessment is not done ");
                    test.Log(Status.Info,
                        "Import assessment is not done");
                    takeScreenshot();



                }
                Thread.Sleep(1000);
                string count = getText("Importcount_Xpath");
                test.Log(Status.Info," Number of assessments shown for import is -" + count);
                reportPass(" Number of assessments shown for import is -" + count + " for user: "  + data["Username"] );

            }
        }

        public static object[] getData()
        {
            //reads data for only testCaseName

            return DataUtil.getTestData(xls, testCaseName);

        }

        [TearDown]
        public void quit()
        {

            rep.Flush();
            if (driver != null)
                driver.Quit();
        }

    }


}