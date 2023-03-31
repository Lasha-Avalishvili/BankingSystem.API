using Azure;
using BankingSystem.DB;
using BankingSystem.Features.InternetBank.Auth;
using Microsoft.EntityFrameworkCore;


namespace BankingSystem.Features.InternetBank.User.LoginUser
{
    public interface ILoginUserRepository
    {
        Task<LoginUserResponse> LoginUserAsync(LoginUserRequest request);
    }

    public class LoginUserRepository : ILoginUserRepository
    {
        private readonly AppDbContext _db;
        private readonly TokenGenerator _tokenGenerator;
        public LoginUserRepository(AppDbContext db, TokenGenerator tokenGenerator)
        {
            _db = db;
            _tokenGenerator = tokenGenerator;
        }

        public async Task<LoginUserResponse> LoginUserAsync(LoginUserRequest request)
        {
            var user = await _db.Users.Where(u => u.PersonalNumber == request.PersonalNumber).FirstOrDefaultAsync();
        //    var userpass = await _db.Users.Where(u => u.Password == request.Password).FirstOrDefaultAsync();

            var response = new LoginUserResponse(); 
            
            if (user == null)
            {
                response.IsSuccessful = false;
                response.ErrorMessage = "User Not Found";
                response.JWT = null;
            }
            //else if(userpass == null)
            //{
            //   response.IsSuccessful = false;
            //   response.ErrorMessage = "Invalid ID or password";
            //   response.JWT = null;
            //}
            else
            {
                response.IsSuccessful = true;
                response.ErrorMessage = null;
                response.JWT = _tokenGenerator.Generate("api-user", user.Id.ToString());
            }
            return response;
        }
    }
}
