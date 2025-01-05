namespace GerenciadorImobiliario_API.ViewModels.User
{
    public class UserDetailsViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? PostalCode { get; set; }
        public string? Creci { get; set; }
        public string? Specialties { get; set; }
        public int YearsOfExperience { get; set; }
        public string? Description { get; set; }
        public string? LinkedIn { get; set; }
        public string? Instagram { get; set; }
        public string? SubscriptionPlan { get; set; }
    }
}
