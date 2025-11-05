using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GestionColegioJose1.Data;
using GestionColegioJose1.Models;

namespace GestionColegioJose1.Controllers
{
    public class ExpedientesController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ExpedientesController(ApplicationDbContext context) { _context = context; }

        public async Task<IActionResult> Index()
        {
            var lista = _context.Expedientes
                .Include(e => e.Alumno)
                .Include(e => e.Materia)
                .AsNoTracking();
            return View(await lista.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var expediente = await _context.Expedientes
                .Include(e => e.Alumno)
                .Include(e => e.Materia)
                .FirstOrDefaultAsync(e => e.ExpedienteId == id);
            if (expediente == null) return NotFound();
            return View(expediente);
        }

        public IActionResult Create()
        {
            LoadDropdowns();
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Expediente expediente)
        {
            if (!ModelState.IsValid)
            {
                LoadDropdowns(expediente.AlumnoId, expediente.MateriaId);
                return View(expediente);
            }

            bool existe = await _context.Expedientes
                .AnyAsync(e => e.AlumnoId == expediente.AlumnoId && e.MateriaId == expediente.MateriaId);
            if (existe)
            {
                ModelState.AddModelError("", "Ya existe un expediente para este alumno y materia.");
                LoadDropdowns(expediente.AlumnoId, expediente.MateriaId);
                return View(expediente);
            }

            _context.Add(expediente);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var expediente = await _context.Expedientes.FindAsync(id);
            if (expediente == null) return NotFound();
            LoadDropdowns(expediente.AlumnoId, expediente.MateriaId);
            return View(expediente);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Expediente expediente)
        {
            if (id != expediente.ExpedienteId) return NotFound();
            if (!ModelState.IsValid)
            {
                LoadDropdowns(expediente.AlumnoId, expediente.MateriaId);
                return View(expediente);
            }

            _context.Update(expediente);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var expediente = await _context.Expedientes
                .Include(e => e.Alumno)
                .Include(e => e.Materia)
                .FirstOrDefaultAsync(m => m.ExpedienteId == id);
            if (expediente == null) return NotFound();
            return View(expediente);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var expediente = await _context.Expedientes.FindAsync(id);
            if (expediente != null) _context.Expedientes.Remove(expediente);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Promedios()
        {
            var data = await _context.Expedientes
                .Include(e => e.Alumno)
                .GroupBy(e => new { e.AlumnoId, e.Alumno!.Nombre, e.Alumno!.Apellido })
                .Select(g => new PromedioVM
                {
                    AlumnoId = g.Key.AlumnoId,
                    Alumno = g.Key.Nombre + " " + g.Key.Apellido,
                    Promedio = g.Average(x => (double?)(x.NotaFinal ?? 0)) ?? 0
                })
                .OrderBy(x => x.Alumno)
                .ToListAsync();

            return View(data);
        }

        private void LoadDropdowns(int? alumnoId = null, int? materiaId = null)
        {
            ViewData["AlumnoId"] = new SelectList(
                _context.Alumnos
                        .Select(a => new { a.AlumnoId, Texto = a.Nombre + " " + a.Apellido }),
                "AlumnoId", "Texto", alumnoId);

            ViewData["MateriaId"] = new SelectList(
                _context.Materias,
                "MateriaId", "NombreMateria", materiaId);
        }
    }

    public class PromedioVM
    {
        public int AlumnoId { get; set; }
        public string Alumno { get; set; } = "";
        public double Promedio { get; set; }
    }
}
