using CitasApp.Domain.Interfaces;
using CitasApp.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace CitasApp.Web.Controllers
{
    public class MedicoController : Controller
    {
        private readonly IMedicoRepository _repo;
        public MedicoController(IMedicoRepository repo) { _repo = repo; }

        public IActionResult Index() => View(_repo.ObtenerTodos());

        public IActionResult Detalle(int id)
        {
            var medico = _repo.ObtenerPorId(id);
            return medico == null ? NotFound() : View(medico);
        }

        public IActionResult Crear() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Crear(Medico medico)
        {
            if (!ModelState.IsValid) return View(medico);

            _repo.Agregar(medico);
            return RedirectToAction(nameof(Index));
        }
    }
}
