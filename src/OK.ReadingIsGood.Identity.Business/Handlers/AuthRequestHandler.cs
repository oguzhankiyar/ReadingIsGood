using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OK.ReadingIsGood.Identity.Business.Abstractions;
using OK.ReadingIsGood.Identity.Business.Config;
using OK.ReadingIsGood.Identity.Business.Helpers;
using OK.ReadingIsGood.Identity.Contracts.Models;
using OK.ReadingIsGood.Identity.Contracts.Requests;
using OK.ReadingIsGood.Identity.Contracts.Responses;
using OK.ReadingIsGood.Identity.Persistence.Contexts;
using OK.ReadingIsGood.Identity.Persistence.Entities;
using OK.ReadingIsGood.Shared.Core.Exceptions;

namespace OK.ReadingIsGood.Identity.Business.Handlers
{
    public class AuthRequestHandler : IAuthRequestHandler
    {
        private readonly IdentityDataContext _context;
        private readonly IPasswordHelper _passwordHelper;
        private readonly IdentityBusinessConfig _config;

        public AuthRequestHandler(
            IdentityDataContext context,
            IPasswordHelper passwordHelper,
            IdentityBusinessConfig config)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _passwordHelper = passwordHelper ?? throw new ArgumentNullException(nameof(passwordHelper));
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public async Task<AuthTokenResponse> Handle(AuthTokenRequest request, CancellationToken cancellationToken)
        {
            var entity = await _context.Users.FirstOrDefaultAsync(x => x.Email == request.Email);
            if (entity == null || !_passwordHelper.Verify(request.Password, entity.Password))
            {
                throw new RequestNotValidatedException("The username or password is invalid.");
            }

            return new AuthTokenResponse
            {
                Data = new AuthTokenModel
                {
                    AccessToken = GenerateAccessToken(entity),
                    ExpiresIn = _config.ExpirationMinutes
                }
            };
        }

        private string GenerateAccessToken(UserEntity user)
        {
            var claims = new List<Claim>();

            claims.Add(new Claim(JwtRegisteredClaimNames.UniqueName, user.Id.ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Email));
            claims.Add(new Claim("name", user.FullName));

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _config.Issuer,
                audience: _config.Audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(_config.ExpirationMinutes),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(
                        Encoding.ASCII.GetBytes(_config.SecurityKey)),
                        SecurityAlgorithms.HmacSha256
                )
            );
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.WriteToken(jwtSecurityToken);
            return jwtToken;
        }
    }
}