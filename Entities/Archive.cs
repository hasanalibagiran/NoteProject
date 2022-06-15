namespace Entities
{
    public class Archive:BaseEntity
    {   
        public string Title { get; set; }
        public string Note { get; set; }
        public int CategoryId { get; set; }
        public int OwnerUserId { get; set; }
        public bool IsPublic { get; set; }
        public bool IsForSale { get; set; }
        public int Price { get; set; }
        
    }
}

