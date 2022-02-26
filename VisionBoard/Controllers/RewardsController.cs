using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VisionBoard.DAL;
using VisionBoard.Models;
using VisionBoard.Utilis;

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
        }


        // GET: Rewards/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id != null)
            {
                var reward = await rewardsRepo.GetReward((int)id);

                if (reward != null)
                {
                    return View(reward);
                }
            }

            return NotFound();
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
                var rewards = await rewardsRepo.GetAllRewards();
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "IndexRewards", rewards) });
            }
            ViewData["GoalId"] = new SelectList(await goalRepo.GetAllGoals(), "Id", "Name", reward.GoalId);
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "Create", reward) });
        }


        // GET: Rewards/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id != null)
            {
                var reward = await rewardsRepo.GetReward((int)id);

                if (reward != null)
                {
                    ViewData["GoalId"] = new SelectList(await goalRepo.GetAllGoals(), "Id", "Name", reward.GoalId);
                    return View(reward);
                }
            }

            return NotFound();
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
                    var rewards = await rewardsRepo.GetAllRewards();
                    return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "IndexRewards", rewards) });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await RewardExists(reward.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            ViewData["GoalId"] = new SelectList(await goalRepo.GetAllGoals(), "Id", "Name", reward.GoalId);
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "Edit", reward) });
        }


        // GET: Rewards/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id != null)
            {
                var reward = await rewardsRepo.GetReward((int)id);

                if (reward != null)
                {
                    return View(reward);
                }
            }

            return NotFound();
        }


        // POST: Rewards/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await rewardsRepo.DeleteReward(id);
            var rewards = await rewardsRepo.GetAllRewards();
            return Json(new { success = true, html = Helper.RenderRazorViewToString(this, "IndexRewards", rewards) });
        }


        private async Task<bool> RewardExists(int id)
        {
            return await rewardsRepo.IsRewardExist(id);
        }


    }
}
