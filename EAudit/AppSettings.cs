namespace EAudit
{
    public class AppSettings
    {
        public const string SectionName = "ConnectionStrings";
        public string DefaultConnection { get; set; }

        public AppSettings()
        {
        }
        public string ConnectionStrings { get; set; }
    }
}
