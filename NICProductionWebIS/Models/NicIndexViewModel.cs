using System.Collections.Generic;

namespace NICProductionWebIS.Models
{
    public class NicIndexViewModel
    {
        public IEnumerable<NicModel> Items { get; set; } = new List<NicModel>();
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public string? Query { get; set; }
    }
}