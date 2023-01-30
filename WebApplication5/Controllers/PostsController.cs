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
    public class PostsController : Controller
    {
        private readonly modulContext _context;

        public PostsController(modulContext context)
        {
            _context = context;
        }

        // GET: Posts
        public async Task<IActionResult> Index()
        {
            var dataPost = await _context.Posts.FromSqlRaw("dbo.indexPost").ToListAsync();
        
            return View(dataPost);
        }

        // GET: Posts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // GET: Posts/Create
        public IActionResult Create()
        {
            //var dataPost = _context.Posts.FromSqlRaw("dbo.indexPost").ToListAsync();
            ViewData["Должность"] = new SelectList(_context.Posts, "Id", "Должность");
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Post post)
        {
            if (ModelState.IsValid)
            {
                SqlParameter dol = new SqlParameter("@dol", post.Должность);

                await _context.Database.ExecuteSqlRawAsync("exec dbo.createPost @dol", dol);

                return RedirectToAction(nameof(Index));
            }
            return View(post);
        }

        // GET: Posts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            SqlParameter Id = new SqlParameter("@id", id);
            var post = await _context.Posts.FromSqlRaw("dbo.SelectByIdPost @id", Id).ToListAsync();

            if (post[0] == null)
            {
                return NotFound();
            }
            return View(post[0]);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Post post)
        {
            if (id != post.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    SqlParameter Id = new SqlParameter("@id", post.Id);
                    SqlParameter pos = new SqlParameter("@SummaB", post.Должность);
                    await _context.Database.ExecuteSqlRawAsync("exec dbo.editPost @id,@SummaB", Id, pos);
       }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(post.Id))
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
            return View(post);
        }

        // GET: Posts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            SqlParameter Id = new SqlParameter("@Id", id);
            var budget = await _context.Posts.FromSqlRaw("dbo.SelectByIdPost @Id", Id).ToListAsync();
            if (budget[0] == null)
            {
                return NotFound();
            }
            return View(budget[0]);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            SqlParameter Id = new SqlParameter("@Id", id);//ID
            await _context.Database.ExecuteSqlRawAsync("exec dbo.deletePost @Id", Id);
            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(int id)
        {
            return _context.Posts.Any(e => e.Id == id);
        }
    }
}
