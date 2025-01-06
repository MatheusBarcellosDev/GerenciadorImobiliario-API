namespace GerenciadorImobiliario_API.ViewModels.User
{
    public class UserViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string? SubscriptionPlan { get; set; }
    }
}
