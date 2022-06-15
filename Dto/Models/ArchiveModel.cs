using Dto.Common;

namespace Dto.Models
{
    public class ArchiveModel:BaseModel
    {
        public string Title { get; set; }
        public string Note { get; set; }
        public int CategoryId { get; set; }
        public bool IsPublic { get; set; }
        public bool IsForSale { get; set; }
        public int Price { get; set; }
        
        
    }
}