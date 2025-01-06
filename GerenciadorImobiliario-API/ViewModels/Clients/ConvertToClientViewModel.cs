namespace GerenciadorImobiliario_API.ViewModels.Clients
{
    public class ConvertToClientViewModel
    {
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
        public List<PropertyPreferenceViewModel> PropertyPreferences { get; set; }
        public bool IsOwner { get; set; }
    }
}
