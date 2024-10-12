using Microsoft.EntityFrameworkCore;
using mvc_app.Models;

namespace mvc_app.Services
{
    public interface IServiceProducts
    {
        public ProductContext? _productContext {  get; set;}
        public Product? Create (Product? product);
        public IEnumerable<Product>? Read();
        public Product? GetById (int id);
        public Product? Update (int id, Product? product);
        public bool Delete(int id);
        public IEnumerable<Product>? Search(string searchString);
    }
    public class ServiceProducts : IServiceProducts
    {
        public ProductContext? _productContext { get; set; }

        public Product? Create(Product? product)
        {
            _productContext?.Products.Add(product);
            _productContext?.SaveChanges();
            return product;
        }

        public bool Delete(int id)
        {
           Product? product = _productContext?.Products
                .FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return false;
            }
            else
            {
                _productContext?.Products.Remove(product);
                _productContext?.SaveChanges(); 
                return true;
            }
        }

        public Product? GetById(int id)
        {
            return _productContext?.Products
                .FirstOrDefault(p => p.Id == id);
        }

        public IEnumerable<Product>? Read()
        {
            return _productContext?.Products.ToList();
        }

        public Product? Update(int id, Product? product)
        {
            if(id != product?.Id)
            {
                return null;
            }
            else
            {
                try
                {
                    _productContext?.Products.Update(product);
                    _productContext?.SaveChanges();
                    return product;

                }
                catch(DbUpdateConcurrencyException ex)
                {
                    Console.WriteLine(ex.StackTrace);
                    return null;
                }
            }
        }

        public IEnumerable<Product>? Search(string searchString)
        {
            if (string.IsNullOrEmpty(searchString))
            {
                return _productContext?.Products.ToList();
            }
            return _productContext?.Products
                .Where(p => p.Name.Contains(searchString))
                .ToList();
        }
    }
}
