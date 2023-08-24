using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Repository
{
    public class BlogPostLikeRepository : IBlogPostLikeRepository
    {
        private readonly BloggieDbContext bloggieDbContext;

        public BlogPostLikeRepository(BloggieDbContext bloggieDbContext)
        {
            this.bloggieDbContext = bloggieDbContext;
        }

        public async Task<BlogPostLike> AddLikeForBlog(BlogPostLike blogPostLike)
        {
            var exisitingLikes = bloggieDbContext.BlogPostLikes.Where(x => x.BlogPostId == blogPostLike.BlogPostId && x.UserId == blogPostLike.UserId);
            if (exisitingLikes.Any()) 
            {
                return null;
            }
            await bloggieDbContext.AddAsync(blogPostLike);
            await bloggieDbContext.SaveChangesAsync();
            return blogPostLike;
        }

        public async Task<BlogPostLike?> DeleteLike(BlogPostLike blogPostLike)
        {
            var userId = blogPostLike.UserId;
            var blogPostId = blogPostLike.BlogPostId;
            var existingLike = await bloggieDbContext.BlogPostLikes.FirstOrDefaultAsync(x => x.BlogPostId == blogPostId && x.UserId == userId);
            if (existingLike != null) 
            {
                bloggieDbContext.Remove(existingLike);
                await bloggieDbContext.SaveChangesAsync();
                return existingLike;
            }

            return null;
        }

        public async Task<IEnumerable<BlogPostLike>> GetLikesForBlog(Guid blogPostId)
        {
            return await bloggieDbContext.BlogPostLikes.Where(x => x.BlogPostId == blogPostId).ToListAsync();
        }

        public async Task<int> GetTotalLikes(Guid blogPostId)
        {
            return await bloggieDbContext.BlogPostLikes.CountAsync(x => x.BlogPostId == blogPostId);
        }
    }
}
