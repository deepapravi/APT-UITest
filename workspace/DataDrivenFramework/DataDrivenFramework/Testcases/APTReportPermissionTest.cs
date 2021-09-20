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

    public class APTReportPermissionChecks : BaseTest
    {
        static ExcelReaderFile xls = new ExcelReaderFile(ConfigurationManager.AppSettings["xlsPath"]);
        static string testCaseName = "APTReportPermissionTest";
        [Test, TestCaseSource("getData")]
        public void APTReportPermissionCheck(Dictionary<string, string> data)
        {
            rep = ExtentManager.getInstance();


            //if (data["TestId"].Equals("1"))
            //    test = rep.CreateTest("APTReportPermission Test-" + data["TestId"] + "", "This test will describe APT Report Permission user with pupil level access only");
            //else if (data["TestId"].Equals("2"))
            //    test = rep.CreateTest("APTReportPermission Test-" + data["TestId"] + "", "This test will describe APT Report Permission user with School level access but no pupil level access");

            //else
            //    test = rep.CreateTest("APT Report Permission Test", "This test will APT Report Permission checks");

            if ((data["PupilLevel"]).Equals("Y") && (data["SchoolLevel"]).Equals("N"))
            {
                test = rep.CreateTest("APTReportPermission Test-" + data["TestId"] + "", "This test will describe APT Report Permission user with pupil level access only");

            }

            else if ((data["PupilLevel"]).Equals("N") && (data["SchoolLevel"]).Equals("Y"))
            {

                test = rep.CreateTest("APTReportPermission Test-" + data["TestId"] + "", "This test will describe APT Report Permission user with School level access but no pupil level access");

            }

            else
            {
                test = rep.CreateTest("APT Report Permission Test", "This test will APT Report Permission checks");


            }

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
                try
                {
                    if ((data["PupilLevel"]).Equals("Y") && (data["SchoolLevel"]).Equals("N"))
                    {
                        hoverElement("PupiltrackingMenu_Xpath");
                        Thread.Sleep(1000);
                        click("firstreportmenu_Xpath");
                        explicitWait("PupilsLabel_Xpath");
                        if (waitforElementPresent("PupilsSubLabel_Xpath").Equals(false))
                        {
                            reportFailure("APT report PermissionTest has failed");
                            test.Log(Status.Info,
                                "APT PermissionreportTest has failed because APT Pupils Report is not shown");
                            
                            takeScreenshot();

                        }

                        reportPass("APT Pupils report is shown");

                        click("ScatterplotLink_Xpath");
                            test.Log(Status.Info, "Checking APT Scatterplot report");
                          
                            if (waitforElementPresent("scatterplotsection_Xpath").Equals(false))
                            {
                                reportFailure("APT reportTest has failed");
                            test.Log(Status.Info,
                                "APT Permission reportTest has failed because APT Scatter plot Report is not shown");
                                   
                                takeScreenshot();

                            }
                        reportPass("APT Scatterplot report is shown ");

                        Thread.Sleep(1000);
                        click("Pathwaylink_Xpath");
                        test.Log(Status.Info, "Checking APT Pathway report");
                        explicitWait("Pathwayover_Xpath");
                        Thread.Sleep(3000);
                        if (waitforElementPresent("PathwayAllpupil_Xpath").Equals(true))
                        {
                            reportFailure("APT reportTest has failed");
                            test.Log(Status.Info,
                                "APT reportTest has failed because APT Pathway report All Pupil drop down is  shown");
                           
                            takeScreenshot();

                        }
                        reportPass("APT Pathway report is shown but not shown All Pupils in the drop down ");
                        Thread.Sleep(1000);
                        click("Summaryreportlink_Xpath");
                        test.Log(Status.Info, "Checking APT Summary report");
                        Thread.Sleep(2000);
                        if (waitforElementPresent("Noaccesslink_Xpath").Equals(false))
                        {
                            reportFailure("APT reportTest has failed");
                            test.Log(Status.Info,
                                "APT reportTest has failed because APT Summary report has  shown");

                            takeScreenshot();

                        }
                        reportPass("Summary report gives No access ");
                    }

                    if ((data["PupilLevel"]).Equals("N") && (data["SchoolLevel"]).Equals("Y"))

                    {

                        hoverElement("PupiltrackingMenu_Xpath");
                        Thread.Sleep(1000);
                        click("firstreportmenu_Xpath");
                        explicitWait("reportHeader_Xpath");

                        test.Log(Status.Info, "Checking the Summary report");
                        if (waitforElementPresent("Summarydatatable_Xpath").Equals(false))
                        {
                            reportFailure("APT reportTest has failed");
                            test.Log(Status.Info,
                                "APT reportTest has failed because the Summary report table is not shown for"
                               );
                            takeScreenshot();


                        }


                        reportPass("APT Summary report is shown ");
                        Thread.Sleep(1000);
                        click("APTyeargrouplink_Xpath");
                        test.Log(Status.Info, "Checking APT Yeargroup report");
                        explicitWait("reportHeader_Xpath");

                        if (waitforElementPresent("AllYearLabel_Xpath").Equals(false))
                        {
                            reportFailure("APT reportTest has failed");
                            test.Log(Status.Info,
                                "APT reportTest has failed because APT Year group report table is not shown"
                              );
                            takeScreenshot();




                        }

                        reportPass("APT  Year group report is shown ");

                        Thread.Sleep(1000);
                        click("APTClassesreportlink_Xpath");
                        test.Log(Status.Info, "Checking APT Classes report");
                        explicitWait("reportHeader_Xpath");

                        if (waitforElementPresent("AllClassLabel_Xpath").Equals(false))
                        {
                            reportFailure("APT reportTest has failed");
                            test.Log(Status.Info,
                                "APT reportTest has failed because APT Classes report table is not shown"
                              );
                            takeScreenshot();

                        }
                        reportPass("APT  Classes report is shown ");

                        Thread.Sleep(1000);

                        click("APTpupilgrouplink_Xpath");
                        test.Log(Status.Info, "Checking APT Pupilgroup report");
                        explicitWait("reportHeader_Xpath");

                        if (waitforElementPresent("AllpupilLabel1_Xpath").Equals(false) &
                               waitforElementPresent("AllpupilLabel2_Xpath").Equals(false))
                        {
                            reportFailure("APT reportTest has failed");
                            test.Log(Status.Info,
                                "APT reportTest has failed because APT Pupilgroup report table is not shown"
                               );
                            takeScreenshot();

                        }

                        reportPass("APT  Pupilgroup report is shown ");

                        Thread.Sleep(1000);
                        click("MySchoolgrouplink_Xpath");
                        test.Log(Status.Info, "Checking APT Schoolgroup report");
                        explicitWait("reportHeader_Xpath");

                        if (waitforElementPresent("SchoolGroupLabel_Xpath").Equals(false))
                        {
                            reportFailure("APT reportTest has failed");
                            test.Log(Status.Info,
                                "APT reportTest has failed because APT School group report table is not shown"
                                );
                            takeScreenshot();

                        }
                        reportPass("APT  School group report is shown ");


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
                        Thread.Sleep(2000);
                        if (waitforElementPresent("Noaccesslink_Xpath").Equals(false))
                        {
                            reportFailure("APT reportTest has failed");
                            test.Log(Status.Info,
                                "APT reportTest has failed because APT Summary report has  shown");

                            takeScreenshot();

                        }
                        reportPass("APT Pupils report gives No access");
                    }

                        reportPass("APT Report Permission Test Passed");

                }

                catch (Exception e)
                {

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