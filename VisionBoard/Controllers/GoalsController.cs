using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VisionBoard.DAL;
using VisionBoard.Models;

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
        public async Task<IActionResult> Index()
        {
            var goals = await goalsRepo.GetAllGoals();
            return View(goals);
            //var goalTrackerContext = _context.Goals.Include(g => g.Reward).Include(g => g.Tag);
            //return View(await goalTrackerContext.ToListAsync());
        }


        // GET: Goals/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var goal = await goalsRepo.GetGoal((int)id);
            //var goal = await _context.Goals
            //    .Include(g => g.Reward)
            //    .Include(g => g.Tag)
            //    .FirstOrDefaultAsync(m => m.Id == id);
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

        // POST: Goals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,Name,Description,StartOn,EndingOn,Magnitude,PictureUrl,TagId,RewardId,Status")] Goal goal)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        await goalsRepo.AddGoal(goal);
        //        //_context.Add(goal);
        //        //await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["RewardId"] = new SelectList(await rewardRepo.GetAllRewards(), "Id", "Name", goal.RewardId);
        //    ViewData["TagId"] = new SelectList(await tagRepo.GetAllTags(), "Id", "Name", goal.TagId);
        //    return View(goal);
        //}


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,StartOn,EndingOn,Magnitude,Picture,TagId,RewardId,Status")] CreateGoal createGoal)
        {
            Goal goal = null;
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;
                string filePath = null;
                if (createGoal.Picture != null)
                {
                    string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + createGoal.Picture.FileName;
                    filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    createGoal.Picture.CopyTo(new FileStream(filePath, FileMode.Create));
                }

                goal = new Goal()
                {
                    Name = createGoal.Name,
                    Description = createGoal.Description,
                    StartOn = createGoal.StartOn,
                    EndingOn = createGoal.EndingOn,
                    Magnitude = createGoal.Magnitude,
                    PictureUrl = filePath,
                    TagId = createGoal.TagId,
                    RewardId = createGoal.RewardId
                };

                await goalsRepo.AddGoal(goal);
                //_context.Add(goal);
                //await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RewardId"] = new SelectList(await rewardRepo.GetAllRewards(), "Id", "Name", goal.RewardId);
            ViewData["TagId"] = new SelectList(await tagRepo.GetAllTags(), "Id", "Name", goal.TagId);
            return View(goal);
        }


        // GET: Goals/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var goal = await _context.Goals.FindAsync(id);
            var goal = await goalsRepo.GetGoal((int)id);

            if (goal == null)
            {
                return NotFound();
            }
            ViewData["RewardId"] = new SelectList(await rewardRepo.GetAllRewards(), "Id", "Name", goal.RewardId);
            ViewData["TagId"] = new SelectList(await tagRepo.GetAllTags(), "Id", "Name", goal.TagId);
            return View(goal);
        }

        // POST: Goals/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
                    //_context.Update(goal);
                    //await _context.SaveChangesAsync();
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
            ViewData["TagId"] = new SelectList(await tagRepo.GetAllTags(), "Id", "Name", goal.TagId);
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
            //var goal = await _context.Goals
            //    .Include(g => g.Reward)
            //    .Include(g => g.Tag)
            //    .FirstOrDefaultAsync(m => m.Id == id);
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
            //var goal = await _context.Goals.FindAsync(id);
            //_context.Goals.Remove(goal);
            //await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> GoalExists(int id)
        {
            return await goalsRepo.IsGoalExist(id);
            //return _context.Goals.Any(e => e.Id == id);
        }


    }
}
