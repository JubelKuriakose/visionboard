using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VisionBoard.DAL;
using VisionBoard.Models;

namespace VisionBoard.Controllers
{
    public class RewardsController : Controller
    {
        private readonly IRewardRepository rewardsRepo;
        private readonly IGoalRepository goalRepo;

        public RewardsController(IRewardRepository rewardsRepo, IGoalRepository goalRepo)
        {
            this.rewardsRepo = rewardsRepo;
            this.goalRepo = goalRepo;
        }

        // GET: Rewards
        public async Task<IActionResult> Index()
        {
            var rewards = await rewardsRepo.GetAllRewards();
            return View(rewards);
            //var goalTrackerContext = _context.Rewards.Include(r => r.Goal);
            //return View(await goalTrackerContext.ToListAsync());
        }

        // GET: Rewards/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var reward = await rewardsRepo.GetReward((int)id);

            //var reward = await _context.Rewards
            //    .Include(r => r.Goal)
            //    .FirstOrDefaultAsync(m => m.Id == id);
            if (reward == null)
            {
                return NotFound();
            }

            return View(reward);
        }

        // GET: Rewards/Create
        public async Task<IActionResult> Create()
        {
            ViewData["GoalId"] = new SelectList(await goalRepo.GetAllGoals(), "Id", "Name");
            return View();
        }

        // POST: Rewards/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Descrption,GoalId,PictureUrl,Status")] Reward reward)
        {
            if (ModelState.IsValid)
            {
                await rewardsRepo.AddReward(reward);
                //_context.Add(reward);
                //await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GoalId"] = new SelectList(await goalRepo.GetAllGoals(), "Id", "Name", reward.GoalId);
            return View(reward);
        }

        // GET: Rewards/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reward = await rewardsRepo.GetReward((int)id);
            //var reward = await _context.Rewards.FindAsync(id);
            if (reward == null)
            {
                return NotFound();
            }
            ViewData["GoalId"] = new SelectList( await goalRepo.GetAllGoals(), "Id", "Name", reward.GoalId);
            return View(reward);
        }

        // POST: Rewards/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Descrption,GoalId,PictureUrl,Status")] Reward reward)
        {
            if (id != reward.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await rewardsRepo.UpdateReward(reward);
                    //_context.Update(reward);
                    //await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (! await RewardExists(reward.Id))
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
            ViewData["GoalId"] = new SelectList(await goalRepo.GetAllGoals(), "Id", "Name", reward.GoalId);
            return View(reward);
        }

        // GET: Rewards/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reward = await rewardsRepo.GetReward((int)id);
            //var reward = await _context.Rewards
            //    .Include(r => r.Goal)
            //    .FirstOrDefaultAsync(m => m.Id == id);
            if (reward == null)
            {
                return NotFound();
            }

            return View(reward);
        }

        // POST: Rewards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await rewardsRepo.DeleteReward(id);
            //var reward = await _context.Rewards.FindAsync(id);
            //_context.Rewards.Remove(reward);
            //await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> RewardExists(int id)
        {
            return await rewardsRepo.IsRewardExist(id);
            //return _context.Rewards.Any(e => e.Id == id);
        }
    }
}
