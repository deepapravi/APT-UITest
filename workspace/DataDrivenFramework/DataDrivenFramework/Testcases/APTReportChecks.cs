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
  
    public class AI_APTReportChecks : BaseTest
    {
        static ExcelReaderFile xls = new ExcelReaderFile(ConfigurationManager.AppSettings["xlsPath"]);
        static string testCaseName = "APTReportValidation";

        [Test, TestCaseSource("getData")]
        public void APTReports(Dictionary<string, string> data)
        {
            rep = ExtentManager.getInstance();
            test = rep.CreateTest("APTReportValidation Test-" + data["TestId"] + "",
                "This test will validate each of the APT reports and structure");
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

                    reportFailure("APT reportTest has failed");
                    test.Log(Status.Info,
                        "APT reportTest has failed because the given assessment does not exist: " +
                        data["AssessmentName"]);
                    takeScreenshot();

                }

                click("EnterpageEditAssessmentLink_Xpath");
                Thread.Sleep(1000);
                explicitWait("EnterpagepupilLabel_Xpath");
                Thread.Sleep(1000);
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
                else
                {
                    // test.Log(Status.Info, ": " + data["AssessmentName"]);
                    reportPass("APT Summary report Overview looks Ok for assessment :" + data["AssessmentName"]);
                }


                if(VerifyCompareReport("ComparewithfirstAssessment_Xpath").Equals(false))
                {

                    reportFailure("APT Compare reportTest has failed");
                    test.Log(Status.Info,
                        "APT Compare report Test has failed for : " +
                        data["AssessmentName"]);
                    takeScreenshot();
                }
                else
                {

                    reportPass("SummaryReport-APT compare report is working fine for assessment created :" + data["AssessmentName"]);
                }


                Thread.Sleep(1000);
                click("APTyeargrouplink_Xpath");
                test.Log(Status.Info, "Checking APT Yeargroup report");
                explicitWait("reportHeader_Xpath");

                if (waitforElementPresent("AllYearLabel_Xpath").Equals(false))
                {
                    reportFailure("APT reportTest has failed");
                    test.Log(Status.Info,
                        "APT reportTest has failed because APT Year group report table is not shown for : " +
                        data["AssessmentName"]);
                    takeScreenshot();

                }
                else
                {
                    reportPass("APT Yeargroup report looks Ok for assessment :" + data["AssessmentName"]);
                }
                Thread.Sleep(1000);
                if (VerifyCompareReport("ComparewithfirstAssessment_Xpath").Equals(false))
                {

                    reportFailure("APT Compare reportTest has failed");
                    test.Log(Status.Info,
                        "APT Compare report Test has failed for : " +
                        data["AssessmentName"]);
                    takeScreenshot();
                }
                else
                {

                    reportPass(" Yeargroup report-APT compare report is working fine for assessment created :" + data["AssessmentName"]);
                }

                Thread.Sleep(1000);
                click("APTClassesreportlink_Xpath");
                test.Log(Status.Info, "Checking APT Classes report");
                explicitWait("reportHeader_Xpath");

                if (waitforElementPresent("AllClassLabel_Xpath").Equals(false))
                {
                    reportFailure("APT reportTest has failed");
                    test.Log(Status.Info,
                        "APT reportTest has failed because APT Classes report table is not shown for : " +
                        data["AssessmentName"]);
                    takeScreenshot();

                }
                else
                {
                    reportPass("APT Classes report looks Ok for assessment :" + data["AssessmentName"]);
                }
                Thread.Sleep(1000);
                if (VerifyCompareReport("ComparewithfirstAssessment_Xpath").Equals(false))
                {

                    reportFailure("APT Compare reportTest has failed");
                    test.Log(Status.Info,
                        "APT Compare report Test has failed for : " +
                        data["AssessmentName"]);
                    takeScreenshot();
                }
                else
                {

                    reportPass(" Classes report-APT compare report is working fine for assessment created :" + data["AssessmentName"]);
                }
                Thread.Sleep(1000);

                click("APTpupilgrouplink_Xpath");
                test.Log(Status.Info, "Checking APT Pupilgroup report");
                explicitWait("reportHeader_Xpath");

                if (waitforElementPresent("AllpupilLabel1_Xpath").Equals(false) &
                       waitforElementPresent("AllpupilLabel2_Xpath").Equals(false))
                {
                    reportFailure("APT reportTest has failed");
                    test.Log(Status.Info,
                        "APT reportTest has failed because APT Pupilgroup report table is not shown for : " +
                        data["AssessmentName"]);
                    takeScreenshot();

                }
                else
                {
                    reportPass("APT Pupil group report looks Ok for assessment :" + data["AssessmentName"]);
                }
                Thread.Sleep(1000);
                if (VerifyCompareReport("ComparewithfirstAssessment_Xpath").Equals(false))
                {

                    reportFailure("APT Compare reportTest has failed");
                    test.Log(Status.Info,
                        "APT Compare report Test has failed for : " +
                        data["AssessmentName"]);
                    takeScreenshot();
                }
                else
                {

                    reportPass("Pupil group report-APT compare report is working fine for assessment created :" + data["AssessmentName"]);
                }
                Thread.Sleep(1000);
                click("MySchoolgrouplink_Xpath");
                test.Log(Status.Info, "Checking APT Schoolgroup report");
                explicitWait("reportHeader_Xpath");

                if (waitforElementPresent("SchoolGroupLabel_Xpath").Equals(false))
                {
                    reportFailure("APT reportTest has failed");
                    test.Log(Status.Info,
                        "APT reportTest has failed because APT School group report table is not shown for : " +
                        data["AssessmentName"]);
                    takeScreenshot();

                }
                else
                {
                    reportPass("APT School group report looks Ok for assessment :" + data["AssessmentName"]);
                }

                Thread.Sleep(2000);
                if (VerifyCompareReport("ComparewithfirstAssessment_Xpath").Equals(true))
                {

                    reportFailure("APT Compare reportTest has failed");
                    test.Log(Status.Info,
                        "APT Compare report link shown in My School group report. Test has failed for : " +
                        data["AssessmentName"]);
                    takeScreenshot();

                }
                else
                {
                    reportPass("MySchoolgroupReport-APT compare report is not shown in MySchoolgroupReport");
                }


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
                    reportPass("APT Pupils report looks Ok for assessment :" + data["AssessmentName"]);
                }
                Thread.Sleep(1000);

                if (VerifyCompareReport("ComparePupilsReport_Xpath").Equals(false))
                {

                    reportFailure("APT Compare reportTest has failed");
                    test.Log(Status.Info,
                        "APT Compare report Test has failed for : " +
                        data["AssessmentName"]);
                    takeScreenshot();
                }
                else
                {

                    reportPass("Pupils report-APT compare report is working fine for assessment created :" + data["AssessmentName"]);
                }

                Thread.Sleep(1000);
                click("ScatterplotLink_Xpath");
                explicitWait("StopcomparingLabel_Xpath");
                click("StopComaringButton_Xpath");
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
                    reportPass("APT Scatter plot report looks Ok for assessment :" + data["AssessmentName"]);
                }
                Thread.Sleep(1000);
                if (VerifyCompareReport("ComparewithfirstAssessment_Xpath").Equals(true))
                {

                    reportFailure("APT Compare reportTest has failed");
                    test.Log(Status.Info,
                        "APT Compare report link shown in Scatterplot Report. Test has failed for : " +
                        data["AssessmentName"]);
                    takeScreenshot();

                }
                else
                {
                    reportPass("ScatterplotReport-APT compare report is not shown in ScatterplotReport");
                }
                Thread.Sleep(1000);
                click("Pathwaylink_Xpath");
                test.Log(Status.Info, "Checking APT Pathway report");
                explicitWait("PathwayAllpupil_Xpath");
                if (waitforElementPresent("Pathwayover_Xpath").Equals(false))
                {
                    reportFailure("APT reportTest has failed");
                    test.Log(Status.Info,
                        "APT reportTest has failed because APT Pathway report is not shown for : " +
                        data["AssessmentName"]);
                    takeScreenshot();

                }
                else
                {
                    reportPass("APT Pathway report looks Ok for assessment :" + data["AssessmentName"]);
                }
                Thread.Sleep(1000);

                if (VerifyCompareReport("ComparewithfirstAssessment_Xpath").Equals(true))
                {

                    reportFailure("APT Compare reportTest has failed");
                    test.Log(Status.Info,
                        "APT Compare report link shown in Pathway Report. Test has failed for : " +
                        data["AssessmentName"]);
                    takeScreenshot();

                }
                else
                {
                    reportPass("Pathway report-APT compare report is not shown in Pathway report");
                }
               
                //click("ComparereportLink_Xpath");
                //Thread.Sleep(2000);
                //if (isElementPresent("ComparewithfirstAssessment_Xpath").Equals(true))
                //{

                //    string AssessmentName = getElement("ComparewithfirstAssessment_Xpath").Text;
                //    String Assessment_Xpath =
                //                "//span[contains(text(), " + "'" + AssessmentName + "'" + ")]";
                //    click("ComparewithfirstAssessment_Xpath");
                //    explicitWait("ComparereportLabel_Xpath");
                //    if (iselementExist(Assessment_Xpath).Equals(false))
                //    {

                //        reportFailure("APT Compare reportTest has failed");
                //        test.Log(Status.Info,
                //            "APT Compare report Test has failed for : " +
                //            data["AssessmentName"]);
                //        takeScreenshot();


                //    }
                //    else
                //    {
                //        reportPass("APT compare report is working fine for assessment created :" + data["AssessmentName"]);
                //    }


                //}
                //else
                //{

                //    test.Log(Status.Info,
                //            "No matching assessment assessment found for comparison " +
                //            data["AssessmentName"]);
                //    takeScreenshot();
                //}

            

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
