// using UnityEditor;
// using UnityEngine;
//
// namespace AVT.FetchGoogleSheet
// {
//     [CustomPropertyDrawer(typeof(FetchConfig))]
//     public class FetchConfigDrawer : PropertyDrawer
//     {
//         private bool foldout = true;
//
//         public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//         {
//             EditorGUI.BeginProperty(position, label, property);
//             
//             // Draw the foldout label for FetchConfig
//             foldout = EditorGUILayout.BeginFoldoutHeaderGroup(foldout, label);
//             
//             if (foldout)
//             {
//                 EditorGUILayout.BeginVertical(GUI.skin.box);
//                 
//                 EditorGUI.indentLevel++;
//                 
//                 // Draw the property fields
//                 var source = property.FindPropertyRelative("source");
//                 EditorGUILayout.PropertyField(source);
//                 
//                 var gid = property.FindPropertyRelative("gid");
//                 EditorGUILayout.PropertyField(gid);
//                 
//                 var format = property.FindPropertyRelative("format");
//                 EditorGUILayout.PropertyField(format);
//                 
//                 var hasHeader = property.FindPropertyRelative("hasHeader");
//                 EditorGUILayout.PropertyField(hasHeader);
//                 
//                 var range = property.FindPropertyRelative("range");
//                 EditorGUILayout.PropertyField(range);
//                 
//                 EditorGUI.indentLevel--;
//                 
//                 EditorGUILayout.EndVertical();
//             }
//             
//             EditorGUILayout.EndFoldoutHeaderGroup();
//
//             EditorGUI.EndProperty();
//         }
//     }
// }
