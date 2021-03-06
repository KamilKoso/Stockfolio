using Microsoft.IdentityModel.Tokens;
using Stockfolio.Shared.Abstractions.Auth;
using Stockfolio.Shared.Abstractions.Time;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Stockfolio.Shared.Infrastructure.Auth;

public sealed class JwtAccessTokenProvider : IAccessTokenProvider
{
    private static readonly Dictionary<string, IEnumerable<string>> EmptyClaims = new();
    private readonly AuthOptions _options;
    private readonly IClock _clock;
    private readonly SigningCredentials _signingCredentials;
    private readonly string _issuer;

    public JwtAccessTokenProvider(AuthOptions options, IClock clock)
    {
        var issuerSigningKey = options.IssuerSigningKey;
        if (issuerSigningKey is null)
        {
            throw new InvalidOperationException("Issuer signing key not set.");
        }

        _options = options;
        _clock = clock;
        _signingCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.IssuerSigningKey)), SecurityAlgorithms.HmacSha256);
        _issuer = options.Issuer;
    }

    public JsonWebToken CreateToken(Guid userId, IEnumerable<string> roles = null, string audience = null,
        IDictionary<string, IEnumerable<string>> claims = null)
    {
        var now = _clock.CurrentDate();
        var jwtClaims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new(JwtRegisteredClaimNames.UniqueName, userId.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Iat, new DateTimeOffset(now).ToUnixTimeMilliseconds().ToString())
        };

        foreach (var role in roles)
        {
            jwtClaims.Add(new Claim(ClaimTypes.Role, role));
        }

        if (!string.IsNullOrWhiteSpace(audience))
        {
            jwtClaims.Add(new Claim(JwtRegisteredClaimNames.Aud, audience));
        }

        if (claims?.Any() is true)
        {
            var customClaims = new List<Claim>();
            foreach (var (claim, values) in claims)
            {
                customClaims.AddRange(values.Select(value => new Claim(claim, value)));
            }

            jwtClaims.AddRange(customClaims);
        }

        var expires = now.Add(_options.Expiry);

        var jwt = new JwtSecurityToken(
            _issuer,
            claims: jwtClaims,
            notBefore: now,
            expires: expires,
            signingCredentials: _signingCredentials
        );

        var token = new JwtSecurityTokenHandler().WriteToken(jwt);

        return new JsonWebToken
        {
            AccessToken = token,
            Expiry = new DateTimeOffset(expires).ToUnixTimeMilliseconds(),
            UserId = userId,
            Roles = roles,
            Claims = claims ?? EmptyClaims
        };
    }
}