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
        private readonly IMesurementRepository MeasurementRepo;

        public MeasurementsController(IMesurementRepository measurementRepo, IGoalRepository goalRepo)
        {
            MeasurementRepo = measurementRepo;
            this.goalRepo = goalRepo;
        }


        // GET: Mesurements
        public async Task<IActionResult> Index()
        {
            var measurements = await MeasurementRepo.GetAllMesurements();
            return View(measurements);
        }


        // GET: Mesurements/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id != null)
            {
                var mesurement = await MeasurementRepo.GetMesurement((int)id);

                if (mesurement != null)
                {
                    return View(mesurement);
                }
            }

            return NotFound();
        }


        // GET: Mesurements/Create
        public async Task<IActionResult> Create()
        {
            ViewData["GoalId"] = new SelectList(await goalRepo.GetAllGoals(), "Id", "Name");
            return View();
        }


        // POST: Mesurements/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,GoalId,Type,CurrentValue,TotalValue,Unit")] Mesurement mesurement)
        {
            if (ModelState.IsValid)
            {
                await MeasurementRepo.AddMesurement(mesurement);
                var measurements = await MeasurementRepo.GetAllMesurements();
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "IndexMeasurement", measurements) });
            }

            ViewData["GoalId"] = new SelectList(await goalRepo.GetAllGoals(), "Id", "Name");
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "Create", mesurement) });

        }


        // GET: Mesurements/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id != null)
            {
                var mesurement = await MeasurementRepo.GetMesurement((int)id);
                if (mesurement != null)
                {
                    ViewData["GoalId"] = new SelectList(await goalRepo.GetAllGoals(), "Id", "Name", mesurement.GoalId);
                    return View(mesurement);
                }
            }
            return NotFound();
        }


        // POST: Mesurements/Edit/5
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
                    await MeasurementRepo.UpdateMesurement(mesurement);
                    var measurements = await MeasurementRepo.GetAllMesurements();
                    return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "IndexMeasurement", measurements) });
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
            }
            ViewData["GoalId"] = new SelectList(await goalRepo.GetAllGoals(), "Id", "Name", mesurement.GoalId);
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "Edit", mesurement) });
        }


        // GET: Mesurements/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mesurement = await MeasurementRepo.GetMesurement((int)id);
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
            await MeasurementRepo.DeleteMesurement(id);
            return RedirectToAction(nameof(Index));
        }


        private bool MesurementExists(int id)
        {
            return MeasurementRepo.IsMesurementExist(id);
        }


    }
}
