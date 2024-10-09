using swp_be.Models;
using swp_be.Data;
using swp_be.Utils;
using Zxcvbn;


namespace swp_be.Services
{
    public class TokenServiceResult : ITokenServiceResult
    {
        public string accessToken { get; set; }
        public string refreshToken { get; set; }
    }

    public class TokenService : ITokenService
    {
        private TokenUtils tokenUtils = new TokenUtils();
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

            result.accessToken = tokenUtils.Sign(user, TokenType.ACCESS_TOKEN);
            result.refreshToken = tokenUtils.Sign(user, TokenType.REFRESH_TOKEN);
            
            unitOfWork.TokenRepository.Create(new Token
            {
                UserID = user.UserID,
                RefreshToken = result.refreshToken,
                CreateAt = DateTime.Now,
                ExpireAt = DateTime.Now.AddDays(7),
            });

            unitOfWork.Save();

            return result;
        }



        public ITokenServiceResult Refresh(string refreshToken)
        {
            Token token = unitOfWork.TokenRepository.GetByToken(refreshToken);

            if (token == null)
            {
                return null;
            }

            TokenServiceResult result = new TokenServiceResult();

            result.accessToken = tokenUtils.Sign(token.User, TokenType.ACCESS_TOKEN);
            result.refreshToken = tokenUtils.Sign(token.User, TokenType.REFRESH_TOKEN);

            token.RefreshToken = result.refreshToken;
            token.ExpireAt = DateTime.Now.AddDays(7);
            unitOfWork.Save();

            return result;
        }

    }
}
