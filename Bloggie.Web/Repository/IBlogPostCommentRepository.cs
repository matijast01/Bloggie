using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;

namespace Bloggie.Web.Repository
{
    public interface IBlogPostCommentRepository
    {
        Task<BlogPostComment> AddAsync(BlogPostComment blogPostComment);

        Task<IEnumerable<BlogPostComment>> GetCommentsByBlogIdAsync(Guid blogPostId);

        Task<BlogPostComment?> DeleteCommentByIdAsync(Guid commentId, Guid userId);

        Task<BlogPostComment?> UpdateCommentByIdAsync(Guid commentId, EditCommentRequest editCommentRequest, Guid userId);
    }
}
