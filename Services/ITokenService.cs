using CodingPortfolio.Models;

namespace CodingPortfolio.Services
{

        public interface ITokenService
        {
            string CreateToken(Developer developer);
        }
    }

