using CitasApp.Domain.Interfaces;
using CitasApp.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace CitasApp.Web.Controllers
{
    public class CitaController : Controller
    {
        private readonly ICitaRepository _citaRepo;
        private readonly IPacienteRepository _pacienteRepo;
        private readonly IMedicoRepository _medicoRepo;

        public CitaController(ICitaRepository citaRepo,
                              IPacienteRepository pacienteRepo,
                              IMedicoRepository medicoRepo)
        {
            _citaRepo = citaRepo;
            _pacienteRepo = pacienteRepo;
            _medicoRepo = medicoRepo;
        }

        public IActionResult Index()
        {
            ViewBag.Pacientes = _pacienteRepo.ObtenerTodos();
            ViewBag.Medicos = _medicoRepo.ObtenerTodos();
            return View(_citaRepo.ObtenerTodos());
        }

        public IActionResult PorPaciente(int pacienteId)
        {
            ViewBag.Pacientes = _pacienteRepo.ObtenerTodos();
            ViewBag.Medicos = _medicoRepo.ObtenerTodos();
            return View(_citaRepo.ObtenerPorPaciente(pacienteId));
        }

        public IActionResult Crear()
        {
            CargarListas();
            return View(new Cita { Fecha = DateOnly.FromDateTime(DateTime.Today), Hora = new TimeOnly(9, 0) });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Crear(Cita cita)
        {
            if (!ModelState.IsValid)
            {
                CargarListas();
                return View(cita);
            }

            if (string.IsNullOrWhiteSpace(cita.Estado)) cita.Estado = "Pendiente";
            _citaRepo.Agregar(cita);
            return RedirectToAction(nameof(Index));
        }

        private void CargarListas()
        {
            ViewBag.Pacientes = _pacienteRepo.ObtenerTodos();
            ViewBag.Medicos = _medicoRepo.ObtenerTodos();
        }
    }
}
