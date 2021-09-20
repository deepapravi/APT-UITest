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
 
    public class MATaccessToAPTReport:BaseTest
    {
        static ExcelReaderFile xls = new ExcelReaderFile(ConfigurationManager.AppSettings["xlsPath"]);
        static string testCaseName = "MATPermissionToAPTReport";
        [Test, TestCaseSource("getData")]
        public void MATaccessToAPTReportTest(Dictionary<string, string> data)
        {
            rep = ExtentManager.getInstance();
            if ((data["APTPupilLevelOnly"]).Equals("N") && (data["APTSchoolLevelOnly"]).Equals("Y"))
                      
                test = rep.CreateTest("MAT Pemission Test-" + data["TestId"] + "", "This test will describe MAT access to APT report with No pupil level permisssion");

            if ((data["APTPupilLevelOnly"]).Equals("Y") && (data["APTSchoolLevelOnly"]).Equals("N"))

                test = rep.CreateTest("MAT Pemission Test-" + data["TestId"] + "", "This test will describe MAT access to APT report with No School level permisssion");

            if ((data["APTPupilLevelOnly"]).Equals("Y") && (data["APTSchoolLevelOnly"]).Equals("Y"))

                test = rep.CreateTest("MAT Pemission Test-" + data["TestId"] + "", "This test will describe MAT access to APT report with both pupil and school level permission");

            // test = rep.CreateTest("MAT access to APT Reports", "This test checks MAT -School can access APT report ");

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
                test.Log(Status.Info, "Test failed beacuse of invalid Login: " + data["Username"]);
                takeScreenshot();

            }
            try
            {
                if ((data["APTPupilLevelOnly"]).Equals("N") && (data["APTSchoolLevelOnly"]).Equals("Y"))
                {
                    explicitWait("MATHomepage_Xpath");
                Thread.Sleep(3000);
                click("MATSchoolselect_Xpath");

                explicitWait("APTHomePage_Xpath");
                Thread.Sleep(3000);
                hoverElement("pupiltrackingMenu_Xpath");
                test.Log(Status.Info, "Checking the CT unavailable Label for MAT user");
                if (IsElementVisible("CTnotavailableLabel_Xpath").Equals(false))
                    {

                    reportFailure("APT reportTest has failed");
                    test.Log(Status.Info,
                        "MAT do not have access to CT is not shown");
                    takeScreenshot();

                }
                reportPass("MAT user do not have access to CT");
               
                    click("pupiltrackingMenu_Xpath");
                    explicitWait("reportHeader_Xpath");
                    test.Log(Status.Info, "Verifying that Enter pupil assessment link");
                    if (iselementExist("EnterassessmentLink_Xpath").Equals(true))
                    {
                        reportFailure("Enter assessmentLink is shown for user with No pupil access");


                    }
                    reportPass("Enter pupil assessment link is not shown for MAT user");

                    test.Log(Status.Info, "Verifying Compare assessment link");
                    if (isElementPresent("ComparereportLink__Plink").Equals(false))
                    {
                        reportFailure("Compare assessment link is not shown");

                    }
                    reportPass("Compare assessment link is shown for MAT user");

                    APTAggregateReportTest();

                    Thread.Sleep(1000);
                    click("Pathwaylink_Xpath");
                    test.Log(Status.Info, "Checking APT Pathway report");
                    explicitWait("PathwayAllpupilLabel_Xpath");
                    if (waitforElementPresent("Allpupildropdown_Xpath").Equals(true))
                    {
                        reportFailure("APT reportTest has failed");
                        test.Log(Status.Info,
                            "APT reportTest has failed because APT Pathway report Pupil drop down is shown"
                            );
                        takeScreenshot();

                    }

                    reportPass("APT Pathway report is shown but no Pupils drop down is shown ");


                    Thread.Sleep(1000);
                    click("Pupilreportlink_Xpath");
                    test.Log(Status.Info, "Checking APT Pupils report");
                    if (waitforElementPresent("Noaccesslink_Xpath").Equals(false))
                    {
                        reportFailure("APT reportTest has failed");
                        test.Log(Status.Info,
                            "APT reportTest has failed because APT Pupils report has  shown");

                        takeScreenshot();

                    }
                    reportPass("APT Pupils report gives No access");
                   
                    Thread.Sleep(1000);
                    driver.Navigate().Back();
                    explicitWait("PathwayAllpupilLabel_Xpath");
                    Thread.Sleep(1000);
                    click("ScatterplotReportLink_Xpath");
                    test.Log(Status.Info, "Checking APT Scatterplot report");

                    if (waitforElementPresent("Noaccesslink_Xpath").Equals(false))
                    {
                        reportFailure("APT reportTest has failed");
                        test.Log(Status.Info,
                            "APT reportTest has failed because APT Pupils report has  shown");

                        takeScreenshot();

                    }
                    reportPass("APT Scatter plot report gives No access");

                  /*  Thread.Sleep(1000);
                    driver.Navigate().Back();
                    Thread.Sleep(4000);
                    hoverElement("pupiltrackingMenu_Xpath");
                    Thread.Sleep(1000);
                    test.Log(Status.Info, "Checking MATuser access to completed year assessment");
                    click("CompletedYearMenu_Xpath");
                    explicitWait("BucketYearTextBanner_Xpath");

                    if (isElementPresent("ViewreportLinkforPupilOnly_Xpath").Equals(false) | isElementPresent("ViewpupilassessmenyLink_Xpath").Equals(true))
                    {

                        reportFailure("Check the Bucket Years Assessment links for user " + data["Username"]);

                    }

                    else
                    {

                        test.Log(Status.Info, "report Link shown and view pupilassessment link,export links are not shown");
                        reportPass("MATuser with no pupil level permission access to completed year assessment-  pupilassessment link,export links are not shown for completed year assessments");


                    }
                    click("ViewreportLinkforPupilOnly_Xpath");

                    var newWindowHandle = driver.WindowHandles[1];
                    driver.SwitchTo().Window(newWindowHandle);
                    Thread.Sleep(2000);
                    test.Log(Status.Info, "Checking the APT report for completed year assessments in MAT level");
                    explicitWait("reportHeader_Xpath");

                    APTAggregateReportTest();

                    Thread.Sleep(1000);
                    click("Pupilreportlink_Xpath");
                    test.Log(Status.Info, "Checking APT Pupils report");
                    if (waitforElementPresent("Noaccesslink_Xpath").Equals(false))
                    {
                        reportFailure("APT reportTest has failed");
                        test.Log(Status.Info,
                            "APT reportTest has failed because APT Pupils report has  shown");

                        takeScreenshot();

                    }
                    reportPass("APT Pupils report gives No access for MATuser- completed Year assessment");*/
                }

              else  if ((data["APTPupilLevelOnly"]).Equals("Y") && (data["APTSchoolLevelOnly"]).Equals("N"))
                {

                    test.Log(Status.Info,
                            "Checking the MAT user home page with No school level access");
                    explicitWait("NoSchoolLabel_Xpath");

                    if (waitforElementPresent("NoSchoolLabel_Xpath").Equals(false))
                    {
                        reportFailure("MAT Test has failed");
                        test.Log(Status.Info,
                            "MAT Test has failed -School list is shown for MAT user with No School level acces");

                        takeScreenshot();

                    }
                    reportPass("No schools to display Label is shown for MAT user with No School level access");

                }

                else if ((data["APTPupilLevelOnly"]).Equals("Y") && (data["APTSchoolLevelOnly"]).Equals("Y"))
                {

                    explicitWait("MATHomepage_Xpath");
                    Thread.Sleep(3000);
                    click("MATSchoolselectStepAcademy_Xpath");

                    explicitWait("APTHomePage_Xpath");
                    Thread.Sleep(3000);
                    hoverElement("pupiltrackingMenu_Xpath");
                    test.Log(Status.Info, "Checking the CT unavailable Label for MAT user");
                    if (IsElementVisible("CTnotavailableLabel_Xpath").Equals(false))
                    {

                        reportFailure("APT reportTest has failed");
                        test.Log(Status.Info,
                            "MAT do not have access to CT is not shown");
                        takeScreenshot();

                    }
                    reportPass("MAT user do not have access to CT");

                    click("pupiltrackingMenu_Xpath");
                    explicitWait("reportHeader_Xpath");
                    test.Log(Status.Info, "Verifying that Enter pupil assessment link");
                    if (iselementExist("EnterassessmentLink_Xpath").Equals(true))
                    {
                        reportFailure("Enter assessmentLink is shown for user with No pupil access");


                    }
                    reportPass("Enter pupil assessment link is not shown for MAT user");

                    test.Log(Status.Info, "Verifying Compare assessment link");
                    if (isElementPresent("ComparereportLink__Plink").Equals(false))
                    {
                        reportFailure("Compare assessment link is not shown");

                    }
                    reportPass("Compare assessment link is shown for MAT user");

                    APTAggregateReportTest();
                    Thread.Sleep(1000);
                    click("Pathwaylink_Xpath");
                    test.Log(Status.Info, "Checking APT Pathway report");
                    explicitWait("PathwayAllpupil_Xpath");
                    if (waitforElementPresent("Allpupildropdown_Xpath").Equals(false))
                    {
                        reportFailure("APT reportTest has failed");
                        test.Log(Status.Info,
                            "APT reportTest has failed because APT Pathway report Pupil drop down is not shown"
                            );
                        takeScreenshot();

                    }

                    reportPass("APT Pathway report is shown and can see ALL ppupils dropdown ");


                    Thread.Sleep(1000);
                    click("Pupilreportlink_Xpath");
                    test.Log(Status.Info, "Checking APT Pupils report");
                    explicitWait("PupilsLabel_Xpath");
                    if (waitforElementPresent("PupilsSubLabel_Xpath").Equals(false))
                    {
                        reportFailure("APT reportTest has failed");
                        test.Log(Status.Info,
                            "APT reportTest has failed because APT Pupils Report is not shown for : " +
                            data["AssessmentName"]);
                        takeScreenshot();

                    }
                    else
                    {
                        reportPass("APT Pupils report looks Ok for MAT user");
                    }
                    Thread.Sleep(1000);

                   
                    click("ScatterplotLink_Xpath");
                   
                   
                    test.Log(Status.Info, "Checking APT Scatterplot report");
                    explicitWait("scatterplotAllPupilbtn_Xpath");
                    if (waitforElementPresent("scatterplothighlightLabel_Xpath").Equals(false))
                    {
                        reportFailure("APT reportTest has failed");
                        test.Log(Status.Info,
                            "APT reportTest has failed because APT Scatter plot Report is not shown for : " +
                            data["AssessmentName"]);
                        takeScreenshot();

                    }
                    else
                    {
                        reportPass("APT Scatter plot report looks Ok for MAT user");
                    }
                  /*  Thread.Sleep(1000);
                    hoverElement("pupiltrackingMenu_Xpath");
                    Thread.Sleep(1000);
                    test.Log(Status.Info, "Checking MATuser access to completed year assessment");
                    click("MATCompletedYearMenu_Xpath");
                    explicitWait("BucketYearTextBanner_Xpath");
                    if (isElementPresent("ViewpupilassessmenyLink_Xpath").Equals(true) | isElementPresent("ExportdataLink_Xpath").Equals(false) | isElementPresent("reportLink_Xpath").Equals(false))
                    {

                        reportFailure(
                            "Check the Bucket Years Assessment links for user " + data["Username"]);


                    }

                    else
                    {
                        test.Log(Status.Info, "report Link ,export links are shown");
                        reportPass("MATuser with all permission access to completed year assessment- report Link ,export links are shown. View pupil assessment link is not shown");



                    }

                    click("reportLink_Xpath");

                    var newWindowHandle = driver.WindowHandles[1];
                    driver.SwitchTo().Window(newWindowHandle);
                    Thread.Sleep(2000);
                    test.Log(Status.Info, "Checking the APT report for completed year assessments in MAT level");
                    explicitWait("reportHeader_Xpath");

                    APTAggregateReportTest();

                    Thread.Sleep(1000);
                   
                    click("Pupilreportlink_Xpath");
                    test.Log(Status.Info, "Checking APT Pupils report");
                    explicitWait("PupilsLabel_Xpath");
                    if (waitforElementPresent("PupilsSubLabel_Xpath").Equals(false))
                    {
                        reportFailure("APT reportTest has failed");
                        test.Log(Status.Info,
                            "APT reportTest has failed because APT Pupils Report is not shown for : " +
                            data["AssessmentName"]);
                        takeScreenshot();

                    }
                    else
                    {
                        reportPass("APT Pupils report looks Ok for MAT user");
                    }
                    Thread.Sleep(1000);

                    test.Log(Status.Info, "Checking other links in the Report");

                    if (iselementExist("ComparereportLink_Xpath").Equals(true))
                    {
                        reportFailure("APT Compare report Link is shown for MAT user Bucket Years Assessment Report");


                    }

                    else if (iselementExist("EnterassessmentLink_Xpath").Equals(true))
                    {
                        reportFailure("Enter assessmentLink is shown for  MAT user Bucket Years Assessment Report");


                    }

                    reportPass("Enter assessment,Compare assessment link  is not shown for  MAT user Bucket Years Assessment Report");*/
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
