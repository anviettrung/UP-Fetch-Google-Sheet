using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace AVT.FetchGoogleSheet
{
    [CustomPropertyDrawer(typeof(FetchConfig))]
    public class FetchConfigDrawer : PropertyDrawer
    {
        private bool foldout = true;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            
            // Draw the foldout label for FetchConfig
            foldout = EditorGUILayout.BeginFoldoutHeaderGroup(foldout, label);
            
            if (foldout)
            {
                EditorGUILayout.BeginVertical(GUI.skin.box);
                
                EditorGUI.indentLevel++;
                
                // Draw the property fields
                var source = property.FindPropertyRelative("source");
                EditorGUILayout.PropertyField(source);
                
                var gid = property.FindPropertyRelative("gid");
                EditorGUILayout.PropertyField(gid);
                
                var format = property.FindPropertyRelative("format");
                EditorGUILayout.PropertyField(format);
                
                var hasHeader = property.FindPropertyRelative("hasHeader");
                EditorGUILayout.PropertyField(hasHeader);
                
                var range = property.FindPropertyRelative("range");
                EditorGUILayout.PropertyField(range);
                var rangeValue = range.stringValue;
                
                var fetch = fieldInfo.GetCustomAttribute<FetchAttribute>();
                if (fetch != null)
                {
                    // Find the target object that contains the function
                    var targetObject = property.serializedObject.targetObject;
                    
                    // Use reflection to find and invoke the function
                    var methodInfo = targetObject.GetType().GetMethod(fetch.targetName);
                    if (FetchGoogleSheetUtility.IsFetching)
                    {
                        LogWarningLabel("Wait for fetching...");
                    }
                    else if (methodInfo != null)
                    {
                        var parameters = methodInfo.GetParameters();

                        if (CheckParameters(parameters))
                        {
                            // Draw the button
                            if (GUILayout.Button("Fetch"))
                            {
                                var formatValue = format.ToSheetFormat();
                                FetchGoogleSheetUtility.GetRawTextFromUrl(GetFetchUrl(source.stringValue, format, gid.stringValue), 
                                    (success, text) =>
                                    {
                                        if (!success)
                                            return;
                                
                                        var block = rangeValue.ToSheetRange().Validate();
                                        var table = new SheetTable(text, formatValue).Trim(block);
                                        var para = new object[1];
                                        para[0] = table;
                                        methodInfo.Invoke(targetObject, para);
                                    });
                            }
                        }
                        else
                        {
                            LogWarningLabel($"Function \"{fetch.targetName}\" not has correct parameters!");
                        }
                    }
                    else
                    {
                        LogWarningLabel($"Function \"{fetch.targetName}\" not found!");
                    }
                }
                
                EditorGUI.indentLevel--;
                
                EditorGUILayout.EndVertical();
            }
            
            EditorGUILayout.EndFoldoutHeaderGroup();


            EditorGUI.EndProperty();
        }
        
        private static string GetFetchUrl(string source, SerializedProperty format, string gid)
            => $"{source}?output={format.ToSheetFormat().GetName()}&single=true&gid={gid}";

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
