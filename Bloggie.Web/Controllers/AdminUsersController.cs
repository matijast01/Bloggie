using Bloggie.Web.Data;
using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Bloggie.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminUsersController : Controller
    {
        private readonly IUserRepository userRepository;
        private readonly UserManager<IdentityUser> userManager;
        private readonly BloggieDbContext bloggieDbContext;

        public AdminUsersController(IUserRepository userRepository, UserManager<IdentityUser> userManager, BloggieDbContext bloggieDbContext)
        {
            this.userRepository = userRepository;
            this.userManager = userManager;
            this.bloggieDbContext = bloggieDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var currentUser = await userManager.FindByIdAsync(userManager.GetUserId(User));
            await userManager.GetRolesAsync(currentUser);
            var userRoles = await userManager.GetRolesAsync(currentUser);
            var users = await userRepository.GetAll(userRoles);
            var usersViewModel = new UserViewModel();
            usersViewModel.Users = new List<User>();

            foreach (var user in users) 
            {
                usersViewModel.Users.Add(new User 
                { 
                    Id = Guid.Parse(user.Id),
                    Username = user.UserName,
                    EmailAddress = user.Email
                }); 
            }

            return View(usersViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> List(UserViewModel request) 
        {

            if (!ModelState.IsValid) 
            {
                return await List();
            }
            var identityUser = new IdentityUser 
            {
                UserName = request.Username,
                Email = request.Email
            };

            var identityResult = await userManager.CreateAsync(identityUser, request.Password);

            if (identityResult is not null) 
            {
                if (identityResult.Succeeded) 
                {
                    // assign roles to this user
                    var roles = new List<string> { "User" };

                    if (request.AdminRoleCheckBox) 
                    {
                        roles.Add("Admin");
                    }

                    identityResult = await userManager.AddToRolesAsync(identityUser, roles);

                    if (identityResult is not null && identityResult.Succeeded) 
                    {
                        return RedirectToAction("List", "AdminUsers");
                    }
                }
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id) 
        {
            var userToDelete = await userManager.FindByIdAsync(id.ToString());
            var user = await userManager.FindByIdAsync(userManager.GetUserId(User));
            await userManager.GetRolesAsync(user);
            var userRoles = await userManager.GetRolesAsync(user);

            if (userToDelete is not null)
            {
                var userToDeleteRoles = await userManager.GetRolesAsync(userToDelete);
                if (userRoles.Count > userToDeleteRoles.Count)
                {
                    var identityResult = await userRepository.DeleteUser(userToDelete);

                    if (identityResult is not null && identityResult.Succeeded)
                    {
                        await bloggieDbContext.SaveChangesAsync();
                        return RedirectToAction("List", "AdminUsers");
                    }
                }
            }
            return RedirectToAction("List", "AdminUsers");
        }
    }
}
