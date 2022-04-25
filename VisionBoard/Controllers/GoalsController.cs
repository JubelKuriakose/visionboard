using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Net.Http.Headers;
using VisionBoard.DAL;
using VisionBoard.Models;
using VisionBoard.Utilis;

namespace VisionBoard.Controllers
{
    public class GoalsController : Controller
    {
        private readonly IGoalRepository goalsRepo;
        private readonly IRewardRepository rewardRepo;
        private readonly ITagRepository tagRepo;
        private readonly IErrorLogRepository errorLogRepository;
        private readonly IHostingEnvironment hostingEnvironment;

        public GoalsController(IGoalRepository goalsRepo, IRewardRepository rewardRepo, ITagRepository tagRepo, IErrorLogRepository errorLogRepository, IHostingEnvironment hostingEnvironment)
        {
            this.goalsRepo = goalsRepo;
            this.rewardRepo = rewardRepo;
            this.tagRepo = tagRepo;
            this.errorLogRepository = errorLogRepository;
            this.hostingEnvironment = hostingEnvironment;
        }


        // GET: Goals
        public async Task<IActionResult> Index(int[] tagIds)
        {
            try
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
            catch (Exception ex)
            {
                await errorLogRepository.AddErrorLog(ex.TargetSite.ReflectedType.DeclaringType.Name, ex.TargetSite.ReflectedType.Name, ex.Message);
            }
            return View("../Shared/Error", null);

        }


        // GET: Goals/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            try
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
            catch (Exception ex)
            {
                await errorLogRepository.AddErrorLog(ex.TargetSite.ReflectedType.DeclaringType.Name, ex.TargetSite.ReflectedType.Name, ex.Message);
            }
            return View("../Shared/Error", null);

        }


        // GET: Goals/Create
        public async Task<IActionResult> Create()
        {
            try
            {
                ViewData["RewardId"] = new SelectList(await rewardRepo.GetAllRewards(), "Id", "Name");
                ViewData["TagId"] = new SelectList(await tagRepo.GetAllTags(), "Id", "Name");
                return View();
            }
            catch (Exception ex)
            {
                await errorLogRepository.AddErrorLog(ex.TargetSite.ReflectedType.DeclaringType.Name, ex.TargetSite.ReflectedType.Name, ex.Message);
            }
            return View("../Shared/Error", null);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,StartOn,EndingOn,Magnitude,PictureUrl,TagIds,RewardId")] CreateGoal createGoal)
        {
            try
            {
                Goal goal = null;
                List<GoalTags> goalTags = null;
                //goal completed or not
                createGoal.Status = false;

                if (ModelState.IsValid)
                {
                    if (createGoal.TagIds.Length > 0)
                    {
                        goalTags = createGoal.TagIds.Select(t => new GoalTags() { GoalId = createGoal.Id, TagId = t }).ToList();
                    }

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
            catch (Exception ex)
            {
                await errorLogRepository.AddErrorLog(ex.TargetSite.ReflectedType.DeclaringType.Name, ex.TargetSite.ReflectedType.Name, ex.Message);
            }
            return View("../Shared/Error", null);

        }


        // GET: Goals/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            try
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
                int[] TagIds = goal.GoalTags.Select(gt => gt.TagId).ToArray();
                ViewData["TagId"] = new MultiSelectList(await tagRepo.GetAllTags(), "Id", "Name", TagIds);
                ViewData["RewardId"] = new SelectList(await rewardRepo.GetAllRewards(), "Id", "Name", goal.RewardId);
                return View(goal);
            }
            catch (Exception ex)
            {
                await errorLogRepository.AddErrorLog(ex.TargetSite.ReflectedType.DeclaringType.Name, ex.TargetSite.ReflectedType.Name, ex.Message);
            }
            return View("../Shared/Error", null);

        }


        // POST: Goals/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,StartOn,EndingOn,Magnitude,PictureUrl,TagId,RewardId,Status")] Goal goal)
        {
            try
            {
                if (id != goal.Id)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    await goalsRepo.UpdateGoal(goal);
                    return RedirectToAction(nameof(Index));
                }
                int[] TagIds = goal.GoalTags.Select(gt => gt.TagId).ToArray();
                ViewData["TagId"] = new MultiSelectList(await tagRepo.GetAllTags(), "Id", "Name", TagIds);
                ViewData["RewardId"] = new SelectList(await rewardRepo.GetAllRewards(), "Id", "Name", goal.RewardId);
                return View(goal);
            }
            catch (Exception ex)
            {
                if (!await GoalExists(goal.Id))
                {
                    return NotFound();
                }
                await errorLogRepository.AddErrorLog(ex.TargetSite.ReflectedType.DeclaringType.Name, ex.TargetSite.ReflectedType.Name, ex.Message);
            }
            return View("../Shared/Error", null);

        }


        // GET: Goals/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            try
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
            catch (Exception ex)
            {
                await errorLogRepository.AddErrorLog(ex.TargetSite.ReflectedType.DeclaringType.Name, ex.TargetSite.ReflectedType.Name, ex.Message);
            }
            return View("../Shared/Error", null);

        }


        // POST: Goals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var goal = await goalsRepo.DeleteGoal(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                await errorLogRepository.AddErrorLog(ex.TargetSite.ReflectedType.DeclaringType.Name, ex.TargetSite.ReflectedType.Name, ex.Message);
            }
            return View("../Shared/Error", null);

        }


        private async Task<bool> GoalExists(int id)
        {
            try
            {
                return await goalsRepo.IsGoalExist(id);
            }
            catch (Exception ex)
            {
                await errorLogRepository.AddErrorLog(ex.TargetSite.ReflectedType.DeclaringType.Name, ex.TargetSite.ReflectedType.Name, ex.Message);
            }
            return false;

        }


    }
}
