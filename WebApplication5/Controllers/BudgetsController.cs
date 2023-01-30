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
    public class BudgetsController : Controller
    {
        private readonly modulContext _context;

        public BudgetsController(modulContext context)
        {
            _context = context;
        }

        // GET: Budgets
        public async Task<IActionResult> Index()
        {
            var dataBudjet = await _context.Budgets.FromSqlRaw("dbo.indexBudget").ToListAsync();
            return View(dataBudjet);
        }

        // GET: Budgets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            SqlParameter Id = new SqlParameter("@id", id);
            var bud = await _context.Budgets.FromSqlRaw("dbo.selectByIdBudget @id", Id).ToListAsync();
            if (bud[0] == null)
            {
                return NotFound();
            }
            return View(bud[0]);
        }

        // GET: Budgets/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Budgets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Budget budget)
        {
            if (ModelState.IsValid)
            {
                SqlParameter Summa = new SqlParameter("@Summa", budget.СуммаБюджета);
                SqlParameter Bonus = new SqlParameter("@Bonus", budget.Бонус);
                SqlParameter Pro = new SqlParameter("@Pro", budget.Процент);

                await _context.Database.ExecuteSqlRawAsync("exec dbo.createBudget @Summa, @Бонус, @Процент", Summa, Bonus, Pro);

                return RedirectToAction(nameof(Index));
            }
            return View(budget);
        }

        // GET: Budgets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            SqlParameter Id = new SqlParameter("@id", id);
            var budget = await _context.Budgets.FromSqlRaw("dbo.selectByIdBudget @id", Id).ToListAsync();
           
            if (budget[0] == null)
            {
                return NotFound();
            }
            return View(budget[0]);
        }

        // POST: Budgets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Budget budget)
        {
            if (id != budget.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    SqlParameter Id = new SqlParameter("@id", budget.Id);
                    SqlParameter SummaB = new SqlParameter("@SummaB", budget.СуммаБюджета);
                    SqlParameter Bonus = new SqlParameter("@Bonus", budget.Бонус);
                    SqlParameter Pro = new SqlParameter("@Pro", budget.Процент);


                    await _context.Database.ExecuteSqlRawAsync("exec dbo.editBudget @id,@SummaB, @Bonus,@Pro", Id, SummaB, Bonus, Pro);

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BudgetExists(budget.Id))
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
            return View(budget);
        }

        // GET: Budgets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            SqlParameter Id = new SqlParameter("@Id", id);
            var budget = await _context.Budgets.FromSqlRaw("dbo.selectByIdBudget @Id", Id).ToListAsync();
            if (budget[0] == null)
            {
                return NotFound();
            }
            return View(budget[0]);
        }

        // POST: Budgets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            SqlParameter Id = new SqlParameter("@Id", id);//ID
            await _context.Database.ExecuteSqlRawAsync("exec dbo.deleteBudget @Id", Id);
            return RedirectToAction(nameof(Index));
        }

        private bool BudgetExists(int id)
        {
            return _context.Budgets.Any(e => e.Id == id);
        }
    }
}
