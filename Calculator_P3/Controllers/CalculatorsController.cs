using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Calculator_P3.Data;
using Calculator_P3.Models;

namespace Calculator_P3.Controllers
{
    public class CalculatorsController : Controller
    {
        private readonly CalculatorDbContext _context;

        public CalculatorsController(CalculatorDbContext context)
        {
            _context = context;
        }

        // GET: Calculators
        public async Task<IActionResult> Index()
        {
              return _context.Calculators != null ? 
                          View(await _context.Calculators.ToListAsync()) :
                          Problem("Entity set 'CalculatorDbContext.Calculators'  is null.");
        }

        // GET: Calculators/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Calculators == null)
            {
                return NotFound();
            }

            var calculator = await _context.Calculators
                .FirstOrDefaultAsync(m => m.ID == id);
            if (calculator == null)
            {
                return NotFound();
            }

            return View(calculator);
        }

        // GET: Calculators/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Calculators/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Calcul,Rezultat,Data")] Calculator calculator)
        {
            if (ModelState.IsValid)
            {
                calculator.ID = Guid.NewGuid();
                _context.Add(calculator);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(calculator);
        }

        // GET: Calculators/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Calculators == null)
            {
                return NotFound();
            }

            var calculator = await _context.Calculators.FindAsync(id);
            if (calculator == null)
            {
                return NotFound();
            }
            return View(calculator);
        }

        // POST: Calculators/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("ID,Calcul,Rezultat,Data")] Calculator calculator)
        {
            if (id != calculator.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(calculator);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CalculatorExists(calculator.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(calculator);
        }

        // GET: Calculators/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Calculators == null)
            {
                return NotFound();
            }

            var calculator = await _context.Calculators
                .FirstOrDefaultAsync(m => m.ID == id);
            if (calculator == null)
            {
                return NotFound();
            }

            return View(calculator);
        }

        // POST: Calculators/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Calculators == null)
            {
                return Problem("Entity set 'CalculatorDbContext.Calculators'  is null.");
            }
            var calculator = await _context.Calculators.FindAsync(id);
            if (calculator != null)
            {
                _context.Calculators.Remove(calculator);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CalculatorExists(Guid id)
        {
          return (_context.Calculators?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
