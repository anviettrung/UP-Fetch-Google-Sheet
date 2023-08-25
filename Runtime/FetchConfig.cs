using System;

namespace AVT.FetchGoogleSheet
{
    [Serializable]
    public class FetchConfig
    {
        [Regex(@"https:\/\/docs\.google\.com\/spreadsheets\/d\/e\/.*?\/pub", "Should contain Google Sheet Publish URL!", useFirstMatch = true)]
        public string source;
        
        [Regex("^[0-9]+$", "Should contain number only!")]
        public string gid = "0";
        
        [Regex(@"^(?:[A-Za-z0-9_]+!)?[A-Z]+[1-9]\d*(?::[A-Z]+[1-9]\d*)?$", "Should contain sheet range using A1 notation!")]
        public string range;
        
        public SheetFormat format = SheetFormat.TSV;
        
        // public string FetchUrl => $"{source}?output={format.ToString()}&single=true&gid={gid}";
    }
}
