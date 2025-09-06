using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;

namespace CORE
{
    [CustomPropertyDrawer(typeof(SubclassSelectorAttribute))]
    public class SubclassSelectorDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.ManagedReference)
            {
                EditorGUI.LabelField(position, label.text, "Use [SubclassSelector] with [SerializeReference]");
                return;
            }

            EditorGUI.BeginProperty(position, label, property);

            var currentType = GetManagedReferenceType(property.managedReferenceFullTypename);
            var baseType = GetBaseTypeFromField(property);

            if (baseType == null)
            {
                EditorGUI.LabelField(position, label.text, "Cannot determine base type");
                return;
            }

            var types = GetSubclassTypes(baseType);
            var typeNames = types.Select(t => t.FullName).ToList();
            typeNames.Insert(0, "<None>");

            var dropdownPosition = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            int currentIndex = currentType != null ? types.IndexOf(currentType) + 1 : 0;
            int newIndex = EditorGUI.Popup(dropdownPosition, label.text, currentIndex,
                typeNames.Select(t => t.Split('.').Last()).ToArray());

            if (newIndex != currentIndex)
            {
                if (newIndex == 0)
                {
                    property.managedReferenceValue = null;
                }
                else
                {
                    var newType = types[newIndex - 1];
                    property.managedReferenceValue = Activator.CreateInstance(newType);
                }

                property.serializedObject.ApplyModifiedProperties();
            }

            if (property.managedReferenceValue != null)
            {
                SerializedProperty iterator = property.Copy();
                SerializedProperty endProperty = iterator.GetEndProperty();

                position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

                iterator.NextVisible(true);
                while (!SerializedProperty.EqualContents(iterator, endProperty))
                {
                    // ПРОВЕРЯЕМ ВИДИМОСТЬ ПОЛЯ
                    bool shouldDraw = ShouldDrawProperty(iterator);
            
                    if (shouldDraw)
                    {
                        position.height = EditorGUI.GetPropertyHeight(iterator, true);
                        EditorGUI.PropertyField(position, iterator, true);
                        position.y += position.height + EditorGUIUtility.standardVerticalSpacing;
                    }
            
                    iterator.NextVisible(false);
                }
            }

            EditorGUI.EndProperty();
        }

        private bool ShouldDrawProperty(SerializedProperty property)
        {
            var targetObject = property.serializedObject.targetObject;
            var field = targetObject.GetType().GetField(property.name, 
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
    
            if (field == null) return true;
    
            // Проверяем атрибуты HideIf
            var hideIfAttributes = field.GetCustomAttributes(typeof(HideIfAttribute), true);
            foreach (HideIfAttribute hideIf in hideIfAttributes)
            {
                // Используем рефлексию чтобы получить условие
                var conditionField = hideIf.GetType().GetField("condition", 
                    BindingFlags.Instance | BindingFlags.NonPublic);
                if (conditionField != null)
                {
                    string conditionName = (string)conditionField.GetValue(hideIf);
                    if (IsConditionMet(targetObject, conditionName))
                        return false;
                }
            }
    
            // Проверяем атрибуты ShowIf
            var showIfAttributes = field.GetCustomAttributes(typeof(ShowIfAttribute), true);
            foreach (ShowIfAttribute showIf in showIfAttributes)
            {
                var conditionField = showIf.GetType().GetField("condition", 
                    BindingFlags.Instance | BindingFlags.NonPublic);
                if (conditionField != null)
                {
                    string conditionName = (string)conditionField.GetValue(showIf);
                    if (!IsConditionMet(targetObject, conditionName))
                        return false;
                }
            }
    
            return true;
        }
        
        private bool IsConditionMet(object target, string conditionName)
        {
            var method = target.GetType().GetMethod(conditionName, 
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
    
            if (method != null && method.ReturnType == typeof(bool))
            {
                return (bool)method.Invoke(target, null);
            }
    
            var field = target.GetType().GetField(conditionName, 
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
    
            if (field != null && field.FieldType == typeof(bool))
            {
                return (bool)field.GetValue(target);
            }
    
            var property = target.GetType().GetProperty(conditionName, 
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
    
            if (property != null && property.PropertyType == typeof(bool))
            {
                return (bool)property.GetValue(target);
            }
    
            return false;
        }
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float totalHeight = EditorGUIUtility.singleLineHeight;

            if (property.managedReferenceValue != null)
            {
                SerializedProperty iterator = property.Copy();
                SerializedProperty endProperty = iterator.GetEndProperty();

                iterator.NextVisible(true);
                while (!SerializedProperty.EqualContents(iterator, endProperty))
                {
                    totalHeight += EditorGUI.GetPropertyHeight(iterator, true) +
                                   EditorGUIUtility.standardVerticalSpacing;
                    iterator.NextVisible(false);
                }
            }

            return totalHeight;
        }

        private Type GetBaseTypeFromField(SerializedProperty property)
        {
            if (property == null || property.serializedObject == null)
            {
                Debug.LogError("SerializedProperty or its serializedObject is null");
                return null;
            }

            var path = property.propertyPath.Split('.');
            Type currentType = property.serializedObject.targetObject.GetType();

            foreach (var fieldName in path)
            {
                if (fieldName == "Array") continue; 
                if (fieldName.StartsWith("data[")) continue; 

                var field = currentType.GetField(fieldName,
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                if (field == null)
                {
                    Debug.LogError($"Cannot find field '{fieldName}' in type '{currentType.FullName}'");
                    return null;
                }

                currentType = field.FieldType;

                if (currentType.IsArray)
                {
                    currentType = currentType.GetElementType();
                }
                else if (currentType.IsGenericType && currentType.GetGenericTypeDefinition() == typeof(List<>))
                {
                    currentType = currentType.GetGenericArguments()[0];
                }
            }

            return currentType;
        }

        private Type GetManagedReferenceType(string managedReferenceFullTypename)
        {
            if (string.IsNullOrEmpty(managedReferenceFullTypename))
                return null;

            var split = managedReferenceFullTypename.Split(' ');
            if (split.Length != 2)
                return null;

            var assemblyName = split[0];
            var typeName = split[1];

            return Type.GetType($"{typeName}, {assemblyName}");
        }

        private List<Type> GetSubclassTypes(Type baseType)
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => baseType.IsAssignableFrom(type) && !type.IsAbstract && type.IsClass)
                .ToList();
        }
    }
}