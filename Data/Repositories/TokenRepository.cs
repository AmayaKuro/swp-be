using swp_be.data.Repositories;
using swp_be.Models;

namespace swp_be.Data.Repositories
{
    public class TokenRepository : GenericRepository<Token>
    {
        public TokenRepository(ApplicationDBContext context) : base(context) 
        {
        }

        public Token GetByToken(string token)
        {
            return _context.Tokens.Where(userToken => userToken.RefreshToken == token).Single();
        }
    }
}
