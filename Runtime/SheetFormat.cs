using System;

namespace AVT.FetchGoogleSheet
{
    public enum SheetFormat
    {
        TSV,
        CSV
    }
    
    public static class SheetFormatExtension
    {
        public static char GetSeparator(this SheetFormat format)
        {
            return format switch
            {
                SheetFormat.TSV => '\t',
                SheetFormat.CSV => ',',
                _ => throw new ArgumentOutOfRangeException(nameof(format), format, null)
            };
        }

        public static string GetName(this SheetFormat format)
        {
            return format switch
            {
                SheetFormat.TSV => "tsv",
                SheetFormat.CSV => "csv",
                _ => throw new ArgumentOutOfRangeException(nameof(format), format, null)
            };
        }
    }
}
