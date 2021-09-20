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
    public class AC_APTDuplicateAssessmentTest : BaseTest
    {
        static ExcelReaderFile xls = new ExcelReaderFile(ConfigurationManager.AppSettings["xlsPath"]);
        static string testCaseName = "APTDuplicateAssessmentTest";
        [Test, TestCaseSource("getData")]
        public void Duplicate(Dictionary<string, string> data)
        {
            rep = ExtentManager.getInstance();
            test = rep.CreateTest("APTDuplicateAssessmentTest Test-" + data["TestId"] + "", "This test will describe APTDuplicateAssessment functionality");
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
                test.Log(Status.Info, "Create Assessment has failed beacuse of invalid Login: " + data["Username"]);
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
                    click("AssessmentTab_Xpath");
                    click("EditAssessmentTab_Xpath");
                    Thread.Sleep(2000);
                    String APTAssessment_Xpath = "//span[contains(text(), " + "'" + data["AssessmentName"] + "'" + ")]";
                    test.Log(Status.Info,
                        "Look for Assessment: " + data["AssessmentName"] + " in the Edit assessment page");
                    type("SearchAssessmentTxt_Xpath", data["AssessmentName"]);
                    if (iselementExist(APTAssessment_Xpath).Equals(false))
                    {

                     
                        test.Log(Status.Info,
                            "Duplicate Assessment has failed beacuse the given assessment does not exist: " +
                            data["AssessmentName"]);
                        reportFailure("Duplicate APT Assessment has failed");
                        takeScreenshot();

                    }
                    
                    
           
                    

                        //   IWebElement element = driver.FindElement(By.ClassName("data - button mr - smaller trackable"));
                        IWebElement element = driver.FindElement(By.XPath("//li[1]//div[1]//p[2]//a[3]"));

                  
                        element.Click();
                    Thread.Sleep(2000);
                    type("DuplicateAssessmentTxt_Xpath", data["DuplicateAssessmentName"]);

                    if (data.ContainsKey("Description").Equals(true))
                    {
                        type("DuplicateAssessmentDesc_Xpath", data["Description"]);
                    }
                    Thread.Sleep(1000);

                    if(data.ContainsKey("CopyPupilAssessment").Equals(true))
                    {
                        if(data["CopyPupilAssessment"].Equals("Yes"))
                        {

                            click("IncludePupilAssessmentCheckbox_Xpath");

                        }


                    }

                    
                    Thread.Sleep(1000);
                    click("SaveDuplicateAssessmentButton_Xpath");
                    String Assessment_Xpath =
                               "//span[contains(text(), " + "'" + data["DuplicateAssessmentName"] + "'" + ")]";
                    Thread.Sleep(1000);
                    explicitWait("EditAssessmentLabel_Xpath");
                    if (IsElementVisible("EditAssessmentLabel_Xpath").Equals(true) &
                        iselementExist(Assessment_Xpath).Equals(true))
                    {

                        test.Log(Status.Info, "DuplicateAssessment Created -" + data["DuplicateAssessmentName"]);
                        reportPass("DuplicateAssessment Created successfully");

                    }
                    else
                    {

                        test.Log(Status.Info, "DuplicateAssessment Not Created-" + data["DuplicateAssessmentName"]);
                        reportFailure("Assessment not Created");

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