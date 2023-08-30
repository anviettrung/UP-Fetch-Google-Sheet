using System.Collections.Generic;
using UnityEngine;

// ------------------------------------------------------------------------------------------------------------------------------
// Source of sample data can be found here: https://docs.google.com/spreadsheets/d/1x0M9_qgQiVXtdWL3DXXnf4Pp2fkVALfHcHoqETKwCnY/edit?usp=sharing
// ------------------------------------------------------------------------------------------------------------------------------

namespace AVT.FetchGoogleSheet
{
    [CreateAssetMenu]
    public class SampleFetchUsingAttribute : ScriptableObject
    {
        [Fetch("FetchUnit")] 
        public FetchConfig fetchConfig;
        
        public List<UnitData> units;

        private void FetchUnit(SheetTable table)
        {
            FetchGoogleSheet.SheetTableToList(table, units);
        }
    }
    
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
    
    
}

