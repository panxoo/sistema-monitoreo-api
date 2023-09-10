using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MonitorApi.Data;
using MonitorApi.Models.DataBase;
using MonitorApi.Models.Reponses;
using MonitorApi.Models.Request;
using MonitorApi.Services;

namespace MonitorApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        //private readonly SignInManager<IdentityUser> _signManager;
        private readonly UsersDbContext _context;
        private readonly TokenService _tokenService;
        //private readonly IHttpContextAccessor _httpContext;

        public AuthenticationController(UserManager<User> userManager, UsersDbContext context, TokenService tokenService, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _context = context;
            _tokenService = tokenService;
            _roleManager = roleManager;
            //_signManager = signManager;
            //_httpContext = httpContextAccessor;
        }



        [HttpPost]
        [Route("addrol")]
        public async Task<IActionResult> AddRol(string rol)
        {
            var res = await _roleManager.CreateAsync(new IdentityRole(rol));

            if (res.Succeeded)
                return Ok();
            return BadRequest(res);
        }

        [HttpGet]
        [Authorize(Roles = "admins")]
        [Route("test")]
        public async Task<IActionResult> test()
        {
            return Ok();
        }


        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(RegistrationRequest request)
        {

            foreach (string item in request.Rols)
                if (!await _roleManager.RoleExistsAsync(item))
                    return BadRequest(new { Code = "Erreur du rol", Description = "Le rol n'existe pas" });


            User usr = new User
            {
                Email = request.Email,
                UserName = request.UserName
            };

            var res = await _userManager.CreateAsync(usr, request.Password);

            if (!res.Succeeded)
                return BadRequest(res);

            var resrol = await _userManager.AddToRolesAsync(usr, request.Rols);

            if (!resrol.Succeeded)
                return BadRequest(resrol);

            return Ok();
        }

        [HttpPost]
        [Route("login")]
        //[IgnoreAntiforgeryToken]
        public async Task<ActionResult<AuthResponse>> Authenticate([FromBody] AuthRequest request)
        {

            var managedUser = await _userManager.FindByNameAsync(request.UserName);

            if (managedUser == null || !await _userManager.CheckPasswordAsync(managedUser, request.Password))
                return Unauthorized();

            var roles = await _userManager.GetRolesAsync(managedUser);

            string accessToken = _tokenService.CreateToken(managedUser, roles.ToList());
            RefreshToken refreshToken = _tokenService.GenerateRefreshToken(ipAddress());

           managedUser.RefreshTokens = new List<RefreshToken>();

            managedUser.RefreshTokens.Add(refreshToken);
            
           _context.SaveChanges();

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Domain = "localhost",
                Path = "/",
                Expires = refreshToken.Expires  
            };
            
            Response.Cookies.Append("Token", refreshToken.Token);
            return Ok(new AuthResponse
            {
                Username = managedUser.UserName,
                Email = managedUser.Email,
                Token = accessToken,
            });

        }

        [HttpPost]
        // [ValidateAntiForgeryToken]
        [Authorize(Roles = "dd")]
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
            Response.Cookies.Delete("Token");
            var sss = ipAddress();

            //await _userManager..SignOutAsync();
            return Ok();
        }

        private string ipAddress()
        {
            // get source ip address for the current request
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }

        private void removeOldRefreshTokens(User user)
        {
            // remove old inactive refresh tokens from user based on TTL in app settings
            /*user.RefreshTokens.RemoveAll(x =>
                !x.IsActive &&
                x.Created.AddDays(_appSettings.RefreshTokenTTL) <= DateTime.UtcNow);
   */     }

        private async void InactifTokenRefresh(string userId)
        {
            //await _context.refreshTokens.Where(w => w.UserId == userId &&  w.IsActive).ForEachAsync(f =>
            //{
            //    f.IsActive = false;
            //});
        }

        
    }
}
