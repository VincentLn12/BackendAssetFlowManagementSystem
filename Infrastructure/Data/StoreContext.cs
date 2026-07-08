using API.Entities;
using Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class StoreContext(DbContextOptions options) : IdentityDbContext<AppUser>(options)
{
    //สาขา
    public DbSet<Departments> departments { get; set; }
    //คำนำหน้า
    public DbSet<Prefixes> prefixes { get; set; }
    //ตำแหน่ง
    public DbSet<Positions> positions { get; set; }
    //เจ้าหน้าที่
    public DbSet<Staffs> staffs { get; set; }
    //ปีงบประมาณ
    public DbSet<Fiscal_years> fiscal_years { get; set; }
    //หมวดหมู่เงิน
    public DbSet<Fund_categories> fund_categories { get; set; }
    //งบประมาณ
    public DbSet<Budget_sources> budget_sources { get; set; }
    //บริษัท
    public DbSet<Vendors> vendors { get; set; }
    //ประเภทดำเนินการ 
    public DbSet<Operation_types> operation_types { get; set; }
    //ประเภทค่าใช้จ่าย 
    public DbSet<Expense_types> expense_types { get; set; }
    //บันทึกการจัดซื้อจัดจ้าง
    public DbSet<Procurement_records> procurement_records { get; set; }
    //โครงการ
    public DbSet<Projects> projects { get; set; }
    //รายละเอียดการจัดซื้อจัดจ้าง
    public DbSet<HireDetail> hireDetails { get; set; }
    //หมวดหมู่ทรัพย์สิน
    public DbSet<AssetCategory> assetCategories { get; set; }
    //หน่วยนับ
    public DbSet<MaterialUnit> units { get; set; }
    //คุรุภัณฑ์
    public DbSet<AssetItem> assetItems { get; set; }
    //คุรุภัณฑ์ย่อย
    public DbSet<AssetSubItem> assetSubItems { get; set; }
    //การซ่อมแซมคุรุภัณฑ์
    public DbSet<AssetRepair> assetRepairs { get; set; }
    //วิธีการได้มา
    public DbSet<AcquisitionMethod> acquisitionMethods { get; set; }
    //ประวัติการได้มา
    public DbSet<AssetWithdrawal> assetWithdrawals { get; set; }
    //ประเภทการใช้งาน
    public DbSet<AssetUsageType> assetUsageTypes { get; set; }
    //ประวัติการใช้งาน
    public DbSet<AssetSubItemHistory> assetSubItemHistories { get; set; }
    //รายการสินค้า
    public DbSet<MaterialItem> materialItems { get; set; }
    //เบิกจ่ายวัสดุ
    public DbSet<MaterialIssueDetail> materialIssueDetails { get; set; }
    //รับเข้าวัสดุ
    public DbSet<MaterialReceiveDetail> materialReceiveDetails { get; set; }
    //transaction วัสดุ
    public DbSet<MaterialStockCard> materialStockCards { get; set; }
    //ประวัติการเบิกจ่ายวัสดุ
    public DbSet<MaterialWithdrawal> materialWithdrawals { get; set; }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(StoreContext).Assembly);
    }
}
