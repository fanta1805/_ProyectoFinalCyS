namespace BE_ProyectoFinal.Model
{
    public class Horario
    {
        public int Id { get; set; }
        public DateTime Inicio { get; set; }
        public DateTime Fin { get; set; }

        // Relación con la sala
        public int SalaId { get; set; }
        public Salas Sala { get; set; }
    }
}
