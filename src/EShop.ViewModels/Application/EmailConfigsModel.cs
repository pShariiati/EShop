namespace EShop.ViewModels.Application
{
    public class EmailConfigsModel
    {
        public string SiteTitle { get; set; }
        public string SiteAddress { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public bool UseSSL { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}