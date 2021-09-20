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

    public class AH_BucketYearsPermissionTest : BaseTest
    {

        static ExcelReaderFile xls = new ExcelReaderFile(ConfigurationManager.AppSettings["xlsPath"]);
        static string testCaseName = "APTBucketYearsPermissionTest";
        [Test, TestCaseSource("getData")]
        public void BucketYears(Dictionary<string, string> data)
        {
            rep = ExtentManager.getInstance();
            if ((data["CreateAssessment"]).Equals("Y") && (data["EnterAssessment"]).Equals("Y") &&
                        (data["APTSchoolLevelOnly"]).Equals("Y") && (data["APTPupilLevelOnly"]).Equals("Y"))
                test = rep.CreateTest("BucketYearsPermission Test-" + data["TestId"] + "", "This test will describe APT BucketYears access to user with Create/Edit and Enter pupil data ");

            else if ((data["CreateAssessment"]).Equals("Y") && (data["EnterAssessment"]).Equals("N") &&
                       (data["APTSchoolLevelOnly"]).Equals("Y") && (data["APTPupilLevelOnly"]).Equals("Y"))
                test = rep.CreateTest("BucketYearsPermission Test-" + data["TestId"] + "", "This test will describe APT BucketYears access to user with Create/Edit data permission but No Enter data permission");

            else if ((data["CreateAssessment"]).Equals("N") && (data["EnterAssessment"]).Equals("Y") &&
                                 (data["APTSchoolLevelOnly"]).Equals("Y") && (data["APTPupilLevelOnly"]).Equals("Y"))
                test = rep.CreateTest("BucketYearsPermission Test-" + data["TestId"] + "", "This test will describe APT BucketYears access to user with No Create/Edit data permission but Enter data permission");
          
            else if ((data["CreateAssessment"]).Equals("N") && (data["EnterAssessment"]).Equals("N") &&
                                 (data["APTSchoolLevelOnly"]).Equals("Y") && (data["APTPupilLevelOnly"]).Equals("N"))

                test = rep.CreateTest("BucketYearsPermission Test-" + data["TestId"] + "", "This test will describe APT BucketYears access to user with School level report access Only, No pupil level");

            else if ((data["CreateAssessment"]).Equals("N") && (data["EnterAssessment"]).Equals("N") &&
                                   (data["APTSchoolLevelOnly"]).Equals("N") && (data["APTPupilLevelOnly"]).Equals("Y"))

                test = rep.CreateTest("BucketYearsPermission Test-" + data["TestId"] + "", "This test will describe APT BucketYears access to user with Pupil level report access only but no School level access");

            else
                test = rep.CreateTest("BucketYearsPermission Test", "This test checks BucketYearsPermission Test");
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



                    if ((data["CreateAssessment"]).Equals("Y") && (data["EnterAssessment"]).Equals("Y") &&
                      (data["APTSchoolLevelOnly"]).Equals("Y") && (data["APTPupilLevelOnly"]).Equals("Y"))
                    {
                        //test = rep.CreateTest("CTPermission Test-" + data["TestId"] + "", "This test will describe Permission check for user with APT Pupil level access only ");
                        click("Span_Xpath");
                        click("AssessmentTab_Xpath");
                        click("EditAssessmentTab_Xpath");
                      
                        Thread.Sleep(2000);
                        JavaScriptExecutor("BucketYearTab_Xpath");
                     
                        explicitWait("BucketYearTextBanner_Xpath");
                        test.Log(Status.Info,
                            "Testing the user : " + data["Username"] +
                            " with Create/Edit and EnterDataPermission");
                        if (isElementPresent("ViewpupilassessmenyLink_Xpath").Equals(false) | isElementPresent("ExportdataLink_Xpath").Equals(false) | isElementPresent("reportLink_Xpath").Equals(false))
                        {

                            reportFailure(
                                "Check the Bucket Years Assessment links for user " + data["Username"]);


                        }

                        else
                        {
                            test.Log(Status.Info, "ViewPupilAssessmentLink,Export DataLink and report Links are shown");
                            reportPass("Bucket Years Permission checks passed for user with Create/Edit and Enter data permission!!! ");


                        }

                    }

                    else if ((data["CreateAssessment"]).Equals("Y") && (data["EnterAssessment"]).Equals("N") &&
                      (data["APTSchoolLevelOnly"]).Equals("Y") && (data["APTPupilLevelOnly"]).Equals("Y"))
                    {
                     
                        click("Span_Xpath");
                        click("AssessmentTab_Xpath");
                        click("EditAssessmentTab_Xpath");
                     
                        Thread.Sleep(2000);

                        JavaScriptExecutor("BucketYearTab_Xpath");
                        explicitWait("BucketYearTextBanner_Xpath");

                        test.Log(Status.Info,
                            "Testing the user : " + data["Username"] +
                            " with Create/Edit but No EnterDataPermission ");
                        if (isElementPresent("ViewpupilassessmenyLink_Xpath").Equals(true) | isElementPresent("ExportdataLink_Xpath").Equals(false) | isElementPresent("reportLink_Xpath").Equals(false))
                        {

                            reportFailure(
                                "Check the Bucket Years Assessment links for user " + data["Username"]);


                        }
                        else
                        {
                            test.Log(Status.Info, "Export DataLink,report Links are shown and ViewPupilAssessment link is not shown ");
                            reportPass("Bucket Years Permission checks passed for user with Create/Edit but with No Enter data permission ");


                        }

                    }
                    else if ((data["CreateAssessment"]).Equals("N") && (data["EnterAssessment"]).Equals("Y") &&
                       (data["APTSchoolLevelOnly"]).Equals("Y") && (data["APTPupilLevelOnly"]).Equals("Y"))
                    {

                        test.Log(Status.Info,
                         "Testing the user : " + data["Username"] +
                         " with No Create/Edit but EnterDataPermission ");
                        click("Span_Xpath");
                        click("AssessmentTab_Xpath");
                        click("EnterAssessmentTab_Xpath");
                        explicitWait("EnterAssessmentLabel_Xpath");
                        JavaScriptExecutor("BucketYearTab_Xpath");
                        explicitWait("BucketYearTextBanner_Xpath");

                        if (isElementPresent("ViewpupilassessmenyLinkforEnter_Xpath").Equals(false) | isElementPresent("ExportdataLink_Xpath").Equals(false) | isElementPresent("reportLink_Xpath").Equals(false))
                        {

                            reportFailure(
                                "Check the Bucket Years Assessment links for user " + data["Username"]);


                        }

                        else
                        {
                            test.Log(Status.Info, "ViewPupilAssessmentLink,Export DataLink and report Links are shown");
                            reportPass("Bucket Years Permission checks passed for user with Enter data permission only");


                        }


                    }


                    else if ((data["CreateAssessment"]).Equals("N") && (data["EnterAssessment"]).Equals("N") &&
                      (data["APTSchoolLevelOnly"]).Equals("Y") && (data["APTPupilLevelOnly"]).Equals("N"))


                    {

                        test.Log(Status.Info,
                        "Testing the user : " + data["Username"] +
                        " with School Level report access only No pupil level ");

                        click("BucketYearsHomepageButton_Xpath");
                        explicitWait("BucketYearTextBanner_Xpath");

                        if (isElementPresent("ViewreportLinkforPupilOnly_Xpath").Equals(false) | isElementPresent("ViewpupilassessmenyLink_Xpath").Equals(true))
                        {

                            reportFailure("Check the Bucket Years Assessment links for user " + data["Username"]);

                        }

                        else
                        {

                            test.Log(Status.Info, "report Link shown and view pupilassessment link,export links are not shown");
                            reportPass("Bucket Years Permission checks passed for user with School Level report access only");


                        }

                    }

                    else if ((data["CreateAssessment"]).Equals("N") && (data["EnterAssessment"]).Equals("N") &&
                    (data["APTSchoolLevelOnly"]).Equals("N") && (data["APTPupilLevelOnly"]).Equals("Y"))
                    {
                        test.Log(Status.Info,
                       "Testing the user : " + data["Username"] +
                       " with Pupil level report access only ");

                        click("BucketYearsHomepageButton_Xpath");
                        explicitWait("BucketYearTextBanner_Xpath");

                        if (isElementPresent("ViewpupilassessmenyLink_Xpath").Equals(true) | isElementPresent("ExportdataLink_Xpath").Equals(false) | isElementPresent("reportLink_Xpath").Equals(false))
                        {

                            reportFailure("Check the Bucket Years Assessment links for user " + data["Username"]);

                        }

                        else
                        {
                            test.Log(Status.Info, "Export DataLink,report Links are shown and view pupilassessment link is not shown");
                            reportPass("Bucket Years Permission checks passed for user with Pupil Level report access only");


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