using Blog.Services;
using GerenciadorImobiliario_API;
using GerenciadorImobiliario_API.Data;
using GerenciadorImobiliario_API.Enums;
using GerenciadorImobiliario_API.Models;
using GerenciadorImobiliario_API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;


var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddUserSecrets<Program>();
ConfigureAuthentication(builder);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? string.Empty;

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(
    x => x.CustomSchemaIds(y => y.FullName)
    );

ConfigureServices(builder);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var app = builder.Build();
LoadConfiguration(app);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseSwagger();
app.UseSwaggerUI();

app.Run();


void ConfigureServices(WebApplicationBuilder builder)
{
    builder.Services.AddDbContext<AppDbContext>(
        x => x.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
        );
    builder.Services
        .AddIdentityCore<User>()
        .AddRoles<IdentityRole<long>>()
    .AddEntityFrameworkStores<AppDbContext>()
        .AddApiEndpoints();
    builder.Services.AddTransient<TokenService>();
    builder.Services.AddTransient<EmailService>();

    SeedData(builder);
}

void LoadConfiguration(WebApplication app)
{
    Configuration.JwtKey = app.Configuration.GetValue<string>("JwtKey");

    var smtp = new Configuration.SmtpConfigurarion();
    app.Configuration.GetSection("Smtp").Bind(smtp);
    Configuration.Smtp = smtp;
}

void ConfigureAuthentication(WebApplicationBuilder builder)
{
    var jwtKey = builder.Configuration["JwtKey"];
    var key = Encoding.ASCII.GetBytes(jwtKey);
    Console.WriteLine($"JwtKey: {jwtKey}"); // Verifique se a chave est� correta

    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false
        };

        // Adicione eventos para rastrear problemas com valida��o
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine($"Autentica��o falhou: {context.Exception.Message}");
                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                Console.WriteLine("Token validado com sucesso.");
                return Task.CompletedTask;
            }
        };
    });
}



static void SeedData(WebApplicationBuilder builder)
{
    using var scope = builder.Services.BuildServiceProvider().CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    if (!context.SubscriptionPlans.Any())
    {
        var plans = new List<SubscriptionPlan>
        {
            new SubscriptionPlan
            {
                Name = ESubscriptionPlan.FreeTrial,
                Price = 0,
                Features = new[] { "Acesso ao cat�logo de im�veis", "Busca b�sica de im�veis", "Visualiza��o de detalhes do im�vel" }
            },
            new SubscriptionPlan
            {
                Name = ESubscriptionPlan.Basic,
                Price = 19.99m,
                 Features = new[] { "Todas as funcionalidades do plano Free Trial", "Cadastro de im�veis", "Gerenciamento de im�veis", "Gerenciamento de clientes", "Suporte priorit�rio" }
            },
              new SubscriptionPlan
            {
                Name = ESubscriptionPlan.Premium,
                Price = 49.99m,
                   Features = new[] { "Todas as funcionalidades do plano Basic", "Relat�rios avan�ados de desempenho", "Sistema de match entre prefer�ncias do cliente e im�veis", "Suporte priorit�rio" }
            },
            new SubscriptionPlan
            {
                Name = ESubscriptionPlan.Enterprise,
                Price = 99.99m,
                 Features = new[] { "Todas as funcionalidades do plano Premium", "Acesso � API para integra��o com outros sistemas", "Acesso a ferramentas de an�lise de conversas com IA", "Suporte priorit�rio" }
            }
        };

        context.SubscriptionPlans.AddRange(plans);
        context.SaveChanges();
    }
}
