using BE_ProyectoFinal.Model;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public DbSet<Salas> Salas { get; set; }
    public DbSet<Reservas> Reservas { get; set; }
    public DbSet<Usuarios> Usuarios { get; set; }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configuración explícita para Reservas
        modelBuilder.Entity<Reservas>()
            .HasKey(r => r.IdReserva);

        modelBuilder.Entity<Reservas>()
            .HasOne(r => r.Sala)
            .WithMany(s => s.Reservas)
            .HasForeignKey(r => r.SalaId);

        modelBuilder.Entity<Reservas>()
            .HasOne(r => r.Usuario)
            .WithMany(u => u.Reservas)
            .HasForeignKey(r => r.UsuarioId);
    }


}
