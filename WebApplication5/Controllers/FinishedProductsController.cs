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
    public class FinishedProductsController : Controller
    {
        private readonly modulContext _context;

        public FinishedProductsController(modulContext context)
        {
            _context = context;
        }

        // GET: FinishedProducts
        public async Task<IActionResult> Index()
        {
            var datafinish = await _context.FinishedProducts.FromSqlRaw("dbo.indexFinishedProduct").ToListAsync();
            var dataUnits = await _context.Units.FromSqlRaw("dbo.indexUnits").ToListAsync();
            foreach (var e in datafinish)
            {
                foreach (var p in dataUnits)
                {
                    if (e.ЕдиницаИзмерения == p.Id)
                    {
                        e.ЕдиницаИзмерения_.Наименование = p.Наименование;
                    }
                }
            }
            return View(datafinish);
        }

     
        public async Task<IActionResult> Details(int? id)
        {
            
            if (id == null)
            {
                return NotFound();
            }
            SqlParameter Id = new SqlParameter("@id", id);
            var finish = await _context.FinishedProducts.FromSqlRaw("dbo.SelectByIdFinishproducts @id", Id).ToListAsync();
            var dataUnits = await _context.Units.FromSqlRaw("dbo.indexUnits").ToListAsync();
            if (finish[0] == null)
            {
                return NotFound();
            }
            ViewData["ЕдиницаИзмерения"] = new SelectList(dataUnits, "Id", "Наименование", finish[0].ЕдиницаИзмерения);
            return View(finish[0]);
        }


        public IActionResult Create()
        {
            var units = _context.Units.FromSqlRaw("dbo.indexUnits").ToList();
            ViewData["ЕдиницаИзмерения"] = new SelectList(units, "Id", "Наименование");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FinishedProduct finished)
        {
            if (ModelState.IsValid)
            {
                SqlParameter name = new SqlParameter("@name", finished.Наименование);
                SqlParameter unit = new SqlParameter("@ed_is", finished.ЕдиницаИзмерения);
                SqlParameter summa = new SqlParameter("@sum", finished.Сумма);
                SqlParameter kol = new SqlParameter("@kol", finished.Количество);
             
  await _context.Database.ExecuteSqlRawAsync("exec dbo.createFinishedProducts @name, @ed_is, @sum, @kol", 
      name, unit, summa, kol);

                return RedirectToAction(nameof(Index));
            }
            var units = _context.Units.FromSqlRaw("dbo.indexUnits").ToList();
            ViewData["ЕдиницаИзмерения"] = new SelectList(units, "Id", "Наименование");
            return View(finished);
        }

        // GET: FinishedProducts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            SqlParameter Id = new SqlParameter("@id", id);
            var finish = await _context.FinishedProducts.FromSqlRaw("dbo.SelectByIdFinishproducts @id", Id).ToListAsync(); 
            var dataUnits = await _context.Units.FromSqlRaw("dbo.indexUnits").ToListAsync();

            if (finish[0] == null)
            {
                return NotFound();
            }
            ViewData["ЕдиницаИзмерения"] = new SelectList(dataUnits, "Id", "Наименование", finish[0].ЕдиницаИзмерения);
            return View(finish[0]);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, FinishedProduct finished)
        {
            if (id != finished.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    SqlParameter Id = new SqlParameter("@id", finished.Id);
                    SqlParameter name = new SqlParameter("@name", finished.Наименование);
                    SqlParameter unit = new SqlParameter("@unit", finished.ЕдиницаИзмерения);
                    SqlParameter summa = new SqlParameter("@summa", finished.Сумма);
                    SqlParameter kol = new SqlParameter("@kol", finished.Количество);


    await _context.Database.ExecuteSqlRawAsync("exec dbo.editFinishedProducts @id,@name, @unit, @summa, @kol",
        Id, name, unit, summa, kol);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FinishedProductExists(finished.Id))
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
            var units = _context.Units.FromSqlRaw("dbo.indexUnits").ToList();
            ViewData["ЕдиницаИзмерения"] = new SelectList(units, "Id", "Наименование");
            return View(finished);
        }

        // GET: FinishedProducts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            SqlParameter Id = new SqlParameter("@Id", id);
            var finisheds = await _context.FinishedProducts.FromSqlRaw("dbo.SelectByIdFinishproducts @Id", Id).ToListAsync();

            SqlParameter IdPost = new SqlParameter("@Id", finisheds[0].ЕдиницаИзмерения);
            var units = await _context.Units.FromSqlRaw("dbo.selectByIdUnits @Id", IdPost).ToListAsync();
            if (finisheds[0] == null)
            {
                return NotFound();
            }

            return View(finisheds[0]);
        }

        // POST: FinishedProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            SqlParameter Id = new SqlParameter("@Id", id);//ID
            await _context.Database.ExecuteSqlRawAsync("exec dbo.deleteFinishedProducts @Id", Id);
            return RedirectToAction(nameof(Index));
        }

        private bool FinishedProductExists(int id)
        {
            return _context.FinishedProducts.Any(e => e.Id == id);
        }
    }
}
