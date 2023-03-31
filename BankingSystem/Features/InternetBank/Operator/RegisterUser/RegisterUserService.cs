using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankingSystem.DB.Entities;
using BankingSystem.Features.InternetBank.Operator.AddUser;
using BankingSystem.Features.InternetBank.Operator.AuthOperator;
using BankingSystem.Features.InternetBank.Operator.RegisterUser;
using Microsoft.Identity.Client;

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
                var userByPersonalNumber = await _repository.UserExists(request.PersonalNumber);

                if(userByPersonalNumber == true)
                {
                    response.IsSuccessful = false;
                    response.ErrorMessage = "User with this personal number already exists";
                }
                else
                {
                    var newUser = new UserEntity();
                    newUser.FirstName = request.FirstName;
                    newUser.LastName = request.LastName;
                 //   newUser.Password = request.Password;
                    newUser.PersonalNumber = request.PersonalNumber;
                    newUser.UserName = request.Email;
                    newUser.Email = request.Email;
                    newUser.DateOfBirth = request.DateOfBirth;
                    newUser.RegisteredAt = DateTime.UtcNow;

                    var result = await _repository.AddUserAsync(newUser, request.Password);

                    response.IsSuccessful = result.Succeeded;
                    response.ErrorMessage = result.Errors?.FirstOrDefault()?.Description ?? "";
                    response.FirstName = request.FirstName;
                    response.LastName = request.LastName;
                    response.UserId = newUser.Id;
                }
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
