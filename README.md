# 📦 AssetFlow Management System
> **ระบบบริหารจัดการพัสดุ ครุภัณฑ์ การจัดซื้อจัดจ้าง และคลังวัสดุ (Full-Stack Web Application)**
> พัฒนาด้วย **.NET 9 Web API** (Backend) และ **Angular 21** (Frontend) ตามสถาปัตยกรรม Clean Architecture และ Modern Component-Driven Development

---

## 📑 สารบัญ (Table of Contents)
- [📖 ภาพรวมระบบ (System Overview)](#-ภาพรวมระบบ-system-overview)
- [🖥️ 1. Backend System (.NET 9 Web API)](#️-1-backend-system-net-9-web-api)
  - [📁 โครงสร้างโฟลเดอร์แบบละเอียด (Folder Structure)](#-โครงสร้างโฟลเดอร์แบบละเอียด-folder-structure)
  - [🗄️ สรุปโครงสร้างฐานข้อมูล (Database Schema & Tables)](#️-สรุปโครงสร้างฐานข้อมูล-database-schema--tables)
  - [📌 การจำแนกประเภทตาราง (Classification of Database Tables)](#-การจำแนกประเภทตาราง-classification-of-database-tables)
  - [🛠️ เทคโนโลยีที่ใช้ (Backend Tech Stack)](#️-เทคโนโลยีที่ใช้-backend-tech-stack)
- [💻 2. Frontend System (Angular 21 Web Application)](#-2-frontend-system-angular-21-web-application)
  - [📁 โครงสร้างโฟลเดอร์แบบละเอียด (Folder Structure)](#-โครงสร้างโฟลเดอร์แบบละเอียด-folder-structure-1)
  - [🌟 คุณสมบัติและฟังก์ชันการทำงานหลัก (Key Features)](#-คุณสมบัติและฟังก์ชันการทำงานหลัก-key-features)
  - [🛠️ เทคโนโลยีและไลบรารีที่ใช้ (Frontend Tech Stack & Libraries)](#️-เทคโนโลยีและไลบรารีที่ใช้-frontend-tech-stack--libraries)
- [🚀 3. ขั้นตอนการติดตั้งและเริ่มใช้งาน (Getting Started)](#-3-ขั้นตอนการติดตั้งและเริ่มใช้งาน-getting-started)

---

## 📖 ภาพรวมระบบ (System Overview)

**AssetFlow Management System** คือระบบบริหารจัดการพัสดุและคลังสินค้าครบวงจร ออกแบบมาเพื่อรองรับกระบวนการทำงานของหน่วยงานภาครัฐ สถาบันการศึกษา หรือองค์กรขนาดใหญ่ ครอบคลุมวงจรชีวิตของพัสดุและทรัพย์สิน ตั้งแต่การจัดซื้อจัดจ้าง การรับเข้าคลัง การเบิกจ่ายวัสดุสิ้นเปลือง การควบคุมสต็อกการ์ดแบบ Real-time การถือครองและยืมครุภัณฑ์ การส่งซ่อมบำรุง การตัดจำหน่าย ตลอดจนระบบพอร์ตอลสาธารณะสำหรับสแกน **QR Code** ตรวจสอบครุภัณฑ์

---

## 🖥️ 1. Backend System (.NET 9 Web API)

### 📁 โครงสร้างโฟลเดอร์แบบละเอียด (Folder Structure)

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
