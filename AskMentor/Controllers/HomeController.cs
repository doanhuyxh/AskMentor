using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace AskMentor.Controllers
{

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly AskMentor.Helper.Helper helper;
        private readonly RoomService roomService;

        public HomeController(ILogger<HomeController> logger, UserManager<ApplicationUser> userManager, ApplicationDbContext context, Helper.Helper helper, RoomService roomService)
        {
            _logger = logger;
            _userManager = userManager;
            _dbContext = context;
            this.helper = helper;
            this.roomService = roomService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Leaner()
        {
            string userId = await helper.GetUserId(User.Identity.Name);
            ViewBag.userId = userId;
            List<ApplicationUser> Mentors = await helper.GetUserByRole("Mentor");
            return View(Mentors);
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Chat()
        {
            string userId = await helper.GetUserId(User.Identity.Name);
            ViewBag.userId = userId;
            List<Room> listRoom = roomService.getRoomsList(userId);
            return View(listRoom);
        }

    }
}
