using System;
using UnityEditor;

namespace AVT.FetchGoogleSheet
{
    public enum SheetFormat
    {
        TSV,
        CSV
    }
    
    public static class SheetFormatExtension
    {
        #if UNITY_EDITOR
        public static SheetFormat ToSheetFormat(this SerializedProperty property)
        {
            return (SheetFormat)Enum.ToObject(typeof(SheetFormat), property.enumValueIndex);
        }
        #endif
        
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
