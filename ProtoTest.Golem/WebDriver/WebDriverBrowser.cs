﻿using System;
using System.Configuration;
using Gallio.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Safari;
using ProtoTest.Golem.Core;

namespace ProtoTest.Golem.WebDriver
{
    /// <summary>
    ///     Contains all functionality relating to launching the webdriver browsers.
    /// </summary>
    public class WebDriverBrowser
    {
        public enum Browser
        {
            Firefox,
            Chrome,
            IE,
            Safari,
            PhantomJS,
            Android,
            IPhone,
            IPad,
            None
        }

        public IWebDriver driver;

        public static Browser getBrowserFromString(string name)
        {
            return (Browser) Enum.Parse(typeof (Browser), name);
        }

        public IWebDriver LaunchBrowser(Browser browser)
        {
            switch (browser)
            {
                case Browser.IE:
                    driver = StartIEBrowser();
                    break;
                case Browser.Chrome:
                    driver = StartChromeBrowser();
                    break;
                case Browser.Safari:
                    driver = StartSafariBrowser();
                    break;
                case Browser.PhantomJS:
                    driver = StartPhantomJSBrowser();
                    break;
                default:
                    driver = StartFirefoxBrowser();
                    break;
            }

            driver.Manage().Cookies.DeleteAllCookies();
            SetBrowserSize();
            var eDriver = new EventedWebDriver(driver);

            return eDriver.driver;
        }

        private void SetBrowserSize()
        {
            var resolution = Config.Settings.runTimeSettings.BrowserResolution;
            if (resolution.Contains("Default"))
            {
                driver.Manage().Window.Maximize();
            }
            else
            {
                driver.Manage().Window.Size = Common.GetSizeFromResolution(resolution);
            }
        }

        public IWebDriver StartFirefoxBrowser()
        {
            var capabilities = new DesiredCapabilities();
            var proxy = new OpenQA.Selenium.Proxy();
            if (Config.Settings.httpProxy.useProxy)
            {
                proxy.HttpProxy = Config.Settings.httpProxy.proxyUrl + ":" + TestBase.proxy.proxyPort;
                proxy.SslProxy = Config.Settings.httpProxy.proxyUrl + ":" + TestBase.proxy.proxyPort;
                proxy.FtpProxy = Config.Settings.httpProxy.proxyUrl + ":" + TestBase.proxy.proxyPort;
                capabilities.SetCapability("proxy", proxy);
            }

            return new FirefoxDriver(capabilities);
        }

        public IWebDriver StartChromeBrowser()
        {
            var options = new ChromeOptions();

            // Add the WebDriver proxy capability.
            if (Config.Settings.httpProxy.useProxy)
            {
                var proxy = new OpenQA.Selenium.Proxy();
                proxy.HttpProxy = Config.Settings.httpProxy.proxyUrl + ":" + TestBase.proxy.proxyPort;
                proxy.SslProxy = Config.Settings.httpProxy.proxyUrl + ":" + TestBase.proxy.proxyPort;
                proxy.FtpProxy = Config.Settings.httpProxy.proxyUrl + ":" + TestBase.proxy.proxyPort;
                options.Proxy = proxy;
            }

            return new ChromeDriver(options);
        }

        public IWebDriver StartIEBrowser()
        {
            var options = new InternetExplorerOptions();
            options.IntroduceInstabilityByIgnoringProtectedModeSettings = true;
            options.IgnoreZoomLevel = true;
            if (Config.Settings.httpProxy.useProxy)
            {
                var proxy = new OpenQA.Selenium.Proxy();
                proxy.HttpProxy = Config.Settings.httpProxy.proxyUrl + ":" + TestBase.proxy.proxyPort;
                proxy.SslProxy = Config.Settings.httpProxy.proxyUrl + ":" + TestBase.proxy.proxyPort;
                proxy.FtpProxy = Config.Settings.httpProxy.proxyUrl + ":" + TestBase.proxy.proxyPort;
                options.Proxy = proxy;
                options.UsePerProcessProxy = true;
            }

            return new InternetExplorerDriver(options);
        }

        public IWebDriver StartPhantomJSBrowser()
        {
            var options = new PhantomJSOptions();

            if (Config.Settings.httpProxy.useProxy)
            {
                var proxy = new OpenQA.Selenium.Proxy();
                proxy.HttpProxy = Config.Settings.httpProxy.proxyUrl + ":" + TestBase.proxy.proxyPort;
                proxy.SslProxy = Config.Settings.httpProxy.proxyUrl + ":" + TestBase.proxy.proxyPort;
                proxy.FtpProxy = Config.Settings.httpProxy.proxyUrl + ":" + TestBase.proxy.proxyPort;
                options.AddAdditionalCapability("proxy", proxy);
            }

            return new PhantomJSDriver(options);
        }

        public IWebDriver StartSafariBrowser()
        {
            var options = new SafariOptions();

            // Add the WebDriver proxy capability.
            if (Config.Settings.httpProxy.useProxy)
            {
                var proxy = new OpenQA.Selenium.Proxy();
                proxy.HttpProxy = Config.Settings.httpProxy.proxyUrl + ":" + TestBase.proxy.proxyPort;
                proxy.SslProxy = Config.Settings.httpProxy.proxyUrl + ":" + TestBase.proxy.proxyPort;
                proxy.FtpProxy = Config.Settings.httpProxy.proxyUrl + ":" + TestBase.proxy.proxyPort;
                options.AddAdditionalCapability("proxy", proxy);
            }

            return new SafariDriver(options);
        }

        public DesiredCapabilities GetCapabilitiesForBrowser(Browser browser)
        {
            switch (browser)
            {
                case Browser.PhantomJS:
                    return DesiredCapabilities.PhantomJS();
                case Browser.IE:
                    return DesiredCapabilities.InternetExplorer();
                case Browser.Chrome:
                    return DesiredCapabilities.Chrome();
                case Browser.Safari:
                    return DesiredCapabilities.Safari();
                case Browser.Android:
                    return DesiredCapabilities.Android();
                case Browser.IPhone:
                    var capabilities = DesiredCapabilities.IPhone();
                    capabilities.SetCapability("device", "iphone");
                    capabilities.SetCapability("app", "safari");
                    return capabilities;
                case Browser.IPad:
                    return DesiredCapabilities.IPad();
                case Browser.Firefox:
                default:
                    return DesiredCapabilities.Firefox();
            }
        }

        public IWebDriver LaunchRemoteBrowser(Browser browser, string host)
        {
            String URIStr = null;

            if (Config.Settings.sauceLabsSettings.UseSauceLabs)
            {
                
                WebDriverTestBase.testData.browserInfo.capabilities.SetCapability("username", Config.Settings.sauceLabsSettings.SauceLabsUsername);
                WebDriverTestBase.testData.browserInfo.capabilities.SetCapability("accessKey", Config.Settings.sauceLabsSettings.SauceLabsAPIKey);
                WebDriverTestBase.testData.browserInfo.capabilities.SetCapability("name", TestContext.CurrentContext.TestStep.FullName);
                Common.Log(string.Format("Starting {0}:{1} browser on SauceLabs : {2}", browser,
                    WebDriverTestBase.testData.browserInfo.capabilities,
                    Config.Settings.sauceLabsSettings.SauceLabsUrl));
                var sauceLabs = new Uri(Config.Settings.sauceLabsSettings.SauceLabsUrl);
                return new EventedWebDriver(new RemoteWebDriver(sauceLabs, WebDriverTestBase.testData.browserInfo.capabilities)).driver;
            }

            if (host.Equals(Config.Settings.browserStackSettings.BrowserStackRemoteURL))
            {
                var user = Config.Settings.browserStackSettings.BrowserStack_User;
                var key = Config.Settings.browserStackSettings.BrowserStack_Key;
                var os = Config.Settings.browserStackSettings.BrowserStack_OS;
                var os_version = Config.Settings.browserStackSettings.BrowserStack_OS_Version;

                if (user == null)
                {
                    throw new ConfigurationErrorsException(
                        "Framework configured to use BrowserStack, however 'BrowserStack_User' is not defined in App.config");
                }
                if (key == null)
                {
                    throw new ConfigurationErrorsException(
                        "Framework configured to use BrowserStack, however 'BrowserStack_Key' is not defined in App.config");
                }
                if (os == null)
                {
                    throw new ConfigurationErrorsException(
                        "Framework configured to use BrowserStack, however 'BrowserStack_OS' is not defined in App.config");
                }
                if (os_version == null)
                {
                    throw new ConfigurationErrorsException(
                        "Framework configured to use BrowserStack, however 'BrowserStack_OS_Version' is not defined in App.config");
                }

                // Browser stack does not require a remote host port
                WebDriverTestBase.testData.browserInfo.capabilities.SetCapability("browserstack.user", user);
                WebDriverTestBase.testData.browserInfo.capabilities.SetCapability("browserstack.key", key);
                WebDriverTestBase.testData.browserInfo.capabilities.SetCapability("os", os);
                WebDriverTestBase.testData.browserInfo.capabilities.SetCapability("os_version", os_version);

                URIStr = string.Format("http://{0}/wd/hub", host);
                Common.Log(string.Format("Starting {0} browser on host : {1}", browser, host));
            }
            else
            {
                URIStr = string.Format("http://{0}:{1}/wd/hub", host, Config.Settings.runTimeSettings.RemoteHostPort);
                Common.Log(string.Format("Starting {0} browser on host : {1}:{2}", browser, host,
                    Config.Settings.runTimeSettings.RemoteHostPort));
            }

            var remoteAddress = new Uri(URIStr);
            return new EventedWebDriver(new ScreenshotRemoteWebDriver(remoteAddress, WebDriverTestBase.testData.browserInfo.capabilities)).driver;
        }
    }
}