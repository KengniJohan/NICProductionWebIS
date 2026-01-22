using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NICProductionWebIS.Data;
using NICProductionWebIS.Models;
using NICProductionWebIS.Repositories;
using System.Diagnostics;
using System.Linq;

namespace NICProductionWebIS.Controllers
{
    public class NicController(ApplicationDbContext context, NicRepository repository) : Controller
    {
        private readonly ApplicationDbContext _context = context;
        private readonly NicRepository _repository = repository;

        // GET: Nic
        // Adds pagination and optional search query
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10, string? q = null)
        {
            if (page < 1) page = 1;
            if (pageSize <= 0) pageSize = 10;

            var query = _context.NicTable.AsQueryable();

            if (!string.IsNullOrWhiteSpace(q))
            {
                var normalized = q.Trim().ToLower();
                query = query.Where(n =>
                    n.Name.ToLower().Contains(normalized) ||
                    n.Surname.ToLower().Contains(normalized) ||
                    n.Profession.ToLower().Contains(normalized));
            }

            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var items = await query
                .OrderBy(n => n.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var vm = new NicIndexViewModel
            {
                Items = items,
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = totalPages,
                Query = q
            };

            return View(vm);
        }

        // GET: Nic/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var nic = await _context.NicTable.FindAsync(id);
            if (nic == null) return NotFound();

            return View(nic);
        }
        
        // GET: Nic/Create
        public IActionResult Create()  
        {
            ViewBag.Genders = Enum.GetValues<Gender>()
                .Cast<Gender>()
                .Select(static g => new SelectListItem
                {
                        Value = g.ToString(),
                        Text = g.ToString().StartsWith("M") ? "Masculin" : "Feminin"
                })
                .ToList();
            return View();
        }

        // POST: Nic/Create
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NicModel model, IFormFile? photoFile)
        {
            model.Photo = await _repository.FromImage(photoFile);

            if (ModelState.IsValid)
            {
                model.ExpiredDate = model.IssueDate.AddYears(10);
                _context.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Nic/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var nic = await _context.NicTable.FindAsync(id);
            if (nic == null) return NotFound();

            ViewBag.Genders = Enum.GetValues<Gender>()
                .Cast<Gender>()
                .Select(g => new SelectListItem
                {
                    Value = g.ToString(),
                    Text = g.ToString().StartsWith("M") ? "Masculin" : "Feminin"
                })
                .ToList();

            return View(nic);
        }

        // POST: Nic/Edit/5
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, NicModel model, IFormFile? photoFile, bool removePhoto = false)
        {
            if (id != model.Id) return NotFound();

            // Fetch existing for preservation when no new photo is uploaded
            var existing = await _context.NicTable.AsNoTracking().FirstOrDefaultAsync(n => n.Id == id);
            if (existing == null) return NotFound();

            if (removePhoto)
            {
                model.Photo = null;
            }
            else if (photoFile != null && photoFile.Length > 0)
            {
                // Use uploaded image
                model.Photo = await _repository.FromImage(photoFile);
            }
            else
            {
                // Preserve existing photo when no upload and not removing
                model.Photo = existing.Photo;
            }

            // Preserve IssueDate/ExpiredDate if not bound/changed by the form
            if (model.IssueDate == default) model.IssueDate = existing.IssueDate;
            if (model.ExpiredDate == default) model.ExpiredDate = existing.ExpiredDate;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(model);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _context.NicTable.AnyAsync(e => e.Id == model.Id))
                        return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Genders = Enum.GetValues<Gender>()
                .Cast<Gender>()
                .Select(g => new SelectListItem
                {
                    Value = g.ToString(),
                    Text = g.ToString().StartsWith("M") ? "Masculin" : "Feminin"
                })
                .ToList();

            return View(model);
        }

        // GET: Nic/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var nic = await _context.NicTable.FindAsync(id);
            if (nic == null) return NotFound();
            _context.NicTable.Remove(nic);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: Nic/Delete/5
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var nic = await _context.NicTable.FindAsync(id);
            if (nic != null)
            {
                _context.NicTable.Remove(nic);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}