using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VisionBoard.Models;

namespace VisionBoard.Controllers
{
    public class MeasurementsController : Controller
    {
        private readonly GoalTrackerContext _context;

        public MeasurementsController(GoalTrackerContext context)
        {
            _context = context;
        }

        // GET: Mesurements
        public async Task<IActionResult> Index()
        {
            var goalTrackerContext = _context.Mesurements.Include(m => m.Goal);
            return View(await goalTrackerContext.ToListAsync());
        }

        // GET: Mesurements/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mesurement = await _context.Mesurements
                .Include(m => m.Goal)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mesurement == null)
            {
                return NotFound();
            }

            return View(mesurement);
        }

        // GET: Mesurements/Create
        public IActionResult Create()
        {
            ViewData["GoalId"] = new SelectList(_context.Goals, "Id", "Name");
            return View();
        }

        // POST: Mesurements/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,GoalId,Type,CurrentValue,TotalValue,Unit")] Mesurement mesurement)
        {
            if (ModelState.IsValid)
            {
                _context.Add(mesurement);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GoalId"] = new SelectList(_context.Goals, "Id", "Name", mesurement.GoalId);
            return View(mesurement);
        }

        // GET: Mesurements/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mesurement = await _context.Mesurements.FindAsync(id);
            if (mesurement == null)
            {
                return NotFound();
            }
            ViewData["GoalId"] = new SelectList(_context.Goals, "Id", "Name", mesurement.GoalId);
            return View(mesurement);
        }

        // POST: Mesurements/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,GoalId,Type,CurrentValue,TotalValue,Unit")] Mesurement mesurement)
        {
            if (id != mesurement.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mesurement);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MesurementExists(mesurement.Id))
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
            ViewData["GoalId"] = new SelectList(_context.Goals, "Id", "Name", mesurement.GoalId);
            return View(mesurement);
        }

        // GET: Mesurements/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mesurement = await _context.Mesurements
                .Include(m => m.Goal)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mesurement == null)
            {
                return NotFound();
            }

            return View(mesurement);
        }

        // POST: Mesurements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mesurement = await _context.Mesurements.FindAsync(id);
            _context.Mesurements.Remove(mesurement);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MesurementExists(int id)
        {
            return _context.Mesurements.Any(e => e.Id == id);
        }
    }
}
