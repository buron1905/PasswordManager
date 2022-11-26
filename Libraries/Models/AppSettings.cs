namespace Models
{
    public class AppSettings
    {
        public string Secret { get; set; }
        public int JweTokenMinutesTTL { get; set; }
    }
}
