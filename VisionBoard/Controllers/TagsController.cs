using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VisionBoard.DAL;
using VisionBoard.Models;
using VisionBoard.Utilis;

namespace VisionBoard.Controllers
{
    public class TagsController : Controller
    {
        private readonly ITagRepository tagRepository;
        private readonly IErrorLogRepository errorLogRepository;

        public TagsController(ITagRepository tagRepository, IErrorLogRepository errorLogRepository)
        {
            this.tagRepository = tagRepository;
            this.errorLogRepository = errorLogRepository;
        }


        // GET: Tags
        public async Task<IActionResult> Index()
        {
            try
            {
                var tags = await tagRepository.GetAllTags();
                return View(tags);
            }
            catch (Exception ex)
            {
                await errorLogRepository.AddErrorLog(ex.TargetSite.ReflectedType.DeclaringType.Name, ex.TargetSite.ReflectedType.Name, ex.Message);
            }
            return View("../Shared/Error", null);

        }


        // GET: Tags/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                if (id != null)
                {
                    var tag = await tagRepository.GetTag((int)id);

                    if (tag != null)
                    {
                        return View(tag);
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


        // GET: Tags/Create
        public async Task<IActionResult> Create(string source)
        {
            try
            {
                ViewBag.Source = source;
                return View();
            }
            catch (Exception ex)
            {
                await errorLogRepository.AddErrorLog(ex.TargetSite.ReflectedType.DeclaringType.Name, ex.TargetSite.ReflectedType.Name, ex.Message);
            }
            return View("../Shared/Error", null);

        }


        // POST: Tags/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string source, [Bind("Id,Name,Colour")] Tag tag)
        {
            try
            {
                //tag active or not
                tag.Status = true;

                if (ModelState.IsValid)
                {
                    var newTag = await tagRepository.AddTag(tag);

                    if (source == "DropDown")
                    {
                        return Json(new { isValid = true, Source = source, Id = newTag.Id, Name = newTag.Name });
                    }
                    else
                    {
                        var tags = await tagRepository.GetAllTags();
                        return Json(new { isValid = true, source = "Index", html = Helper.RenderRazorViewToString(this, "IndexTags", tags) });
                    }
                }

                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "Create", tag) });
            }
            catch (Exception ex)
            {
                await errorLogRepository.AddErrorLog(ex.TargetSite.ReflectedType.DeclaringType.Name, ex.TargetSite.ReflectedType.Name, ex.Message);
            }
            return View("../Shared/Error", null);

        }


        // GET: Tags/Edit/5
        public async Task<IActionResult> Edit(int? id, string source)
        {
            try
            {
                if (id != null)
                {
                    var tag = await tagRepository.GetTag((int)id);

                    if (tag != null)
                    {
                        ViewBag.Source = source;
                        return View(tag);
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


        // POST: Tags/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, string source, [Bind("Id,Name,Colour,Status")] Tag tag)
        {
            try
            {
                if (id != tag.Id)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    var newTag = await tagRepository.UpdateTag(tag);

                    if (source == "DropDown")
                    {
                        return Json(new { isValid = true, Source = source, Id = newTag.Id, Name = newTag.Name });
                    }
                    else
                    {
                        var tags = await tagRepository.GetAllTags();
                        return Json(new { isValid = true, source = "Index", html = Helper.RenderRazorViewToString(this, "IndexTags", tags) });
                    }

                }
                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "Edit", tag) });
            }
            catch (Exception ex)
            {
                if (!await TagExists(tag.Id))
                {
                    return NotFound();
                }
                await errorLogRepository.AddErrorLog(ex.TargetSite.ReflectedType.DeclaringType.Name, ex.TargetSite.ReflectedType.Name, ex.Message);
            }
            return View("../Shared/Error", null);

        }


        // GET: Tags/Delete/5
        public async Task<IActionResult> Delete(int? id, string source)
        {
            try
            {
                if (id != null)
                {
                    var tag = await tagRepository.GetTag((int)id);

                    if (tag != null)
                    {
                        ViewBag.Source = source;
                        return View(tag);
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


        // POST: Tags/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id, string source)
        {
            try
            {
                var newTag = await tagRepository.DeleteTag(id);
                if (source == "DropDown")
                {
                    return Json(new { isValid = true, Source = source, Id = newTag.Id, Name = newTag.Name });
                }
                else
                {
                    var tags = await tagRepository.GetAllTags();
                    return Json(new { isValid = true, source = "Index", html = Helper.RenderRazorViewToString(this, "IndexTags", tags) });
                }
            }
            catch (Exception ex)
            {
                await errorLogRepository.AddErrorLog(ex.TargetSite.ReflectedType.DeclaringType.Name, ex.TargetSite.ReflectedType.Name, ex.Message);
            }
            return View("../Shared/Error", null);

        }


        private async Task<bool> TagExists(int id)
        {
            try
            {
                return await tagRepository.IsTagExist(id);
            }
            catch (Exception ex)
            {
                await errorLogRepository.AddErrorLog(ex.TargetSite.ReflectedType.DeclaringType.Name, ex.TargetSite.ReflectedType.Name, ex.Message);
            }
            return false;

        }


    }
}
