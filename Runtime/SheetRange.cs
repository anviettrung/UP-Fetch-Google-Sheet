using System.Text.RegularExpressions;
using UnityEngine;

namespace AVT.FetchGoogleSheet
{
    public class SheetRange
    {
        public Vector2Int start;
        public Vector2Int end;
        
        public override string ToString() => $"start = {start} | end = {end}";

        public SheetRange Validate()
        {
            var newStart = new Vector2Int(Mathf.Min(start.x, end.x), Mathf.Min(start.y, end.y));
            var newEnd = new Vector2Int(Mathf.Max(start.x, end.x), Mathf.Max(start.y, end.y));

            start = newStart;
            end = newEnd;

            return this;
        }
    }
    
    public static class SheetRangeExtension
    {
        public static SheetRange ToSheetRange(this string rangeA1Notation)
        {
            var result = new SheetRange();
            var cells = rangeA1Notation.Split(':');
            switch (cells.Length)
            {
                case 1: // Select a cell
                    result.start = result.end = cells[0].ToCellCoordinate();
                    break;
                case 2: // Select range of cells
                    result.start = cells[0].ToCellCoordinate();
                    result.end = cells[1].ToCellCoordinate();
                    break;
                default: // Exception
                    result.start = result.end = Vector2Int.zero;
                    Debug.LogError($"{rangeA1Notation} is not a valid range using A1 Notation!");
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
            var result = 0;
            var coefficient = 1;
            
            for (var i = colA1.Length - 1; i >= 0; i--, coefficient *= 26)
                result += (colA1[i] - 'A' + 1) * coefficient;
            
            return result - 1; // -1 for ensure index start from 0
        }

        private static int RowA1ToIndex(string rowA1)
        {
            return int.Parse(rowA1) - 1;
        }
    }
}
