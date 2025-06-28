namespace WorldCupStats.Data.Models
{
    public class DataSourceSettings
    {
        public const string SectionName = "DataSource"; // Matches appsettings.json key

        public bool UseApi { get; set; } = true;
        public string ApiBaseUrl { get; set; } = "https://worldcup-vua.nullbit.hr";
        public string JsonDataPath { get; set; } = "Data/Json";
    }
}