using System.Collections.Generic;
using UnityEngine;

namespace AVT.FetchGoogleSheet
{
    public class MasterConfig : ScriptableObject
    {
        public List<FetchConfigV2> sheetConnects;
    }
}