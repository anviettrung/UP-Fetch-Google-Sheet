using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Plugins.AVT.FetchGoogleSheet
{
    public static class SheetDataReader
    {
        public static void FromSheetData<T>(this List<T> list, 
            string sheet, 
            SheetFormat format = SheetFormat.CSV)
            where T : IGoogleSheetDataSetter, new()
        {
            var rows = sheet.ToRows();
            var propKeys = rows[0].ToCells(format);
            
            list.Clear();
            for (var i = 1; i < rows.Length; i++)
            {
                var record = new T();
                var propValues = rows[i].ToCells(format);
                record.SetDataFromSheet(CreateRecord(propKeys, propValues));
                list.Add(record);
            }
        }

        public static List<List<string>> TrimSheetMatrix(this List<List<string>> sheetMatrix, SheetBlock block)
        {
            var endBound = block.end.y + 1;
            if (endBound < sheetMatrix.Count)
                sheetMatrix.RemoveRange(endBound,  sheetMatrix.Count - endBound);
            sheetMatrix.RemoveRange(0, block.start.y);

            endBound = block.end.x + 1;
            foreach (var row in sheetMatrix)
            {
                if (endBound < row.Count) 
                    row.RemoveRange(endBound, row.Count - endBound);
                row.RemoveRange(0, block.start.x);
            }
            
            return sheetMatrix;
        }

        public static List<List<string>> ToSheetMatrix(this string sheetText, SheetFormat format = SheetFormat.CSV)
        {
            var res = new List<List<string>>();

            var rows = sheetText.ToRows();
            res.Capacity = rows.Length;
            res.AddRange(rows.Select(t => t.ToCells(format).ToList()));

            return res;
        }
        
        public static string[] ToRows(this string source) 
            => source.Split("\n"[0]);
        public static string[] ToCells(this string source, params char[] separator) 
            => source.Trim().Split(separator);

        public static string[] ToCells(this string source, 
            SheetFormat format = SheetFormat.CSV)
        {
            switch (format)
            {
                case SheetFormat.CSV:
                    return source.ToCells(","[0]);
                case SheetFormat.TSV:
                    return source.ToCells("\t"[0]);
                default:
                    throw new ArgumentOutOfRangeException(nameof(format), format, null);
            }
        }

        public static Dictionary<string, string> CreateRecord(string[] keys, string[] values)
        {
            var result = new Dictionary<string, string>();
            for (var i = 0; i < keys.Length && i < values.Length; i++)
                result.Add(keys[i], values[i]);

            return result;
        }
    }

    public class SheetBlock
    {
        public Vector2Int start;
        public Vector2Int end;

        public override string ToString()
        {
            return $"start = {start} | end = {end}";
        }

        public SheetBlock ToValidBlock()
        {
            var newStart = new Vector2Int(Mathf.Min(start.x, end.x), Mathf.Min(start.y, end.y));
            var newEnd = new Vector2Int(Mathf.Max(start.x, end.x), Mathf.Max(start.y, end.y));

            start = newStart;
            end = newEnd;

            return this;
        }
    }
    
    public static class SheetBlockExtension
    {
        public static SheetBlock ToSheetBlock(this string rangeA1Notation)
        {
            var result = new SheetBlock();
            var cells = rangeA1Notation.Split(':');
            switch (cells.Length)
            {
                case 1:
                    result.start = result.end = cells[0].ToCellCoordinate();
                    break;
                case 2:
                    result.start = cells[0].ToCellCoordinate();
                    result.end = cells[1].ToCellCoordinate();
                    break;
                default:
                    result.start = result.end = Vector2Int.zero;
                    break;
            }

            return result;
        }

        private static Vector2Int ToCellCoordinate(this string cellA1Notation)
        {
            var match = Regex.Matches(cellA1Notation, "(^[A-Z]+)|([0-9]+$)");
            if (match.Count != 2)
            {
                Debug.LogError("Invalid cell reference");
                return Vector2Int.zero;
            }

            var colA1 = match[0];
            var rowA1 = match[1];

            return new Vector2Int(ColA1ToIndex(colA1.Value), RowA1ToIndex(rowA1.Value));
        }

        private static int ColA1ToIndex(string colA1)
        {
            if (colA1.Length > 2)
            {
                Debug.LogError("Expected column label.");
                return -1;
            }
            
            var result = colA1[colA1.Length - 1] - 'A';
            if (colA1.Length == 2)
                result += 26 * (colA1[0] - 'A' + 1);
            return result;
        }

        private static int RowA1ToIndex(string rowA1)
        {
            return int.Parse(rowA1) - 1;
        }
    }

    public enum SheetFormat
    {
        CSV,
        TSV
    }
}

