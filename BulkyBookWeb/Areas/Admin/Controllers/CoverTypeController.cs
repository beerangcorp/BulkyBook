using BulkyBook.Model;
using BulkyBook.DataAccess.Data;
using Microsoft.AspNetCore.Mvc;
using BulkyBook.DataAccess.Repository.IRepository;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    public class CoverTypeController : Controller
    {
        private readonly IUnitOfWork _uniteOfWork;

        public CoverTypeController(IUnitOfWork uniteOfWork)
        {
            _uniteOfWork = uniteOfWork;

        }
        public IActionResult Index()
        {
            IEnumerable<CoverType> objCategoriesList = _uniteOfWork.CoverType.GetAll();
            return View(objCategoriesList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public IActionResult Create(CoverType obj)
        {
            if (ModelState.IsValid)
            {
                _uniteOfWork.CoverType.Add(obj);
                _uniteOfWork.Save();
                TempData["success"] = "CoverType has been created Successfully ";
                return RedirectToAction("Index");
            }
            else
            {
                return View(obj);
            }
        }

        public IActionResult Edit(int? id)
        {
            var CategotyFromDbFirst = _uniteOfWork.CoverType.GetFirstorDefault(u => u.Id == id);
            return View(CategotyFromDbFirst);
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public IActionResult Edit(CoverType obj)
        {
            if (ModelState.IsValid)
            {
                _uniteOfWork.CoverType.Update(obj);
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
            var CategotyFromDbFirst = _uniteOfWork.CoverType.GetFirstorDefault(u => u.Id == id);
            return View(CategotyFromDbFirst);
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var CategotyFromDbFirst = _uniteOfWork.CoverType.GetFirstorDefault(u => u.Id == id);
            _uniteOfWork.CoverType.Remove(CategotyFromDbFirst);
            _uniteOfWork.Save();
            return RedirectToAction("Index");
        }

    }

}
