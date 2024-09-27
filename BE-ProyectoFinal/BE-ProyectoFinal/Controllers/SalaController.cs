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

        [HttpPut("actualizar/{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] SalaDTO salaDTO)
        {
            try
            {

                var sala = await _context.Salas.FirstOrDefaultAsync(r => r.IdSala == id);

                if (sala == null)
                {
                    return BadRequest(new { message = "No encontro la sala" });
                }


                sala.NombreSala = salaDTO.NombreSala;
                sala.Ubicacion = salaDTO.Ubicacion;
                sala.capacidad = salaDTO.capacidad;

                await _context.SaveChangesAsync();
                return Ok(new { message = "La sala fue actualizada" });

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);

            }
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> delete(int id)
        {
            try
            {
                var sala = await _context.Salas.FindAsync(id);

                if (sala == null)
                {
                    return NotFound(new { message = "Sala no encontrada." });
                }

                var reservasExistentes = await _context.Reservas
                .Where(r => r.SalaId == id)
                .ToListAsync();

                foreach (var item in reservasExistentes)
                {
                    _context.Reservas.Remove(item);
                    await _context.SaveChangesAsync();


                }

                _context.Salas.Remove(sala);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Sala eliminada" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error: {ex.Message}" });
            }
        }



        // POST: SalaController/Delete/5


    }
}
