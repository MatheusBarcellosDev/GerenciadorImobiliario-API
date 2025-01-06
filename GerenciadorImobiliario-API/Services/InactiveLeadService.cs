using GerenciadorImobiliario_API.Data;
using GerenciadorImobiliario_API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GerenciadorImobiliario_API.Services
{
    public class InactiveLeadService : BackgroundService
    {
        private readonly ILogger<InactiveLeadService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;

        public InactiveLeadService(ILogger<InactiveLeadService> logger, IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Serviço de verificação de leads inativos iniciado.");

            var schedulerFactory = _serviceProvider.GetRequiredService<ISchedulerFactory>();
            var scheduler = await schedulerFactory.GetScheduler(stoppingToken);

            var job = JobBuilder.Create<InactiveLeadJob>()
                .WithIdentity("InactiveLeadJob", "InactiveLeadGroup")
                .Build();

            // Obtém o horário de execução a partir da configuração
            var time = _configuration.GetValue<string>("InactiveLeadCheckTime", "08:00");
            var hour = int.Parse(time.Split(':')[0]);
            var minute = int.Parse(time.Split(':')[1]);

            // Obtém o fuso horário do Brasil (Horário de Brasília)
            var brazilTimeZone = TimeZoneInfo.FindSystemTimeZoneById("America/Sao_Paulo");

            // Cria um DateTime com o horário desejado no fuso horário do Brasil
            var brazilTime = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day, hour, minute, 0);
            var brazilTimeWithTimeZone = TimeZoneInfo.ConvertTimeToUtc(brazilTime, brazilTimeZone);

            // Obtém o horário em UTC para agendar o job
            var utcHour = brazilTimeWithTimeZone.Hour;
            var utcMinute = brazilTimeWithTimeZone.Minute;

            var trigger = Quartz.TriggerBuilder.Create()
                .WithIdentity("InactiveLeadTrigger", "InactiveLeadGroup")
                .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(utcHour, utcMinute))
                .Build();

            await scheduler.ScheduleJob(job, trigger, stoppingToken);
            await scheduler.Start(stoppingToken);

            await Task.CompletedTask;
            _logger.LogInformation("Serviço de verificação de leads inativos agendado.");
        }
    }

    public class InactiveLeadJob : IJob
    {
        private readonly ILogger<InactiveLeadJob> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;

        public InactiveLeadJob(ILogger<InactiveLeadJob> logger, IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _configuration = configuration;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("Job de verificação de leads inativos iniciado.");

            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var daysInactive = _configuration.GetValue<int>("InactiveLeadDays", 5);

                try
                {
                    var leads = await dbContext.Leads
                        .Where(l => l.IsActive)
                        .ToListAsync();

                    foreach (var lead in leads)
                    {
                        if (IsLeadInactive(lead, daysInactive))
                        {
                            lead.IsActive = false;
                            _logger.LogInformation($"Lead {lead.Id} marcado como inativo.");
                        }
                        else if (!IsLeadInactive(lead, daysInactive) && !lead.IsActive)
                        {
                            lead.IsActive = true;
                            _logger.LogInformation($"Lead {lead.Id} marcado como ativo.");
                        }
                    }

                    await dbContext.SaveChangesAsync();
                    _logger.LogInformation("Job de verificação de leads inativos concluído.");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Erro ao verificar leads inativos: {ex.Message}");
                }
            }
        }

        private bool IsLeadInactive(Lead lead, int daysInactive)
        {
            if (lead.LastInteractionDate == null) return false;

            var timeSpan = DateTime.UtcNow - lead.LastInteractionDate.Value;
            return timeSpan.TotalDays > daysInactive;
        }
    }
}
