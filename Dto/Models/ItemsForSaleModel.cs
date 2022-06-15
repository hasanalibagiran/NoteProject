namespace Dto.Models
{
    public class ItemsForSaleModel
    {
        public string Title { get; set; }
        public List<CategoryModel> CategoryList { get; set; }
        public List<ArchiveModel> ArchiveList { get; set; }
    }
}