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
        public async Task<IActionResult> Post([FromBody] List<ReservaDTO> nuevasReserva)
        {
            try
            {
                bool flag = false;
                foreach (var resAgregado in nuevasReserva)
                {
                    foreach (var item in nuevasReserva)
                    {
                        if (!(resAgregado.Equals(item)))
                        {
                            if (!(resAgregado.HoraFin <= item.HoraInicio || resAgregado.HoraInicio >= item.HoraFin))
                            {
                                flag = true;
                            }
                        }
                    }
                }

                if (flag)
                {
                    return BadRequest("La nueva reserva se superpone con la reserva que se encuentra en tu lista temporal de reservas");
                }

                foreach (var res in nuevasReserva)
                {
                    var usuario = await _context.Usuarios.FindAsync(res.UsuarioId);
                    if (usuario == null)
                    {
                        return NotFound("El usuario especificado no existe.");
                    }

                    var salas = await _context.Salas.ToListAsync();
                    Salas salaMejor = null;

                    foreach (var item in salas)
                    {
                        var capacidadSala = await _context.Salas
                            .Where(r => r.IdSala == item.IdSala)
                            .Select(r => r.capacidad)
                            .FirstOrDefaultAsync();

                        var piso = await _context.Salas
                            .Where(r => r.IdSala == item.IdSala)
                            .Select(r => r.Ubicacion)
                            .FirstOrDefaultAsync();

                        if (res.capacidad > capacidadSala)
                        {
                            continue;
                        }

                        if (salaMejor == null)
                        {
                            salaMejor = item;
                        }
                        else
                        {
                            if ((salaMejor.capacidad - res.capacidad) > (item.capacidad - res.capacidad))
                            {
                                if ((salaMejor.Ubicacion - usuario.piso) > (item.Ubicacion - usuario.piso))
                                {
                                    salaMejor = item;
                                }
                            }
                        }

                        var reservasExistentes = await _context.Reservas
                            .Where(r => r.SalaId == salaMejor.IdSala &&
                                        r.HoraInicio.Date == res.HoraInicio.Date)
                            .ToListAsync();

                        bool salaOcupada = false;

                        foreach (var reserva in reservasExistentes)
                        {
                            bool hayInterseccion = !(res.HoraFin <= reserva.HoraInicio || res.HoraInicio >= reserva.HoraFin);

                            if (hayInterseccion)
                            {
                                salaOcupada = true;

                                if (res.Prioridad > reserva.Prioridad)
                                {
                  
                                    _context.Reservas.Remove(reserva);
                                    await _context.SaveChangesAsync();
                                    salaOcupada = false; 
                                    break;
                                }

                            }
                        }

                        if (salaOcupada)
                        {
                            salaMejor = null;  
                            continue;
                        }

                        if (!salaOcupada)
                        {
                            break;
                        }
                    }

                    if (salaMejor == null)
                    {
                        return BadRequest("No se encontró ninguna sala disponible para las horas solicitadas.");
                    }

                    var existeReserva = await _context.Reservas
                        .AnyAsync(r => r.SalaId == salaMejor.IdSala &&
                                       r.UsuarioId == res.UsuarioId &&
                                       r.HoraInicio == res.HoraInicio &&
                                       r.HoraFin == res.HoraFin);

                    if (existeReserva)
                    {
                        return BadRequest("Ya existe una reserva con los mismos parámetros.");
                    }

                    Reservas crearReserva = new Reservas(
                        salaMejor.IdSala,
                        res.UsuarioId,
                        res.HoraInicio,
                        res.HoraFin,
                        res.Prioridad,
                        res.capacidad
                    );

                    _context.Reservas.Add(crearReserva);
                }

                await _context.SaveChangesAsync();

                return Ok(new { message = "Reservas creadas con éxito." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error: {ex.Message}" });
            }



        }



        [HttpPut("actualizar/{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] ReservaDTO nuevaReserva)
        {
            try
            {
                var reservaExistente = await _context.Reservas.FirstOrDefaultAsync(r => r.IdReserva == id);

                if (reservaExistente == null)
                {
                    return BadRequest(new { message = "No se encontró la reserva" });
                }

                var reservasExistentes = await _context.Reservas
                    .Where(r => r.SalaId == reservaExistente.SalaId &&
                                r.HoraInicio.Date == nuevaReserva.HoraInicio.Date &&
                                r.IdReserva != id) // Excluir la reserva actual
                    .ToListAsync();

                bool salaOcupada = false;

                foreach (var reserva in reservasExistentes)
                {
                    bool hayInterseccion = !(nuevaReserva.HoraFin <= reserva.HoraInicio || nuevaReserva.HoraInicio >= reserva.HoraFin);

                    if (hayInterseccion)
                    {
                        salaOcupada = true;

                        if (nuevaReserva.Prioridad > reserva.Prioridad)
                        {
                            _context.Reservas.Remove(reserva);
                            await _context.SaveChangesAsync();
                            salaOcupada = false; 
                            break;
                        }
                    }
                }

              
                if (salaOcupada)
                {
                    return BadRequest(new { message = "La nueva reserva se superpone con otra reserva de mayor o igual prioridad." });
                }

                reservaExistente.HoraInicio = nuevaReserva.HoraInicio;
                reservaExistente.HoraFin = nuevaReserva.HoraFin;
                reservaExistente.Prioridad = nuevaReserva.Prioridad;

                await _context.SaveChangesAsync();

                return Ok(new { message = "La reserva fue actualizada con éxito" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error: {ex.Message}" });
            }
        }


        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> delete(int id)
        {
            try
            {
                var reserva = await _context.Reservas.FindAsync(id);

                if (reserva == null)
                {
                    return NotFound(new { message = "Reserva no encontrada." });
                }

                _context.Reservas.Remove(reserva);
                await _context.SaveChangesAsync();
                return Ok(new { message = "Reserva eliminada correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error: {ex.Message}" });
            }
        }



    }
}
