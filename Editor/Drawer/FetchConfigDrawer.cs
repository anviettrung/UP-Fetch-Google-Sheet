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
        private bool isFetching;

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
                var range = property.FindPropertyRelative("range");
                EditorGUILayout.PropertyField(range);
                var format = property.FindPropertyRelative("format");
                EditorGUILayout.PropertyField(format);
                
                var fetch = fieldInfo.GetCustomAttribute<FetchAttribute>();
                if (fetch != null)
                {
                    // Find the target object that contains the function
                    var targetObject = property.serializedObject.targetObject;
                    
                    // Use reflection to find and invoke the function
                    var methodInfo = targetObject.GetType().GetMethod(fetch.functionName);
                    if (isFetching)
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
                                isFetching = true;

                                var rangeValue = range.stringValue;
                                var formatValue = format.ToSheetFormat();
                                FetchGoogleSheetUtility.GetRawTextFromUrl(GetFetchUrl(source, format, gid), 
                                    (success, text) =>
                                    {
                                        isFetching = false;
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
                            LogWarningLabel($"Function \"{fetch.functionName}\" not has correct parameters!");
                        }
                    }
                    else
                    {
                        LogWarningLabel($"Function \"{fetch.functionName}\" not found!");
                    }
                }
                
                EditorGUI.indentLevel--;
                
                EditorGUILayout.EndVertical();
            }
            
            EditorGUILayout.EndFoldoutHeaderGroup();


            EditorGUI.EndProperty();
        }
        
        private static string GetFetchUrl(SerializedProperty source, SerializedProperty format, SerializedProperty gid)
            => $"{source.stringValue}?output={format.ToSheetFormat().GetName()}&single=true&gid={gid.stringValue}";

        private static bool CheckParameters(IReadOnlyList<ParameterInfo> para)
        {
            if (para.Count != 1)
                return false;
            
            return para[0].ParameterType == typeof(SheetTable);
        }

        // private static SheetFormat ToSheetFormat(SerializedProperty propFormat)
        // {
        //     return (SheetFormat)Enum.ToObject(typeof(SheetFormat), propFormat.enumValueIndex);
        // }
        //
        // private static string SheetFormatToString(SerializedProperty propFormat)
        // {
        //     var format = (SheetFormat)Enum.ToObject(typeof(SheetFormat), propFormat.enumValueIndex);
        //     return format switch
        //     {
        //         SheetFormat.TSV => "tsv",
        //         SheetFormat.CSV => "csv",
        //         _ => "csv"
        //     };
        // }

        private static void LogWarningLabel(string text)
        {
            GUI.color = Color.yellow;
            EditorGUILayout.LabelField(text);
            GUI.color = Color.white;
        }
    }
}
