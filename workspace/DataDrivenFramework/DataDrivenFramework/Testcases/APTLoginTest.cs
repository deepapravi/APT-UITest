using NUnit.Framework;
using AventStack.ExtentReports;
using System.Collections.Generic;
using DataDrivenFramework.Util;
using System.Configuration;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using Newtonsoft.Json;
using System.Threading;

namespace DataDrivenFramework.Testcases
{
    [TestFixture]
   
    public class AA_APTLoginTest : BaseTest
    {
        static ExcelReaderFile xls = new ExcelReaderFile(ConfigurationManager.AppSettings["xlsPath"]);
        static string testCaseName = "LoginTest";

        [Test, TestCaseSource("getData")]
        public void doLogin(Dictionary<string, string> data)
        {
            rep = ExtentManager.getInstance();
            test = rep.CreateTest("LoginTest-" + data["TestId"] + "", "This test will describe APTLogin functionality");
            if (!DataUtil.isTestRunnable(testCaseName, xls) || data["Runmode"].Equals("N"))
            {

                test.Log(Status.Skip, "Skipping the Test as the runmode is No");
                Assert.Ignore("Skipping the test as runmode is No");

            }

            openBrowser(data["Browser"]);
            navigate("appurl");

            var username = string.Empty;
            var password = string.Empty;
            if (data.ContainsKey("Username"))
            {
                username = data["Username"];
            }

            password = GetPassword(username);
            //if (password.Equals(string.Empty))
            //{

            //    test.Log(Status.Info, "Skipping the Test as the runMode is No");
            //}


            bool actualResult = doLogin(username, password);
            bool expectedResult = false;
            if (data["ExpectedResult"].Equals("Y"))
                expectedResult = true;
            else
                expectedResult = false;
            if (expectedResult != actualResult)
            {
                reportFailure("Test Failed");
                takeScreenshot();
            }
            else
                reportPass("Login Test passed");
            
            

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
