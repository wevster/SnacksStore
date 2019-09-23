using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SnacksStore.Models;


namespace SnacksStore.Controllers.api
{
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    public class ProductsController : Controller
    {
        // GET api/products
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult GetProducts()
        {
            List<ProductsStock> ProductListQuery = new List<ProductsStock>();

            var cookieValue = Request.Cookies["UserLogged"];
            var cookieValueUserID = Request.Cookies["UserLoggedID"];


            if (cookieValue == "1")
            {
                ProductListQuery = (from p in _context.Products
                                    join o in _context.TypeProduct on p.TypeProduct equals o.TypeProductID
                                    join s in _context.Stock on p.ProductID equals s.ProductID
                                    where s.ProductID == p.ProductID //making sure have stock and availability
                                    select new ProductsStock
                                    {
                                        ProductID = p.ProductID,
                                        ProductName = p.ProductName,
                                        Details = o.Details,
                                        Price = p.Price,
                                        Photo = p.Photo,
                                        ProductLiked= _context.Likes.Where(x=>x.ProductID==p.ProductID && x.UserID==Convert.ToInt32(cookieValueUserID)).Select(x=>x.Liked).FirstOrDefault(),
                                        likes = _context.Likes
                                                    .Where(x=>x.ProductID == p.ProductID && x.Liked == true)
                                                    .Select(x=>x.LikeID).Count()
                                    }).ToList();
            }
            else {
                ProductListQuery = (from p in _context.Products
                                    join o in _context.TypeProduct on p.TypeProduct equals o.TypeProductID
                                    join s in _context.Stock on p.ProductID equals s.ProductID
                                    where s.Quantity > 0 && s.Available == true && s.ProductID == p.ProductID //making sure have stock and availability
                                    select new ProductsStock
                                    {
                                        ProductID = p.ProductID,
                                        ProductName = p.ProductName,
                                        Details = o.Details,
                                        Price = p.Price,
                                        Photo = p.Photo,
                                        ProductLiked = _context.Likes.Where(x => x.ProductID == p.ProductID && x.UserID == Convert.ToInt32(cookieValueUserID)).Select(x => x.Liked).FirstOrDefault(),
                                        likes = _context.Likes
                                                    .Where(x => x.ProductID == p.ProductID && x.Liked==true)
                                                    .Select(x => x.LikeID).Count()
                                    }).ToList();
            }

            
            

            return Json(new { data = ProductListQuery });
            //return ProductListQuery;
        }

        

        [HttpPost]
        public async Task<IActionResult> BuyProducts([FromBody] ProductsStock prod)
        {
            if (prod.quantity<1)
            {
                return Json(new { success = false, message = "Please, type the amount of products to buy" });
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            
                

                var CurrProductStock = _context.Stock.Where(x => x.ProductID == prod.ProductID).FirstOrDefault();


            int quantity=0, InStock=0;

            InStock = CurrProductStock.Quantity;
            quantity = prod.quantity;
            if( quantity>InStock) {
                return Json(new { success = false, message = "There is not enough products in stock to this quantity" });
            }

            quantity = InStock - quantity;
                CurrProductStock.Quantity = quantity;
                


                _context.Stock.Update(CurrProductStock);

                await _context.SaveChangesAsync();

            // Purchase log
            var cookieValueUserID = Request.Cookies["UserLoggedID"];
            LogPurchases ProductsPurchased = new LogPurchases();
            ProductsPurchased.ProductID = prod.ProductID;
            ProductsPurchased.Quantity = prod.quantity;
            ProductsPurchased.UserID = Convert.ToInt32(cookieValueUserID);
            ProductsPurchased.Date = DateTime.Now;
            _context.LogPurchases.Add(ProductsPurchased);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "You buy "+ prod.quantity + " of " + prod.ProductName });
            
        }
        // POST api/products
        [HttpPost]
        public async Task<IActionResult> PostProduct([FromBody] ProductsStock prod)
        {
                if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (prod.ProductID == 0)
            {

                Products NewProduct = new Products();
                NewProduct.ProductName = prod.ProductName;
                NewProduct.Price = prod.Price;
                NewProduct.TypeProduct = prod.ProductType;
                NewProduct.Photo = prod.Photo;

                _context.Products.Add(NewProduct);
                await _context.SaveChangesAsync();

                Stock NewProductToStore = new Stock();
                NewProductToStore.Available = prod.available;
                NewProductToStore.Quantity = prod.quantity;
                NewProductToStore.ProductID = NewProduct.ProductID;

                
                _context.Stock.Add(NewProductToStore);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Product added successfully." });
            }
            else
            {
                var ProductToEdit =  _context.Products.Where(x => x.ProductID == prod.ProductID).FirstOrDefault();
                ProductToEdit.ProductID = prod.ProductID;
                ProductToEdit.ProductName = prod.ProductName;
                if (prod.Price != ProductToEdit.Price) {
                    // Price changing log
                    var cookieValueUserID = Request.Cookies["UserLoggedID"];
                    LogPrices PriceChanged = new LogPrices();
                    PriceChanged.PrevPrice = ProductToEdit.Price;
                    PriceChanged.NewPrice = prod.Price;
                    PriceChanged.ProductID = prod.ProductID;
                    PriceChanged.UserID = Convert.ToInt32(cookieValueUserID);
                    PriceChanged.Date = DateTime.Now;
                    _context.LogPrices.Update(PriceChanged);
                    await _context.SaveChangesAsync();
                }
                ProductToEdit.Price = prod.Price;
                ProductToEdit.TypeProduct = prod.ProductType;
                ProductToEdit.Photo = prod.Photo;

                _context.Products.Update(ProductToEdit);

                await _context.SaveChangesAsync();

                var CurrProductStock = _context.Stock.Where(x => x.ProductID == prod.ProductID).FirstOrDefault();
                
                CurrProductStock.Available = prod.available;
                CurrProductStock.Quantity = prod.quantity;
                CurrProductStock.ProductID = prod.ProductID;

                                
                _context.Stock.Update(CurrProductStock);

                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Product edited successfully." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> LikeProduct([FromBody] string prodID)
        {
            int Prod = Convert.ToInt32(prodID);
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var cookieValue = Request.Cookies["UserLoggedID"];


                Likes like = _context.Likes.Where(x => x.ProductID == Prod && x.UserID == Convert.ToInt32(cookieValue)).FirstOrDefault();
                if (like != null)
                {
                    if (like.Liked)
                    {
                        like.Liked = false;
                    }
                    else
                    {
                        like.Liked = true;
                    }
                    await _context.SaveChangesAsync();

                }
                else
                {
                    Likes likeToSave = new Likes();
                    likeToSave.Liked = true;
                    likeToSave.ProductID = Prod;
                    likeToSave.UserID = Convert.ToInt32(cookieValue);
                    _context.Add(likeToSave);
                    await _context.SaveChangesAsync();

                }
                return Json(new { success = true, message = "Product liked successfully." });
            }
            catch {
                return Json(new { success = false, message = "There is a problem trying to like the product" });
            }
            
        }

        // PUT api/products/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/products/5
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var Product = await _context.Products.SingleOrDefaultAsync(m => m.ProductID == id);
            if (Product == null)
            {
                return NotFound();
            }

            var Stock= await _context.Stock.SingleOrDefaultAsync(m => m.ProductID == id);

            _context.Products.Remove(Product);
            _context.Stock.Remove(Stock);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Delete success." });
        }
    }
}
