using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SnacksStore.Models;

namespace SnacksStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var cookieValue = Request.Cookies["UserLogged"];
            var cookieValueName = Request.Cookies["UserLoggedName"];
            ViewBag.NameUser = cookieValueName;
            if (cookieValue == null) {
                ViewBag.IsAdmin =3;
                return View();
            }

            if (cookieValue == "1")
            {
                ViewBag.IsAdmin = 1;
            }
            else {
                ViewBag.IsAdmin = 2;
            }
            return View();
        }

        

        public IActionResult Login()
        {
            foreach (var cookie in Request.Cookies.Keys)
            {
                Response.Cookies.Delete(cookie);
            }
            return View();
        }

        public IActionResult BuyProduct(int id = 0) {
            
            if (id == 0)
            {
                ProductsStock prod = new ProductsStock();
                prod.available = true;
                return View(prod);
            }
            else
            {
                

                var ProductListQuery = (from p in _context.Products
                                        join o in _context.TypeProduct on p.TypeProduct equals o.TypeProductID
                                        join s in _context.Stock on p.ProductID equals s.ProductID
                                        where p.ProductID == id
                                        select new ProductsStock
                                        {
                                            ProductID = p.ProductID,
                                            ProductName = p.ProductName,
                                            Details = o.Details,
                                            Price = p.Price,
                                            quantity = s.Quantity,
                                            available = s.Available,
                                            Photo = p.Photo
                                        }).FirstOrDefault();


                return View(ProductListQuery);
            }
        }

        
        public IActionResult GetLogPrices(int id = 0)
        {
            //int id = Convert.ToInt32(prodID);
            var logPrices = (from l in _context.LogPrices
                             join u in _context.Users on l.UserID equals u.UserID
                             join p in _context.Products on l.ProductID equals p.ProductID
                             where l.ProductID == id //making sure have stock and availability
                             select new logUserPurchases
                             {
                                 ProductID = l.ProductID,
                                 PrevPrice = l.PrevPrice,
                                 NewPrice = l.NewPrice,
                                 User = u.UserName,
                                 ProductName = p.ProductName,
                                 Date = l.Date
                             }).ToList();

            return View(logPrices);
        }

        public IActionResult GetLogPurchases(int id = 0)
        {
            //int id = Convert.ToInt32(prodID);
            var logPurchases = (from l in _context.LogPurchases
                             join u in _context.Users on l.UserID equals u.UserID
                             join p in _context.Products on l.ProductID equals p.ProductID
                             where l.ProductID == id //making sure have stock and availability
                             select new LogPurchasesUser
                             {
                                 
                                 Quantity=l.Quantity,
                                 User = u.UserName,
                                 ProductName = p.ProductName,
                                 Date = l.Date
                             }).ToList();

            return View(logPurchases);
        }


        public IActionResult AddEditProduct(int id = 0)
        {
            ViewBag.ProductTypes = GetProductTypes();
            if (id == 0)
            {
                ProductsStock prod = new ProductsStock();
                prod.available = true;
                return View(prod);
            }
            else
            {
                

                var ProductListQuery = (from p in _context.Products
                                    join o in _context.TypeProduct on p.TypeProduct equals o.TypeProductID
                                    join s in _context.Stock on p.ProductID equals s.ProductID
                                    where p.ProductID==id
                                    select new ProductsStock
                                    {
                                        ProductID = p.ProductID,
                                        ProductName = p.ProductName,
                                        Details = o.Details,
                                        Price = p.Price,
                                        quantity=s.Quantity,
                                        available=s.Available,
                                        Photo = p.Photo
                                    }).FirstOrDefault();


                return View(ProductListQuery);
            }

        }

        public List<SelectListItem> GetProductTypes()
        {
            var query = (from tbl in _context.TypeProduct
                         select new SelectListItem()
                         {
                             Text = tbl.Details,
                             Value = tbl.TypeProductID.ToString()
                         }).ToList();

            return query;
        }


    }
}