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
    public class ProductionsController : Controller
    {
        private readonly modulContext _context;

        public ProductionsController(modulContext context)
        {
            _context = context;
        }

        // GET: Productions
        public async Task<IActionResult> Index()
        {
            var prod = await _context.Productions.FromSqlRaw("dbo.indexProduction").ToListAsync();
            var dataEmpl = await _context.Employees.FromSqlRaw("dbo.indexEmployees").ToListAsync();
            var dataFinish = await _context.FinishedProducts.FromSqlRaw("dbo.indexFinishedProduct").ToListAsync();
            foreach (var e in prod)
            {
                foreach (var l in dataFinish)    {
                foreach (var p in dataEmpl)   {
                        if (e.Продукция == l.Id)
                            {
                        if (e.Сотрудник == p.Id)
                        { 
                                e.Продукция_.Наименование = l.Наименование;
                                e.Сотрудники.Фио = p.Фио;
                               
                            }
                        }
                    }
                }
            }
            return View(prod);
        }

        // GET: Productions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var production = await _context.Productions
                .Include(p => p.Продукция_)
                .Include(p=>p.Сотрудники)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (production == null)
            {
                return NotFound();
            }

            return View(production);
        }

        // GET: Productions/Create
        public IActionResult Create()
        {
            var prod = _context.FinishedProducts.FromSqlRaw("dbo.indexFinishedProduct").ToList();
            var Empl = _context.Employees.FromSqlRaw("dbo.indexEmployees").ToList();
            ViewData["Продукция"] = new SelectList(prod, "Id", "Наименование");
            ViewData["Сотрудник"] = new SelectList(Empl, "Id", "Фио");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Production prod)
        {
            if (ModelState.IsValid)
            {
                SqlParameter produk = new SqlParameter("@prod", prod.Продукция);
                SqlParameter kol = new SqlParameter("@kol", prod.Количество);
                SqlParameter data = new SqlParameter("@data", prod.Дата);
                SqlParameter empl = new SqlParameter("@empl", prod.Сотрудник);
                   var outParam = new SqlParameter
                {
                    ParameterName = "@r",
                    DbType = System.Data.DbType.Int32,
                    Size = 10,
                    Direction = System.Data.ParameterDirection.Output
                };

            await _context.Database.ExecuteSqlRawAsync("exec dbo.Lab_4_Product @prod,@kol,@data,@empl,@r OUT",
                produk, kol, data, empl, outParam);
    // await _context.Database.ExecuteSqlRawAsync("exec dbo.createProduction @prod, @kol,@data,@empl",
         //                   produk, kol, data, empl);5 
                 if (outParam.SqlValue.ToString() == "0")
               {
                    ModelState.AddModelError("Количество", "Не хватае сырья!");
                }
                else
                {
                    return RedirectToAction(nameof(Index));
              }

            }
            var produ = _context.FinishedProducts.FromSqlRaw("dbo.indexFinishedProduct").ToList();
            var Empl = _context.Employees.FromSqlRaw("dbo.indexEmployees").ToList();
            ViewData["Продукция"] = new SelectList(produ, "Id", "Наименование");
            ViewData["Сотрудник"] = new SelectList(Empl, "Id", "Фио");
            return View(prod);
        }

        // GET: Productions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            SqlParameter Id = new SqlParameter("@id", id);
            var prod = await _context.Productions.FromSqlRaw("dbo.selectByIdProduction @id", Id).ToListAsync();
            var dataProd = await _context.FinishedProducts.FromSqlRaw("dbo.indexFinishedProduct").ToListAsync();
            var Empl = await _context.Employees.FromSqlRaw("dbo.indexEmployees").ToListAsync();
           
            if (prod[0] == null)
            {
                return NotFound();
            }
            ViewData["Продукция"] = new SelectList(dataProd, "Id", "Наименование", prod[0].Продукция);
            ViewData["Сотрудник"] = new SelectList(Empl, "Id", "Фио", prod[0].Сотрудник);
            return View(prod[0]);
        }

        // POST: Productions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Production prod)
        {
            if (id != prod.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    SqlParameter Id = new SqlParameter("@id", prod.Id);
                    SqlParameter produk = new SqlParameter("@produ", prod.Продукция);
                    SqlParameter kol = new SqlParameter("@kol", prod.Количество);
                    SqlParameter data = new SqlParameter("@data", prod.Дата);
                    SqlParameter empl = new SqlParameter("@empl", prod.Сотрудник);

                    var outParam = new SqlParameter
                    {
                        ParameterName = "@r",
                        DbType = System.Data.DbType.Int32,
                        Size = 10,
                        Direction = System.Data.ParameterDirection.Output
                    };

                    // await _context.Database.ExecuteSqlRawAsync("exec dbo.Lab_4_Product @prod,@kol,@data,@empl,@r OUT",
                    //   produk, kol, data, empl, outParam);
                    //

                    await _context.Database.ExecuteSqlRawAsync("exec dbo.editProduction @id,@produ, @kol,@data,@empl,@r OUT",
                               Id, produk, kol, data, empl, outParam);
                    //await _context.Database.ExecuteSqlRawAsync("exec dbo.createProduction @prod, @kol,@data,@empl",
                    //                   produk, kol, data, empl);5 
                    if (outParam.SqlValue.ToString() == "0")
                    {
                        ModelState.AddModelError("Количество", "Не хватае сырья!");
                    }
                    else
                    {
                        return RedirectToAction(nameof(Index));
                    }

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductionExists(prod.Id))
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
            var prodd = _context.FinishedProducts.FromSqlRaw("dbo.indexFinishedProduct").ToList();
            var Empl = _context.Employees.FromSqlRaw("dbo.indexEmployees").ToList();

            ViewData["Продукция"] = new SelectList(prodd, "Id", "Наименование");
           // ViewData["Сырьё"] = new SelectList(RawMat, "Id", "Наименование");
            ViewData["Сотрудник"] = new SelectList(Empl, "Id", "Фио");
          
            return View(prod);
        }

        // GET: Productions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            SqlParameter Id = new SqlParameter("@Id", id);
            var prod = await _context.Productions.FromSqlRaw("dbo.selectByIdProduction @Id", Id).ToListAsync();

            SqlParameter IdPost = new SqlParameter("@Id", prod[0].Продукция);
            var finsh = await _context.FinishedProducts.FromSqlRaw("dbo.SelectByIdFinishproducts @Id", IdPost).ToListAsync();

            SqlParameter IdRawMat = new SqlParameter("@Id", prod[0].Сотрудник);
            var Emol = await _context.Employees.FromSqlRaw("dbo.SelectByIdEmployees @Id", IdRawMat).ToListAsync();

            if (prod[0] == null)
            {
                return NotFound();
            }

            return View(prod[0]);
        }

        // POST: Productions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            SqlParameter Id = new SqlParameter("@Id", id);//ID
            await _context.Database.ExecuteSqlRawAsync("exec dbo.deleteProduction @Id", Id);
            return RedirectToAction(nameof(Index));
        }

        private bool ProductionExists(int id)
        {
            return _context.Productions.Any(e => e.Id == id);
        }
    }
}
