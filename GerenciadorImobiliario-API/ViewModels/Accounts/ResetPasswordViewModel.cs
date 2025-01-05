namespace GerenciadorImobiliario_API.ViewModels.Accounts
{
    public class ResetPasswordViewModel
    {
        public string Token { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }
}
