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

    public class AH_BucketYearsAssessmnetsAndReportChecks : BaseTest
    {
        static ExcelReaderFile xls = new ExcelReaderFile(ConfigurationManager.AppSettings["xlsPath"]);
        static string testCaseName = "APTBucketYearsAssessmentAndReportTest";
        [Test, TestCaseSource("getData")]
        public void BucketYearsReportChecks(Dictionary<string, string> data)
        {
            rep = ExtentManager.getInstance();
            test = rep.CreateTest("BucketYears Assessment And Report Test", "This test checks BucketYears Assessment data and Report Test");
         
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



                    
                        //test = rep.CreateTest("CTPermission Test-" + data["TestId"] + "", "This test will describe Permission check for user with APT Pupil level access only ");
                        click("Span_Xpath");
                        click("AssessmentTab_Xpath");
                        click("EditAssessmentTab_Xpath");
                        explicitWait("EditAssessmentLabel_Xpath");
                       
                        JavaScriptExecutor("BucketYearTab_Xpath");

                        explicitWait("BucketYearTextBanner_Xpath");
                    test.Log(Status.Info, "Verifying the pupil assessment export for completed year assessment");

                    click("ExportdataLink_Xpath");
                    if(VerifyFileDownload().Equals(false))
                    {

                        reportFailure("Export is not working");

                    }
                    reportPass(" pupil assessment export is working fine for completed year assessment");
                    click("ViewpupilassessmenyLink_Xpath");
                    explicitWait("EnterPupilassessmentLabel_Xpath");

                    if(getElement("EnterPageYeardropdown_Xpath").Enabled.Equals(true))
                    {

                        reportFailure("Year drop down is enabled in the Enter pupil assessment page for Bucket years assessment " + data["Username"]);
                    }

                    else if (iselementExist("APTreportlink_Xpath").Equals(true))
                    {
                                            
                     reportFailure("View report link is shown in the Enter pupil assessment page for Bucket Years Assessment " + data["Username"]);
                                                
                    }
                    else if(iselementExist("SaveTA_Xpath").Equals(true))
                    {
                        reportFailure("Save button isshown in the Enter pupil assessment page for Bucket Years Assessment " + data["Username"]);

                    }

                    else if(getElement("APTEnterMathsTA_Xpath").Enabled.Equals(true))
                    {

                        reportFailure("Assessment data field is Enabled in the Enter pupil assessment page for Bucket Years Assessment " + data["Username"]);

                    }

                    else
                    {
                        reportPass("Enter pupil assessment page validation Done for Bucket Years Assessment and Test has Passed!!!");


                    }

                    click("BacktoListLink_Xpath");
                 //   JavaScriptExecutor("BacktoListLink_Xpath");
                    explicitWait("BucketYearTab_Xpath");
                    Thread.Sleep(2000);
                    click("reportLink_Xpath");
                  
                    var newWindowHandle = driver.WindowHandles[1];
                    driver.SwitchTo().Window(newWindowHandle);
                    Thread.Sleep(2000);
                    explicitWait("reportHeader_Xpath");

                    test.Log(Status.Info, "Checking the Summary report");
                    if (waitforElementPresent("Summarydatatable_Xpath").Equals(false))
                    {
                        reportFailure("APT reportTest has failed");
                        test.Log(Status.Info,
                            "APT reportTest has failed because the Summary report table is not shown for  Completed Year assessment");
                        takeScreenshot();


                    }
                    else
                    {
                        // test.Log(Status.Info, ": " + data["AssessmentName"]);
                        reportPass("APT Summary report Overview looks Ok for Completed Year assessment :");
                    }

                    Thread.Sleep(1000);
                    click("APTyeargrouplink_Xpath");
                    test.Log(Status.Info, "Checking APT Yeargroup report");
                    explicitWait("reportHeader_Xpath");

                    if (waitforElementPresent("AllYearLabel_Xpath").Equals(false))
                    {
                        reportFailure("APT reportTest has failed");
                        test.Log(Status.Info,
                            "APT reportTest has failed because APT Year group report table is not shown Completed Year assessment");
                        takeScreenshot();

                    }
                    else
                    {
                        reportPass("APT Yeargroup report looks Ok for  Completed Year assessment");
                    }
                    Thread.Sleep(1000);
                    click("APTClassesreportlink_Xpath");
                    test.Log(Status.Info, "Checking APT Classes report");
                    explicitWait("reportHeader_Xpath");

                    if (waitforElementPresent("AllClassLabel_Xpath").Equals(false))
                    {
                        reportFailure("APT reportTest has failed");
                        test.Log(Status.Info,
                            "APT reportTest has failed because APT Classes report table is not shown for Completed Year assessment");
                        takeScreenshot();

                    }
                    else
                    {
                        reportPass("APT Classes report looks Ok for Completed Year assessment");
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
                            "APT reportTest has failed because APT Pupilgroup report table is not shown for Completed Year assessment");
                        takeScreenshot();

                    }
                    else
                    {
                        reportPass("APT Pupil group report looks Ok for  Completed Year assessment");
                    }

                    Thread.Sleep(1000);
                    click("MySchoolgrouplink_Xpath");
                    test.Log(Status.Info, "Checking APT Schoolgroup report");
                    explicitWait("reportHeader_Xpath");

                    if (waitforElementPresent("SchoolGroupLabel_Xpath").Equals(false))
                    {
                        reportFailure("APT reportTest has failed");
                        test.Log(Status.Info,
                            "APT reportTest has failed because APT School group report table is not shown for Completed Year assessment");
                        takeScreenshot();

                    }
                    else
                    {
                        reportPass("APT School group report looks Ok forCompleted Year assessment");
                    }

                    Thread.Sleep(1000);
                    click("Pupilreportlink_Xpath");
                    test.Log(Status.Info, "Checking APT Pupils report");
                    explicitWait("PupilsLabel_Xpath");
                    if (waitforElementPresent("PupilsSubLabel_Xpath").Equals(false))
                    {
                        reportFailure("APT reportTest has failed");
                        test.Log(Status.Info,
                            "APT reportTest has failed because APT Pupils Report is not shown for Completed Year assessment");
                        takeScreenshot();

                    }
                    else
                    {
                        reportPass("APT Pupils report looks Ok for Completed Year assessment");
                    }
                    Thread.Sleep(1000);
                    if (iselementExist("ScatterplotLink_Xpath").Equals(true))
                    {
                        reportFailure("APT Scatterplot Report is shown for Bucket Years Assessment");

                    }

                     if(iselementExist("Pathwaylink_Xpath").Equals(true))
                   
                    {
                        reportFailure("APT Pathway Report is shown for Bucket Years Assessment");

                    }
                  
                                       
                        reportPass("Verified that Scatterplot and Pathway report has not shown for Bucket Years Assessment");

                    test.Log(Status.Info, "Verifying other links and Filters in the report");

                   if(iselementExist("ComparereportLink_Xpath").Equals(true))
                    {
                        reportFailure("APT Compare report Link is shown for Bucket Years Assessment Report");


                    }

                   else if (iselementExist("EnterassessmentLink_Xpath").Equals(true))
                    {
                        reportFailure("Enter assessmentLink is shown for Bucket Years Assessment Report");


                    }
                   else if(iselementExist("KSdropdown_Xpath").Equals(true))
                    {

                        reportFailure("KS drop down is shown for Bucket Years Assessment Report");

                    }

                    else if (iselementExist("Filters_Xpath").Equals(true))
                    {

                        reportFailure("Filters are shown in the APT Report for Completed Year Assessments");

                    }

                   else
                    {
                        reportPass("Verified that Compare,Enterpupil Assessment link as well as KS drop down and Filters are not shown in the Report");



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