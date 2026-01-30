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
            try
            {
                var users = await _adminUserService.GetAllUsersAsync();
                return Ok(users);
            }
            catch(UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
        }



        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateUserStatusAsync(Guid id,UpdateUserStatusDto request)
        {
            try
            {
                var result = await _adminUserService.UpdateUserStatusAsync(id, request.IsActive);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
        }
    }
}
