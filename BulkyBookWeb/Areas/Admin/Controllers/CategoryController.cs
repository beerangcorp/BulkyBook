using BulkyBook.Model;
using BulkyBook.DataAccess.Data;
using Microsoft.AspNetCore.Mvc;
using BulkyBook.DataAccess.Repository.IRepository;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _uniteOfWork;

        public CategoryController(IUnitOfWork uniteOfWork)
        {
            _uniteOfWork = uniteOfWork;

        }
        public IActionResult Index()
        {
            IEnumerable<Category> objCategoriesList = _uniteOfWork.Category.GetAll();
            return View(objCategoriesList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public IActionResult Create(Category obj)
        {
            if (ModelState.IsValid)
            {
                _uniteOfWork.Category.Add(obj);
                _uniteOfWork.Save();
                TempData["success"] = "Category has been created Successfully ";
                return RedirectToAction("Index");
            }
            else
            {
                return View(obj);
            }
        }

        public IActionResult Edit(int? id)
        {
            var CategotyFromDbFirst = _uniteOfWork.Category.GetFirstorDefault(u => u.Id == id);
            return View(CategotyFromDbFirst);
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public IActionResult Edit(Category obj)
        {
            if (ModelState.IsValid)
            {
                _uniteOfWork.Category.Update(obj);
                _uniteOfWork.Save();
                return RedirectToAction("Index");
            }
            else
            {
                return View(obj);
            }
        }

        public IActionResult Delete(int? id)
        {
            var CategotyFromDbFirst = _uniteOfWork.Category.GetFirstorDefault(u => u.Id == id);
            return View(CategotyFromDbFirst);
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var CategotyFromDbFirst = _uniteOfWork.Category.GetFirstorDefault(u => u.Id == id);
            _uniteOfWork.Category.Remove(CategotyFromDbFirst);
            _uniteOfWork.Save();
            return RedirectToAction("Index");
        }

    }

}
