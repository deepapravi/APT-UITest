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
    public class ZB_CTDeleteAssessment:BaseTest
    { 
     static ExcelReaderFile xls = new ExcelReaderFile(ConfigurationManager.AppSettings["xlsPath"]);
     static string testCaseName = "DeleteCurriculum";
    
        [Test, TestCaseSource("getData")]
        public void GG_CTDeleteAssessment(Dictionary<string, string> data)
        {
            rep = ExtentManager.getInstance();
            test = rep.CreateTest("DeleteCTAssessment Test-" + data["TestId"] + "", "This test will describe DeleteCTAssessment functionality");
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
                test.Log(Status.Info, "Delete Assessment has failed beacuse of invalid Login: " + data["Username"]);
                takeScreenshot();

            }
            else
            {
                try
                {


                    if (CheckforElement("Dontshow_link"))
                    {
                        click("Dontshow_link");
                    }

                    click("Span_Xpath");
                    click("CurriculumTab_Xpath");
                    click("EditCTtab_Xpath");
                    explicitWait("EditCTLabel_Xpath");
                    Thread.Sleep(4000);
                    String CTAssessment_Xpath = "//h3[contains(text(), " + "'" + data["CurriculumName"] + "'" + ")]";
                    test.Log(Status.Info,
                        "Look for Assessment: " + data["CurriculumName"] + " in the Edit assessment page");
                    type("SearchCtTxt_Xpath", data["CurriculumName"]);
                    Thread.Sleep(1000);
                    JavaScriptExecutor("CTSearchbtn_Xpath");
                    Thread.Sleep(1000);
                    if (iselementExist(CTAssessment_Xpath).Equals(false))
                    {

                        reportFailure("Delete CT Assessment has failed beacuse the given assessment does not exist");
                        test.Log(Status.Info,
                            "Delete Assessment has failed beacuse the given assessment does not exist: " +
                            data["CurriculumName"]);
                        takeScreenshot();

                    }
                    else
                    {

                        Thread.Sleep(3000);
                        click("EditCTLink_Xpath");
                        Thread.Sleep(2000);
                        scrolldown("DeleteCTButton_Xpath");
                        Thread.Sleep(1000);
                        click("DeleteCTButton_Xpath");
                        Thread.Sleep(1000);

                        click("DeleteCTConfirmButton_Xpath");
                        //String CTAssessment_Xpath = "//span[contains(text(), " + "'" + data["CurriculumName"] + "'" + ")]";
                        Thread.Sleep(3000);
                        explicitWait("EditCTLabel_Xpath");
                        if (IsElementVisible("EditCTLabel_Xpath").Equals(true) &
                            iselementExist(CTAssessment_Xpath).Equals(false))
                        {

                            test.Log(Status.Info, "Assessment Deleted: " + data["CurriculumName"]);
                            reportPass("Assessment has Deleted successfully ");

                        }
                        else
                        {

                            test.Log(Status.Info, "Assessment Not Deleted: " + data["CurriculumName"]);
                            reportFailure("Assessment not Deleted");

                        }


                    }
                }
                catch (Exception ex)
                {

                    if (errcount != 1)
                    {
                        reportFailure(ex.Message);
                    }

                    Assert.Fail("Fail the Test-" + ex.Message);

                }

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
