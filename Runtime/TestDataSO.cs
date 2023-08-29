using System.Collections.Generic;
using UnityEngine;

namespace AVT.FetchGoogleSheet
{
    [System.Serializable]
    public struct UnitData : IGoogleSheetDataSetter
    {
        public string name;
        public int health;
        public int damage;

        public void SetDataFromSheet(SheetRecord record)
        {
            name = record["name"];
            health = int.Parse(record["health"]);
            damage = int.Parse(record["damage"]);
        }
    }
    
    [CreateAssetMenu]
    public class TestDataSO : ScriptableObject
    {
        [Fetch("FetchUnit")] 
        public FetchConfig fetchUnitConfig;
        public FetchConfig fetdshUnitConfig;
        
        public List<UnitData> units;

        public void FetchUnit(SheetTable table)
        {
            Debug.Log(table);
            FetchGoogleSheet.SheetTableToList(table, units);
        }
    }
}

