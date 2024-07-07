

namespace Store.Services.Services.ProductService.Dtos
{
    public class ProductDetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string PictureURL { get; set; }
        public string BrandName { get; set; }
        public string TypeName { get; set; }
    }
}
