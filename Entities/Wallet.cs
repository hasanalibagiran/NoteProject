namespace Entities
{
    public class Wallet:BaseEntity
    {
        public int Balance { get; set; }
        public int OwnerUserId { get; set; }

    }
}