namespace GerenciadorImobiliario_API
{
    public class Configuration
    {
        public static string JwtKey { get; set; } = "ZjQ3YWMxMGItNThjYy00MzcyLWE1NjctMGUwMmIyYzNkNDc5";
        public static SmtpConfigurarion Smtp { get; set; } = new SmtpConfigurarion();

        public class SmtpConfigurarion
        {
            public string Host { get; set; }
            public int Port { get; set; } = 25;
            public string UserName { get; set; }
            public string Password { get; set; }
        }
    }
}
