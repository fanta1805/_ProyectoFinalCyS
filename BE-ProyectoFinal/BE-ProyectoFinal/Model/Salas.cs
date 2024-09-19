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


        public Salas(string nombreSala, string ubicacion)
        {
            this.NombreSala = nombreSala;
            this.Ubicacion = ubicacion;
            this.Horarios = new List<Horario>();
        }
    }

}
