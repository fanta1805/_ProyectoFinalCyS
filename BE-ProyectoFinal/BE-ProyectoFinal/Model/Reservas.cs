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

        
        private DateTime _horaInicio { get; set; }

        
        private DateTime _horaFin { get; set; }

        [Required]
        public DateTime HoraInicio
        {
            get => _horaInicio;
            set => _horaInicio = DateTime.SpecifyKind(value, DateTimeKind.Utc);
        }

        [Required]
        public DateTime HoraFin
        {
            get => _horaFin;
            set => _horaFin = DateTime.SpecifyKind(value, DateTimeKind.Utc);
        }

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
