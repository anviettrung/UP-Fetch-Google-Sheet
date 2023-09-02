using System;
using System.Collections.Generic;
using System.Linq;

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

        public static void SheetTableToList<T>(SheetTable table, List<T> list) where T : IGoogleSheetDataSetter, new()
        {
            list.Clear();
            foreach (var record in table.Records)
            {
                var recordT = new T();
                recordT.SetDataFromSheet(record);
                list.Add(recordT);
            }
        }

        public static void SheetTableToList(SheetTable table, List<int> list) =>
            SheetTableToListPrimitiveType(table, list, int.Parse);
        
        public static void SheetTableToList(SheetTable table, List<float> list) =>
            SheetTableToListPrimitiveType(table, list, float.Parse);
        
        public static void SheetTableToList(SheetTable table, List<string> list) =>
            SheetTableToListPrimitiveType(table, list, s => s);
        
        public static void SheetTableToList(SheetTable table, List<bool> list) =>
            SheetTableToListPrimitiveType(table, list, bool.Parse);
        
        private static void SheetTableToListPrimitiveType<T>(SheetTable table, List<T> list, Func<string, T> parseFunc)
        {
            list.Clear();
            list.AddRange(table.Records.Select(record => parseFunc(record[0])));
        }

        #endregion
    }
}
