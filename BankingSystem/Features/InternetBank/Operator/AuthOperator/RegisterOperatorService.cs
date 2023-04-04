using BankingSystem.DB.Entities;

namespace BankingSystem.Features.InternetBank.Operator.AuthOperator
{
    //public class RegisterOperatorService
    //{
    //    private readonly RegisterOperatorRepository _repository; 
    //    public RegisterOperatorService(RegisterOperatorRepository repository)
    //    {
    //        _repository = repository;
    //    }

    //    public async Task<RegisterOperatorResponse> RegisterOperatorAsync(RegisterOperatorRequest request)
    //    {
    //        var response = new RegisterOperatorResponse();
    //        try
    //        {
    //            var operatorByPersonalNumber = await _repository.OperatorExists(request.PersonalNumber);
    //            if(operatorByPersonalNumber == true)
    //            {
    //                response.IsSuccessful = false;
    //                response.ErrorMessage = "Operator with this personal number already exists";
    //            }
    //            else
    //            {
    //                var newOperator = new OperatorEntity();
    //                newOperator.FirstName = request.FirstName;
    //                newOperator.LastName = request.LastName;
    //                newOperator.Password = request.Password;
    //                newOperator.PersonalNumber = request.PersonalNumber;

    //                await _repository.AddOperatorAsync(newOperator);
    //                await _repository.SaveChangesAsync();

    //                response.IsSuccessful = true;
    //                response.FirstName = request.FirstName;
    //                response.LastName = request.LastName;
    //            }
    //            return response;
    //        }
    //        catch (Exception ex)
    //        {
    //            response.IsSuccessful = false;
    //            response.ErrorMessage = ex.Message;
    //        }
    //        return response;
    //    }
    //}
}
