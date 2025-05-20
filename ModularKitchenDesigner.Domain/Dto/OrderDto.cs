namespace ModularKitchenDesigner.Domain.Dto
{
    public sealed class OrderDto
    {
        public string KitchenTitle { get; set; }
        public string KitchenType { get; set; }
        public string KitchenTypeCode { get; set; }

        private List<SectionDto> _sections;
        public List<SectionDto> Sections
        {
            get => _sections;
            set
            {
                if (value == null || !value.Any())
                {
                    throw new ArgumentException("Sections cannot be null or empty.");
                }
                _sections = value;
            }
        }
        public double Width { get; set; }
        public double Price { get; set; }
    }
}
