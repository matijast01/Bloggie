using Bloggie.Web.Data;
using Microsoft.AspNetCore.Identity;

using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly AuthDbContext authDbContext;
        private readonly Microsoft.AspNetCore.Identity.UserManager<IdentityUser> userManager;
        private readonly BloggieDbContext bloggieDbContext;

        public UserRepository(AuthDbContext authDbContext, UserManager<IdentityUser> userManager, BloggieDbContext bloggieDbContext)
        {
            this.authDbContext = authDbContext;
            this.userManager = userManager;
            this.bloggieDbContext = bloggieDbContext;
        }

        public async Task<IdentityResult> DeleteUser(IdentityUser userToDelete)
        {
            var userToDeleteId = await userManager.GetUserIdAsync(userToDelete);
            var identityResult = await userManager.DeleteAsync(userToDelete);

            if (identityResult.Succeeded && identityResult is not null) 
            {
                var likesToDelete = bloggieDbContext.BlogPostLikes.Where(x => x.UserId == Guid.Parse(userToDeleteId));
                foreach(var like in likesToDelete) 
                {
                    bloggieDbContext.BlogPostLikes.Remove(like);

                }

                var commentsToDelete = bloggieDbContext.BlogPostComments.Where(x => x.UserId == Guid.Parse(userToDeleteId));
                foreach (var comment in commentsToDelete)
                {
                    bloggieDbContext.BlogPostComments.Remove(comment);
                }
            }
            return identityResult;
        }

        public async Task<IEnumerable<IdentityUser>> GetAll(IList<String> userRoles)
        {
            var users = await authDbContext.Users.ToListAsync();
            
            var authorizedUsers = await authDbContext.Users.ToListAsync();

            foreach (var user in users)
            {
                var currentUserRoles = await userManager.GetRolesAsync(user);
                if (currentUserRoles.Count >= userRoles.Count) 
                {
                    authorizedUsers.Remove(user);
                }
            }

            return authorizedUsers;
        }
    }
}
