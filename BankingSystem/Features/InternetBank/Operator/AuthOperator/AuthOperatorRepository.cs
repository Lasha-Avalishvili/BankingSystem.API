using Azure.Core;
using BankingSystem.DB;
using BankingSystem.DB.Entities;
using Microsoft.EntityFrameworkCore;

namespace BankingSystem.Features.InternetBank.Operator.AuthOperator
{
    public interface IOperatorRepository
    {
        public Task<OperatorEntity> RegisterOperatorAsync(OperatorRegisterRequest request);
        public Task<OperatorEntity> GetOperatorByNameAsync(OperatorLoginRequest request);
        public Task<OperatorEntity> GetOperatorByPasswordAsync(OperatorLoginRequest request);
        Task SaveChangesAsync();
        bool OperatorExists(string personalNumber);
    }
    public class AuthOperatorRepository : IOperatorRepository
    {
        private readonly AppDbContext _db;
        public AuthOperatorRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<OperatorEntity> RegisterOperatorAsync(OperatorRegisterRequest request)
        {
            var newOperator = new OperatorEntity();
            newOperator.FirstName = request.FirstName;
            newOperator.LastName = request.LastName;
            newOperator.Password = request.Password;
            newOperator.PersonalNumber = request.PersonalNumber;
            await _db.Operators.AddAsync(newOperator);

            return newOperator;
        }

        public bool OperatorExists(string personalNumber)
        {
            return _db.Operators.Any(o => o.PersonalNumber == personalNumber);
        }

        public async Task<OperatorEntity> GetOperatorByNameAsync(OperatorLoginRequest request)
        {
            var response = await _db.Operators.Where(o => o.FirstName == request.FirstName).FirstOrDefaultAsync();
            return response;
        }

        public async Task<OperatorEntity> GetOperatorByPasswordAsync(OperatorLoginRequest request)
        {
            var response = await _db.Operators.Where(o => o.Password == request.Password).FirstOrDefaultAsync();
            return response;
        }
        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
