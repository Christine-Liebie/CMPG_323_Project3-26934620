using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using DeviceManagement_WebApp.Data;
using DeviceManagement_WebApp.Models;
using DeviceManagement_WebApp.Repository;

namespace DeviceManagement_WebApp.Controllers
{
    [Authorize]
    public class CategoriesController : Controller
    {
        private readonly ICategoryRepository _categoriesRepository;

        public CategoriesController(ICategoryRepository categoryRepository)
        {
            _categoriesRepository = categoryRepository;
        }

        // GET: retrieve all the Categories records from DB
        public async Task<IActionResult> Index()
        {
            return View(_categoriesRepository.GetAll());
        }

        // GET: receive Categories's Details from a record
        public async Task<IActionResult> Details(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = _categoriesRepository.GetById(id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        //Add new record to DB
        // GET part of creating Categories
        public IActionResult Create()
        {
            return View();
        }

        //Add new record to DB
        // POST part of creating Categories
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryId,CategoryName,CategoryDescription,DateCreated")] Category category)
        {
            category.CategoryId = Guid.NewGuid();
            _categoriesRepository.Add(category);
            _categoriesRepository.Save();
            return RedirectToAction(nameof(Index));
        }

        // GET part of editing Categories
        public async Task<IActionResult> Edit(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = _categoriesRepository.GetById(id);

            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST part of editing Categories
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("CategoryId,CategoryName,CategoryDescription,DateCreated")] Category category)
        {
            if (id != category.CategoryId)
            {
                return NotFound();
            }
            try
            {
                _categoriesRepository.Update(category);
                _categoriesRepository.Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(category.CategoryId))
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

        // GET part of deleting Categories
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = _categoriesRepository.GetById(id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST part of deleting Categories
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var category = _categoriesRepository.GetById(id);
            _categoriesRepository.Remove(category);
            _categoriesRepository.Save();
            return RedirectToAction(nameof(Index));
        }

        //Check if Category record exists
        private bool CategoryExists(Guid id)
        {
            Category category = _categoriesRepository.GetById(id);

            if (category != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
