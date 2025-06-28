namespace WorldCupStats.WpfApp.ViewModels.Models
{
    public class LanguageOption
    {
        public string DisplayName { get; set; }
        public string Code { get; set; }

        public LanguageOption(string displayName, string code)
        {
            DisplayName = displayName;
            Code = code;
        }

        public override string ToString() => DisplayName;
    }
}
