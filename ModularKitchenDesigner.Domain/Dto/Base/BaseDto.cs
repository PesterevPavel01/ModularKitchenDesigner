using System.ComponentModel.DataAnnotations;

namespace ModularKitchenDesigner.Domain.Dto.Base
{
    public abstract class BaseDto
    {
        public BaseDto(){}

        public BaseDto(string title, string code)
        {
            Title = title;
            Code = code;
        }

        [Required(ErrorMessage = "Title cannot be null or empty.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Code cannot be null or empty.")]
        public string Code { get; set; }
    }

}
