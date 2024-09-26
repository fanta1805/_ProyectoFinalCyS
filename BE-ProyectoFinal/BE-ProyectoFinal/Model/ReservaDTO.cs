namespace BE_ProyectoFinal.Model
{
    public class ReservaDTO
    {
        public int UsuarioId { get; set; }
        public DateTime HoraInicio { get; set; }
        public DateTime HoraFin { get; set; }
        public int Prioridad { get; set; }
        public int capacidad{ get; set; }
    }
}
