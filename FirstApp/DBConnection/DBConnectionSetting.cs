namespace FirstApp.DBConnection
{
    public class DBConnectionSetting: IDatabseSetting
    {
        public string CustomerConnectionString { get; set; }
    }

    public interface IDatabseSetting
    {
        public string CustomerConnectionString { get; set; }
    }
}
