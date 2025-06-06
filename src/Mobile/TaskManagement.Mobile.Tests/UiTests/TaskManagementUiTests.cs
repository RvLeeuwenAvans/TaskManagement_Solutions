using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Enums;

namespace TaskManagement.Mobile.Tests.UiTests;
// todo; streamline the database seeders
// todo write readme for the tests, UI tests only work when triggered with a working/seeded emulator
[TestFixture]
public class TaskManagementAndroidUiTests
{
    private AndroidDriver _driver;

    [OneTimeSetUp]
    public void SetUp()
    {
        // Set Appium Server URL
        var serverUri = new Uri(Environment.GetEnvironmentVariable("APPIUM_HOST") ?? "http://127.0.0.1:4723/");

        // Appium Options for the Android
        var driverOptions = new AppiumOptions()
        {
            PlatformName = "Android",
            AutomationName = AutomationName.AndroidUIAutomator2,
            DeviceName = "Android Emulator"
        };

        driverOptions.AddAdditionalAppiumOption("appPackage",
            "com.companyname.taskmanagement.mobileapp");
        driverOptions.AddAdditionalAppiumOption("appActivity", "MainActivity");
        driverOptions.AddAdditionalAppiumOption("noReset", true);

        _driver = new AndroidDriver(serverUri, driverOptions, TimeSpan.FromMinutes(3));
        _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        _driver.Dispose();
    }

    [Test]
    public void VerifyMainPageLoads()
    {
        // Assert main page loads by finding one of the filter tabs.
        var filterTab = _driver.FindElement(MobileBy.AndroidUIAutomator(
            "new UiSelector().text(\"Alle\")"));
        Assert.That(filterTab, Is.Not.Null, "Main Page Title element not found.");
        Assert.That(filterTab.Displayed, Is.True, "Main Page is not displayed properly.");
    }

    [Test]
    public void VerifyTaskCardsLoad()
    {
        // Assert that TaskCards load by finding one of the buttons on it.
        var closeTaskButton = _driver.FindElement(By.Id("CloseTaskButton"));

        Assert.That(closeTaskButton, Is.Not.Null, "Taskcard edit button element not found.");
        Assert.That(closeTaskButton.Displayed, Is.True, "Taskcard edit button is not displayed properly.");
    }

    [Test]
    public void VerifyTaskCardEditButtonNavigation()
    {
        // Find and click the "Add" button
        var editButton = _driver.FindElement(By.Id("EditTaskButton"));

        editButton.Click();
        // Verify navigation to add a Task page
        var editPageSubtitle = _driver.FindElement(MobileBy.AndroidUIAutomator(
            "new UiSelector().text(\"Bewerken\")"));

        Assert.That(editPageSubtitle, Is.Not.Null, "Navigation to Edit Task page failed.");
        Assert.That(editPageSubtitle.Displayed, Is.True, "Edit Task page is not displayed properly.");

        _driver.Navigate().Back();
    }

    [Test]
    public void VerifyAddButtonNavigation()
    {
        // Find and click the "Add" button
        var addButton = _driver.FindElement(MobileBy.AndroidUIAutomator(
            "new UiSelector().text(\"+\")"));
        addButton.Click();
        // Verify navigation to add a Task page
        var addTaskPageTitle = _driver.FindElement(MobileBy.AndroidUIAutomator(
            "new UiSelector().text(\"Nieuwe taak\")"));

        Assert.That(addTaskPageTitle, Is.Not.Null, "Navigation to Add Task page failed.");
        Assert.That(addTaskPageTitle.Displayed, Is.True, "Add Task page is not displayed properly.");

        _driver.Navigate().Back();
    }
}