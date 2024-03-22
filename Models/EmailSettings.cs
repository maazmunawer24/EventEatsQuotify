namespace EventEatsQuotify.Models
{
    public class EmailSettings
    {
        public string SmtpServer { get; set; }
        public int SmtpPort { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FromAddress { get; set; }
        public string FromName { get; set; }
        public int Timeout { get; set; } // Add Timeout property

        public bool EnableSSL {  get; set; }
    }

}
