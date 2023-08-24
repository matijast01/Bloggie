using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Bloggie.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostCommentController : ControllerBase
    {
        private readonly IBlogPostCommentRepository blogPostCommentRepository;
        private readonly UserManager<IdentityUser> userManager;

        public BlogPostCommentController(IBlogPostCommentRepository blogPostCommentRepository, UserManager<IdentityUser> userManager)
        {
            this.blogPostCommentRepository = blogPostCommentRepository;
            this.userManager = userManager;
        }

        [HttpPost]
        [Route("Delete/{commentId:Guid}")]
        public async Task<IActionResult> DeleteCommentById([FromRoute] Guid commentId) 
        {
            var userId = userManager.GetUserId(User);
            await blogPostCommentRepository.DeleteCommentByIdAsync(commentId, Guid.Parse(userId));
            return Ok();
        }

        [HttpPost]
        [Route("Update/{commentId:Guid}")]
        public async Task<IActionResult> UpdateCommentById([FromRoute] Guid commentId, [FromBody] EditCommentRequest editCommentRequest)
        {
            var userId = userManager.GetUserId(User);
            await blogPostCommentRepository.UpdateCommentByIdAsync(commentId, editCommentRequest, Guid.Parse(userId));
            return Ok();
        }
    }
}
