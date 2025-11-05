using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionColegioJose1.Models
{
    public class Alumno
    {
        [Key]
        public int AlumnoId { get; set; }

        [Required, StringLength(60)]
        public string Nombre { get; set; } = "";

        [Required, StringLength(60)]
        public string Apellido { get; set; } = "";

        [DataType(DataType.Date)]
        public DateTime? FechaNacimiento { get; set; }

        [StringLength(30)]
        public string? Grado { get; set; }

        public ICollection<Expediente> Expedientes { get; set; } = new List<Expediente>();

        [NotMapped]
        public string NombreCompleto => $"{Nombre} {Apellido}";
    }
}
