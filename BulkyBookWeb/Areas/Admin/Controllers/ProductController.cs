using BulkyBook.Model;
using BulkyBook.DataAccess.Data;
using Microsoft.AspNetCore.Mvc;
using BulkyBook.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.Rendering;
using BulkyBook.Model.ViewModels;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _uniteOfWork;
        private readonly IWebHostEnvironment _hostEnviroment;

        public ProductController(IUnitOfWork uniteOfWork, IWebHostEnvironment hostEnviroment)
        {
            _uniteOfWork = uniteOfWork;
            _hostEnviroment = hostEnviroment;
        }
        public IActionResult Index()
        {
           return View();
        }

        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new()
            {
                
                Product = new(),
                CategoryList = _uniteOfWork.Category.GetAll().Select
                (u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                CoverTypeList = _uniteOfWork.CoverType.GetAll().Select
                (u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                })

            };
            
            

            if (id == null || id == 0)
            {
                //Create
                ViewBag.Title = "Create a Product";
                return View(productVM);
            }
            else
            {
                ViewBag.Title = "Update the Product";
                productVM.Product = _uniteOfWork.Product.GetFirstorDefault(u => u.Id == id);
                //update product
                return View(productVM);

            }
        }

        [HttpPost]
        public IActionResult Upsert(ProductVM obj, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _hostEnviroment.WebRootPath;
                if (file != null) 
                { 
                 string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"images\products");
                    var extension = Path.GetExtension(file.FileName);

                    if (obj.Product.ImageUrl!= null)
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, obj.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);    
                        }
                    }
                    using (var filesSteams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        file.CopyTo(filesSteams);
                    }

                    obj.Product.ImageUrl = @"\images\products\" + fileName + extension;
                }

                if (obj.Product.Id == 0)
                {
                    _uniteOfWork.Product.Add(obj.Product);
                }
                else
                {
                    _uniteOfWork.Product.Update(obj.Product);
                }
                    _uniteOfWork.Save();
                    TempData["success"] = "Product addess successfully";
                    return RedirectToAction("Index");
               
            }
          
                return View(obj);
         
        }

       

    
        

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var productlist = _uniteOfWork.Product.GetAll(includeProperties:"Category,CoverType");
            return Json(new {data=productlist});
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var obj = _uniteOfWork.Product.GetFirstorDefault(u => u.Id == id);
            if (obj == null)
            {
                return Json(new { success = false, message = "Error While Deleting" });
            }
            
            var oldImagePath = Path.Combine( _hostEnviroment.WebRootPath, obj.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }
            _uniteOfWork.Product.Remove(obj);
            _uniteOfWork.Save();
            return Json(new { success = true, message = "Deleting Succesfull" });
        }
        #endregion

    }

}
