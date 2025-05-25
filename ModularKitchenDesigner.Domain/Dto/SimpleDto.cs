using ModularKitchenDesigner.Domain.Entityes.Base;
using ModularKitchenDesigner.Domain.Interfaces.Base;
using System.ComponentModel.DataAnnotations;

namespace ModularKitchenDesigner.Domain.Dto
{
    public sealed class SimpleDto : ISimpleEntity
    {
        [Required(ErrorMessage = "Title cannot be null or empty.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Code cannot be null or empty.")]
        public string Code { get; set; }
    }
}