﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Automation;

namespace ProtoTest.Golem.White.PurpleElements
{
    public class PurpleCheckBox : PurpleElementBase
    {
        //TODO: Finish this shit.
        public bool Checked { get; set; }

        public PurpleCheckBox(string name, string locatorPath) : base(name, locatorPath)
        {
        }

        private bool IsElementToggledOn()
        {
            return true;
        }

        public void Check()
        {
            if (IsElementToggledOn())
            {
                
            }
            else
            {
                
            }
        }
    }
}
