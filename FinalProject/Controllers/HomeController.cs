using FinalProject.CustomPolicyProvider;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FinalProject.Controllers
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
            return View();
        }

        [Authorize(Policy ="Claim.DoB")] 
        public IActionResult SecretPolicy()
        {
            return View("Secret");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult SecretRole()
        {
            return View("Secret");
        }

        [SecurityLevel(5)]
        public IActionResult SecretLevel()
        {
            return View("Secret");
        }

        [SecurityLevel(10)]
        public IActionResult SecretHigherLevel()
        {
            return View("Secret");
        }

        [AllowAnonymous]
        public IActionResult Authenticate()
        {
            var studentClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, "Dinesh"),
                new Claim(ClaimTypes.Email, "Dinesh@Panjeta.com"),
                new Claim(ClaimTypes.DateOfBirth, "07/07/1996"),
                new Claim(ClaimTypes.Role, "Admin"),
                new Claim(DynamicPolicies.SecurityLevel, "7"),
                new Claim("Student.ID", "200469114"),
            };

            var licenseClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, "Dinesh Panjeta"),
                new Claim("Driving.License", "p89f7wehjdsuytsadg"),
            };

            var studentIdentity = new ClaimsIdentity(studentClaims, "Student Identity");
            var licenseIdentity = new ClaimsIdentity(licenseClaims, "License Identity");


            var userPrincipal = new ClaimsPrincipal(new[] { studentIdentity, licenseIdentity });

            //---------------------------------------------------------------------------------
            HttpContext.SignInAsync(userPrincipal);

            return RedirectToAction("Index"); 
        }

        public async Task<IActionResult> DoStuff(
            [FromServices] IAuthorizationService authorizationService)
        {


        //we are doing stuff here

        var builder = new AuthorizationPolicyBuilder("Schema");
            var customPolicy = builder.RequireClaim("Hello").Build();
            
            var authResult = await authorizationService.AuthorizeAsync(HttpContext.User, customPolicy);

            if(authResult.Succeeded)
            {
                return View("Index"); 
            }

            return View("Index");
        }

    }
}
