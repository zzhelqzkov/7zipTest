# 7zipTest
Appium Automated Tests for 7-Zip Windows App


1. Implement Appium UI automated tests for the 7-zip file archiver.
   The target app version for testing is 7Zip version 19.00. Download it from: https://www.7-zip.org.
 
2. To implement the automated tests, use C#, NUnit, Appium and WinAppDriver.
   7-Zip Testing Scenario. Implement the following testing scenario: 
   a. Create a 7-Zip archive, holding all the files from the 7-Zip program folder with LZMA2 maximum compression level, 
   b. Extract the archive and compare the original and the extracted files.
   c. Start 7-Zip and navigate to folder “C:\Program Files\7-Zip\”:
   d. Select all files from this folder using [Ctrl+A] and click the [Add] button to create a new 7-Zip archive.
   e. Select the “maximum compression” settings, as shown below (7z file format, Ultra compression level, LZMA2 algorithm, dictionary size 1536MB, word size 273):
   f. Choose a temporary folder for the 7-Zip archive, e. g. c:\temp. The output may look like this:
 
Now, extract the archive into the same temporary folder:
 
Finally, compare these two files:
•	C:\Program Files\7-Zip\7zFM.exe\7zFM.exe
•	C:\temp\7zFM.exe
They should be the same (same file size and same content).
