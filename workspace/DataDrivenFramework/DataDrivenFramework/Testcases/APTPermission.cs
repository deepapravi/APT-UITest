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

    public class AFF_APTPermission : BaseTest
    {
        /* APT permission Test */
        static ExcelReaderFile xls = new ExcelReaderFile(ConfigurationManager.AppSettings["xlsPath"]);
        static string testCaseName = "APTPermissionTest";
        [Test, TestCaseSource("getData")]
        public void EnterDataPermissionCheck(Dictionary<string, string> data)
        {
            rep = ExtentManager.getInstance();
             if ((data["CreateAssessment"]).Equals("Y") && (data["EnterAssessment"]).Equals("Y"))
                test = rep.CreateTest("APTPermission Test-" + data["TestId"] + "", "This test will describe Permission check for user with both Create/Edit and Enter pupil data");
            else if ((data["CreateAssessment"]).Equals("Y") && (data["EnterAssessment"]).Equals("N"))
                test = rep.CreateTest("APTPermission Test-" + data["TestId"] + "", "This test will describe Permission check for user with Create/Edit but No Enter Pupil data");
            else  if ((data["CreateAssessment"]).Equals("N") && (data["EnterAssessment"]).Equals("Y"))
                    test = rep.CreateTest("APTPermission Test-" + data["TestId"] + "", "This test will describe Permission check for user with Enter data permission but No Create/Edit");
            else if (data["TestId"].Equals("4"))
                test = rep.CreateTest("APTPermission Test-" + data["TestId"] + "", "This test will describe Permission check for user with APT School level access only");
            else if (data["TestId"].Equals("5"))
                test = rep.CreateTest("APTPermission Test-" + data["TestId"] + "", "This test will describe Permission check for user with report access only ");
            else
                test = rep.CreateTest("APTPermission Test", "This test will APT Permission checks");
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
            bool actualResult = doLogin(username,password);
            if (actualResult.Equals(false))
            {

                reportFailure("Login has failed");
                test.Log(Status.Info, "Permission Test has failed because of invalid Login: " + data["Username"]);
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
                      if ((data["CreateAssessment"]).Equals("N") && (data["EnterAssessment"]).Equals("N"))
                    {
                     //   test = rep.CreateTest("APTPermission Test-" + data["TestId"] + "", "This test will describe Permission check for user with No permission to Create/Edit nor Enter data");
                        test.Log(Status.Info, "Testing the user : " + data["Username"] + " with No permission to Create and Enter Assessment");
                        if (isElementPresent("Span_Xpath").Equals(true))
                        {
                            test.Log(Status.Info, "Spanner is shown for user with no permission to Create and Edit: " + data["Username"]);
                            reportFailure("Admin spanner is shown for user with no permission to Create/Edit. Please check the permission settings for user!!!");


                        }
                         else
                        {
                            reportPass(" Admin spanner is not shown for user with No permission to Create and Enter pupil assessment Test Passed :-)");

                        }

                    }
                   
                  else  if ((data["CreateAssessment"]).Equals("Y") && (data["EnterAssessment"]).Equals("Y"))
                    {

                      //  test = rep.CreateTest("APTPermission Test-" + data["TestId"] + "", "This test will describe Permission check for user with both Create/Edit and Enter pupil data");
                        test.Log(Status.Info, "Testing the user : " + data["Username"] + " with Permission to Create and Enter Pupil Assessment");
                        click("Span_Xpath");
                        click("AssessmentTab_Xpath");
                        // click("CreateCTtab_Xpath");
                        Thread.Sleep(5000);

                        if (isElementPresent("CreateAssessmentTab_Xpath").Equals(false))
                        {
                            test.Log(Status.Info, "Create Assessment link is not shown for user: " + data["Username"]);
                            reportFailure("Create Assessment link is not shown in the spanner");


                        }
                        else
                        {

                            click("CreateAssessmentTab_Xpath");
                            Thread.Sleep(3000);
                            String EnterAssessmentButton_Xpath = "//button[contains(text(),'Create & enter pupil assessments')]";
                            if (iselementExist(EnterAssessmentButton_Xpath).Equals(false))
                            {

                                test.Log(Status.Info, "Enter pupil assessment button is not shown in the Create page or please check the setup status: " + data["Username"]);
                                reportFailure("Enter pupil assessment button is not shown in the Create Page or please check the APT setup status for the school");

                            }

                            else
                                reportPass(" Create Assessment link is shown in the spanner and Enter pupil assessment button is shown in the create page as well as in the spanner. Test Passed :-)");

                        }

                       

                    }
                    else if ((data["CreateAssessment"]).Equals("Y") && (data["EnterAssessment"]).Equals("N"))
                    {
                        
                     //   test = rep.CreateTest("APTPermission Test-" + data["TestId"] + "", "This test will describe Permission check for user with Create/Edit but No Enter Pupil data");
                        test.Log(Status.Info, "Testing the user : " + data["Username"] + " with Permission to Create but No Enter Pupil Assessment");
                        click("Span_Xpath");
                        click("AssessmentTab_Xpath");
                        Thread.Sleep(3000);
                        if (isElementPresent("CreateAssessmentTab_Xpath").Equals(false))
                        {
                            test.Log(Status.Info, "Create Assessment link is not shown for user: " + data["Username"]);
                            reportFailure("Create Assessment link is not shown in the spanner even with permission to Create. Please check the permission settings!!!");


                        }
                        else if((isElementPresent("EnterAssessmentTab_Xpath").Equals(true)))
                            {
                            test.Log(Status.Info, "Enter Assessment link is shown for user: " + data["Username"]);
                            reportFailure("Enter Assessment link is shown in the spanner even with no Permission to enter pupil data.Please check the permission settings!!!");


                            }
                        else
                        {
                            click("CreateAssessmentTab_Xpath");
                            Thread.Sleep(3000);
                            String EnterAssessmentButton_Xpath = "//button[contains(text(),'Create & enter pupil assessments')]";
                            if (iselementExist(EnterAssessmentButton_Xpath).Equals(true))
                            {

                                test.Log(Status.Info, "Enter pupil assessment button is shown in the Create page for user with No permission or please check the setup status: " + data["Username"]);
                                reportFailure("Enter pupil assessment button is shown in the Create Page for user with No permission or please check the APT setup status for the school");

                            }

                            else
                                reportPass(" Enter pupil assessment button is not shown in the create page as well as in the spanner. Test Passed :-)");
                        }


                    }
                    else if ((data["CreateAssessment"]).Equals("N") && (data["EnterAssessment"]).Equals("Y"))
                    {
                      //  test.Log(Status.Info, "Testing the user : " + data["Username"] + " with No Permission to Create/Edit but can Enter Pupil Assessment");
                        click("Span_Xpath");
                        click("AssessmentTab_Xpath");
                        Thread.Sleep(3000);
                        if (isElementPresent("CreateAssessmentTab_Xpath").Equals(true))
                        {
                            test.Log(Status.Info, "Create Assessment link is  shown for user: " + data["Username"]);
                            reportFailure("Create Assessment link is  shown in the spanner even with no permission to Create");


                        }

                        else if ((isElementPresent("EnterAssessmentTab_Xpath").Equals(false)))
                        {
                            test.Log(Status.Info, "Enter Assessment link is not shown for user: " + data["Username"]);
                            reportFailure("Enter Assessment link is not shown in the spanner even with Permission");


                        }

                        else
                        {

                            click("EnterAssessmentTab_Xpath");
                            explicitWait("EnterAssessmentLabel_Xpath");
                           

                                if ((isElementPresent("EnterpageEditAssessmentLink_Xpath").Equals(true)))
                            {
                                test.Log(Status.Info, "Edit Assessment link is shown in the Enter pupil assessment page for user: " + data["Username"]);
                                reportFailure("Edit Assessment link is shown in the Enter pupil assessment page");

                            }

                              else if ((isElementPresent("DuplicateassessmentLink_Xpath").Equals(true)))
                            {
                                test.Log(Status.Info, "Duplicate assessment link is shown in the Enter pupil assessment page for user: " + data["Username"]);
                                reportFailure("Duplicate assessment link is shown in the Enter pupil assessment page");

                            }
                        }

                        reportPass(" Test is passed with Checks // Enter pupil assesment link is shown in the spanner but no Create//No Edit and Duplicate assessment link is shown in the Enter pupil assessment page .Test Passed :-)");
                    }

                   



                }

                catch(Exception ex)
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