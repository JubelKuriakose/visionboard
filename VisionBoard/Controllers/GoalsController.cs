using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.Net.Http.Headers;
using VisionBoard.DAL;
using VisionBoard.Models;
using VisionBoard.Utilis;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;


namespace VisionBoard.Controllers
{
    public class GoalsController : Controller
    {
        private readonly IGoalRepository goalsRepo;
        private readonly IRewardRepository rewardRepo;
        private readonly ITagRepository tagRepo;
        private readonly IHostingEnvironment hostingEnvironment;

        public GoalsController(IGoalRepository goalsRepo, IRewardRepository rewardRepo, ITagRepository tagRepo, IHostingEnvironment hostingEnvironment)
        {
            this.goalsRepo = goalsRepo;
            this.rewardRepo = rewardRepo;
            this.tagRepo = tagRepo;
            this.hostingEnvironment = hostingEnvironment;
        }


        // GET: Goals
        public async Task<IActionResult> Index(int[] tagIds)
        {
            var goals = await goalsRepo.GetAllGoals(tagIds);
            ViewData["TagId"] = new SelectList(await tagRepo.GetAllTags(), "Id", "Name");
            string requestType = Request.Headers[HeaderNames.XRequestedWith];

            if (requestType == "XMLHttpRequest")
            {
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "IndexGoals", goals) });
            }
            else
            {
                return View(goals);
            }

        }


        // GET: Goals/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var goal = await goalsRepo.GetGoal((int)id);

            if (goal == null)
            {
                return NotFound();
            }

            return View(goal);
        }


        // GET: Goals/Create
        public async Task<IActionResult> Create()
        {
            ViewData["RewardId"] = new SelectList(await rewardRepo.GetAllRewards(), "Id", "Name");
            ViewData["TagId"] = new SelectList(await tagRepo.GetAllTags(), "Id", "Name");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,StartOn,EndingOn,Magnitude,PictureUrl,TagIds,RewardId,Status")] CreateGoal createGoal)
        {
            Goal goal = null;
            if (ModelState.IsValid)
            {

                List<GoalTags> goalTags = createGoal.TagIds.Select(t => new GoalTags() { GoalId = createGoal.Id, TagId = t }).ToList();

                goal = new Goal()
                {
                    Name = createGoal.Name,
                    Description = createGoal.Description,
                    StartOn = createGoal.StartOn,
                    EndingOn = createGoal.EndingOn,
                    Magnitude = createGoal.Magnitude,
                    PictureUrl = createGoal.PictureUrl,
                    GoalTags = goalTags,
                    RewardId = createGoal.RewardId
                };

                await goalsRepo.AddGoal(goal);
                return RedirectToAction("Details", new { id = goal.Id });
            }
            ViewData["RewardId"] = new SelectList(await rewardRepo.GetAllRewards(), "Id", "Name", goal.RewardId);
            ViewData["TagId"] = new SelectList(await tagRepo.GetAllTags(), "Id", "Name");
            return View(goal);
        }


        // GET: Goals/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var goal = await goalsRepo.GetGoal((int)id);

            if (goal == null)
            {
                return NotFound();
            }
            ViewData["RewardId"] = new SelectList(await rewardRepo.GetAllRewards(), "Id", "Name", goal.RewardId);
            return View(goal);
        }


        // POST: Goals/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,StartOn,EndingOn,Magnitude,PictureUrl,TagId,RewardId,Status")] Goal goal)
        {
            if (id != goal.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await goalsRepo.UpdateGoal(goal);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await GoalExists(goal.Id))
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
            ViewData["RewardId"] = new SelectList(await rewardRepo.GetAllRewards(), "Id", "Name", goal.RewardId);
            return View(goal);
        }


        // GET: Goals/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var goal = await goalsRepo.GetGoal((int)id);

            if (goal == null)
            {
                return NotFound();
            }

            return View(goal);
        }


        // POST: Goals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var goal = await goalsRepo.DeleteGoal(id);
            return RedirectToAction(nameof(Index));
        }


        private async Task<bool> GoalExists(int id)
        {
            return await goalsRepo.IsGoalExist(id);
        }

        public IActionResult CustomCrop()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CustomCrop(string filename, IFormFile blob)
        {
            string newfileName = string.Empty;
            string filepath = string.Empty;

            try
            {
                using (var image = SixLabors.ImageSharp.Image.Load(blob.OpenReadStream()))
                {
                    string systemFileExtenstion = filename.Substring(filename.LastIndexOf('.'));

                    image.Mutate(x => x.Resize(345, 289));
                    newfileName = $"{"Photo_345_289"}_{DateTime.Now:yyyyMMddHHmmssfff}_{Guid.NewGuid():N}{systemFileExtenstion}";
                    filepath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images")).Root + $@"\{newfileName}";
                    image.Save(filepath);

                }
                return Json(new { Message = "SUCCESS", SelectedImage = newfileName });
            }
            catch (Exception)
            {
                return Json(new { Message = "ERROR", SelectedImage = string.Empty });
            }
        }



    }
}
