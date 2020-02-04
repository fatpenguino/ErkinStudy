﻿using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ErkinStudy.Domain.Entities;
using ErkinStudy.Domain.Entities.OnlineCourses;
using ErkinStudy.Infrastructure.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace ErkinStudy.Web.Controllers.Admin
{
    public class OnlineCourseWeekController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _appEnvironment;
        public OnlineCourseWeekController(AppDbContext context, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
        }

        // GET: OnlineCourseWeek
        [Authorize]
        public IActionResult Index(long? onlineCourseId)
        {
            ViewBag.OnlineCourseId = onlineCourseId;
            return onlineCourseId.HasValue ? View(_context.OnlineCourseWeeks.Include(l => l.OnlineCourse).Where(x => x.OnlineCourseId == onlineCourseId).AsQueryable())
                : View(_context.OnlineCourseWeeks.Include(x => x.OnlineCourse).AsQueryable());
        }

        // GET: OnlineCourseWeek/Details/5
        [Authorize]
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var onlineCourseWeek = await _context.OnlineCourseWeeks
                .Include(o => o.OnlineCourse)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (onlineCourseWeek == null)
            {
                return NotFound();
            }

            return View(onlineCourseWeek);
        }

        // GET: OnlineCourseWeek/Create
        [Authorize]
        public IActionResult Create(long onlineCourseId)
        {
            ViewBag.OnlineCourseId = onlineCourseId;
            return View();
        }

        // POST: OnlineCourseWeek/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,OnlineCourseId,Name,Description,StartDate,Order,StreamUrl")] OnlineCourseWeek onlineCourseWeek)
        {
            if (ModelState.IsValid)
            {
                _context.Add(onlineCourseWeek);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { onlineCourseId = onlineCourseWeek.OnlineCourseId});
            }
            return View(onlineCourseWeek);
        }

        // GET: OnlineCourseWeek/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var onlineCourseWeek = await _context.OnlineCourseWeeks.FindAsync(id);
            if (onlineCourseWeek == null)
            {
                return NotFound();
            }
            return View(onlineCourseWeek);
        }

        // POST: OnlineCourseWeek/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        
        [Authorize]
        public async Task<IActionResult> Edit(long id, [Bind("Id,OnlineCourseId,Name,Description,StartDate,Order,StreamUrl")] OnlineCourseWeek onlineCourseWeek)
        {
            if (id != onlineCourseWeek.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(onlineCourseWeek);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OnlineCourseWeekExists(onlineCourseWeek.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { onlineCourseId = onlineCourseWeek.OnlineCourseId });
            }
            return View(onlineCourseWeek);
        }

        // GET: OnlineCourseWeek/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var onlineCourseWeek = await _context.OnlineCourseWeeks
                .Include(o => o.OnlineCourse)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (onlineCourseWeek == null)
            {
                return NotFound();
            }

            return View(onlineCourseWeek);
        }
        [Authorize]
        public async Task<IActionResult> Homeworks(long id)
        {
            var homework = await _context.OnlineCourseWeeks.Include(x => x.Homeworks).FirstOrDefaultAsync(x => x.Id == id);
            return View(homework);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UploadHomework(IFormFile uploadedHomework, long onlineCourseWeekId)
        {
            if (uploadedHomework != null)
            {
                // путь к папке Homeworks
                string path = "/Homeworks/" + uploadedHomework.FileName;
                // сохраняем файл в папку Homeworks в каталоге wwwroot
                await using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await uploadedHomework.CopyToAsync(fileStream);
                }
                var homework = new Homework() { Name = uploadedHomework.FileName, Path = path, OnlineCourseWeekId = onlineCourseWeekId, UploadTime = DateTime.UtcNow};
                _context.Homeworks.Add(homework);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Homeworks", new { id = onlineCourseWeekId });
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> DeleteHomework(long id)
        {
            var homework = await _context.Homeworks.FirstOrDefaultAsync(x => x.Id == id);
            var onlineCourseWeekId = homework.OnlineCourseWeekId;
            _context.Homeworks.Remove(homework);
            await _context.SaveChangesAsync();
            try
            {
                System.IO.File.Delete(_appEnvironment.WebRootPath + homework.Path);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return RedirectToAction("Homeworks", new { id = onlineCourseWeekId });
            }
            return RedirectToAction("Homeworks", new {id = onlineCourseWeekId});
        }

        [Authorize]
        public async Task<ActionResult> DownloadHomework(long id)
        {
            var homework = await _context.Homeworks.FirstOrDefaultAsync(x => x.Id == id);
            byte[] fileBytes = System.IO.File.ReadAllBytes(_appEnvironment.WebRootPath + homework.Path);
            return File(fileBytes, "application/force-download", homework.Name);
        }

        // POST: OnlineCourseWeek/Delete/5
        [HttpPost, ActionName("Delete")]
        
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var onlineCourseWeek = await _context.OnlineCourseWeeks.FindAsync(id);
            _context.OnlineCourseWeeks.Remove(onlineCourseWeek);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { onlineCourseId = onlineCourseWeek.OnlineCourseId });
        }

        private bool OnlineCourseWeekExists(long id)
        {
            return _context.OnlineCourseWeeks.Any(e => e.Id == id);
        }
    }
}
