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

        // POST: SalaController/Create
        [HttpPost("agregar")]
        public async Task<IActionResult> Post([FromBody] Salas sala)
        {
            try
            {
                _context.Add(sala);
                await _context.SaveChangesAsync();
                return Ok(sala);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        // GET: SalaController
        public ActionResult Index()
        {
            return View();
        }

        // GET: SalaController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SalaController/Create
        public ActionResult Create()
        {
            return View();
        }

        // GET: SalaController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

 
        // GET: SalaController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SalaController/Delete/5
       
        
    }
}
