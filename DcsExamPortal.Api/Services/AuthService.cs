using DcsExamPortal.Api.Data;
using DcsExamPortal.Api.DTOs;
using DcsExamPortal.Api.Models;
using DcsExamPortal.Api.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DcsExamPortal.Api.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public ApiResponse Register(RegisterDto dto)
        {
            if (_context.Users.Any(u => u.Email == dto.Email))
            {
                return new ApiResponse
                {
                    Success = false,
                    Message = "Email already exists"
                };
            }

            var hashedPassword = PasswordHasher.HashPassword(dto.Password);

            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                PasswordHash = hashedPassword,
                Role = "User"
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return new ApiResponse
            {
                Success = true,
                Message = "Registered successfully",
                Data = new
                {
                    user.Id,
                    user.Name,
                    user.Email,
                    user.Role
                }
            };
        }

        public ApiResponse Login(LoginDto dto)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == dto.Email);
            if (user == null || !PasswordHasher.VerifyPassword(dto.Password, user.PasswordHash))
            {
                return new ApiResponse
                {
                    Success = false,
                    Message = "Invalid credentials"
                };
            }

            var secretKey = _configuration["Jwt:Key"];
            var token = JwtHelper.GenerateJwtToken(user.Id.ToString(), user.Email, user.Role, secretKey);

            return new ApiResponse
            {
                Success = true,
                Message = "Login successful",
                Data = new
                {
                    user.Id,
                    user.Name,
                    user.Email,
                    user.Role,
                    Token = token
                }
            };
        }
    }
}
