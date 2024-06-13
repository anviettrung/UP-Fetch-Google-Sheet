using System;
using JetBrains.Annotations;

namespace AVT.FetchGoogleSheet
{
    [Serializable]
    public class FetchConfig
    {
        public enum SourceType
        {
            PublicGoogleSheet = 0,
            LocalSheet = 1,
            // PrivateGoogleSheet, // Not support yet. Update in the future
        }

        public SourceType sourceType;
        
        [Regex("SourceRegexPattern", "SourceRegexErrorMsg", useFirstMatch = true, dynamicPattern = true, dynamicErrorMessage = true)]
        public string source;
        
        [Regex("^[0-9]+$", "Should contain number only!")]
        public string gid = "0";
        
        public SheetFormat format = SheetFormat.TSV;
        
        public bool hasHeader = true;
        
        [Regex(@"^(?:[A-Za-z0-9_]+!)?[A-Z]+[1-9]\d*(?::[A-Z]+[1-9]\d*)?$", "Should contain sheet range using A1 notation!")]
        public string range = "A1";

        [UsedImplicitly]
        public string SourceRegexPattern => sourceType switch
        {
            SourceType.PublicGoogleSheet => @"https:\/\/docs\.google\.com\/spreadsheets\/d\/e\/.*?\/pub",
            SourceType.LocalSheet => "^.*$",
            _ => ""
        };
        
        [UsedImplicitly]
        public string SourceRegexErrorMsg => sourceType switch
        {
            SourceType.PublicGoogleSheet => "Should contain Google Sheet Publish URL!",
            SourceType.LocalSheet => "Should contain file folder path!",
            _ => ""
        };
        
        public string FetchUrl => $"{source}?output={format.GetName()}&single=true&gid={gid}";
    }
}
