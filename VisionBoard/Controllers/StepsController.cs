using System;
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
        private readonly IErrorLogRepository errorLogRepository;

        public StepsController(IStepRepository stepsRepo, IGoalRepository goalRepo, IErrorLogRepository errorLogRepository)
        {
            this.stepsRepo = stepsRepo;
            this.goalRepo = goalRepo;
            this.errorLogRepository = errorLogRepository;
        }


        // GET: Steps
        public async Task<IActionResult> Index(int? goalId)
        {
            try
            {
                var steps = await stepsRepo.GetAllSteps();
                return View(steps);
            }
            catch (Exception ex)
            {
                await errorLogRepository.AddErrorLog(ex.TargetSite.ReflectedType.DeclaringType.Name, ex.TargetSite.ReflectedType.Name, ex.Message);
            }
            return View("../Shared/Error", null);

        }


        // GET: Steps/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            try
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
            catch (Exception ex)
            {
                await errorLogRepository.AddErrorLog(ex.TargetSite.ReflectedType.DeclaringType.Name, ex.TargetSite.ReflectedType.Name, ex.Message);
            }
            return View("../Shared/Error", null);

        }


        // GET: Steps/Create
        public async Task<IActionResult> Create(int? goalId)
        {
            try
            {
                if (goalId == null)
                {
                    ViewData["CurrentGoalId"] = goalId;
                    ViewData["GoalId"] = new SelectList(await goalRepo.GetAllGoalsWithoutInnerObjects(), "Id", "Name");
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
            catch (Exception ex)
            {
                await errorLogRepository.AddErrorLog(ex.TargetSite.ReflectedType.DeclaringType.Name, ex.TargetSite.ReflectedType.Name, ex.Message);
            }
            return View("../Shared/Error", null);

        }


        // POST: Steps/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int? currentGoalId, [Bind("Id,Name,Description,Weight,DueDate,GoalId")] Step step)
        {
            try
            {
                step.Status = false;

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
                        ViewData["CurrentGoalId"] = currentGoalId;
                        steps = await stepsRepo.GetAllSteps((int)currentGoalId);
                    }

                    return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "IndexSteps", steps) });
                }

                if (currentGoalId == null)
                {
                    ViewData["GoalId"] = new SelectList(await goalRepo.GetAllGoalsWithoutInnerObjects(), "Id", "Name");
                }
                else
                {
                    IEnumerable<Goal> goal = new List<Goal>()
                    {
                        await goalRepo.GetGoal((int)currentGoalId)
                    };
                    ViewData["CurrentGoalId"] = currentGoalId;
                    ViewData["GoalId"] = new SelectList(goal, "Id", "Name");
                }

                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "Create", step) });
            }
            catch (Exception ex)
            {
                await errorLogRepository.AddErrorLog(ex.TargetSite.ReflectedType.DeclaringType.Name, ex.TargetSite.ReflectedType.Name, ex.Message);
            }
            return View("../Shared/Error", null);

        }


        // GET: Steps/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if (id != null)
                {
                    var step = await stepsRepo.GetStep((int)id);
                    if (step != null)
                    {
                        ViewData["GoalId"] = new SelectList(await goalRepo.GetAllGoalsWithoutInnerObjects(), "Id", "Name", step.GoalId);
                        return View(step);
                    }
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                await errorLogRepository.AddErrorLog(ex.TargetSite.ReflectedType.DeclaringType.Name, ex.TargetSite.ReflectedType.Name, ex.Message);
            }
            return View("../Shared/Error", null);

        }


        // POST: Steps/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Weight,DueDate,Status,GoalId")] Step step)
        {
            try
            {
                if (id != step.Id)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    await stepsRepo.UpdateStep(step);
                    var steps = await stepsRepo.GetAllSteps(step.GoalId);
                    return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "IndexSteps", steps) });
                }
                ViewData["GoalId"] = new SelectList(await goalRepo.GetAllGoalsWithoutInnerObjects(), "Id", "Name", step.GoalId);
                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "Edit", step) });
            }
            catch (Exception ex)
            {
                if (!await StepExists(step.Id))
                {
                    return NotFound();
                }
                await errorLogRepository.AddErrorLog(ex.TargetSite.ReflectedType.DeclaringType.Name, ex.TargetSite.ReflectedType.Name, ex.Message);
            }
            return View("../Shared/Error", null);

        }


        // GET: Steps/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            try
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
            catch (Exception ex)
            {
                await errorLogRepository.AddErrorLog(ex.TargetSite.ReflectedType.DeclaringType.Name, ex.TargetSite.ReflectedType.Name, ex.Message);
            }
            return View("../Shared/Error", null);

        }


        // POST: Steps/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await stepsRepo.DeleteStep(id);
                var steps = await stepsRepo.GetAllSteps();
                return Json(new { success = true, html = Helper.RenderRazorViewToString(this, "IndexSteps", steps) });
            }
            catch (Exception ex)
            {
                await errorLogRepository.AddErrorLog(ex.TargetSite.ReflectedType.DeclaringType.Name, ex.TargetSite.ReflectedType.Name, ex.Message);
            }
            return View("../Shared/Error", null);

        }


        private async Task<bool> StepExists(int id)
        {
            try
            {
                return await stepsRepo.IsStepExist(id);
            }
            catch (Exception ex)
            {
                await errorLogRepository.AddErrorLog(ex.TargetSite.ReflectedType.DeclaringType.Name, ex.TargetSite.ReflectedType.Name, ex.Message);
            }
            return false;

        }


    }
}
