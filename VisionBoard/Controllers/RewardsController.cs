using System.Linq;
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
        public async Task<IActionResult> Create(string source)
        {
            ViewData["GoalId"] = new SelectList(await goalRepo.GetAllGoals(), "Id", "Name");
            ViewBag.Source = source;
            return View();
        }


        // POST: Rewards/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string source, [Bind("Id,Name,Descrption,GoalId,PictureUrl,Status")] Reward reward)
        {
            if (ModelState.IsValid)
            {
                var newReward = await rewardsRepo.AddReward(reward);

                if (source == "DropDown")
                {
                    return Json(new { isValid = true, Source = source, Id = newReward.Id, Name = newReward.Name });
                }
                else
                {
                    var rewards = await rewardsRepo.GetAllRewards();
                    return Json(new { isValid = true, source = "Index", html = Helper.RenderRazorViewToString(this, "IndexRewards", rewards) });
                }
            }
            ViewData["GoalId"] = new SelectList(await goalRepo.GetAllGoals(), "Id", "Name", reward.GoalId);
            ViewBag.Source = source;
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "Create", reward) });
        }


        // GET: Rewards/Edit/5
        public async Task<IActionResult> Edit(int? id, string source)
        {
            if (id != null)
            {
                var reward = await rewardsRepo.GetReward((int)id);

                if (reward != null)
                {
                    ViewData["GoalId"] = new SelectList(await goalRepo.GetAllGoals(), "Id", "Name", reward.GoalId);
                    ViewBag.Source = source;
                    return View(reward);
                }
            }

            return NotFound();
        }


        // POST: Rewards/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, string source, [Bind("Id,Name,Descrption,GoalId,PictureUrl,Status")] Reward reward)
        {
            if (id != reward.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var newReward = await rewardsRepo.UpdateReward(reward);

                    if (source == "DropDown")
                    {
                        return Json(new { isValid = true, Source = source, Id = newReward.Id, Name = newReward.Name });
                    }
                    else
                    {
                        var rewards = await rewardsRepo.GetAllRewards();
                        return Json(new { isValid = true, source = "Index", html = Helper.RenderRazorViewToString(this, "IndexRewards", rewards) });
                    }
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
        public async Task<IActionResult> Delete(int? id, string source)
        {
            if (id != null)
            {
                var reward = await rewardsRepo.GetReward((int)id);

                if (reward != null)
                {
                    ViewBag.Source = source;
                    return View(reward);
                }
            }

            return NotFound();
        }


        // POST: Rewards/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id, string source)
        {
            var newReward = await rewardsRepo.DeleteReward(id);

            if (source == "DropDown")
            {
                return Json(new { isValid = true, Source = source, Id = newReward.Id, Name = newReward.Name });
            }
            else
            {
                var rewards = await rewardsRepo.GetAllRewards();
                return Json(new { isValid = true, source = "Index", html = Helper.RenderRazorViewToString(this, "IndexRewards", rewards) });
            }

        }


        private async Task<bool> RewardExists(int id)
        {
            return await rewardsRepo.IsRewardExist(id);
        }


    }
}
