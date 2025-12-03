using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NICProductionWebIS.Data;
using NICProductionWebIS.Models;
using System.Diagnostics;

namespace NICProductionWebIS.Controllers
{
    public class NicController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NicController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Nic
        public async Task<IActionResult> Index()
        {
            var list = await _context.NicTable.ToListAsync();
            return View(list);
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
            if (photoFile != null && photoFile.Length > 0)
            {
                using var ms = new MemoryStream();
                await photoFile.CopyToAsync(ms);
                model.Photo = ms.ToArray();
            }

            if (ModelState.IsValid)
            {
                model.ExpiredDate = model.IssueDate.AddYears(10);
                Console.WriteLine("ACCeptation :" + model.Name);
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

            if (photoFile != null && photoFile.Length > 0)
            {
                using var ms = new MemoryStream();
                await photoFile.CopyToAsync(ms);
                model.Photo = ms.ToArray();
            }
            else if (removePhoto)
            {
                model.Photo = null;
            }
            else
            {
                // preserve existing photo if none uploaded — fetch existing bytes
                var existing = await _context.NicTable.AsNoTracking().FirstOrDefaultAsync(n => n.Id == id);
                if (existing != null)
                {
                    model.Photo = existing.Photo;
                }
            }

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
            return View(model);
        }

        // GET: Nic/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var nic = await _context.NicTable.FindAsync(id);
            if (nic == null) return NotFound();
            return View(nic);
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
