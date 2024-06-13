using System.Reflection;
using UnityEngine;
using UnityEditor;

namespace AVT.FetchGoogleSheet
{
    [CustomPropertyDrawer(typeof(RegexAttribute))]
    public class RegexAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var regex = attribute as RegexAttribute;
            
            if (property.propertyType == SerializedPropertyType.String)
            {
                EditorGUI.BeginChangeCheck();
                var newValue = EditorGUI.TextField(position, label, property.stringValue);
                
                if (EditorGUI.EndChangeCheck() && regex != null)
                {
                    var pattern = regex.pattern;
                    var errorMsg = regex.errorMessage;

                    if (regex.dynamicPattern || regex.dynamicErrorMessage)
                    {
                        var owner = GetObjectOwner(property);

                        if (regex.dynamicPattern)
                        {
                            var dynamicValue = GetMemberValue<string>(owner, regex.pattern);
                            
                            if (dynamicValue != null)
                            {
                                pattern = dynamicValue;
                            }
                        }
                        
                        if (regex.dynamicErrorMessage)
                        {
                            var dynamicValue = GetMemberValue<string>(owner, regex.errorMessage);
                            if (dynamicValue != null)
                            {
                                errorMsg = dynamicValue;
                            }
                        }
                    }
                    
                    // Validate the input using the custom regex pattern
                    if (!System.Text.RegularExpressions.Regex.IsMatch(newValue, pattern))
                    {
                        // Invalid input; display the custom error message
                        Debug.LogWarning($"Field \"{property.displayName}\": {errorMsg}");
                    }
                    else
                    {
                        // Input is valid; update the property
                        property.stringValue = regex.useFirstMatch ? 
                            System.Text.RegularExpressions.Regex.Match(newValue, pattern).Value : 
                            newValue;
                    }
                }
            }
            else
                EditorGUI.LabelField(position, label.text, "Use Regex with string.");
        }

        private static T GetMemberValue<T>(object obj, string memberName)
        {
            var targetType = obj.GetType();

            // Try to get the field
            var field = targetType.GetField(memberName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (field != null)
            {
                return (T)field.GetValue(obj);
            }

            // Try to get the property
            var prop = targetType.GetProperty(memberName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (prop != null)
            {
                return (T)prop.GetValue(obj);
            }

            Debug.LogError($"Member '{memberName}' not found in type '{targetType}'");
            return default;
        }

        private static object GetObjectOwner(SerializedProperty property)
        {
            var obj = property.serializedObject.targetObject as object;
            
            var elements = property.propertyPath.Split('.');
            if (elements.Length <= 1) return obj;
            
            var objType = obj.GetType();
            var field = objType.GetField(elements[0]);
            obj = field.GetValue(obj);
                    
            for (var i = 1; i < elements.Length - 1; i++)
            {
                objType = obj.GetType();
                field = objType.GetField(elements[i]);
                obj = field.GetValue(obj);
            }

            return obj;
        }
    }
}