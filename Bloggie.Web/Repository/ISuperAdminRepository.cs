using Bloggie.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Bloggie.Web.Repository
{
    public interface ISuperAdminRepository
    {
        Task<SuperAdminViewModel> DeleteOrphans();
    }
}
