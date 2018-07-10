using System;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Remote;

namespace EFC_UWP_Test
{
 [TestClass]
 public class UnitTests
 {

  /// <summary>
  /// 
  /// </summary>
  [TestMethod]
  public void TestMLL()
  {

   // You can find the Application Id of your application in the generated AppX\vs.appxrecipe file under RegisteredUserModeAppID node. E.g. c24c8163-548e-4b84-a466-530178fc0580_scyf5npe3hv32!App
   DesiredCapabilities appCapabilities = new DesiredCapabilities();
   appCapabilities.SetCapability("app", "1c0748ac-ce4c-4925-a3d3-e8cbd8d20db6_j0c0e5hxdtz1g!App");
   appCapabilities.SetCapability("deviceName", "Windows"); // Notwendig bei Start über Appium, nicht notwendig bei WinAppDriver.exe

   // Appium: http://127.0.0.1:4723/wd/hub
   // WinAppDriver.exe: http://127.0.0.1:4723
   using (var app = new WindowsDriver<WindowsElement>(new Uri("http://127.0.0.1:4723"), appCapabilities)) 
   {
    app.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(0.0);

    Assert.IsNotNull(app);
    Assert.IsNotNull(app.SessionId);

    var text1 = "Dies ist eine Aufgabe";
    var text2 = "Dies ist noch eine Aufgabe";
    // Use the session to control the app
    app.FindElementByName("Remove all").Click();

    var sc = app.GetScreenshot();
    sc.SaveAsFile("Screen1.png");

    app.FindElementByAccessibilityId("C_Task").Click();

    app.FindElementByAccessibilityId("C_Datum").Click();

    var Feb = app.FindElementByAccessibilityId("HeaderButton");
    Feb.Click();
    var Maerz = app.FindElementByName("März");
    Maerz.Click();
    var eins = app.FindElementByName("1");
    eins.Click();
    //app.FindElementByAccessibilityId("C_Datum").Click();

    app.FindElementByAccessibilityId("C_Task").SendKeys(text1);

    // https://github.com/Microsoft/WinAppDriver/blob/master/Tests/UWPControls/DatePicker.cs


    app.FindElementByName("Add").Click();

    app.FindElementByAccessibilityId("C_Task").SendKeys(text2);
    app.FindElementByName("Add").Click();

    var listenelemente = app.FindElements(By.XPath($"//ListItem"));
    Assert.AreEqual(2, listenelemente.Count);
    var e = listenelemente[0];
    Assert.AreEqual(e.Text, "EFC_UWP.DAL.Task");
    // app.FindElements(By.XPath($"//Button"))

    string b = app.FindElementByAccessibilityId("C_Details").Text;
    //Assert.AreEqual(System.DateTime.Now.ToShortDateString() + ": " + text1, b);
    Assert.AreEqual("01.03." + System.DateTime.Now.Year + ": " + text1, b);
    var alleAufgaben = app.FindElements(By.XPath($"//*[contains(@Name, \"Aufgabe\")]"));
    Assert.IsTrue(alleAufgaben[0].Text.Contains(text1));
    Assert.IsTrue(alleAufgaben[1].Text.Contains(text2));

    // geht so nicht, weil die Liste sich durch "Done" verändert
    //var alleAufgabenDoneButtons = app.FindElements(By.XPath($"//*[contains(@Name, \"Done\")]"));
    //foreach (var done in alleAufgabenDoneButtons.Reverse().ToList())
    //{
    // done.Click();
    //}

    ReadOnlyCollection<WindowsElement> alleAufgabenDoneButtons;
    do
    {
     alleAufgabenDoneButtons = app.FindElements(By.XPath($"//*[contains(@Name, \"Done\")]"));
     alleAufgabenDoneButtons[0].Click();
    } while (alleAufgabenDoneButtons.Count > 1);


    var listenelemente2 = app.FindElements(By.XPath($"//ListItem"));
    Assert.AreEqual(0, listenelemente2.Count);

    app.CloseApp();
   }
  }

  
  [TestMethod]
  public void TestNotepad()
  {
   // Launch Notepad
   DesiredCapabilities appCapabilities = new DesiredCapabilities();
  appCapabilities.SetCapability("app", @"C:\Windows\System32\notepad.exe");
appCapabilities.SetCapability("appArguments", @"MyTestFile.txt");
appCapabilities.SetCapability("appWorkingDir", @"C:\temp\");
var NotepadSession = new WindowsDriver<WindowsElement>(new Uri("http://127.0.0.1:4723"), appCapabilities);

// Use the session to control the app
NotepadSession.FindElementByClassName("Edit").SendKeys("This is some text");
 }

  /// <summary>
  /// Region für Apps auf "united kingdom" stellen!!!
  /// </summary>
  [TestMethod]
  public void WindowsAlarm()
  {

   // Launch the Alarms &Clock app
   DesiredCapabilities appCapabilities = new DesiredCapabilities();
   appCapabilities.SetCapability("app", "Microsoft.WindowsAlarms_8wekyb3d8bbwe!App");
  var AlarmClockSession = new WindowsDriver<WindowsElement>(new Uri("http://127.0.0.1:4723"), appCapabilities);

   // Use the session to control the app
   AlarmClockSession.FindElementByAccessibilityId("AddAlarmButton").Click();
   AlarmClockSession.FindElementByAccessibilityId("AlarmNameTextBox").Clear();

  }
 }
}
