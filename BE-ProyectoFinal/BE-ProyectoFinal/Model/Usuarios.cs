using System.ComponentModel.DataAnnotations;

namespace BE_ProyectoFinal.Model
{
    public class Usuarios
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string email { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Rol { get; set; }
        [Required]
        public string contrasena { get; set; }

        // Navegación a Reservas
        public ICollection<Reservas> Reservas { get; set; }

        public Usuarios(string email, string name, string rol, string contrasena)
        {
            this.email = email;
            this.Name = name;
            this.Rol = rol;
            this.contrasena = contrasena;
            this.Reservas = new List<Reservas>(); // Inicializa la colección
        }
    }

}
