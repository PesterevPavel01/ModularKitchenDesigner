namespace ModularKitchenDesigner.Domain.Dto.Authorization
{
    public sealed class TokenDto
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
