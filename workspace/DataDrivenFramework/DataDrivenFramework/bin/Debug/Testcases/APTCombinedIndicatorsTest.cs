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
    public class AC_APTCombinedIndicatorsTest : BaseTest
    {
        static ExcelReaderFile xls = new ExcelReaderFile(ConfigurationManager.AppSettings["xlsPath"]);
        static string testCaseName = "APTCombinedIndicatorsTest";
        [Test, TestCaseSource("getData")]
        public void A_CreateCombinedTestAssessment(Dictionary<string, string> data)
        {
            try
            {
                rep = ExtentManager.getInstance();
                test = rep.CreateTest("APTCombinedIndicatorsTest", "This test will validate the Create Assessment page structure");
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
                    else
                        if (CheckforElement("SetupButton_Xpath").Equals(true))
                    {

                        test.Log(Status.Info, "Create Assessment failed because Setup has not done for user: " + data["Username"]);
                        reportFailure("Setup has not Done");


                    }
                    else
                    {

                        type("AssessmentNameInput_Xpath", data["AssessmentName"]);
                        Thread.Sleep(1000);
                        click("CreateAssessmentLabel_Xpath");
                        Thread.Sleep(2000);
                        if (IsElementVisible("AssessmentValidationText_Xpath").Equals(true))
                        {
                            test.Log(Status.Info, "Assessment already exist: " + data["AssessmentName"]);
                            reportFailure("Assessment already exist!!!");


                        }
                        else
                        {

                            if (data.ContainsKey("Testdescription").Equals(true))
                            {
                                type("AssessmentDesc_Xpath", data["Testdescription"]);
                            }
                            //   if (data["Description"] != "")



                            IWebElement Ksdropdown = getElement("Keystage_Id");
                            SelectElement s = new SelectElement(Ksdropdown);
                            Thread.Sleep(1000);
                            s.SelectByText(data["KeyStage"]);

                            if (data["KeyStage"].Equals("KS1"))
                            {

                                click("Year2_Xpath");



                            }
                            else



                                click("Year6_Xpath");


                            Thread.Sleep(1000);

                            click("AssessmentTest_Xpath");

                            IJavaScriptExecutor js1 = (IJavaScriptExecutor)driver;
                            js1.ExecuteScript("window.scrollBy(0,400)");
                            click("Testprovider_Xpath");

                            IWebElement dropdown = getElement("Testprovider_Xpath");
                            SelectElement s1 = new SelectElement(dropdown);
                            if (data["TestProvider"].Equals("RisingStars"))

                            {

                                s1.SelectByText("Rising Stars");
                                click("Testprovider_Xpath");
                                Thread.Sleep(2000);


                                if (data["KeyStage"].Equals("KS1"))
                                {
                                    click("SubjectKS1RisingStarMathsTest_Xpath");
                                    click("SubjectKS1RisingReadingTest_Xpath");

                                   

                                     click("KS1-RSWritingTA_Xpath");

                                   // JavaScriptExecutor("KS1-RSWritingTA_Id");


                                    IWebElement TAdropdown = getElement("KS1-RSWritingTA_Xpath");
                                    try
                                    {
                                        SelectElement TA = new SelectElement(TAdropdown);
                                        TA.SelectByText(data["TestAssessmentName"]);
                                        test.Log(Status.Info, "Selected the Teacher Assessment: " + data["TestAssessmentName"] + " from the Writing TA dropdown");

                                    }

                                    catch (Exception ex)
                                    {

                                        test.Log(Status.Info, "Teacher Assessment: " + data["TestAssessmentName"] + " not shown in the Writing TA dropdown");
                                        reportFailure("Teacher Assessment not shown in the Writing TA dropdown");


                                    }




                                }
                                else
                                {
                                    click("SubjectKS2RisingStarMathsTest_Xpath");
                                    click("SubjectKS2RisingStarReadingTest_Xpath");
                                    click("SubjectKS2RisingStarGPSTest_Xpath");
                                   
                                    click("KS2-RSWritingTA_Xpath");

                                    IWebElement TAdropdown = getElement("KS2-RSWritingTA_Xpath");
                                    try
                                    {
                                        SelectElement TA = new SelectElement(TAdropdown);
                                        TA.SelectByText(data["TestAssessmentName"]);
                                        test.Log(Status.Info, "Selected the Teacher Assessment: " + data["TestAssessmentName"] + " from the Writing TA dropdown");

                                    }

                                    catch (Exception ex)
                                    {

                                        test.Log(Status.Info, "Teacher Assessment: " + data["TestAssessmentName"] + " not shown in the Writing TA dropdown");
                                        reportFailure("Teacher Assessment not shown in the Writing TA dropdown");


                                    }


                                    //  click("SubLiveKS2_Xpath");

                                }

                                click("CreateAssessmentButton_Xpath");
                                String Assessment_Xpath =
                                    "//span[contains(text(), " + "'" + data["AssessmentName"] + "'" + ")]";
                                Thread.Sleep(5000);
                                explicitWait("EditAssessmentLabel_Xpath");
                                if (IsElementVisible("EditAssessmentLabel_Xpath").Equals(true) &
                                    iselementExist(Assessment_Xpath).Equals(true))
                                {

                                    test.Log(Status.Info, "Assessment Created -" + data["AssessmentName"]);
                                    reportPass("Assessment Created successfully");

                                }
                                else
                                {

                                    test.Log(Status.Info, "Assessment Not Created-" + data["AssessmentName"]);
                                    reportFailure("Assessment not Created");

                                }

                                type("SearchAssessmentTxt_Xpath", data["AssessmentName"]);
                                if (iselementExist(Assessment_Xpath).Equals(false))
                                {

                                    reportFailure("The given APT assessment not exist ");
                                    test.Log(Status.Info,
                                        "The given assessment does not exist: " +
                                        data["AssessmentName"]);
                                    takeScreenshot();

                                }
                                else
                                {

                                    Thread.Sleep(2000);
                                    test.Log(Status.Info,
                                        "Look for Assessment: " + data["AssessmentName"] + " in the Edit assessment page");

                                    click("EnterpageEditAssessmentLink_Xpath");
                                    Thread.Sleep(1000);
                                    explicitWait("EnterpagepupilLabel_Xpath");


                                    if (data.ContainsKey("TA").Equals(true))
                                    {
                                        type("EnterTestTA_Xpath", data["TA"]);
                                    }

                                    click("EnterpagepupilLabel_Xpath");

                                    click("SaveTA_Xpath");

                                    explicitWait("AssessmentSavedLabel_Xpath");


                                    test.Log(Status.Info, "Assessment Created and Saved the data -" + data["AssessmentName"]);
                                    reportPass("Assessment Created and Entered the data successfully");
                                }


                                click("APTreportlink_Xpath");
                                explicitWait("reportHeader_Xpath");
                                test.Log(Status.Info, "Checking the Summary report");
                                if (waitforElementPresent("Summarydatatable_Xpath").Equals(false))
                                {
                                    reportFailure("APT reportTest has failed");
                                    test.Log(Status.Info,
                                        "APT reportTest has failed because the Summary report table is not shown for : " +
                                        data["AssessmentName"]);
                                    takeScreenshot();


                                }

                                if (data["KeyStage"].Equals("KS2"))
                                {
                                    foreach (KeyValuePair<int, string> indicator in KS2IndicatorsList.VerifyKS2IndicatorsList())

                                    {

                                        if (iselementExist(indicator.Value).Equals(false))
                                        {

                                            reportFailure("Missing Indicators");
                                            test.Log(Status.Info,
                                                "APT reportTest has failed because the Summary report table is missing Indicators for : " +
                                                data["AssessmentName"]);
                                            takeScreenshot();
                                            break;

                                        }

                                    }


                                    test.Log(Status.Info, "Verified the Combined indicators in the KS2 Summary report for  -" + data["AssessmentName"]);
                                    reportPass("All the KS2 indicators are available in the Summary Report-Test Passed");


                                 }

                                if (data["KeyStage"].Equals("KS1"))
                                {

                                    foreach (KeyValuePair<int, string> indicator in KS1IndicatorsList.VerifyKS1IndicatorsList())

                                    {

                                        if (iselementExist(indicator.Value).Equals(false))
                                        {

                                            reportFailure("Missing Indicators");
                                            test.Log(Status.Info,
                                                "APT reportTest has failed because the Summary report table is missing Indicators for : " +
                                                data["AssessmentName"]);
                                            takeScreenshot();
                                            break;

                                        }

                                    }


                                    test.Log(Status.Info, "Verified the Combined indicators in the KS1 Summary report for  -" + data["AssessmentName"]);
                                    reportPass("All the KS1 indicators are available in the Summary Report-Test Passed");



                                }


                            }
                        }
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