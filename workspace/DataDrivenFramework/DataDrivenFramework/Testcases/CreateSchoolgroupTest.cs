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
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DataDrivenFramework.Testcases
{
    [TestFixture]

    public class AD_CreateSchoolgroupTest : BaseTest
    {
        static ExcelReaderFile xls = new ExcelReaderFile(ConfigurationManager.AppSettings["xlsPath"]);
        static string testCaseName = "CreateSchoolgroup";

        
        [Test, TestCaseSource("getData")]
        public void AJ_CreateSchoolgroup(Dictionary<string, string> data)
        {
            rep = ExtentManager.getInstance();

          
            
                test = rep.CreateTest("Create Schoolgroup Test-" + data["TestId"] + "",
                    "This test will describe Create Schoolgroup functionality");
            

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

            if (CheckforElement("Dontshow_link"))
            {
                click("Dontshow_link");
            }

            try
            {
                click("Span_Xpath");
                Thread.Sleep(1000);
                click("AssessmentTab_Xpath");
                Thread.Sleep(2000);
                IWebElement element=   driver.FindElement(By.XPath("//a[contains(text(),'Create & edit my school groups')]"));

                element.Click();

                //click("CreateSchoolgroupTab_Xpath");
                Thread.Sleep(1000);
                explicitWait("MySchoolgroupLabel_Xpath");
              
             
                if (IsElementVisible("MySchoolgroupLabel_Xpath").Equals(false))
                {


                    test.Log(Status.Info, "My Schoolgroup Page not shown!!");
                    reportFailure("My School group Page not shown!!!");

                }
              

          
                Thread.Sleep(2000);



                // JavaScriptExecutor("CreateSchoolgroupTab_Xpath");

                 click("CreateSchoolgroupTab_Xpath");
                explicitWait("CreateSchoolgroupLabel_Xpath");

                if (IsElementVisible("CreateSchoolgroupLabel_Xpath").Equals(false))
                {


                    test.Log(Status.Info, "CreateSchoolgroup Page not shown!!");
                    reportFailure("Create Schoolgroup Page not shown!!");

                }

                type("SchoolgroupnameInbox_Xpath",data["SchoolgroupName"]);

                click("CreateSchoolgroupLabel_Xpath");
                Thread.Sleep(2000);
                if (isElementPresent("GroupExistLabel_Xpath").Equals(true))
                {
                    test.Log(Status.Info, "SchoolGroup already exist: " + data["SchoolgroupName"]);
                    reportFailure("SchoolgroupName already exist!!!");


                }
                
                if (data.ContainsKey("SubgroupName").Equals(true))
                {
                    type("AddsubgroupInput_Xpath", data["SubgroupName"]);
                    Thread.Sleep(1000);
                    click("AddSubgroupButton_Xpath");

                    test.Log(Status.Info, "Creating a School group with Subgroup Name -" + data["SubgroupName"]);

                }


                if (data.ContainsKey("Description").Equals(true))
                {
                    type("SchoolgroupDescription_Xpath", data["Description"]);

                }

                Thread.Sleep(1000);
                click("CreateGroupButton_Xpath");

                String Group_Xpath = "//span[contains(text(), " + "'" + data["SchoolgroupName"] + "'" + ")]";

               
                explicitWait("MySchoolgroupLabel_Xpath");
                if (IsElementVisible("MySchoolgroupLabel_Xpath").Equals(true) & iselementExist(Group_Xpath).Equals(true))
                {

                    test.Log(Status.Info, "SchoolGroup Created -" + data["SchoolgroupName"]);
                 //   reportPass("SchoolGroup Created successfully");

                }
                else
                {

                    test.Log(Status.Info, "SchoolGroup Not Created-" + data["SchoolgroupName"]);
                     reportFailure("Schoolgroup not Created");

                }


                click("SchoolgroupSearchInput_Xpath");
                Thread.Sleep(1000);
                type("SchoolgroupSearchInput_Xpath", data["SchoolgroupName"]);
                Thread.Sleep(1000);
                click("AddpupilsLink_Xpath");
                explicitWait("AddpupilsToMygroupLabel_Xpath");
                click("AddpupilCheckbox_Xpath");
                click("pupilgroupSaveButton_Xpath");
                explicitWait("AssessmentSavedLabel_Xpath");



                test.Log(Status.Info, "Schoolgroup Created and have added pupil data  -" + data["SchoolgroupName"]);
                reportPass("Schoolgroup Created and have added pupils to group");


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