using BankingSystem.DB.Entities;
using BankingSystem.Features.InternetBank.Operator.AddUser;
using BankingSystem.Features.InternetBank.Operator.RegisterUser;
using Microsoft.AspNetCore.Identity;

namespace BankingSystem.Features.InternetBank.Operator.AuthUser
{
    public class RegisterUserService
    {
        private readonly RegisterUserRepository _repository;
        private readonly UserManager<UserEntity> _userManager;
        private readonly RoleManager<RoleEntity> _roleManager;
        public RegisterUserService(RegisterUserRepository repository, UserManager<UserEntity> userManager, RoleManager<RoleEntity> roleManager)
        {
            _repository = repository;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<RegisterUserResponse> RegisterUserAsync(RegisterUserRequest request)
        {
            var response = new RegisterUserResponse();
            try
            {
                var limitedAge = DateTime.UtcNow.AddYears(-18);
                if (request.DateOfBirth > limitedAge)
                {
                    throw new Exception("User must be at least 18 years old");
                }
                var userByEmail = await _userManager.FindByEmailAsync(request.Email);

                if (userByEmail != null)
                {
                    throw new Exception("User with this email already exists");
                }
                var userExists = await _repository.UserExists(request.PersonalNumber);
                var userByPersonalNumber = await _repository.FindUser(request.PersonalNumber);
                IList<string> userRole = null;
                if (userByPersonalNumber != null)
                {
                     userRole = await _repository.GetRoleAsync(userByPersonalNumber);
                }
                if (userExists == true && userRole[0] != "api-admin")
                {
                    throw new Exception("User with this personal number already exists");
                }
                var newUser = new UserEntity();
                newUser.FirstName = request.FirstName;
                newUser.LastName = request.LastName;
                newUser.PersonalNumber = request.PersonalNumber;
                newUser.UserName = request.Email;
                newUser.Email = request.Email;
                newUser.DateOfBirth = request.DateOfBirth;
                newUser.RegisteredAt = DateTime.UtcNow;
                newUser.EmailConfirmed = true;

                var result = await _userManager.CreateAsync(newUser, request.Password);

                var addToRoleResult = await _userManager.AddToRoleAsync(newUser, "api-user");

                response.IsSuccessful = result.Succeeded;
                response.ErrorMessage = result.Errors?.FirstOrDefault()?.Description ?? "";
                response.FirstName = request.FirstName;
                response.LastName = request.LastName;
                response.UserId = newUser.Id;
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.ErrorMessage = ex.Message;
            }

            return response;
        }
    }
}
