
using Core.Interfaces.Specifications.AssetItem;
using Infrastructure.Migrations;


namespace API.Controllers;

public class AssetItemController(IUnitOfWork unit, IMapper mapper) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<AssetItem>>> GetAssetItems([FromQuery] AssetItemSpecParams assetItemParams)
    {
        var spec = new AssetItemSpecification(assetItemParams);

        var assetItems = await unit.Repository<AssetItem>().ListAsync(spec);
         
        var countSpec = new AssetItemForCountSpecification(assetItemParams);    
        var totalItems = await unit.Repository<AssetItem>()
            .CountAsync(countSpec);

        var data = mapper.Map<List<AssetItemDto>>(assetItems);

        return Ok(new Pagination<AssetItemDto>(
            assetItemParams.PageIndex,
            assetItemParams.PageSize,
            totalItems,
            data
        ));
    }

    [HttpGet("by-procurement/{procurementRecordId}")]
    public async Task<ActionResult<Pagination<AssetItemDto>>> GetAssetItemsByProcurementRecordId(
      int procurementRecordId,
      [FromQuery] AssetItemSpecParams assetItemParams)
    {
        assetItemParams.ProcurementRecordId = procurementRecordId;

        var spec = new AssetItemSpecification(assetItemParams);

        var assetItems = await unit.Repository<AssetItem>().ListAsync(spec);

        var countSpec = new AssetItemForCountSpecification(assetItemParams);

        var totalItems = await unit.Repository<AssetItem>()
            .CountAsync(countSpec);

        var data = mapper.Map<List<AssetItemDto>>(assetItems);

        return Ok(new Pagination<AssetItemDto>(
            assetItemParams.PageIndex,
            assetItemParams.PageSize,
            totalItems,
            data
        ));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AssetItem>> GetAssetItem(int id)
    {
        var assetItem = await unit.Repository<AssetItem>().GetByIdAsync(id);

        if (assetItem == null) return NotFound();
        return assetItem;
    }

    [HttpGet("details/{id}")]
    public async Task<ActionResult<AssetItemDetailsDto>> GetAssetItemDetails(int id)
    {
        var asset = await unit.Repository<AssetItem>().GetByIdAsync(id);

        if (asset == null) return NotFound();

        

        var dto = new AssetItemDetailsDto
        {
            asset_id = asset.asset_id,
            receive_date = asset.receive_date
        };

        //if (asset.staff_id != null)
        //{
        //    var staff = await unit.Repository<Staffs>().GetByIdAsync(asset.staff_id.Value);

        //    if (staff != null)
        //    {
        //        var prefixName = "";

        //        if (staff.prefix_id != null)
        //        {
        //            var prefix = await unit.Repository<Prefixes>().GetByIdAsync(staff.prefix_id);
        //            prefixName = prefix?.prefix_name ?? "";
        //        }

        //        dto.staff_name = $"{prefixName}{staff.first_name} {staff.last_name}";

        //        if (staff.department_id != null)
        //        {
        //            var department = await unit.Repository<Departments>()
        //                .GetByIdAsync(staff.department_id);

        //            dto.department_name = department?.department_name ?? "";
        //        }
        //    }
        //}

        if (asset.fund_category_id != null)
        {
            var fundCategory = await unit.Repository<Fund_categories>().GetByIdAsync(asset.fund_category_id.Value);
            dto.fund_name = fundCategory?.fund_name ?? "";
        }

        //if (asset.vendor_id != null)
        //{
        //    var vendor = await unit.Repository<Vendors>().GetByIdAsync(asset.vendor_id.Value);
        //    dto.vendor_name = vendor?.vendor_name ?? "";
        //    dto.vendor_address = vendor?.address ?? "";
        //    dto.vendor_tel = vendor?.phone ?? "";
        //}

        if (asset.acquisition_method_id != null)
        {
            var method = await unit.Repository<AcquisitionMethod>()
                .GetByIdAsync(asset.acquisition_method_id.Value);

            dto.acquisition_method_name = method?.acquisition_method_name ?? "";
        }

        if (asset.procurement_record_id != null)
        {
            var procurement = await unit.Repository<Procurement_records>()
                .GetByIdAsync(asset.procurement_record_id.Value);

            if (procurement != null)
            {
                var project = await unit.Repository<Projects>()
                    .GetByIdAsync(procurement.project_id);

                dto.project_code = project?.project_code ?? "";
            }
        }

        var subItems = await unit.Repository<AssetSubItem>().ListAllAsync();
        var categories = await unit.Repository<AssetCategory>().ListAllAsync();
        var units = await unit.Repository<MaterialUnit>().ListAllAsync();

        var subItemsByAsset = subItems
            .Where(x => x.asset_id == asset.asset_id)
            .ToList();

        dto.asset_sub_items = subItemsByAsset
            .Select(x => new AssetSubItemDto
            {
                asset_sub_item_id = x.asset_sub_item_id,
                asset_id = x.asset_id,
                item_no = x.item_no,
                sub_item_name = x.sub_item_name,
                asset_category_id = x.asset_category_id,
                category_name = categories
                    .FirstOrDefault(c => c.asset_category_id == x.asset_category_id)?.category_name ?? "",
                running_start_no = x.running_start_no,
                running_end_no = x.running_end_no,
                fiscal_asset_year = x.fiscal_asset_year,
                quantity = x.quantity,
                unit_id = x.unit_id,
                unit_name = units
                    .FirstOrDefault(u => u.unit_id == x.unit_id)?.unit_name ?? "",
                unit_price = x.unit_price,
                total_price = x.total_price,
                useful_life_year = x.useful_life_year
            })
            .ToList();

        return Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<AssetItemCreateDto>> CreateAssetItem(
         AssetItemCreateDto dto)
        {
        var assetItem = mapper.Map<AssetItem>(dto);

        assetItem.is_active = true;

        unit.Repository<AssetItem>().Add(assetItem);
        if (await unit.Complete())
        {
            return CreatedAtAction(
                nameof(GetAssetItem),
                new { id = assetItem.asset_id },
                assetItem
            );
        }

        return BadRequest("Problem creating project");
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAssetItem(int id, AssetItemCreateDto dto)
    {
        if (id != dto.asset_id)
            return BadRequest("Cannot update this asset item");

        var existingAssetItem = await unit.Repository<AssetItem>().GetByIdAsync(id);

        if (existingAssetItem == null)
            return NotFound("Asset item not found");

        mapper.Map(dto, existingAssetItem);

        existingAssetItem.updated_at = DateTime.Now;
        if (await unit.Complete())
            return NoContent();

        return BadRequest("Problem updating the project");
    }

    //[Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAssetItem(int id)
    {
        var assetItem = await unit.Repository<AssetItem>().GetByIdAsync(id);

        if (assetItem == null) return NotFound();
        assetItem.is_active = false;
        assetItem.updated_at = DateTime.UtcNow;

        unit.Repository<AssetItem>().Update(assetItem);

        if (await unit.Complete())
        {
            return NoContent();
        }

        return BadRequest("Problem deleting asset item");
    }

}
