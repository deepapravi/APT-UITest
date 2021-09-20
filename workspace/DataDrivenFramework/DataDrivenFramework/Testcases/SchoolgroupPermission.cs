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
 
    public class AGG_SchoolgroupPermission : BaseTest
    {
        static ExcelReaderFile xls = new ExcelReaderFile(ConfigurationManager.AppSettings["xlsPath"]);
        static string testCaseName = "SchoolgroupPermissionTest";

        [Test, TestCaseSource("getData")]
        public void SchoolgroupPermissionCheck(Dictionary<string, string> data)
        {
            rep = ExtentManager.getInstance();


            if (data["TestId"].Equals("1"))
                test = rep.CreateTest("School group Permission Test-" + data["TestId"] + "", "This test will describe Permission check for user with both Create/Edit and Pupil level access");
            else if (data["TestId"].Equals("2"))
                test = rep.CreateTest("School group Permission Test-" + data["TestId"] + "", "This test will describe Permission check for user with  Create/Edit and No pupil level access");
            //else if (data["TestId"].Equals("3"))
            //    test = rep.CreateTest("School group Permission Test-" + data["TestId"] + "", "This test will describe Permission check for user with  No Create/Edit but No Pupil level access");
            //else if (data["TestId"].Equals("4"))
            //    test = rep.CreateTest("School group Permission Test-" + data["TestId"] + "", "This test will describe Permission check for user with No  Create/Edit but  Pupil level access ");
            
            else
                test = rep.CreateTest("School group Permission Test", "This test will School group Permission checks");



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
                    if ((data["CreateAssessment"]).Equals("Y") && (data["APTPupilLevel"]).Equals("Y"))
                    {
                        //test = rep.CreateTest("School group Permission Test-" + data["TestId"] + "",
                        //    "This test will describe Permission check for user with permission to Create/Edit and Pupil level access");

                        click("Span_Xpath");
                        Thread.Sleep(1000);
                        click("AssessmentTab_Xpath");
                        Thread.Sleep(2000);
                       // IWebElement element = driver.FindElement(By.XPath("//a[contains(text(),'Create & edit my school groups')]"));
                        String Createschoolgroup_Xpath = "//a[contains(text(),'Create & edit my school groups')]";
                        if (iselementExist(Createschoolgroup_Xpath).Equals(false))
                        {
                            test.Log(Status.Info, "Create School group link is not shown for user: " + data["Username"]);
                            reportFailure("Create School group link is not shown in the spanner even with permission to Create. Please check the permission settings!!!");

                        }

                        else if(isElementPresent("AddpupilToSchoolgroupLink_Xpath").Equals(false))

                        {

                            test.Log(Status.Info, "Add pupil to School group link is not shown for user: " + data["Username"]);
                            reportFailure("Add pupil to School group not shown in the spanner even with pupil level permission. Please check the permission settings!!!");

                        }
                        else
                            reportPass(" Create /Edit School group and Add pupil to School group link is shown in the spanner. Test Passed :-)");

                    }

                    else if ((data["CreateAssessment"]).Equals("N") && (data["APTPupilLevel"]).Equals("N"))
                    {
                       // test = rep.CreateTest("School group Permission Test-" + data["TestId"] + "",
                        //    "This test will describe Permission check for user with No permission to Create/Edit and No Pupil level access");
                        if (isElementPresent("Span_Xpath").Equals(true))
                        {
                            test.Log(Status.Info, "Spanner is shown for user with no permission to Create and Edit: " + data["Username"]);
                            reportFailure("Admin spanner is shown for user with no permission to Create/Edit. Please check the permission settings for user!!!");


                        }
                        else
                        {
                            reportPass(" Admin spanner is not shown for user with No permission to Create/Edit Test Passed :-)");

                        }

                    }

                    else if ((data["CreateAssessment"]).Equals("Y") && (data["APTPupilLevel"]).Equals("N"))
                    {
                        //test = rep.CreateTest("School group Permission Test-" + data["TestId"] + "",
                          //  "This test will describe Permission check for user with permission to Create/Edit and No Pupil level access");

                        click("Span_Xpath");
                        Thread.Sleep(1000);
                        click("AssessmentTab_Xpath");
                        Thread.Sleep(2000);

                        String Createschoolgroup_Xpath = "//a[contains(text(),'Create & edit my school groups')]";
                        if (iselementExist(Createschoolgroup_Xpath).Equals(false))
                        {
                            test.Log(Status.Info, "Create School group link is not shown for user: " + data["Username"]);
                            reportFailure("Create School group link is not shown in the spanner even with permission to Create. Please check the permission settings!!!");

                        }

                        
                            else if (isElementPresent("AddpupilToSchoolgroupLink_Xpath").Equals(true))

                        {

                            test.Log(Status.Info, "Add pupil to School group link is  shown for user: " + data["Username"]);
                            reportFailure("Add pupil to School group  shown in the spanner even with No pupil level permission. Please check the permission settings!!!");

                        }
                        else
                        {

                            Thread.Sleep(1000);
                            IWebElement element = driver.FindElement(By.XPath("//a[contains(text(),'Create & edit my school groups')]"));
                            element.Click();
                            Thread.Sleep(3000);
                            explicitWait("MySchoolgroupLabel_Xpath");
                            if (isElementPresent(("AddpupilToSchoolgroupTab_Xpath")).Equals(true))
                            {

                                test.Log(Status.Info, "Add pupil to School group tab is  shown for user: " + data["Username"]);
                                reportFailure("Add pupil to School group tab  even with No pupil level permission. Please check the permission settings!!!");
                            }

                        }

                        
                            reportPass(" Create /Edit School group is shown and Add pupil to School group link is not shown in the spanner as well as in the My School group Page tabs . Test Passed :-)");


                    }




                    //else if ((data["CreateAssessment"]).Equals("N") && (data["APTPupilLevel"]).Equals("Y"))
                    //{
                    //   // test = rep.CreateTest("School group Permission Test-" + data["TestId"] + "",
                    //    //    "This test will describe Permission check for user with No Create/Edit but Pupil level access");

                    //    click("Span_Xpath");
                    //    Thread.Sleep(1000);
                    //    click("AssessmentTab_Xpath");
                    //    Thread.Sleep(2000);

                    //    String Createschoolgroup_Xpath = "//a[contains(text(),'Create & edit my school groups')]";
                    //    if (iselementExist(Createschoolgroup_Xpath).Equals(true))
                    //    {
                    //        test.Log(Status.Info, "Create School group link is shown for user: " + data["Username"]);
                    //        reportFailure("Create School group is shown in the spanner even with No permission to Create. Please check the permission settings!!!");

                    //    }


                    //    else if (isElementPresent("AddpupilToSchoolgroupLink_Xpath").Equals(false))

                    //    {

                    //        test.Log(Status.Info, "Add pupil to School group link is not  shown for user: " + data["Username"]);
                    //        reportFailure("Add pupil to School group is not shown in the spanner even with pupil level permission. Please check the permission settings!!!");

                    //    }
                    //    else
                    //        reportPass(" Create /Edit School group is not shown and Add pupil to School group link is shown . Test Passed :-)");


                    //}




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