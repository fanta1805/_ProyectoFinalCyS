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
        public int Ubicacion { get; set; }

        public ICollection<Horario> Horarios { get; set; }
        [Required]
        public int capacidad { get; set; }


        public Salas(string nombreSala, int ubicacion, int capacidad)
        {
            this.NombreSala = nombreSala;
            this.Ubicacion = ubicacion;
            this.Horarios = new List<Horario>();
            this.capacidad = capacidad;
        }

        public Salas() { }
    }

}
