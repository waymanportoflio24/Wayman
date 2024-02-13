using BooksWeb.Data;
using BooksWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Identity.Client;

namespace BooksWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            List<Category> objCategoryList = _db.Categories.ToList();
            return View(objCategoryList);
        }

        public IActionResult Create()
        {
            return View();
        }
        // Dodawanie Kategorii 
        [HttpPost]
        public IActionResult Create(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("Name", "The DisplayOrder cannot exactly match the Name.");
            }

            bool isDuplicateName = _db.Categories.Any(c => c.Name == obj.Name);

            if (isDuplicateName)
            {
                ModelState.AddModelError("Name", "A category with the same name already exists.");
            }

            if (obj.DisplayOrder < 1 || obj.DisplayOrder > 99)
            {
                ModelState.AddModelError("DisplayOrder", "DisplayOrder must be in the range of 1 to 99.");
            }

            bool isDuplicateDisplayOrder = _db.Categories.Any(c => c.DisplayOrder == obj.DisplayOrder);

            if (isDuplicateDisplayOrder)
            {
                ModelState.AddModelError("DisplayOrder", "A category with the same DisplayOrder already exists.");
            }

            if (ModelState.IsValid)
            {
                _db.Categories.Add(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View();
        }

        public IActionResult Edit(int id)
        {
            return View();
        }

        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("Name", "The DisplayOrder cannot exactly match the Name.");
                return View(obj);
            }

            bool isDuplicateName = _db.Categories.Any(c => c.Name == obj.Name && c.Id != obj.Id);

            if (isDuplicateName)
            {
                ModelState.AddModelError("Name", "A category with the same name already exists.");
                return View(obj);
            }

            if (obj.DisplayOrder < 1 || obj.DisplayOrder > 99)
            {
                ModelState.AddModelError("DisplayOrder", "DisplayOrder must be in the range of 1 to 99.");
                return View(obj);
            }

            bool isDuplicateDisplayOrder = _db.Categories.Any(c => c.DisplayOrder == obj.DisplayOrder && c.Id != obj.Id);

            if (isDuplicateDisplayOrder)
            {
                ModelState.AddModelError("DisplayOrder", "A category with the same DisplayOrder already exists.");
                return View(obj);
            }

            if (ModelState.IsValid)
            {
                // Aktualizuj istniejący rekord zamiast dodawania nowego
                Category existingCategory = _db.Categories.Find(obj.Id);

                if (existingCategory == null)
                {
                    return NotFound();
                }

                existingCategory.Name = obj.Name;
                existingCategory.DisplayOrder = obj.DisplayOrder;

                _db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(obj);
        }
        public IActionResult Delete(int Id)
        {
            return View();
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? Id)
        {

            Category obj = _db.Categories.Find(Id);

            if (obj == null)
            {
                return NotFound();
            }
            _db.Categories.Remove(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");

        }
    }
}
