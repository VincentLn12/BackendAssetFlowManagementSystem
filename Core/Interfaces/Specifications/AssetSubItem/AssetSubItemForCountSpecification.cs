using Core.Interfaces.Specifications.AssetSubItem;
using Core.Specifications;

public class AssetSubItemForCountSpecification : BaseSpecification<AssetSubItem>
{
    public AssetSubItemForCountSpecification(AssetSubItemSpecParams specParams)
        : base(x =>
            x.is_active &&
            (
              string.IsNullOrEmpty(specParams.Search) ||
      x.sub_item_name.ToLower().Contains(specParams.Search)

            ))
    {
    }
}