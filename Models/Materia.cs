using System.ComponentModel.DataAnnotations;

namespace GestionColegioJose1.Models
{
    public class Materia
    {
        [Key]
        public int MateriaId { get; set; }

        [Required, StringLength(80)]
        public string NombreMateria { get; set; } = "";

        [StringLength(80)]
        public string? Docente { get; set; }

        public ICollection<Expediente> Expedientes { get; set; } = new List<Expediente>();
    }
}
