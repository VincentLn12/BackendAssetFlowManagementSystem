using Core.Interfaces.Specifications.AssetItem;
using Core.Specifications;

public class AssetItemForCountSpecification : BaseSpecification<AssetItem>
{
    public AssetItemForCountSpecification(AssetItemSpecParams specParams)
        : base(x =>
            x.is_active &&
            (
              string.IsNullOrEmpty(specParams.Search) ||
      x.asset_name.ToLower().Contains(specParams.Search)

            ))
    {
    }
}