using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SnacksStore.Models;

namespace SnacksStore.Controllers.Api
{

    [Produces("application/json")]
    [Route("api/login")]
    public class LoginController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LoginController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpPost]
        public IActionResult LoginUser([FromBody] Users user) {

            var userLogged = (from u in _context.Users
                          join r in _context.Roles on u.RoleID equals r.RoleID
                          where u.UserName == user.UserName && u.Password == user.Password //making sure have stock and availability
                          select u).FirstOrDefault();
            if (userLogged != null) {
                Response.Cookies.Append(
                  "UserLogged",
                  userLogged.RoleID.ToString());

                Response.Cookies.Append(
                  "UserLoggedName",
                  userLogged.UserName);

                Response.Cookies.Append(
                  "UserLoggedID",
                  userLogged.UserID.ToString());

                return Json(new { success = true, data = userLogged, message = "Product edited successfully." });
            }
            


            return Json(new { success = false, data = userLogged, message = "Incorrect User or Password." });

        }
    }
}