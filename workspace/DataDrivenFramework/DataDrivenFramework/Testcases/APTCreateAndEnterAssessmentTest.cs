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

    public class AB_APTAssessmentTest:BaseTest
    {
        
        static ExcelReaderFile xls = new ExcelReaderFile(ConfigurationManager.AppSettings["xlsPath"]);
        static string testCaseName = "APTCreate&EnterTA-Assessment";
   
     /*Create assessment Test*/
        [Test, TestCaseSource("getData")]
        public void A_CreateAndEnterAssessmentTest(Dictionary<string, string> data)
        {
            rep = ExtentManager.getInstance();
            test = rep.CreateTest("APTCreate&EnterTA-Assessment Test-" + data["TestId"] + "", "This test will describe APT Create Teacher Assessment and Enter Assessment data");
         
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
                test.Log(Status.Info, "Create Assessment has failed because of invalid Login: " + data["Username"]);
                takeScreenshot();

            }
            else
            {
                if (CheckforElement("Dontshow_link"))
                {
                    click("Dontshow_link");
                }

                click("Span_Xpath");
                click("AssessmentTab_Xpath");
                click("CreateAssessmentTab_Xpath");
                Thread.Sleep(1000);
                explicitWait("CreateAssessmentLabel_Xpath");
                if (isElementPresent("CreateAssessmentLabel_Xpath").Equals(false))
                {
                    test.Log(Status.Info, "Create Assessment page is not shown for user: " + data["Username"]);
                    reportFailure("Create Assessment page is not shown");
                    
                    
                }
                else
                    if (CheckforElement("SetupButton_Xpath").Equals(true))
                    {

                    test.Log(Status.Info, "Create Assessment failed because Setup has not done for user: " + data["Username"]);
                    reportFailure("Setup has not Done");
                    
                    
                }
                else
                {

                    type("AssessmentNameInput_Xpath", data["AssessmentName"]);
                    Thread.Sleep(1000);
                    click("CreateAssessmentLabel_Xpath");
                    Thread.Sleep(2000);
                    if (IsElementVisible("AssessmentValidationText_Xpath").Equals(true))
                    {
                        test.Log(Status.Info, "Assessment already exist: " + data["AssessmentName"]);
                        reportFailure("Assessment already exist!!!");


                    }
                    else
                    {
                        // type("Keystage_Id",data["KeyStage"]);
                        try
                        {
                            if (data.ContainsKey("Testdescription").Equals(true))
                            {
                                type("AssessmentDesc_Xpath", data["Testdescription"]);
                            }
                            //   if (data["Description"] != "")



                            IWebElement Ksdropdown = getElement("Keystage_Id");
                            SelectElement s = new SelectElement(Ksdropdown);
                            Thread.Sleep(1000);
                            s.SelectByText(data["KeyStage"]);

                            if (data["KeyStage"].Equals("KS1"))
                            {
                                if (data["NcYear"].Equals("Single"))

                                    click("Year2_Xpath");
                                else
                                {
                                    click("Year2_Xpath");
                                    click("Year1_Xpath");
                                }

                            }
                            else
                            {
                                if (data["NcYear"].Equals("Single"))
                                    click("Year6_Xpath");
                                else
                                {
                                   // click("Year6_Xpath");
                                    click("Year5_Xpath");
                                    click("Year4_Xpath");
                                    click("Year3_Xpath");
                                }
                            }

                            click("chooseTATest_Xpath");

                            //type("teacherAssessment_Id", "DFE Teacher assessment");

                            IWebElement dropdown = getElement("teacherAssessment_Id");
                            SelectElement s1 = new SelectElement(dropdown);
                            s1.SelectByText("DFE Teacher assessment");
                            Thread.Sleep(2000);

                            if (data["KeyStage"].Equals("KS1"))
                            {
                                //
                                click("SubKS1TAMaths_Xpath");
                                click("SubKS1TAReading_Xpath");
                                click("SubKS1TAWriting_Xpath");

                                //click("SubKS1TAMathsLive_Xpath");
                                //click("SubKS1TAReadingLive_Xpath");
                                //click("SubKS1TAWritingLive_Xpath");

                            }
                            else
                            {

                                click("SubKS2TAMaths_Xpath");
                                click("SubKS2TAReading_Xpath");
                                click("SubKS2TAWriting_Xpath");
                                click("SubKS2TAGPS_Xpath");

                                //click("SubKS2TAMathsLive_Xpath");
                                //click("SubKS2TAReadingLive_Xpath");
                                //click("SubKS2TAWriting_Xpath");
                                //click("SubKS2TAGPS_Xpath");

                            }

                            click("CreateAssessmentButton_Xpath");
                            String Assessment_Xpath =
                                "//span[contains(text(), " + "'" + data["AssessmentName"] + "'" + ")]";
                            Thread.Sleep(5000);
                            explicitWait("EditAssessmentLabel_Xpath");
                            if (IsElementVisible("EditAssessmentLabel_Xpath").Equals(true) &
                                iselementExist(Assessment_Xpath).Equals(true))
                            {

                                test.Log(Status.Info, "Assessment Created -" + data["AssessmentName"]);
                                reportPass("Assessment Created successfully");

                            }
                            else
                            {

                                test.Log(Status.Info, "Assessment Not Created-" + data["AssessmentName"]);
                                 reportFailure("Assessment not Created");

                            }

                            type("SearchAssessmentTxt_Xpath", data["AssessmentName"]);
                            if (iselementExist(Assessment_Xpath).Equals(false))
                            {

                                reportFailure("The given APT assessment not exist ");
                                test.Log(Status.Info,
                                    "The given assessment does not exist: " +
                                    data["AssessmentName"]);
                                takeScreenshot();

                            }
                            else
                            {

                                Thread.Sleep(2000);
                                test.Log(Status.Info,
                                    "Look for Assessment: " + data["AssessmentName"] + " in the Edit assessment page");

                                click("EnterpageEditAssessmentLink_Xpath");
                                Thread.Sleep(1000);
                                explicitWait("EnterpagepupilLabel_Xpath");

                                IWebElement TAMathsdropdown = getElement("APTEnterMathsTA_Xpath");
                                SelectElement selectMaths = new SelectElement(TAMathsdropdown);


                                IWebElement TAReadingdropdown = getElement("APTEnterReadingTA_Xpath");
                                SelectElement selectReading = new SelectElement(TAReadingdropdown);



                                IWebElement TAWritingdropdown = getElement("APTEnterWritingTA_Xpath");
                                SelectElement selectWriting = new SelectElement(TAWritingdropdown);

                               

                                if (data["KeyStage"].Equals("KS1"))
                                { 
                                    if (data["TA"] != "")
                                
                                    selectMaths.SelectByText(data["TA"]);
                                    selectReading.SelectByText(data["TA"]);
                                    selectWriting.SelectByText(data["TA"]);

                                    click("EnterAssessmentSave_Xpath");


                                    explicitWait("AssessmentSavedLabel_Xpath");
                                }

                                else
                                {
                                    if (data["TA"] != "")
                                    {

                                        IWebElement TAGPSdropdown = getElement("APTEnterGPSTA_Xpath");
                                        SelectElement selectGPS = new SelectElement(TAGPSdropdown);

                                        selectMaths.SelectByText(data["TA"]);
                                        selectReading.SelectByText(data["TA"]);
                                        selectWriting.SelectByText(data["TA"]);
                                        selectGPS.SelectByText(data["TA"]);
                                    }
                                    click("EnterAssessmentSave_Xpath");


                                    explicitWait("AssessmentSavedLabel_Xpath");


                                }

                                test.Log(Status.Info, "Assessment Created and Saved the data -" + data["AssessmentName"]);
                                    reportPass("Assessment Created and Entered the data successfully");
                                

                            }
                        }
                        catch (Exception ex)
                        {

                            if (errcount != 1)
                            {
                                reportFailure(ex.Message);
                            }

                            Assert.Fail("Fail the Test-" + ex.Message);
                        }
                        // String Assessment_Xpath = "//span[contains(text(), "+data["AssessmentName"]+ ")]";*/

                    }

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