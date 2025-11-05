using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestionColegioJose1.Data;
using GestionColegioJose1.Models;

namespace GestionColegioJose1.Controllers
{
    public class MateriasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MateriasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Materias
        public async Task<IActionResult> Index()
        {
            var materias = await _context.Materias.AsNoTracking().ToListAsync();
            return View(materias);
        }

        // GET: Materias/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var materia = await _context.Materias
                .Include(m => m.Expedientes)
                .ThenInclude(e => e.Alumno)
                .FirstOrDefaultAsync(m => m.MateriaId == id);

            if (materia == null)
                return NotFound();

            return View(materia);
        }

        // GET: Materias/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Materias/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Materia materia)
        {
            if (ModelState.IsValid)
            {
                _context.Add(materia);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(materia);
        }

        // GET: Materias/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var materia = await _context.Materias.FindAsync(id);
            if (materia == null)
                return NotFound();

            return View(materia);
        }

        // POST: Materias/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Materia materia)
        {
            if (id != materia.MateriaId)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(materia);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Materias.Any(e => e.MateriaId == materia.MateriaId))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(materia);
        }

        // GET: Materias/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var materia = await _context.Materias
                .FirstOrDefaultAsync(m => m.MateriaId == id);

            if (materia == null)
                return NotFound();

            return View(materia);
        }

        // POST: Materias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var materia = await _context.Materias.FindAsync(id);
            if (materia != null)
            {
                _context.Materias.Remove(materia);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
