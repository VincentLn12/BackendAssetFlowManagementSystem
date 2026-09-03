# 📦 AssetFlow Management System - Backend API (.NET 9)

> **ระบบ Backend API สำหรับระบบบริหารจัดการพัสดุ ครุภัณฑ์ การจัดซื้อจัดจ้าง และคลังวัสดุ**  
> พัฒนาด้วย **.NET 9 (ASP.NET Core Web API)**, **Entity Framework Core 9 (SQL Server)**,  
> **ASP.NET Core Identity & JWT**, **AutoMapper**, **Clean Architecture**  
> (Repository & Unit of Work Pattern, Specification Pattern) และ **File Upload Service**

---

## 📚 สารบัญ
- [ภาพรวมระบบ](#-ภาพรวมระบบ)
- [โครงสร้างโปรเจกต์](#-โครงสร้างโปรเจกต์-folder-structure)
- [สรุปฐานข้อมูล](#-สรุปโครงสร้างฐานข้อมูล-database-schema--tables)
- [การจำแนกประเภทตาราง](#-การจำแนกประเภทตาราง-classification-of-database-tables)
- [เทคโนโลยีที่ใช้](#-เทคโนโลยีที่ใช้-backend-tech-stack--libraries)
- [วิธีติดตั้งและเริ่มใช้งาน](#-ขั้นตอนการติดตั้งและเริ่มใช้งาน-getting-started)

---

## 🔎 ภาพรวมระบบ

Backend นี้รองรับงานหลัก:
- จัดการครุภัณฑ์หลัก/ย่อย
- จัดการคลังวัสดุ (รับเข้า/เบิกจ่าย/Stock Card)
- จัดการจัดซื้อจัดจ้างและสถานะเอกสาร
- จัดการบุคลากร หน่วยงาน ผู้ขาย โครงการ และงบประมาณ
- ระบบผู้ใช้งาน + Role/Permission (ASP.NET Identity + JWT)
- API สาธารณะสำหรับตรวจสอบครุภัณฑ์ผ่าน QR

---

## 📁 โครงสร้างโปรเจกต์ (Folder Structure)


```text
AssetFlowManagementSystem/
├── APi/                                          # โครงสร้างโปรเจกต์หลัก (Web API Project)
│   ├── Controllers/                               # [API Controllers] รองรับ HTTP Requests (35 Controllers)
│   │   ├── AccountController.cs                   # ระบบเข้าสู่ระบบ / ลงทะเบียน / โปรไฟล์ผู้ใช้งาน
│   │   ├── AcquisitionMethodController.cs         # จัดการข้อมูลวิธีการได้มาของทรัพย์สิน
│   │   ├── AssetCategoriesController.cs           # จัดการข้อมูลหมวดหมู่ครุภัณฑ์
│   │   ├── AssetItemController.cs                 # จัดการข้อมูลครุภัณฑ์หลัก
│   │   ├── AssetRepairController.cs               # จัดการประวัติและการแจ้งซ่อมแซมครุภัณฑ์
│   │   ├── AssetSubItemHistoryController.cs      # จัดการประวัติการโอนย้าย/ใช้งานครุภัณฑ์ย่อย
│   │   ├── AssetUsageTypeController.cs            # จัดการประเภทการใช้งานครุภัณฑ์
│   │   ├── AssetWithdrawalController.cs           # จัดการระบบเบิกจ่าย/ยืม/ครอบครองครุภัณฑ์
│   │   ├── AssetsubItemController.cs              # จัดการข้อมูลครุภัณฑ์ย่อย (Sub-Items)
│   │   ├── BaseApiController.cs                  # Base Controller หลักสำหรับกำหนด API Route
│   │   ├── Budget_sourcesController.cs            # จัดการข้อมูลแหล่งเงินงบประมาณ
│   │   ├── BuggyController.cs                    # ทดสอบระบบการคืนค่า Error Response
│   │   ├── DepartmentsController.cs              # จัดการข้อมูลแผนก / คณะ / หน่วยงาน
│   │   ├── Expense_typesController.cs            # จัดการข้อมูลประเภทค่าใช้จ่าย
│   │   ├── FallbackController.cs                 # รองรับการส่งต่อ Single Page Application (SPA Fallback)
│   │   ├── Fiscal_yearsController.cs             # จัดการข้อมูลปีงบประมาณ
│   │   ├── Fund_categoriesController.cs          # จัดการข้อมูลหมวดหมู่เงิน
│   │   ├── HiredetailsController.cs              # จัดการรายละเอียดสัญญาจัดจ้างทำของ
│   │   ├── MaterialIssueDetailController.cs      # จัดการรายการเบิกจ่ายวัสดุออกจากคลัง
│   │   ├── MaterialItemController.cs              # จัดการทะเบียนพรรณนาวัสดุสิ้นเปลือง
│   │   ├── MaterialReceiveDetailController.cs    # จัดการรายการตรวจรับวัสดุเข้าคลัง
│   │   ├── MaterialStockCardController.cs        # จัดการข้อมูลสต็อกการ์ดคลังวัสดุแบบ Real-time
│   │   ├── MaterialUnitController.cs             # จัดการหน่วยนับของวัสดุ
│   │   ├── MaterialWithdrawalController.cs       # จัดการเอกสารใบขอเบิกวัสดุ
│   │   ├── Operation_typesController.cs          # จัดการประเภทการดำเนินงานจัดซื้อจัดจ้าง
│   │   ├── PositionsController.cs                # จัดการตำแหน่งงานของบุคลากร
│   │   ├── PrefixesController.cs                 # จัดการคำนำหน้าชื่อ
│   │   ├── Procurement_recordsController.cs      # จัดการเอกสารบันทึกการจัดซื้อจัดจ้างหลัก
│   │   ├── ProjectsController.cs                 # จัดการข้อมูลโครงการ/แผนงาน
│   │   ├── PublicPortalController.cs              # API สาธารณะสำหรับระบบสแกน QR Code ตรวจสอบครุภัณฑ์
│   │   ├── RolesController.cs                    # จัดการบทบาทและสิทธิ์การใช้งาน (Roles)
│   │   ├── StaffController.cs                    # จัดการข้อมูลบุคลากร / เจ้าหน้าที่
│   │   ├── SystemSettingsController.cs            # จัดการการตั้งค่าระบบ
│   │   ├── UsersController.cs                    # จัดการข้อมูลผู้ใช้งานในระบบ
│   │   └── VendorsController.cs                  # จัดการข้อมูลบริษัท / ผู้ขาย / ผู้รับจ้าง / คู่ค้า
│   │
│   ├── DTOs/                                     # [Data Transfer Objects] รับ-ส่งข้อมูล API (23 DTOs)
│   │   ├── AssetItemDto.cs                       # DTO ข้อมูลครุภัณฑ์หลัก
│   │   ├── AssetRepairDto.cs                     # DTO การแจ้งซ่อมแซมครุภัณฑ์
│   │   ├── AssetSubItemDisposalDto.cs            # DTO การตัดจำหน่ายครุภัณฑ์ย่อย
│   │   ├── AssetSubItemDto.cs                    # DTO ข้อมูลครุภัณฑ์ย่อย
│   │   ├── AssetSubItemHistoryDto.cs             # DTO ประวัติการโอนย้ายครุภัณฑ์ย่อย
│   │   ├── AssetWithdrawalDto.cs                 # DTO การเบิก/ยืมครุภัณฑ์
│   │   ├── HireDetailDto.cs                      # DTO รายละเอียดสัญญาจ้าง
│   │   ├── MaterialIssueDetailDto.cs             # DTO การตัดจ่ายวัสดุ
│   │   ├── MaterialItemDto.cs                    # DTO ทะเบียนพรรณนาวัสดุ
│   │   ├── MaterialReceiveDetailDto.cs           # DTO การรับวัสดุเข้าคลัง
│   │   ├── MaterialStockCardDto.cs               # DTO รายงานสต็อกการ์ด
│   │   ├── MaterialWithdrawal.cs                 # DTO เอกสารใบขอเบิกวัสดุ
│   │   ├── ProcurementAssetFullCreateDto.cs      # DTO สร้างจัดซื้อพร้อมลงทะเบียนครุภัณฑ์
│   │   ├── ProcurementHireFullCreateDto.cs       # DTO สร้างจัดซื้อพร้อมสัญญาจ้าง
│   │   ├── ProcurementMaterialFullCreateDto.cs   # DTO สร้างจัดซื้อพร้อมรับเข้าวัสดุ
│   │   ├── ProcurementRecordStatusHistoryDto.cs  # DTO ประวัติสถานะจัดซื้อจัดจ้าง
│   │   ├── Procurement_recordsDto.cs             # DTO บันทึกการจัดซื้อจัดจ้าง
│   │   ├── ProjectDto.cs                         # DTO ข้อมูลโครงการ
│   │   ├── PublicPortalDtos.cs                   # DTO ข้อมูลสาธารณะสแกน QR Code
│   │   ├── RegisterDto.cs                        # DTO ลงทะเบียนผู้ใช้งานใหม่
│   │   ├── RoleDto.cs                            # DTO จัดการสิทธิ์และบทบาท
│   │   ├── StaffDto.cs                           # DTO ข้อมูลบุคลากร
│   │   └── UserDto.cs                            # DTO ข้อมูลผู้ใช้งานและ Token
│   │
│   ├── Errors/                                   # [Error Handling] รูปแบบ HTTP Response สำหรับ Error
│   │   ├── ApiException.cs                       # Response Exception สำหรับ Developer/Production
│   │   ├── ApiResponse.cs                        # Standard API Response (StatusCode & Message)
│   │   └── ApiValidationErrorResponse.cs         # Response สำหรับ Model State Validation Failed
│   │
│   ├── Extensions/                               # [Service Extensions]
│   │   ├── ApplicationServicesExtensions.cs     # ลงทะเบียน Repositories, AutoMapper, CORS, DbContext
│   │   └── IdentityServiceExtensions.cs          # ลงทะเบียน ASP.NET Core Identity & JWT Configuration
│   │
│   ├── Helper/                                   # [Object Mapping Profiles]
│   │   └── MappingProfiles.cs                    # AutoMapper Configurations (Entities <-> DTOs)
│   │
│   ├── Middleware/                               # [Custom Middlewares]
│   │   └── ExceptionMiddleware.cs                # Middleware ดักจับ Unhandled Exceptions
│   │
│   ├── RequestHelpers/                           # [Query Helpers]
│   │   └── Pagination.cs                         # Class Generic สำหรับส่งคืนผลลัพธ์แบบแบ่งหน้า (Paging)
│   │
│   ├── Program.cs                                # จุดเริ่มต้นโปรเจกต์ (DI Setup, Pipeline, Middleware)
│   └── APi.csproj                                # ไฟล์คอนฟิกและ Dependencies ของ Web API Project
│
├── Core/                                         # [Domain Layer] Center Layer ของ Clean Architecture
│   ├── Entities/                                 # [Domain Models / Entity Definitions] (32 Entities)
│   │   ├── 📘 [Master Data Models]
│   │   │   ├── AcquisitionMethod.cs              # โมเดลวิธีการได้มาของทรัพย์สิน
│   │   │   ├── AppUser.cs                        # โมเดลผู้ใช้งานระบบ (สืบทอดจาก IdentityUser)
│   │   │   ├── AssetCategory.cs                  # โมเดลหมวดหมู่ครุภัณฑ์
│   │   │   ├── AssetItem.cs                      # โมเดลครุภัณฑ์หลัก
│   │   │   ├── AssetSubItem.cs                   # โมเดลครุภัณฑ์ย่อย
│   │   │   ├── asset_usage_types.cs              # โมเดลประเภทการใช้งานครุภัณฑ์
│   │   │   ├── Budget_sources.cs                 # โมเดลแหล่งเงินงบประมาณ
│   │   │   ├── Departments.cs                    # โมเดลแผนก/คณะ/หน่วยงาน
│   │   │   ├── Expense_types.cs                  # โมเดลประเภทค่าใช้จ่าย
│   │   │   ├── Fiscal_years.cs                   # โมเดลปีงบประมาณ
│   │   │   ├── Fund_categories.cs                # โมเดลหมวดหมู่เงิน
│   │   │   ├── MaterialItem.cs                   # โมเดลทะเบียนพรรณนาวัสดุ
│   │   │   ├── MaterialUnit.cs                   # โมเดลหน่วยนับวัสดุ
│   │   │   ├── Operation_types.cs                # โมเดลประเภทการดำเนินงานจัดซื้อ
│   │   │   ├── Positions.cs                      # โมเดลตำแหน่งงาน
│   │   │   ├── Prefixes.cs                       # โมเดลคำนำหน้าชื่อ
│   │   │   ├── Projects.cs                       # โมเดลโครงการ/แผนงาน
│   │   │   ├── Staffs.cs                         # โมเดลข้อมูลบุคลากร/เจ้าหน้าที่
│   │   │   └── Vendors.cs                        # โมเดลบริษัท/คู่ค้า/ผู้รับจ้าง
│   │   │
│   │   ├── 🔄 [Transaction Data Models]
│   │   │   ├── AssetRepair.cs                    # โมเดลการแจ้งซ่อมและประวัติซ่อมแซมครุภัณฑ์
│   │   │   ├── AssetSubItemDisposal.cs           # โมเดลการตัดจำหน่ายครุภัณฑ์ย่อย
│   │   │   ├── AssetSubItemHistory.cs            # โมเดลประวัติการย้าย/ใช้งานครุภัณฑ์ย่อย
│   │   │   ├── AssetWithdrawal.cs                # โมเดลการเบิก/ยืม/ครอบครองครุภัณฑ์
│   │   │   ├── HireDetail.cs                     # โมเดลรายละเอียดสัญญาจัดจ้างทำของ
│   │   │   ├── MaterialIssueDetail.cs            # โมเดลรายการจ่ายวัสดุออกจากคลัง
│   │   │   ├── MaterialReceiveDetail.cs          # โมเดลรายการรับวัสดุเข้าคลัง
│   │   │   ├── MaterialStockCard.cs              # โมเดลสต็อกการ์ดคลังวัสดุ (Inventory Ledger)
│   │   │   ├── MaterialWithdrawal.cs             # โมเดลเอกสารใบขอเบิกวัสดุ
│   │   │   ├── ProcurementRecordStatusHistory.cs # โมเดลประวัติการเปลี่ยนสถานะจัดซื้อจัดจ้าง
│   │   │   └── Procurement_records.cs            # โมเดลบันทึกการจัดซื้อจัดจ้างหลัก
│   │   │
│   │   └── ⚙️ [System & Config Models]
│   │       ├── BaseEntity.cs                     # Class พื้นฐานสำหรับ Entities (มี Id)
│   │       └── SystemSetting.cs                  # โมเดลการตั้งค่าระบบ
│   │
│   ├── Interfaces/                               # [Service Interfaces & Contracts]
│   │   ├── IGenericRepository.cs                 # Interface สำหรับ Generic Repository CRUD
│   │   ├── ISpecification.cs                     # Interface สำหรับ Specification Pattern Query
│   │   ├── IUnitOfWork.cs                        # Interface สำหรับ Unit of Work Transaction
│   │   └── Specifications/                       # Implementation ของ Specification Pattern
│   │       ├── BaseSpecification.cs              # Core Specification Logic
│   │       └── PagingParams.cs                   # Standard Parameters สำหรับการแบ่งหน้า
│   │
│   └── Core.csproj                               # ไฟล์คอนฟิก Core Class Library
│
└── Infrastructure/                               # [Data Access & Infrastructure Layer]
    ├── Config/                                   # [EF Core Configurations] การกำหนด Fluent API
    ├── Data/                                     # [Data Logic & Repository Implementation]
    │   ├── GenericRepository.cs                  # Generic Repository Implementation
    │   ├── SpecificationEvaluator.cs            # Evaluator แปลง Specification เป็น LINQ Query
    │   ├── StoreContext.cs                       # EF Core DbContext Class (IdentityDbContext)
    │   ├── StoreContextSeed.cs                   # Logic การลงข้อมูลเริ่มต้น (Data Seeding)
    │   ├── UnitOfWork.cs                         # Unit of Work Implementation
    │   └── SeedData/                             # โฟลเดอร์จัดเก็บไฟล์ Seed Data JSON
    │
    ├── Migrations/                               # [EF Core Migrations] ประวัติการเปลี่ยนโครงสร้าง DB
    ├── Services/                                 # [Infrastructure Services]
    │   └── FileService.cs                        # บริการจัดการอัปโหลด/บันทึก/ลบไฟล์แนบ
    └── Infrastructure.csproj                     # ไฟล์คอนฟิก Infrastructure Class Library

---

## 🗄️ สรุปโครงสร้างฐานข้อมูล (Database Schema & Tables)

อ้างอิงจาก Entity Framework Core Models ในระบบ

### 1) AspNetUsers (AppUser)
- PK: `Id` (string)
- Fields: `UserName, Email, PasswordHash, PhoneNumber, DisplayName`
- Relation: One-to-Many กับ `Staffs`, สิทธิ์ผ่าน `AspNetUserRoles`

### 2) Departments
- PK: `department_id`
- Fields: `department_name`
- Relation: One-to-Many กับ `Staffs, Procurement_records, MaterialStockCards`

### 3) Positions
- PK: `position_id`
- Fields: `position_name`
- Relation: One-to-Many กับ `Staffs`

### 4) Prefixes
- PK: `prefix_id`
- Fields: `prefix_name, short_name`
- Relation: One-to-Many กับ `Staffs`

### 5) Staffs
- PK: `staff_id`
- FK: `prefix_id, position_id, department_id`
- Fields: `first_name, last_name, email, phone_number`
- Relation: ไปยังเอกสารธุรกรรมหลายตาราง

### 6) Vendors
- PK: `vendor_id`
- Fields: `vendor_name, tax_id, address, phone_number, contact_name`
- Relation: One-to-Many กับ `Procurement_records`

### 7) Fiscal_years
- PK: `fiscal_year_id`
- Fields: `year_name, start_date, end_date, is_active`
- Relation: One-to-Many กับ `Procurement_records, MaterialStockCards`

### 8) Fund_categories
- PK: `fund_category_id`
- Fields: `fund_category_name`
- Relation: One-to-Many กับ `Procurement_records`

### 9) Budget_sources
- PK: `budget_source_id`
- Fields: `budget_source_name`
- Relation: One-to-Many กับ `Procurement_records`

### 10) Expense_types
- PK: `expense_type_id`
- Fields: `expense_type_name`
- Relation: One-to-Many กับ `Procurement_records`

### 11) Operation_types
- PK: `operation_type_id`
- Fields: `operation_type_name`
- Relation: One-to-Many กับ `Procurement_records`

### 12) Projects
- PK: `project_id`
- Fields: `project_code, project_name, description`
- Relation: One-to-Many กับ `Procurement_records`

### 13) AssetCategories
- PK: `asset_category_id`
- Fields: `category_code, category_name`
- Relation: One-to-Many กับ `AssetItems`

### 14) AcquisitionMethods
- PK: `acquisition_method_id`
- Fields: `method_name`
- Relation: One-to-Many กับ `AssetItems`

### 15) AssetUsageTypes
- PK: `asset_usage_type_id`
- Fields: `usage_type_name`

### 16) MaterialUnits
- PK: `unit_id`
- Fields: `unit_name`
- Relation: One-to-Many กับ `MaterialItems`

### 17) AssetItems
- PK: `asset_item_id`
- FK: `procurement_record_id, asset_category_id, acquisition_method_id, department_id`
- Fields: `asset_code, asset_name, price, useful_life, received_date`
- Relation: One-to-Many กับ `AssetSubItems`

### 18) AssetSubItems
- PK: `asset_sub_item_id`
- FK: `asset_item_id`
- Fields: `sub_item_code, serial_number, status, storage_location`
- Relation: One-to-Many กับ `AssetSubItemHistories, AssetSubItemDisposals`

### 19) MaterialItems
- PK: `material_item_id`
- FK: `unit_id`
- Fields: `material_code, material_name, unit_price, min_quantity, max_quantity`
- Relation: One-to-Many กับ `MaterialReceiveDetails, MaterialIssueDetails, MaterialStockCards`

### 20) Procurement_records
- PK: `procurement_record_id`
- FK: `fiscal_year_id, operation_type_id, expense_type_id, department_id, vendor_id, fund_category_id, budget_source_id, staff_id, project_id`
- Fields: `document_no, document_date, inspection_date, total_amount, amount_text, status, reference_no, attachment_file_path`
- Relation: One-to-Many กับ `HireDetails, ProcurementRecordStatusHistories, AssetItems, MaterialReceiveDetails, AssetWithdrawals`

### 21) HireDetails
- PK: `hire_detail_id`
- FK: `procurement_record_id`
- Fields: `contract_no, start_date, end_date, contract_amount, work_description`

### 22) ProcurementRecordStatusHistories
- PK: `status_history_id`
- FK: `procurement_record_id`
- Fields: `previous_status, new_status, changed_at, changed_by, remarks`

### 23) AssetWithdrawals
- PK: `procurement_withdrawal_id`
- FK: `procurement_record_id, staff_id`
- Fields: `withdrawal_document_no, withdrawal_date, end_date, storage_location, purpose`
- Relation: One-to-Many กับ `AssetRepairs`

### 24) AssetRepairs
- PK: `asset_repair_id`
- FK: `procurement_withdrawal_id, staff_id`
- Fields: `repair_document_no, repair_date, problem_description, repair_shop_name, repair_cost, status`

### 25) AssetSubItemHistories
- PK: `sub_item_history_id`
- FK: `asset_sub_item_id, staff_id, department_id`
- Fields: `action_type, action_date, location, remarks`

### 26) AssetSubItemDisposals
- PK: `sub_item_disposal_id`
- FK: `asset_sub_item_id`
- Fields: `disposal_date, disposal_method, disposal_reason, approved_by, quantity_disposed`

### 27) MaterialWithdrawals
- PK: `material_withdrawal_id`
- FK: `staff_id, procurement_record_id`
- Fields: `withdrawal_document_no, receive_document_no, remark`

### 28) MaterialReceiveDetails
- PK: `receive_detail_id`
- FK: `procurement_record_id, material_item_id`
- Fields: `item_no, quantity, unit_price, total_amount`
- Relation: One-to-Many กับ `MaterialStockCards`

### 29) MaterialIssueDetails
- PK: `issue_detail_id`
- FK: `procurement_record_id, material_item_id, staff_id`
- Fields: `issue_date, quantity, unit_price, total_amount`
- Relation: One-to-Many กับ `MaterialStockCards`

### 30) MaterialStockCards
- PK: `stock_card_id`
- FK: `material_item_id, receive_detail_id, issue_detail_id, fiscal_year_id, department_id`
- Fields: `transaction_date, transaction_type, quantity_in, quantity_out, balance_qty, unit_price, total_amount`

### 31) SystemSettings
- PK: `id`
- Fields: `system_name, system_code, setting_value`

---

## 📌 การจำแนกประเภทตาราง (Classification of Database Tables)

### 1) 📘 Master Data
- AspNetUsers
- Departments
- Positions
- Prefixes
- Staffs
- Vendors
- Fiscal_years
- Fund_categories
- Budget_sources
- Expense_types
- Operation_types
- Projects
- AssetCategories
- AcquisitionMethods
- AssetUsageTypes
- MaterialUnits
- AssetItems
- AssetSubItems
- MaterialItems

### 2) 🔄 Transaction Data
- procurement_records
- hireDetails
- procurementRecordStatusHistories
- assetWithdrawals
- assetRepairs
- assetSubItemHistories
- assetSubItemDisposals
- materialWithdrawals
- materialReceiveDetails
- materialIssueDetails
- materialStockCards

### 3) ⚙️ System & Config Data
- system_settings
- AspNetRoles / AspNetUserRoles

---

## 🛠️ เทคโนโลยีที่ใช้ (Backend Tech Stack & Libraries)

| หมวดหมู่ | เทคโนโลยี / ไลบรารี | เวอร์ชัน | วัตถุประสงค์ |
|---|---|---|---|
| Framework | .NET SDK | 9.0 | Framework หลักสำหรับพัฒนา Web API |
| Web API Engine | ASP.NET Core Web API | 9.0 | Routing, Controller, DI |
| Database Engine | Microsoft SQL Server | - | RDBMS |
| ORM / Data Access | Entity Framework Core | 9.0.14 | ORM + LINQ |
| EF Core Provider | Microsoft.EntityFrameworkCore.SqlServer | 9.0.14 | เชื่อมต่อ SQL Server |
| EF Core Tools | Microsoft.EntityFrameworkCore.Tools / .Design | 9.0.14 | จัดการ Migration |
| Identity & Security | Microsoft.AspNetCore.Identity.EntityFrameworkCore | 9.0.14 | User/Role Management |
| Authentication | System.IdentityModel.Tokens.Jwt | - | JWT Token |
| Object Mapper | AutoMapper | 16.1.1 | Mapping Entity ↔ DTO |
| API Documentation | Swashbuckle.AspNetCore | 9.0.6 | Swagger UI |
| OpenAPI Spec | Microsoft.AspNetCore.OpenApi | 9.0.14 | OpenAPI Metadata |
| Payment Integration | Stripe.net | 51.0.0 | เชื่อมต่อชำระเงินภายนอก |

---

## 🚀 ขั้นตอนการติดตั้งและเริ่มใช้งาน (Getting Started)

### Prerequisites
- .NET 9.0 SDK
- Microsoft SQL Server (LocalDB / SQL Express / SQL Server Instance)

### 1) ตั้งค่า Connection String
แก้ไฟล์ `APi/appsettings.json` หรือ `APi/appsettings.Development.json`

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=AssetFlowDb;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

### 2) อัปเดตฐานข้อมูลด้วย EF Core Migrations

```bash
dotnet ef database update --project Infrastructure --startup-project APi
```

### 3) รัน Backend API

```bash
dotnet run --project APi
```

จากนั้นเข้าใช้งาน Swagger ได้ที่:

```text
https://localhost:7001/swagger
```
