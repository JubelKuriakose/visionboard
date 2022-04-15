using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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
        public async Task<IActionResult> Create()
        {
            ViewData["GoalId"] = new SelectList(await goalRepo.GetAllGoals(null), "Id", "Name");
            return View();
        }


        // POST: Mesurements/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,GoalId,Type,CurrentValue,TotalValue,Unit")] Measurement measurement)
        {
            if (ModelState.IsValid)
            {
                await MeasurementRepo.AddMeasurement(measurement);
                var measurements = await MeasurementRepo.GetAllMeasurements();
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "IndexMeasurement", measurements) });
            }

            ViewData["GoalId"] = new SelectList(await goalRepo.GetAllGoals(null), "Id", "Name");
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
                    //ViewData["GoalId"] = new SelectList(await goalRepo.GetAllGoals(), "Id", "Name", mesurement.GoalId);
                    return View(measurement);
                }
            }
            return NotFound();
        }


        // POST: Measurements/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,GoalId,Type,CurrentValue,TotalValue,Unit")] Measurement measurement)
        {
            if (id != measurement.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await MeasurementRepo.UpdateMeasurement(measurement);
                    var measurements = await MeasurementRepo.GetAllMeasurements();
                    return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "IndexMeasurement", measurements) });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MeasurementExists(measurement.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            //ViewData["GoalId"] = new SelectList(await goalRepo.GetAllGoals(), "Id", "Name", mesurement.GoalId);
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
            var measurements = await MeasurementRepo.GetAllMeasurements();
            return Json(new { success = true, html = Helper.RenderRazorViewToString(this, "IndexMeasurement", measurements) });
        }


        private bool MeasurementExists(int id)
        {
            return MeasurementRepo.IsMeasurementExist(id);
        }


    }
}
