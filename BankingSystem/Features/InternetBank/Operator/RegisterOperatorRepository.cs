using Azure.Core;
using BankingSystem.DB;
using BankingSystem.DB.Entities;
using BankingSystem.Features.InternetBank.Operator.AuthOperator;
using BankingSystem.Features.InternetBank.Operator;
using Microsoft.EntityFrameworkCore;

namespace BankingSystem.Features.InternetBank.Operator
{
    public interface IOperatorRepository
    {     
        public Task AddOperatorAsync(OperatorEntity entity);
        public Task<OperatorEntity> GetOperatorByNameAsync(OperatorLoginRequest request);
        public Task<OperatorEntity> GetOperatorByPasswordAsync(OperatorLoginRequest request);
        Task SaveChangesAsync();
        bool OperatorExists(string personalNumber);
    }
    public class RegisterOperatorRepository : IOperatorRepository
    {
        private readonly AppDbContext _db;
        public RegisterOperatorRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task AddOperatorAsync(OperatorEntity entity)
        {
            await _db.Operators.AddAsync(entity);
            await _db.SaveChangesAsync();
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
