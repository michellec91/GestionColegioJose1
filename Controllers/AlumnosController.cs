using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestionColegioJose1.Data;
using GestionColegioJose1.Models;

namespace GestionColegioJose1.Controllers
{
    public class AlumnosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AlumnosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Alumnos
        public async Task<IActionResult> Index()
        {
            var alumnos = await _context.Alumnos.AsNoTracking().ToListAsync();
            return View(alumnos);
        }

        // GET: Alumnos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var alumno = await _context.Alumnos
                .Include(a => a.Expedientes)
                .ThenInclude(e => e.Materia)
                .FirstOrDefaultAsync(a => a.AlumnoId == id);

            if (alumno == null)
                return NotFound();

            return View(alumno);
        }

        // GET: Alumnos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Alumnos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Alumno alumno)
        {
            if (ModelState.IsValid)
            {
                _context.Add(alumno);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(alumno);
        }

        // GET: Alumnos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var alumno = await _context.Alumnos.FindAsync(id);
            if (alumno == null)
                return NotFound();

            return View(alumno);
        }

        // POST: Alumnos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Alumno alumno)
        {
            if (id != alumno.AlumnoId)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(alumno);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Alumnos.Any(e => e.AlumnoId == alumno.AlumnoId))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(alumno);
        }

        // GET: Alumnos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var alumno = await _context.Alumnos
                .FirstOrDefaultAsync(a => a.AlumnoId == id);

            if (alumno == null)
                return NotFound();

            return View(alumno);
        }

        // POST: Alumnos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var alumno = await _context.Alumnos.FindAsync(id);
            if (alumno != null)
            {
                _context.Alumnos.Remove(alumno);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
