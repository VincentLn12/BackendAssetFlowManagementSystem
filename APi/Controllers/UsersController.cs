using APi.DTOs;
using API.RequestHelpers;
using Microsoft.AspNetCore.Identity;
namespace API.Controllers;

public class UsersController(UserManager<AppUser> userManager) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<Pagination<UserDto>>> GetUsers(
         [FromQuery] UserSpecParams userParams)
    {
        var query = userManager.Users.AsQueryable();

        if (!string.IsNullOrWhiteSpace(userParams.Search))
        {
            query = query.Where(x =>
                x.UserName!.Contains(userParams.Search) ||
                x.Email!.Contains(userParams.Search)
            );
        }

        var totalItems = await query.CountAsync();

        var users = await query
            .Skip((userParams.PageIndex - 1) * userParams.PageSize)
            .Take(userParams.PageSize)
            .Select(x => new UserDto
            {
                id = x.Id,
                userName = x.UserName ?? "",
                email = x.Email ?? "",
                phoneNumber = x.PhoneNumber ?? ""
            })
            .ToListAsync();

        return Ok(new Pagination<UserDto>(
            userParams.PageIndex,
            userParams.PageSize,
            totalItems,
            users
        ));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetUser(string id)
    {
        var user = await userManager.FindByIdAsync(id);

        if (user == null) return NotFound();

        return new UserDto
        {
            id = user.Id,
            userName = user.UserName ?? "",
            email = user.Email ?? "",
            phoneNumber = user.PhoneNumber ?? ""
        };
    }

    [HttpPost]
    public async Task<ActionResult> CreateUser(CreateUserDto dto)
    {
        var exists = await userManager.FindByEmailAsync(dto.email);

        if (exists != null) return BadRequest("มีผู้ใช้งานอีเมลนี้อยู่แล้ว");

        var user = new AppUser
        {
            UserName = dto.userName,
            Email = dto.email,
            PhoneNumber = dto.phoneNumber
        };

        var result = await userManager.CreateAsync(user, dto.password);

        if (!result.Succeeded) return BadRequest(result.Errors);

        return Ok(new { message = "เพิ่มผู้ใช้งานสำเร็จ" });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(string id, UpdateUserDto dto)
    {
        var user = await userManager.FindByIdAsync(id);

        if (user == null) return NotFound();

        user.UserName = dto.userName;
        user.Email = dto.email;
        user.PhoneNumber = dto.phoneNumber;

        var result = await userManager.UpdateAsync(user);

        if (!result.Succeeded) return BadRequest(result.Errors);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(string id)
    {
        var user = await userManager.FindByIdAsync(id);

        if (user == null) return NotFound();

        var result = await userManager.DeleteAsync(user);

        if (!result.Succeeded) return BadRequest(result.Errors);

        return NoContent();
    }
}