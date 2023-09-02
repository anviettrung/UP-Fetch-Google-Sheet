using System.Collections.Generic;
using UnityEngine;

// ------------------------------------------------------------------------------------------------------------------------------
// Source of sample data can be found here: https://docs.google.com/spreadsheets/d/1x0M9_qgQiVXtdWL3DXXnf4Pp2fkVALfHcHoqETKwCnY/edit?usp=sharing
// ------------------------------------------------------------------------------------------------------------------------------

namespace AVT.FetchGoogleSheet
{
    [CreateAssetMenu(menuName = "Fetch Google Sheet Sample/Fetch Primitive Data")]
    public class FetchPrimitiveData : ScriptableObject
    {
        [Fetch("FetchLevelExp")] 
        public FetchConfig fetchConfig;

        public List<int> levelExp;
        
        private void FetchLevelExp(SheetTable table)
        {
            FetchGoogleSheet.SheetTableToList(table, levelExp);
        }
        
        // ---------------------------------------------------------------------
        
        [Fetch("FetchIsBossLevel")] 
        public FetchConfig isBossLevelFetchConfig;

        public List<bool> isBossLevel;
            
        private void FetchIsBossLevel(SheetTable table)
        {
            FetchGoogleSheet.SheetTableToList(table, isBossLevel);
        }
    }
}
