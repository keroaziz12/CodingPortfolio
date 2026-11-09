using CodingPortfolio.Data;
using CodingPortfolio.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CodingPortfolio.Services
{
    public class AuthService : IAuthService
    { 
        private readonly ApplicationDbContext _context;
        private readonly ItokenService _tokenService;
        private readonly PasswordHasher<Developer> _passwordHasher = new(); 
        public AuthService(ApplicationDbContext context, ItokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

       

        public  async Task<Developer?> RegisterAsync(string name, string email, string password)
        {
            var normalizedEmail = email.Trim().ToLowerInvariant();
            var exists = await _context.Developers.AnyAsync(d => d.Email == normalizedEmail);
            if (exists)
                return null;

            {
                var developer = new Developer
                {
                    Name = name.Trim(),
                    Email = normalizedEmail,
                };
                developer.PasswordHash = _passwordHasher.HashPassword(developer, password);
                _context.Developers.Add(developer);
                await _context.SaveChangesAsync();
                return developer;
            }


        }
        public async Task<string?> LoginAsync(string email, string password)
        {
            var normalizedEmail = email.Trim().ToLowerInvariant();

            var developer = await _context.Developers
                .FirstOrDefaultAsync(d => d.Email == normalizedEmail);

            if (developer is null)
                return null;

            var result = _passwordHasher.VerifyHashedPassword(
                developer, developer.PasswordHash, password);

            if (result == PasswordVerificationResult.Failed)
                return null;

            return _tokenService.CreateToken(developer);
        }



        }
    }

