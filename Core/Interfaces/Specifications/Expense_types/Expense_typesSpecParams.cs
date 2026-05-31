using Core.Specifications;

namespace Core.Interfaces.Specification.Expense_types
{
    public class Expense_typesSpecParams : PagingParams
    {
        public string? Sort { get; set; }

        private string? _search;

        public string Search
        {
            get => _search ?? "";
            set => _search = value.ToLower();
        }
    }
}
