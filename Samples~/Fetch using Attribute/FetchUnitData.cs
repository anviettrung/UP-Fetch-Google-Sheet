using System.Collections.Generic;
using UnityEngine;

// ------------------------------------------------------------------------------------------------------------------------------
// Source of sample data can be found here: https://docs.google.com/spreadsheets/d/1x0M9_qgQiVXtdWL3DXXnf4Pp2fkVALfHcHoqETKwCnY/edit?usp=sharing
// ------------------------------------------------------------------------------------------------------------------------------

namespace AVT.FetchGoogleSheet
{
    [CreateAssetMenu(menuName = "Fetch Google Sheet Sample/Fetch Unit Data")]
    public class FetchUnitData : ScriptableObject
    {
        [Fetch("FetchUnit")] 
        public FetchConfig fetchConfig;
        
        public List<UnitData> units;

        private void FetchUnit(SheetTable table)
        {
            FetchGoogleSheet.SheetTableToList(table, units);
        }
        
        // ---------------------------------------------------------------------
        
        [Fetch("FetchSuccess")] 
        public FetchConfig customFetchConfig;
        
        private void FetchSuccess(SheetTable table, FetchConfig config)
        {
            for (var i = 0; i < table.RecordCount; i++)
            {
                for (var j = 0; j < table.FieldCount; j++)
                {
                    Debug.Log($"[{i},{j}] {table[i].Keys[j]} : {table[i][j]}");
                }
            }
            
            Debug.Log(config);
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

