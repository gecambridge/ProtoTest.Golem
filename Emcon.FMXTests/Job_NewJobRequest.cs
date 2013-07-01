﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Golem.Framework;
using Golem.PageObjects.Emcon.FMX;
using MbUnit.Framework;
using OpenQA.Selenium;
namespace Emcon.FMXTests
{
    class Job_NewJobRequest : TestBaseClass
    {
        private string envUrl = Config.GetConfigValue("EnvironmentUrl", "http://demo.fmx.bz/");
        [Test]
        [TestsOn("5 - Job Requests")]
        public void JobRequestSearch()
        {
            FmXwelcomePage.OpenFMX(envUrl)
                          .Login("PROTOTEST", "!TEST1234")
                          .NewJobRequest()
                          .CustomerSearch("EMCON TEST")
                          .EnterJobRequestInfo("Repair and Maintenance", "Blue Team", "Store", "ProtoTest Automation",
                                               "webDriver",
                                               "Here is a short Description")
                          .AddNewLocation()
                          .AddNewJob("Quoted", "Standard", "PROTOTEST-0001",
                                     "This is a job created by an automated script")
                          .AddWorkScopes("Doors", "Doors-Metal", "Here's a work scope Description")
                          .AddVendor("Doors")
                          .ClickBidRequests()
                          .CreateNewBidRequest("10", "13.99")
                          .ClickProposalsTab()
                          .CreateNewProposal("surban@prototest.com", "ApprovedBy Name")
                          .ClickWorkOrdersTab()
                          .AddNewWorkOrder("surban@prototest.com", "BoSSFName", "BoSSLName",
                                           "This was created automatically by Prototest");
        }
    }
}