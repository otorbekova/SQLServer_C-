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
    public class IngredientsController : Controller
    {
        private readonly modulContext _context;

        public IngredientsController(modulContext context)
        {
            _context = context;
        }

        // GET: Ingredients
        public async Task<IActionResult> Index()
        {
            var dataIngr = await _context.Ingredients.FromSqlRaw("dbo.indexIngredients").ToListAsync();
            var dataProd = await _context.FinishedProducts.FromSqlRaw("dbo.indexFinishedProduct").ToListAsync();
            var dataRawMat = await _context.RawMaterials.FromSqlRaw("dbo.indexRawMaterials").ToListAsync();
            foreach (var e in dataIngr)
            {
                foreach (var p in dataProd)
                {
                    foreach (var l in dataRawMat)
                    {
                        if (e.Продукция == p.Id)
                            if (e.Сырьё == l.Id)
                            {
                                e.Продукция_.Наименование = p.Наименование;
                                e.Сырьё_.Наименование = l.Наименование;
                            }
                    
                }
            }
        }
    
            return View(dataIngr);
        }

        // GET: Ingredients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            SqlParameter Id = new SqlParameter("@id", id);
            var ingr = await _context.Ingredients.FromSqlRaw("dbo.selectByIdIngredients @id", Id).ToListAsync();
            var dataProd = _context.FinishedProducts.FromSqlRaw("dbo.indexFinishedProduct").ToList();
            var dataRawMat = await _context.RawMaterials.FromSqlRaw("dbo.indexRawMaterials").ToListAsync();
           // var dataprod = await _context.Productions.FromSqlRaw("dbo.indexUnits").ToListAsync();
            if (ingr[0] == null)
            {
                return NotFound();
            }
            ViewData["Продукция"] = new SelectList(dataProd, "Id", "Наименование", ingr[0].Продукция);
            ViewData["Сырьё"] = new SelectList(dataRawMat, "Id", "Наименование", ingr[0].Сырьё);
          //  ViewData["ЕдиницаИзмерения"] = new SelectList(dataUnits, "Id", "Наименование", finish[0].ЕдиницаИзмерения);

            return View(ingr[0]);
        }

        // GET: Ingredients/Create
        public IActionResult Create()
        {
            var prod = _context.FinishedProducts.FromSqlRaw("dbo.indexFinishedProduct").ToList();
            var RawMat = _context.RawMaterials.FromSqlRaw("dbo.indexRawMaterials").ToList();
            ViewData["Продукция"] = new SelectList(prod, "Id", "Наименование");
            ViewData["Сырьё"] = new SelectList(RawMat, "Id", "Наименование");
            return View();
        }

    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Ingredient ingr)
        {
            if (ModelState.IsValid)
            {
                SqlParameter produ = new SqlParameter("@prod", ingr.Продукция);
                SqlParameter rawmat = new SqlParameter("@rawmat", ingr.Сырьё);
               SqlParameter kol = new SqlParameter("@kol", ingr.Количество);

        await _context.Database.ExecuteSqlRawAsync("exec dbo.createIngedient @prod, @rawmat, @kol",
                    produ, rawmat, kol);

                return RedirectToAction(nameof(Index));
            }
            var prod = _context.FinishedProducts.FromSqlRaw("dbo.indexFinishedProduct").ToList();
            var RawMat = _context.RawMaterials.FromSqlRaw("dbo.indexRawMaterials").ToList();
            ViewData["Продукция"] = new SelectList(prod, "Id", "Наименование");
            ViewData["Сырьё"] = new SelectList(RawMat, "Id", "Наименование");

            return View(ingr);
        }

        // GET: Ingredients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            SqlParameter Id = new SqlParameter("@id", id);
            var ingr = await _context.Ingredients.FromSqlRaw("dbo.selectByIdIngredients @id", Id).ToListAsync();
            var dataProd = await _context.FinishedProducts.FromSqlRaw("dbo.indexFinishedProduct").ToListAsync();
            var RawMat = await _context.RawMaterials.FromSqlRaw("dbo.indexRawMaterials").ToListAsync();
            if (ingr[0] == null)
            {
                
                return NotFound();
            }
            ViewData["Продукция"] = new SelectList(dataProd, "Id", "Наименование", ingr[0].Продукция);
            ViewData["Сырьё"] = new SelectList(RawMat, "Id", "Наименование",ingr[0].Сырьё);
     //ViewData["ЕдиницаИзмерения"] = new SelectList(dataUnits, "Id", "Наименование", finish[0].ЕдиницаИзмерения);
            return View(ingr[0]);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Ingredient ingr)
        {
            if (id != ingr.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    SqlParameter Id = new SqlParameter("@id", ingr.Id);
                    SqlParameter produk = new SqlParameter("@prod", ingr.Продукция);
                    SqlParameter rawmat = new SqlParameter("@rawmat", ingr.Сырьё);
                    SqlParameter kol = new SqlParameter("@kol", ingr.Количество);

 await _context.Database.ExecuteSqlRawAsync("exec dbo.editIngredients @id,@prod, @rawmat, @kol",
                        Id, produk, rawmat, kol);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IngredientExists(ingr.Id))
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
            var prod = _context.FinishedProducts.FromSqlRaw("dbo.indexFinishedProduct").ToList();
            var RawMat = _context.RawMaterials.FromSqlRaw("dbo.indexRawMaterials").ToList();

            ViewData["Продукция"] = new SelectList(prod, "Id", "Продукция");
            ViewData["Сырьё"] = new SelectList(RawMat, "Id", "Наименование");
            return View(ingr);
        }

        // GET: Ingredients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            SqlParameter Id = new SqlParameter("@Id", id);
            var ingr = await _context.Ingredients.FromSqlRaw("dbo.selectByIdIngredients @Id", Id).ToListAsync();

            SqlParameter IdPost = new SqlParameter("@Id", ingr[0].Продукция);
            var prod = await _context.FinishedProducts.FromSqlRaw("dbo.SelectByIdFinishproducts @Id", IdPost).ToListAsync();

            SqlParameter IdRawMat = new SqlParameter("@Id", ingr[0].Сырьё);
            var rawmat = await _context.RawMaterials.FromSqlRaw("dbo.SelectByIdRawMaterials @Id", IdRawMat).ToListAsync();

            if (ingr[0] == null)
            {
                return NotFound();
            }

            return View(ingr[0]);
        }

        // POST: Ingredients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            SqlParameter Id = new SqlParameter("@Id", id);//ID
            await _context.Database.ExecuteSqlRawAsync("exec dbo.deleteIngredients @Id", Id);
            return RedirectToAction(nameof(Index));
        }

        private bool IngredientExists(int id)
        {
            return _context.Ingredients.Any(e => e.Id == id);
        }
    }
}
