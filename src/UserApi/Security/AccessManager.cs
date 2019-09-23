using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using UserData;

namespace UserApi.Security
{
    public class AccessManager
    {
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        private SigningConfigurations _signingConfigurations;
        private TokenConfigurations _tokenConfigurations;

        public AccessManager(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            SigningConfigurations signingConfigurations,
            TokenConfigurations tokenConfigurations)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _signingConfigurations = signingConfigurations;
            _tokenConfigurations = tokenConfigurations;
        }

        public bool ValidateCredentials(User user)
        {
            bool credenciaisValidas = false;
            if (user != null && !string.IsNullOrWhiteSpace(user.UserID))
            {
                // Verifica a existência do usuário nas tabelas do
                // ASP.NET Core Identity
                var userIdentity = _userManager
                    .FindByNameAsync(user.UserID).Result;
                if (userIdentity != null)
                {
                    // Efetua o login com base no Id do usuário e sua senha
                    var resultadoLogin = _signInManager
                        .CheckPasswordSignInAsync(userIdentity, user.Password, false)
                        .Result;
                    if (resultadoLogin.Succeeded)
                    {
                        return true;
                        // Verifica se o usuário em questão possui
                        // a role Acesso-APIProdutos
                        credenciaisValidas = _userManager.IsInRoleAsync(
                            userIdentity, "Admin").Result;
                    }
                }
            }

            return credenciaisValidas;
        }

        public Token GenerateToken(User user)
        {
            var _user =  _userManager.FindByNameAsync(user.UserID).Result;
            var userRoles = _userManager.GetRolesAsync(_user).Result;
            ClaimsIdentity identity = new ClaimsIdentity(
                new GenericIdentity(user.UserID, "Login"),
                new[] {
                        new Claim("ID", Guid.NewGuid().ToString("N")),
                        new Claim(ClaimTypes.NameIdentifier, _user.Id.ToString()),
                        new Claim(ClaimTypes.Role, userRoles.FirstOrDefault())
                }
            );

            DateTime dataCriacao = DateTime.Now;
            DateTime dataExpiracao = dataCriacao +
                TimeSpan.FromSeconds(_tokenConfigurations.Seconds);

            var handler = new JwtSecurityTokenHandler();
            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _tokenConfigurations.Issuer,
                Audience = _tokenConfigurations.Audience,
                SigningCredentials = _signingConfigurations.SigningCredentials,
                Subject = identity,
                NotBefore = dataCriacao,
                Expires = dataExpiracao
            });
            var token = handler.WriteToken(securityToken);

            return new Token()
            {
                Authenticated = true,
                Created = dataCriacao.ToString("yyyy-MM-dd HH:mm:ss"),
                Expiration = dataExpiracao.ToString("yyyy-MM-dd HH:mm:ss"),
                AccessToken = token,
                Message = "OK"
            };
        }
    }
}