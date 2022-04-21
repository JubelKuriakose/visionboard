using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VisionBoard.DAL;
using VisionBoard.Models;
using VisionBoard.Utilis;

namespace VisionBoard.Controllers
{
    public class MeasurementsController : Controller
    {
        private readonly IGoalRepository goalRepo;
        private readonly IErrorLogRepository errorLogRepository;
        private readonly IMeasurementRepository MeasurementRepo;

        public MeasurementsController(IMeasurementRepository measurementRepo, IGoalRepository goalRepo, IErrorLogRepository errorLogRepository)
        {
            MeasurementRepo = measurementRepo;
            this.goalRepo = goalRepo;
            this.errorLogRepository = errorLogRepository;
        }


        // GET: Mesurements
        public async Task<IActionResult> Index()
        {
            try
            {
                var measurements = await MeasurementRepo.GetAllMeasurements();
                return View(measurements);
            }
            catch (Exception ex)
            {
                await errorLogRepository.AddErrorLog(ex.TargetSite.ReflectedType.DeclaringType.Name, ex.TargetSite.ReflectedType.Name, ex.Message);
            }
            return View("../Shared/Error", null);
            
        }


        // GET: Mesurements/Details/5        
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                if (id != null)
                {
                    var measurement = await MeasurementRepo.GetMeasurement((int)id);

                    if (measurement != null)
                    {
                        return View(measurement);
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


        // GET: Mesurements/Create
        public async Task<IActionResult> Create(int? goalId)
        {
            try
            {
                ViewData["GoalId"] = goalId;
                return View();
            }
            catch (Exception ex)
            {
                await errorLogRepository.AddErrorLog(ex.TargetSite.ReflectedType.DeclaringType.Name, ex.TargetSite.ReflectedType.Name, ex.Message);
            }
            return View("../Shared/Error", null);
            
        }


        // POST: Mesurements/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int goalId, [Bind("Id,Type,CurrentValue,TotalValue,Unit")] Measurement measurement)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Measurement mesurement = await MeasurementRepo.AddMeasurement(measurement);
                    Goal goal = await goalRepo.GetGoal(goalId);
                    goal.MeasurementId = mesurement.Id;
                    await goalRepo.UpdateGoal(goal);
                    return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "Details", measurement) });
                }

                ViewData["GoalId"] = goalId;
                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "Create", measurement) });
            }
            catch (Exception ex)
            {
                await errorLogRepository.AddErrorLog(ex.TargetSite.ReflectedType.DeclaringType.Name, ex.TargetSite.ReflectedType.Name, ex.Message);
            }
            return View("../Shared/Error", null);            

        }


        // GET: Mesurements/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if (id != null)
                {
                    var measurement = await MeasurementRepo.GetMeasurement((int)id);
                    if (measurement != null)
                    {
                        return View(measurement);
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


        // POST: Measurements/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GoalId")] int goalId, [Bind("Id,Type,CurrentValue,TotalValue,Unit")] Measurement measurement)
        {
            try
            {
                if (id != measurement.Id)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    var mesurement = await MeasurementRepo.UpdateMeasurement(measurement);
                    Goal goal = await goalRepo.GetGoal(goalId);
                    goal.MeasurementId = mesurement.Id;
                    await goalRepo.UpdateGoal(goal);
                    return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "Details", measurement) });
                }
                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "Edit", measurement) });
            }
            catch (Exception ex)
            {
                await errorLogRepository.AddErrorLog(ex.TargetSite.ReflectedType.DeclaringType.Name, ex.TargetSite.ReflectedType.Name, ex.Message);
            }
            return View("../Shared/Error", null);
            
        }


        // GET: Mesurements/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }
                var measurement = await MeasurementRepo.GetMeasurement((int)id);

                if (measurement == null)
                {
                    return NotFound();
                }

                return View(measurement);
            }
            catch (Exception ex)
            {
                await errorLogRepository.AddErrorLog(ex.TargetSite.ReflectedType.DeclaringType.Name, ex.TargetSite.ReflectedType.Name, ex.Message);
            }
            return View("../Shared/Error", null);
            
        }


        // POST: Measurements/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await MeasurementRepo.DeleteMeasurement(id);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                await errorLogRepository.AddErrorLog(ex.TargetSite.ReflectedType.DeclaringType.Name, ex.TargetSite.ReflectedType.Name, ex.Message);
            }
            return View("../Shared/Error", null);
            
        }


        private async Task< bool> MeasurementExists(int id)
        {
            try
            {
                return await MeasurementRepo.IsMeasurementExist(id);
            }
            catch (Exception ex)
            {
                await errorLogRepository.AddErrorLog(ex.TargetSite.ReflectedType.DeclaringType.Name, ex.TargetSite.ReflectedType.Name, ex.Message);
            }
            return false;
            
        }


    }
}
