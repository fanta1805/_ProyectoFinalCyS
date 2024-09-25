using BE_ProyectoFinal.Model;
using LinqToDB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BE_ProyectoFinal.Controllers
{

    [Route("api/reserva/usuario")]
    [ApiController]
    public class ReservaController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ReservaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: ReservaController/Create
        [HttpGet("listaReservas")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var listaReservas = await _context.Reservas.ToListAsync();
                return Ok(listaReservas);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST: ReservaController/Edit/5
        [HttpPost("reservar")]
        public async Task<IActionResult> Post([FromBody] ReservaDTO nuevaReserva)
        {
            try
            {
                var sala = await _context.Salas.FindAsync(nuevaReserva.SalaId);
                var usuario = await _context.Usuarios.FindAsync(nuevaReserva.UsuarioId);

                if (sala == null)
                {
                    return NotFound("La sala especificada no existe.");
                }

                if (usuario == null)
                {
                    return NotFound("El usuario especificado no existe.");
                }

                var capacidadSala = await _context.Salas
                    .Where(r => r.IdSala == nuevaReserva.SalaId)
                    .Select(r => r.capacidad)
                    .FirstOrDefaultAsync();

                if (nuevaReserva.capacidad > capacidadSala)
                {
                    return BadRequest(new { message = "La capacidad solicitada excede la capacidad de la sala." });
                }

                var reservasExistentes = await _context.Reservas
                    .Where(r => r.SalaId == nuevaReserva.SalaId &&
                                r.HoraInicio.Date == nuevaReserva.HoraInicio.Date)
                    .ToListAsync();

                foreach (var reserva in reservasExistentes)
                {
                    bool hayInterseccion = !(nuevaReserva.HoraFin <= reserva.HoraInicio || nuevaReserva.HoraInicio >= reserva.HoraFin);

                    if (hayInterseccion)
                    {
                        if (nuevaReserva.Prioridad > reserva.Prioridad)
                        {
                            _context.Reservas.Remove(reserva);
                            await _context.SaveChangesAsync();
                            break;
                        }
                        else
                        {
                            return BadRequest("La nueva reserva se superpone con una reserva existente de igual o mayor prioridad.");
                        }
                    }
                }

                Reservas crearReserva = new Reservas(
                    nuevaReserva.SalaId,
                    nuevaReserva.UsuarioId,
                    nuevaReserva.HoraInicio,
                    nuevaReserva.HoraFin,
                    nuevaReserva.Prioridad
                );

                _context.Reservas.Add(crearReserva);
                await _context.SaveChangesAsync();

                // Devuelve un objeto JSON en lugar de texto plano
                return Ok(new { message = "Reserva creada con éxito." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error: {ex.Message}" });
            }
        }









        [HttpPut("actualizar/{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] ReservaDTO reservaActualizada)
        {
            try
            {

                var reservaExistente = await _context.Reservas.FirstOrDefaultAsync(r => r.IdReserva == id);

                if (reservaExistente == null) {
                    return BadRequest(new { message = "No encontro la reserva" });
                }

                reservaExistente.SalaId = reservaActualizada.SalaId;
                reservaExistente.UsuarioId = reservaActualizada.UsuarioId;
                reservaExistente.HoraInicio = reservaActualizada.HoraInicio;
                reservaExistente.HoraFin = reservaActualizada.HoraFin;
                reservaExistente.Prioridad = reservaActualizada.Prioridad;

                await _context.SaveChangesAsync();
                return Ok(new { message = "La reserva fue actualizada" });

            }
            catch (Exception ex) {

                return BadRequest(ex.Message);

            }
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> EliminarReserva(int id)
        {
            try
            {
                var reserva = await _context.Reservas.FindAsync(id);
                if (reserva == null)
                {
                    return NotFound(new { message = "Reserva no encontrada" });
                }

                _context.Reservas.Remove(reserva);
                await _context.SaveChangesAsync();
                return Ok(new { message = "Reserva eliminada correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
