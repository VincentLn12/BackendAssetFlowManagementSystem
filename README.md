# AssetFlow Management System Backend Project

ระบบ Backend API สำหรับบริหารจัดการพัสดุ ครุภัณฑ์ การจัดซื้อจัดจ้าง และคลังวัสดุ  
พัฒนาด้วย **.NET 9 (ASP.NET Core Web API)** พร้อมระบบยืนยันตัวตน (**ASP.NET Core Identity / JWT**), **Entity Framework Core (SQL Server)**, **AutoMapper**, **Repository & Unit of Work Pattern**, **Specification Pattern** และระบบจัดการไฟล์แนบ

---

## 📚 สารบัญ (Table of Contents)

- [📌 ภาพรวมโปรเจกต์](#-ภาพรวมโปรเจกต์)
- [🛠️ เทคโนโลยีที่ใช้](#️-เทคโนโลยีที่ใช้)
- [📁 โครงสร้างโฟลเดอร์](#-โครงสร้างโฟลเดอร์)
- [🗄️ โครงสร้างฐานข้อมูลโดยสรุป](#️-โครงสร้างฐานข้อมูลโดยสรุป)
- [📋 รายการตารางหลัก](#-รายการตารางหลัก)
- [✅ จุดเด่นของระบบ](#-จุดเด่นของระบบ)

---

## 📌 ภาพรวมโปรเจกต์

โปรเจกต์นี้ออกแบบตามแนวทาง **Clean Architecture** แบ่งชั้นการทำงานเป็น 3 ส่วนหลัก:

- **API Layer** — รับคำขอจากผู้ใช้งานและส่งผลลัพธ์กลับ
- **Core Layer** — เก็บ Entity, Interface และ Business Domain
- **Infrastructure Layer** — จัดการฐานข้อมูล, Repository, Services และการเชื่อมต่อภายนอก

---

## 🛠️ เทคโนโลยีที่ใช้

- **Framework:** .NET 9 (ASP.NET Core Web API)
- **Database:** SQL Server
- **ORM:** Entity Framework Core 9
- **Authentication:** ASP.NET Core Identity + JWT Bearer Token
- **Object Mapping:** AutoMapper
- **Architecture:** Clean Architecture, Generic Repository, Unit of Work, Specification Pattern
- **API Documentation:** Swagger / OpenAPI
- **Payment Integration:** Stripe.net

---

## 📁 โครงสร้างโฟลเดอร์

```text
AssetFlowManagementSystem/
├── APi/                         # โปรเจกต์หลักของ Web API
│   ├── Controllers/             # API Controllers
│   ├── DTOs/                    # Data Transfer Objects
│   ├── Errors/                  # รูปแบบการจัดการ Error Response
│   ├── Extensions/              # Service Extensions
│   ├── Helper/                  # AutoMapper Profiles
│   ├── Middleware/              # Custom Middleware
│   ├── RequestHelpers/          # Helper สำหรับ Paging
│   ├── Program.cs               # จุดเริ่มต้นของโปรเจกต์
│   └── APi.csproj               # ไฟล์โปรเจกต์
│
├── Core/                        # Domain Layer
│   ├── Entities/                # Entity Models
│   ├── Interfaces/              # Interface สำหรับ Repository / Specification / Unit of Work
│   └── Core.csproj
│
└── Infrastructure/              # Data Access Layer
    ├── Config/                  # EF Core Configurations
    ├── Data/                    # DbContext, Repository, UnitOfWork, Seed Data
    ├── Migrations/              # EF Core Migrations
    ├── Services/                # Infrastructure Services
    └── Infrastructure.csproj
```

---

## 🗄️ โครงสร้างฐานข้อมูลโดยสรุป

ฐานข้อมูลของระบบออกแบบให้รองรับ 3 กลุ่มหลัก:

### 1) Master Data (ข้อมูลอ้างอิงหลัก)
ข้อมูลอ้างอิงหลักที่ใช้ในระบบ

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

### 2) Transaction Data (ข้อมูลธุรกรรม)
ข้อมูลรายการธุรกรรมหรือเหตุการณ์ที่เกิดขึ้นในระบบ

- Procurement_records
- HireDetails
- ProcurementRecordStatusHistories
- AssetWithdrawals
- AssetRepairs
- AssetSubItemHistories
- AssetSubItemDisposals
- MaterialWithdrawals
- MaterialReceiveDetails
- MaterialIssueDetails
- MaterialStockCards

### 3) System & Config Data (ข้อมูลตั้งค่า/สิทธิ์ระบบ)
ข้อมูลตั้งค่าและสิทธิ์ของระบบ

- SystemSettings
- AspNetRoles
- AspNetUserRoles

---

## 📋 รายการตารางหลัก

### กลุ่ม Master Data Tables (1-19)

### 1. AspNetUsers
ตารางผู้ใช้งานระบบ

- **Primary Key:** Id
- **ฟิลด์สำคัญ:** UserName, Email, PasswordHash, PhoneNumber, DisplayName
- **ความสัมพันธ์:** เชื่อมกับ Staffs และ AspNetUserRoles

### 2. Departments
ตารางข้อมูลแผนก / คณะ / หน่วยงาน

- **Primary Key:** department_id
- **ฟิลด์สำคัญ:** department_name
- **ความสัมพันธ์:** เชื่อมกับ Staffs, Procurement_records, MaterialStockCards

### 3. Positions
ตารางตำแหน่งงาน

- **Primary Key:** position_id
- **ฟิลด์สำคัญ:** position_name
- **ความสัมพันธ์:** เชื่อมกับ Staffs

### 4. Prefixes
ตารางคำนำหน้าชื่อ

- **Primary Key:** prefix_id
- **ฟิลด์สำคัญ:** prefix_name, short_name
- **ความสัมพันธ์:** เชื่อมกับ Staffs

### 5. Staffs
ตารางทะเบียนบุคลากร

- **Primary Key:** staff_id
- **Foreign Keys:** prefix_id, position_id, department_id
- **ฟิลด์สำคัญ:** first_name, last_name, email, phone_number

### 6. Vendors
ตารางผู้ขาย / ผู้รับจ้าง / คู่ค้า

- **Primary Key:** vendor_id
- **ฟิลด์สำคัญ:** vendor_name, tax_id, address, phone_number, contact_name

### 7. Fiscal_years
ตารางปีงบประมาณ

- **Primary Key:** fiscal_year_id
- **ฟิลด์สำคัญ:** year_name, start_date, end_date, is_active

### 8. Fund_categories
ตารางหมวดหมู่เงินงบประมาณ

- **Primary Key:** fund_category_id
- **ฟิลด์สำคัญ:** fund_category_name

### 9. Budget_sources
ตารางแหล่งเงินงบประมาณ

- **Primary Key:** budget_source_id
- **ฟิลด์สำคัญ:** budget_source_name

### 10. Expense_types
ตารางประเภทค่าใช้จ่าย

- **Primary Key:** expense_type_id
- **ฟิลด์สำคัญ:** expense_type_name

### 11. Operation_types
ตารางประเภทการดำเนินงานจัดซื้อจัดจ้าง

- **Primary Key:** operation_type_id
- **ฟิลด์สำคัญ:** operation_type_name

### 12. Projects
ตารางโครงการ / แผนงาน

- **Primary Key:** project_id
- **ฟิลด์สำคัญ:** project_code, project_name, description

### 13. AssetCategories
ตารางหมวดหมู่ครุภัณฑ์

- **Primary Key:** asset_category_id
- **ฟิลด์สำคัญ:** category_code, category_name

### 14. AcquisitionMethods
ตารางวิธีการได้มาของทรัพย์สิน

- **Primary Key:** acquisition_method_id
- **ฟิลด์สำคัญ:** method_name

### 15. AssetUsageTypes
ตารางประเภทการใช้งานครุภัณฑ์

- **Primary Key:** asset_usage_type_id
- **ฟิลด์สำคัญ:** usage_type_name

### 16. MaterialUnits
ตารางหน่วยนับวัสดุ

- **Primary Key:** unit_id
- **ฟิลด์สำคัญ:** unit_name

### 17. AssetItems
ตารางทะเบียนครุภัณฑ์หลัก

- **Primary Key:** asset_item_id
- **Foreign Keys:** procurement_record_id, asset_category_id, acquisition_method_id, department_id
- **ฟิลด์สำคัญ:** asset_code, asset_name, price, useful_life, received_date

### 18. AssetSubItems
ตารางทะเบียนครุภัณฑ์ย่อย

- **Primary Key:** asset_sub_item_id
- **Foreign Key:** asset_item_id
- **ฟิลด์สำคัญ:** sub_item_code, serial_number, status, storage_location

### 19. MaterialItems
ตารางทะเบียนวัสดุสิ้นเปลือง

- **Primary Key:** material_item_id
- **Foreign Key:** unit_id
- **ฟิลด์สำคัญ:** material_code, material_name, unit_price, min_quantity, max_quantity

### กลุ่ม Transaction Data Tables (20-30)

### 20. Procurement_records
ตารางบันทึกเอกสารการจัดซื้อจัดจ้างหลัก

- **Primary Key:** procurement_record_id
- **Foreign Keys:** fiscal_year_id, operation_type_id, expense_type_id, department_id, vendor_id, fund_category_id, budget_source_id, staff_id, project_id
- **ฟิลด์สำคัญ:** document_no, document_date, inspection_date, total_amount, amount_text, status, reference_no, attachment_file_path

### 21. HireDetails
ตารางรายละเอียดสัญญาจัดจ้างทำของ

- **Primary Key:** hire_detail_id
- **Foreign Key:** procurement_record_id
- **ฟิลด์สำคัญ:** contract_no, start_date, end_date, contract_amount, work_description

### 22. ProcurementRecordStatusHistories
ตารางประวัติการเปลี่ยนสถานะเอกสารจัดซื้อจัดจ้าง

- **Primary Key:** status_history_id
- **Foreign Key:** procurement_record_id
- **ฟิลด์สำคัญ:** previous_status, new_status, changed_at, changed_by, remarks

### 23. AssetWithdrawals
ตารางประวัติการเบิก / ยืมครุภัณฑ์

- **Primary Key:** procurement_withdrawal_id
- **Foreign Keys:** procurement_record_id, staff_id
- **ฟิลด์สำคัญ:** withdrawal_document_no, withdrawal_date, end_date, storage_location, purpose

### 24. AssetRepairs
ตารางแจ้งซ่อมครุภัณฑ์

- **Primary Key:** asset_repair_id
- **Foreign Keys:** procurement_withdrawal_id, staff_id
- **ฟิลด์สำคัญ:** repair_document_no, repair_date, problem_description, repair_shop_name, repair_cost, status

### 25. AssetSubItemHistories
ตารางประวัติการเคลื่อนย้ายครุภัณฑ์ย่อย

- **Primary Key:** sub_item_history_id
- **Foreign Keys:** asset_sub_item_id, staff_id, department_id
- **ฟิลด์สำคัญ:** action_type, action_date, location, remarks

### 26. AssetSubItemDisposals
ตารางตัดจำหน่ายครุภัณฑ์ย่อย

- **Primary Key:** sub_item_disposal_id
- **Foreign Key:** asset_sub_item_id
- **ฟิลด์สำคัญ:** disposal_date, disposal_method, disposal_reason, approved_by, quantity_disposed

### 27. MaterialWithdrawals
ตารางใบขอเบิกวัสดุ

- **Primary Key:** material_withdrawal_id
- **Foreign Keys:** staff_id, procurement_record_id
- **ฟิลด์สำคัญ:** withdrawal_document_no, receive_document_no, remark

### 28. MaterialReceiveDetails
ตารางรายการตรวจรับวัสดุเข้าคลัง

- **Primary Key:** receive_detail_id
- **Foreign Keys:** procurement_record_id, material_item_id
- **ฟิลด์สำคัญ:** item_no, quantity, unit_price, total_amount

### 29. MaterialIssueDetails
ตารางรายการตัดจ่ายวัสดุออกจากคลัง

- **Primary Key:** issue_detail_id
- **Foreign Keys:** procurement_record_id, material_item_id, staff_id
- **ฟิลด์สำคัญ:** issue_date, quantity, unit_price, total_amount

### 30. MaterialStockCards
ตารางสต็อกการ์ดวัสดุ

- **Primary Key:** stock_card_id
- **Foreign Keys:** material_item_id, receive_detail_id, issue_detail_id, fiscal_year_id, department_id
- **ฟิลด์สำคัญ:** transaction_date, transaction_type, reference_document_no, quantity_in, quantity_out, balance_qty, unit_price, total_amount

### กลุ่ม System & Config Tables (31+)

### 31. SystemSettings
ตารางตั้งค่าระบบ

- **Primary Key:** id
- **ฟิลด์สำคัญ:** system_name, system_code, setting_value

---

## ✅ จุดเด่นของระบบ

- รองรับงานพัสดุ ครุภัณฑ์ และวัสดุแบบครบวงจร
- แยกโครงสร้างตาม Clean Architecture ชัดเจน
- มีระบบยืนยันตัวตนและสิทธิ์การใช้งาน
- รองรับการจัดการไฟล์แนบ
- ออกแบบฐานข้อมูลรองรับงานจัดซื้อจัดจ้างและคลังวัสดุ
