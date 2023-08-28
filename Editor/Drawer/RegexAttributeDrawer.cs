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
                    // Validate the input using the custom regex pattern
                    if (!System.Text.RegularExpressions.Regex.IsMatch(newValue, regex.pattern))
                    {
                        // Invalid input; display the custom error message
                        Debug.LogWarning($"Field \"{property.displayName}\": {regex.errorMessage}");
                    }
                    else
                    {
                        // Input is valid; update the property
                        property.stringValue = regex.useFirstMatch ? 
                            System.Text.RegularExpressions.Regex.Match(newValue, regex.pattern).Value : 
                            newValue;
                    }
                }
            }
            else
                EditorGUI.LabelField(position, label.text, "Use Regex with string.");
        }
    }
}