using Microsoft.EntityFrameworkCore;
using GestionColegioJose1.Models;

namespace GestionColegioJose1.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Alumno> Alumnos => Set<Alumno>();
        public DbSet<Materia> Materias => Set<Materia>();
        public DbSet<Expediente> Expedientes => Set<Expediente>();

        protected override void OnModelCreating(ModelBuilder mb)
        {
            base.OnModelCreating(mb);

            mb.Entity<Alumno>().HasKey(a => a.AlumnoId);
            mb.Entity<Materia>().HasKey(m => m.MateriaId);
            mb.Entity<Expediente>().HasKey(e => e.ExpedienteId);

            mb.Entity<Expediente>()
              .HasOne(e => e.Alumno)
              .WithMany(a => a.Expedientes)
              .HasForeignKey(e => e.AlumnoId)
              .OnDelete(DeleteBehavior.Cascade);

            mb.Entity<Expediente>()
              .HasOne(e => e.Materia)
              .WithMany(m => m.Expedientes)
              .HasForeignKey(e => e.MateriaId)
              .OnDelete(DeleteBehavior.Cascade);

            mb.Entity<Expediente>()
              .HasIndex(e => new { e.AlumnoId, e.MateriaId })
              .IsUnique();
        }
    }
}
