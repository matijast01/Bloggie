using Microsoft.AspNetCore.Identity;

namespace Bloggie.Web.Repository
{
    public interface IUserRepository
    {
        Task<IEnumerable<IdentityUser>> GetAll(IList<String> userRoles);

        Task<IdentityResult> DeleteUser(IdentityUser userToDelete);
    }
}
