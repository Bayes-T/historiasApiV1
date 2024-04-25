using ApiHistorias.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ApiHistorias;

public class ApplicationDbContext: IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions options): base(options) { }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<ProfesionalPaciente>()
            .HasKey(al => new { al.ProfesionalId, al.PacienteId });
    }
    
    public DbSet<Profesional> Profesionales { get; set; }
    public DbSet<Paciente> Pacientes { get; set; }
    public DbSet<Historia> Historias { get; set; }
    public DbSet<ProfesionalPaciente> ProfesionalesPacientes { get; set; }
    
}