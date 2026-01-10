using BikeStoreWeb.Core.DTOs;
using BikeStoreWeb.Core.Entities;
using BikeStoreWeb.Core.Interfaces;
using BikeStoreWeb.Core.Responses;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BikeStoreWeb.Service.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public AuthService(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }


        public async Task<ServiceResponse<AuthResponseDto>> RegisterAsync(RegisterDto registerDto)
        {
            var response = new ServiceResponse<AuthResponseDto>();

            //Şifre uyuşma kontrolü
            if (registerDto.Password != registerDto.ConfirmPassword)
            {
                response.Success = false;
                response.Message = "Şifreler uyuşmuyor.";
                return response;
            }

            //Kullanıcı Nesnesi oluşturma
            var user = new ApplicationUser
            {
                Email = registerDto.Email,
                UserName = registerDto.Email,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
            };

            //Veritabanına kaydetme -Hashli şifre
            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
            {
                response.Success = false;
                response.Message = string.Join(", ", result.Errors.Select(e => e.Description));
                return response;
            }

            //Başarılıysa Token üret ve dönüş yap
            var token = GenerateJwtToken(user);

            response.Data = new AuthResponseDto
            {
                UserId = user.Id,
                Email = user.Email,
                Token = token
            };
            response.Success = true;
            response.Message = "Kullanıcı başarıyla oluşturuldu.";
            return response;
        }


        public async Task<ServiceResponse<AuthResponseDto>> LoginAsync(LoginDto loginDto)
        {
            var response = new ServiceResponse<AuthResponseDto>();

            //Kullanıcıyı bulma
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
            {
                response.Success = false;
                response.Message = "kullanıcı bulunamadı veya şifre hatalı.";
                return response;
            }

            //şifre kontrolü
            var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!isPasswordValid)
            {
                response.Success = false;
                response.Message = "Kullanıcı bulunamadı veya şifre hatalı.";
                return response;

            }

            //Başarılıysa token üret
            var token = GenerateJwtToken(user);
            response.Data = new AuthResponseDto
            {
                UserId = user.Id,
                Email = user.Email,
                Token = token
            };
            response.Success = true;
            response.Message = "Giriş başaşrılı.";
            return response;
        }


        //Token üretme metodu
        private string GenerateJwtToken(ApplicationUser user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]);

            //Token içine gömülecek bilgiler
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id),   //UserId
                new Claim(ClaimTypes.Email,user.Email), //email
                new Claim(ClaimTypes.Name,$"{user.FirstName} {user.LastName}") //ad-soyad
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"]
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);

        }
    }
}
