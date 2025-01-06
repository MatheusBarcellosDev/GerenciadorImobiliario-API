using GerenciadorImobiliario_API.Data;
using GerenciadorImobiliario_API.Models;
using GerenciadorImobiliario_API.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using GerenciadorImobiliario_API.Mappers;
using Blog.Extensions;
using GerenciadorImobiliario_API.ViewModels.SubscriptionPlans;
using GerenciadorImobiliario_API.ViewModels.Property;

namespace GerenciadorImobiliario_API.Controllers
{
    [ApiController]
    [Route("v1/properties")]
    public class PropertiesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PropertiesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetProperties()
        {
            try
            {
                var properties = await _context.Properties
                    .Include(p => p.Address)
                    .Include(p => p.Owner)
                    .Include(p => p.User)
                    .ToListAsync();

                if (properties == null || !properties.Any())
                {
                    return NotFound(new ResultViewModel<string>("No properties found."));
                }

                var propertyViewModels = properties.Select(property => PropertyMapper.MapToViewModel(property)).ToList();
                return Ok(new ResultViewModel<List<PropertyViewModel>>(propertyViewModels));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResultViewModel<string>("Error getting properties."));
            }
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetProperty(long id)
        {
            try
            {
                var property = await _context.Properties
                    .Include(p => p.Address)
                    .Include(p => p.Owner)
                    .Include(p => p.User)
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (property == null)
                {
                    return NotFound(new ResultViewModel<string>("Property not found."));
                }

                return Ok(new ResultViewModel<PropertyViewModel>(PropertyMapper.MapToViewModel(property)));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResultViewModel<string>("Error getting property."));
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateProperty([FromBody] CreatePropertyViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));
            }

            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var address = new Address
                {
                    Street = model.Address.Street,
                    City = model.Address.City,
                    State = model.Address.State,
                    Country = model.Address.Country,
                    ZipCode = model.Address.ZipCode
                };

                _context.Address.Add(address);
                await _context.SaveChangesAsync();

                var property = new Property
                {
                    AddressId = address.Id,
                    Description = model.Description,
                    Price = model.Price,
                    OwnerId = model.OwnerId,
                    Type = model.Type,
                    NumberOfBedrooms = model.NumberOfBedrooms,
                    NumberOfBathrooms = model.NumberOfBathrooms,
                    NumberOfParkingSpaces = model.NumberOfParkingSpaces,
                    TotalArea = model.TotalArea,
                    UsableArea = model.UsableArea,
                    IsFurnished = model.IsFurnished,
                    HasBuiltInWardrobes = model.HasBuiltInWardrobes,
                    HasPool = model.HasPool,
                    HasBarbecueGrill = model.HasBarbecueGrill,
                    HasBalcony = model.HasBalcony,
                    HasGarden = model.HasGarden,
                    RentPrice = model.RentPrice,
                    CondominiumFee = model.CondominiumFee,
                    Iptu = model.Iptu,
                    Status = model.Status,
                    UserId = long.Parse(userId)
                };

                _context.Properties.Add(property);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetProperty), new { id = property.Id }, new ResultViewModel<PropertyViewModel>(PropertyMapper.MapToViewModel(property)));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResultViewModel<string>("Error created properties"));
            }

        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateProperty(long id, [FromBody] Property property)
        {
            if (id != property.Id)
            {
                return BadRequest(new ResultViewModel<string>("Invalid property ID."));
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));
            }

            try
            {
                _context.Entry(property).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok(new ResultViewModel<string>("Property updated successfully."));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PropertyExists(id))
                {
                    return NotFound(new ResultViewModel<string>("Property not found."));
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResultViewModel<string>("Error updating property."));
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteProperty(long id)
        {
            try
            {
                var property = await _context.Properties.FindAsync(id);
                if (property == null)
                {
                    return NotFound(new ResultViewModel<string>("Property not found."));
                }

                _context.Properties.Remove(property);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResultViewModel<string>("Error deleting property."));
            }
        }

        private bool PropertyExists(long id)
        {
            return _context.Properties.Any(e => e.Id == id);
        }
    }
}
