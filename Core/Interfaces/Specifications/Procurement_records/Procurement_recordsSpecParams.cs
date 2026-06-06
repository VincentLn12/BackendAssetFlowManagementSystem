using Core.Specifications;

namespace Core.Interfaces.Specifications.Procurement_records
{
    public class Procurement_recordsSpecParams : PagingParams
    {
        public int? ProjectId { get; set; }

        public string? Sort { get; set; }

        private string? _search;

        public string Search
        {
            get => _search ?? "";
            set => _search = value.ToLower();
        }

        // เพิ่มตัวนี้
        public int? ExpenseTypeId { get; set; }

        // เพิ่มตัวนี้
        public int? FiscalYearId { get; set; }
    }
}