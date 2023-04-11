using System.Collections.Generic;
using UnityEditor;

namespace Plugins.AVT.FetchGoogleSheet
{
    public static class FetchGoogleSheet
    {
        public static void SheetToList<T>(this UnityEngine.Object obj, 
            string url, 
            List<T> list) 
            where T : IGoogleSheetDataSetter, new()
        {
            obj.GetRawTextFromUrl(url, (success, result) =>
            {
                if (!success)
                {
                    UnityEngine.Debug.Log(result); 
                    return;
                }
                
                list.FromSheetData(result);
            });
        }

        public static void SheetMatrixToList<T>(List<List<string>> sheetMatrix, List<T> list) where T : IGoogleSheetDataSetter, new()
        {
            var propKeys = sheetMatrix[0].ToArray();
            
            list.Clear();
            for (var i = 1; i < sheetMatrix.Count; i++)
            {
                var record = new T();
                var propValues = sheetMatrix[i].ToArray();
                record.SetDataFromSheet(SheetDataReader.CreateRecord(propKeys, propValues));
                list.Add(record);
            }
#if UNITY_EDITOR
            AssetDatabase.SaveAssets();
#endif
        }
    }

    public interface IGoogleSheetDataSetter
    {
        void SetDataFromSheet(Dictionary<string, string> source);
    }
}

