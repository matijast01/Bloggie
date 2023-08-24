namespace Bloggie.Web.Models.ViewModels
{
    public class DeleteLikeRequest
    {
        public Guid BlogPostId { get; set; }
        public Guid UserId { get; set; }
    }
}
