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
        private readonly IMeasurementRepository MeasurementRepo;

        public MeasurementsController(IMeasurementRepository measurementRepo, IGoalRepository goalRepo)
        {
            MeasurementRepo = measurementRepo;
            this.goalRepo = goalRepo;
        }


        // GET: Mesurements
        public async Task<IActionResult> Index()
        {
            var measurements = await MeasurementRepo.GetAllMeasurements();
            return View(measurements);
        }


        // GET: Mesurements/Details/5        
        public async Task<IActionResult> Details(int? id)
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


        // GET: Mesurements/Create
        public async Task<IActionResult> Create(int? goalId)
        {
            ViewData["GoalId"] = goalId;
            return View();
        }


        // POST: Mesurements/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int goalId, [Bind("Id,Type,CurrentValue,TotalValue,Unit")] Measurement measurement)
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


        // GET: Mesurements/Edit/5
        public async Task<IActionResult> Edit(int? id)
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


        // POST: Measurements/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GoalId")] int goalId, [Bind("Id,Type,CurrentValue,TotalValue,Unit")] Measurement measurement)
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


        // GET: Mesurements/Delete/5
        public async Task<IActionResult> Delete(int? id)
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


        // POST: Measurements/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await MeasurementRepo.DeleteMeasurement(id);
            return Json(new { success = true });
        }


        private bool MeasurementExists(int id)
        {
            return MeasurementRepo.IsMeasurementExist(id);
        }


    }
}
