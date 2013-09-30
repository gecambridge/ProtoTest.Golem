﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Golem.Framework;
using Golem.PageObjects.Cael.MyAccount;
using OpenQA.Selenium;

namespace Golem.PageObjects.Cael
{
    public class LoggedOutHeader : BasePageObject
    {
        public Element CreateAccount_Link = new Element("CreateAccount_Link", By.LinkText("Create Account"));
        public Element SignIn_Link = new Element("Sign In Link", By.LinkText("Sign In"));
        public Element Logo_Button = new Element("Logo", By.Id("logo"));

        public Element HowItWorks_Link = new Element("HowItWorks Link", By.LinkText("How it Works"));
        public Element TimeAndCost_Link = new Element("Time And Cost Link", By.LinkText("Time & Cost"));
        public Element AffiliatedUniversities_Link = new Element("Affiliated Universities Link", By.LinkText("Affiliated Universities"));
        public Element ContactLearningCounts_Link = new Element("Contact LearningCounts Button", By.LinkText("Contact LearningCounts"));

        public CreateUserPage ClickCreateAccount()
        {
            CreateAccount_Link.Click();
            return new CreateUserPage();
        }

        public LoginPage SignIn()
        {
            SignIn_Link.Click();
            return new LoginPage();
        }

        public override void WaitForElements()
        {
            CreateAccount_Link.VerifyVisible(30);
            SignIn_Link.VerifyVisible(30);
            HowItWorks_Link.VerifyVisible(30);
            TimeAndCost_Link.VerifyVisible(30);
            AffiliatedUniversities_Link.VerifyVisible(30);
            ContactLearningCounts_Link.VerifyVisible(30);
        }
    }
}
