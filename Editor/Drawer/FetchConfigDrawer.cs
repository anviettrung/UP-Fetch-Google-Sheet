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
            
            var targetObject = property.serializedObject.targetObject;
            var config = (FetchConfig)fieldInfo.GetValue(targetObject);
            var methodInfo = targetObject.GetType().GetMethod(fetch.targetName);
            
            hasFetch = true;
            contentRect.y += slh + svs;
            
            if (FetchGoogleSheetUtility.IsFetching)
            {
                LogWarningLabel(contentRect, "Wait for fetching...");
            }
            else if (methodInfo != null)
            {
                var parameters = methodInfo.GetParameters();
            
                if (CheckParameters(parameters))
                {
                    // Draw the button
                    
                    if (GUI.Button(contentRect, "Fetch"))
                    {
                        var formatValue = config.format;
                        FetchGoogleSheetUtility.GetRawTextFromUrl(config.FetchUrl, 
                            (success, text) =>
                            {
                                if (!success)
                                    return;
                                
                                var block = config.range.ToSheetRange().Validate();
                                var table = new SheetTable(text, formatValue).Trim(block);
                                var para = new object[1];
                                para[0] = table;
                                methodInfo.Invoke(targetObject, para);
                            });
                    }
                }
                else
                {
                    LogWarningLabel(contentRect, $"Function \"{fetch.targetName}\" not has correct parameters!");
                }
            }
            else
            {
                LogWarningLabel(contentRect, $"Function \"{fetch.targetName}\" not found!");
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

        #region Utils

        private static bool CheckParameters(IReadOnlyList<ParameterInfo> para)
        {
            if (para.Count != 1)
                return false;
            
            return para[0].ParameterType == typeof(SheetTable);
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
