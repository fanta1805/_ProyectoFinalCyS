using BE_ProyectoFinal.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;



namespace BE_ProyectoFinal.Controllers
{
    [Route("api/salas")]
    [ApiController]
    public class SalaController : Controller
    {

        private readonly ApplicationDbContext _context;
        public SalaController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpGet("salasDisponibles")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var listaSalas = await _context.Salas.ToListAsync();
                return Ok(listaSalas);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        // POST: SalaController/Create
        [HttpPost("agregar")]
        public async Task<IActionResult> Post([FromBody] SalaDTO sala)
        {
            try
            {
                Salas salaNueva = new Salas(sala.NombreSala, sala.Ubicacion, sala.capacidad);
                _context.Add(salaNueva);
                await _context.SaveChangesAsync();
                return Ok(salaNueva);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("actualizar")]
        public async Task<IActionResult> Put(int id, [FromBody] ReservaDTO reservaActualizada)
        {
            try
            {

                var reservaExistente = await _context.Reservas.FirstOrDefaultAsync(r => r.IdReserva == id);

                if (reservaExistente == null)
                {
                    return BadRequest(new { message = "No encontro la reserva" });
                }


                reservaExistente.UsuarioId = reservaActualizada.UsuarioId;
                reservaExistente.HoraInicio = reservaActualizada.HoraInicio;
                reservaExistente.HoraFin = reservaActualizada.HoraFin;
                reservaExistente.Prioridad = reservaActualizada.Prioridad;

                await _context.SaveChangesAsync();
                return Ok(new { message = "La reserva fue actualizada" });

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);

            }
        }



        // POST: SalaController/Delete/5


    }
}
