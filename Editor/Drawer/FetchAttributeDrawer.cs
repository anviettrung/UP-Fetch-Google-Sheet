using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEditor;

namespace AVT.FetchGoogleSheet
{
    [CustomPropertyDrawer(typeof(FetchAttribute))]
    public class FetchAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // var fetch = attribute as FetchAttribute;
            // var targetObject = property.serializedObject.targetObject;
            //
            // var methodInfo = targetObject.GetType().GetMethod(fetch.targetName);
            // var config = (FetchConfig)fieldInfo.GetValue(targetObject);
            
            EditorGUI.LabelField(position, label);
            
            EditorGUILayout.PropertyField(property);
            
            // if (FetchGoogleSheetUtility.IsFetching)
            // {
            //     LogWarningLabel("Wait for fetching...");
            // }
            // else if (methodInfo != null)
            // {
            //     var parameters = methodInfo.GetParameters();
            //
            //     if (CheckParameters(parameters))
            //     {
            //         // Draw the button
            //         if (GUILayout.Button("Fetch"))
            //         {
            //             var formatValue = config.format;
            //             FetchGoogleSheetUtility.GetRawTextFromUrl(config.FetchUrl, 
            //                 (success, text) =>
            //                 {
            //                     if (!success)
            //                         return;
            //                     
            //                     var block = config.range.ToSheetRange().Validate();
            //                     var table = new SheetTable(text, formatValue).Trim(block);
            //                     var para = new object[1];
            //                     para[0] = table;
            //                     methodInfo.Invoke(targetObject, para);
            //                 });
            //         }
            //     }
            //     else
            //     {
            //         LogWarningLabel($"Function \"{fetch.targetName}\" not has correct parameters!");
            //     }
            // }
            // else
            // {
            //     LogWarningLabel($"Function \"{fetch.targetName}\" not found!");
            // }
            
            //EditorGUI.EndProperty();
        }

        private static bool CheckParameters(IReadOnlyList<ParameterInfo> para)
        {
            if (para.Count != 1)
                return false;
            
            return para[0].ParameterType == typeof(SheetTable);
        }

        private static void LogWarningLabel(string text)
        {
            GUI.color = Color.yellow;
            EditorGUILayout.LabelField(text);
            GUI.color = Color.white;
        }
    }
}
