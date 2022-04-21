using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using VisionBoard.DAL;
using VisionBoard.Models;
using VisionBoard.Utilis;

namespace VisionBoard.Controllers
{
    public class RewardsController : Controller
    {

        private readonly IRewardRepository rewardsRepo;
        private readonly IGoalRepository goalRepo;
        private readonly IErrorLogRepository errorLogRepository;

        public RewardsController(IRewardRepository rewardsRepo, IGoalRepository goalRepo, IErrorLogRepository errorLogRepository)
        {
            this.rewardsRepo = rewardsRepo;
            this.goalRepo = goalRepo;
            this.errorLogRepository = errorLogRepository;
        }


        // GET: Rewards
        public async Task<IActionResult> Index()
        {
            try
            {
                var rewards = await rewardsRepo.GetAllRewards();
                return View(rewards);
            }
            catch (Exception ex)
            {
                await errorLogRepository.AddErrorLog(ex.TargetSite.ReflectedType.DeclaringType.Name, ex.TargetSite.ReflectedType.Name, ex.Message);
            }
            return View("../Shared/Error", null);

        }


        // GET: Rewards/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            try
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
            catch (Exception ex)
            {
                await errorLogRepository.AddErrorLog(ex.TargetSite.ReflectedType.DeclaringType.Name, ex.TargetSite.ReflectedType.Name, ex.Message);
            }
            return View("../Shared/Error", null);

        }


        // GET: Rewards/Create
        public async Task<IActionResult> Create(string source)
        {
            try
            {
                ViewData["GoalId"] = new SelectList(await goalRepo.GetAllGoals(null), "Id", "Name");
                ViewBag.Source = source;
                return View();
            }
            catch (Exception ex)
            {
                await errorLogRepository.AddErrorLog(ex.TargetSite.ReflectedType.DeclaringType.Name, ex.TargetSite.ReflectedType.Name, ex.Message);
            }
            return View("../Shared/Error", null);

        }


        // POST: Rewards/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string source, [Bind("Id,Name,Descrption,GoalId,PictureUrl,Status")] Reward reward)
        {
            try
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
                ViewBag.Source = source;
                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "Create", reward) });
            }
            catch (Exception ex)
            {
                await errorLogRepository.AddErrorLog(ex.TargetSite.ReflectedType.DeclaringType.Name, ex.TargetSite.ReflectedType.Name, ex.Message);
            }
            return View("../Shared/Error", null);

        }


        // GET: Rewards/Edit/5
        public async Task<IActionResult> Edit(int? id, string source)
        {
            try
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
            catch (Exception ex)
            {
                await errorLogRepository.AddErrorLog(ex.TargetSite.ReflectedType.DeclaringType.Name, ex.TargetSite.ReflectedType.Name, ex.Message);
            }
            return View("../Shared/Error", null);

        }


        // POST: Rewards/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, string source, [Bind("Id,Name,Descrption,GoalId,PictureUrl,Status")] Reward reward)
        {
            try
            {
                if (id != reward.Id)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
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
                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "Edit", reward) });
            }
            catch (Exception ex)
            {
                if (!await RewardExists(reward.Id))
                {
                    return NotFound();
                }
                await errorLogRepository.AddErrorLog(ex.TargetSite.ReflectedType.DeclaringType.Name, ex.TargetSite.ReflectedType.Name, ex.Message);
            }
            return View("../Shared/Error", null);

        }


        // GET: Rewards/Delete/5
        public async Task<IActionResult> Delete(int? id, string source)
        {
            try
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
            catch (Exception ex)
            {
                await errorLogRepository.AddErrorLog(ex.TargetSite.ReflectedType.DeclaringType.Name, ex.TargetSite.ReflectedType.Name, ex.Message);
            }
            return View("../Shared/Error", null);

        }


        // POST: Rewards/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id, string source)
        {
            try
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
            catch (Exception ex)
            {
                await errorLogRepository.AddErrorLog(ex.TargetSite.ReflectedType.DeclaringType.Name, ex.TargetSite.ReflectedType.Name, ex.Message);
            }
            return View("../Shared/Error", null);

        }


        private async Task<bool> RewardExists(int id)
        {
            try
            {
                return await rewardsRepo.IsRewardExist(id);
            }
            catch (Exception ex)
            {
                await errorLogRepository.AddErrorLog(ex.TargetSite.ReflectedType.DeclaringType.Name, ex.TargetSite.ReflectedType.Name, ex.Message);
            }
            return false;

        }


    }
}
