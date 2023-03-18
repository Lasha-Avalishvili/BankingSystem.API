using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankingSystem.DB.Entities;

namespace BankingSystem.Features.InternetBank.Operator.AuthOperator
{
    public class RegisterOperatorService
    {
        private readonly RegisterOperatorRepository _repository; // change name

        public RegisterOperatorService(RegisterOperatorRepository repository)
        {
            _repository = repository;
        }

        public async Task<RegisterOperatorResponse> RegisterOperatorAsync(RegisterOperatorRequest request)
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
                response.FirstName= request.FirstName;
                response.LastName= request.LastName;
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
