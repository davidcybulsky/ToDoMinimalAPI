using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ToDoMinimalAPI.Context;
using ToDoMinimalAPI.DTOs;

namespace ToDoMinimalAPI.Auth
{
    public interface IAuthService
    {
        string Login(LoginDto loginDto);
        int SignUp(SignUpDto signUpDto);
    }

    public class AuthService : IAuthService
    {
        private readonly ApiContext _db;
        private readonly AuthSettings _authSettings;

        public AuthService(ApiContext db, AuthSettings authSetings)
        {
            _db = db;
            _authSettings = authSetings;
        }

        private string GenerateToken(User user)
        {
            List<Claim> claims = new List<Claim> {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authSettings.Key));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: cred,
                issuer: _authSettings.Issuer,
                audience: _authSettings.Audience,
                expires: DateTime.UtcNow.AddDays(90)
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return $"Bearer {jwt}";
        }

        public string Login(LoginDto loginDto)
        {
            var user = _db.Users.FirstOrDefault(u => u.Email == loginDto.Email);
            if (user is null)
            {
                return null;
            }
            var result = BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash);
            if (!result)
            {
                return null;
            }

            var token = GenerateToken(user);

            return token;
        }

        public int SignUp(SignUpDto signUpDto)
        {
            var user = new User
            {
                Email = signUpDto.Email,
                ToDos = new()
            };

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(signUpDto.Password);
            user.PasswordHash = hashedPassword;

            _db.Add(user);
            _db.SaveChanges();
            return user.Id;
        }
    }
}