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
    public class EmployeesController : Controller
    {
        private readonly modulContext _context;

        public EmployeesController(modulContext context)
        {
            _context = context;
        }

        // GET: Employees
        public async Task<IActionResult> Index()
        {
            var dataEmploye = await _context.Employees.FromSqlRaw("dbo.indexEmployees").ToListAsync();
            var dataPost = await _context.Posts.FromSqlRaw("dbo.indexPost").ToListAsync();
            foreach (var e in dataEmploye)
            {
                foreach (var p in dataPost)
                {
                    if (e.Должность == p.Id)
                    {
                        e.Должность_.Должность = p.Должность;
                    }
                }
            }
            return View(dataEmploye);
        }

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }
            SqlParameter Id = new SqlParameter("@id", id);
            var empl = await _context.Employees.FromSqlRaw("dbo.SelectByIdEmployees @id", Id).ToListAsync();
            var dataPost = await _context.Posts.FromSqlRaw("dbo.indexPost").ToListAsync();
            if (empl[0] == null)
            {
                return NotFound();
            }
            ViewData["Должность"] = new SelectList(dataPost, "Id", "Должность", empl[0].Должность);
            return View(empl[0]);
        }

        // GET: Employees/Create
        public IActionResult Create()
        {
            var post = _context.Posts.FromSqlRaw("dbo.indexPost").ToList();
            ViewData["Должность"] = new SelectList(post, "Id", "Должность");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                SqlParameter Фио = new SqlParameter("@FIO", employee.Фио);
                SqlParameter Должность = new SqlParameter("@Dol", employee.Должность);
                SqlParameter Оклад = new SqlParameter("@oklad", employee.Оклад);
                SqlParameter Адрес = new SqlParameter("@adres", employee.Адрес);
                SqlParameter Телефон = new SqlParameter("@telefon", employee.Телефон);

                await _context.Database.ExecuteSqlRawAsync("exec dbo.createEmployees @FIO, @Dol, @oklad, @adres, @telefon", Фио, Должность, Оклад, Адрес, Телефон);

                return RedirectToAction(nameof(Index));
            }
            var post = _context.Posts.FromSqlRaw("dbo.indexPost").ToList();
           ViewData["Должность"] = new SelectList(post, "Id", "Фио", employee.Должность);
            return View(employee);
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            SqlParameter Id = new SqlParameter("@id", id);
            var employee = await _context.Employees.FromSqlRaw("dbo.SelectByIdEmployees @id", Id).ToListAsync();
            var dataPost = await _context.Posts.FromSqlRaw("dbo.indexPost").ToListAsync();
            //var employee = await _context.Employees.FindAsync(id);
            if (employee[0] == null)
            {
                return NotFound();
            }
            ViewData["Должность"] = new SelectList(dataPost, "Id", "Должность", employee[0].Должность);
            return View(employee[0]);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Employee employee)
        {
            if (id != employee.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    SqlParameter Id = new SqlParameter("@id", employee.Id);
                    SqlParameter Фио = new SqlParameter("@FIO", employee.Фио);
                    SqlParameter Должность = new SqlParameter("@Dol", employee.Должность);
                    SqlParameter Оклад = new SqlParameter("@oklad", employee.Оклад);
                    SqlParameter Адрес = new SqlParameter("@adres", employee.Адрес);
                    SqlParameter Телефон = new SqlParameter("@telefon", employee.Телефон);

                    await _context.Database.ExecuteSqlRawAsync("exec dbo.editEmployees @id,@FIO, @Dol, @oklad, @adres, @telefon",Id, Фио, Должность, Оклад, Адрес, Телефон);

                    //_context.Update(employee);
                    //await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.Id))
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
            var dataPost = await _context.Posts.FromSqlRaw("dbo.indexPost").ToListAsync();
        //    ViewData["Должность"] = new SelectList(dataPost, "Id", "Должность", employee.Должность);
            ViewData["Должность"] = new SelectList(_context.Posts, "Id", "Должность", employee.Должность);
            return View(employee);
        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            SqlParameter Id = new SqlParameter("@Id", id);
            var employee = await _context.Employees.FromSqlRaw("dbo.SelectByIdEmployees @Id", Id).ToListAsync();

            SqlParameter IdPost = new SqlParameter("@Id", employee[0].Должность);
            var post = await _context.Posts.FromSqlRaw("dbo.SelectByIdPost @Id", IdPost).ToListAsync();
            if (employee[0] == null)
            {
                return NotFound();
            }

            return View(employee[0]);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            SqlParameter Id = new SqlParameter("@Id", id);//ID
            await _context.Database.ExecuteSqlRawAsync("exec dbo.deleteEmployees @Id", Id);
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }
    }
}
