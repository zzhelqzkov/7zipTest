using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Support.UI;
using System;
using System.IO;
using System.Linq;
using System.Threading;

namespace AppiumExercises
{
    [TestFixture]
    public class Tests
    {
        private const string AppiumServerUrl = "http://[::1]:4723/wd/hub";
        private WindowsDriver<WindowsElement> driver;
        private WindowsDriver<WindowsElement> desktopDriver;
        private string pathToProgram;
        private string workDir;

        [OneTimeSetUp]
        public void Setup()
        {
            pathToProgram = @"C:\Program Files\7-Zip";
            var appiumOptions = new AppiumOptions() { PlatformName = "Windows" };
            appiumOptions.AddAdditionalCapability("app", pathToProgram + @"\7zFM.exe");
            driver = new WindowsDriver<WindowsElement>(new Uri(AppiumServerUrl), appiumOptions);

            var rootCapabilities = new AppiumOptions() { PlatformName = "Windows" };
            rootCapabilities.AddAdditionalCapability("app", "Root");
            desktopDriver = new WindowsDriver<WindowsElement>(new Uri(AppiumServerUrl), rootCapabilities);
            workDir = Directory.GetCurrentDirectory() + @"\workdir";
        }

        [Test]
        public void Archive7z()
        {
            var directory = driver.FindElementByXPath("/Window/Pane/Pane/ComboBox/Edit");
            directory.SendKeys(pathToProgram);
            directory.SendKeys(Keys.Enter);

            var directoryList = driver.FindElementByAccessibilityId("1001");
            directoryList.SendKeys(Keys.Control + "a");

            var buttonAddToArchive = driver.FindElementByName("Add");
            buttonAddToArchive.Click();

            Thread.Sleep(500);
            var addToArchiveWindow = desktopDriver.FindElementByName("Add to Archive");

            var archiveTextField = addToArchiveWindow.FindElementByXPath("Window/ComboBox/Edit[@Name='Archive:']");
            var fileName = DateTime.Now.Ticks + ".7z";
            var archiveDestinationFolder = workDir + @"\" + fileName;
            archiveTextField.SendKeys(archiveDestinationFolder);

            var fieldArchiveFormat = addToArchiveWindow.FindElementByXPath("/Window/ComboBox[@Name='Archive format:']");
            fieldArchiveFormat.SendKeys("7z");

            var fieldCompressionLevel = addToArchiveWindow.FindElementByXPath("/Window/ComboBox[@Name='Compression level:']");
            fieldCompressionLevel.SendKeys("Ultra");
            
            var fieldCompressionMethod = addToArchiveWindow.FindElementByXPath("/Window/ComboBox[@Name='Compression method:']");
            fieldCompressionMethod.SendKeys("LZMA2");            
            
            var fieldDictionarySize = addToArchiveWindow.FindElementByXPath("/Window/ComboBox[@Name='Dictionary size:']");
            fieldDictionarySize.SendKeys(Keys.End);            
            
            var fieldWordSize = addToArchiveWindow.FindElementByXPath("/Window/ComboBox[@Name='Word size:']");
            fieldWordSize.SendKeys(Keys.End);

            var buttonOk = addToArchiveWindow.FindElementByXPath("Window/Button[@Name='OK']");
            buttonOk.Click();

            Thread.Sleep(1000);
            var filesInWorkdir = Directory.GetFiles(workDir);
            
            bool fileIsCreated = false;
            
            foreach(string file in filesInWorkdir)
            {
                if (file.Contains(fileName))
                {
                  fileIsCreated = true;
                }
            }

            Assert.IsTrue(fileIsCreated);

            directory.SendKeys(archiveDestinationFolder + Keys.Enter);

            var buttonExtract = driver.FindElementByName("Extract");
            buttonExtract.Click();

            var buttonExtractFormOk = driver.FindElementByName("OK");
            buttonExtractFormOk.Click();

            string executable7ZipOriginal = @"C:\Program Files\7-Zip\7zFM.exe";
            string executable7ZipExtracted = workDir + @"\7zFM.exe";
            Thread.Sleep(300);
            FileAssert.AreEqual(executable7ZipOriginal, executable7ZipExtracted);

        }

        [OneTimeTearDown]
        public void TearDown()
        {
            Directory.Delete(workDir, true);
            driver.Quit();
            desktopDriver.Quit();
        }
    }
}