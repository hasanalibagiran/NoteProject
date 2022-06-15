namespace Entities
{
    public class Category:BaseEntity
    {
        public string? Name { get; set; }
        public int OwnerUserId { get; set; }
        public bool IsPublic { get; set; }
        public bool IsForSale { get; set; }
        public int Price { get; set; }
    }
}