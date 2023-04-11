using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Plugins.AVT.FetchGoogleSheet
{
    [CreateAssetMenu]
    public class TestDataSO : ScriptableObject
    {
        [Multiline]
        public string sheetUrl;
        public string range;
        public List<UnitData> units;

        #if UNITY_EDITOR
        [MenuItem("CONTEXT/TestDataSO/Fetch")]
        public static void Fetch(MenuCommand command)
        {
            var _ = (TestDataSO)command.context;
            
            // Type 1
            //_.SheetToList(_.sheetUrl, _.units);

            // Type 2
            var block = _.range.ToSheetBlock().ToValidBlock();
            _.GetRawTextFromUrl(_.sheetUrl, (success, text) =>
            {
                if (!success)
                    return;
                
                FetchGoogleSheet.SheetMatrixToList(text.ToSheetMatrix().TrimSheetMatrix(block), _.units);
                #if UNITY_EDITOR
                EditorUtility.SetDirty(_);
                AssetDatabase.SaveAssets();
                #endif
            });
        }
        #endif
    }

    [System.Serializable]
    public struct UnitData : IGoogleSheetDataSetter
    {
        public string name;
        public int health;
        public int damage;

        public void SetDataFromSheet(Dictionary<string, string> source)
        {
            name = source["name"];
            health = int.Parse(source["health"]);
            damage = int.Parse(source["damage"]);
        }
    }
}

