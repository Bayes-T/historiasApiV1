using ApiHistorias.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApiHistorias;

public class ApplicationDbContext: DbContext
{
    public ApplicationDbContext(DbContextOptions options): base(options) { }
    
    public DbSet<Profesional> Profesionales { get; set; }
    public DbSet<Paciente> Pacientes { get; set; }
    public DbSet<Historia> Historias { get; set; }
}