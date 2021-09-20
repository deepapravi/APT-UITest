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

    public class AB_APTCheckforUpdateTest : BaseTest
    {
        static ExcelReaderFile xls = new ExcelReaderFile(ConfigurationManager.AppSettings["xlsPath"]);
        static string testCaseName = "APTCheckforUpdateTest";

        [Test, TestCaseSource("getData")]
        public void CheckforUpdate(Dictionary<string, string> data)
        {
            rep = ExtentManager.getInstance();
            test = rep.CreateTest("APT CheckforUpdate-" + data["TestId"] + "", "This test will describe APT CheckforUpdate functionality");
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
                click("CreateAssessmentTab_Xpath");
                Thread.Sleep(1000);
                explicitWait("CreateAssessmentLabel_Xpath");
                if (isElementPresent("CreateAssessmentLabel_Xpath").Equals(false))
                {
                    test.Log(Status.Info, "Create Assessment page is not shown for user: " + data["Username"]);
                    reportFailure("Create Assessment page is not shown");


                }

                click("Checkforupdate_Xpath");

                explicitWait("startupdatebutton_Xpath");
                click("startupdatebutton_Xpath");
                if(waitforElementPresent("updatedatafoundLabel_Xpath").Equals(false))

                {
                    reportFailure("Check for update is not done ");
                    test.Log(Status.Info,
                        "Check for update Function failed");
                    takeScreenshot();


                }
                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                js.ExecuteScript("window.scrollBy(0,500)");
                Thread.Sleep(1000);
                click("FinishUpdateButton_Xpath");
                    explicitWait("UpdatecompleteLabel_Xpath");
                
                    test.Log(Status.Info, "Check for update done for School user -" + data["Username"]);
                    reportPass("Check for update done successfully!!!");


                
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
