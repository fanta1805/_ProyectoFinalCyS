using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BE_ProyectoFinal.Model
{
    public class Reservas
    {
        [Key]
        public int IdReserva { get; set; }

        [Required]
        public int SalaId { get; set; } // Clave foránea para Sala

        [ForeignKey("SalaId")]
        public Salas Sala { get; set; } // Navegación a Sala

        [Required]
        public int UsuarioId { get; set; } // Clave foránea para Usuario

        [ForeignKey("UsuarioId")]
        public Usuarios Usuario { get; set; } // Navegación a Usuario

        [Required]
        public DateTime HoraInicio { get; set; }

        [Required]
        public DateTime HoraFin { get; set; }

        [Required]
        public int Prioridad { get; set; }
        [Required]
        public int capacidad { get; set; }


        public Reservas(int salaId, int usuarioId, DateTime horaInicio, DateTime horaFin, int prioridad, int capacidad)
        {
            this.SalaId = salaId;
            this.UsuarioId = usuarioId;
            this.HoraInicio = horaInicio;
            this.HoraFin = horaFin;
            this.Prioridad = prioridad;
            this.capacidad = capacidad;

        }

        // Constructor sin parámetros (necesario para Entity Framework)
        public Reservas()
        {
        }

        public ICollection<Horario> getHorarios() { 
            return Sala.Horarios;
        }


    }


}
