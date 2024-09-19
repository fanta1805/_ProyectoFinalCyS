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
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: ReservaController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ReservaController/Edit/5
        [HttpPost("reservar")]
        public async Task<IActionResult> Post([FromBody] ReservaDTO nuevaReserva)
        {
            try
            {
                var reservasExistentes = await _context.Reservas
                    .Where(r => r.SalaId == nuevaReserva.SalaId &&
                                r.HoraInicio.Date == nuevaReserva.HoraInicio.Date)
                    .ToListAsync();

                foreach (var reserva in reservasExistentes)
                {
                    if (!(nuevaReserva.HoraFin < reserva.HoraInicio || nuevaReserva.HoraInicio > reserva.HoraFin))
                    {
                        if (nuevaReserva.Prioridad > reserva.Prioridad)
                        {
                            foreach (var hora in reserva.Sala.Horarios) {
                                if (hora.Equals(reserva.HoraInicio) || hora.Equals(reserva.HoraFin)) { 
                                    reserva.Sala.Horarios.Remove(hora);
                                }
                            }
                            reserva.Sala.Horarios.Add(new Horario(nuevaReserva.HoraInicio, nuevaReserva.HoraFin, nuevaReserva.SalaId));
                            _context.Reservas.Remove(reserva);
                            await _context.SaveChangesAsync();
                        }
                        else
                        {
                            return BadRequest("La nueva reserva se superpone con una reserva existente de igual o mayor prioridad.");
                        }
                    }
                }

                Reservas crearReserva = new Reservas(nuevaReserva.SalaId, nuevaReserva.UsuarioId, nuevaReserva.HoraInicio, nuevaReserva.HoraFin, nuevaReserva.Prioridad);

                _context.Reservas.Add(crearReserva);
                await _context.SaveChangesAsync();
                return Ok("Reserva creada con éxito.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        // GET: ReservaController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ReservaController/Delete/5
        /*[HttpPut("{id}")]
        public async Task<IActionResult> ActualizarReserva(int id, [FromBody] Reserva reserva)
        {
            // Actualizar reserva existente
            // Devolver respuesta
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarReserva(int id)
        {
            // Eliminar reserva
            // Devolver respuesta
        }*/
    }
}
