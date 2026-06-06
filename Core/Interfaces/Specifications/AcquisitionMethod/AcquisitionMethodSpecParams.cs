using Core.Specifications;

namespace Core.Interfaces.Specifications.AcquisitionMethod
{
    public class AcquisitionMethodSpecParams : PagingParams
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
