using Bloggie.Web.Data;
using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bloggie.Web.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class SuperAdminController : Controller
    {
        private readonly ISuperAdminRepository superAdminRepository;

        public SuperAdminController(ISuperAdminRepository superAdminRepository)
        {
            this.superAdminRepository = superAdminRepository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var superAdminViewModel = new SuperAdminViewModel
            {
                DeletedOrphans = 0
            };
            return View(superAdminViewModel);
        }

        [HttpPost]
        [ActionName("Index")]
        public async Task<IActionResult> Index(SuperAdminViewModel superAdminViewModel)
        {
            superAdminViewModel = await superAdminRepository.DeleteOrphans();        
            return View("Index", superAdminViewModel);
        }
    }
}
