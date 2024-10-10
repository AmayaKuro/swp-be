using Microsoft.EntityFrameworkCore;
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
            return _context.Tokens
                .Where(userToken => userToken.RefreshToken == token)
                .Include(token => token.User)
                .SingleOrDefault();
        }

        public bool RemoveByToken(string token)
        {
            Token info = GetByToken(token);

            if (info == null)
            {
                return false;
            }

            Remove(info);
            return true;
        }
    }
}
