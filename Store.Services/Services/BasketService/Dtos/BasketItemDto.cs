using System.ComponentModel.DataAnnotations;

namespace Store.Services.Services.BasketService.Dtos
{
    public class BasketItemDto
    {
        [Required]
        [Range(0, int.MaxValue)]
        public int ProductId { get; set; }
        [Required]
        public string ProductName { get; set; }
        [Required]
        [Range(0.1, double.MaxValue, ErrorMessage = "Message must be greater than Zero!!")]
        public decimal Price { get; set; }
        [Required]
        [Range(1, 10, ErrorMessage = "Quantity must be Between 1 and 10 Pieces!!")]
        public int Quantity { get; set; }
        [Required]
        public string PictureURL { get; set; }
        [Required]
        public string BrandName { get; set; }
        [Required]
        public string TypeName { get; set; }
    }
}