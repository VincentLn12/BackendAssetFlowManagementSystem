using AutoMapper;
using Core.Interfaces;
using Core.Interfaces.Specifications.Vendors;
using Microsoft.OpenApi.Models;

namespace API.Controllers;

public class VendorsController(IUnitOfWork unit) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Vendors>>> GetVendors([FromQuery] VendorsSpecParams vendorsParams)
    {
        var spec = new VendorsSpecification(vendorsParams);

        return await CreatePagedResult(unit.Repository<Vendors>(), spec,
            vendorsParams.PageIndex, vendorsParams.PageSize);
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<Vendors>> GetVendor(int id)
    {
        var vendor = await unit.Repository<Vendors>().GetByIdAsync(id);

        if (vendor == null) return NotFound();
        return vendor;
    }

    //[Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<Vendors>> CreateVendor(Vendors vendor)
    {
        vendor.is_active = true;
        vendor.updated_at = null;
        unit.Repository<Vendors>().Add(vendor);

        if (await unit.Complete())
        {
            return CreatedAtAction("GetVendor", new { id = vendor.vendor_id }, vendor);
        }
        ;

        return BadRequest("Problem creating vendor");
    }

    //[Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateVendor(int id, Vendors vendor)
    {
        if (id != vendor.vendor_id)
            return BadRequest("Cannot update this vendor");

        var existingVendor = await unit.Repository<Vendors>().GetByIdAsync(id);

        if (existingVendor == null)
            return NotFound("Vendor not found");

        existingVendor.vendor_name = vendor.vendor_name;
        existingVendor.tax_no = vendor.tax_no;
        existingVendor.address = vendor.address;
        existingVendor.phone = vendor.phone;
        existingVendor.contact_name = vendor.contact_name;
        existingVendor.email = vendor.email;
        existingVendor.updated_at = DateTime.Now;
        if (await unit.Complete())
            return NoContent();

        return BadRequest("Problem updating the vendor");
    }

    //[Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteVendor(int id)
    {
        var vendor = await unit.Repository<Vendors>().GetByIdAsync(id);

        if (vendor == null) return NotFound();
        vendor.is_active = false;
        vendor.updated_at = DateTime.UtcNow;

        unit.Repository<Vendors>().Update(vendor);

        if (await unit.Complete())
        {
            return NoContent();
        }

        return BadRequest("Problem deleting vendor");
    }

    private bool VendorExists(int id)
    {
        return unit.Repository<Vendors>().GetByIdAsync(id).Result != null;
    }
}
