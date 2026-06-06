using Core.Interfaces.Specifications.AssetSubItem;
using Core.Specifications;

public class AssetSubItemSpecification : BaseSpecification<AssetSubItem>
{
    public AssetSubItemSpecification(AssetSubItemSpecParams specParams)
        : base(x => x.is_active &&
            (
                string.IsNullOrEmpty(specParams.Search) ||
                x.sub_item_name.ToLower().Contains(specParams.Search.ToLower())
            ) &&
            (
                !specParams.Asset_id.HasValue ||
                x.asset_id == specParams.Asset_id.Value
            )
        )
    {
        //AddInclude(x => x.AssetCategory);
        //AddInclude(x => x.Unit);
        AddInclude(x => x.assetItem);
        AddInclude(x => x.asset_category);
        AddInclude(x => x.materialUnit);

        switch (specParams.Sort)
        {
            case "nameAsc":
                AddOrderBy(x => x.item_no);
                break;

            case "nameDesc":
                AddOrderByDescending(x => x.item_no);
                break;

            case "idDesc":
                AddOrderByDescending(x => x.created_at);
                break;

            default:
                AddOrderBy(x => x.item_no);
                break;
        }

        ApplyPaging(
            specParams.PageSize * (specParams.PageIndex - 1),
            specParams.PageSize
        );
    }
}