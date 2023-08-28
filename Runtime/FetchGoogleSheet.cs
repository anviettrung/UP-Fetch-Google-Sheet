using System.Collections.Generic;
using UnityEditor;

namespace AVT.FetchGoogleSheet
{
    #region Define
    
    public interface IGoogleSheetDataSetter
    {
        void SetDataFromSheet(SheetRecord record);
    }

    #endregion
    
    public static class FetchGoogleSheet
    {
        #region API

        public static void SheetTableToList<T>(SheetTable sheetMatrix, List<T> list) where T : IGoogleSheetDataSetter, new()
        {
            var propKeys = sheetMatrix[0].data;
            
            list.Clear();
            for (var i = 1; i < sheetMatrix.RecordCount; i++)
            {
                var record = new T();
                var propValues = sheetMatrix[i].data;
                record.SetDataFromSheet(new SheetRecord(propKeys, propValues));
                list.Add(record);
            }
#if UNITY_EDITOR
            AssetDatabase.SaveAssets();
#endif
        }

        #endregion
    }
}

#region Old

// public static void FromSheetData<T>(this List<T> list, List<List<string>> table)
//     where T : IGoogleSheetDataSetter, new()
// {
//     var propKeys = table[0];
//     
//     list.Clear();
//     for (var i = 1; i < table.Count; i++)
//     {
//         var record = new T();
//         var propValues = table[i];
//         // record.SetDataFromSheet(FetchGoogleSheetUtility.CreateRecord(propKeys, propValues));
//         list.Add(record);
//     }
// }
//
// // public static void SheetToList<T>(this UnityEngine.Object obj, 
// //     string url, 
// //     List<T> list) 
// //     where T : IGoogleSheetDataSetter, new()
// // {
// //     GetRawTextFromUrl(url, (success, result) =>
// //     {
// //         if (!success)
// //         {
// //             Debug.Log(result); 
// //             return;
// //         }
// //         
// //         list.FromSheetData(result.ToTable());
// //     });
// // }

#endregion