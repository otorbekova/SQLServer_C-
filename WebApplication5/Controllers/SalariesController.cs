using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication5.Models;

namespace WebApplication5.Controllers
{
    public class SalariesController : Controller
    {
        private readonly modulContext _context;

        public SalariesController(modulContext context)
        {
            _context = context;
        }

        // GET: Salaries
        public async Task<IActionResult> Index()
        {
            var modulContext = _context.Salaries.Include(s => s.Сотрудник_);
            return View(await modulContext.ToListAsync());
        }

        // GET: Salaries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salary = await _context.Salaries
                .Include(s => s.Сотрудник_)
                .FirstOrDefaultAsync(m => m.IdЗарплата == id);
            if (salary == null)
            {
                return NotFound();
            }

            return View(salary);
        }

        // GET: Salaries/Create
        public IActionResult Create()
        {
            ViewData["Сотрудник"] = new SelectList(_context.Employees, "Id", "Id");
            return View();
        }

        // POST: Salaries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdЗарплата,Год,Месяц,Сотрудник,Зарплата,Производство,Закупка,Продажа,ОбщееКоличество,Оклад,Бонус")] Salary salary)
        {
            if (ModelState.IsValid)
            {
                _context.Add(salary);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Сотрудник"] = new SelectList(_context.Employees, "Id", "Id", salary.Сотрудник);
            return View(salary);
        }

        // GET: Salaries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salary = await _context.Salaries.FindAsync(id);
            if (salary == null)
            {
                return NotFound();
            }
            ViewData["Сотрудник"] = new SelectList(_context.Employees, "Id", "Id", salary.Сотрудник);
            return View(salary);
        }

        // POST: Salaries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdЗарплата,Год,Месяц,Сотрудник,Зарплата,Производство,Закупка,Продажа,ОбщееКоличество,Оклад,Бонус")] Salary salary)
        {
            if (id != salary.IdЗарплата)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(salary);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SalaryExists(salary.IdЗарплата))
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
            ViewData["Сотрудник"] = new SelectList(_context.Employees, "Id", "Id", salary.Сотрудник);
            return View(salary);
        }

        // GET: Salaries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salary = await _context.Salaries
                .Include(s => s.Сотрудник_)
                .FirstOrDefaultAsync(m => m.IdЗарплата == id);
            if (salary == null)
            {
                return NotFound();
            }

            return View(salary);
        }

        // POST: Salaries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var salary = await _context.Salaries.FindAsync(id);
            _context.Salaries.Remove(salary);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SalaryExists(int id)
        {
            return _context.Salaries.Any(e => e.IdЗарплата == id);
        }
    }
}
