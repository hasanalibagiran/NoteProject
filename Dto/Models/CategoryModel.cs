using Dto.Common;

namespace Dto.Models
{
    public class CategoryModel:BaseModel
    {
        public string? Name { get; set; }
        public bool IsPublic { get; set; }
        public bool IsForSale { get; set; }
        public int Price { get; set; }
    }
}