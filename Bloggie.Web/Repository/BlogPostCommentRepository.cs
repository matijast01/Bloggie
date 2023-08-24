using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Repository
{
    public class BlogPostCommentRepository : IBlogPostCommentRepository
    {
        private readonly BloggieDbContext bloggieDbContext;
        private readonly UserManager<IdentityUser> userManager;

        public BlogPostCommentRepository(BloggieDbContext bloggieDbContext, UserManager<IdentityUser> userManager) 
        {
            this.bloggieDbContext = bloggieDbContext;
            this.userManager = userManager;
        }

        public async Task<BlogPostComment> AddAsync(BlogPostComment blogPostComment)
        {
            await bloggieDbContext.BlogPostComments.AddAsync(blogPostComment);
            await bloggieDbContext.SaveChangesAsync();
            return blogPostComment;
        }

        public async Task<BlogPostComment?> DeleteCommentByIdAsync(Guid commentId, Guid userId)
        {
            var commentToDelete = await bloggieDbContext.BlogPostComments.FindAsync(commentId);
            var user = await userManager.FindByIdAsync(userId.ToString());
            var rolesList = await userManager.GetRolesAsync(user);

            if (commentToDelete != null)
            {
                if (commentToDelete.UserId == userId || rolesList.Contains("Admin"))
                {
                    bloggieDbContext.BlogPostComments.Remove(commentToDelete);
                    await bloggieDbContext.SaveChangesAsync();
                    return commentToDelete;
                }
            }
            return null;     
        }

        public async Task<IEnumerable<BlogPostComment>> GetCommentsByBlogIdAsync(Guid blogPostId)
        {
            return await bloggieDbContext.BlogPostComments.Where(x => x.BlogPostId == blogPostId)
                .ToListAsync();
        }

        public async Task<BlogPostComment?> UpdateCommentByIdAsync(Guid commentId, EditCommentRequest editCommentRequest, Guid userId)
        {
            var existingComment = await bloggieDbContext.BlogPostComments.FirstOrDefaultAsync(x => x.Id == commentId);

            if (existingComment != null) 
            {
                if (existingComment.UserId == userId)
                {
                    existingComment.Description = editCommentRequest.Description;

                    await bloggieDbContext.SaveChangesAsync();
                    return existingComment;
                }
            }
            return null;
        }
    }
}
