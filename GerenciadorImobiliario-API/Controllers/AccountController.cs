using Blog.Extensions;
using GerenciadorImobiliario_API.Data;
using GerenciadorImobiliario_API.Enums;
using GerenciadorImobiliario_API.Models;
using GerenciadorImobiliario_API.Services;
using GerenciadorImobiliario_API.ViewModels;
using GerenciadorImobiliario_API.ViewModels.Accounts;
using GerenciadorImobiliario_API.ViewModels.SubscriptionPlans;
using GerenciadorImobiliario_API.ViewModels.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

using Microsoft.Extensions.Logging;
using Blog.Services;

namespace GerenciadorImobiliario_API.Controllers
{
    [ApiController]
    [Route("v1/accounts")]
    public class AccountsController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly AppDbContext _context;
        private readonly TokenService _tokenService;
        private readonly ILogger<AccountsController> _logger;
        private readonly EmailService _emailService;

        public AccountsController(UserManager<User> userManager, AppDbContext context, TokenService tokenService, ILogger<AccountsController> logger, EmailService emailService)
        {
            _userManager = userManager;
            _context = context;
            _tokenService = tokenService;
            _logger = logger;
            _emailService = emailService;
        }

        private string GenerateSlugWithId(string name, string userId)
        {
            var slug = name.ToLower().Replace(" ", "-");
            return $"{slug}-{userId}";
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));
            }

            var user = new User
            {
                UserName = model.Email,
                Email = model.Email,
                Name = model.Name,
               
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return BadRequest(new ResultViewModel<string>(result.Errors.Select(e => e.Description).ToList()));
            }

            user.Slug = GenerateSlugWithId(user.Name, user.Id.ToString());


            await _userManager.UpdateAsync(user);

            var emailToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationLink = Url.Action("ConfirmEmail", "Accounts", new { userId = user.Id, token = emailToken }, Request.Scheme);

            
             _emailService.Send(
                user.Name,
                user.Email,
                subject: "Confirmação de E-mail",
                body: $"Por favor, confirme seu e-mail clicando no link: <a href='{confirmationLink}'>Confirmar E-mail</a>"
            );

            var freeTrialPlan = await _context.SubscriptionPlans
                .FirstOrDefaultAsync(sp => sp.Name == ESubscriptionPlan.FreeTrial);

            if (freeTrialPlan == null)
            {
                return StatusCode(500, new ResultViewModel<string>("Plano FreeTrial não encontrado."));
            }

            user.SubscriptionPlanId = freeTrialPlan.Id;
            user.TrialStartDate = DateTime.UtcNow;
            user.TrialEndDate = DateTime.UtcNow.AddDays(14);
            user.TrialUsed = false;
            await _userManager.UpdateAsync(user);

            return Ok(new ResultViewModel<string>("Usuário cadastrado com sucesso!", null));
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                return BadRequest(new ResultViewModel<string>("UserId ou token inválido."));
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound(new ResultViewModel<string>("Usuário não encontrado."));
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                return BadRequest(new ResultViewModel<string>(result.Errors.Select(e => e.Description).ToList()));
            }

            return Ok(new ResultViewModel<string>("E-mail confirmado com sucesso! Você pode fazer login agora.", null));
        }


        [HttpPost("login")]
        public async Task<IActionResult> Post([FromBody] LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
                return NotFound(new ResultViewModel<string>("Usuário não encontrado."));

            var passwordValid = await _userManager.CheckPasswordAsync(user, model.Password);

            if (!passwordValid)
                return BadRequest(new ResultViewModel<string>("Usuário ou senha inválida."));

            try
            {
                var token = _tokenService.GenerateToken(user);
                return Ok(new ResultViewModel<string>(token, null));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResultViewModel<string>(ex.Message));
            }
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetMe()
        {

            var authHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();

            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdString))
                return NotFound(new ResultViewModel<string>("Usuário não encontrado."));

            if (!long.TryParse(userIdString, out var userId))
            {
                return BadRequest(new ResultViewModel<string>("UserId inválido."));
            }

            var user = await _context.Users
                .Include(u => u.SubscriptionPlan)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return NotFound(new ResultViewModel<string>("Usuário não encontrado."));

            var userViewModel = new UserViewModel
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                UserName = user.UserName,
                SubscriptionPlan = user.SubscriptionPlan?.Name.ToString()
            };

            return Ok(new ResultViewModel<UserViewModel>(userViewModel));
        }

        [HttpGet("me/details")]
        [Authorize]
        public async Task<IActionResult> GetUserDetails()
        {
            _logger.LogInformation("Recebendo requisição para o endpoint GetUserDetails.");

            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _logger.LogInformation($"UserId encontrado: {userIdString}");

            if (string.IsNullOrEmpty(userIdString) || !long.TryParse(userIdString, out var userId))
            {
                return BadRequest(new ResultViewModel<string>("UserId inválido."));
            }

            var user = await _context.Users
                .Include(u => u.SubscriptionPlan)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return NotFound(new ResultViewModel<string>("Usuário não encontrado."));
            }

            var userDetailsViewModel = new UserDetailsViewModel
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                UserName = user.UserName,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                City = user.City,
                State = user.State,
                PostalCode = user.PostalCode,
                Creci = user.Creci,
                Specialties = user.Specialties,
                YearsOfExperience = user.YearsOfExperience,
                Description = user.Description,
                LinkedIn = user.LinkedIn,
                Instagram = user.Instagram,
                SubscriptionPlan = user.SubscriptionPlan?.Name.ToString()
            };

            return Ok(new ResultViewModel<UserDetailsViewModel>(userDetailsViewModel));
        }

        [HttpPut("update-profile")]
        [Authorize]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));
            }

            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdString) || !long.TryParse(userIdString, out var userId))
            {
                return BadRequest(new ResultViewModel<string>("UserId inválido."));
            }

            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
            {
                return NotFound(new ResultViewModel<string>("Usuário não encontrado."));
            }

            user.Name = model.Name;
            user.PhoneNumber = model.PhoneNumber;
            user.Address = model.Address;
            user.City = model.City;
            user.State = model.State;
            user.PostalCode = model.PostalCode;
            user.Creci = model.Creci;
            user.Specialties = model.Specialties;
            user.YearsOfExperience = model.YearsOfExperience;
            user.Description = model.Description;
            user.LinkedIn = model.LinkedIn;
            user.Instagram = model.Instagram;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return BadRequest(new ResultViewModel<string>(result.Errors.Select(e => e.Description).ToList()));
            }

            return Ok(new ResultViewModel<string>("Perfil atualizado com sucesso!", null));
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));
            }

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                return NotFound(new ResultViewModel<string>("Usuário não encontrado."));
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetLink = Url.Action("ResetPassword", "Accounts", new { token, email = user.Email }, Request.Scheme);


            _emailService.Send(
                user.Name,
                user.Email,
                subject: "Redefinição de Senha",
                body: $"Clique no link para redefinir sua senha: <a href='{resetLink}'>Redefinir Senha</a>"
            );

            return Ok(new ResultViewModel<string>("Link de redefinição de senha enviado com sucesso!", null));
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));
            }

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                return NotFound(new ResultViewModel<string>("Usuário não encontrado."));
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);

            if (!result.Succeeded)
            {
                return BadRequest(new ResultViewModel<string>(result.Errors.Select(e => e.Description).ToList()));
            }

            return Ok(new ResultViewModel<string>("Senha redefinida com sucesso!", null));
        }

        [HttpPost("resend-confirmation-email")]
        [Authorize]
        public async Task<IActionResult> ResendConfirmationEmail()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userIdString);

            if (user == null)
            {
                return NotFound(new ResultViewModel<string>("Usuário não encontrado."));
            }

            var emailToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationLink = Url.Action("ConfirmEmail", "Accounts", new { userId = user.Id, token = emailToken }, Request.Scheme);

            _emailService.Send(
                user.Name,
                user.Email,
                subject: "Confirmação de E-mail",
                body: $"Por favor, confirme seu e-mail clicando no link: <a href='{confirmationLink}'>Confirmar E-mail</a>"
            );

            return Ok(new ResultViewModel<string>("E-mail de confirmação reenviado com sucesso!", null));
        }

        [HttpDelete("{id}")]
        [Authorize] 
        public async Task<IActionResult> DeleteUser(long id)
        {
            
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (currentUserId == null || currentUserId != id.ToString())
            {
                return Forbid(); 
            }

            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return NotFound(new ResultViewModel<string>("Usuário não encontrado."));
            }

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest(new ResultViewModel<string>(result.Errors.Select(e => e.Description).ToList()));
            }

            return Ok(new ResultViewModel<string>("Usuário excluído com sucesso.", null));
        }


    }


}
