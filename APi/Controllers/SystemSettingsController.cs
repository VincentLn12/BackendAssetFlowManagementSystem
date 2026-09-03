using API.Controllers;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class SystemSettingsController(IUnitOfWork unit, FileService fileService) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<SystemSetting>> GetSettings()
    {
        var settings = await unit.Repository<SystemSetting>().ListAllAsync();
        var setting = settings.FirstOrDefault(x => x.is_active);

        if (setting == null)
        {
            setting = new SystemSetting
            {
                project_name = "ระบบบริหารพัสดุ",
                logo_path = "/institute-Logo.png",
                is_active = true
            };
            unit.Repository<SystemSetting>().Add(setting);
            await unit.Complete();
        }

        return Ok(setting);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut]
    public async Task<IActionResult> UpdateSettings(SystemSetting settingDto)
    {
        var settings = await unit.Repository<SystemSetting>().ListAllAsync();
        var setting = settings.FirstOrDefault(x => x.is_active);

        if (setting == null)
        {
            setting = new SystemSetting
            {
                project_name = settingDto.project_name,
                logo_path = settingDto.logo_path,
                is_active = true
            };
            unit.Repository<SystemSetting>().Add(setting);
        }
        else
        {
            setting.project_name = settingDto.project_name;
            if (!string.IsNullOrEmpty(settingDto.logo_path))
            {
                setting.logo_path = settingDto.logo_path;
            }
            setting.updated_at = DateTime.UtcNow;
            unit.Repository<SystemSetting>().Update(setting);
        }

        if (await unit.Complete())
        {
            return NoContent();
        }

        return BadRequest("มีปัญหาในการอัปเดตการตั้งค่าระบบ");
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("upload-logo")]
    public async Task<IActionResult> UploadLogo(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("ไม่พบไฟล์รูปภาพ");
        }

        if (file.Length > 2 * 1024 * 1024)
        {
            return BadRequest("ขนาดไฟล์โลโก้ต้องไม่เกิน 2MB");
        }

        try
        {
            var filePath = await fileService.UploadFileAsync(file, "uploads/logo");
            return Ok(new { logoPath = filePath });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("reset")]
    public async Task<IActionResult> ResetSettings()
    {
        var settings = await unit.Repository<SystemSetting>().ListAllAsync();
        var setting = settings.FirstOrDefault(x => x.is_active);

        if (setting != null)
        {
            setting.project_name = "ระบบบริหารพัสดุ";
            setting.logo_path = "/institute-Logo.png";
            setting.updated_at = DateTime.UtcNow;
            unit.Repository<SystemSetting>().Update(setting);
            await unit.Complete();
        }

        return Ok(new { project_name = "ระบบบริหารพัสดุ", logo_path = "/institute-Logo.png" });
    }
}
