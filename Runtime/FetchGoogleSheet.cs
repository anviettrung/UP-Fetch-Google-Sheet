using System.Collections.Generic;

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

        #endregion
    }
}
