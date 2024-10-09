using swp_be.Models;
using swp_be.Data;
using swp_be.Utils;


namespace swp_be.Services
{
    public class TokenServiceResult : ITokenServiceResult
    {
        public string accessToken { get; set; }
        public string refreshToken { get; set; }
    }

    public class TokenService : ITokenService
    {
        public ApplicationDBContext _context;
        private readonly UnitOfWork unitOfWork;

        public TokenService(ApplicationDBContext context)
        {
            this._context = context;
            this.unitOfWork = new UnitOfWork(context);
        }

        public ITokenServiceResult CreateToken(User user)
        {
            TokenServiceResult result = new TokenServiceResult();
            TokenUtils tokenUtils = new TokenUtils();

            result.accessToken = tokenUtils.Sign(user, TokenType.ACCESS_TOKEN);
            result.refreshToken = tokenUtils.Sign(user, TokenType.REFRESH_TOKEN);
            
            unitOfWork.TokenRepository.Create(new Token
            {
                UserID = user.UserID,
                RefreshToken = result.refreshToken,
                CreateAt = DateTime.Now,
                ExpireAt = DateTime.Now.AddDays(7),
            });

            return result;
        }

        //public Token RefreshToken(string userID)
        //{
        //    return unitOfWork.UserTokenRepository.GetByToken(userID);
        //}

    }
}
