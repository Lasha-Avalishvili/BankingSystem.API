using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankingSystem.DB.Entities;
using Microsoft.AspNetCore.Identity;

namespace BankingSystem.Features.InternetBank.Auth
{
    public class AuthService
    {
        private readonly TokenGenerator _tokenGenerator;
        private readonly UserManager<UserEntity> _userManager;
        public AuthService(TokenGenerator tokenGenerator, UserManager<UserEntity> userManager)
        {
            _tokenGenerator = tokenGenerator;
            _userManager = userManager;
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            var response = new LoginResponse();

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                response.IsSuccessful = false;
                response.ErrorMessage = "User Not Found";
                response.JWT = null;
                return response;
            }

            var userpass = await _userManager.CheckPasswordAsync(user, request.Password);
            var roles = await _userManager.GetRolesAsync(user);

            if (userpass == false)
            {
                response.IsSuccessful = false;
                response.ErrorMessage = "Invalid ID or password";
                response.JWT = null;
            }
            else
            {
                response.IsSuccessful = true;
                response.ErrorMessage = null;
                response.JWT = _tokenGenerator.Generate(roles, user.Id.ToString());
            }
            return response;
        }
    }
}
