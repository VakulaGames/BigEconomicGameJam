using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CORE.CONST_SELECTOR
{
    public class StringAttributeDrawer: PropertyDrawer
    {
        protected List<string> values = null;

        protected List<string> Values
        {
            get
            {
                return values;
            }

            set
            {
                values = value;
            }
        }

        protected void AddItem(string value)
        {
            Values.Add(value);
        }

        protected void AddItem(IEnumerable<string> values)
        {
            Values.AddRange(values);
        }

        protected virtual Rect Init(Rect position, SerializedProperty property)
        {
            return position;
        }
        
        protected virtual void CreateArrays(int count)
        {
            Values = new List<string>(count);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if(property == null)
            {
                return;
            }

            EditorGUI.BeginProperty(position, label, property);

            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            position = Init(position, property);

            if(Values.Count == 0)
            {
                return;
            }

            int indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            Rect pathRect = new Rect(position.x + 0 * position.width / 2 + 0 * 4, position.y, position.width - 6, position.height);

            string stringValue = property.stringValue;

            int intValue = Values.IndexOf(stringValue);
            intValue = Mathf.Clamp(EditorGUI.Popup(pathRect, intValue, Values.ToArray()), 0, Values.Count - 1);
            property.stringValue = Values[intValue];

            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }
    }
}