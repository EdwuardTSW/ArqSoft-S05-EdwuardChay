using CitasApp.Domain.Interfaces;
using CitasApp.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace CitasApp.Web.Controllers
{
    public class PacienteController : Controller
    {
        private readonly IPacienteRepository _repo;
        public PacienteController(IPacienteRepository repo) { _repo = repo; }

        public IActionResult Index() => View(_repo.ObtenerTodos());

        public IActionResult Detalle(int id)
        {
            var paciente = _repo.ObtenerPorId(id);
            return paciente == null ? NotFound() : View(paciente);
        }

        public IActionResult Crear() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Crear(Paciente paciente)
        {
            if (!ModelState.IsValid) return View(paciente);

            _repo.Agregar(paciente);
            return RedirectToAction(nameof(Index));
        }
    }
}
