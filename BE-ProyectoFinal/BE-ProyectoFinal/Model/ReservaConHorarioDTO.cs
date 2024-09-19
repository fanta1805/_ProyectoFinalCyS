namespace BE_ProyectoFinal.Model
{
    public class ReservaConHorarioDTO
    {
        public int SalaId { get; set; }
        public ReservaDTO Reserva { get; set; }
        public HorarioDTO Horario { get; set; }
    }
}
