using Core.Entities;
using Core.Specifications;

public class MaterialReceiveDetailByProcurementForDeleteSpec
    : BaseSpecification<MaterialReceiveDetail>
{
    public MaterialReceiveDetailByProcurementForDeleteSpec(int procurementRecordId)
        : base(x =>
            x.is_active &&
            x.procurement_record_id == procurementRecordId)
    {
        AddInclude(x => x.MaterialItem!);
    }
}

public class MaterialStockCardByReceiveDetailForDeleteSpec
    : BaseSpecification<MaterialStockCard>
{
    public MaterialStockCardByReceiveDetailForDeleteSpec(int receiveDetailId)
        : base(x =>
            x.receive_detail_id == receiveDetailId &&
            x.transaction_type == "IN" &&
            x.is_active)
    {
    }
}