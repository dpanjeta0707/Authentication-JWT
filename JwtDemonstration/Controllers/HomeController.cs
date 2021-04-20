using JwtDemonstration.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace JwtDemonstration.Controllers
{
    public class HomeController : Controller
    {
        
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Secret()
        {
            var webClient = new WebClient();
            var json = webClient.DownloadString(@"D:\Encoding\Country.json");
            var countries = JsonConvert.DeserializeObject<Countries>(json);
            return View(countries);
        }


        public IActionResult LoginUser(Users user)
        {
            verifierClass _verifierToken = new verifierClass();
            var userToken = _verifierToken.LoginUser(user.USERID.Trim(), user.PASSWORD.Trim());
            if (userToken != null)
            {
                return Ok(new { access_token = userToken });
            }
            else
            {
                return View("Index");
            }
        }

    }
}
