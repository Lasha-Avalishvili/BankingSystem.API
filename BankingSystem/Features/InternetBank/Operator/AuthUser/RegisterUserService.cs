using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankingSystem.DB.Entities;
using BankingSystem.Features.InternetBank.Operator.AddUser;
using BankingSystem.Features.InternetBank.Operator.AuthOperator;
using BankingSystem.Features.InternetBank.Operator.RegisterUser;

namespace BankingSystem.Features.InternetBank.Operator.AuthUser
{
    public class RegisterUserService
    {
        private readonly RegisterUserRepository _repository;  
        public RegisterUserService(RegisterUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<RegisterUserResponse> RegisterUserAsync(RegisterUserRequest request)
        {
            var response = new RegisterUserResponse();

            try
            {
                var newUser = new UserEntity();
                newUser.FirstName = request.FirstName;
                newUser.LastName = request.LastName;
                newUser.Password = request.Password;
                newUser.PersonalNumber = request.PersonalNumber;
                newUser.Email = request.Email;
                newUser.DateOfBirth= request.DateOfBirth;
                newUser.RegisteredAt = DateTime.UtcNow;

                // Additional business logic related to registration can go here
                // For example, validating the request data, sending a confirmation email to the user, etc.

                await _repository.AddUserAsync(newUser);

                response.IsSuccessful = true;
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
