using UnityEngine;

namespace AVT.FetchGoogleSheet
{
    public class FetchAttribute : PropertyAttribute
    {
        public readonly string targetName;
        public readonly TargetType targetType;

        public FetchAttribute(string targetName, TargetType targetType = TargetType.METHOD)
        {
            this.targetName = targetName;
            this.targetType = targetType;
        }
        
        public enum TargetType
        {
            METHOD,
            PROPERTY
        }
    }
}



