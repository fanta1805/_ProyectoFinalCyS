using System.ComponentModel.DataAnnotations;

namespace BE_ProyectoFinal.Model
{
    public class Salas
    {
        [Key]
        public int IdSala { get; set; }

        [Required]
        public string NombreSala { get; set; }

        [Required]
        public string Ubicacion { get; set; }

        public ICollection<Horario> Horarios { get; set; }

        // Navegación a Reservas
        public ICollection<Reservas> Reservas { get; set; }
    }
}
