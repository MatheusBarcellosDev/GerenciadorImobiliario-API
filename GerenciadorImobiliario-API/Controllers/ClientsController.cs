using Blog.Extensions;
using Blog.Services;
using GerenciadorImobiliario_API.Data;
using GerenciadorImobiliario_API.Enums;
using GerenciadorImobiliario_API.Models;
using GerenciadorImobiliario_API.ViewModels.Clients;
using GerenciadorImobiliario_API.ViewModels.SubscriptionPlans;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GerenciadorImobiliario_API.Controllers
{
    [ApiController]
    [Route("v1/clients")]
    public class ClientsController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly AppDbContext _context;
        private readonly TokenService _tokenService;
        private readonly ILogger<AccountsController> _logger;
        private readonly EmailService _emailService;

        public ClientsController(UserManager<User> userManager, AppDbContext context, TokenService tokenService, ILogger<AccountsController> logger, EmailService emailService)
        {
            _userManager = userManager;
            _context = context;
            _tokenService = tokenService;
            _logger = logger;
            _emailService = emailService;
        }

        private LeadViewModel MapToViewModel(Lead lead)
        {
            return new LeadViewModel
            {
                Id = lead.Id,
                InitialNotes = lead.InitialNotes,
                Name = lead.Name,
                Email = lead.Email,
                Telephone = lead.Telephone,
                DateContacted = lead.DateContacted,
                LeadStatus = lead.LeadStatus?.Name,
                CurrentPipelineStage = lead.CurrentPipelineStage?.Name
            };
        }

        private bool IsLeadInactive(Lead lead, int daysInactive = 5)
        {
            if (lead.LastInteractionDate == null) return false;

            var timeSpan = DateTime.UtcNow - lead.LastInteractionDate.Value;
            return timeSpan.TotalDays > daysInactive;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetClients()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(userId))
                {
                    return BadRequest(new ResultViewModel<string>("User ID not found."));
                }


                var clients = await _context.Clients
                    .Where(c => c.UserId.ToString() == userId)
                    .Include(c => c.PropertyPreferences)
                    .Include(c => c.Address)
                    .ToListAsync();

                if (clients == null || !clients.Any())
                {
                    return NotFound(new ResultViewModel<string>("No clients found."));
                }


                var clientViewModels = clients.Select(async client =>
                {
                    var lead = await _context.Leads
                        .Include(l => l.LeadStatus)
                        .Include(l => l.CurrentPipelineStage)
                        .FirstOrDefaultAsync(l => l.ClientId == client.Id);

                    return new LeadViewModel
                    {
                        Id = client.Id,
                        Name = client.Name,
                        Email = client.Email,
                        Telephone = client.Telephone,
                        Address = client.Address,
                        LeadStatus = lead?.LeadStatus?.Name,
                        CurrentPipelineStage = lead?.CurrentPipelineStage?.Name,
                        PropertyPreferences = client.PropertyPreferences?.Select(p => new PropertyPreferenceViewModel
                        {
                            Type = p.Type,
                            Location = p.Location,
                            MinBedrooms = p.MinBedrooms,
                            MaxBedrooms = p.MaxBedrooms,
                            MinPrice = p.MinPrice,
                            MaxPrice = p.MaxPrice,
                            AdditionalPreferences = p.AdditionalPreferences
                        }).ToList()
                    };
                }).Select(t => t.Result).ToList();


                return Ok(new ResultViewModel<List<LeadViewModel>>(clientViewModels));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResultViewModel<string>("Error getting clients."));
            }
        }

        
        [HttpGet("leads/new")]
        [Authorize]
        public async Task<IActionResult> GetNewLeads()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(userId))
                {
                    return BadRequest(new ResultViewModel<string>("User ID not found."));
                }

                var leads = await _context.Leads
                    .Where(l => l.UserId.ToString() == userId && l.LeadStatusId == (int)ELeadStatusEnum.NovoLead)
                    .Include(l => l.LeadStatus)
                    .Include(l => l.CurrentPipelineStage)
                    .ToListAsync();

                if (leads == null || !leads.Any())
                {
                    return NotFound(new ResultViewModel<string>("No new leads found."));
                }

                var leadViewModels = leads.Select(lead => new LeadViewModel
                {
                    Id = lead.Id,
                    Name = lead.Name,
                    Email = lead.Email,
                    Telephone = lead.Telephone,
                    InitialNotes = lead.InitialNotes,
                    DateContacted = lead.DateContacted,
                    LeadStatus = lead.LeadStatus?.Name,
                    CurrentPipelineStage = lead.CurrentPipelineStage?.Name
                }).ToList();

                return Ok(new ResultViewModel<List<LeadViewModel>>(leadViewModels));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResultViewModel<string>("Error getting leads."));

            }
        }

        [HttpGet("leads/converted")]
        [Authorize]
        public async Task<IActionResult> GetConverteds()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(userId))
                {
                    return BadRequest(new ResultViewModel<string>("User ID not found."));
                }

                var leads = await _context.Leads
                    .Where(l => l.UserId.ToString() == userId && l.ClientId != null)
                    .Include(l => l.LeadStatus)
                    .Include(l => l.CurrentPipelineStage)
                    .ToListAsync();

                if (leads == null || !leads.Any())
                {
                    return NotFound(new ResultViewModel<string>("No new leads found."));
                }

                var leadViewModels = leads.Select(lead => new LeadViewModel
                {
                    Id = lead.Id,
                    Name = lead.Name,
                    Email = lead.Email,
                    Telephone = lead.Telephone,
                    InitialNotes = lead.InitialNotes,
                    DateContacted = lead.DateContacted,
                    LeadStatus = lead.LeadStatus?.Name,
                    CurrentPipelineStage = lead.CurrentPipelineStage?.Name
                }).ToList();

                return Ok(new ResultViewModel<List<LeadViewModel>>(leadViewModels));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResultViewModel<string>("Error getting clients."));

            }
        }

        [HttpGet("leads/inactive")]
        [Authorize]
        public async Task<IActionResult> GetInactiveLeads(int daysInactive = 5)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(userId))
                {
                    return BadRequest(new ResultViewModel<string>("User ID not found."));
                }

                var leads = await _context.Leads
                    .Where(l => l.UserId.ToString() == userId)
                    .Include(l => l.LeadStatus)
                    .Include(l => l.CurrentPipelineStage)
                    .ToListAsync();

                if (leads == null || !leads.Any())
                {
                    return NotFound(new ResultViewModel<string>("No leads found."));
                }

                var inactiveLeads = leads
                .Where(lead => IsLeadInactive(lead, daysInactive) && lead.IsActive)
                .Select(lead =>
                {
                    lead.IsActive = false;
                    return lead;
                })
                .Select(lead => MapToViewModel(lead))
                .ToList();

                await _context.SaveChangesAsync();

                return Ok(new ResultViewModel<List<LeadViewModel>>(inactiveLeads));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResultViewModel<string>("Error retrieving inactive leads."));
            }
        }

        [HttpGet("leads/pipeline")]
        [Authorize]
        public async Task<IActionResult> GetPipelineLeads()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(userId))
                {
                    return BadRequest(new ResultViewModel<string>("User ID not found."));
                }

                var leads = await _context.Leads
                    .Where(l => l.UserId.ToString() == userId && l.LeadStatusId == (int)ELeadStatusEnum.NovoLead)
                    .OrderByDescending(l => l.DateContacted)
                    .Include(l => l.LeadStatus)
                    .Include(l => l.CurrentPipelineStage)
                    .ToListAsync();

                var leadViewModels = leads.Select(lead => MapToViewModel(lead)).ToList();

                return Ok(new ResultViewModel<List<LeadViewModel>>(leadViewModels));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResultViewModel<string>("Error retrieving pipeline leads."));
            }
        }

        [HttpPost("leads")]
        [Authorize]
        public async Task<ActionResult> Post([FromBody] CreateUpdateLeadViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var lead = new Lead
            {
                InitialNotes = model.InitialNotes,
                Name = model.Name,
                Email = model.Email,
                Telephone = model.Telephone,
                DateContacted = DateTime.UtcNow,
                LeadStatusId = (int)ELeadStatusEnum.NovoLead,
                CurrentPipelineStageId = (int)EPipelineStageEnum.EsperandoAtendimento,
                UserId = long.Parse(userId),
                LastInteractionDate = DateTime.UtcNow,
                IsActive = true
            };



            _context.Leads.Add(lead);
            await _context.SaveChangesAsync();

            return Ok(new ResultViewModel<LeadViewModel>(MapToViewModel(lead)));
        }
  
        [HttpPost("leads/{id}/convert-to-client")]
        [Authorize]
        public async Task<IActionResult> ConvertToClient(long id, [FromBody] ConvertToClientViewModel model)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var lead = await _context.Leads
                    .Include(l => l.LeadStatus)
                    .Include(l => l.CurrentPipelineStage)
                    .FirstOrDefaultAsync(l => l.Id == id);

                if (lead == null || lead.UserId != long.Parse(userId))
                {
                    return NotFound(new ResultViewModel<string>("Lead not found or you do not have permission to update it."));
                }

                var client = new Client
                {
                    Name = lead.Name,
                    Email = lead.Email,
                    Telephone = lead.Telephone,
                    RegistrationDate = DateTime.UtcNow,
                    LastUpdatedDate = DateTime.UtcNow,
                    UserId = long.Parse(userId),
                    Type = model.IsOwner ? ClientType.Ambos : ClientType.Comprador,
                    Address = new Address
                    {
                        Street = model.Address,
                        City = model.City,
                        State = model.State,
                        Country = model.Country,
                        ZipCode = model.ZipCode
                    }
                };

                _context.Clients.Add(client);
                await _context.SaveChangesAsync();

                lead.ClientId = client.Id;
                lead.LeadStatusId = (int)ELeadStatusEnum.Cliente;
                lead.LastInteractionDate = DateTime.UtcNow;
                _context.Leads.Update(lead);



                foreach (var preference in model.PropertyPreferences)
                {
                    var propertyPreference = new PropertyPreference
                    {
                        ClientId = client.Id,
                        Type = preference.Type,
                        Location = preference.Location,
                        MinBedrooms = preference.MinBedrooms,
                        MaxBedrooms = preference.MaxBedrooms,
                        MinPrice = preference.MinPrice,
                        MaxPrice = preference.MaxPrice,
                        AdditionalPreferences = preference.AdditionalPreferences
                    };
                    _context.PropertyPreferences.Add(propertyPreference);
                }


                await _context.SaveChangesAsync();

                return Ok(new ResultViewModel<string>("Lead successfully converted to client.", null));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResultViewModel<string>("Error converting lead to client."));
            }
        }

        [HttpPut("leads/{id}/pipeline")]
        [Authorize]
        public async Task<IActionResult> UpdateLeadPipeline(long id, [FromBody] int newPipelineStageId)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var lead = await _context.Leads.FindAsync(id);

                if (lead == null || lead.UserId != long.Parse(userId))
                {
                    return NotFound(new ResultViewModel<string>("Lead not found or you do not have permission to update it."));
                }

                lead.CurrentPipelineStageId = newPipelineStageId;
                lead.LastInteractionDate = DateTime.UtcNow;
                await _context.SaveChangesAsync();

                return Ok(new ResultViewModel<LeadViewModel>(MapToViewModel(lead)));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResultViewModel<string>("Error updating lead pipeline stage."));
            }
        }

        [HttpDelete("leads/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteLead(long id)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var lead = await _context.Leads.FindAsync(id);

                if (lead == null || lead.UserId != long.Parse(userId))
                {
                    return NotFound(new ResultViewModel<string>("Lead not found or you do not have permission to delete it."));
                }

                _context.Leads.Remove(lead);
                await _context.SaveChangesAsync();

                return NoContent(); // 204 No Content
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResultViewModel<string>("Error deleting lead."));
            }
        }

       

    }
}
