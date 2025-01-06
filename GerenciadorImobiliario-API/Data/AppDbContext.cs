using GerenciadorImobiliario_API.Data.Mappings;
using GerenciadorImobiliario_API.Enums;
using GerenciadorImobiliario_API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;

namespace GerenciadorImobiliario_API.Data
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<SubscriptionPlan> SubscriptionPlans { get; set; }
        public DbSet<UserSubscription> UserSubscriptions { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Lead> Leads { get; set; }
        public DbSet<LeadStatus> LeadStatuses { get; set; }
        public DbSet<PipelineStage> PipelineStages { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<PropertyPreference> PropertyPreferences { get; set; }
        public DbSet<Address> Address { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserMapping());
            modelBuilder.ApplyConfiguration(new ClientMapping());
            modelBuilder.ApplyConfiguration(new SubscriptionPlanMapping());
            modelBuilder.ApplyConfiguration(new UserSubscriptionMapping());
            modelBuilder.ApplyConfiguration(new LeadMapping());
            modelBuilder.ApplyConfiguration(new LeadStatusMapping());
            modelBuilder.ApplyConfiguration(new PipelineStageMapping());
            modelBuilder.ApplyConfiguration(new PropertyMapping());
            modelBuilder.ApplyConfiguration(new PropertyDocumentConfiguration());
            modelBuilder.ApplyConfiguration(new PropertyImageConfiguration());
            modelBuilder.ApplyConfiguration(new ClientPropertyInterestMapping());
            modelBuilder.ApplyConfiguration(new AddressMapping());

            modelBuilder.Entity<SubscriptionPlan>()
                        .Property(x => x.Name)
                        .HasConversion(new EnumToStringConverter<ESubscriptionPlan>());

            modelBuilder.Entity<LeadStatus>().HasData(
        new LeadStatus { Id = 1, Status = ELeadStatusEnum.NovoLead, Name = "Novo Lead" },
        new LeadStatus { Id = 2, Status = ELeadStatusEnum.Cliente, Name = "Cliente" }
    );

            modelBuilder.Entity<PipelineStage>().HasData(
                new PipelineStage { Id = 1, Stage = EPipelineStageEnum.EsperandoAtendimento, Name = "Esperando Atendimento" },
                new PipelineStage { Id = 2, Stage = EPipelineStageEnum.EmAtendimento, Name = "Em Atendimento" },
                new PipelineStage { Id = 3, Stage = EPipelineStageEnum.EmVisita, Name = "Em Visita" },
                new PipelineStage { Id = 4, Stage = EPipelineStageEnum.PropostaEnviada, Name = "Proposta Enviada" },
                new PipelineStage { Id = 5, Stage = EPipelineStageEnum.Documentacao, Name = "Documentação" },
                new PipelineStage { Id = 6, Stage = EPipelineStageEnum.VendaConcluida, Name = "Venda Concluída" },
                new PipelineStage { Id = 7, Stage = EPipelineStageEnum.VendaPerdida, Name = "Venda Perdida" }
            );

        }
    }
}
