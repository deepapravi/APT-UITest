using AventStack.ExtentReports;
using AventStack.ExtentReports.Utils;
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
  
    public class AE_CTPermission:BaseTest
    {
        static ExcelReaderFile xls = new ExcelReaderFile(ConfigurationManager.AppSettings["xlsPath"]);
        static string testCaseName = "CTPermissionTest";
        [Test, TestCaseSource("getData")]
        public void CTPermission(Dictionary<string, string> data)
        {
            rep = ExtentManager.getInstance();
            if ((data["CreateAssessment"]).Equals("N") && (data["EnterAssessment"]).Equals("N") &&
                        (data["APTSchoolLevelOnly"]).Equals("N") && (data["APTPupilLevelOnly"]).Equals("Y"))
                test = rep.CreateTest("CTPermission Test-" + data["TestId"] + "", "This test will describe Permission check for user with APT Pupil level access only ");

            else if ((data["CreateAssessment"]).Equals("N") && (data["EnterAssessment"]).Equals("N") &&
                        (data["APTSchoolLevelOnly"]).Equals("Y") && (data["APTPupilLevelOnly"]).Equals("N"))

                test = rep.CreateTest("CTPermission Test-" + data["TestId"] + "", "This test will describe Permission check for user with APT School level access only");

             else if ((data["CreateAssessment"]).Equals("Y") && (data["EnterAssessment"]).Equals("Y"))

                test = rep.CreateTest("CTPermission Test-" + data["TestId"] + "", "This test will describe Permission check for user with both Create/Edit and Enter pupil data");

            else if ((data["CreateAssessment"]).Equals("Y") && (data["EnterAssessment"]).Equals("N"))
                test = rep.CreateTest("CTPermission Test-" + data["TestId"] + "", "This test will describe Permission check for user with Create/Edit but No Enter Pupil data");

            else if ((data["CreateAssessment"]).Equals("N") && (data["EnterAssessment"]).Equals("Y"))

                test = rep.CreateTest("CTPermission Test-" + data["TestId"] + "","This test will describe Permission check for user with Enter data permission but No Create/Edit");

            else
               test = rep.CreateTest("CTPermission Test", "This test will CT Permission checks");
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
                try
                {


                    if ((data["CreateAssessment"]).Equals("N") && (data["EnterAssessment"]).Equals("N") && 
                        (data["APTSchoolLevelOnly"]).Equals("N") && (data["APTPupilLevelOnly"]).Equals("Y"))
                    {
                        //test = rep.CreateTest("CTPermission Test-" + data["TestId"] + "", "This test will describe Permission check for user with APT Pupil level access only ");

                        test.Log(Status.Info,
                            "Testing the user : " + data["Username"] +
                            " with No permisssion to Create and Enter Assessment but have only APT Pupil level access ");
                        if (isElementPresent("Span_Xpath").Equals(true))
                        {
                            test.Log(Status.Info,
                                "Spanner is shown for user with no permission to Create and Edit: " + data["Username"]);
                            reportFailure(
                                "Admin spanner is shown for user with no permission to Create/Edit. Please check the permission settings for user!!!");


                        }
                        else
                        {
                            hoverElement("PupiltrackingMenu_Xpath");
                            click("PupiltrackingCTMenu_Xpath");

                            Thread.Sleep(3000);
                           explicitWait("ViewCTLabel_Xpath");

                            if (isElementPresent("ViewCTLabel_Xpath").Equals(false))
                            {
                                test.Log(Status.Info, "View CT Page is not shown for user: " + data["Username"]);
                                reportFailure("View CT Page is not shown for user");

                            }
                            else if (isElementPresent("ViewandenterCTLink_Xpath").Equals(true))
                            {

                                click("ViewandenterCTLink_Xpath");
                                Thread.Sleep(3000);
                                if (isElementPresent("CTNoeditLabel_Xpath").Equals(false))
                                {
                                    test.Log(Status.Info, "The label cant edit pupil assessment is not shown for user: " + data["Username"]);
                                    reportFailure("The label cant edit pupil assessment is not shown for user");

                                }
                               else if (isElementPresent("CTshare_Xpath").Equals(true))
                                {
                                    test.Log(Status.Info, "CT share button is  shown for user: " + data["Username"]);
                                    reportFailure("CT share button is  shown for user");

                                }
                                else if (isElementPresent("CTAllPupilLabel_Xpath").Equals(false))

                                {

                                    test.Log(Status.Info, "Can't see the Pupil's list in the CT page for user: " + data["Username"]);
                                    reportFailure("Can't see the Pupil's list in the CT page");

                                }
                            }
                            else
                            {
                                reportPass("Spanner is not shown //can't check the CT page as there is no assessment exist Test passes :-)");
                            }

                        }
                        reportPass("Spanner is not shown // can see the enter CT page but can't share or  enter pupil data but can see pupils and their data).   . Test passes :-)");

                    }

                    //Checking second condition
                    //-------------------------------------------------------------------------------------------



                    if ((data["CreateAssessment"]).Equals("N") && (data["EnterAssessment"]).Equals("N") &&
                        (data["APTSchoolLevelOnly"]).Equals("Y") && (data["APTPupilLevelOnly"]).Equals("N"))
                    {
                        //test = rep.CreateTest("CTPermission Test-" + data["TestId"] + "", "This test will describe Permission check for user with APT School level access only");

                        {
                            test.Log(Status.Info,
                                "Testing the user : " + data["Username"] +
                                " with No permisssion to Create and Enter Assessment but have only APT School level access ");
                            if (isElementPresent("Span_Xpath").Equals(true))
                            {
                                test.Log(Status.Info,
                                    "Spanner is shown for user with no permission to Create and Edit: " + data["Username"]);
                                reportFailure(
                                    "Admin spanner is shown for user with no permission to Create/Edit. Please check the permission settings for user!!!");


                            }
                            else
                            {
                                hoverElement("PupiltrackingMenu_Xpath");
                                click("PupiltrackingCTMenu_Xpath");

                                Thread.Sleep(3000);
                                explicitWait("ViewCTLabel_Xpath");

                                if (isElementPresent("ViewCTLabel_Xpath").Equals(false))
                                {
                                    test.Log(Status.Info, "View CT Page is not shown for user: " + data["Username"]);
                                    reportFailure("View CT Page is not shown for user");

                                }
                                else if (isElementPresent("ViewandenterCTLink_Xpath").Equals(true))
                                {

                                    click("ViewandenterCTLink_Xpath");
                                  
                                    Thread.Sleep(6000);

                                    if(isElementPresent(("NoaccessToAPTLabel_Xpath")).Equals(false))
                                    {

                                        test.Log(Status.Info, "APT No access is not shown for School level access only  user: " + data["Username"]);
                                        reportFailure("APT No access page is not shown when try to access CT enter page");


                                    }
                            
                                }
                                else
                                {
                                    reportPass("Spanner is not shown //can't check the CT page as there is no assessment exist Test passes :-)");
                                }

                            }
                            reportPass("Spanner is not shown // can see CT view page and gives APT No access message when try to access enter CT page . Test passes :-)");

                        }


                        
                    }


                    //Checking third condition
                    //================================================================

                    else if ((data["CreateAssessment"]).Equals("Y") && (data["EnterAssessment"]).Equals("Y"))
                    {
                      //  test = rep.CreateTest("CTPermission Test-" + data["TestId"] + "", "This test will describe Permission check for user with both Create/Edit and Enter pupil data");
                        test.Log(Status.Info, "Testing the user : " + data["Username"] + " with Permission to Create and Enter Pupil Assessment");
                        click("Span_Xpath");
                        click("CurriculumTab_Xpath");
                        Thread.Sleep(5000);

                        if (isElementPresent("CreateCTtab_Xpath").Equals(false))
                        {
                            test.Log(Status.Info, "Create CT Assessment link is not shown for user: " + data["Username"]);
                            reportFailure("Create CT Assessment link is not shown in the spanner");


                        }
                        else if (isElementPresent("EditCTtab_Xpath").Equals(false))
                        {
                            test.Log(Status.Info, "Enter CT Assessment link is not shown for user: " + data["Username"]);
                            reportFailure("Enter CT Assessment link is not shown in the spanner");


                        }
                        else
                        {
                            click("EditCTtab_Xpath");
                            
                            explicitWait("EditCTLabel_Xpath");
                            if (isElementPresent("ViewandenterCTLink_Xpath").Equals(true))
                            {
                                explicitWait("ViewandenterCTLink_Xpath");
                                Thread.Sleep(2000);
                                click("ViewandenterCTLink_Xpath");
                                Thread.Sleep(4000);
                                if (isElementPresent("CTshare_Xpath").Equals(false))
                                {
                                    test.Log(Status.Info, "CT share button is not shown for user: " + data["Username"]);
                                    reportFailure("CT share button is not shown for user");

                                }
                                else
                                {

                                    reportPass("Create and Edit CT is shown in the spanner also CT share button is shown in the Enter Assessment page  . Test passes :-)");
                                }
                            }
                            else

                            {

                                reportPass("Create and Edit CT is shown in the spanner and cannot check the enter CT page as there is no CT assessment exist . Test passes :-)");
                            }

                        }
                    }

                    //Checking Fourth condition
                    //=======================================================

                    else if ((data["CreateAssessment"]).Equals("Y") && (data["EnterAssessment"]).Equals("N"))
                    {
                       // test = rep.CreateTest("CTPermission Test-" + data["TestId"] + "", "This test will describe Permission check for user with Create/Edit but No Enter Pupil data");
                        test.Log(Status.Info, "Testing the user : " + data["Username"] + " with Permission to Create/Edit and No Enter Pupil Assessment");
                        click("Span_Xpath");
                        click("CurriculumTab_Xpath");
                        if (isElementPresent("CreateCTtab_Xpath").Equals(false))
                        {
                            test.Log(Status.Info, "Create CT Assessment link is not shown for user: " + data["Username"]);
                            reportFailure("Create CT Assessment link is not shown in the spanner");


                        }
                        else if (isElementPresent("EditCTtab_Xpath").Equals(true))
                        {
                            test.Log(Status.Info, "Enter CT Assessment link is shown for user: " + data["Username"]);
                            reportFailure("Enter CT Assessment link is shown in the spanner");


                        }
                        else
                        {
                            click("CreateCTtab_Xpath");
                           
                            explicitWait("CreateCTLabel_Xpath");
                            click("ViewCTTab_Xpath");
                            explicitWait("ViewCTLabel_Xpath");

                            
                            if (isElementPresent("ViewandenterCTLink_Xpath").Equals(true))
                            {
                                explicitWait("ViewandenterCTLink_Xpath");
                                Thread.Sleep(2000);
                                click("ViewandenterCTLink_Xpath");
                                Thread.Sleep(4000);
                                if (isElementPresent("CTshare_Xpath").Equals(true))
                                {
                                    test.Log(Status.Info, "CT share button is shown for user: " + data["Username"]);
                                    reportFailure("CT share button is shown for user without having enter data permission");

                                }
                                else if (isElementPresent("CTNoeditLabel_Xpath").Equals(false))
                                {

                                    test.Log(Status.Info, "The label cant edit pupil assessment is not shown for user: " + data["Username"]);
                                    reportFailure("The label cant edit pupil assessment is not shown for user");
                                }
                                else
                                {
                                    reportPass("Test is passed with checks//1. CT Create link is shown in the spanner and No Enter link//2. CT share button is not shown in the enter assessment page// 3. and can't edit pupil assessments . Test Passed :-) ");

                                }

                            }
                            else
                            {
                                reportPass("Test is passed with checks//CT Create link is shown in the spanner and No Enter link and cannot check the enter CT page as there is no assessment exist . Test Passed :-) ");

                            }
                        }
                    }

                    //Checking fifth condition
                    //=============================================================

                    else if ((data["CreateAssessment"]).Equals("N") && (data["EnterAssessment"]).Equals("Y"))
                    {
                       // test = rep.CreateTest("CTPermission Test-" + data["TestId"] + "",
                        //    "This test will describe Permission check for user with Enter data permission but No Create/Edit");
                        test.Log(Status.Info, "Testing the user : " + data["Username"] + " with  Enter Pupil Assessment permission but No Create/Edit");
                        click("Span_Xpath");
                        click("CurriculumTab_Xpath");
                        if (isElementPresent("CreateCTtab_Xpath").Equals(true))
                        {
                            test.Log(Status.Info, "Create CT Assessment link is shown for user: " + data["Username"]);
                            reportFailure("Create CT Assessment link is  shown in the spanner");


                        }
                        else if (isElementPresent("EditCTtab_Xpath").Equals(false))
                        {
                            test.Log(Status.Info, "Enter CT Assessment link is not shown for user: " + data["Username"]);
                            reportFailure("Enter CT Assessment link is not shown in the spanner");


                        }


                        else
                        {
                            click("EditCTtab_Xpath");
                            Thread.Sleep(3000);
                            explicitWait("EditCTLabel_Xpath");

                            if (isElementPresent("ViewandenterCTLink_Xpath").Equals(true))
                            {
                                explicitWait("ViewandenterCTLink_Xpath");
                                click("ViewandenterCTLink_Xpath");
                                Thread.Sleep(3000);
                                if (isElementPresent("CTshare_Xpath").Equals(false))
                                {
                                    test.Log(Status.Info, "CT share button is not shown for user: " + data["Username"]);
                                    reportFailure("CT share button is shown not for user with enter data permission");
                                }
                                else
                                {
                                    reportPass("Enter CT link is  shown in the spanner but No create and also CT share button is shown in the enter assessment page. Test passes :-)");

                                }
                            }
                            else
                            {
                                reportPass("Enter CT link is n shown in the spanner but No create and cannot test the enter CT page as there is no assessment exist. Test passes :-)");

                            }
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
