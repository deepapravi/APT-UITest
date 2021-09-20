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
 
    public class AC_APTCreateCT : BaseTest
    {
        static ExcelReaderFile xls = new ExcelReaderFile(ConfigurationManager.AppSettings["xlsPath"]);
        static string testCaseName = "CreateCurriculum&EnterTA";
        [Test, TestCaseSource("getData")]
        public void APTCreateCT(Dictionary<string, string> data)
        {
            rep = ExtentManager.getInstance();
            test = rep.CreateTest("CreateCurriculum&EnterTA Test-" + data["TestId"] + "", "This test will describe CreateCTAssessment and enter TA functionality");
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
                    click("Span_Xpath");
                    Thread.Sleep(1000);
                    click("CurriculumTab_Xpath");
                    Thread.Sleep(1000);
                    click("CreateCTtab_Xpath");
                    explicitWait("CreateCTLabel_Xpath");
                    if (isElementPresent("CreateCTLabel_Xpath").Equals(false))
                    {
                        test.Log(Status.Info, "Create CT page is not shown for user: " + data["Username"]);
                        reportFailure("Create CT page is not shown");

                    }
                  

                    type("CTAssessmentName_Id", data["CurriculumName"]);
                    
                    explicitWait("CreateCTLabel_Xpath");
                    Thread.Sleep(4000);
                    click("CreateCTLabel_Xpath");
                    Thread.Sleep(1000);

                    if (isElementPresent("CTValidationText_Xpath").Equals(true))
                    {
                        test.Log(Status.Info, "Assessment already exist: " + data["CurriculumName"]);
                        reportFailure("Assessment already exist!!!");


                    }



                    if (data.ContainsKey("Description").Equals(true))
                    {
                        type("CTAssessmentDesc_Id", data["Description"]);
                    }
                    else
                    {
                        click("CTAssessmentDesc_Id");
                        click("CreateCTLabel_Xpath");
                        Thread.Sleep(1000);
                        if (isElementPresent("CTDescriptionValidation_Xpath").Equals(true))
                        {
                            test.Log(Status.Info, "Description is not given for Assessment for  : " + data["Username"]);
                            reportFailure("Cannot create the Assessment as the description field is blank!!!");

                        }
                    }

                    click("CTSubjectgroupDropdown_Xpath");
                    string subject_Xath = "//label[contains(text()," + "'" + data["Subjects"] + "'" + ")]";
                    IWebElement Subjectdropdown = driver.FindElement(By.XPath(subject_Xath));
                    Thread.Sleep(1000);
                    Subjectdropdown.Click();
                    test.Log(Status.Info, "Selected the Subject : " + data["Subjects"]);

                    PerformActionClick("CreateCTLabel_Xpath");
                    Thread.Sleep(1000);
                    click("CTYeargroupDropdown_Xpath");
                    Thread.Sleep(1000);

                    string Year_Xpath = "//label[contains(text()," + "'" + data["NcYear"] +
                                        "'" + ")]";

                    //if (data["NcYear"].Equals("5") || data["NcYear"].Equals("6"))
                    //{
                    //    IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                    //    IWebElement Yeardropdown = driver.FindElement(By.XPath(Year_Xpath));
                    //    js.ExecuteScript("arguments[0].scrollIntoView(true);", Yeardropdown);
                    //    Yeardropdown.Click();
                    //}
                    //else
                    //{
                        IWebElement Yeardropdown1 = driver.FindElement(By.XPath(Year_Xpath));
                        Yeardropdown1.Click();
                    //}


                    test.Log(Status.Info, "Selected the Year : " + data["NcYear"]);
                    PerformActionClick("CTYeargroupLabel_Xpath");
                    Thread.Sleep(1000);
                    scrolldown("CTTemplate_Xpath");
                    Thread.Sleep(1000);
                 //   click("CTTemplate_Xpath");
                    IWebElement CTTemplate = getElement("CTTemplate_Xpath");
                    SelectElement s = new SelectElement(CTTemplate);
                    Thread.Sleep(1000);
                    s.SelectByText(data["Template"]);

                    //Thread.Sleep(1000);
                    //String Template_Xpath = "//label[contains(text(), " + "'" + data["Template"] + "'" + ")]";
                    //IWebElement Template = driver.FindElement(By.XPath(Template_Xpath));
                    //Template.Click();
                    test.Log(Status.Info, "Selected the Template : " + data["Template"]);
                    Thread.Sleep(4000);
                    click("CreateCTButton_Xpath");
                  
                    explicitWait("EditCTLabel_Xpath");

                    string Assessment_Xpath = "//h3[contains(text(), " + "'" + data["CurriculumName"] + "'" + ")]";
                    Thread.Sleep(2000);
                        IJavaScriptExecutor js1 = (IJavaScriptExecutor)driver;
                        js1.ExecuteScript("window.scrollBy(0,-500)");
                    if (IsElementVisible("EditCTLabel_Xpath").Equals(true) &
                        iselementExist(Assessment_Xpath).Equals(true))
                    {

                        test.Log(Status.Info, "Assessment Created: " + data["CurriculumName"]);
                        reportPass("Assessment Created successfully");

                    }
                    else
                    {

                        test.Log(Status.Info, "Assessment Not Created: " + data["CurriculumName"]);
                        reportFailure("Assessment not Created");

                    }

                    //IWebElement search = getElement("CTSearchbtn_Xpath");
                    //js1.ExecuteScript("arguments[0].scrollIntoView(true);", search);
                    Thread.Sleep(4000);
                    explicitWaitUntilClickable("SearchCtTxt_Xpath");
                    IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                    js.ExecuteScript("window.scrollBy(0,-300)");
                    type("SearchCtTxt_Xpath", data["CurriculumName"]);
                    Thread.Sleep(1000);
                    explicitWaitUntilClickable("CTSearchbtn_Xpath");

                    
                    click("CTSearchbtn_Xpath");
                   
                   
                   // click("CTSearchbtn_Xpath");
                    if (iselementExist(Assessment_Xpath).Equals(false))
                    {

                        reportFailure("Test has failed because the given CT assessment does not exist");
                        test.Log(Status.Info,
                            "Test has failed because the given CT assessment does not exist: " +
                            data["CurriculumName"]);


                    }
                    else
                    {
                        Thread.Sleep(3000);
                        click("ViewandenterCTLink_Xpath");
                        explicitWait("CTAllPupilLabel_Xpath");

                        // IWebElement TAdropdown = getElement("CTenterTA_Xpath");
                        IJavaScriptExecutor js2 = (IJavaScriptExecutor)driver;
                        js2.ExecuteScript("window.scrollBy(0,300)");


                        // scrolldown("CTenterTA_Xpath");

                        Thread.Sleep(1000);


                        //if (data["TA"] != "")
                        //{
                            click("CTenterTA_Xpath");
                            Thread.Sleep(1000);
                            click("TA_Xpath");
                            //string TA_Xpath = "//span[contains(text()," + "'" + data["TA"] + "'" + ")]";
                            //IWebElement TA = driver.FindElement(By.XPath(TA_Xpath));

                            //TA.Click();
                        //}

                        Thread.Sleep(2000);

                        click("EnterCTSave_Xpath");
                        Thread.Sleep(2000);
                        //  explicitWait("AssessmentSavedLabel_Xpath");
                        if (CheckforElement("UnsavedChangesLabel_Xpath").Equals(true))
                        {

                            test.Log(Status.Info,
                                "Assessment Created but not Saved the data -" + data["AssessmentName"]);
                            reportFailure("CT Assessment Created but not entered data");

                        }

                        //click("CTBacktoListlink_Xpath");

                        //explicitWait("EditCTLabel_Xpath");

                        //string pupilcount = getText("CTPupilcount_Xpath");

                        //if (pupilcount.Equals(0))
                        //    {

                        //    test.Log(Status.Info,
                        //       "Pupil with assessment has not been updated for  -" + data["AssessmentName"]);
                        //    reportFailure("Pupil with assessment has not been updated even after adding ");

                        //     }



                        test.Log(Status.Info,
                            "CT Assessment Created and entered the TA value: " + data["CurriculumName"]);
                        reportPass("CTAssessment Created and entered data successfully");

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

