using Amazon.SecretsManager;
using AventStack.ExtentReports;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using Org.BouncyCastle.Bcpg.OpenPgp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using Newtonsoft.Json;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System.IO.MemoryMappedFiles;
using System.IO;

namespace DataDrivenFramework.Testcases
{
    public class BaseTest
    {
        public IWebDriver driver = null;
        //  public ChromeDriver driver = null;
        public ExtentReports rep;
        public ExtentTest test;
        public int errcount = 0;

        public void openBrowser(string bType)
        {
            test.Log(Status.Info, "Opening the Browser" + bType);
            if (bType.Equals("Mozilla"))
            {
                FirefoxDriverService service = FirefoxDriverService.CreateDefaultService("geckodriver.exe");
                service.FirefoxBinaryPath = @"C:\Program Files\Mozilla Firefox\firefox.exe";
                //   driver = new FirefoxDriver(service);

            }
            else if (bType.Equals("Chrome"))
            {
                // ChromeOptions options = new ChromeOptions();
                String dir = AppDomain.CurrentDomain.BaseDirectory;
                FileInfo fileInfo = new FileInfo(dir);
                DirectoryInfo currentDir = fileInfo.Directory.Parent.Parent;
                string parentDirName = currentDir.FullName;
                // string filePath = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory);

                ChromeOptions options = new ChromeOptions();
                options.AddArguments("--start-maximized");
                options.AddArguments("--disable-gpu");

                driver = new ChromeDriver(parentDirName + "\\Chrome", options);
                //options.AddArgument("--start-maximized");
                //driver = new ChromeDriver(options);
                //driver.Manage().Window.Maximize();


                //ChromeOptions options = new ChromeOptions();
                //options.AddArgument("--start-maximized");
                //driver = new ChromeDriver(options);

            }
            else if (bType.Equals("IE"))
            {

                //      driver = new InternetExplorerDriver(ConfigurationManager.AppSettings["IEDriverPath"]);

            }
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(20);
            test.Log(Status.Info, "Browser has opened successfully :" + bType);

        }

        public void navigate(string urlKey)
        {
            test.Log(Status.Info, "Navigating to " + ConfigurationManager.AppSettings[urlKey]);
            driver.Url = ConfigurationManager.AppSettings[urlKey];


        }

        public void click(string xpathExpKey)
        {
            try
            {
                test.Log(Status.Info, "Clicking on " + xpathExpKey);
                getElement(xpathExpKey).Click();
                test.Log(Status.Info, "Clicked successfully on " + xpathExpKey);
                //Thread.Sleep(4000);
            }
            catch (Exception ex)
            {

                //fail test and report the error
                //  reportFailure(ex.Message);
                //  Assert.Fail("Fail the Test-" + ex.Message);


                if (errcount != 1)
                {
                    reportFailure(ex.Message);
                }

                Assert.Fail("Fail the Test-" + ex.Message);

            }


        }

        public void type(string xpathExpKey, string data)
        {
            test.Log(Status.Info, "Typing in" + xpathExpKey + "-Data: " + data);
            getElement(xpathExpKey).SendKeys(data);
            test.Log(Status.Info, "Typed successfully in " + xpathExpKey);

        }

        public IWebElement getElement(string locatorkey)
        {
            IWebElement e = null;
            try
            {
                if (locatorkey.EndsWith("_Xpath"))
                {
                    e = driver.FindElement(By.XPath(ConfigurationManager.AppSettings[locatorkey]));
                  

                }
                else if (locatorkey.EndsWith("_Id"))
                {
                    e = driver.FindElement(By.Id(ConfigurationManager.AppSettings[locatorkey]));

                }
                else if (locatorkey.EndsWith("_name"))
                {
                    e = driver.FindElement(By.Name(ConfigurationManager.AppSettings[locatorkey]));

                }
                else if (locatorkey.EndsWith("_link"))
                {

                    e = driver.FindElement(By.LinkText(ConfigurationManager.AppSettings[locatorkey]));
                }

                else if (locatorkey.EndsWith("_Plink"))
                {

                    e = driver.FindElement(By.PartialLinkText(ConfigurationManager.AppSettings[locatorkey]));
                }
                else if (locatorkey.EndsWith("_Class"))
                {

                    e = driver.FindElement(By.ClassName(ConfigurationManager.AppSettings[locatorkey]));
                }
                else
                {

                    reportFailure("Locator is not correct" + locatorkey);
                    Assert.Fail("Locator not correct" + locatorkey);

                }
            }
            catch (Exception ex)
            {

                //fail test and report the error
                reportFailure(ex.Message);
                Assert.Fail("Fail the Test-" + ex.Message);

            }
            return e;
        }

        public void hoverElement(string locatorkey)
        {

            try
            {

                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                var element = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(ConfigurationManager.AppSettings[locatorkey])));

                Actions action = new Actions(driver);
                action.MoveToElement(element).Perform();
            }
            catch (Exception ex)
            {

                //fail test and report the error
                reportFailure(ex.Message);
                Assert.Fail("Fail the Test-" + ex.Message);

            }

        }

        /********************Validation Funcions*****************/

        public bool verifyTitle()

        {
            return false;

        }


        public bool IsElementVisible(string locatorkey)

        {
            IWebElement element = getElement(locatorkey);

            bool b = element.Displayed;
       
            if (b.Equals(true))
            {
                return true;
            }

            return false;
        }

        public bool isElementPresent(string locatorkey)
        {
            IList<IWebElement> elementList = null;
            try
            {


                if (locatorkey.EndsWith("_Xpath"))
                {
                    elementList = driver.FindElements(By.XPath(ConfigurationManager.AppSettings[locatorkey]));

                }
                else if (locatorkey.EndsWith("_Id"))
                {
                    elementList = driver.FindElements(By.Id(ConfigurationManager.AppSettings[locatorkey]));

                }
                else if (locatorkey.EndsWith("_name"))
                {
                    elementList = driver.FindElements(By.Name(ConfigurationManager.AppSettings[locatorkey]));

                }
                else if (locatorkey.EndsWith("_link"))
                {
                    elementList = driver.FindElements(By.LinkText(ConfigurationManager.AppSettings[locatorkey]));

                }
                else if (locatorkey.EndsWith("_Class"))
                {
                    elementList = driver.FindElements(By.ClassName(ConfigurationManager.AppSettings[locatorkey]));

                }
                else if (locatorkey.EndsWith("_Plink"))
                {

                    elementList = driver.FindElements(By.PartialLinkText(ConfigurationManager.AppSettings[locatorkey]));
                }
                else
                {

                    reportFailure("Locator is not correct" + locatorkey);
                    Assert.Fail("Locator not correct" + locatorkey);

                }
            }
            catch (Exception ex)
            {

                //fail test and report the error
                reportFailure(ex.Message);
                // Assert.Fail("Fail the Test-" + ex.Message);

            }

            if (elementList.Count == 0)

                return false;
            else
                return true;
        }





        public bool CheckforElement(string locatorkey)
        {
            try
            {
                IList<IWebElement> elementList = null;
                if (locatorkey.EndsWith("_Xpath"))
                {
                    elementList = driver.FindElements(By.XPath(ConfigurationManager.AppSettings[locatorkey]));

                }
                else if (locatorkey.EndsWith("_Id"))
                {
                    elementList = driver.FindElements(By.Id(ConfigurationManager.AppSettings[locatorkey]));

                }
                else if (locatorkey.EndsWith("_name"))
                {
                    elementList = driver.FindElements(By.Name(ConfigurationManager.AppSettings[locatorkey]));

                }
                else if (locatorkey.EndsWith("_link"))
                {
                    elementList = driver.FindElements(By.LinkText(ConfigurationManager.AppSettings[locatorkey]));

                }
                else if (locatorkey.EndsWith("_Class"))
                {
                    elementList = driver.FindElements(By.ClassName(ConfigurationManager.AppSettings[locatorkey]));

                }
                //else
                //{

                //    reportFailure("Locator is not correct" + locatorkey);
                //    Assert.Fail("Locator not correct" + locatorkey);

                //}
                if (elementList.Count == 0)

                    return false;
                else
                    return true;
            }
            catch (Exception e)
            {

                throw e;
            }


        }

        public bool verifyText(string locatorKey, string expectedTextKey)
        {
            string actualText = getElement(locatorKey).Text;
            string expectedTest = expectedTextKey;
            if (actualText.Equals(expectedTest))
                return true;
            else
                return false;


        }

        public void clickAndWait(string locator_clicked, string locator_present)
        {
            test.Log(Status.Info, "Clicking on -" + locator_clicked + "waiting for-" + locator_present);
            int count = 5;
            for (int i = 0; i < count; i++)
            {

                getElement(locator_clicked).Click();
                Thread.Sleep(5000);
                isElementPresent(locator_present);
                break;

            }

        }

        public void explicitWait(string locator)
        {
            try
            {

                WebDriverWait waitForElement = new WebDriverWait(driver, TimeSpan.FromSeconds(60));
                // waitForElement.Until(ExpectedConditions.ElementIsVisible(By.XPath(locator)));
                waitForElement.Until(
                    ExpectedConditions.ElementIsVisible(By.XPath(ConfigurationManager.AppSettings[locator])));
            }

            catch (Exception e)
            {
                reportFailure(e.Message);
                // Assert.Fail("Fail the Test-" + e.Message);

            }
        }

        public void explicitWaitUntilClickable(string locator)
        {
            try
            {

                WebDriverWait waitForElement = new WebDriverWait(driver, TimeSpan.FromSeconds(60));
                // waitForElement.Until(ExpectedConditions.ElementIsVisible(By.XPath(locator)));
                waitForElement.Until(
                    ExpectedConditions.ElementToBeClickable(By.XPath(ConfigurationManager.AppSettings[locator])));

                            }

            catch (Exception e)
            {
                reportFailure(e.Message);
                // Assert.Fail("Fail the Test-" + e.Message);

            }
        }


        public bool waitforElementPresent(string locator)

        {
            try
            {

                WebDriverWait waitForElement = new WebDriverWait(driver, TimeSpan.FromSeconds(60));
                // waitForElement.Until(ExpectedConditions.ElementIsVisible(By.XPath(locator)));
                waitForElement.Until(
                    ExpectedConditions.ElementIsVisible(By.XPath(ConfigurationManager.AppSettings[locator])));
            }

            catch (Exception e)
            {
                return false;

            }

            return true;
        }
        public bool isAlertPresent()
        {

            bool presentFlag = false;

            try
            {

                // Check the presence of alert
              var alert = driver.SwitchTo().Alert();
                // Alert present; set the flag
                presentFlag = true;
                // if present consume the alert
                alert.Accept(); 
                //( Now, click on ok or cancel button )

            }
            catch (NoAlertPresentException ex)
            {
                // Alert not present
                // ex.printStackTrace();
                return true;
            }

            return presentFlag;
        }

        public string getText(string locatorKey)
        {
            test.Log(Status.Info, "Getting the text from" + locatorKey);
            return getElement(locatorKey).Text;

        }

        /******************Reporting Functions********************/

        public void reportPass(string msg)
        {
            test.Log(Status.Pass, msg);

        }

        public void reportFailure(String msg)
        {
            test.Log(Status.Fail, msg);
            takeScreenshot();
            errcount = 1;
            Assert.Fail(msg);

        }


        public void takeScreenshot()
        {
            //filename of the screenshot
            //string screenshotFile = DateTime.Now.ToString().Replace("/", "_").Replace(":", "_").Replace(" ", "_") + ".gif";
            //ITakesScreenshot screenshotDriver = driver as ITakesScreenshot;
            //Screenshot screenshot = screenshotDriver.GetScreenshot();
            //string filePath = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory);
            //filePath = Directory.GetParent(Directory.GetParent(filePath).FullName).FullName;
            //screenshot.SaveAsFile(filePath + ".\\Reports\\Screenshots\\" + screenshotFile, ScreenshotImageFormat.Gif);

            //string screenshotPath = filePath + ".\\Reports\\Screenshots\\" + screenshotFile;
            //test.Log(Status.Info, "Screenshot-", MediaEntityBuilder.CreateScreenCaptureFromPath(screenshotPath).Build());
            //test.AddScreenCaptureFromPath(screenshotPath);
            //-----------------------------------------------------------------------------------------------


            //string screenshotFile = DateTime.Now.ToString().Replace("/", "_").Replace(":", "_").Replace(" ", "_") + ".gif";
            //ITakesScreenshot screenshotDriver = driver as ITakesScreenshot;
            //Screenshot screenshot = screenshotDriver.GetScreenshot();
            //string screenshotPath =  ".\\Reports\\Screenshots\\" + screenshot+".png";


            //test.Log(Status.Info, "Screenshot-", MediaEntityBuilder.CreateScreenCaptureFromPath(screenshotPath).Build());

            //string screenshotname = Appscreenshots.getscreenhot(name);


            //-----------------------------------------------------------------------

            //filename of the screenshot
            String dir = AppDomain.CurrentDomain.BaseDirectory;
            FileInfo fileInfo = new FileInfo(dir);
            DirectoryInfo currentDir = fileInfo.Directory.Parent.Parent;
            string parentDirName = currentDir.FullName;
            Screenshot file = ((ITakesScreenshot)driver).GetScreenshot();
            string screenshotFile = DateTime.Now.ToString().Replace("/", "_").Replace(":", "_").Replace(" ", "_") + ".png";
            ITakesScreenshot screenshotDriver = driver as ITakesScreenshot;



            // file.SaveAsFile(@"\\mercury\Development\Testresults\Screenshots\" + screenshotFile, ScreenshotImageFormat.Png);

            file.SaveAsFile(@"\\mimas.fft.local\tfs-build-reports\Testresults\Screenshots\" + screenshotFile, ScreenshotImageFormat.Png);

            // test.Log(Status.Info, "Screenshot-", MediaEntityBuilder.CreateScreenCaptureFromPath(parentDirName + "\\Reports\\Screenshots\\" + screenshotFile).Build());

            test.Log(Status.Info, "Screenshot-", MediaEntityBuilder.CreateScreenCaptureFromPath(@"\\mimas.fft.local\tfs-build-reports\Testresults\Screenshots\" + screenshotFile).Build());

            test.AddScreenCaptureFromPath(@"\\mimas.fft.local\tfs-build-reports\Testresults\Screenshots\" + screenshotFile);

            // test.AddScreenCaptureFromPath(parentDirName + "\\Reports\\Screenshots\\" + screenshotFile);










        }



        public void TakeFullScreenshot()
        {
            //  ChromeDriver driver = new ChromeDriver();

            //String dir = AppDomain.CurrentDomain.BaseDirectory;
            //FileInfo fileInfo = new FileInfo(dir);
            //DirectoryInfo currentDir = fileInfo.Directory.Parent.Parent;
            //string parentDirName = currentDir.FullName;

            //Dictionary<string, Object> metrics = new Dictionary<string, Object>();
            //metrics["width"] = driver.ExecuteScript("return Math.max(window.innerWidth,document.body.scrollWidth,document.documentElement.scrollWidth)");
            //metrics["height"] = driver.ExecuteScript("return Math.max(window.innerHeight,document.body.scrollHeight,document.documentElement.scrollHeight)");
            //metrics["deviceScaleFactor"] = (double)driver.ExecuteScript("return window.devicePixelRatio");
            //metrics["mobile"] = driver.ExecuteScript("return typeof window.orientation !== 'undefined'");
            //driver.ExecuteChromeCommand("Emulation.setDeviceMetricsOverride", metrics);
            //Screenshot file = ((ITakesScreenshot)driver).GetScreenshot();
            //string screenshotFile = DateTime.Now.ToString().Replace("/", "_").Replace(":", "_").Replace(" ", "_") + ".png";



            ////Execute the emulation Chrome Command to change browser to a custom device that is the size of the entire page

            //ITakesScreenshot screenshotDriver = driver as ITakesScreenshot;




            //file.SaveAsFile(@"\\mimas.fft.local\tfs-build-reports\Testresults\Screenshots\" + screenshotFile, ScreenshotImageFormat.Png);


            //driver.ExecuteChromeCommand("Emulation.clearDeviceMetricsOverride", new Dictionary<string, Object>());
            //test.Log(Status.Info, "Screenshot-", MediaEntityBuilder.CreateScreenCaptureFromPath(@"\\mimas.fft.local\tfs-build-reports\Testresults\Screenshots\" + screenshotFile).Build());

            //test.AddScreenCaptureFromPath(@"\\mimas.fft.local\tfs-build-reports\Testresults\Screenshots\" + screenshotFile);





        }




        /******************Application Functions*****************************/

        public bool doLogin(string username, string password)
        {
            test.Log(Status.Info, "Trying to login with " + username + " , " + password);
            //navigate("appurl");
            type("Username_Id", username);
            type("Password_Id", password);
            click("Login_Id");
            if (isElementPresent("Homepage_Xpath"))
            {
                test.Log(Status.Info, "Login Success");
                return true;
            }
            else
            {
                test.Log(Status.Info, "Login Failed");

                return false;

            }
        }

        public bool iselementExist(string locatorkey)
        {
            IList<IWebElement> elementList = null;
            try
            {
                elementList = driver.FindElements(By.XPath(locatorkey));
            }
            catch (Exception e)
            {
                reportFailure(e.Message);
            }



            if (elementList.Count == 0)

                return false;
            else
                return true;
        }






        public string getElementText(string locatorkey)
        {
            IWebElement element = null;
            try
            {
                element = driver.FindElement(By.XPath(locatorkey));
            }
            catch (Exception e)
            {
                reportFailure(e.Message);
            }

            return element.Text;

        }

        public void getElementClick(string locatorkey)
        {
            IWebElement element = null;
            try
            {
                element = driver.FindElement(By.XPath(locatorkey));
            }
            catch (Exception e)
            {
                reportFailure(e.Message);
            }

            element.Click();

        }



        public void PerformActionClick(string locatorkey)
        {
            //IWebElement element = driver.FindElement(By.XPath(locatorkey));
            IWebElement element = getElement(locatorkey);
            Actions actions = new Actions(driver);
            actions.MoveToElement(element).Click().Perform();


        }
        public void scrolldown(string locatorkey)
        {
            // IWebElement s = driver.FindElement(By.XPath(locatorkey)); 
            IWebElement s = getElement(locatorkey);
            IJavaScriptExecutor je = (IJavaScriptExecutor)driver;
            je.ExecuteScript("arguments[0].scrollIntoView(true);", s);
            //return this;
        }
        public void scrollPageDown(int height)
        {

            IJavaScriptExecutor js1 = (IJavaScriptExecutor)driver;
            js1.ExecuteScript("window.scrollBy(0," + height + ")");
            // js1.ExecuteScript("window.scrollBy(0,500)");
        }

        public string GetPassword(string username)
        {
            try
            {

                /* var secretClient = new AmazonSecretsManagerClient();

                // SecretId = ConfigurationManager.AppSettings["secretId"];
                 var value = secretClient.GetSecretValue(new GetSecretValueRequest
                 {
                     SecretId = ConfigurationManager.AppSettings["secretId"]
                 });

                 var userDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(value.SecretString);
                 var password = userDictionary[username]; // this is where we read the value from AWS Secrets Manager*/
                var password = "testpassword";
                return password;
            }

            catch (Exception e)
            {

                // Assert.Fail("Fail the Test-");
              //  reportFailure("The key is not present in the AWS Secrets Manager ");
                return null;

            }



        }
    

        public void JavaScriptExecutor(string locatorkey)
        {
            IWebElement e = getElement(locatorkey);

            IJavaScriptExecutor executor = (IJavaScriptExecutor)driver;
            test.Log(Status.Info, "Clicking on " + locatorkey);
            executor.ExecuteScript("arguments[0].click();", e);


        }

        public bool VerifyFileDownload()

        {
            string profile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string expectedFilePath = profile + "\\Downloads\\assessment.xlsx";
            bool fileExist = false;
            ChromeOptions chromeOptions = new ChromeOptions();
            chromeOptions.AddUserProfilePreference("download.default_directory", profile + "\\Downloads");

            try
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
                wait.Until<bool>(x => fileExist = File.Exists(expectedFilePath));
                FileInfo fileInfo = new FileInfo(expectedFilePath);
                Assert.AreEqual(fileInfo.FullName, profile + "\\Downloads\\assessment.xlsx");
            }
            catch (Exception e)
            {
                reportFailure(e.Message);
                return false;

            }
            finally
            {
                if (File.Exists(expectedFilePath))
                    File.Delete(expectedFilePath);

            }
            return true;
        }


        public bool VerifyCompareReport(string CompareAssessmentPath)
        {
            
            
                if (isElementPresent("ComparereportLink__Plink").Equals(true))
                    click("ComparereportLink__Plink");
                else
                    return false;
            try { 
                Thread.Sleep(2000);
                if (isElementPresent(CompareAssessmentPath).Equals(true))
                {

                    string AssessmentName = getElement(CompareAssessmentPath).Text;
                    String Assessment_Xpath =
                                "//span[contains(text(), " + "'" + AssessmentName + "'" + ")]";
                    click(CompareAssessmentPath);
                    explicitWait("ComparereportLabel_Xpath");
                    if (iselementExist(Assessment_Xpath).Equals(false))
                    {
                        return false;
                       
                    }
                    else
                    {
                       
                        return true;

                    }

                }
                else
                {

                    test.Log(Status.Info,
                            "No matching assessment assessment found for comparison ");

                    takeScreenshot();
                }

                              
            }

            catch (Exception e)
            {
               // reportFailure(e.Message);
                return false;

            }
            finally
            {
                if (isElementPresent("DeleteCompare_Xpath").Equals(true))
                {

                    click("DeleteCompare_Xpath");
                    explicitWait("reportHeader_Xpath");
                    Thread.Sleep(1000);
                }

            }
            return true;
        }


        public void APTAggregateReportTest()
        {


            test.Log(Status.Info, "Checking the Summary report");
            if (waitforElementPresent("Summarydatatable_Xpath").Equals(false))
            {
                reportFailure("APT reportTest has failed");
                test.Log(Status.Info,
                    "APT reportTest has failed because the Summary report table is not shown");
                takeScreenshot();


            }
            else
            {
                // test.Log(Status.Info, ": " + data["AssessmentName"]);
                reportPass("MAT user can view APT Summary report for selected school");
            }

            Thread.Sleep(1000);
           
            click("APTyeargrouplink_Xpath");
            test.Log(Status.Info, "Checking APT Yeargroup report");
            explicitWait("reportHeader_Xpath");

            if (waitforElementPresent("AllYearLabel_Xpath").Equals(false))
            {
                reportFailure("APT reportTest has failed");
                test.Log(Status.Info,
                    "APT reportTest has failed because APT Year group report table is not shown ");
                takeScreenshot();

            }
            else
            {
                reportPass("MAT user can view APT Year group report for selected school");
            }
            Thread.Sleep(1000);


            Thread.Sleep(1000);
            click("APTClassesreportlink_Xpath");
            test.Log(Status.Info, "Checking APT Classes report");
            explicitWait("reportHeader_Xpath");

            if (waitforElementPresent("AllClassLabel_Xpath").Equals(false))
            {
                reportFailure("APT reportTest has failed");
                test.Log(Status.Info,
                    "APT reportTest has failed because APT Classes report table is not shown");
                takeScreenshot();

            }
            else
            {
                reportPass("MAT user can view APT Classes report for selected school");
            }
            Thread.Sleep(1000);


            click("APTpupilgrouplink_Xpath");
            test.Log(Status.Info, "Checking APT Pupilgroup report");
            explicitWait("reportHeader_Xpath");

            if (waitforElementPresent("AllpupilLabel1_Xpath").Equals(false) &
                   waitforElementPresent("AllpupilLabel2_Xpath").Equals(false))
            {
                reportFailure("APT reportTest has failed because APT Pupilgroup report table is not shown");
                test.Log(Status.Info,
                    "APT reportTest has failed because APT Pupilgroup report table is not shown");
                takeScreenshot();

            }
            else
            {
                reportPass("MAT user can view APT Pupil group report for selected school");
            }
            Thread.Sleep(1000);


            click("MySchoolgrouplink_Xpath");
            test.Log(Status.Info, "Checking APT Schoolgroup report");
            explicitWait("reportHeader_Xpath");

            if (waitforElementPresent("SchoolGroupLabel_Xpath").Equals(false))
            {
                reportFailure("APT reportTest has failed");
                test.Log(Status.Info,
                    "APT reportTest has failed because APT School group report table is not shown");
                takeScreenshot();

            }
            else
            {
                reportPass("MAT user can view APT School group report for selected school");
            }


        }

    }
}
