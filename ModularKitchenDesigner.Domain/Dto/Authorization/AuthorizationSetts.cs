namespace ModularKitchenDesigner.Domain.Dto.Authorization
{
    public sealed class AuthorizationSetts
    {
        public string Issuer { get; set;}
        public string Audience { get; set; }
        public string SecretKey { get; set; }
        public string Authority { get; set; }
        public int LifetimeInMinutes { get; set; }
        public int RefreshTokenValidityInDays { get; set; }
    }
}
