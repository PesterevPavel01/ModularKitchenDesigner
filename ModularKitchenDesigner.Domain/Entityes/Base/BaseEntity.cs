using System.ComponentModel.DataAnnotations;

namespace ModularKitchenDesigner.Domain.Entityes.Base
{
    public class BaseEntity : Identity
    {
        protected BaseEntity(){}
        public BaseEntity(string title, string code, bool enabled = true)
        {
            Title = title;
            Code = code;
            Enabled = enabled;
        }

        [Required(ErrorMessage = "Title cannot be null or empty.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Code cannot be null or empty.")]
        public string Code { get; set; }
        public bool Enabled { get; set; } = true;
    }
}
