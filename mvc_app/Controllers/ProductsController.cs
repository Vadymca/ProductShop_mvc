using Microsoft.AspNetCore.Mvc;
using mvc_app.Models;
using mvc_app.Services;

namespace mvc_app.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IServiceProducts? _serviceProducts;
        private readonly ProductContext? _productContext;
        public ProductsController(IServiceProducts? serviceProducts, ProductContext? productContext)
        {
            _productContext = productContext;
            _serviceProducts = serviceProducts;
            _serviceProducts._productContext = productContext;
        }
        // GET: http://localhost:[port]/products
        public ViewResult Index(string searchString)
        {
            var products = _serviceProducts?.Search(searchString);
            ViewData["SearchString"] = searchString;
            return View(products);
        }
        //GET: http://localhost:[port]/products/{id}
        public ViewResult Details(int id) => View(_serviceProducts?.GetById(id));
        //GET: http://localhost:[port]/products/create
        public ViewResult Create() => View();
        //POST: http://localhost:[port]/products/create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Name,Price,Description")] Product product)
        {
            if (ModelState.IsValid)
            {
                _= _serviceProducts?.Create(product);
                return RedirectToAction(nameof(Index));
            }
               return View(product);
        }
        public ViewResult Update(int? id) => View();
        //POST: http://localhost:[port]/products/update/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(int id, [Bind("Id,Name,Price,Description")] Product product)
        {
            if (ModelState.IsValid)
            {
                _ = _serviceProducts?.Update(id,product);
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }
        public ViewResult Delete(int? id)
        {
            Product? product = _productContext?.Products
                .FirstOrDefault(x => x.Id == id);
            return View(product);
        }
        //POST: http://localhost:[port]/products/delete/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            _=_serviceProducts?.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
