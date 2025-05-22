using Newtonsoft.Json;

namespace ModularKitchenDesigner.Application.Validators
{
    public class ErrorMessage
    {
        public string Title { get; set; } = null!;
        public string Entity { get; set; } = null!;
        public string Message { get; set; } = null!;
        public string? CallerObject { get; set; }
        public string? MethodName { get; set; }
        public object? MethodArgument { get; set; }

        public int Code { get; set; }

        public String ToJson()
            => JsonConvert.SerializeObject(this, Formatting.Indented);
    }
}
