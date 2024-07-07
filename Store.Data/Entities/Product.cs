namespace Store.Data.Entities
{
    public class Product : BaseEntity<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string PictureURL { get; set; }
        public ProductBrand Brand { get; set; }
        public int BrandId { get; set; }
        public ProductType Type { get; set; }
        public int TypeId { get; set; }


    }
}
