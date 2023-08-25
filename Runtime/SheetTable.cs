using System;
using System.Collections.Generic;
using System.Linq;

namespace AVT.FetchGoogleSheet
{
    public class SheetTable
    {
        public bool hasHeader;
        internal List<SheetRecord> data = new();

        public SheetRecord this[int key]
        {
            get => data[key];
            set => data[key] = value;
        }

        public int RecordCount => data.Count;
        
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
            var endBound = range.end.y + 1;
            if (endBound < data.Count)
                data.RemoveRange(endBound,  data.Count - endBound);
            data.RemoveRange(0, range.start.y);

            endBound = range.end.x + 1;
            foreach (var record in data)
            {
                if (endBound < record.Count)
                {
                    record.RemoveRange(endBound, record.Count - endBound);
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
