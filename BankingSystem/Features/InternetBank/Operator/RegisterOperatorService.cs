using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankingSystem.DB.Entities;
using BankingSystem.Features.InternetBank.Operator.AuthOperator;
using BankingSystem.Features.InternetBank.Operator;

namespace BankingSystem.Features.InternetBank.Operator
{
    public class RegisterOperatorService
    {
        private readonly RegisterOperatorRepository _repository; // change name

        public RegisterOperatorService(RegisterOperatorRepository repository)
        {
            _repository = repository;
        }

        public async Task<RegisterOperatorResponse> RegisterOperatorAsync(OperatorRegisterRequest request)
        {
            var response = new RegisterOperatorResponse();

            try
            {
                var newOperator = new OperatorEntity();
                newOperator.FirstName = request.FirstName;
                newOperator.LastName = request.LastName;
                newOperator.Password = request.Password;
                newOperator.PersonalNumber = request.PersonalNumber;

                // Additional business logic related to registration can go here
                // For example, validating the request data, sending a confirmation email to the user, etc.

                await _repository.AddOperatorAsync(newOperator);

                response.IsSuccessful = true;
                response.Operator = newOperator;
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
