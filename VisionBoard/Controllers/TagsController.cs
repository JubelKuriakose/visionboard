﻿using System.Threading.Tasks;
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

        public TagsController(ITagRepository tagRepository)
        {
            this.tagRepository = tagRepository;
        }


        // GET: Tags
        public async Task<IActionResult> Index()
        {
            var tags = await tagRepository.GetAllTags();
            return View(tags);
        }


        // GET: Tags/Details/5
        public async Task<IActionResult> Details(int? id)
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


        // GET: Tags/Create
        public IActionResult Create(string source)
        {
            ViewBag.Source = source;
            return View();
        }


        // POST: Tags/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string source, [Bind("Id,Name,Colour,Status")] Tag tag)
        {
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


        // GET: Tags/Edit/5
        public async Task<IActionResult> Edit(int? id, string source)
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


        // POST: Tags/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, string source, [Bind("Id,Name,Colour,Status")] Tag tag)
        {
            if (id != tag.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
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
                catch (DbUpdateConcurrencyException)
                {
                    if (!await TagExists(tag.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "Edit", tag) });
        }


        // GET: Tags/Delete/5
        public async Task<IActionResult> Delete(int? id, string source)
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


        // POST: Tags/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id, string source)
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


        private async Task<bool> TagExists(int id)
        {
            return await tagRepository.IsTagExist(id);
        }


    }
}
