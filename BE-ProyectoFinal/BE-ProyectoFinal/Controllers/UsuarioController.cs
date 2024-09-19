using BE_ProyectoFinal.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BE_ProyectoFinal.Controllers
{
    [Route("api/usuario")]
    [ApiController]
    public class UsuarioController : Controller

    {
        private readonly ApplicationDbContext _context;
        public UsuarioController(ApplicationDbContext context) {
            _context = context;
        }

        /*En este metodo lo que hacemos es registrar al usuario a la base de datos y verificar que no este*/
        [HttpPost("registro")]
        public async Task<IActionResult> Post([FromBody] RegistroDTO registroUser)
        {
            bool userExists = await _context.Usuarios.AnyAsync(u => u.email == registroUser.email);
            if (!userExists)
            {
                Usuarios nuevoUsuario = new Usuarios
                (
                    registroUser.email,
                    registroUser.Name,
                    registroUser.Rol,
                    registroUser.contrasena
                );
                _context.Add(nuevoUsuario);
                await _context.SaveChangesAsync();
                return Ok(nuevoUsuario);
            }
            else
            {
                return BadRequest(new { message = "Nombre de usuario existente" });
            }
        }

        [HttpPost("autenticacion")]
        /*Verifica que el email y la contrasena pasada por parametro sean validas*/
        public async Task<IActionResult> Post([FromBody] AutenticacionDTO user)
        {
            // Espera a que se resuelva la tarea de AnyAsync
            bool userA = await _context.Usuarios.AnyAsync(u => u.email == user.email && u.contrasena == user.contrasena);

            if (userA)
            {
                return Ok(userA);
            }
            else {
                return BadRequest(new { message = "No se encontro el usuario" });
            }

            // Retorna el resultado como una respuesta Ok
        }




        // GET: UsuarioController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UsuarioController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UsuarioController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UsuarioController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
