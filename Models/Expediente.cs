using System.ComponentModel.DataAnnotations;

namespace GestionColegioJose1.Models
{
    public class Expediente
    {
        [Key]
        public int ExpedienteId { get; set; }

        [Required] public int AlumnoId { get; set; }
        [Required] public int MateriaId { get; set; }

        [Range(0, 10)]
        [Display(Name = "Nota final")]
        public decimal? NotaFinal { get; set; }

        [StringLength(500)]
        public string? Observaciones { get; set; }

        public Alumno? Alumno { get; set; }
        public Materia? Materia { get; set; }
    }
}
