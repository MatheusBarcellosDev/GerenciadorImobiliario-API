using Blog.Extensions;
using GerenciadorImobiliario_API.Data;
using GerenciadorImobiliario_API.Models;
using GerenciadorImobiliario_API.ViewModels.SubscriptionPlans;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GerenciadorImobiliario_API.Controllers
{
    [ApiController]
    [Route("v1/subscription-plans")]
    public class SubscriptionPlansController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SubscriptionPlansController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateUpdateSubscriptionPlanViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));
            }

            var subscriptionPlan = new SubscriptionPlan
            {
                Name = model.Name,
                Price = model.Price,
                Features = model.Features
            };

            _context.SubscriptionPlans.Add(subscriptionPlan);
            await _context.SaveChangesAsync();

            return Ok(new ResultViewModel<SubscriptionPlanViewModel>(MapToViewModel(subscriptionPlan)));
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var subscriptionPlans = await _context.SubscriptionPlans.ToListAsync();
            return Ok(new ResultViewModel<List<SubscriptionPlanViewModel>>(subscriptionPlans.Select(MapToViewModel).ToList()));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            var subscriptionPlan = await _context.SubscriptionPlans.FindAsync(id);
            if (subscriptionPlan == null)
            {
                return NotFound(new ResultViewModel<string>("Plano não encontrado."));
            }
            return Ok(new ResultViewModel<SubscriptionPlanViewModel>(MapToViewModel(subscriptionPlan)));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(long id, [FromBody] CreateUpdateSubscriptionPlanViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));
            }

            var subscriptionPlan = await _context.SubscriptionPlans.FindAsync(id);
            if (subscriptionPlan == null)
            {
                return NotFound(new ResultViewModel<string>("Plano não encontrado."));
            }

            subscriptionPlan.Name = model.Name;
            subscriptionPlan.Price = model.Price;
            subscriptionPlan.Features = model.Features;

            _context.SubscriptionPlans.Update(subscriptionPlan);
            await _context.SaveChangesAsync();

            return Ok(new ResultViewModel<SubscriptionPlanViewModel>(MapToViewModel(subscriptionPlan)));
        }

        private SubscriptionPlanViewModel MapToViewModel(SubscriptionPlan subscriptionPlan)
        {
            return new SubscriptionPlanViewModel
            {
                Id = subscriptionPlan.Id,
                Name = subscriptionPlan.Name,
                Price = subscriptionPlan.Price,
                Features = subscriptionPlan.Features
            };
        }
    }
}
