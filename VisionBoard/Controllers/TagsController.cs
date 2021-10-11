﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VisionBoard.DAL;
using VisionBoard.Models;

namespace VisionBoard.Controllers
{
    public class TagsController : Controller
    {
        private readonly ITagRepository tagRepository;

        public TagsController( ITagRepository tagRepository)
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
            if (id == null)
            {
                return NotFound();
            }

            var tag = await tagRepository.GetTag((int)id);

            if (tag == null)
            {
                return NotFound();
            }

            return View(tag);
        }


        // GET: Tags/Create
        public IActionResult Create()
        {
            return View();
        }


        // POST: Tags/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Colour,Status")] Tag tag)
        {
            if (ModelState.IsValid)
            {
                await tagRepository.AddTag(tag);
                return RedirectToAction(nameof(Index));
            }
            return View(tag);
        }


        // GET: Tags/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tag = await tagRepository.GetTag((int) id);

            if (tag == null)
            {
                return NotFound();
            }
            return View(tag);
        }


        // POST: Tags/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Colour,Status")] Tag tag)
        {
            if (id != tag.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await tagRepository.UpdateTag(tag);
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
                return RedirectToAction(nameof(Index));
            }
            return View(tag);
        }


        // GET: Tags/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tag = await tagRepository.GetTag((int)id);
            if (tag == null)
            {
                return NotFound();
            }

            return View(tag);
        }


        // POST: Tags/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await tagRepository.DeleteTag(id);
            return RedirectToAction(nameof(Index));
        }


        private async Task<bool> TagExists(int id)
        {
            return await tagRepository.IsTagExist(id);
        }


    }
}
