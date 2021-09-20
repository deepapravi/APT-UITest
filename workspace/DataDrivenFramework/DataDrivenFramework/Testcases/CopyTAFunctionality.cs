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
    public class AG_CopyTAFunctionality: BaseTest
    {
        static ExcelReaderFile xls = new ExcelReaderFile(ConfigurationManager.AppSettings["xlsPath"]);
        static string testCaseName = "CopyTAFunctionalityTest";


        [Test, TestCaseSource("getData")]
        public void CopyTAFunction(Dictionary<string, string> data)
        {
            rep = ExtentManager.getInstance();
            test = rep.CreateTest("CopyTAFunctionality Test-" + data["TestId"] + "", "This test will describe CopyTA Functionality");

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
                        click("Span_Xpath");
                        Thread.Sleep(1000);
                        click("CurriculumTab_Xpath");
                        Thread.Sleep(1000);
                        click("EditCTtab_Xpath");
                        explicitWait("EditCTLabel_Xpath");
                        Thread.Sleep(4000);
                        String CTAssessment_Xpath =
                            "//h3[contains(text(), " + "'" + data["CurriculumName"] + "'" + ")]";
                        test.Log(Status.Info,
                            "Look for Assessment: " + data["CurriculumName"] + " in the Edit assessment page");
                        type("SearchCtTxt_Xpath", data["CurriculumName"]);
                    Thread.Sleep(1000);
                    JavaScriptExecutor("CTSearchbtn_Xpath");
                    Thread.Sleep(4000);
                    if (iselementExist(CTAssessment_Xpath).Equals(false))
                        {

                         
                            test.Log(Status.Info,
                                "Test has failed because the given CT assessment does not exist: " +
                                data["CurriculumName"]);
                            reportFailure("Test has failed because the given CT assessment does not exist");


                    }
                        else
                        {
                            try
                            {
                            explicitWaitUntilClickable("ViewandenterCTLink_Xpath");
                            click("ViewandenterCTLink_Xpath");
                                explicitWait("CTAllPupilLabel_Xpath");
                                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                                js.ExecuteScript("window.scrollBy(0,300)");
                            IWebElement TAdropdown1 = getElement("CTenterTA_Xpath");
                            //    SelectElement select1 = new SelectElement(TAdropdown1);

                            //string actualTA= select1.SelectedOption.Text;

                            string actualTA = TAdropdown1.Text;

                            if (actualTA.Equals("Choose"))
                            {
                                actualTA = "";

                            }

                        //    click("CTenterTA_Xpath");

                            Thread.Sleep(2000);
                                click("CopyCTButton_Xpath");
                                string Assessment_Xpath =
                                    "//label[contains(text(), " + "'" + data["AssessmentName"] + "'" + ")]";
                                //IJavaScriptExecutor js = (IJavaScriptExecutor) driver;
                                IWebElement Assessment = driver.FindElement(By.XPath(Assessment_Xpath));

                                js.ExecuteScript("arguments[0].scrollIntoView(true);", Assessment);
                            if (iselementExist(Assessment_Xpath).Equals(false))
                                {

                                    
                                    test.Log(Status.Info,
                                        "Test has failed because the given APT assessment does not exist in the copy list: " +
                                        data["AssessmentName"]);
                                    reportFailure(
                                        "Test has failed because the given APT assessment does not exist in the Copy list");


                            }
                                else
                                {
                                    Assessment.Click();
                                  Thread.Sleep(1000);
                                click("ChooseAssessmentBtn_Xpath");
                                    Thread.Sleep(2000);
                                    click("CTOverwriteTxt_Xpath");
                                    Thread.Sleep(1000);
                                    type("CTOverwriteTxt_Xpath", "overwrite");
                                    Thread.Sleep(1000);
                                    click("OverwriteCopyBtn_Xpath");
                                test.Log(Status.Info,
                                    "Trying to Copy TA: " +
                                    actualTA);
                                Thread.Sleep(1000);
                                    click("openAssessmentLink_Xpath");
                                    var newWindowHandle = driver.WindowHandles[1];
                                driver.SwitchTo().Window(newWindowHandle);
                                explicitWait("EnterPupilassessmentLabel_Xpath");

                            


                                IWebElement TAdropdown = getElement("APTEnterMathsTA_Xpath");
                                SelectElement select = new SelectElement(TAdropdown);

                                string TA = select.SelectedOption.Text;



                                if (TA.Equals(actualTA))
                                    {

                                    test.Log(Status.Info, "TA's are copied successfully to Assessment: " + data["AssessmentName"]);
                                    reportPass("TA data " +actualTA+ " are copied successfully ");


                                    }

                                    else
                                    {
                                   
                                    test.Log(Status.Info,
                                        "Test has failed TA values are not copied correctly: " +
                                        data["AssessmentName"]);
                                    reportFailure(
                                        "Test has failed TA values are not copied correctly");
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
                catch (Exception e)
                {
                    Assert.Fail(e.Message);

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
