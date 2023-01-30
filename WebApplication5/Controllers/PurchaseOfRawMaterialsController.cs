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
    public class PurchaseOfRawMaterialsController : Controller
    {
        private readonly modulContext _context;

        public PurchaseOfRawMaterialsController(modulContext context)
        {
            _context = context;
        }

        // GET: PurchaseOfRawMaterials
        public async Task<IActionResult> Index()
        {
            var purch = await _context.PurchaseOfRawMaterials.FromSqlRaw("dbo.indexPurchaseOfRawMaterials").ToListAsync();
            var dataRaw = await _context.RawMaterials.FromSqlRaw("dbo.indexRawMaterials").ToListAsync();
            var dataEmpl = await _context.Employees.FromSqlRaw("dbo.indexEmployees").ToListAsync();
           
            foreach (var e in purch)
            {
                foreach (var l in dataRaw)
                {
                    foreach (var p in dataEmpl)
                    {
                        if (e.Сырьё == l.Id)
                        {
                            if (e.Сотрудник == p.Id)
                            {
                                e.Сырьё_.Наименование = l.Наименование;
                                e.Сотрудник_.Фио = p.Фио;

                            }
                        }
                    }
                }
            }
            return View(purch);
        }

        // GET: PurchaseOfRawMaterials/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchaseOfRawMaterial = await _context.PurchaseOfRawMaterials
                .Include(p => p.Сотрудник_)
                .Include(p => p.Сырьё_)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (purchaseOfRawMaterial == null)
            {
                return NotFound();
            }

            return View(purchaseOfRawMaterial);
        }

        // GET: PurchaseOfRawMaterials/Create
        public IActionResult Create()
        {
            var Raw = _context.RawMaterials.FromSqlRaw("dbo.indexRawMaterials").ToList();
            var Empl = _context.Employees.FromSqlRaw("dbo.indexEmployees").ToList();
            ViewData["Сырьё"] = new SelectList(Raw, "Id", "Наименование");
            ViewData["Сотрудник"] = new SelectList(Empl, "Id", "Фио");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PurchaseOfRawMaterial purch)
        {
          //  Budget b;
         //   var budget = _context.Budgets.Where(u => u.Id == 1).FirstOrDefault();
            if (ModelState.IsValid)
            {

                SqlParameter raw = new SqlParameter("@raw", purch.Сырьё);
                SqlParameter kol = new SqlParameter("@kol", purch.Количество);
                SqlParameter sum = new SqlParameter("@rawmat", purch.Сумма);
                SqlParameter data = new SqlParameter("@data", purch.Дата);
                SqlParameter empl = new SqlParameter("@empl", purch.Сотрудник);
             //   SqlParameter bud = new SqlParameter("budget", purch..СуммаБюджета);
               //  await _context.Database.ExecuteSqlRawAsync("exec dbo.createPurchaseOfRawMaterials @raw, @kol, @rawmat,@data,@empl",
                 //           raw, kol, sum, data, empl);
              //  SqlInfoMessageEventArgs("Erroe not");
        
              

                var outParam = new SqlParameter
                {
                    ParameterName = "@r",
                    DbType = System.Data.DbType.Int32,
                    Size = 100,
                    Direction = System.Data.ParameterDirection.Output
                };

          await _context.Database.ExecuteSqlRawAsync("exec dbo.BuyProduct @raw,@kol,@rawmat,@data,@empl,@r OUT",
              raw, kol, sum, data,empl,outParam);
                if (outParam.SqlValue.ToString() == "0")
                {
                    ModelState.AddModelError("Сумма", "Not budget!");
                }
                else
                {
                    return RedirectToAction(nameof(Index));
                }

               // return RedirectToAction(nameof(Index));
            }
            var Raw = _context.RawMaterials.FromSqlRaw("dbo.indexRawMaterials").ToList();
            var Empl = _context.Employees.FromSqlRaw("dbo.indexEmployees").ToList();
            ViewData["Сырьё"] = new SelectList(Raw, "Id", "Наименование");
            ViewData["Сотрудник"] = new SelectList(Empl, "Id", "Фио");
            return View(purch);
        }

        // GET: PurchaseOfRawMaterials/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            SqlParameter Id = new SqlParameter("@id", id);
            var purch = await _context.PurchaseOfRawMaterials.FromSqlRaw("dbo.SelectByIdPurchaseOfRawMaterials @id", Id).ToListAsync();
            var dataProd = await _context.RawMaterials.FromSqlRaw("dbo.indexRawMaterials").ToListAsync();
            var Empl = await _context.Employees.FromSqlRaw("dbo.indexEmployees").ToListAsync();
            if (purch[0] == null) 
            {

                return NotFound();
            }
            ViewData["Сырьё"] = new SelectList(dataProd, "Id", "Наименование", purch[0].Сырьё);
            ViewData["Сотрудник"] = new SelectList(Empl, "Id", "Фио", purch[0].Сотрудник);
            //ViewData["ЕдиницаИзмерения"] = new SelectList(dataUnits, "Id", "Наименование", finish[0].ЕдиницаИзмерения);
            return View(purch[0]);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PurchaseOfRawMaterial purch)
        {
            if (id != purch.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    SqlParameter Ihd = new SqlParameter("@id", purch.Id);
                    SqlParameter raw = new SqlParameter("@raw", purch.Сырьё);
                    SqlParameter kol = new SqlParameter("@kol", purch.Количество);
                    SqlParameter sum = new SqlParameter("@sum", purch.Сумма);
                    SqlParameter data = new SqlParameter("@data", purch.Дата);
                    SqlParameter emplo = new SqlParameter("@empl", purch.Сотрудник);

                    
                    var outParam = new SqlParameter
                    {
                        ParameterName = "@r",
                        DbType = System.Data.DbType.Int32,
                        Size = 100,
                        Direction = System.Data.ParameterDirection.Output
                    };

                    await _context.Database.ExecuteSqlRawAsync("exec dbo.editPurchaseOfRawMaterials @id, @raw, @kol, @sum,@data,@empl,@r",
                             Ihd, raw, kol, sum, data, emplo, outParam);
                    if (outParam.SqlValue.ToString() == "0")
                    {
                        ModelState.AddModelError("Сумма", "Not budget!");
                        //  ViewBag.Message = "Валидация пройдена";
                    }
                  //  else
                    //{
                      //  return RedirectToAction(nameof(Index));
                    //}

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PurchaseOfRawMaterialExists(purch.Id))
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
            var prod = _context.RawMaterials.FromSqlRaw("dbo.indexRawMaterials").ToList();
            var empl = _context.Employees.FromSqlRaw("dbo.indexEmployees").ToList();

            ViewData["Сырьё"] = new SelectList(prod, "Id", "Наименование");
            ViewData["Сотрудник"] = new SelectList(empl, "Id", "Наименование");
            return View(purch);
        }

        // GET: PurchaseOfRawMaterials/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            SqlParameter Id = new SqlParameter("@Id", id);
            var purch = await _context.PurchaseOfRawMaterials.FromSqlRaw("dbo.SelectByIdPurchaseOfRawMaterials @Id", Id).ToListAsync();

            SqlParameter IdPost = new SqlParameter("@Id", purch[0].Сырьё);
            var RawMat = await _context.RawMaterials.FromSqlRaw("dbo.SelectByIdRawMaterials @Id", IdPost).ToListAsync();

            SqlParameter IdRawMat = new SqlParameter("@Id", purch[0].Сотрудник);
            var Empl = await _context.Employees.FromSqlRaw("dbo.SelectByIdEmployees @Id", IdRawMat).ToListAsync();

            if (purch[0] == null)
            {
                return NotFound();
            }

            return View(purch[0]);
        }

        // POST: PurchaseOfRawMaterials/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
           // PurchaseOfRawMaterial purch;
            SqlParameter Id = new SqlParameter("@Id", id);//ID
            //SqlParameter kol = new SqlParameter("@kol", purch.Количество);
            //SqlParameter sum = new SqlParameter("@sum", purch.Сумма);
            await _context.Database.ExecuteSqlRawAsync("exec dbo.deletePurchaseOfRawMaterials @Id", Id);
            return RedirectToAction(nameof(Index));
        }

        private bool PurchaseOfRawMaterialExists(int id)
        {
            return _context.PurchaseOfRawMaterials.Any(e => e.Id == id);
        }
    }
}
