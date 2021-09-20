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

    public class AD_APTCreateAssessmentPageValidation : BaseTest
    {

        static ExcelReaderFile xls = new ExcelReaderFile(ConfigurationManager.AppSettings["xlsPath"]);
        static string testCaseName = "CreateAssessmentPageValidation";
        string PUMA2016_Xpath, PIRA2016_Xpath, PUMA2020_Xpath, PIRA2020_Xpath, WritingTA_Xpath, GPSTA_Xpath;

[Test, TestCaseSource("getData")]
        public void APTCreatePageValidation(Dictionary<string, string> data)
        {
            try
            {
                rep = ExtentManager.getInstance();
                test = rep.CreateTest("CreateAssessmentPageValidation", "This test will validate the Create Assessment page structure");
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
                    
                    click("Span_Xpath");
                    Thread.Sleep(1000);
                    click("AssessmentTab_Xpath");
                    click("CreateAssessmentTab_Xpath");
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
                        IWebElement Ksdropdown = getElement("Keystage_Id");
                        SelectElement s = new SelectElement(Ksdropdown);
                        Thread.Sleep(1000);
                        s.SelectByText(data["KeyStage"]);

                        if (data["KeyStage"].Equals("KS1"))
                        {
                            if (data["NcYear"].Equals("Single"))
                            {
                                click("Year2_Xpath");
                                 PUMA2016_Xpath = "//label[contains(text(),'PUMA 2016 2 Autumn')]";
                                 PIRA2016_Xpath = "//label[contains(text(),'PiRA 2016 2 Autumn')]";
                                 PUMA2020_Xpath = "//label[contains(text(),'PUMA 2020 2 Autumn')]";
                                 PIRA2020_Xpath = "//label[contains(text(),'PUMA 2020 2 Autumn')]";
                                WritingTA_Xpath = ConfigurationManager.AppSettings["KS1-RSWritingTA_Xpath"];
                                GPSTA_Xpath = ConfigurationManager.AppSettings["KS1-RSGPSTA_Xpath"];
                                //  WritingTA_Xpath = "//body/div[@id='wrapper']/div[1]/div[2]/div[1]/div[1]/div[4]/div[2]/div[7]/div[2]/div[1]/div[1]/div[1]/div[2]/div[2]/div[1]/div[2]/div[3]/div[5]/div[1]/select[1]";
                                // GPSTA_Xpath= "//body/div[@id='wrapper']/div[1]/div[2]/div[1]/div[1]/div[4]/div[2]/div[7]/div[2]/div[1]/div[1]/div[1]/div[2]/div[2]/div[1]/div[2]/div[2]/div[5]/div[2]/select[1]";
                            }
                            else
                            {
                                click("Year2_Xpath");
                                click("Year1_Xpath");

                                 PUMA2016_Xpath = "//label[contains(text(),'PUMA 2016 1 Autumn')]";
                                 PIRA2016_Xpath = "//label[contains(text(),'PiRA 2016 1 Autumn')]";
                                 PUMA2020_Xpath = "//label[contains(text(),'PUMA 2020 1 Autumn')]";
                                 PIRA2020_Xpath = "//label[contains(text(),'PUMA 2020 1 Autumn')]";

                                WritingTA_Xpath = ConfigurationManager.AppSettings["KS1-Multiple-RSWritingTA_Xpath"];
                                GPSTA_Xpath = ConfigurationManager.AppSettings["KS1-Multiple-RSGPSTA_Xpath"];
                                //  WritingTA_Xpath = "//body/div[@id='wrapper']/div[1]/div[2]/div[1]/div[1]/div[4]/div[2]/div[7]/div[2]/div[1]/div[1]/div[1]/div[2]/div[2]/div[1]/div[1]/div[3]/div[5]/div[1]/select[1]";
                                // GPSTA_Xpath = "//body/div[@id='wrapper']/div[1]/div[2]/div[1]/div[1]/div[4]/div[2]/div[7]/div[2]/div[1]/div[1]/div[1]/div[2]/div[2]/div[1]/div[1]/div[2]/div[5]/div[2]/select[1]";

                            }

                        }
                        else
                        {
                            if (data["NcYear"].Equals("Single"))
                            {
                                click("Year6_Xpath");
                                 PUMA2016_Xpath = "//label[contains(text(),'PUMA 2016 6 Autumn')]";
                                 PIRA2016_Xpath = "//label[contains(text(),'PiRA 2016 6 Autumn')]";
                                 PUMA2020_Xpath = "//label[contains(text(),'PUMA 2020 6 Autumn')]";
                                 PIRA2020_Xpath = "//label[contains(text(),'PUMA 2020 6 Autumn')]";
                                WritingTA_Xpath = ConfigurationManager.AppSettings["KS2-RSWritingTA_Xpath"];
                                GPSTA_Xpath = ConfigurationManager.AppSettings["KS2-RSGPSTA_Xpath"];
                                //  WritingTA_Xpath = "//body/div[@id='wrapper']/div[1]/div[2]/div[1]/div[1]/div[4]/div[2]/div[7]/div[2]/div[1]/div[1]/div[1]/div[2]/div[2]/div[1]/div[6]/div[3]/div[6]/div[1]/select[1]";
                                // GPSTA_Xpath= "//body/div[@id='wrapper']/div[1]/div[2]/div[1]/div[1]/div[4]/div[2]/div[7]/div[2]/div[1]/div[1]/div[1]/div[2]/div[2]/div[1]/div[6]/div[1]/div[6]/div[2]/select[1]";
                            }

                            else
                            {
                                click("Year6_Xpath");
                                click("Year5_Xpath");
                                click("Year4_Xpath");
                                click("Year3_Xpath");

                                 PUMA2016_Xpath = "//label[contains(text(),'PUMA 2016 3 Autumn')]";
                                 PIRA2016_Xpath = "//label[contains(text(),'PiRA 2016 3 Autumn')]";
                                 PUMA2020_Xpath = "//label[contains(text(),'PUMA 2020 3 Autumn')]";
                                 PIRA2020_Xpath = "//label[contains(text(),'PUMA 2020 3 Autumn')]";
                                WritingTA_Xpath = ConfigurationManager.AppSettings["KS2-Multiple-RSWritingTA_Xpath"];
                                GPSTA_Xpath = ConfigurationManager.AppSettings["KS2-Multiple-RSGPSTA_Xpath"];

                                // WritingTA_Xpath = "//body/div[@id='wrapper']/div[1]/div[2]/div[1]/div[1]/div[4]/div[2]/div[7]/div[2]/div[1]/div[1]/div[1]/div[2]/div[2]/div[1]/div[3]/div[3]/div[6]/div[1]/select[1]";
                                // GPSTA_Xpath= "//body/div[@id='wrapper']/div[1]/div[2]/div[1]/div[1]/div[4]/div[2]/div[7]/div[2]/div[1]/div[1]/div[1]/div[2]/div[2]/div[1]/div[3]/div[1]/div[6]/div[2]/select[1]";
                            }
                        }

                        click("chooseTest_Xpath");
                        IWebElement dropdown = getElement("testprovider_Id");
                        SelectElement s1 = new SelectElement(dropdown);
                        s1.SelectByText("Rising Stars");
                        test.Log(Status.Info, "Selected Rising star Test and validating the drop down for subprovider, Term and SubTest!!");
                        if (isElementPresent("subTestProvider_Id").Equals(false))
                        {

                            test.Log(Status.Info, "Sub provider is not shown for Rising Star Test ");
                            reportFailure("Test failed !!! Sub provider is not shown for Rising Star Test");


                        }

                        else
                            if(isElementPresent("Term_Id").Equals(false))
                        {

                            test.Log(Status.Info, "Term is not shown for Rising Star Test ");
                            reportFailure("Test failed !!!Test Term is not shown for Rising Star Test");

                        }
                        else
                            if(iselementExist(PUMA2016_Xpath).Equals(false)|iselementExist(PUMA2020_Xpath).Equals(false)| iselementExist(PIRA2016_Xpath).Equals(false) | iselementExist(PIRA2020_Xpath).Equals(false))
                        {

                            test.Log(Status.Info, "SubTest  not shown for Rising Star Test ");
                            reportFailure("Test failed !!!RS Subtest is not shown for Rising Star Test");

                        }

                     /*   else if(iselementExist(WritingTA_Xpath).Equals(false)| iselementExist(GPSTA_Xpath).Equals(true))
                        {

                            test.Log(Status.Info, "Writing TA drop down has not shown/or GPS TA shown for Rising Star PIRA/PUMA Test ");
                            reportFailure("Writing TA drop down has not shown/or GPS TA shown for Rising Star PIRA/PUMA Test");


                        }*/

                        // test.Log(Status.Info, "Test passed for Rising Star page validation");
                            reportPass("Test passed for Rising Star page validation and SubTests shown :-)");
                        Thread.Sleep(1000);
                        s1.SelectByText("NfER");
                        test.Log(Status.Info, "Selected NfER Test and validating the page structure!!");
                        if (isElementPresent("subTestProvider_Id").Equals(true))
                        {

                            test.Log(Status.Info, "Sub provider is shown for NFER Test ");
                            reportFailure(" Test is failed as theSub provider is not shown for NFER Test");


                        }
                        else
                        if (isElementPresent("Term_Id").Equals(false))
                        {

                            test.Log(Status.Info, "Term is not shown for NfER Test ");
                            reportFailure("Test is failed as the Test Term is not shown for NfER Test");

                        }
                        reportPass("Test passed for NfER page validation :-)");

                        s1.SelectByText("Renaissance");
                        test.Log(Status.Info, "Selected Renaissance Test and validating the page structure!!");
                        if (isElementPresent("subTestProvider_Id").Equals(true))
                        {

                            test.Log(Status.Info, "Sub provider is shown for Renaissance Test ");
                            reportFailure("Test is failed !!! The Sub provider is shown for Renaissance Test");


                        }
                        else
                       if (isElementPresent("Term_Id").Equals(true))
                        {

                            test.Log(Status.Info, "Term is  shown for Renaissance Test ");
                            reportFailure("Test failed !!!Test Term is shown for Renaissance Test");

                        }

                        reportPass("Test passed for Renaissance page validation :-)");

                        s1.SelectByText("DFE");
                        test.Log(Status.Info, "Selected DFE Test and validating the page structure!!");
                        if (isElementPresent("subTestProvider_Id").Equals(true))
                        {

                            test.Log(Status.Info, "Sub provider is shown for DFE Test ");
                            reportFailure("Test is failed !!! The Sub provider is shown for DFE");


                        }
                        else
                       if (isElementPresent("Term_Id").Equals(true))
                        {

                            test.Log(Status.Info, "Term is  shown for DFE Test ");
                            reportFailure("Test failed !!!Test Term is shown for DFE Test");

                        }

                        reportPass("Test passed for DFE page validation :-)");



                        s1.SelectByText("GL Assessment");
                        test.Log(Status.Info, "Selected GL Assessment Test and validating the page structure!!");
                        if (isElementPresent("subTestProvider_Id").Equals(true))
                        {

                            test.Log(Status.Info, "Sub provider is shown for GL Assessment Test ");
                            reportFailure("Test is failed !!! The Sub provider is shown for GL Assessment Test");


                        }
                        else
                       if (isElementPresent("Term_Id").Equals(true))
                        {

                            test.Log(Status.Info, "Term is  shown for GL Assessment Test ");
                            reportFailure("Test Term is shown for GL Test");

                        }

                        reportPass("Test passed for GL Assessment page validation :-)");

                        test.Log(Status.Info, "CreateAssessment Page validation Test passed");
                    }
                }
            }
            catch (Exception ex)
            {

               // reportFailure(ex.Message);
                Assert.Fail("Fail the Test-" + ex.Message);

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
