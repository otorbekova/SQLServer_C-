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
    public class SaleOfProductsController : Controller
    {
        private readonly modulContext _context;

        public SaleOfProductsController(modulContext context)
        {
            _context = context;
        }

        // GET: SaleOfProducts
        public async Task<IActionResult> Index()
        {
            var prod = await _context.SaleOfProducts.FromSqlRaw("dbo.indexSaleOfProducts").ToListAsync();
            var dataFinish = await _context.FinishedProducts.FromSqlRaw("dbo.indexFinishedProduct").ToListAsync();
            var dataEmpl = await _context.Employees.FromSqlRaw("dbo.indexEmployees").ToListAsync();
           
            foreach (var e in prod)
            {
                foreach (var l in dataFinish)
                {
                    foreach (var p in dataEmpl)
                    {
                        if (e.Продукция == l.Id)
                        {
                            if (e.Сотрудник == p.Id)
                            {
                                e.Продукция_.Наименование = l.Наименование;
                                e.Сотрудник_.Фио = p.Фио;

                            }
                        }
                    }
                }
            }
            return View(prod);
        }

        // GET: SaleOfProducts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var saleOfProduct = await _context.SaleOfProducts
                .Include(s => s.Продукция_)
                .Include(s => s.Сотрудник_)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (saleOfProduct == null)
            {
                return NotFound();
            }

            return View(saleOfProduct);
        }

        // GET: SaleOfProducts/Create
        public IActionResult Create()
        {
            var prod = _context.FinishedProducts.FromSqlRaw("dbo.indexFinishedProduct").ToList();
            var Empl = _context.Employees.FromSqlRaw("dbo.indexEmployees").ToList();
            ViewData["Продукция"] = new SelectList(prod, "Id", "Наименование");
            ViewData["Сотрудник"] = new SelectList(Empl, "Id", "Фио");
            return View();
        }

        // POST: SaleOfProducts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SaleOfProduct sale)
        {
            if (ModelState.IsValid)
            {
                SqlParameter produk = new SqlParameter("@prod", sale.Продукция);
                SqlParameter kol = new SqlParameter("@kol", sale.Количество);
                SqlParameter sum = new SqlParameter("@sum", sale.Сумма);
                SqlParameter data = new SqlParameter("@data", sale.Дата);
                SqlParameter empl = new SqlParameter("@empl", sale.Сотрудник);

               // await _context.Database.ExecuteSqlRawAsync("exec dbo.createSaleOfProducts @prod, @kol,@sum,@data,@empl",
                 //           produk, kol,sum, data, empl);1

                var outParam = new SqlParameter
                {
                    ParameterName = "@out",
                    DbType = System.Data.DbType.Int32,
                    Size = 10,
                    Direction = System.Data.ParameterDirection.Output
                };

                //    await _context.Database.ExecuteSqlRawAsync("exec dbo.Lab_4_Product @prod,@kol,@r OUT", produk, kol, outParam);
                await _context.Database.ExecuteSqlRawAsync("exec dbo.createSaleOfProducts @prod, @kol,@sum,@data,@empl,@out OUT",
                          produk, kol, sum, data, empl,outParam);
                if (outParam.SqlValue.ToString() == "0")
                {
                    ModelState.AddModelError("Количество", "Не хватает готовой продукции!!");
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
            return View(sale);
        }

        // GET: SaleOfProducts/Edit/51
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            SqlParameter Id = new SqlParameter("@id", id);
            var prod = await _context.SaleOfProducts.FromSqlRaw("dbo.SelecrByIdSaleOfProducts @id", Id).ToListAsync();
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

        // POST: SaleOfProducts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SaleOfProduct sale)
        {
            if (id != sale.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    SqlParameter Id = new SqlParameter("@id", sale.Id);
                    SqlParameter produk = new SqlParameter("@prod", sale.Продукция);
                    SqlParameter kol = new SqlParameter("@kol", sale.Количество);
                    SqlParameter sum = new SqlParameter("@sum", sale.Сумма);
                    SqlParameter data = new SqlParameter("@data", sale.Дата);
                    SqlParameter empl = new SqlParameter("@empl", sale.Сотрудник);

                   
                    var outParam = new SqlParameter
                    {
                        ParameterName = "@out",
                        DbType = System.Data.DbType.Int32,
                        Size = 10,
                        Direction = System.Data.ParameterDirection.Output
                    };

                    //    await _context.Database.ExecuteSqlRawAsync("exec dbo.Lab_4_Product @prod,@kol,@r OUT", produk, kol, outParam);
                    await _context.Database.ExecuteSqlRawAsync("exec dbo.editSaleOfProducts @id,@prod, @kol,@sum,@data,@empl,@out",
                                Id, produk, kol, sum, data, empl, outParam);
                    if (outParam.SqlValue.ToString() == "0")
                    {
                        ModelState.AddModelError("Количество", "Не хватает готовой продукции!!");
                    }
                    else
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SaleOfProductExists(sale.Id))
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

            return View(sale);
        }

        // GET: SaleOfProducts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            SqlParameter Id = new SqlParameter("@Id", id);
            var sale = await _context.SaleOfProducts.FromSqlRaw("dbo.SelecrByIdSaleOfProducts @Id", Id).ToListAsync();

            SqlParameter IdPost = new SqlParameter("@Id", sale[0].Продукция);
            var finsh = await _context.FinishedProducts.FromSqlRaw("dbo.SelectByIdFinishproducts @Id", IdPost).ToListAsync();

            SqlParameter IdRawMat = new SqlParameter("@Id", sale[0].Сотрудник);
            var Emol = await _context.Employees.FromSqlRaw("dbo.SelectByIdEmployees @Id", IdRawMat).ToListAsync();

            if (sale[0] == null)
            {
                return NotFound();
            }
            return View(sale[0]);
        }

        // POST: SaleOfProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            SqlParameter Id = new SqlParameter("@Id", id);//ID
            await _context.Database.ExecuteSqlRawAsync("exec dbo.deleteSaleOfProducts @Id", Id);
            return RedirectToAction(nameof(Index));
        }

        private bool SaleOfProductExists(int id)
        {
            return _context.SaleOfProducts.Any(e => e.Id == id);
        }
    }
}
