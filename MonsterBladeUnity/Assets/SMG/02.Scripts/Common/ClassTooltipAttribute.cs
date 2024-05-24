using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SMG.Utility
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class ClassTooltipAttribute : PropertyAttribute
    {
        public readonly string description;

        public ClassTooltipAttribute(string desc)
        {
            this.description = desc;
        }
    }
}



