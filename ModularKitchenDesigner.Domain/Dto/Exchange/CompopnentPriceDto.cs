using System.ComponentModel.DataAnnotations;

namespace ModularKitchenDesigner.Domain.Dto.Exchange
{
    public class CompopnentPriceDto
    {
        [Required(ErrorMessage = "Code cannot be null or empty.")]
        public string Code { get; set; }

        public double Price { get; set; }
    }
}
