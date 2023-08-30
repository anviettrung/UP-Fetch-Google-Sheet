using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AVT.FetchGoogleSheet
{
    [CustomPropertyDrawer(typeof(FetchConfig))]
    public class FetchConfigDrawer : PropertyDrawer
    {
        private bool foldout = true;
        private bool hasFetch;
        
        private static readonly float slh = EditorGUIUtility.singleLineHeight;
        private static readonly float svs = EditorGUIUtility.standardVerticalSpacing;
        private static readonly GUIStyle boldFoldoutStyle = new GUIStyle(EditorStyles.foldout)
        {
            fontStyle = FontStyle.Bold
        };
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            DrawProperty(position, property, label);
            EditorGUI.EndProperty();
        }

        private Rect DrawProperty(Rect position, SerializedProperty property, GUIContent label)
        {
            var contentRect = new Rect(position.x, position.y, position.width, slh);
            var boxRect = new Rect(position.x, position.y, 
                position.width, GetPropertyHeight(property, label));
            GUI.Box(boxRect, GUIContent.none, GUI.skin.window);

            contentRect.x += 20;
            contentRect.width -= 25;
            foldout = EditorGUI.Foldout(contentRect, foldout, label, true, boldFoldoutStyle);
            
            if (foldout)
            {
                EditorGUI.indentLevel++;
                
                // Calculate rect for the property fields
                contentRect.y += slh;
                
                // Draw the property fields
                var source = property.FindPropertyRelative("source");
                EditorGUI.PropertyField(contentRect, source);

                contentRect.y += slh + svs;
                var gid = property.FindPropertyRelative("gid");
                EditorGUI.PropertyField(contentRect, gid);

                contentRect.y += slh + svs;
                var format = property.FindPropertyRelative("format");
                EditorGUI.PropertyField(contentRect, format);

                contentRect.y += slh + svs;
                var hasHeader = property.FindPropertyRelative("hasHeader");
                EditorGUI.PropertyField(contentRect, hasHeader);

                contentRect.y += slh + svs;
                var range = property.FindPropertyRelative("range");
                EditorGUI.PropertyField(contentRect, range);
                
                DrawAttribute(contentRect, property, label);
                
                EditorGUI.indentLevel--;
            }

            return contentRect;
        }

        private Rect DrawAttribute(Rect position, SerializedProperty property, GUIContent label)
        {
            var contentRect = new Rect(position.x, position.y, position.width, slh);
            
            hasFetch = false;
            var fetch = fieldInfo.GetCustomAttribute<FetchAttribute>();
            if (fetch == null) return contentRect;
            
            hasFetch = true;
            contentRect.y += slh + svs;
            
            if (FetchGoogleSheetUtility.IsFetching)
            {
                LogWarningLabel(contentRect, "Wait for fetching...");
            }
            else 
            {
                switch (fetch.targetType)
                {
                    case FetchAttribute.TargetType.METHOD:
                        DrawFetchButtonWithMethod(contentRect, property, fetch);
                        break;
                    case FetchAttribute.TargetType.PROPERTY:
                        DrawFetchButtonWithProperty(contentRect, property, fetch);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            
            return contentRect;
        }
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var height = slh + svs;
            if (foldout)
            {
                height += (slh + svs) * 5; // 5 property fields
                height += hasFetch ? slh + svs : 0;
            }
            
            return height;
        }

        #region Fetch Attribute

        private void DrawFetchButtonWithMethod(Rect position, SerializedProperty property, FetchAttribute fetch)
        {
            var targetObject = property.serializedObject.targetObject;
            var config = (FetchConfig)fieldInfo.GetValue(targetObject);
            var method = targetObject.GetType().GetMethod(fetch.targetName);

            if (method == null)
            {
                LogWarningLabel(position, $"Method \"{fetch.targetName}\" not found!");
                return;
            }

            var parameters = method.GetParameters();
            var methodTypeId = CheckMethodParameters(parameters);
            if (methodTypeId < 0)
            {
                LogWarningLabel(position, string.Format(LogById(methodTypeId), fetch.targetName));
                return;
            }
            
            // Draw the button
            if (GUI.Button(position, "Fetch"))
            {
                FetchGoogleSheetUtility.GetRawTextFromUrl(config.FetchUrl, 
                    (success, text) =>
                    {
                        if (!success)
                            return;
                            
                        var block = config.range.ToSheetRange().Validate();
                        var table = new SheetTable(text, config).Trim(block);

                        object[] para;
                        switch (methodTypeId)
                        {
                            case 0:
                                method.Invoke(targetObject, null);
                                break;
                            case 1:
                                para = new object[1];
                                para[0] = table;
                                method.Invoke(targetObject, para);
                                break;
                            case 2:
                                para = new object[2];
                                para[0] = table;
                                para[1] = config;
                                method.Invoke(targetObject, para);
                                break;
                        }
                    });
            }
        }

        private void DrawFetchButtonWithProperty(Rect position, SerializedProperty property, FetchAttribute fetch)
        {
            var targetObject = property.serializedObject.targetObject;
            var config = (FetchConfig)fieldInfo.GetValue(targetObject);
            var field = targetObject.GetType().GetField(fetch.targetName);
            
            if (field == null)
            {
                LogWarningLabel(position, $"Field \"{fetch.targetName}\" not found!");
                return;
            }
            
            // Draw the button
            if (GUI.Button(position, "Fetch"))
            {
                FetchGoogleSheetUtility.GetRawTextFromUrl(config.FetchUrl, 
                    (success, text) =>
                    {
                        if (!success)
                            return;
                            
                        var block = config.range.ToSheetRange().Validate();
                        var table = new SheetTable(text, config).Trim(block);
                        //FetchGoogleSheet.SheetTableToList(table, field);
                    });
            }
        }
        
        private static int CheckMethodParameters(IReadOnlyList<ParameterInfo> para)
        {
            return para.Count switch
            {
                0 => 0,
                1 => para[0].ParameterType == typeof(SheetTable) ? 1 : -1,
                2 => para[0].ParameterType == typeof(SheetTable) 
                && para[1].ParameterType == typeof(FetchConfig) ? 2 : -1,
                _ => -1
            };
        }

        private static bool CheckField(FieldInfo field)
        {
            return true;
        }

        #endregion

        #region Utils

        private static string LogById(int errorId)
        {
            return errorId switch
            {
                -1 => "Method \"{0}\" has incorrect parameters!",
                _ => "None"
            };
        }
        
        private static void LogWarningLabel(Rect position, string text)
        {
            GUI.color = Color.yellow;
            EditorGUI.LabelField(position, text);
            GUI.color = Color.white;
        }

        #endregion
    }
}
