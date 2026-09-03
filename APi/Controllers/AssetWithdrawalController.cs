using APi.DTOs;
using API.RequestHelpers;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Interfaces.Specifications.AssetWithdrawal;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class AssetWithdrawalController(IUnitOfWork unit, IMapper mapper) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<Pagination<AssetWithdrawalDto>>> GetAssetWithdrawals(
        [FromQuery] AssetWithdrawalSpecParams specParams
    )
    {
        var spec = new AssetWithdrawalSpecification(specParams);
        var items = await unit.Repository<AssetWithdrawal>().ListAsync(spec);
        var totalItems = await unit.Repository<AssetWithdrawal>().CountAsync(spec);
        var data = mapper.Map<List<AssetWithdrawalDto>>(items);

        return Ok(
            new Pagination<AssetWithdrawalDto>(
                specParams.PageIndex,
                specParams.PageSize,
                totalItems,
                data
            )
        );
    }

    [HttpGet("by-procurement/{procurementRecordId:int}")]
    public async Task<ActionResult<Pagination<AssetWithdrawalDto>>> GetAssetWithdrawalsByProcurement(
        int procurementRecordId,
        [FromQuery] AssetWithdrawalSpecParams specParams
    )
    {
        var spec = new AssetWithdrawalSpecification(specParams, procurementRecordId);
        var items = await unit.Repository<AssetWithdrawal>().ListAsync(spec);
        var totalItems = await unit.Repository<AssetWithdrawal>().CountAsync(spec);
        var data = mapper.Map<List<AssetWithdrawalDto>>(items);

        return Ok(
            new Pagination<AssetWithdrawalDto>(
                specParams.PageIndex,
                specParams.PageSize,
                totalItems,
                data
            )
        );
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<AssetWithdrawalDto>> GetAssetWithdrawal(int id)
    {
        var assetWithdrawal = await unit.Repository<AssetWithdrawal>().GetByIdAsync(id);

        if (assetWithdrawal == null || !assetWithdrawal.is_active)
        {
            return NotFound();
        }

        return Ok(mapper.Map<AssetWithdrawalDto>(assetWithdrawal));
    }

    [HttpPost]
    public async Task<ActionResult<AssetWithdrawalDto>> CreateAssetWithdrawal(
        AssetWithdrawalCreateDto dto
    )
    {
        var assetWithdrawal = mapper.Map<AssetWithdrawal>(dto);

        var count = await unit.Repository<AssetWithdrawal>().CountAsync(
            new AssetWithdrawalSpecification(
                new AssetWithdrawalSpecParams { PageIndex = 1, PageSize = int.MaxValue },
                dto.procurement_record_id
            )
        );

        assetWithdrawal.withdrawal_document_no = (count + 1).ToString();
        assetWithdrawal.is_active = true;
        assetWithdrawal.created_at = DateTime.UtcNow;

        unit.Repository<AssetWithdrawal>().Add(assetWithdrawal);

        if (await unit.Complete())
        {
            return CreatedAtAction(
                nameof(GetAssetWithdrawal),
                new { id = assetWithdrawal.procurement_withdrawal_id },
                mapper.Map<AssetWithdrawalDto>(assetWithdrawal)
            );
        }

        return BadRequest("Problem creating asset withdrawal");
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateAssetWithdrawal(int id, AssetWithdrawalCreateDto dto)
    {
        var existing = await unit.Repository<AssetWithdrawal>().GetByIdAsync(id);

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

        return BadRequest("Problem updating asset withdrawal");
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAssetWithdrawal(int id)
    {
        var existing = await unit.Repository<AssetWithdrawal>().GetByIdAsync(id);

        if (existing == null || !existing.is_active)
        {
            return NotFound();
        }

        existing.is_active = false;
        existing.updated_at = DateTime.UtcNow;
        unit.Repository<AssetWithdrawal>().Update(existing);

        if (await unit.Complete())
        {
            return NoContent();
        }

        return BadRequest("Problem deleting asset withdrawal");
    }
}
