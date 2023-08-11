using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MonitorApi.Data;
using MonitorApi.Models.DataBase;
using MonitorApi.Models.Reponses;
using MonitorApi.Models.Request;
using MonitorApi.Models.Setting;
using MonitorApi.Services;
using Microsoft.AspNetCore.Http;
using System.Net;
using Microsoft.Net.Http.Headers;

namespace MonitorApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        //private readonly SignInManager<IdentityUser> _signManager;
        private readonly UsersDbContext _context;
        private readonly TokenService _tokenService; 
        //private readonly IHttpContextAccessor _httpContext;

        public AuthenticationController(  UserManager<IdentityUser> userManager,  UsersDbContext context, TokenService tokenService )
        {
            _userManager = userManager;
            _context = context;
            _tokenService = tokenService; 
            //_signManager = signManager;
            //_httpContext = httpContextAccessor;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(RegistrationRequest request )
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userManager.CreateAsync(
                new IdentityUser { UserName = request.UserName, Email = request.Email },
                request.Password);
            if (result.Succeeded)
            {
                request.Password = "";
                return CreatedAtAction(nameof(Register),new { email = request.Email }, request);
            }

            foreach(var error in result.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }
            return BadRequest(ModelState);
        }

        [HttpPost]
        [Route("login")]
        [IgnoreAntiforgeryToken]
        public async Task<ActionResult<AuthResponse>> Authenticate([FromBody] AuthRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //if (_signManager.IsSignedIn(User))
            //{
            //    Console.WriteLine("zzz");
            //}



             var managedUser = await _userManager.FindByEmailAsync(request.Email);
            //if (managedUser == null)
            //{
            //    return BadRequest("Bad credentials");
            //}
            var isPasswordValid = await _userManager.CheckPasswordAsync(managedUser, request.Password);
            //var result = await _signManager.PasswordSignInAsync(managedUser.UserName, request.Password, true, lockoutOnFailure:false);
            if(!isPasswordValid)
            {
                return BadRequest("Bad credentials");
            }
            var userInDb= await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (userInDb is null)
            {
                return Unauthorized();
            }
            var accessToken= _tokenService.CreateToken(userInDb);
            await _context.SaveChangesAsync();

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Domain = "localhost",
                Path = "/",
               // Expires = DateTime.Now.AddDays(2)  
            };

          //  Response.Cookies.Append("Token", "dale que dale", cookieOptions);
           Response.Cookies.Append("Token", "dale que dale" ); 
            return Ok(new AuthResponse
            {
                Username = userInDb.UserName,
                Email = userInDb.Email,
                Token = accessToken,
            });

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("prueba")]
        public async Task<IActionResult> prueba()
        {
           Request.Cookies.TryGetValue("Token", out var refreshToken);

            var refreshToke = Request.Cookies["Token"];
          


            return Ok();
        }

        [HttpGet]
       // [ValidateAntiForgeryToken]
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {


            return Ok();
        }

        //[HttpPost]
        //[Route("logout")]
        //public async Task<IActionResult> Logout()
        //{
        //    await _signManager.SignOutAsync();
        //    return Ok();
        //}


        //[HttpPost]
        //[AllowAnonymous]
        //public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        //{
        //    JwtSecurityToken token;
        //    DateTime expiration;
        //    //Tu validación de usuario password
        //    var llave = Encoding.UTF8.GetBytes(Config["Tokens:Key"]);
        //    var key = new SymmetricSecurityKey(llave);
        //    var creds = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);
        //    token = new JwtSecurityToken(Config["Tokens:Issuer"],
        //                                    Config["Tokens:Issuer"],
        //                                    claims,
        //                                    expires: DateTime.Now.AddDays(30),
        //                                    signingCredentials: creds);
        //    var claims = new Claim[] {
        //      new Claim(ClaimTypes.Sid, usuario.Id.ToString()),
        //              new Claim(ClaimTypes.Role, usuario.Rol),
        //      new Claim("Empresa",usuario.Empresa.ToString())
        //          };
        //    tokenHandler = new JwtSecurityTokenHandler().WriteToken(token);
        //    expiration = token.ValidTo;
        //}

    }
}
