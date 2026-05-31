using APi.DTOs;
using API.RequestHelpers;
using Core.Interfaces.Specifications.Roles;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class RolesController(RoleManager<IdentityRole> roleManager) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<Pagination<RoleDto>>> GetRoles(
     [FromQuery] RolesSpecParams rolesParams)
    {
        var query = roleManager.Roles.AsQueryable();

        var totalItems = await query.CountAsync();

        var spec = new RolesSpecification();

        query = spec.Apply(query, rolesParams);

        var roles = await query
            .Select(x => new RoleDto
            {
                id = x.Id,
                name = x.Name ?? ""
            })
            .ToListAsync();

        return Ok(new Pagination<RoleDto>(
            rolesParams.PageIndex,
            rolesParams.PageSize,
            totalItems,
            roles
        ));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RoleDto>> GetRole(string id)
    {
        var role = await roleManager.FindByIdAsync(id);

        if (role == null) return NotFound();

        return new RoleDto
        {
            id = role.Id,
            name = role.Name ?? ""
        };
    }

    [HttpPost]
    public async Task<ActionResult> CreateRole(CreateRoleDto dto)
    {
        var exists = await roleManager.RoleExistsAsync(dto.name);

        if (exists) return BadRequest("มีสิทธิ์ผู้ใช้งานนี้อยู่แล้ว");

        var role = new IdentityRole
        {
            Name = dto.name
        };

        var result = await roleManager.CreateAsync(role);

        if (!result.Succeeded) return BadRequest(result.Errors);

        return Ok(new { message = "เพิ่มสิทธิ์ผู้ใช้งานสำเร็จ" });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRole(string id, UpdateRoleDto dto)
    {
        var role = await roleManager.FindByIdAsync(id);

        if (role == null) return NotFound();

        role.Name = dto.name;

        var result = await roleManager.UpdateAsync(role);

        if (!result.Succeeded) return BadRequest(result.Errors);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRole(string id)
    {
        var role = await roleManager.FindByIdAsync(id);

        if (role == null) return NotFound();


        var result = await roleManager.DeleteAsync(role);

        if (!result.Succeeded) return BadRequest(result.Errors);

        return NoContent();
    }
}