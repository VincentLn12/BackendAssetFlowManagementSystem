# AssetFlow Management System Backend Project (AssetFlowManagementSystem)

ระบบ Backend API สำหรับระบบบริหารจัดการพัสดุ ครุภัณฑ์ การจัดซื้อจัดจ้าง และคลังวัสดุ  
พัฒนาด้วย **.NET 9 (ASP.NET Core Web API)** พร้อมระบบยืนยันตัวตน (**ASP.NET Core Identity / JWT**), **Entity Framework Core (SQL Server)**, **AutoMapper**, **Repository & Unit of Work Pattern**, **Specification Pattern** และระบบจัดการไฟล์สื่อ

---

## 📁 โครงสร้างโฟลเดอร์แบบละเอียด (Folder Structure)

```text
AssetFlowManagementSystem/
├── APi/                                          # โครงสร้างโปรเจกต์หลัก (Web API Project)
│   ├── Controllers/                               # [API Controllers] รองรับ HTTP Requests (35 Controllers)
│   │   ├── AccountController.cs                   # ระบบเข้าสู่ระบบ / ลงทะเบียน / โปรไฟล์ผู้ใช้งาน (Authentication)
│   │   ├── AcquisitionMethodController.cs         # จัดการข้อมูลวิธีการได้มาของทรัพย์สิน
│   │   ├── AssetCategoriesController.cs           # จัดการข้อมูลหมวดหมู่ครุภัณฑ์
│   │   ├── AssetItemController.cs                 # จัดการข้อมูลครุภัณฑ์หลัก
│   │   ├── AssetRepairController.cs               # จัดการประวัติและการแจ้งซ่อมแซมครุภัณฑ์
│   │   ├── AssetSubItemHistoryController.cs       # จัดการประวัติการโอนย้าย/ใช้งานครุภัณฑ์ย่อย
│   │   ├── AssetUsageTypeController.cs            # จัดการประเภทการใช้งานครุภัณฑ์
│   │   ├── AssetWithdrawalController.cs           # จัดการระบบเบิกจ่าย/ยืม/ครอบครองครุภัณฑ์
│   │   ├── AssetsubItemController.cs              # จัดการข้อมูลครุภัณฑ์ย่อย (Sub-Items)
│   │   ├── BaseApiController.cs                   # Base Controller หลักสำหรับกำหนด API Route
│   │   ├── Budget_sourcesController.cs            # จัดการข้อมูลแหล่งเงินงบประมาณ
│   │   ├── BuggyController.cs                     # ทดสอบระบบการคืนค่า Error Response
│   │   ├── DepartmentsController.cs               # จัดการข้อมูลแผนก / คณะ / หน่วยงาน
│   │   ├── Expense_typesController.cs             # จัดการข้อมูลประเภทค่าใช้จ่าย
│   │   ├── FallbackController.cs                  # รองรับการส่งต่อ Single Page Application (SPA Fallback)
│   │   ├── Fiscal_yearsController.cs              # จัดการข้อมูลปีงบประมาณ
│   │   ├── Fund_categoriesController.cs           # จัดการข้อมูลหมวดหมู่เงิน
│   │   ├── HiredetailsController.cs               # จัดการรายละเอียดสัญญาจัดจ้างทำของ
│   │   ├── MaterialIssueDetailController.cs       # จัดการรายการเบิกจ่ายวัสดุออกจากคลัง
│   │   ├── MaterialItemController.cs              # จัดการทะเบียนพรรณนาวัสดุสิ้นเปลือง
│   │   ├── MaterialReceiveDetailController.cs     # จัดการรายการตรวจรับวัสดุเข้าคลัง
│   │   ├── MaterialStockCardController.cs         # จัดการข้อมูลสต็อกการ์ดคลังวัสดุแบบ Real-time
│   │   ├── MaterialUnitController.cs              # จัดการหน่วยนับของวัสดุ
│   │   ├── MaterialWithdrawalController.cs        # จัดการเอกสารใบขอเบิกวัสดุ
│   │   ├── Operation_typesController.cs           # จัดการประเภทการดำเนินงานจัดซื้อจัดจ้าง
│   │   ├── PositionsController.cs                 # จัดการตำแหน่งงานของบุคลากร
│   │   ├── PrefixesController.cs                  # จัดการคำนำหน้าชื่อ
│   │   ├── Procurement_recordsController.cs       # จัดการเอกสารบันทึกการจัดซื้อจัดจ้างหลัก
│   │   ├── ProjectsController.cs                  # จัดการข้อมูลโครงการ/แผนงาน
│   │   ├── PublicPortalController.cs              # API สาธารณะสำหรับระบบสแกน QR Code ตรวจสอบครุภัณฑ์
│   │   ├── RolesController.cs                     # จัดการบทบาทและสิทธิ์การใช้งาน (Roles)
│   │   ├── StaffController.cs                     # จัดการข้อมูลบุคลากร / เจ้าหน้าที่
│   │   ├── SystemSettingsController.cs            # จัดการการตั้งค่าระบบ
│   │   ├── UsersController.cs                     # จัดการข้อมูลผู้ใช้งานในระบบ
│   │   └── VendorsController.cs                   # จัดการข้อมูลบริษัท / ผู้ขาย / ผู้รับจ้าง / คู่ค้า
│   │
│   ├── DTOs/                                      # [Data Transfer Objects] รับ-ส่งข้อมูล API (23 DTOs)
│   │   ├── AssetItemDto.cs                        # DTO ข้อมูลครุภัณฑ์หลัก
│   │   ├── AssetRepairDto.cs                      # DTO การแจ้งซ่อมแซมครุภัณฑ์
│   │   ├── AssetSubItemDisposalDto.cs             # DTO การตัดจำหน่ายครุภัณฑ์ย่อย
│   │   ├── AssetSubItemDto.cs                     # DTO ข้อมูลครุภัณฑ์ย่อย
│   │   ├── AssetSubItemHistoryDto.cs              # DTO ประวัติการโอนย้ายครุภัณฑ์ย่อย
│   │   ├── AssetWithdrawalDto.cs                  # DTO การเบิก/ยืมครุภัณฑ์
│   │   ├── HireDetailDto.cs                       # DTO รายละเอียดสัญญาจ้าง
│   │   ├── MaterialIssueDetailDto.cs              # DTO การตัดจ่ายวัสดุ
│   │   ├── MaterialItemDto.cs                     # DTO ทะเบียนพรรณนาวัสดุ
│   │   ├── MaterialReceiveDetailDto.cs            # DTO การรับวัสดุเข้าคลัง
│   │   ├── MaterialStockCardDto.cs                # DTO รายงานสต็อกการ์ด
│   │   ├── MaterialWithdrawal.cs                  # DTO เอกสารใบขอเบิกวัสดุ
│   │   ├── ProcurementAssetFullCreateDto.cs       # DTO สร้างจัดซื้อพร้อมลงทะเบียนครุภัณฑ์
│   │   ├── ProcurementHireFullCreateDto.cs        # DTO สร้างจัดซื้อพร้อมสัญญาจ้าง
│   │   ├── ProcurementMaterialFullCreateDto.cs    # DTO สร้างจัดซื้อพร้อมรับเข้าวัสดุ
│   │   ├── ProcurementRecordStatusHistoryDto.cs   # DTO ประวัติสถานะจัดซื้อจัดจ้าง
│   │   ├── Procurement_recordsDto.cs              # DTO บันทึกการจัดซื้อจัดจ้าง
│   │   ├── ProjectDto.cs                          # DTO ข้อมูลโครงการ
│   │   ├── PublicPortalDtos.cs                    # DTO ข้อมูลสาธารณะสแกน QR Code
│   │   ├── RegisterDto.cs                         # DTO ลงทะเบียนผู้ใช้งานใหม่
│   │   ├── RoleDto.cs                             # DTO จัดการสิทธิ์และบทบาท
│   │   ├── StaffDto.cs                            # DTO ข้อมูลบุคลากร
│   │   └── UserDto.cs                             # DTO ข้อมูลผู้ใช้งานและ Token
│   │
│   ├── Errors/                                    # [Error Handling] รูปแบบ HTTP Response สำหรับ Error
│   │   ├── ApiException.cs                        # Response Exception สำหรับ Developer/Production
│   │   ├── ApiResponse.cs                         # Standard API Response (StatusCode & Message)
│   │   └── ApiValidationErrorResponse.cs          # Response สำหรับ Model State Validation Failed
│   │
│   ├── Extensions/                                # [Service Extensions]
│   │   ├── ApplicationServicesExtensions.cs       # ลงทะเบียน Repositories, AutoMapper, CORS, DbContext
│   │   └── IdentityServiceExtensions.cs           # ลงทะเบียน ASP.NET Core Identity & JWT Configuration
│   │
│   ├── Helper/                                    # [Object Mapping Profiles]
│   │   └── MappingProfiles.cs                     # AutoMapper Configurations (Entities <-> DTOs)
│   │
│   ├── Middleware/                                # [Custom Middlewares]
│   │   └── ExceptionMiddleware.cs                 # Middleware ดักจับ Unhandled Exceptions
│   │
│   ├── RequestHelpers/                            # [Query Helpers]
│   │   └── Pagination.cs                          # Class Generic สำหรับส่งคืนผลลัพธ์แบบแบ่งหน้า (Paging)
│   │
│   ├── Program.cs                                 # จุดเริ่มต้นโปรเจกต์ (DI Setup, Pipeline, Middleware)
│   └── APi.csproj                                 # ไฟล์คอนฟิกและ Dependencies ของ Web API Project
│
├── Core/                                          # [Domain Layer] Center Layer ของ Clean Architecture
│   ├── Entities/                                  # [Domain Models / Entity Definitions] (32 Entities)
│   │   ├── 📘 [Master Data Models]
│   │   │   ├── AcquisitionMethod.cs               # โมเดลวิธีการได้มาของทรัพย์สิน
│   │   │   ├── AppUser.cs                         # โมเดลผู้ใช้งานระบบ (สืบทอดจาก IdentityUser)
│   │   │   ├── AssetCategory.cs                   # โมเดลหมวดหมู่ครุภัณฑ์
│   │   │   ├── AssetItem.cs                       # โมเดลครุภัณฑ์หลัก
│   │   │   ├── AssetSubItem.cs                    # โมเดลครุภัณฑ์ย่อย
│   │   │   ├── asset_usage_types.cs               # โมเดลประเภทการใช้งานครุภัณฑ์
│   │   │   ├── Budget_sources.cs                  # โมเดลแหล่งเงินงบประมาณ
│   │   │   ├── Departments.cs                     # โมเดลแผนก/คณะ/หน่วยงาน
│   │   │   ├── Expense_types.cs                   # โมเดลประเภทค่าใช้จ่าย
│   │   │   ├── Fiscal_years.cs                    # โมเดลปีงบประมาณ
│   │   │   ├── Fund_categories.cs                 # โมเดลหมวดหมู่เงิน
│   │   │   ├── MaterialItem.cs                    # โมเดลทะเบียนพรรณนาวัสดุ
│   │   │   ├── MaterialUnit.cs                    # โมเดลหน่วยนับวัสดุ
│   │   │   ├── Operation_types.cs                 # โมเดลประเภทการดำเนินงานจัดซื้อ
│   │   │   ├── Positions.cs                       # โมเดลตำแหน่งงาน
│   │   │   ├── Prefixes.cs                        # โมเดลคำนำหน้าชื่อ
│   │   │   ├── Projects.cs                        # โมเดลโครงการ/แผนงาน
│   │   │   ├── Staffs.cs                          # โมเดลข้อมูลบุคลากร/เจ้าหน้าที่
│   │   │   └── Vendors.cs                         # โมเดลบริษัท/คู่ค้า/ผู้รับจ้าง
│   │   │
│   │   ├── 🔄 [Transaction Data Models]
│   │   │   ├── AssetRepair.cs                     # โมเดลการแจ้งซ่อมและประวัติซ่อมแซมครุภัณฑ์
│   │   │   ├── AssetSubItemDisposal.cs            # โมเดลการตัดจำหน่ายครุภัณฑ์ย่อย
│   │   │   ├── AssetSubItemHistory.cs             # โมเดลประวัติการย้าย/ใช้งานครุภัณฑ์ย่อย
│   │   │   ├── AssetWithdrawal.cs                 # โมเดลการเบิก/ยืม/ครอบครองครุภัณฑ์
│   │   │   ├── HireDetail.cs                      # โมเดลรายละเอียดสัญญาจัดจ้างทำของ
│   │   │   ├── MaterialIssueDetail.cs             # โมเดลรายการจ่ายวัสดุออกจากคลัง
│   │   │   ├── MaterialReceiveDetail.cs           # โมเดลรายการรับวัสดุเข้าคลัง
│   │   │   ├── MaterialStockCard.cs               # โมเดลสต็อกการ์ดคลังวัสดุ (Inventory Ledger)
│   │   │   ├── MaterialWithdrawal.cs              # โมเดลเอกสารใบขอเบิกวัสดุ
│   │   │   ├── ProcurementRecordStatusHistory.cs  # โมเดลประวัติการเปลี่ยนสถานะจัดซื้อจัดจ้าง
│   │   │   └── Procurement_records.cs             # โมเดลบันทึกการจัดซื้อจัดจ้างหลัก
│   │   │
│   │   └── ⚙️ [System & Config Models]
│   │       ├── BaseEntity.cs                      # Class พื้นฐานสำหรับ Entities (มี Id)
│   │       └── SystemSetting.cs                   # โมเดลการตั้งค่าระบบ
│   │
│   ├── Interfaces/                                # [Service Interfaces & Contracts]
│   │   ├── IGenericRepository.cs                  # Interface สำหรับ Generic Repository CRUD
│   │   ├── ISpecification.cs                      # Interface สำหรับ Specification Pattern Query
│   │   ├── IUnitOfWork.cs                         # Interface สำหรับ Unit of Work Transaction
│   │   └── Specifications/                        # Implementation ของ Specification Pattern
│   │       ├── BaseSpecification.cs               # Core Specification Logic
│   │       └── PagingParams.cs                    # Standard Parameters สำหรับการแบ่งหน้า
│   │
│   └── Core.csproj                                # ไฟล์คอนฟิก Core Class Library
│
└── Infrastructure/                                # [Data Access & Infrastructure Layer]
    ├── Config/                                    # [EF Core Configurations] การกำหนด Fluent API
    ├── Data/                                      # [Data Logic & Repository Implementation]
    │   ├── GenericRepository.cs                   # Generic Repository Implementation
    │   ├── SpecificationEvaluator.cs              # Evaluator แปลง Specification เป็น LINQ Query
    │   ├── StoreContext.cs                        # EF Core DbContext Class (IdentityDbContext)
    │   ├── StoreContextSeed.cs                    # Logic การลงข้อมูลเริ่มต้น (Data Seeding)
    │   ├── UnitOfWork.cs                          # Unit of Work Implementation
    │   └── SeedData/                              # โฟลเดอร์จัดเก็บไฟล์ Seed Data JSON
    │
    ├── Migrations/                                # [EF Core Migrations] ประวัติการเปลี่ยนโครงสร้าง DB
    ├── Services/                                  # [Infrastructure Services]
    │   └── FileService.cs                         # บริการจัดการอัปโหลด/บันทึก/ลบไฟล์แนบ
    └── Infrastructure.csproj                      # ไฟล์คอนฟิก Infrastructure Class Library
```

---

## 🗄️ สรุปโครงสร้างฐานข้อมูล (Database Schema & Tables)

โครงสร้างตารางฐานข้อมูลอ้างอิงจาก Models Class Definition ในระบบ (Entity Framework Core Mapping)

### 1) AspNetUsers (Model: AppUser)
ตารางจัดการผู้ใช้งานระบบ (สืบทอดจาก IdentityUser)

- **Primary Key:** `Id` (string / nvarchar(450))
- **Fields:** `UserName`, `Email`, `PasswordHash`, `PhoneNumber`, `SecurityStamp`, `DisplayName`
- **Relationships:** เชื่อมโยงกับตาราง `Staffs` และจัดการสิทธิ์ผ่าน `AspNetUserRoles`

### 2) Departments (Model: Departments)
ตารางข้อมูลแผนก / คณะ / หน่วยงานภายในองค์กร

- **Primary Key:** `department_id` (int, Identity)
- **Fields:** `department_name` (nvarchar(max))
- **Relationships:** One-to-Many กับ `Staffs`, `Procurement_records`, `MaterialStockCards`

### 3) Positions (Model: Positions)
ตารางตำแหน่งงานของบุคลากร

- **Primary Key:** `position_id` (int, Identity)
- **Fields:** `position_name` (nvarchar(max))
- **Relationships:** One-to-Many กับ `Staffs`

### 4) Prefixes (Model: Prefixes)
ตารางคำนำหน้าชื่อ

- **Primary Key:** `prefix_id` (int, Identity)
- **Fields:** `prefix_name` (nvarchar(max)), `short_name` (nvarchar(max))
- **Relationships:** One-to-Many กับ `Staffs`

### 5) Staffs (Model: Staffs)
ตารางทะเบียนข้อมูลบุคลากร / เจ้าหน้าที่

- **Primary Key:** `staff_id` (int, Identity)
- **Foreign Keys:**
  - `prefix_id` -> `Prefixes.prefix_id`
  - `position_id` -> `Positions.position_id`
  - `department_id` -> `Departments.department_id`
- **Fields:** `first_name` (nvarchar(max)), `last_name` (nvarchar(max)), `email` (nvarchar(max)), `phone_number` (nvarchar(max))
- **Relationships:** One-to-Many กับ `Procurement_records`, `AssetWithdrawals`, `AssetRepairs`, `MaterialIssueDetails`, `MaterialWithdrawals`

### 6) Vendors (Model: Vendors)
ตารางข้อมูลบริษัท / ผู้ขาย / ผู้รับจ้าง / คู่ค้า

- **Primary Key:** `vendor_id` (int, Identity)
- **Fields:** `vendor_name` (nvarchar(max)), `tax_id` (nvarchar(max)), `address` (nvarchar(max)), `phone_number` (nvarchar(max)), `contact_name` (nvarchar(max))
- **Relationships:** One-to-Many กับ `Procurement_records`

### 7) Fiscal_years (Model: Fiscal_years)
ตารางรอบปีงบประมาณ

- **Primary Key:** `fiscal_year_id` (int, Identity)
- **Fields:** `year_name` (int), `start_date` (datetime), `end_date` (datetime), `is_active` (bit)
- **Relationships:** One-to-Many กับ `Procurement_records`, `MaterialStockCards`

### 8) Fund_categories (Model: Fund_categories)
ตารางหมวดหมู่เงินงบประมาณ

- **Primary Key:** `fund_category_id` (int, Identity)
- **Fields:** `fund_category_name` (nvarchar(max))
- **Relationships:** One-to-Many กับ `Procurement_records`

### 9) Budget_sources (Model: Budget_sources)
ตารางแหล่งเงินงบประมาณ

- **Primary Key:** `budget_source_id` (int, Identity)
- **Fields:** `budget_source_name` (nvarchar(max))
- **Relationships:** One-to-Many กับ `Procurement_records`

### 10) Expense_types (Model: Expense_types)
ตารางประเภทค่าใช้จ่าย

- **Primary Key:** `expense_type_id` (int, Identity)
- **Fields:** `expense_type_name` (nvarchar(max))
- **Relationships:** One-to-Many กับ `Procurement_records`

### 11) Operation_types (Model: Operation_types)
ตารางประเภทการดำเนินงานจัดซื้อจัดจ้าง

- **Primary Key:** `operation_type_id` (int, Identity)
- **Fields:** `operation_type_name` (nvarchar(max))
- **Relationships:** One-to-Many กับ `Procurement_records`

### 12) Projects (Model: Projects)
ตารางรายชื่อโครงการ / แผนงาน

- **Primary Key:** `project_id` (int, Identity)
- **Fields:** `project_name` (nvarchar(max)), `project_code` (nvarchar(max)), `description` (nvarchar(max))
- **Relationships:** One-to-Many กับ `Procurement_records`

### 13) AssetCategory (Model: AssetCategory)
ตารางหมวดหมู่ครุภัณฑ์

- **Primary Key:** `asset_category_id` (int, Identity)
- **Fields:** `category_name` (nvarchar(max)), `category_code` (nvarchar(max))
- **Relationships:** One-to-Many กับ `AssetItems`

### 14) AcquisitionMethod (Model: AcquisitionMethod)
ตารางวิธีการได้มาของทรัพย์สิน

- **Primary Key:** `acquisition_method_id` (int, Identity)
- **Fields:** `method_name` (nvarchar(max))
- **Relationships:** One-to-Many กับ `AssetItems`

### 15) asset_usage_types (Model: AssetUsageType)
ตารางประเภทการใช้งานครุภัณฑ์

- **Primary Key:** `asset_usage_type_id` (int, Identity)
- **Fields:** `usage_type_name` (nvarchar(max))

### 16) MaterialUnit (Model: MaterialUnit)
ตารางหน่วยนับพรรณนาวัสดุ

- **Primary Key:** `unit_id` (int, Identity)
- **Fields:** `unit_name` (nvarchar(max))
- **Relationships:** One-to-Many กับ `MaterialItems`

### 17) AssetItem (Model: AssetItem)
ตารางทะเบียนครุภัณฑ์หลัก

- **Primary Key:** `asset_item_id` (int, Identity)
- **Foreign Keys:**
  - `procurement_record_id` -> `Procurement_records.procurement_record_id`
  - `asset_category_id` -> `AssetCategory.asset_category_id`
  - `acquisition_method_id` -> `AcquisitionMethod.acquisition_method_id`
  - `department_id` -> `Departments.department_id`
- **Fields:** `asset_code` (nvarchar(max)), `asset_name` (nvarchar(max)), `price` (decimal(18,2)), `useful_life` (int), `received_date` (datetime)
- **Relationships:** One-to-Many กับ `AssetSubItems`

### 18) AssetSubItem (Model: AssetSubItem)
ตารางทะเบียนครุภัณฑ์ย่อย (Sub-Items / Serial Items)

- **Primary Key:** `asset_sub_item_id` (int, Identity)
- **Foreign Key:** `asset_item_id` -> `AssetItem.asset_item_id`
- **Fields:** `sub_item_code` (nvarchar(max)), `serial_number` (nvarchar(max)), `status` (nvarchar(max)), `storage_location` (nvarchar(max))
- **Relationships:** One-to-Many กับ `AssetSubItemHistories`, `AssetSubItemDisposals`

### 19) MaterialItem (Model: MaterialItem)
ตารางทะเบียนพรรณนาวัสดุสิ้นเปลือง / สินค้าในคลัง

- **Primary Key:** `material_item_id` (int, Identity)
- **Foreign Key:** `unit_id` -> `MaterialUnit.unit_id`
- **Fields:** `material_code` (nvarchar(max)), `material_name` (nvarchar(max)), `unit_price` (decimal(18,2)), `min_quantity` (decimal(18,2)), `max_quantity` (decimal(18,2))
- **Relationships:** One-to-Many กับ `MaterialReceiveDetails`, `MaterialIssueDetails`, `MaterialStockCards`

### 20) Procurement_records (Model: Procurement_records)
ตารางเอกสารบันทึกการจัดซื้อจัดจ้างหลัก

- **Primary Key:** `procurement_record_id` (int, Identity)
- **Foreign Keys:**
  - `fiscal_year_id` -> `Fiscal_years.fiscal_year_id`
  - `operation_type_id` -> `Operation_types.operation_type_id`
  - `expense_type_id` -> `Expense_types.expense_type_id`
  - `department_id` -> `Departments.department_id`
  - `vendor_id` -> `Vendors.vendor_id`
  - `fund_category_id` -> `Fund_categories.fund_category_id`
  - `budget_source_id` -> `Budget_sources.budget_source_id`
  - `staff_id` -> `Staffs.staff_id`
  - `project_id` -> `Projects.project_id`
- **Fields:** `document_no` (nvarchar(max)), `document_date` (datetime), `inspection_date` (datetime), `total_amount` (decimal(18,2)), `amount_text` (nvarchar(max)), `status` (nvarchar(max)), `reference_no` (nvarchar(max)), `attachment_file_path` (nvarchar(max))
- **Relationships:** One-to-Many กับ `HireDetails`, `ProcurementRecordStatusHistories`, `AssetItems`, `MaterialReceiveDetails`, `AssetWithdrawals`

### 21) HireDetail (Model: HireDetail)
ตารางรายละเอียดสัญญาจัดจ้างทำของ

- **Primary Key:** `hire_detail_id` (int, Identity)
- **Foreign Key:** `procurement_record_id` -> `Procurement_records.procurement_record_id`
- **Fields:** `contract_no` (nvarchar(max)), `start_date` (datetime), `end_date` (datetime), `contract_amount` (decimal(18,2)), `work_description` (nvarchar(max))

### 22) ProcurementRecordStatusHistory (Model: ProcurementRecordStatusHistory)
ตารางบันทึกประวัติการเปลี่ยนสถานะจัดซื้อจัดจ้าง

- **Primary Key:** `status_history_id` (int, Identity)
- **Foreign Key:** `procurement_record_id` -> `Procurement_records.procurement_record_id`
- **Fields:** `previous_status` (nvarchar(max)), `new_status` (nvarchar(max)), `changed_at` (datetime), `changed_by` (nvarchar(max)), `remarks` (nvarchar(max))

### 23) AssetWithdrawal (Model: AssetWithdrawal)
ตารางประวัติการเบิก / ยืม / ผู้ครอบครองครุภัณฑ์

- **Primary Key:** `procurement_withdrawal_id` (int, Identity)
- **Foreign Keys:**
  - `procurement_record_id` -> `Procurement_records.procurement_record_id`
  - `staff_id` -> `Staffs.staff_id`
- **Fields:** `withdrawal_document_no` (nvarchar(100)), `withdrawal_date` (date), `end_date` (date), `end_reason` (nvarchar(50)), `storage_location` (nvarchar(255)), `purpose` (nvarchar(500)), `remark` (nvarchar(max))
- **Relationships:** One-to-Many กับ `AssetRepairs`

### 24) AssetRepair (Model: AssetRepair)
ตารางบันทึกการส่งซ่อมแซมและค่าใช้จ่ายซ่อมแซมครุภัณฑ์

- **Primary Key:** `asset_repair_id` (int, Identity)
- **Foreign Keys:**
  - `procurement_withdrawal_id` -> `AssetWithdrawal.procurement_withdrawal_id`
  - `staff_id` -> `Staffs.staff_id`
- **Fields:** `repair_document_no` (nvarchar(max)), `repair_date` (datetime), `problem_description` (nvarchar(max)), `repair_description` (nvarchar(max)), `repair_shop_name` (nvarchar(max)), `repair_cost` (decimal(18,2)), `status` (nvarchar(max))

### 25) AssetSubItemHistory (Model: AssetSubItemHistory)
ตารางประวัติการเคลื่อนย้าย / การใช้งานครุภัณฑ์ย่อย

- **Primary Key:** `sub_item_history_id` (int, Identity)
- **Foreign Keys:**
  - `asset_sub_item_id` -> `AssetSubItem.asset_sub_item_id`
  - `staff_id` -> `Staffs.staff_id`
  - `department_id` -> `Departments.department_id`
- **Fields:** `action_type` (nvarchar(max)), `action_date` (datetime), `location` (nvarchar(max)), `remarks` (nvarchar(max))

### 26) AssetSubItemDisposal (Model: AssetSubItemDisposal)
ตารางบันทึกการตัดจำหน่ายครุภัณฑ์ย่อยออกจากบัญชี

- **Primary Key:** `sub_item_disposal_id` (int, Identity)
- **Foreign Key:** `asset_sub_item_id` -> `AssetSubItem.asset_sub_item_id`
- **Fields:** `disposal_date` (datetime), `disposal_method` (nvarchar(200)), `disposal_reason` (nvarchar(1000)), `approved_by` (nvarchar(200)), `quantity_disposed` (decimal(18,2)), `notes` (nvarchar(1000))

### 27) MaterialWithdrawal (Model: MaterialWithdrawal)
ตารางเอกสารใบขอเบิกพรรณนาวัสดุ

- **Primary Key:** `material_withdrawal_id` (int, Identity)
- **Foreign Keys:**
  - `staff_id` -> `Staffs.staff_id`
  - `procurement_record_id` -> `Procurement_records.procurement_record_id`
- **Fields:** `material_receive_id` (nvarchar(max)), `receive_document_no` (nvarchar(max)), `withdrawal_document_no` (nvarchar(max)), `remark` (nvarchar(max))

### 28) MaterialReceiveDetail (Model: MaterialReceiveDetail)
ตารางรายการตรวจรับวัสดุเข้าคลัง

- **Primary Key:** `receive_detail_id` (int, Identity)
- **Foreign Keys:**
  - `procurement_record_id` -> `Procurement_records.procurement_record_id`
  - `material_item_id` -> `MaterialItem.material_item_id`
- **Fields:** `item_no` (int), `quantity` (decimal(18,2)), `unit_price` (decimal(18,2)), `total_amount` (decimal(18,2)), `operation_reason` (nvarchar(max))
- **Relationships:** One-to-Many กับ `MaterialStockCards`

### 29) MaterialIssueDetail (Model: MaterialIssueDetail)
ตารางรายการตัดจ่ายวัสดุออกจากคลัง

- **Primary Key:** `issue_detail_id` (int, Identity)
- **Foreign Keys:**
  - `procurement_record_id` -> `Procurement_records.procurement_record_id`
  - `material_item_id` -> `MaterialItem.material_item_id`
  - `staff_id` -> `Staffs.staff_id`
- **Fields:** `issue_date` (date), `quantity` (decimal(18,2)), `unit_price` (decimal(18,2)), `total_amount` (decimal(18,2)), `remark` (nvarchar(max))
- **Relationships:** One-to-Many กับ `MaterialStockCards`

### 30) MaterialStockCard (Model: MaterialStockCard)
ตารางสต็อกการ์ดบันทึก Transaction และยอดยกไปคงเหลือ

- **Primary Key:** `stock_card_id` (int, Identity)
- **Foreign Keys:**
  - `material_item_id` -> `MaterialItem.material_item_id`
  - `receive_detail_id` -> `MaterialReceiveDetail.receive_detail_id`
  - `issue_detail_id` -> `MaterialIssueDetail.issue_detail_id`
  - `fiscal_year_id` -> `Fiscal_years.fiscal_year_id`
  - `department_id` -> `Departments.department_id`
- **Fields:** `transaction_date` (date), `transaction_type` (nvarchar(20)), `reference_document_no` (nvarchar(100)), `quantity_in` (decimal(18,2)), `quantity_out` (decimal(18,2)), `balance_qty` (decimal(18,2)), `unit_price` (decimal(18,2)), `total_amount` (decimal(18,2))

### 31) SystemSetting (Model: SystemSetting)
ตารางการตั้งค่าระบบ

- **Primary Key:** `id` (int, Identity)
- **Fields:** `system_name` (nvarchar(max)), `system_code` (nvarchar(max)), `setting_value` (nvarchar(max))

---

## 📌 การจำแนกประเภทตาราง (Classification of Database Tables)

เพื่อความเข้าใจในสถาปัตยกรรมข้อมูล ตารางทั้งหมด 31 ตารางสามารถแบ่งออกตามลักษณะการใช้งานได้ดังนี้

### 1) 📘 Master File / Master Data (ตารางอ้างอิงหลัก)
ข้อมูลอ้างอิงพื้นฐานของระบบที่มีการเปลี่ยนแปลงน้อย ใช้สำหรับอ้างอิงในตารางอื่น ๆ

- AspNetUsers (ผู้ใช้งานระบบ)
- Departments (หน่วยงาน/แผนก/คณะ)
- Positions (ตำแหน่งงาน)
- Prefixes (คำนำหน้าชื่อ)
- Staffs (ข้อมูลบุคลากร/เจ้าหน้าที่)
- Vendors (บริษัท/ผู้ขาย/ผู้รับจ้าง/คู่ค้า)
- Fiscal_years (ปีงบประมาณ)
- Fund_categories (หมวดหมู่เงินงบประมาณ)
- Budget_sources (แหล่งเงินงบประมาณ)
- Expense_types (ประเภทค่าใช้จ่าย)
- Operation_types (ประเภทการดำเนินงานจัดซื้อจัดจ้าง)
- Projects (โครงการ/แผนงาน)
- AssetCategories (หมวดหมู่ครุภัณฑ์)
- AcquisitionMethods (วิธีการได้มาของทรัพย์สิน)
- AssetUsageTypes (ประเภทการใช้งานครุภัณฑ์)
- MaterialUnits (หน่วยนับพรรณนาวัสดุ)
- AssetItems (ทะเบียนครุภัณฑ์หลัก)
- AssetSubItems (ทะเบียนครุภัณฑ์ย่อย)
- MaterialItems (ทะเบียนพรรณนาวัสดุสิ้นเปลือง)

### 2) 🔄 Transaction Data (ตารางรายการ / กิจกรรม)
ข้อมูลที่เกิดจากการสมัคร ทำรายการ หรือประมวลผลตามกิจกรรม/ช่วงเวลา

- Procurement_records (บันทึกเอกสารการจัดซื้อจัดจ้างหลัก)
- HireDetails (รายละเอียดสัญญาจัดจ้างทำของ)
- ProcurementRecordStatusHistories (ประวัติการเปลี่ยนสถานะเอกสารจัดซื้อ)
- AssetWithdrawals (ประวัติการเบิก/ยืม/ครอบครองครุภัณฑ์)
- AssetRepairs (ประวัติการส่งซ่อมแซมและค่าใช้จ่ายครุภัณฑ์)
- AssetSubItemHistories (ประวัติการเคลื่อนย้าย/ใช้งานครุภัณฑ์ย่อย)
- AssetSubItemDisposals (การตัดจำหน่ายครุภัณฑ์ย่อยออกจากบัญชี)
- MaterialWithdrawals (เอกสารใบขอเบิกพรรณนาวัสดุ)
- MaterialReceiveDetails (รายการตรวจรับวัสดุเข้าคลัง)
- MaterialIssueDetails (รายการตัดจ่ายวัสดุออกจากคลัง)
- MaterialStockCards (สต็อกการ์ดคลังวัสดุคำนวณยอดยกไปคงเหลือแบบ Real-time)

### 3) ⚙️ System & Config Data (ตารางตั้งค่าระบบ)
ข้อมูลการตั้งค่า ค่าคอนฟิก และสิทธิ์ในระบบ

- SystemSettings (ตั้งค่าระบบและคอนฟิกพื้นฐาน)
- AspNetRoles / AspNetUserRoles (ตารางจัดการบทบาทและสิทธิ์การใช้งาน)

---

## 🛠️ เทคโนโลยีที่ใช้ (Tech Stack)

- **Framework:** .NET 9 (ASP.NET Core Web API)
- **Database:** SQL Server
- **ORM:** Entity Framework Core 9.0 (Microsoft.EntityFrameworkCore.SqlServer)
- **Authentication:** ASP.NET Core Identity & JWT Bearer Token
- **Object Mapper:** AutoMapper 16.1
- **Architecture:** Clean Architecture (Onion), Generic Repository Pattern, Unit of Work Pattern, Specification Pattern
- **API Documentation:** Swagger / OpenAPI (Swashbuckle.AspNetCore)
- **Payment Integration:** Stripe.net
