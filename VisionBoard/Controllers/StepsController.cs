using System.Collections.Generic;
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
        }


        // GET: Steps/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id != null)
            {
                var step = stepsRepo.GetStep((int)id);
                if (step != null)
                {
                    return View(step);
                }
            }
            return NotFound();
        }


        // GET: Steps/Create
        public async Task<IActionResult> Create(int? goalId)
        {
            if (goalId == null)
            {
                ViewData["GoalId"] = new SelectList(await goalRepo.GetAllGoals(), "Id", "Name");
            }
            else
            {
                IEnumerable<Goal> goal = new List<Goal>()
                {
                    await goalRepo.GetGoal((int)goalId)
                };
                ViewData["GoalId"] = new SelectList(goal, "Id", "Name");
                ViewData["CurrentGoalId"] = goalId;
            }

            return View();
        }


        // POST: Steps/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int? currentGoalId, [Bind("Id,Name,Description,Weight,DueDate,Status,GoalId")] Step step)
        {

            if (ModelState.IsValid)
            {
                await stepsRepo.AddStep(step);

                IEnumerable<Step> steps = new List<Step>();
                if (currentGoalId == null)
                {
                    steps = await stepsRepo.GetAllSteps();
                }
                else
                {
                    steps = await stepsRepo.GetAllSteps((int)currentGoalId);
                }

                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "IndexSteps", steps) });
            }

            if (currentGoalId == null)
            {
                ViewData["GoalId"] = new SelectList(await goalRepo.GetAllGoals(), "Id", "Name");
            }
            else
            {
                IEnumerable<Goal> goal = new List<Goal>()
                {
                    await goalRepo.GetGoal((int)currentGoalId)
                };
                ViewData["GoalId"] = new SelectList(goal, "Id", "Name");
            }

            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "Create", step) });
        }


        // GET: Steps/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id != null)
            {
                var step = await stepsRepo.GetStep((int)id);
                if (step != null)
                {
                    ViewData["GoalId"] = new SelectList(await goalRepo.GetAllGoals(), "Id", "Name", step.GoalId);
                    return View(step);
                }
            }
            return NotFound();
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
                    var steps = await stepsRepo.GetAllSteps();
                    return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "IndexSteps", steps) });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await StepExists(step.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            ViewData["GoalId"] = new SelectList(await goalRepo.GetAllGoals(), "Id", "Name", step.GoalId);
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "Edit", step) });
        }


        // GET: Steps/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id != null)
            {
                var step = await stepsRepo.GetStep((int)id);
                if (step != null)
                {
                    return View(step);
                }
            }
            return NotFound();
        }


        // POST: Steps/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await stepsRepo.DeleteStep(id);
            var steps = await stepsRepo.GetAllSteps();
            return Json(new { success = true, html = Helper.RenderRazorViewToString(this, "IndexSteps", steps) });
        }


        private async Task<bool> StepExists(int id)
        {
            return await stepsRepo.IsStepExist(id);
        }


    }
}
