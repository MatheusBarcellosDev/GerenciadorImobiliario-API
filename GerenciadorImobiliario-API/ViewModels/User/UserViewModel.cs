namespace GerenciadorImobiliario_API.ViewModels.User
{
    public class UserViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string? SubscriptionPlan { get; set; }
    }
}
