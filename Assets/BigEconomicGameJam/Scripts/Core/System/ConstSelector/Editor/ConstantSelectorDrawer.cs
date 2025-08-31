using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CORE.CONST_SELECTOR
{
    [CustomPropertyDrawer(typeof(Constant))]
    public class ConstantSelectorDrawer: StringAttributeDrawer
    {
        const string DEFAULT_CONSTANTS_ROOT = "Assets/Constants/SOConstantsContainer.asset";
    
        protected override Rect Init(Rect position, SerializedProperty property)
        {
            SOConstantsContainer cContainer =
                AssetDatabase.LoadAssetAtPath<SOConstantsContainer>(DEFAULT_CONSTANTS_ROOT);
           
            var attr = (Constant)attribute;
    
            if(Values != null)
            {
                Values.Clear();
            }
            else
            {
                Values = new List<string>();
            }
    
            AddItem("none");
            AddItem(cContainer.GetConsts(attr.ID));
    
            return base.Init(position, property);
        }
    
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var value = property.stringValue;
            
            position = Init(position, property);
            
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
    
            if(GUI.Button(position, value))
            {
    
                ConstSelectorWindow.Open((string res) =>
                    {
                        using(new EditorGUI.PropertyScope(position, label, property))
                        {
                            property.stringValue = res;
                            property.serializedObject.ApplyModifiedProperties();
                        }
                    },
                    value, Values);
            }
        }
    }
}