using Bloggie.Web.Data;
using Bloggie.Web.Models.ViewModels;

namespace Bloggie.Web.Repository
{
    public class SuperAdminRepository : ISuperAdminRepository
    {
        private readonly BloggieDbContext bloggieDbContext;
        private readonly AuthDbContext authDbContext;

        public SuperAdminRepository(BloggieDbContext bloggieDbContext, AuthDbContext authDbContext) 
        {
            this.bloggieDbContext = bloggieDbContext;
            this.authDbContext = authDbContext;
        }
        public async Task<SuperAdminViewModel> DeleteOrphans()
        {
            int counter = 0;
            var users = authDbContext.Users.ToList();
            var userIds = new List<string>();
            foreach (var user in users)
            {
                userIds.Add(user.Id);
            }
            foreach (var comment in bloggieDbContext.BlogPostComments.ToList())
            {
                if (userIds.Contains(comment.UserId.ToString()) == false)
                {
                    bloggieDbContext.BlogPostComments.Remove(comment);
                    counter++;
                }
            }
            foreach(var like in bloggieDbContext.BlogPostLikes.ToList())
            {
                if (userIds.Contains(like.UserId.ToString()) == false)
                {
                    bloggieDbContext.BlogPostLikes.Remove(like);
                    counter++;
                }
            }
            await bloggieDbContext.SaveChangesAsync();
            var superAdminViewModel = new SuperAdminViewModel { DeletedOrphans = counter };
            return superAdminViewModel;
        }
    }
}
