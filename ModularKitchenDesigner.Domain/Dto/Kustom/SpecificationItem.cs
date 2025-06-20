namespace ModularKitchenDesigner.Domain.Dto.Kustom
{
    public record SpecificationItem
    {
        public string Title { get; set; }
        public string Code { get; set; }
        public double Quantity { get; set; }
        public double UnitPrice { get; set; }
        public double TotalPrice { get; set; }
        public string Model { get; set; }
        public string ComponentType { get; set; }
        public string Material { get; set; }

    }
}
