using GerenciadorImobiliario_API.Models;
using GerenciadorImobiliario_API.ViewModels;
using GerenciadorImobiliario_API.ViewModels.Property;

namespace GerenciadorImobiliario_API.Mappers
{
    public static class PropertyMapper
    {
        public static PropertyViewModel MapToViewModel(Property property)
        {
            return new PropertyViewModel
            {
                Id = property.Id,
                Address = property.Address,
                Description = property.Description,
                Price = property.Price,
                OwnerId = property.OwnerId,
                Owner = property.Owner != null ? new OwnerViewModel
                {
                    Id = property.Owner.Id,
                    Name = property.Owner.Name
                } : null,
                Type = property.Type,
                NumberOfBedrooms = property.NumberOfBedrooms,
                NumberOfBathrooms = property.NumberOfBathrooms,
                NumberOfParkingSpaces = property.NumberOfParkingSpaces,
                TotalArea = property.TotalArea,
                UsableArea = property.UsableArea,
                IsFurnished = property.IsFurnished,
                HasBuiltInWardrobes = property.HasBuiltInWardrobes,
                HasPool = property.HasPool,
                HasBarbecueGrill = property.HasBarbecueGrill,
                HasBalcony = property.HasBalcony,
                HasGarden = property.HasGarden,
                RentPrice = property.RentPrice,
                CondominiumFee = property.CondominiumFee,
                Iptu = property.Iptu,
                Status = property.Status,
                UserId = property.UserId
            };
        }
    }
}
