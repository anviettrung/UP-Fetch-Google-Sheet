using UnityEngine;

namespace AVT.FetchGoogleSheet
{
    public class FetchAttribute : PropertyAttribute
    {
        public readonly string functionName;

        public FetchAttribute(string functionName)
        {
            this.functionName = functionName;
        }
    }
}



