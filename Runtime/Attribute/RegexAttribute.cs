using UnityEngine;

namespace AVT.FetchGoogleSheet
{
    public class RegexAttribute : PropertyAttribute
    {
        public string pattern;
        public string errorMessage;
        public bool useFirstMatch = false;

        public RegexAttribute(string pattern, string errorMessage)
        {
            this.pattern = pattern;
            this.errorMessage = errorMessage;
        }
        
        public RegexAttribute(string pattern)
        {
            this.pattern = pattern;
            this.errorMessage = "String is not match regex.";
        }
    }
}