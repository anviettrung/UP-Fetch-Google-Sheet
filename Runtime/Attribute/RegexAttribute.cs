using UnityEngine;

namespace AVT.FetchGoogleSheet
{
    public class RegexAttribute : PropertyAttribute
    {
        public readonly string pattern;
        public readonly string errorMessage;
        public bool useFirstMatch = false;
        public bool dynamicPattern;
        public bool dynamicErrorMessage;

        public RegexAttribute(string pattern, string errorMessage = "String is not match regex.")
        {
            this.pattern = pattern;
            this.errorMessage = errorMessage;
        }
    }
}