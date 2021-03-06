using AventStack.ExtentReports;
using DataDrivenFramework.Util;
using NPOI.SS.Formula.Functions;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;

using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DataDrivenFramework.Testcases
{
    [TestFixture]
    public class ZC_DeleteSchoolgroupTest:BaseTest
    {
        static ExcelReaderFile xls = new ExcelReaderFile(ConfigurationManager.AppSettings["xlsPath"]);
        static string testCaseName = "DeleteSchoolgroup";

        [Test, TestCaseSource("getData")]
        public void AK_DeleteSchoolgroup(Dictionary<string, string> data)
        {
             rep = ExtentManager.getInstance();



            test = rep.CreateTest("Delete Schoolgroup Test-" + data["TestId"] + "",
                "This test will describe Delete Schoolgroup functionality");


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

            if (CheckforElement("Dontshow_link"))
            {
                click("Dontshow_link");
            }

            try
            {
                click("Span_Xpath");
                Thread.Sleep(1000);
                click("AssessmentTab_Xpath");
                Thread.Sleep(2000);
                IWebElement element =
                    driver.FindElement(By.XPath("//a[contains(text(),'Create & edit my school groups')]"));

                element.Click();

                //click("CreateSchoolgroupTab_Xpath");
                Thread.Sleep(3000);
                explicitWait("MySchoolgroupLabel_Xpath");
                if (IsElementVisible("MySchoolgroupLabel_Xpath").Equals(false))
                {


                    test.Log(Status.Info, "My Schoolgroup Page not shown!!");
                    reportFailure("My School group Page not shown!!!");

                }

                type("SchoolgroupSearchInput_Xpath",data["SchoolgroupName"]);

                String Group_Xpath = "//span[contains(text(), " + "'" + data["SchoolgroupName"] + "'" + ")]";

                if (iselementExist(Group_Xpath).Equals(false))
                {
                    test.Log(Status.Info, "SchoolGroup Not exist-" + data["SchoolgroupName"]);
                    reportFailure("Schoolgroup not exist");


                }

                else
                {
                    Thread.Sleep(1000);
                    click("SchoolgroupEditLink_Xpath");
                    Thread.Sleep(1000);
                    click("SchoolgroupDeleteButton_Xpath");
                    Thread.Sleep(1000);
                    click("SchoolgroupConfirmDelete_Xpath");
                    Thread.Sleep(1000);
                    explicitWait("MySchoolgroupLabel_Xpath");
                    if (IsElementVisible("MySchoolgroupLabel_Xpath").Equals(true) & iselementExist(Group_Xpath).Equals(false))
                    {

                        test.Log(Status.Info, "SchoolGroup Deleted -" + data["SchoolgroupName"]);
                        reportPass("SchoolGroup Deleted successfully");

                    }
                    else
                    {

                        test.Log(Status.Info, "SchoolGroup Not Created-" + data["SchoolgroupName"]);
                        reportFailure("Schoolgroup not Created");

                    }



                }
            }
            catch (Exception e)
            {
                if (errcount != 1)
                {
                    reportFailure(e.Message);
                }

                Assert.Fail("Fail the Test-" + e.Message);
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
