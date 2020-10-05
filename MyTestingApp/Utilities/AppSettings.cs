namespace MyTestingApp.Utilities
{
    public class AppSetting
    {
        // public const string Settings = "AppSettings"; 
        public string Issuer { get; set; }
        public string Key { get; set; }  
        public int ExpiryTimeInMinutes { get; set; }   
    }
}