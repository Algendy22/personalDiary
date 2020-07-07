using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using PersonalDiaryApp.Models;

namespace PersonalDiaryApp.Controllers
{
    public class DiaryReportsController : Controller
    {
        private readonly IFileProvider fileProvider;
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly DiaryDBContext _context;

        public DiaryReportsController(DiaryDBContext context,
                          IFileProvider fileprovider, IHostingEnvironment env)
        {
            _context = context;
            fileProvider = fileprovider;
            hostingEnvironment = env;
        }
        // GET: DiaryReports
        public async Task<IActionResult> Index(string searchString)
        {



            var movies = from m in _context.DiaryReport

                         select m;
            if (!String.IsNullOrEmpty(searchString))
            {
                movies = movies.Where(s => s.Title.Contains(searchString) || s.Description.Contains(searchString));
                return View(movies.OrderBy(s => s.DiaryDate));
            }
            //return View(await _context.DiaryReport.ToListAsync());
            return View(await _context.DiaryReport.OrderBy(s => s.DiaryDate).ToListAsync());


        }

        // GET: DiaryReports/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var diaryReport = await _context.DiaryReport
                .FirstOrDefaultAsync(m => m.ItemId == id);
            if (diaryReport == null)
            {
                return NotFound();
            }

            return View(diaryReport);
        }

        // GET: DiaryReports/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DiaryReports/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ItemId,Title,DiaryDate,Description,DiaryImage")] DiaryReport diaryReport, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                _context.Add(diaryReport);
                await _context.SaveChangesAsync();

                // Code to upload image if not null
                if (file != null || file.Length != 0)
                {
                    // Create a File Info 
                    FileInfo fi = new FileInfo(file.FileName);

                    // This code creates a unique file name to prevent duplications 
                    // stored at the file location
                    var newFilename = diaryReport.ItemId + "_" + String.Format("{0:d}",
                                      (DateTime.Now.Ticks / 10) % 100000000) + fi.Extension;
                    var webPath = hostingEnvironment.WebRootPath;
                    var path = Path.Combine("", webPath + @"\ImageFiles\" + newFilename);

                    // IMPORTANT: The pathToSave variable will be save on the column in the database
                    var pathToSave = @"/ImageFiles/" + newFilename;

                    // This stream the physical file to the allocate wwwroot/ImageFiles folder
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    // This save the path to the record
                    diaryReport.DiaryImage = pathToSave;
                    _context.Update(diaryReport);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(diaryReport);
        }

        // GET: DiaryReports/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var diaryReport = await _context.DiaryReport.FindAsync(id);
            if (diaryReport == null)
            {
                return NotFound();
            }
            return View(diaryReport);
        }

        // POST: DiaryReports/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ItemId,Title,DiaryDate,Description,DiaryImage")] DiaryReport diaryReport, IFormFile file)
        {
            if (id != diaryReport.ItemId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    _context.Update(diaryReport);
                    await _context.SaveChangesAsync();

                    if (file != null || file.Length != 0)
                    {
                        // Create a File Info 
                        FileInfo fi = new FileInfo(file.FileName);

                        // This code creates a unique file name to prevent duplications 
                        // stored at the file location
                        var newFilename = diaryReport.ItemId + "_" + String.Format("{0:d}",
                                          (DateTime.Now.Ticks / 10) % 100000000) + fi.Extension;
                        var webPath = hostingEnvironment.WebRootPath;
                        var path = Path.Combine("", webPath + @"\ImageFiles\" + newFilename);

                        // IMPORTANT: The pathToSave variable will be save on the column in the database
                        var pathToSave = @"/ImageFiles/" + newFilename;

                        // This stream the physical file to the allocate wwwroot/ImageFiles folder
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        // This save the path to the record
                        diaryReport.DiaryImage = pathToSave;
                        _context.Update(diaryReport);
                        await _context.SaveChangesAsync();
                    }

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DiaryReportExists(diaryReport.ItemId))
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
            return View(diaryReport);
        }

        // GET: DiaryReports/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var diaryReport = await _context.DiaryReport
                .FirstOrDefaultAsync(m => m.ItemId == id);
            if (diaryReport == null)
            {
                return NotFound();
            }

            return View(diaryReport);
        }

        // POST: DiaryReports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var diaryReport = await _context.DiaryReport.FindAsync(id);
            _context.DiaryReport.Remove(diaryReport);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DiaryReportExists(int id)
        {
            return _context.DiaryReport.Any(e => e.ItemId == id);
        }
    }
}
