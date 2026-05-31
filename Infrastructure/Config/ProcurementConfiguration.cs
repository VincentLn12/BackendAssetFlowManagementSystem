using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Config;

public class ProcurementConfiguration : IEntityTypeConfiguration<Procurement_records>
{
    public void Configure(EntityTypeBuilder<Procurement_records> builder)
    {
        builder.HasOne(x => x.fiscal_Years)
            .WithMany()
            .HasForeignKey(x => x.fiscal_year_id)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.operation_Types)
            .WithMany()
            .HasForeignKey(x => x.operation_type_id)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.expense_Types)
            .WithMany()
            .HasForeignKey(x => x.expense_type_id)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.departments)
            .WithMany()
            .HasForeignKey(x => x.department_id)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.vendors)
            .WithMany()
            .HasForeignKey(x => x.vendor_id)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.fund_Categories)
            .WithMany()
            .HasForeignKey(x => x.fund_category_id)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.budget_Sources)
            .WithMany()
            .HasForeignKey(x => x.budget_source_id)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.staffs)
            .WithMany()
            .HasForeignKey(x => x.staff_id)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Property(x => x.total_amount)
            .HasColumnType("decimal(18,2)");
    }
}