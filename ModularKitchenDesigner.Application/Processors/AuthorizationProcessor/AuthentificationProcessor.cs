﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ModularKitchenDesigner.Domain.Dto.Authorization;
using ModularKitchenDesigner.Domain.Entityes.Authorization;
using Repository;

namespace ModularKitchenDesigner.Application.Processors.AuthorizationProcessor
{
    public sealed class AuthentificationProcessor
    {
        private IRepositoryFactory _repositoryFactory = null!;
        private readonly AuthorizationSetts _authorizationSetts;

        public AuthentificationProcessor(IRepositoryFactory repositoryFactory, IOptions<AuthorizationSetts> authorizationSetts)
        {
            _repositoryFactory = repositoryFactory;
            _authorizationSetts = authorizationSetts.Value;
        }

        public async Task<String> ProcessAsync(LoginDto model) 
        {
            var user = (await _repositoryFactory.GetRepository<ApplicationUser>()
                .GetAllAsync(
                    predicate: x => x.UserName == model.UserName && HashPassword(model) == x.Password,
                    trackingType: TrackingType.Tracking
                )).FirstOrDefault();

            if (user is null)
                throw new ArgumentException("User not found");

            var userToken = (await _repositoryFactory.GetRepository<UserToken>().GetAllAsync(predicate: x => x.UserId == user.Id)).FirstOrDefault();

            var refreshToken = GenerateRefreshToken();

            if (userToken is null)
            {
                userToken = new UserToken(user.Id)
                {
                    RefreshToken = refreshToken,
                    RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7),
                    User = user
                };

                await _repositoryFactory.GetRepository<UserToken>().CreateAsync(userToken);
            }
            else
            {
                userToken.RefreshToken = refreshToken;
                userToken.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            }

            var accessToken = GenerateAccessToken(user);


            return accessToken;
        }

        private string GenerateAccessToken(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, "PesterevPavel90@gmail.com"),
                new Claim(ClaimTypes.Role, "Administrator") // Добавляем claim с ролью
            };

            byte[] secretBytes = Encoding.UTF8.GetBytes(_authorizationSetts.SecretKey);
            var key = new SymmetricSecurityKey(secretBytes);
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _authorizationSetts.Issuer,
                _authorizationSetts.Audience,
                claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials
            );

            var value = new JwtSecurityTokenHandler().WriteToken(token);

            return value;
        }

        private string GenerateRefreshToken()
        {
            var randomNumbers = new byte[32];
            using var randomeNumberGenerator = RandomNumberGenerator.Create();
            randomeNumberGenerator.GetBytes(randomNumbers);
            return Convert.ToBase64String(randomNumbers);
        }

        private string HashPassword(LoginDto model)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(model.Password));
            return BitConverter.ToString(bytes).ToLower();
        }
    }
}
