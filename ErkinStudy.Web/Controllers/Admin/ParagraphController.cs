﻿using System;
using System.Linq;
using System.Threading.Tasks;
using ErkinStudy.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ErkinStudy.Infrastructure.Context;

namespace ErkinStudy.Web.Controllers.Admin
{
    public class ParagraphController : Controller
    {
        private readonly AppDbContext _context;

        public ParagraphController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Folder
        public IActionResult Index(long? degreeId)
        {
	        ViewBag.DegreeId = degreeId;
	        return degreeId.HasValue
		        ? View(_context.Paragraphs.Where(x => x.SubjectId == degreeId).AsQueryable())
		        : View(_context.Paragraphs.AsQueryable());
        }

        // GET: Folder/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paragraph = await _context.Paragraphs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (paragraph == null)
            {
                return NotFound();
            }

            return View(paragraph);
        }

        // GET: Folder/Create
        public IActionResult Create(long degreeId)
        {
	        ViewBag.DegreeId = degreeId;
            return View();
        }

        // POST: Folder/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,Order,SubjectId,IsActive")] Folder folder)
        {
            if (ModelState.IsValid)
            {
				folder.CreatedAt = DateTime.Now;
                _context.Add(folder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { degreeId = folder.SubjectId });
            }
            return View(folder);
        }

        // GET: Folder/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paragraph = await _context.Paragraphs.FindAsync(id);
            if (paragraph == null)
            {
                return NotFound();
            }
            return View(paragraph);
        }

        // POST: Folder/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Name,Description,Order,SubjectId,IsActive")] Folder folder)
        {
            if (id != folder.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(folder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
	                if (!ParagraphExists(folder.Id))
                    {
                        return NotFound();
                    }

	                throw;
                }
                return RedirectToAction(nameof(Index), new { degreeId = folder.SubjectId });
            }
            return View(folder);
        }

        // GET: Folder/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paragraph = await _context.Paragraphs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (paragraph == null)
            {
                return NotFound();
            }

            return View(paragraph);
        }

        // POST: Folder/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var paragraph = await _context.Paragraphs.FindAsync(id);
            _context.Paragraphs.Remove(paragraph);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { degreeId = paragraph.SubjectId });
        }

        private bool ParagraphExists(long id)
        {
            return _context.Paragraphs.Any(e => e.Id == id);
        }
    }
}
