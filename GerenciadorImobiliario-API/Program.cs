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
    Console.WriteLine($"JwtKey: {jwtKey}"); // Verifique se a chave está correta

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

        // Adicione eventos para rastrear problemas com validação
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine($"Autenticação falhou: {context.Exception.Message}");
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
                Features = new[] { "Acesso ao catálogo de imóveis", "Busca básica de imóveis", "Visualização de detalhes do imóvel" }
            },
            new SubscriptionPlan
            {
                Name = ESubscriptionPlan.Basic,
                Price = 19.99m,
                 Features = new[] { "Todas as funcionalidades do plano Free Trial", "Cadastro de imóveis", "Gerenciamento de imóveis", "Gerenciamento de clientes", "Suporte prioritário" }
            },
              new SubscriptionPlan
            {
                Name = ESubscriptionPlan.Premium,
                Price = 49.99m,
                   Features = new[] { "Todas as funcionalidades do plano Basic", "Relatórios avançados de desempenho", "Sistema de match entre preferências do cliente e imóveis", "Suporte prioritário" }
            },
            new SubscriptionPlan
            {
                Name = ESubscriptionPlan.Enterprise,
                Price = 99.99m,
                 Features = new[] { "Todas as funcionalidades do plano Premium", "Acesso à API para integração com outros sistemas", "Acesso a ferramentas de análise de conversas com IA", "Suporte prioritário" }
            }
        };

        context.SubscriptionPlans.AddRange(plans);
        context.SaveChanges();
    }
}
