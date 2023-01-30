using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WebApplication5.Models;

namespace WebApplication5.Controllers
{
    public class RawMaterialsController : Controller
    {
        private readonly modulContext _context;

        public RawMaterialsController(modulContext context)
        {
            _context = context;
        }

        // GET: RawMaterials
        public async Task<IActionResult> Index()
        {
            var dataRawMat = await _context.RawMaterials.FromSqlRaw("dbo.indexRawMaterials").ToListAsync();
            var dataUnit = await _context.Units.FromSqlRaw("dbo.indexUnits").ToListAsync();
            foreach (var e in dataRawMat)
            {
                foreach (var p in dataUnit)
                {
                    if (e.ЕдиницаИзмерения == p.Id)
                    {
                        e.ЕдиницаИзмерения_.Наименование = p.Наименование;
                    }
                }
            }
            return View(dataRawMat);
        }

        // GET: RawMaterials/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rawMaterial = await _context.RawMaterials
                .Include(r => r.ЕдиницаИзмерения_)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rawMaterial == null)
            {
                return NotFound();
            }

            return View(rawMaterial);
        }

        // GET: RawMaterials/Create
        public IActionResult Create()
        {
            var unit = _context.Units.FromSqlRaw("dbo.indexUnits").ToList();
            ViewData["ЕдиницаИзмерения"] = new SelectList(unit, "Id", "Наименование");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RawMaterial rawMaterial)
        {
            if (ModelState.IsValid)
            {
                SqlParameter raw = new SqlParameter("@raw", rawMaterial.Наименование);
                SqlParameter unit = new SqlParameter("@unit", rawMaterial.ЕдиницаИзмерения);
                SqlParameter sum = new SqlParameter("@sum", rawMaterial.Сумма);
                SqlParameter kol = new SqlParameter("@kol", rawMaterial.Количество);

                await _context.Database.ExecuteSqlRawAsync("exec dbo.createRawMaterials @raw, @unit, @sum, @kol",
                    raw, unit, sum, kol);

                return RedirectToAction(nameof(Index));
            }
            var uni = _context.Units.FromSqlRaw("dbo.indexUnits").ToList();
            ViewData["ЕдиницаИзмерения"] = new SelectList(uni, "Id", "Наименование");
            return View(rawMaterial);
        }

        // GET: RawMaterials/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            SqlParameter Id = new SqlParameter("@id", id);
            var RawMat = await _context.RawMaterials.FromSqlRaw("dbo.SelectByIdRawMaterials @id", Id).ToListAsync();
            var unit = await _context.Units.FromSqlRaw("dbo.indexUnits").ToListAsync();
            //var employee = await _context.Employees.FindAsync(id);
            if (RawMat[0] == null)
            {
                return NotFound();
            }
            ViewData["ЕдиницаИзмерения"] = new SelectList(unit, "Id", "Наименование");
            return View(RawMat[0]);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, RawMaterial rawMat)
        {
            if (id != rawMat.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    SqlParameter Id = new SqlParameter("@id", rawMat.Id);
                    SqlParameter raw = new SqlParameter("@raw", rawMat.Наименование);
                    SqlParameter unit = new SqlParameter("@unit", rawMat.ЕдиницаИзмерения);
                    SqlParameter sum = new SqlParameter("@sum", rawMat.Сумма);
                    SqlParameter kol = new SqlParameter("@kol", rawMat.Количество);

                    await _context.Database.ExecuteSqlRawAsync("exec dbo.editRawMaterials @id,@raw, @unit, @sum, @kol",
                        Id,raw, unit, sum, kol);
                    //_context.Update(employee);
                    //await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RawMaterialExists(rawMat.Id))
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
            var dataUnit = await _context.Units.FromSqlRaw("dbo.indexUnits").ToListAsync();
            ViewData["ЕдиницаИзмерения"] = new SelectList(dataUnit, "Id", "Наименование");
            return View(rawMat);
        }

        // GET: RawMaterials/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            SqlParameter Id = new SqlParameter("@Id", id);
            var rawMat = await _context.RawMaterials.FromSqlRaw("dbo.SelectByIdRawMaterials @Id", Id).ToListAsync();

            SqlParameter IdPost = new SqlParameter("@Id", rawMat[0].ЕдиницаИзмерения);
            var post = await _context.Units.FromSqlRaw("dbo.selectByIdUnits @Id", IdPost).ToListAsync();
            if (rawMat[0] == null)
            {
                return NotFound();
            }

            return View(rawMat[0]);
        }

        // POST: RawMaterials/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            SqlParameter Id = new SqlParameter("@Id", id);//ID
            await _context.Database.ExecuteSqlRawAsync("exec dbo.deleteRawMaterials @Id", Id);
            return RedirectToAction(nameof(Index));
        }

        private bool RawMaterialExists(int id)
        {
            return _context.RawMaterials.Any(e => e.Id == id);
        }
    }
}
