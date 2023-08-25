using System.Collections.Generic;
using System.Linq;

namespace AVT.FetchGoogleSheet
{
    public class SheetRecord
    {
        internal Dictionary<string, int> map = new();
        internal List<string> data = new();
        
        public string this[int key]
        {
            get => data[key];
            set => data[key] = value;
        }

        public string this[string key]
        {
            get => data[map[key]];
            set => data[map[key]] = value;
        }

        public int Count => data.Count;

        public override string ToString()
        {
            return data.Aggregate("", (current, item) => current + $"{item}\n");
        }

        public SheetRecord(IReadOnlyList<string> keys, IReadOnlyList<string> values)
        {
            for (var i = 0; i < keys.Count && i < values.Count; i++)
            {
                data.Add(values[i]);
                map.Add(keys[i], i);
            }
        }
        
        public SheetRecord(IReadOnlyList<string> values)
        {
            for (var i = 0; i < values.Count; i++)
            {
                data.Add(values[i]);
                map.Add($"{i}", i);
            }
        }

        public void RemoveRange(int index, int count)
        {
            var end = index + count - 1;
            var keys = new List<string>(map.Keys);
            foreach (var key in keys)
            {
                if (map[key] < index) continue;
                
                if (map[key] > end)
                    map[key] -= count;
                else
                    map.Remove(key);
            }
            
            data.RemoveRange(index, count);
        }
    }
}
