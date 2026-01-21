using ECommerce.Application.DTOs.Users;
using ECommerce.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers
{
    [ApiController]
    [Route("api/admin/users")]
    [Authorize(Roles ="Admin")]
    public class AdminUserController:ControllerBase
    {
        private readonly IAdminUserService _adminUserService;

        public AdminUserController(IAdminUserService adminUserService)
        {
            _adminUserService = adminUserService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _adminUserService.GetAllUsersAsync();
            return Ok(users);
        }



        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateUserStatusAsync(Guid id,UpdateUserStatusDto request)
        {
            var result = await _adminUserService.UpdateUserStatusAsync(id, request.IsActive);
            return Ok(result);
        }
    }
}
