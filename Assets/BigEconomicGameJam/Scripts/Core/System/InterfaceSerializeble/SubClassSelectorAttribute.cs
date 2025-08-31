using System;
using UnityEngine;

namespace CORE
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class SubclassSelectorAttribute : PropertyAttribute
    {
        public SubclassSelectorAttribute() { }
    }
}