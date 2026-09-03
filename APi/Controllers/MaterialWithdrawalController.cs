using APi.DTOs;
using API.RequestHelpers;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Interfaces.Specifications.MaterialWithdrawal;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class MaterialWithdrawalController(IUnitOfWork unit, IMapper mapper) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<Pagination<MaterialWithdrawalDto>>> GetMaterialWithdrawals(
        [FromQuery] MaterialWithdrawalSpecParams specParams
    )
    {
        var spec = new MaterialWithdrawalSpecification(specParams);
        var items = await unit.Repository<MaterialWithdrawal>().ListAsync(spec);
        var totalItems = await unit.Repository<MaterialWithdrawal>().CountAsync(spec);
        var data = mapper.Map<List<MaterialWithdrawalDto>>(items);

        return Ok(
            new Pagination<MaterialWithdrawalDto>(
                specParams.PageIndex,
                specParams.PageSize,
                totalItems,
                data
            )
        );
    }

    [HttpGet("by-procurement/{procurementRecordId:int}")]
    public async Task<ActionResult<Pagination<MaterialWithdrawalDto>>> GetMaterialWithdrawalsByProcurement(
        int procurementRecordId,
        [FromQuery] MaterialWithdrawalSpecParams specParams
    )
    {
        var spec = new MaterialWithdrawalSpecification(specParams, procurementRecordId);
        var items = await unit.Repository<MaterialWithdrawal>().ListAsync(spec);
        var totalItems = await unit.Repository<MaterialWithdrawal>().CountAsync(spec);
        var data = mapper.Map<List<MaterialWithdrawalDto>>(items);

        return Ok(
            new Pagination<MaterialWithdrawalDto>(
                specParams.PageIndex,
                specParams.PageSize,
                totalItems,
                data
            )
        );
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<MaterialWithdrawalDto>> GetMaterialWithdrawal(int id)
    {
        var materialWithdrawal = await unit.Repository<MaterialWithdrawal>().GetByIdAsync(id);

        if (materialWithdrawal == null || !materialWithdrawal.is_active)
        {
            return NotFound();
        }

        return Ok(mapper.Map<MaterialWithdrawalDto>(materialWithdrawal));
    }

    [HttpPost]
    public async Task<ActionResult<MaterialWithdrawalDto>> CreateMaterialWithdrawal(
        MaterialWithdrawalCreateDto dto
    )
    {
        var materialWithdrawal = mapper.Map<MaterialWithdrawal>(dto);

        var count = await unit.Repository<MaterialWithdrawal>().CountAsync(
            new MaterialWithdrawalSpecification(
                new MaterialWithdrawalSpecParams { PageIndex = 1, PageSize = int.MaxValue },
                dto.procurement_record_id
            )
        );

        materialWithdrawal.withdrawal_document_no = (count + 1).ToString();
        materialWithdrawal.is_active = true;
        materialWithdrawal.created_at = DateTime.UtcNow;

        unit.Repository<MaterialWithdrawal>().Add(materialWithdrawal);

        if (await unit.Complete())
        {
            return CreatedAtAction(
                nameof(GetMaterialWithdrawal),
                new { id = materialWithdrawal.material_withdrawal_id },
                mapper.Map<MaterialWithdrawalDto>(materialWithdrawal)
            );
        }

        return BadRequest("Problem creating material withdrawal");
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateMaterialWithdrawal(int id, MaterialWithdrawalCreateDto dto)
    {
        var existing = await unit.Repository<MaterialWithdrawal>().GetByIdAsync(id);

        if (existing == null || !existing.is_active)
        {
            return NotFound();
        }

        mapper.Map(dto, existing);
        existing.updated_at = DateTime.UtcNow;

        if (await unit.Complete())
        {
            return NoContent();
        }

        return BadRequest("Problem updating material withdrawal");
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteMaterialWithdrawal(int id)
    {
        var existing = await unit.Repository<MaterialWithdrawal>().GetByIdAsync(id);

        if (existing == null || !existing.is_active)
        {
            return NotFound();
        }

        existing.is_active = false;
        existing.updated_at = DateTime.UtcNow;
        unit.Repository<MaterialWithdrawal>().Update(existing);

        if (await unit.Complete())
        {
            return NoContent();
        }

        return BadRequest("Problem deleting material withdrawal");
    }
}
