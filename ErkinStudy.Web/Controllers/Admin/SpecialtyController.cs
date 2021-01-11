using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ErkinStudy.Domain.Entities.UbtHub;
using ErkinStudy.Infrastructure.Context;
using ErkinStudy.Web.Models.UbtHub;
using Microsoft.Extensions.Logging;

namespace ErkinStudy.Web.Controllers.Admin
{
    public class SpecialtyController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<SpecialtyController> _logger;

        public SpecialtyController(AppDbContext context, ILogger<SpecialtyController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Specialty
        public async Task<IActionResult> Index()
        {
            return View(await _context.Specialties.ToListAsync());
        }

        // GET: Specialty/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Specialty/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,About,Description,MediaUrl")] Specialty specialty)
        {
            if (ModelState.IsValid)
            {
                _context.Add(specialty);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(specialty);
        }

        // GET: Specialty/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var specialty = await _context.Specialties.FindAsync(id);
            if (specialty == null)
            {
                return NotFound();
            }
            return View(specialty);
        }

        // POST: Specialty/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,About,Description,MediaUrl")] Specialty specialty)
        {
            if (id != specialty.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(specialty);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SpecialtyExists(specialty.Id))
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
            return View(specialty);
        }

        // GET: Specialty/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var specialty = await _context.Specialties
                .FirstOrDefaultAsync(m => m.Id == id);
            if (specialty == null)
            {
                return NotFound();
            }

            return View(specialty);
        }

        // POST: Specialty/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var specialty = await _context.Specialties.FindAsync(id);
            _context.Specialties.Remove(specialty);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SpecialtyExists(int id)
        {
            return _context.Specialties.Any(e => e.Id == id);
        }

        #region Subjects
        public async Task<IActionResult> ManageSubjects(int id)
        {
            var specialty = await _context.Specialties.FirstOrDefaultAsync(x => x.Id == id);
            if (specialty == null)
                return RedirectToAction("Index");

            return View(specialty);
        }

        public async Task<JsonResult> AddSubjects([FromBody] AddSpecialtyItemsViewModel model)
        {
            try
            {
                var specialty = await _context.Specialties.FirstOrDefaultAsync(x => x.Id == model.SpecialtyId);
                if (specialty == null)
                    return Json(0);
                foreach (var subject in model.CheckedItems)
                {
                    await _context.SpecialtySubjects.AddAsync(new SpecialtySubject()
                    { SpecialtyId = specialty.Id, SubjectId = short.Parse(subject) });
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError($"Ошибка при добавлений предметов {e}");
            }
            return Json(0);
        }

        public async Task<IActionResult> DeleteSubject(int specialtyId, short subjectId)
        {
            try
            {
                var subjectSpecialty =
                  await _context.SpecialtySubjects.FirstOrDefaultAsync(x =>
                        x.SpecialtyId == specialtyId && x.SubjectId == subjectId);
                if (subjectSpecialty == null)
                {
                    _logger.LogError($"Нету такой subjectSpecialty specId:{specialtyId}, subjId {subjectId}");
                    return RedirectToAction("ManageSubjects", new { id = specialtyId });
                }
                _context.SpecialtySubjects.Remove(subjectSpecialty);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError($"Ошибка при удалений subjectSpecialty  specId:{specialtyId}, subjId {subjectId}, {e}");
            }
            return RedirectToAction("ManageSubjects", new { id = specialtyId });
        }

        #endregion


        #region Universities
        public async Task<IActionResult> ManageUniversities(int id)
        {
            var specialty = await _context.Specialties.FirstOrDefaultAsync(x => x.Id == id);
            if (specialty == null)
                return RedirectToAction("Index");

            return View(specialty);
        }

        public async Task<JsonResult> AddUniversities([FromBody] AddSpecialtyItemsViewModel model)
        {
            try
            {
                var specialty = await _context.Specialties.FirstOrDefaultAsync(x => x.Id == model.SpecialtyId);
                if (specialty == null)
                    return Json(0);
                foreach (var item in model.CheckedItems)
                {
                    await _context.UniversitySpecialties.AddAsync(new UniversitySpecialty()
                    { SpecialtyId = specialty.Id, UniversityId = int.Parse(item) });
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError($"Ошибка при добавлений UniversitySpecialties {e}");
            }
            return Json(0);
        }

        public async Task<IActionResult> DeleteUniversity(int specialtyId, short universityId)
        {
            try
            {
                var universitySpecialty =
                  await _context.UniversitySpecialties.FirstOrDefaultAsync(x =>
                        x.SpecialtyId == specialtyId && x.UniversityId == universityId);
                if (universitySpecialty == null)
                {
                    _logger.LogError($"Нету такой universitySpecialty specId:{specialtyId}, univerId {universityId}");
                    return RedirectToAction("ManageUniversities", new { id = specialtyId });
                }
                _context.UniversitySpecialties.Remove(universitySpecialty);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError($"Ошибка при удалений universitySpecialty  specId:{specialtyId}, univerId {universityId}, {e}");
            }
            return RedirectToAction("ManageUniversities", new { id = specialtyId });
        }


        #endregion
    }
}
