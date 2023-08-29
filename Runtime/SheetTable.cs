using System;
using System.Collections.Generic;
using System.Linq;

namespace AVT.FetchGoogleSheet
{
    public class SheetTable
    {
        public bool hasHeader;
        internal List<SheetRecord> data = new List<SheetRecord>();

        public SheetRecord this[int key]
        {
            get => data[key];
            set => data[key] = value;
        }

        public int RecordCount => data.Count;
        public int FieldCount => RecordCount > 0 ? data[0].FieldCount : 0;
        public List<SheetRecord> Records => data;
        
        public override string ToString()
        {
            var res = "Table \n \n";
            for (var i = 0; i < RecordCount; i++)
            {
                res += $"Record {i}:\n{data[i]}\n";
            }
            return res;
        }
        
        public SheetTable(string sheetText, SheetFormat format = SheetFormat.CSV)
        {
            var rows = sheetText.ToRows();
            data.Capacity = rows.Length;
            data.AddRange(rows.Select(t => new SheetRecord(t.ToCells(format))));
        }

        public SheetTable Trim(SheetRange range)
        {
            var end = range.end.y + 1;
            if (end < data.Count)
                data.RemoveRange(end,  data.Count - end);
            data.RemoveRange(0, range.start.y);

            end = range.end.x + 1;
            foreach (var record in data)
            {
                if (end < record.FieldCount)
                {
                    record.RemoveRange(end, record.FieldCount - end);
                }
                record.RemoveRange(0, range.start.x);
            }
            
            return this;
        }
    }

    public static class SheetStringExtension
    {
        public static string[] ToRows(this string source) 
            => source.Split('\n');
        
        public static string[] ToCells(this string source, SheetFormat format) 
            => source.ToCells(format.GetSeparator());
        
        private static string[] ToCells(this string source, params char[] separator)
            => source.Trim().Split(separator, StringSplitOptions.RemoveEmptyEntries);
    }
}
