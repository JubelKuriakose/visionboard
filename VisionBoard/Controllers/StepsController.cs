﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VisionBoard.DAL;
using VisionBoard.Models;
using VisionBoard.Utilis;

namespace VisionBoard.Controllers
{
    public class StepsController : Controller
    {
        private readonly IStepRepository stepsRepo;
        private readonly IGoalRepository goalRepo;

        public StepsController(IStepRepository stepsRepo, IGoalRepository goalRepo)
        {
            this.stepsRepo = stepsRepo;
            this.goalRepo = goalRepo;
        }


        // GET: Steps
        public async Task<IActionResult> Index()
        {
            var steps = await stepsRepo.GetAllSteps();
            return View(steps);
            //var goalTrackerContext = _context.Steps.Include(s => s.Goal);
            //return View(await goalTrackerContext.ToListAsync());
        }


        // GET: Steps/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var step = stepsRepo.GetStep((int)id);
            //var step = await _context.Steps
            //    .Include(s => s.Goal)
            //    .FirstOrDefaultAsync(m => m.Id == id);
            if (step == null)
            {
                return NotFound();
            }

            return View(step);
        }

        // GET: Steps/Create
        public async Task<IActionResult> Create()
        {
            ViewData["GoalId"] = new SelectList(await goalRepo.GetAllGoals(), "Id", "Name");
            return View();
        }


        // POST: Steps/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Weight,DueDate,Status,GoalId")] Step step)
        {
            if (ModelState.IsValid)
            {
                await stepsRepo.AddStep(step);
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "IndexSteps", await stepsRepo.GetAllSteps()) });
                //return RedirectToAction(nameof(Index));
            }
            // ViewData["GoalId"] = new SelectList(await goalRepo.GetAllGoals(), "Id", "Name", step.GoalId);
            //return View(step);
            ViewData["GoalId"] = new SelectList(await goalRepo.GetAllGoals(), "Id", "Name");
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "Create", step) });
        }


        // GET: Steps/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var step = await _context.Steps.FindAsync(id);
            var step = await stepsRepo.GetStep((int)id);
            if (step == null)
            {
                return NotFound();
            }
            ViewData["GoalId"] = new SelectList(await goalRepo.GetAllGoals(), "Id", "Name", step.GoalId);
            return View(step);
        }

        // POST: Steps/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Weight,DueDate,Status,GoalId")] Step step)
        {
            if (id != step.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await stepsRepo.UpdateStep(step);
                    //_context.Update(step);
                    //await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (! await StepExists(step.Id))
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
            ViewData["GoalId"] = new SelectList(await goalRepo.GetAllGoals(), "Id", "Name", step.GoalId);
            return View(step);
        }

        // GET: Steps/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var step = await stepsRepo.GetStep((int)id);
            //var step = await _context.Steps
            //    .Include(s => s.Goal)
            //    .FirstOrDefaultAsync(m => m.Id == id);
            if (step == null)
            {
                return NotFound();
            }

            return View(step);
        }

        // POST: Steps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await stepsRepo.DeleteStep(id);
            //var step = await _context.Steps.FindAsync(id);
            //_context.Steps.Remove(step);
            //await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> StepExists(int id)
        {
            return await stepsRepo.IsStepExist(id);
           // return _context.Steps.Any(e => e.Id == id);
        }


    }
}
