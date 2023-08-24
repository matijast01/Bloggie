using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Bloggie.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostLikeController : ControllerBase
    {
        private readonly IBlogPostLikeRepository blogPostLikeRepository;
        private readonly UserManager<IdentityUser> userManager;

        public BlogPostLikeController(IBlogPostLikeRepository blogPostLikeRepository, UserManager<IdentityUser> userManager)
        {
            this.blogPostLikeRepository = blogPostLikeRepository;
            this.userManager = userManager;
        }

        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> AddLike([FromBody] AddLikeRequest addLikeRequest) 
        {

            var model = new BlogPostLike
            {
                BlogPostId = addLikeRequest.BlogPostId,
                UserId = addLikeRequest.UserId,
            };

            var blogPostLike = await blogPostLikeRepository.AddLikeForBlog(model);
            if (blogPostLike is not null)
            {
                var userLikeId = blogPostLike.Id;
                return Ok(userLikeId);
            }
            return Ok();
        }

        [HttpPost]
        [Route("Delete")]
        public async Task<IActionResult> DeleteLike([FromBody] DeleteLikeRequest deleteLikeRequest) 
        {
            var model = new BlogPostLike
            {
                BlogPostId = deleteLikeRequest.BlogPostId,
                UserId = deleteLikeRequest.UserId,
            };

            if (model.UserId == Guid.Parse(userManager.GetUserId(User)))
            {
                await blogPostLikeRepository.DeleteLike(model);
            }
            return Ok();
        }

        [HttpGet]
        [Route("{blogPostId:Guid}/totalLikes")]
        public async Task<IActionResult> GetTotalLikesForBlog([FromRoute] Guid blogPostId) 
        {
            var totalLikes = await blogPostLikeRepository.GetTotalLikes(blogPostId);
            return Ok(totalLikes);
        }
    }
}
