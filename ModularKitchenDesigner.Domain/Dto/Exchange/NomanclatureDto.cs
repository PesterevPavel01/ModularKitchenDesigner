using System.ComponentModel.DataAnnotations;

namespace ModularKitchenDesigner.Domain.Dto.Exchange
{
    public class NomanclatureDto
    {
        [Required(ErrorMessage = "Title cannot be null or empty.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Code cannot be null or empty.")]
        public string Code { get; set; }
        public double Price { get; set; }

        [Required(ErrorMessage = "Parents cannot be null or empty.")]
        public List<SimpleDto> Parents { get; set; }
        public SimpleDto Model { get; set; }
        public double Widht { get; set; }
        public List<SimpleDto> Templates { get; set; }
    }
}
