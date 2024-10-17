using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mvc_app.Models;
using mvc_app.Services;

namespace mvc_app.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IServiceProducts _serviceProducts;
        private readonly ProductContext _productContext;
        public ProductsController(IServiceProducts serviceProducts, ProductContext productContext)
        {
            _serviceProducts = serviceProducts;
            _productContext = productContext;
        }
        // GET: http://localhost:[port]/products
        public async Task<ViewResult> Index(string searchString)
        {
            var products = _serviceProducts.Search(searchString);
            ViewData["SearchString"] = searchString;
            return View(products);
        }
        //GET: http://localhost:[port]/products/{id}
        public async Task<ViewResult> Details(int id)
        {
            var products = await _serviceProducts.GetByIdAsync(id);
            return View(products);
        }
      
        //GET: http://localhost:[port]/products/create
        public async Task<ViewResult> Create()
        {
            return View();
        }
        //POST: http://localhost:[port]/products/create
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Price,Description")] Product product)
        {
            if (ModelState.IsValid)
            {
                _= await _serviceProducts.CreateAsync(product);
                return RedirectToAction(nameof(Index));
            }
               return View(product);
        }
        public ViewResult Update(int? id) => View();
        //POST: http://localhost:[port]/products/update/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, [Bind("Id,Name,Price,Description")] Product product)
        {
            if (ModelState.IsValid)
            {
                _ = await _serviceProducts.UpdateAsync(id,product);
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }
        public ViewResult Delete(int? id)
        {
            var product = _productContext?.Products
                .FirstOrDefault(x => x.Id == id);
            return View(product);
        }
        //POST: http://localhost:[port]/products/delete/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            _= await _serviceProducts.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
